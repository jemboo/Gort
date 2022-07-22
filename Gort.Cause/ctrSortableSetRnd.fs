namespace global
open System
open Gort.Data.DataModel

module ctrSortableSetRnd =

    let RunSortableSetRnd (cz:Cause) (ctxt:Gort.Data.DataModel.IGortContext) =
        match cz.CauseType.Name with
        | "SortableImport" -> 5|>Ok
        | n -> (sprintf "%s not handled in RunSortable" n) |> Error
