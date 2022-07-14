namespace global
open System
open Gort.Data.Instance
open Gort.Data.Instance.CauseBuilder
open Gort.Data.Instance.SeedParams

module WorkspaceRunner = 

    let LoadCauseBuilder (czBuilder:CauseBuilderBase) (ctxt:Gort.Data.DataModel.IGortContext) =
        try
            WorkspaceLoad.LoadCauseBuilder(czBuilder, ctxt)
             |> Ok
        with
            | ex -> ("error in LoadCauseBuilder: " + ex.Message ) |> Result.Error


    let LoadSeedParams (seedParams:SeedParamsBase) (ctxt:Gort.Data.DataModel.IGortContext) =
        try
            WorkspaceLoad.LoadSeedParams(seedParams, ctxt)
                |> Ok
        with
            | ex -> ("error in LoadSeedParams: " + ex.Message ) |> Result.Error


    let LoadStatics (ctxt:Gort.Data.DataModel.IGortContext) =
        try
            WorkspaceLoad.LoadStatics(ctxt)
                |> Ok
        with
            | ex -> ("error in LoadStatics: " + ex.Message ) |> Result.Error


    let GetParamRngId (ctxt:Gort.Data.DataModel.IGortContext) (cbRand:CbRand) =
        try
            CbRandExt.GetParamRngId(cbRand, ctxt) |> Ok
        with
            | ex -> ("error in LoadStatics: " + ex.Message ) |> Result.Error


    let RunCbRandGen (ctxt:Gort.Data.DataModel.IGortContext) (wsName:string) =
        let seedParams = new SeedParamsA();
        result {
            let! resSp = LoadStatics ctxt
            let! resSt = LoadSeedParams seedParams ctxt
            let mutable curCauseDex = 1
            let descr = sprintf "RandGen_%d" curCauseDex
            let cbRnd = new CbRand(wsName, curCauseDex, descr, 
                        seedParams.RngSeed, seedParams.RngType)
            let! resCbRnd = LoadCauseBuilder cbRnd ctxt
            let! dexB = CauseOps.RunNextCause ctxt wsName
            return dexB
        }


    let RunCbRandGenSet (ctxt:Gort.Data.DataModel.IGortContext) (wsName:string) =
        let seedParams = new SeedParamsA();
        result {
            let! resSp = LoadStatics ctxt
            let! resSt = LoadSeedParams seedParams ctxt
            let mutable curCauseDex = 1
            let descr = sprintf "RndGen_%d" curCauseDex
            let cbRnd = new CbRand(
                    wsName, 
                    curCauseDex, 
                    descr, 
                    seedParams.RngSeed, 
                    seedParams.RngType)
            let! resCbRnd = LoadCauseBuilder cbRnd ctxt
            let! dexB1 = CauseOps.RunNextCause ctxt wsName
            let! pRamRngId = GetParamRngId ctxt cbRnd
            curCauseDex <- curCauseDex + 1
            let descr2 = sprintf "RandGenSet_%d" curCauseDex
            let cbRndSet = new CbRandSet(wsName, curCauseDex, 
                    descr2, pRamRngId, seedParams.RngCount)
            let! resCbRnd = LoadCauseBuilder cbRndSet ctxt
            let! dexB2 = CauseOps.RunNextCause ctxt wsName
            return pRamRngId.ParamId
        }


    let RunCbRandSortableSet 
                (ctxt:Gort.Data.DataModel.IGortContext) 
                (wsName:string) =
        let seedParams = new SeedParamsA();
        let resSp = LoadStatics ctxt |> Result.ExtractOrThrow
        let resSt = LoadSeedParams seedParams ctxt |> Result.ExtractOrThrow
        let mutable curCauseDex = 1
        let descr = sprintf "RndSortableSet_%d" curCauseDex
        let cbRnd = 
            new CbRand(
                wsName, 
                curCauseDex, 
                descr, 
                seedParams.RngSeed, 
                seedParams.RngType)
        let resCbRnd = LoadCauseBuilder cbRnd ctxt  |> Result.ExtractOrThrow
        let dexB1 = CauseOps.RunNextCause ctxt wsName  |> Result.ExtractOrThrow
        let pRamRngId = GetParamRngId ctxt cbRnd  |> Result.ExtractOrThrow
        curCauseDex <- curCauseDex + 1
        let descr2 = sprintf "RndGenSet_%d" curCauseDex
        let cbRndSortableSet =  
            new CbRandSortableSet(
                    wsName, 
                    curCauseDex, 
                    descr, 
                    seedParams.Order, 
                    seedParams.SortableCount,
                    seedParams.SortableFormat,
                    pRamRngId)
        let resCbRnd = LoadCauseBuilder cbRndSortableSet ctxt  |> Result.ExtractOrThrow
        let dexB2 = CauseOps.RunNextCause ctxt wsName  |> Result.ExtractOrThrow
        1 |> Ok


        //result {
        //    let! resSp = LoadStatics ctxt
        //    let! resSt = LoadSeedParams seedParams ctxt
        //    let mutable curCauseDex = 1
        //    let descr = sprintf "RndSortableSet_%d" curCauseDex
        //    let cbRnd = 
        //        new CbRand(
        //            wsName, 
        //            curCauseDex, 
        //            descr, 
        //            seedParams.RngSeed, 
        //            seedParams.RngType)
        //    let! resCbRnd = LoadCauseBuilder cbRnd ctxt
        //    let! dexB1 = CauseOps.RunNextCause ctxt wsName
        //    let! pRamRngId = GetParamRngId ctxt cbRnd
        //    curCauseDex <- curCauseDex + 1
        //    let descr2 = sprintf "RndGenSet_%d" curCauseDex
        //    let cbRndSortableSet =  
        //        new CbRandSortableSet(
        //                wsName, 
        //                curCauseDex, 
        //                descr, 
        //                seedParams.Order, 
        //                seedParams.SortableCount,
        //                seedParams.SortableFormat,
        //                pRamRngId)
        //    let! resCbRnd = LoadCauseBuilder cbRndSortableSet ctxt
        //    let! dexB2 = CauseOps.RunNextCause ctxt wsName
        //    return pRamRngId.ParamId
        //}