namespace global

open System

type sorter =
    private
        { sortrId: sorterId
          order: order
          switches: array<switch> }

module Sorter =
    let getSorterId (sortr: sorter) = sortr.sortrId

    let getOrder (sortr: sorter) = sortr.order

    let getSwitches (sortr: sorter) = sortr.switches

    let getSwitchCount (sortr: sorter) =
        sortr.switches.Length |> SwitchCount.create

    let toByteArray (sortr: sorter) =
        sortr |> getSwitches 
              |> Switch.toBitPack (sortr |> getOrder) 
              |> BitPack.getData
              |> Seq.toArray


    let fromSwitches 
            (sorterD:sorterId)
            (order: order) 
            (switches: seq<switch>) =
        { sorter.sortrId = sorterD
          sorter.order = order
          sorter.switches = switches |> Seq.toArray }


    let fromSwitchesWithPrefix
            (sorterD:sorterId)
            (order: order)
            (switchCtTarget: switchCount)
            (switchesPfx: seq<switch>)
            (switches: seq<switch>) =
        let combinedSwitches =
            switchesPfx
            |> Seq.append switches
            |> Seq.take (switchCtTarget |> SwitchCount.value)

        fromSwitches sorterD order combinedSwitches


    let fromStagesWithPrefix
            (sorterD:sorterId)
            (order: order)
            (switchCtTarget: switchCount)
            (switchesPfx: seq<switch>)
            (stages: seq<stage>) =
        let switches = stages |> Seq.map (fun st -> st.switches) |> Seq.concat
        fromSwitchesWithPrefix sorterD order switchCtTarget switchesPfx switches


    // creates a longer sorter with the switches added to the end.
    let appendSwitches
            (sorterD:sorterId)
            (switchesToAppend: seq<switch>) 
            (sorter: sorter) =
        let newSwitches = switchesToAppend |> Seq.append sorter.switches |> Seq.toArray
        fromSwitches sorterD (sorter |> getOrder) newSwitches


    let prependSwitches 
            (sorterD:sorterId)
            (newSwitches: seq<switch>) 
            (sorter: sorter) =
        let newSwitches = sorter.switches |> Seq.append newSwitches |> Seq.toArray
        fromSwitches sorterD (sorter |> getOrder) newSwitches


    let removeSwitchesFromTheStart
            (sorterD:sorterId)
            (newLength: switchCount) 
            (sortr: sorter) =
        let curSwitchCt = sortr |> getSwitchCount |> SwitchCount.value
        let numSwitchesToRemove = curSwitchCt - (SwitchCount.value newLength)

        if numSwitchesToRemove < 0 then
            "New length is longer than sorter" |> Error
        else
            let trimmedSwitches =
                sortr
                |> getSwitches
                |> Seq.skip (numSwitchesToRemove)
                |> Seq.take (newLength |> SwitchCount.value)

            fromSwitches sorterD (sortr |> getOrder) trimmedSwitches |> Ok


    let removeSwitchesFromTheEnd
            (sorterD:sorterId)
            (newLength: switchCount) 
            (sortr: sorter) =
        let curSwitchCt = sortr |> getSwitchCount |> SwitchCount.value
        let numSwitchesToRemove = curSwitchCt - (SwitchCount.value newLength)

        if numSwitchesToRemove < 0 then
            "New length is longer than sorter" |> Error
        else
            let trimmedSwitches = (sortr |> getSwitches) |> Seq.take numSwitchesToRemove
            fromSwitches sorterD (sortr |> getOrder) trimmedSwitches |> Ok


    let getSwitchesFromFirstStages (stageCount: stageCount) (sorter: sorter) =
        sorter.switches
        |> Stage.fromSwitches sorter.order
        |> Seq.take (StageCount.value stageCount)
        |> Seq.map (fun t -> t.switches)
        |> Seq.concat


    let fromTwoCycles
            (sorterD:sorterId)
            (order: order) 
            (switchCtTarget: switchCount) 
            (wPfx: switch seq) 
            (twoCycleSeq: twoCycle seq) =
        let switches =
            twoCycleSeq |> Seq.map (fun tc -> Switch.fromTwoCycle tc) |> Seq.concat

        fromSwitchesWithPrefix sorterD order switchCtTarget wPfx switches


    let makeAltEvenOdd
            (sorterD:sorterId)
            (order: order) 
            (wPfx: switch seq) 
            (switchCount: switchCount) =
        let switches =
            TwoCycle.makeAltEvenOdd order (Permutation.identity order)
            |> Seq.map (fun tc -> Switch.fromTwoCycle tc)
            |> Seq.concat

        fromSwitchesWithPrefix sorterD order switchCount wPfx switches


    //***********  IRando dependent  *********************************

    let randomSwitches
            (sorterD:sorterId)
            (order: order) 
            (wPfx: switch seq) 
            (switchCount: switchCount) 
            (rnd: IRando) =
        let switches = Switch.rndNonDegenSwitchesOfOrder order rnd
        fromSwitchesWithPrefix sorterD order switchCount wPfx switches


    let randomStages
        (switchFreq: switchFrequency)
        (sorterD:sorterId)
        (order: order)
        (wPfx: switch seq)
        (switchCount: switchCount)
        (rando: IRando)  =
        let _switches =
            (Stage.rndSeq order switchFreq rando)
            |> Seq.map (fun st -> st.switches)
            |> Seq.concat

        fromSwitchesWithPrefix sorterD order switchCount wPfx _switches


    let randomSymmetric
            (sorterD:sorterId)
            (order: order) 
            (wPfx: switch seq) 
            (switchCount: switchCount) 
            (rando: IRando) =
        let switches =
            (Stage.rndSymmetric order rando)
            |> Seq.map (fun st -> st.switches)
            |> Seq.concat

        fromSwitchesWithPrefix sorterD order switchCount wPfx switches


    let randomBuddies
        (stageWindowSz: stageWindowSize)
        (sorterD:sorterId)
        (order: order)
        (wPfx: switch seq)
        (switchCount: switchCount)
        (rando: IRando)
        =
        let switches =
            (Stage.rndBuddyStages stageWindowSz SwitchFrequency.max order rando List.empty)
            |> Seq.collect (fun st -> st.switches |> List.toSeq)

        fromSwitchesWithPrefix sorterD order switchCount wPfx switches




