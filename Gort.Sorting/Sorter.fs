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
        let gu = [s.switches :> obj] |> GuidUtils.guidFromObjs
        SorterId.create gu


    let fromSwitches (order:order) 
                     (switches:seq<switch>) =
        let switchArray = switches |> Seq.toArray
        let switchCount = SwitchCount.create switchArray.Length
        {
            sorter.order=order;
            switchCount=switchCount;
            switches = switchArray
        }


    let fromStages (order:order) 
                   (stages:seq<stage>) =
        let switchArray = stages |> Seq.map(fun st->st.switches)
                                    |> Seq.concat
                                    |> Seq.toArray
        let switchCount = SwitchCount.create switchArray.Length
        {
            sorter.order=order;
            switchCount=switchCount;
            switches = switchArray
        }
   

    let appendSwitches (switches:seq<switch>) 
                       (sorter:sorter) =
        let newSwitches = (switches |> Seq.toArray) |> Array.append sorter.switches
        let newSwitchCount = SwitchCount.create newSwitches.Length
        {
            sorter.order = sorter.order;
            switchCount=newSwitchCount;
            switches = newSwitches
        }


    let trimLength (trimEnd:bool) (newLength:switchCount) (sorter:sorter) =
        if (SwitchCount.value sorter.switchCount) < (SwitchCount.value newLength) then
            "New length is longer than sorter" |> Error
        else
        let newSwitches =  match trimEnd with 
                           | true -> sorter.switches 
                                      |> Array.take (SwitchCount.value newLength)
                           | _ -> sorter.switches 
                                      |> Array.skip((sorter.switches.Length) - (SwitchCount.value newLength))
        {
            sorter.order = sorter.order;
            switchCount = newLength;
            switches = newSwitches
        } |> Ok


    let getSwitchPrefix (stageCount:stageCount) 
                        (sorter:sorter) =
        sorter.switches |> Stage.fromSwitches sorter.order
                        |> Seq.take(StageCount.value stageCount)
                        |> Seq.map(fun t -> t.switches)
                        |> Seq.concat


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