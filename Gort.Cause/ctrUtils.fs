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
            let! rr = gcOps.MakeRndGenRecordAndTable rngT rsv cz.CauseId "" ctxt true
            return 1
        }

    let runRngSet (cz:Cause) 
                  (ctxt:Gort.Data.DataModel.IGortContext) =
        result {
            let prams = cz.CauseParams |> Seq.toList
            let pramRngID = prams |> List.find(fun p -> 
                p.CauseParamType.Name = CauseParamTypeName.RngId.ToString())
            let pramCount = prams |> List.find(fun p -> 
                p.CauseParamType.Name = CauseParamTypeName.RngCount.ToString())
            let! rngId = pramRngID.Param.Value |> byteConv.guidFromBytes
            let! rngCt = pramCount.Param.Value |> byteConv.intFromBytes
            let! rng =  gcOps.MakeRndGenFromRecord rngId ctxt
            let randy = rng |> Rando.fromRngGen
            return! gcOps.MakeRndGenSetRecordsAndTable randy rngCt cz.CauseId ctxt
        }


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
        | n -> (sprintf "%s not handled in RunUtilsChildren" n) |> Error