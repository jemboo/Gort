namespace global
open System
open Gort.Data.DataModel

module ctrUtils =
    
    let runRng (cz:Cause) 
               (ctxt:Gort.Data.DataModel.IGortContext) =
        result {
            let prams = cz.CauseParams |> Seq.toList
            let pramRngType = prams |> List.find(fun p -> 
                p.CauseParamType.Name = CauseParamTypeName.RngType.ToString())
            let pramSeed = prams |> List.find(fun p -> 
                p.CauseParamType.Name = CauseParamTypeName.RngSeed.ToString())
            let! rtv = pramRngType.Param.Value |> byteConv.intFromBytes
            let! rngT = miscConv.RndGenTypeFromInt rtv
            let! rsv = pramSeed.Param.Value |> byteConv.intFromBytes
            let! rr = gcOps.MakeRndGenRecordAndTable rngT rsv cz.CauseId "" ctxt
            return 1
        }

    let runRngSet (cz:Cause) 
                  (ctxt:Gort.Data.DataModel.IGortContext) =

        1 |> Ok


    let RunUtils (cz:Cause) (ctxt:Gort.Data.DataModel.IGortContext) =
        match cz.CauseType.Name with
        | "Rng" -> runRng cz ctxt
        | "RngSet" -> runRngSet cz ctxt
        | n -> (sprintf "%s not handled in RunRoot" n) |> Error

    let RunUtilsChildren (cz:Cause) 
                         (pth:string list) 
                         (ctxt:Gort.Data.DataModel.IGortContext) =
        let g::gs = pth
        match g with
        | "Sortable" -> 5 |> Ok
        | "Utils" -> 5 |> Ok
        | "Sorter" -> 5 |> Ok
        | "SwitchList" -> 5 |> Ok
        | "SorterPerf" -> 5 |> Ok
        | "SorterShc" -> 5 |> Ok
        | n -> (sprintf "%s not handled in RunUtilsChildren" n) |> Error