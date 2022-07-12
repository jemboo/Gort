namespace global
open System
open Gort.Data.Instance
open Gort.Data.Instance.CauseBuilder
open Gort.Data.Instance.SeedParams

module WorkspaceBuilder = 

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


    let LoadRun (ctxt:Gort.Data.DataModel.IGortContext) (wsName:string) =
        let seedParams = new SeedParamsA();
        result {
            let! resSp = LoadStatics ctxt
            let! resSt = LoadSeedParams seedParams ctxt
            let mutable curCauseDex = 1
            let descr = sprintf "RndGen_%d" curCauseDex
            let cbRnd = new CbRand(wsName, curCauseDex, descr, seedParams.RngSeed, seedParams.RngType)
            let! resCbRnd = LoadCauseBuilder cbRnd ctxt
            let! dexB = CauseOps.RunNextCause ctxt wsName
            let! pRam = GetParamRngId ctxt cbRnd
            return pRam.ParamId
        }

