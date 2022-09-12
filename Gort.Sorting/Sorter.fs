namespace global
open System

type sorter = 
    { 
        order:order; 
        switches:array<switch>; 
        switchCount:switchCount
    }

module Sorter =

    let makeId (s:sorter) = 
        SorterId.create (Guid.NewGuid())


    let fromSwitches (order:order) 
                     (switchCtTarget:switchCount)
                     (switches:seq<switch>) =
        let switchArray = switches |> Seq.take (switchCtTarget |> SwitchCount.value)
                                   |> Seq.toArray
        let switchCount = SwitchCount.create switchArray.Length
        {
            sorter.order=order;
            switchCount=switchCount;
            switches = switchArray
        }

    let fromSwitchesWithPrefix (order:order)
                               (switchCtTarget:switchCount)
                               (switchesPfx:seq<switch>)
                               (switches:seq<switch>) =
        let combinedSwitches = switchesPfx |> Seq.append switches
        fromSwitches order switchCtTarget combinedSwitches


    let fromStagesWithPrefix (order:order)
                             (switchCtTarget:switchCount)
                             (switchesPfx:seq<switch>)
                             (stages:seq<stage>) =
        let switches = stages |> Seq.map(fun st->st.switches)
                              |> Seq.concat
        fromSwitchesWithPrefix order switchCtTarget switchesPfx switches
   

    // creates a longer sorter with the switches added to the end.
    let appendSwitches (switchesToAppend:seq<switch>) 
                       (sorter:sorter) =
        let newSwitches = switchesToAppend 
                          |> Seq.append sorter.switches
                          |> Seq.toArray
        let newSwitchCount = SwitchCount.create newSwitches.Length
        {
            sorter.order = sorter.order;
            switchCount=newSwitchCount;
            switches = newSwitches
        }


    let prependSwitches (newSwitches:seq<switch>) 
                        (sorter:sorter) =
        let newSwitches = sorter.switches 
                          |> Seq.append newSwitches
                          |> Seq.toArray
        let newSwitchCount = SwitchCount.create newSwitches.Length
        {
            sorter.order = sorter.order;
            switchCount=newSwitchCount;
            switches = newSwitches
        }


    let removeSwitchesFromTheStart (newLength:switchCount) (sorter:sorter) =
        let numSwitchesToRemove = (SwitchCount.value sorter.switchCount) -
                                  (SwitchCount.value newLength)
        if numSwitchesToRemove < 0 then
            "New length is longer than sorter" |> Error
        else
            let trimmedSwitches =  sorter.switches 
                                   |> Seq.skip(numSwitchesToRemove)
            fromSwitches sorter.order newLength trimmedSwitches |> Ok


    let removeSwitchesFromTheEnd (newLength:switchCount) (sorter:sorter) =
        let numSwitchesToRemove = (SwitchCount.value sorter.switchCount) -
                                  (SwitchCount.value newLength)
        if numSwitchesToRemove < 0 then
            "New length is longer than sorter" |> Error
        else
            fromSwitches sorter.order newLength sorter.switches |> Ok


    let getSwitchesFromFirstStages
                    (stageCount:stageCount) 
                    (sorter:sorter) =
        sorter.switches 
        |> Stage.fromSwitches sorter.order
        |> Seq.take(StageCount.value stageCount)
        |> Seq.map(fun t -> t.switches)
        |> Seq.concat


    let fromTwoCycles
                (order:order)
                (switchCtTarget:switchCount)
                (wPfx: switch seq) 
                (twoCycleSeq:twoCycle seq) =
        let switches = 
            twoCycleSeq 
            |> Seq.map(fun tc-> Switch.fromTwoCycle tc)
            |> Seq.concat

        fromSwitchesWithPrefix order switchCtTarget wPfx switches

    
    let makeAltEvenOdd (order:order) 
                       (wPfx: switch seq)
                       (switchCount:switchCount) =
        let switches = 
            TwoCycle.makeAltEvenOdd order (Permutation.identity order)
            |> Seq.map(fun tc-> Switch.fromTwoCycle tc)
            |> Seq.concat

        fromSwitchesWithPrefix order switchCount wPfx switches


    //***********  IRando dependent  *********************************

    let randomSwitches (order:order)
                       (wPfx: switch seq) 
                       (switchCount:switchCount) 
                       (rnd:IRando) =
        let switches = Switch.rndNonDegenSwitchesOfDegree order rnd
        fromSwitchesWithPrefix order switchCount wPfx switches


    let randomStages (switchFreq:switchFrequency) 
                     (order:order) 
                     (wPfx: switch seq) 
                     (switchCount:switchCount) 
                     (rando:IRando) =
        let switches = (Stage.rndSeq order switchFreq rando)
                       |> Seq.map (fun st -> st.switches)
                       |> Seq.concat
        fromSwitchesWithPrefix order switchCount wPfx switches


    let randomSymmetric (order:order) 
                        (wPfx: switch seq) 
                        (switchCount:switchCount) 
                        (rando:IRando) =
        let switches = (Stage.rndSymmetric order rando)
                       |> Seq.map (fun st -> st.switches)
                       |> Seq.concat
        fromSwitchesWithPrefix order switchCount wPfx switches


    let randomBuddies (stageWindowSz:stageWindowSize) 
                      (order:order) 
                      (wPfx: switch seq) 
                      (switchCount:switchCount)
                      (rando:IRando) =
        let switches = (Stage.rndBuddyStages 
                                stageWindowSz 
                                SwitchFrequency.max  
                                order 
                                rando
                                List.empty)
                        |> Seq.collect(fun st -> st.switches |> List.toSeq)

        fromSwitchesWithPrefix order switchCount wPfx switches




