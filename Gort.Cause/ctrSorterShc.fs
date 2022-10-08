namespace global

open System
open Gort.Data.DataModel

module ctrSorterShc =

    let RunSorterShc (cz: Cause) (ctxt: Gort.Data.DataModel.IGortContext) =
        match cz.CauseType.Name with
        | "SorterShc" -> 5 |> Ok
        | n -> (sprintf "%s not handled in RunSorterShc" n) |> Error


    let RunSorterShcChildren (cz: Cause) (pth: string list) (ctxt: Gort.Data.DataModel.IGortContext) =
        let g = pth |> List.head

        match g with
        | n -> (sprintf "%s not handled in RunSorterShcChildren" n) |> Error
