namespace global
open System
open Gort.Data.DataModel

module ctrRoot =

    let RunRootChildren (cz:Cause) 
                        (pth:string list) 
                        (ctxt:Gort.Data.DataModel.IGortContext) =
        match pth with
        | [] -> "No causes are children of Root" |> Error
        | x::[] -> match x with  
            | "Sortable" -> ctrSortable.RunSortable cz ctxt
            | "Utils" -> ctrUtils.RunUtils cz ctxt
            | "Sorter" -> ctrSorter.RunSorter cz ctxt
            | "SwitchList" -> ctrSwitchList.RunSwitchList cz ctxt
            | "SorterPerf" -> ctrSorterPerf.RunSorterPerf cz ctxt
            | "SorterShc" -> ctrSorterShc.RunSorterShc cz ctxt
            | _ -> "Bad path in RunRootChildren" |> Error
        | x::xs -> match x with  
                   | "Sortable" -> ctrSortable.RunSortableChildren cz xs ctxt
                   | "Utils" -> "no CauseTypeGroups are children of Utils" |> Error
                   | "Sorter" -> ctrSorter.RunSorterChildren cz xs ctxt
                   | "SwitchList" -> ctrSwitchList.RunSwitchListChildren cz xs ctxt
                   | "SorterPerf" -> ctrSorterPerf.RunSorterPerfChildren cz xs ctxt
                   | "SorterShc" -> ctrSorterShc.RunSorterShcChildren cz xs ctxt
                   | _ -> "Bad path in RunRootChildren" |> Error