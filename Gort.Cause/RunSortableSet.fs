namespace global
open System
open Gort.Data.Instance
open Gort.Data.Instance.CauseBuilder
open Gort.Data.Instance.SeedParams
open Gort.Data.Instance.CauseBuilder.SortableSet
open Gort.Data.Instance.CauseBuilder.Utils
open Gort.Data.DataModel

module RunSortableSet = 

    let RunCbSortableSetRand
                (ctxt:Gort.Data.DataModel.IGortContext) 
                (wsName:string) =
        let seedParams = new SeedParamsA();
        let resSp = Utils.LoadStatics ctxt |> Result.ExtractOrThrow
        let resSt = Utils.LoadSeedParams seedParams ctxt |> Result.ExtractOrThrow
        let mutable curCauseDex = 1
        let descr = sprintf "RndSortableSet_%d" curCauseDex

        let cbRnd = new CbRand(
                        wsName, 
                        curCauseDex,
                        descr, 
                        seedParams.RngSeed, 
                        seedParams.RngType)
        let resCbRnd = Utils.LoadCauseBuilder cbRnd ctxt  |> Result.ExtractOrThrow
        let dexB1 = CauseOps.RunNextCause ctxt wsName  |> Result.ExtractOrThrow
        let pRamRngId = Utils.GetParamRngId ctxt cbRnd  |> Result.ExtractOrThrow
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
        let resCbRnd = Utils.LoadCauseBuilder cbRndSortableSet ctxt  |> Result.ExtractOrThrow
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

    let RunCbSortableSetAllForOrder
                (ctxt:Gort.Data.DataModel.IGortContext) 
                (wsName:string) =
        let seedParams = new SeedParamsA()        
        let resSp = Utils.LoadStatics ctxt |> Result.ExtractOrThrow
        let resSt = Utils.LoadSeedParams seedParams ctxt |> Result.ExtractOrThrow
        let mutable curCauseDex = 1
        let descr = sprintf "RunSortableSetAllForOrder_%d" curCauseDex     
        
        let cbSortableSetAllForOrder =  
            new CbSortableSetAllForOrder(
                    wsName, 
                    curCauseDex, 
                    descr, 
                    seedParams.Order,
                    seedParams.SortableFormat)
        let resCbRnd = Utils.LoadCauseBuilder cbSortableSetAllForOrder ctxt  |> Result.ExtractOrThrow
        let dexB2 = CauseOps.RunNextCause ctxt wsName  |> Result.ExtractOrThrow
        1 |> Ok



    let RunCbSortableSetImport
                (ctxt:Gort.Data.DataModel.IGortContext) 
                (wsName:string) =
        let seedParams = new SeedParamsA()
        let resSp = Utils.LoadStatics ctxt |> Result.ExtractOrThrow
        let resSt = Utils.LoadSeedParams seedParams ctxt |> Result.ExtractOrThrow
        let mutable curCauseDex = 1
        let descr = sprintf "RunSortableSetImport_%d" curCauseDex
        let paramWorkspaceID = new Param()
        let paramRecordId = new Param()

        let cbSortableSetImport =  
            new CbSortableSetImport(
                    wsName,
                    curCauseDex, 
                    descr, 
                    paramWorkspaceID,
                    seedParams.ImportTableName,
                    paramRecordId,
                    seedParams.ImportRecordPath)
        let resCbRnd = Utils.LoadCauseBuilder cbSortableSetImport ctxt  |> Result.ExtractOrThrow
        let dexB2 = CauseOps.RunNextCause ctxt wsName  |> Result.ExtractOrThrow
        1 |> Ok
    


    let RunCbSortableSetStacked
                (ctxt:Gort.Data.DataModel.IGortContext) 
                (wsName:string) =
        let seedParams = new SeedParamsA()
        let resSp = Utils.LoadStatics ctxt |> Result.ExtractOrThrow
        let resSt = Utils.LoadSeedParams seedParams ctxt |> Result.ExtractOrThrow
        let mutable curCauseDex = 1
        let descr = sprintf "RunSortableSetStacked_%d" curCauseDex
        let cbSortableSetStacked =  
            new CbSortableSetStacked(
                    wsName, 
                    curCauseDex, 
                    descr, 
                    seedParams.OrderStack,
                    seedParams.SortableFormat)

        let resCbRnd = Utils.LoadCauseBuilder cbSortableSetStacked ctxt  |> Result.ExtractOrThrow
        let dexB2 = CauseOps.RunNextCause ctxt wsName  |> Result.ExtractOrThrow
        1 |> Ok