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
            switches
            |> Seq.append switchesPfx
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

    // creates a longer sorter with the switches added to the beginning.
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

    //cross two sorters, and pad with random switches, if necessary
    let crossOver (pfxLength:switchCount)
                  (finalLength:switchCount)
                  (sortrPfx:sorter) 
                  (sortrSfx:sorter)
                  (finalId: sorterId)
                  (rnd: IRando) =

            let switchesPfx = (Switch.rndNonDegenSwitchesOfOrder (sortrPfx |> getOrder) rnd) 
                              |> Seq.append sortrPfx.switches
                              |> Seq.take (pfxLength |> SwitchCount.value)

            let switchesSfx = (Switch.rndNonDegenSwitchesOfOrder (sortrSfx |> getOrder) rnd) 
                              |> Seq.append sortrSfx.switches
                              |> Seq.skip (pfxLength |> SwitchCount.value)

            let finalSwitches = switchesSfx 
                                |> Seq.append switchesPfx
                                |> Seq.take (finalLength |> SwitchCount.value)
            
            fromSwitches finalId (sortrPfx |> getOrder) finalSwitches


    let randomSwitches
            (order: order)
            (wPfx: switch seq)
            (switchCount: switchCount) 
            (rnGen: unit -> rngGen)
            (sorterD:sorterId)
            =
        let randy = (rnGen()) |> Rando.fromRngGen
        let switches = Switch.rndNonDegenSwitchesOfOrder order randy
        fromSwitchesWithPrefix sorterD order switchCount wPfx switches


    let randomStages
        (order: order)
        (switchFreq: switchFrequency)
        (wPfx: switch seq)
        (switchCount: switchCount) 
        (rnGen: unit -> rngGen)
        (sorterD:sorterId)
        =
        let randy = (rnGen()) |> Rando.fromRngGen
        let _switches =
            (Stage.rndSeq order switchFreq randy)
            |> Seq.map (fun st -> st.switches)
            |> Seq.concat
        fromSwitchesWithPrefix sorterD order switchCount wPfx _switches


    let randomStages2
        (order: order)
        (wPfx: switch seq)
        (switchCount: switchCount)
        (rnGen: unit -> rngGen)
        (sorterD:sorterId)
        =
        let randy = (rnGen()) |> Rando.fromRngGen
        let coreTc = TwoCycle.evenMode order
        let _switches =
            (Stage.rndSeq2 coreTc randy)
            |> Seq.map (fun st -> st.switches)
            |> Seq.concat
        fromSwitchesWithPrefix sorterD order switchCount wPfx _switches


    let randomStagesCoConj
        (order: order)
        (wPfx: switch seq)
        (switchCount: switchCount)
        (rnGen: unit -> rngGen) 
        (sorterD:sorterId)
        =
        let randy = (rnGen()) |> Rando.fromRngGen
        let _switches =
            (Stage.rndSeqCoConj order randy)
            |> Seq.concat
            |> Seq.map (fun st -> st.switches)
            |> Seq.concat

        fromSwitchesWithPrefix sorterD order switchCount wPfx _switches


    let randomStagesSeparated
        (minSeparation: int)
        (maxSeparation: int)
        (order: order)
        (wPfx: switch seq)
        (switchCount: switchCount)
        (rnGen: unit -> rngGen)
        (sorterD:sorterId)
        =
        let randy = (rnGen()) |> Rando.fromRngGen
        let _switches =
            (Stage.rndSeqSeparated order minSeparation maxSeparation randy)
            |> Seq.concat
            |> Seq.map (fun st -> st.switches)
            |> Seq.concat

        fromSwitchesWithPrefix sorterD order switchCount wPfx _switches


    let randomPermutaionChoice
        (coreTc:twoCycle)
        (perms: permutation[])
        (order: order)
        (wPfx: switch seq)
        (switchCount: switchCount)
        (rnGen: unit -> rngGen)
        (sorterD:sorterId)
        =
        let randy = (rnGen()) |> Rando.fromRngGen
        let _switches =
            (Stage.rndPermDraw coreTc perms randy)
            |> Seq.map (fun st -> st.switches)
            |> Seq.concat

        fromSwitchesWithPrefix sorterD order switchCount wPfx _switches


    let randomSymmetric
            (order: order)
            (wPfx: switch seq)
            (switchCount: switchCount)
            (rnGen: unit -> rngGen)
            (sorterD:sorterId)
            =
        let randy = (rnGen()) |> Rando.fromRngGen
        let switches =
            (Stage.rndSymmetric order randy)
            |> Seq.map (fun st -> st.switches)
            |> Seq.concat

        fromSwitchesWithPrefix sorterD order switchCount wPfx switches


    let randomBuddies
        (stageWindowSz: stageWindowSize)
        (order: order)
        (wPfx: switch seq)
        (switchCount: switchCount)
        (rnGen: unit -> rngGen)
        (sorterD:sorterId)
        =
        let randy = (rnGen()) |> Rando.fromRngGen
        let switches =
            (Stage.rndBuddyStages stageWindowSz SwitchFrequency.max order randy List.empty)
            |> Seq.collect (fun st -> st.switches |> List.toSeq)

        fromSwitchesWithPrefix sorterD order switchCount wPfx switches



