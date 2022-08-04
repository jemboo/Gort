namespace global
open System
open Gort.Data.DataModel

module ctrSortableSetDef =

    let RunSortableSetDef (cz:Cause) (ctxt:Gort.Data.DataModel.IGortContext) =
        match cz.CauseType.Name with
        | "SortableImport" -> 5|>Ok
        | n -> (sprintf "%s not handled in RunSortable" n) |> Error

    let RunSortableSetDefChildren (cz:Cause) (pth:string list) (ctxt:Gort.Data.DataModel.IGortContext) =
        match pth with
        | [] -> "No path in RunSortableChildren" |> Error
        | x::[] -> match x with  
                   | "SortableSetImport" -> 5 |> Ok
                   | "SortableSetAllForOrder" -> 5 |> Ok
                   | "SortableSetOrbit" -> 5 |> Ok
                   | "SortableSetStacked" -> 5 |> Ok
                   | _ -> "Bad path in RunSortableChildren" |> Error
        | x::xs -> match x with  
                   | "SortableSetDef" -> 5 |> Ok
                   | "SortableSetRnd" -> 5 |> Ok
                   | _ -> "Bad path in RunSortableChildren" |> Error
