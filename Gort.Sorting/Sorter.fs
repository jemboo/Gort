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