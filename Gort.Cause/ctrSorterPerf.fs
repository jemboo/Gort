namespace global

open System
open Gort.Data.DataModel

module ctrSorterPerf =

    let RunSorterPerf (cz: Cause) (ctxt: Gort.Data.DataModel.IGortContext) =
        match cz.CauseType.Name with
        | "SorterPerf" -> 5 |> Ok
        | "SorterSetPerf" -> 5 |> Ok
        | n -> (sprintf "%s not handled in RunSorterPerf" n) |> Error

    let RunSorterPerfChildren (cz: Cause) (pth: string list) (ctxt: Gort.Data.DataModel.IGortContext) =
        let g = pth |> List.head

        match g with
        | n -> (sprintf "%s not handled in RunSorterPerfChildren" n) |> Error
