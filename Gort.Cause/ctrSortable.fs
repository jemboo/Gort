namespace global

open System
open Gort.Data.DataModel

module ctrSortable =
    let runSortableImport (cz: Cause) (ctxt: Gort.Data.DataModel.IGortContext) = 5 |> Ok

    let runSortableSetImport (cz: Cause) (ctxt: Gort.Data.DataModel.IGortContext) = 5 |> Ok

    let runSortableSetDef (cz: Cause) (ctxt: Gort.Data.DataModel.IGortContext) = 5 |> Ok

    let runSortableSetRnd (cz: Cause) (ctxt: Gort.Data.DataModel.IGortContext) =
        result {
            let prams = cz.CauseParams |> Seq.toList

            let pramRngID =
                prams
                |> List.find (fun p -> p.CauseParamType.Name = CauseParamTypeName.RandGenId.ToString())

            let pramOrder =
                prams
                |> List.find (fun p -> p.CauseParamType.Name = CauseParamTypeName.Order.ToString())

            let pramSortableFormat =
                prams
                |> List.find (fun p -> p.CauseParamType.Name = CauseParamTypeName.SortableFormat.ToString())

            let pramSortableCount =
                prams
                |> List.find (fun p -> p.CauseParamType.Name = CauseParamTypeName.SortableCount.ToString())

            let! randy = pramRngID.Param |> paramConv.RandoFromRandGenRecordParam ctxt
            let! ord = pramOrder.Param.Value |> byteConv.intFromBytes
            let ordr = ord |> Order.create

            let! sortableFormat = pramSortableFormat.Param.Value |> byteConv.intFromBytes

            let! sortableCount = pramOrder.Param.Value |> byteConv.intFromBytes
            //let! rngT = miscConv.RndGenTypeFromInt rtv
            //let! rr = gcOps.MakeRndGenRecordAndTable rngT rsv cz.CauseId "" ctxt
            return 1
        }

    let RunSortable (cz: Cause) (ctxt: Gort.Data.DataModel.IGortContext) =
        match cz.CauseType.Name with
        | "SortableImport" -> runSortableImport cz ctxt
        | "SortableSetImport" -> runSortableSetImport cz ctxt
        | n -> (sprintf "The Cause: %s not a child of Sortable" n) |> Error

    let RunSortableChildren (cz: Cause) (pth: string list) (ctxt: Gort.Data.DataModel.IGortContext) =
        match pth with
        | [] -> "No path in RunSortableChildren" |> Error
        | x :: [] ->
            match x with
            | "SortableSetDef" -> ctrSortableSetDef.RunSortableSetDef cz ctxt
            | "SortableSetRnd" -> ctrSortableSetRnd.RunSortableSetRnd cz ctxt
            | _ -> (sprintf "The CauseTypeGroup: %s is not a child of Sortable" x) |> Error
        | x :: xs ->
            match x with
            | "SortableSetDef" -> (sprintf "CauseTypeGroup: %s is not a child of SortableSetDef" xs.[0]) |> Error
            | "SortableSetRnd" -> (sprintf "CauseTypeGroup: %s is not a child of SortableSetRnd" xs.[0]) |> Error
            | _ -> (sprintf "CauseTypeGroup: %s is not a child of Sortable" x) |> Error
