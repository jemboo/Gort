namespace global
open System
open Gort.Data.DataModel

module ctrRoot =

    let RunRoot (cz:Cause) 
                (ctxt:Gort.Data.DataModel.IGortContext) =
        match cz.CauseType.Name with
        | n -> (sprintf "%s not handled in RunRoot" n) |> Error


    let RunRootChildren (cz:Cause) (pth:string list) (ctxt:Gort.Data.DataModel.IGortContext) =
        match pth with
        | [] -> "No path in RunRootChildren" |> Error
        | x::[] -> match x with  
            | "Sortable" -> ctrSorter.RunSortable cz ctxt
            | "Utils" -> ctrUtils.RunUtils cz ctxt
            | "Sorter" -> ctrSortable.RunSorter cz ctxt
            | "SwitchList" -> ctrSwitchList.RunSwitchList cz ctxt
            | "SorterPerf" -> ctrSorterPerf.RunSorterPerf cz ctxt
            | "SorterShc" -> ctrSorterShc.RunSorterShc cz ctxt
            | _ -> "Bad path in RunRootChildren" |> Error
        | x::xs -> match x with  
                   | "Sortable" -> ctrSorter.RunSortableChildren cz xs ctxt
                   | "Utils" -> ctrUtils.RunUtilsChildren cz xs ctxt
                   | "Sorter" -> ctrSortable.RunSorterChildren cz xs ctxt
                   | "SwitchList" -> ctrSwitchList.RunSwitchListChildren cz xs ctxt
                   | "SorterPerf" -> ctrSorterPerf.RunSorterPerfChildren cz xs ctxt
                   | "SorterShc" -> ctrSorterShc.RunSorterShcChildren cz xs ctxt
                   | _ -> "Bad path in RunRootChildren" |> Error


    let RunCause (cz:Cause) 
                 (pth:string list) 
                 (ctxt:Gort.Data.DataModel.IGortContext) =
        if cz.CauseStatus = CauseStatus.Pending then
            result {
                let! res = 
                    match pth with
                    | [] -> "No path in RunCause" |> Error
                    | x::[] -> match x with  
                        | "Root" -> RunRoot cz ctxt
                        | _ -> "Bad path in RunCause" |> Error
                    | x::xs -> match x with  
                               | "Root" -> RunRootChildren cz xs ctxt
                               | _ -> "Bad path in RunCause" |> Error

                if (res > 0) then
                    cz.CauseStatus <- CauseStatus.Complete
                    ctxt.SaveChanges() |> ignore
                return res
            }
        else sprintf "Cause status was %s" (cz.CauseStatus.ToString()) |> Error