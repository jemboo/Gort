namespace global

open System
open Gort.Data.DataModel

module ctrSorter =

    let RunSorter (cz: Cause) (ctxt: Gort.Data.DataModel.IGortContext) =
        match cz.CauseType.Name with
        | "SorterImport" -> 5 |> Ok
        | "SorterGroupName" -> 5 |> Ok
        | n -> (sprintf "%s not handled in RunSorter" n) |> Error

    let RunSorterChildren (cz: Cause) (pth: string list) (ctxt: Gort.Data.DataModel.IGortContext) =

        match pth with
        | [] -> "No path in RunSorterChildren" |> Error
        | x :: [] ->
            match x with
            | "SorterSetDef" -> 5 |> Ok
            | "SorterSetRnd" -> 5 |> Ok
            | _ -> "Bad path in RunSorterChildren" |> Error
        | x :: xs ->
            match x with
            | "SorterSetDef" -> 5 |> Ok
            | "SorterSetRnd" -> 5 |> Ok
            | _ -> "Bad path in RunSorterChildren" |> Error