type sorterUniformMutatorType = |Switch |Stage |StageRfl
type sorterUniformMutator = private {sumType:sorterUniformMutatorType;
                                     mutationRate:mutationRate;
                                     mFunc: sorter -> IRando -> Result<sorter, string> }

module SorterUniformMutator =

    let create t r f  = 
        {
            sorterUniformMutator.sumType = t; 
            mutationRate =r; 
            mFunc = f
        }

    let getSorterUniformMutatorType (sum:sorterUniformMutator) =
        sum.sumType

    let getMutationRateVal (sum:sorterUniformMutator) =
        sum.mutationRate |> MutationRate.getRateValue

    let mutateBySwitch (swMr:switchMutationRate) =

        (fun (sorter:sorter) (randy:IRando) ->
            result {
                let newSwitches = Switch.mutateSwitches sorter.order 
                                                        swMr
                                                        randy
                                                        sorter.switches
                                   |> Seq.toArray
                let newSwitchCount = newSwitches.Length |> SwitchCount.create
                return {
                           sorter.order = sorter.order;
                           sorter.switchCount = newSwitchCount;
                           sorter.switches =  newSwitches
                        }
            }) |> create sorterUniformMutatorType.Switch (swMr |> mutationRate.Switch)


    let mutateByStage (stMr:stageMutationRate) =

        (fun (sorter:sorter) (randy:IRando) ->
            result {
                let mutantStages = 
                          sorter.switches
                             |> Stage.fromSwitches sorter.order
                             |> Seq.toArray
                             |> Array.map(Stage.randomMutate randy stMr)

                let newSwitches = [| for stage in mutantStages do yield! stage.switches |]
                                   |> Seq.toArray
                let newSwitchCount = newSwitches.Length |> SwitchCount.create
                return {
                        sorter.order = sorter.order;
                        sorter.switchCount = newSwitchCount;
                        sorter.switches =  newSwitches
                        }
            }) |> create sorterUniformMutatorType.Stage (stMr |> mutationRate.Stage)


    let mutateByStageRfl (stMr:stageMutationRate) =

        (fun (sorter:sorter) (randy:IRando) ->
            result {
                let mutantStages = 
                          sorter.switches
                             |> Stage.fromSwitches sorter.order
                             |> Seq.toArray
                             |> Array.map(Stage.randomReflMutate randy stMr)

                let newSwitches = [| for stage in mutantStages do yield! stage.switches |]
                let newSwitchCount = newSwitches.Length |> SwitchCount.create

                return {
                        sorter.order = sorter.order;
                        sorter.switchCount = newSwitchCount;
                        sorter.switches =  newSwitches
                        }
            }) |> create sorterUniformMutatorType.StageRfl (stMr |> mutationRate.Stage)


type sorterMutator =
     | Uniform of sorterUniformMutator