namespace global
open System
open Gort.Data.DataModel

module ctrSortable =
    let runSortableImport (cz:Cause) (ctxt:Gort.Data.DataModel.IGortContext) =
        5 |> Ok

    let runSortableSetDef (cz:Cause) (ctxt:Gort.Data.DataModel.IGortContext) =
        5 |> Ok

    let runSortableSetRnd (cz:Cause) (ctxt:Gort.Data.DataModel.IGortContext) =
        result {
            let prams = cz.CauseParams |> Seq.toList
            let pramRngID = prams |> List.find(fun p -> 
                    p.CauseParamType.Name = CauseParamTypeName.RandGenId.ToString())
            let pramOrder = prams |> List.find(fun p -> 
                    p.CauseParamType.Name = CauseParamTypeName.Order.ToString())
            let pramSortableFormat = prams |> List.find(fun p -> 
                    p.CauseParamType.Name = CauseParamTypeName.SortableFormat.ToString())
            let pramSortableCount = prams |> List.find(fun p -> 
                    p.CauseParamType.Name = CauseParamTypeName.SortableCount.ToString())

            let! randy = pramRngID.Param |> paramConv.RandoFromRandGenRecordParam ctxt
            let! ord = pramOrder.Param.Value |> byteConv.intFromBytes
            let ordr = ord |> Order.create

            let! sortableFormat = pramSortableFormat.Param.Value |> byteConv.intFromBytes

            let! sortableCount = pramOrder.Param.Value |> byteConv.intFromBytes
            //let! rngT = miscConv.RndGenTypeFromInt rtv
            //let! rr = gcOps.MakeRndGenRecordAndTable rngT rsv cz.CauseId "" ctxt
            return 1
        }

    let RunSortable (cz:Cause) (ctxt:Gort.Data.DataModel.IGortContext) =
        match cz.CauseType.Name with
        | "SortableImport" -> runSortableImport cz ctxt
        | n -> (sprintf "%s not handled in RunSortable" n) |> Error

    let RunSortableChildren (cz:Cause) (pth:string list) (ctxt:Gort.Data.DataModel.IGortContext) =
        match pth with
        | [] -> "No path in RunSortableChildren" |> Error
        | x::[] -> match x with  
            | "SortableSetDef" -> runSortableSetDef cz ctxt
            | "SortableSetRnd" -> runSortableSetRnd cz ctxt
            | _ -> "Bad path in RunSortableChildren" |> Error
        | x::xs -> match x with  
                   | "SortableSetDef" -> 5 |> Ok
                   | "SortableSetRnd" -> 5 |> Ok
                   | _ -> "Bad path in RunSortableChildren" |> Error
