namespace global
open System
open Gort.Data.DataModel

module ctrSorterShc =

    let RunSorterShc (cz:Cause) (ctxt:Gort.Data.DataModel.IGortContext) =
        match cz.CauseType.Name with
        | "rngName" -> 5|>Ok
        | "rngSetName" -> 5|>Ok
        | n -> (sprintf "%s not handled in RunRoot" n) |> Error


    let RunSorterShcChildren (cz:Cause) (pth:string list) (ctxt:Gort.Data.DataModel.IGortContext) =
        let g::gs = pth
        match g with
        | "Sortable" -> 5 |> Ok
        | "Utils" -> 5 |> Ok
        | "Sorter" -> 5 |> Ok
        | "SwitchList" -> 5 |> Ok
        | "SorterPerf" -> 5 |> Ok
        | "SorterShc" -> 5 |> Ok
        | n -> (sprintf "%s not handled in RunRoot" n) |> Error