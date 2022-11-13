namespace global

open System

type sorter =
    private
        { sortrId: sorterId
          order: order
          switches: array<switch> }

module Sorter =
    let getSorterId (sortr: sorter) = Guid.NewGuid() |> SorterId.create

    let getOrder (sortr: sorter) = sortr.order

    let getSwitches (sortr: sorter) = sortr.switches

    let getSwitchCount (sortr: sorter) =
        sortr.switches.Length |> SwitchCount.create


    let fromSwitches (order: order) (switches: seq<switch>) =
        { sorter.sortrId = SorterId.create (Guid.NewGuid())
          sorter.order = order
          sorter.switches = switches |> Seq.toArray }

    let fromSwitchesWithPrefix
        (order: order)
        (switchCtTarget: switchCount)
        (switchesPfx: seq<switch>)
        (switches: seq<switch>)
        =
        let combinedSwitches =
            switchesPfx
            |> Seq.append switches
            |> Seq.take (switchCtTarget |> SwitchCount.value)

        fromSwitches order combinedSwitches


    let fromStagesWithPrefix
        (order: order)
        (switchCtTarget: switchCount)
        (switchesPfx: seq<switch>)
        (stages: seq<stage>)
        =
        let switches = stages |> Seq.map (fun st -> st.switches) |> Seq.concat
        fromSwitchesWithPrefix order switchCtTarget switchesPfx switches


    // creates a longer sorter with the switches added to the end.
    let appendSwitches (switchesToAppend: seq<switch>) (sorter: sorter) =
        let newSwitches = switchesToAppend |> Seq.append sorter.switches |> Seq.toArray
        fromSwitches (sorter |> getOrder) newSwitches



    let prependSwitches (newSwitches: seq<switch>) (sorter: sorter) =
        let newSwitches = sorter.switches |> Seq.append newSwitches |> Seq.toArray
        fromSwitches (sorter |> getOrder) newSwitches


    let removeSwitchesFromTheStart (newLength: switchCount) (sortr: sorter) =
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

            fromSwitches (sortr |> getOrder) trimmedSwitches |> Ok


    let removeSwitchesFromTheEnd (newLength: switchCount) (sortr: sorter) =
        let curSwitchCt = sortr |> getSwitchCount |> SwitchCount.value
        let numSwitchesToRemove = curSwitchCt - (SwitchCount.value newLength)

        if numSwitchesToRemove < 0 then
            "New length is longer than sorter" |> Error
        else
            let trimmedSwitches = (sortr |> getSwitches) |> Seq.take numSwitchesToRemove
            fromSwitches (sortr |> getOrder) trimmedSwitches |> Ok


    let getSwitchesFromFirstStages (stageCount: stageCount) (sorter: sorter) =
        sorter.switches
        |> Stage.fromSwitches sorter.order
        |> Seq.take (StageCount.value stageCount)
        |> Seq.map (fun t -> t.switches)
        |> Seq.concat


    let fromTwoCycles (order: order) (switchCtTarget: switchCount) (wPfx: switch seq) (twoCycleSeq: twoCycle seq) =
        let switches =
            twoCycleSeq |> Seq.map (fun tc -> Switch.fromTwoCycle tc) |> Seq.concat

        fromSwitchesWithPrefix order switchCtTarget wPfx switches


    let makeAltEvenOdd (order: order) (wPfx: switch seq) (switchCount: switchCount) =
        let switches =
            TwoCycle.makeAltEvenOdd order (Permutation.identity order)
            |> Seq.map (fun tc -> Switch.fromTwoCycle tc)
            |> Seq.concat

        fromSwitchesWithPrefix order switchCount wPfx switches


    //***********  IRando dependent  *********************************

    let randomSwitches (order: order) (wPfx: switch seq) (switchCount: switchCount) (rnd: IRando) =
        let switches = Switch.rndNonDegenSwitchesOfDegree order rnd
        fromSwitchesWithPrefix order switchCount wPfx switches


    let randomStages
        (switchFreq: switchFrequency)
        (order: order)
        (wPfx: switch seq)
        (switchCount: switchCount)
        (rando: IRando)  =
        let _switches =
            (Stage.rndSeq order switchFreq rando)
            |> Seq.map (fun st -> st.switches)
            |> Seq.concat

        fromSwitchesWithPrefix order switchCount wPfx _switches


    let randomSymmetric (order: order) (wPfx: switch seq) (switchCount: switchCount) (rando: IRando) =
        let switches =
            (Stage.rndSymmetric order rando)
            |> Seq.map (fun st -> st.switches)
            |> Seq.concat

        fromSwitchesWithPrefix order switchCount wPfx switches


    let randomBuddies
        (stageWindowSz: stageWindowSize)
        (order: order)
        (wPfx: switch seq)
        (switchCount: switchCount)
        (rando: IRando)
        =
        let switches =
            (Stage.rndBuddyStages stageWindowSz SwitchFrequency.max order rando List.empty)
            |> Seq.collect (fun st -> st.switches |> List.toSeq)

        fromSwitchesWithPrefix order switchCount wPfx switches




type sorterUniformMutatorType =
    | Switch
    | Stage
    | StageRfl

type sorterUniformMutator =
    private
        { sumType: sorterUniformMutatorType
          mutationRate: mutationRate
          mFunc: sorter -> IRando -> Result<sorter, string> }

module SorterUniformMutator =

    let create sorterUniformMutatorTyp mutationRat mutationFun =
        { sorterUniformMutator.sumType = sorterUniformMutatorTyp
          mutationRate = mutationRat
          mFunc = mutationFun }

    let getSorterUniformMutatorType (sum: sorterUniformMutator) = sum.sumType

    let getMutationRateVal (sum: sorterUniformMutator) =
        sum.mutationRate |> MutationRate.getRateValue

    let mutateBySwitch (switchMutationRat: switchMutationRate) =

        (fun (sorter: sorter) (randy: IRando) ->
            result {
                let newSwitches =
                    Switch.mutateSwitches sorter.order switchMutationRat randy sorter.switches
                    |> Seq.toArray

                return Sorter.fromSwitches (sorter |> Sorter.getOrder) newSwitches

            })
        |> create sorterUniformMutatorType.Switch (switchMutationRat |> mutationRate.Switch)


    let mutateByStage (stMr: stageMutationRate) =

        (fun (sorter: sorter) (randy: IRando) ->
            result {
                let mutantStages =
                    sorter.switches
                    |> Stage.fromSwitches sorter.order
                    |> Seq.toArray
                    |> Array.map (Stage.randomMutate randy stMr)

                let newSwitches =
                    [| for stage in mutantStages do
                           yield! stage.switches |]
                    |> Seq.toArray

                return Sorter.fromSwitches (sorter |> Sorter.getOrder) newSwitches
            })
        |> create sorterUniformMutatorType.Stage (stMr |> mutationRate.Stage)


    let mutateByStageRfl (stMr: stageMutationRate) =

        (fun (sorter: sorter) (randy: IRando) ->
            result {
                let mutantStages =
                    sorter.switches
                    |> Stage.fromSwitches sorter.order
                    |> Seq.toArray
                    |> Array.map (Stage.randomReflMutate randy stMr)

                let newSwitches =
                    [| for stage in mutantStages do
                           yield! stage.switches |]

                return Sorter.fromSwitches (sorter |> Sorter.getOrder) newSwitches
            })
        |> create sorterUniformMutatorType.StageRfl (stMr |> mutationRate.Stage)


type sorterMutator = Uniform of sorterUniformMutator