type sorterUniformMutatorType =
    | Switch
    | Stage
    | StageRfl

type sorterUniformMutator =
    private
        { 
          sumType: sorterUniformMutatorType
          mutationRate: mutationRate
          switchCountPfx: switchCount Option
          switchCountFinal: switchCount Option
          mFunc: sorter -> sorterId -> IRando -> Result<sorter, string> 
        }

module SorterUniformMutator =

    let private _create sorterUniformMutatorTyp mutationRat
                        switchCtPfx switchCtFinal mutationFun =
        { 
          sorterUniformMutator.sumType = sorterUniformMutatorTyp
          mutationRate = mutationRat
          mFunc = mutationFun
          switchCountPfx = switchCtPfx
          switchCountFinal = switchCtFinal
        }

    let getSorterUniformMutatorType (sum: sorterUniformMutator) = sum.sumType

    let getMutationRate (sum: sorterUniformMutator) = sum.mutationRate

    let getSorterMutator (sum: sorterUniformMutator) = sum.mFunc
    
    let getPrefixSwitchCount (sum: sorterUniformMutator) = sum.switchCountPfx
    
    let getFinalSwitchCount (sum: sorterUniformMutator) = sum.switchCountFinal

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


    let _makeMutant
            (mFunc: sorter -> sorterId -> IRando -> Result<sorter, string>)
            (switchCtPrefix: switchCount Option)
            (switchCtTarget: switchCount Option)
            (sorter: sorter)          
            (sorterD:sorterId)  
            (randy: IRando)  =
        result {
          let! mutantSortr = mFunc sorter sorterD randy
          let targetSwitchCt = 
            match switchCtTarget with
            | Some swct -> swct
            | None -> sorter |> Sorter.getSwitches 
                      |> Array.length |> SwitchCount.create

          let prefixSwitchCt = 
            match switchCtPrefix with
            | Some swct -> swct
            | None -> 0 |> SwitchCount.create

          return Sorter.crossOver prefixSwitchCt targetSwitchCt sorter mutantSortr sorterD randy
        }


    let create (switchCtPrefix: switchCount Option)
               (switchCtTarget: switchCount Option)
               (sorterUniformMutatorTyp:sorterUniformMutatorType)  
               (mutRate: mutationRate) =
        match sorterUniformMutatorTyp with
        | Switch ->
            _create sorterUniformMutatorTyp mutRate switchCtPrefix switchCtTarget (_makeMutant (_switchMutator mutRate) switchCtPrefix switchCtTarget)
        | Stage ->
            _create sorterUniformMutatorTyp mutRate switchCtPrefix switchCtTarget (_makeMutant (_stageMutator mutRate) switchCtPrefix switchCtTarget)
        | StageRfl ->
            _create sorterUniformMutatorTyp mutRate switchCtPrefix switchCtTarget (_makeMutant (_stageRflMutator mutRate) switchCtPrefix switchCtTarget)


type sorterMutator = Uniform of sorterUniformMutator
