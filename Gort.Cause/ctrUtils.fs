namespace global
open System
open Gort.Data.DataModel

module ctrUtils =
    
    let runMakeRandGenRecord (cz:Cause) 
               (ctxt:Gort.Data.DataModel.IGortContext) =
        result {
            let prams = cz.CauseParams |> Seq.toList
            let pramRandGenType = prams |> List.find(fun p -> 
                p.CauseParamType.Name = CauseParamTypeName.RandGenType.ToString())
            let pramRandGenSeed = prams |> List.find(fun p -> 
                p.CauseParamType.Name = CauseParamTypeName.RandGenSeed.ToString())
            let! rngT = paramConv.RandGenTypeFromParam pramRandGenType.Param
            let! seedVal = pramRandGenSeed.Param.Value |> byteConv.intFromBytes
            let! rr = gcOps.MakeRandGenRecordAndTable rngT seedVal cz.CauseId "" ctxt
            return 1
        }


    let runMakeSetOfRandGenRecords (cz:Cause) 
                  (ctxt:Gort.Data.DataModel.IGortContext) =
        result {
            let prams = cz.CauseParams |> Seq.toList
            let pramRngID = prams |> List.find(fun p -> 
                p.CauseParamType.Name = CauseParamTypeName.RandGenId.ToString())
            let pramCount = prams |> List.find(fun p -> 
                p.CauseParamType.Name = CauseParamTypeName.RandGenCount.ToString())

            let! randGenCt = pramCount.Param.Value |> byteConv.intFromBytes
            let! randy = pramRngID.Param |> paramConv.RandoFromRandGenRecordParam ctxt
            return! gcOps.MakeRandGenSetRecordsAndTable randy randGenCt cz.CauseId ctxt
        }


    let RunUtils (cz:Cause) (ctxt:Gort.Data.DataModel.IGortContext) =
        match cz.CauseType.Name with
        | "Rng" -> runMakeRandGenRecord cz ctxt
        | "RngSet" -> runMakeSetOfRandGenRecords cz ctxt
        | n -> (sprintf "%s not handled in RunRoot" n) |> Error

    let RunUtilsChildren (cz:Cause) 
                         (pth:string list) 
                         (ctxt:Gort.Data.DataModel.IGortContext) =
        let g::gs = pth
        match g with
        | n -> (sprintf "%s not handled in RunUtilsChildren" n) |> Error