type sorterUniformMutatorType =
    | Switch
    | Stage
    | StageRfl

type sorterUniformMutator =
    private
        { sumType: sorterUniformMutatorType
          mutationRate: mutationRate
          mFunc: sorter -> sorterId -> IRando -> Result<sorter, string> }

module SorterUniformMutator =

    let create sorterUniformMutatorTyp mutationRat mutationFun =
        { sorterUniformMutator.sumType = sorterUniformMutatorTyp
          mutationRate = mutationRat
          mFunc = mutationFun }

    let getSorterUniformMutatorType (sum: sorterUniformMutator) = sum.sumType

    let getMutationRate (sum: sorterUniformMutator) = sum.mutationRate

    let getSorterMutator (sum: sorterUniformMutator) = sum.mFunc

    let _switchMutator
            (mutRate:mutationRate) 
            (sorter: sorter)
            (sorterD:sorterId)
            (randy: IRando)  =
          result {
            let newSwitches =
                Switch.mutateSwitches sorter.order mutRate randy sorter.switches
                |> Seq.toArray

            return Sorter.fromSwitches sorterD (sorter |> Sorter.getOrder) newSwitches
          }

    let mutateBySwitch
            (mutRate: mutationRate) =
        create sorterUniformMutatorType.Switch mutRate (_switchMutator mutRate)


    let _stageMutator
            (mutRate:mutationRate) 
            (sorter: sorter)             
            (sorterD:sorterId)  
            (randy: IRando) =
          result {
            let mutantStages =
                sorter.switches
                |> Stage.fromSwitches sorter.order
                |> Seq.toArray
                |> Array.map (Stage.randomMutate randy mutRate)

            let newSwitches =
                [| for stage in mutantStages do
                        yield! stage.switches |]
                |> Seq.toArray

            return Sorter.fromSwitches sorterD (sorter |> Sorter.getOrder) newSwitches
          }

    let mutateByStage
            (mutRate: mutationRate) =
        create sorterUniformMutatorType.Stage mutRate (_stageMutator mutRate )

    let _stageRflMutator
            (mutRate:mutationRate) 
            (sorter: sorter)          
            (sorterD:sorterId)  
            (randy: IRando)  =
          result {
            let mutantStages =
                sorter.switches
                |> Stage.fromSwitches sorter.order
                |> Seq.toArray
                |> Array.map (Stage.randomReflMutate randy mutRate)

            let newSwitches =
                [| for stage in mutantStages do
                        yield! stage.switches |]
                |> Seq.toArray

            return Sorter.fromSwitches sorterD (sorter |> Sorter.getOrder) newSwitches
          }

    let mutateByStageRfl
            (mutRate: mutationRate) =
        create sorterUniformMutatorType.StageRfl mutRate (_stageRflMutator mutRate)


type sorterMutator = Uniform of sorterUniformMutator
