namespace global
open System
open Gort.Data.Instance
open Gort.Data.Instance.CauseBuilder
open Gort.Data.Instance.SeedParams
open Gort.Data.Instance.CauseBuilder.SortableSet
open Gort.Data.Instance.CauseBuilder.Utils

module Utils = 

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

