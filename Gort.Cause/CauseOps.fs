namespace global
open System
open System.Text
open Gort.Data.Utils

module CauseOps =

    let GetAllCausesForWorkspace (ctxt:Gort.Data.DataModel.IGortContext) (wsName:String) =
        try
            CauseQuery.GetAllCausesForWorkspace(wsName, ctxt)
             |> Ok
        with
            | ex -> ("error in GetAllCausesForWorkspace: " + ex.Message ) |> Result.Error


    let GetPendingCauseForWorkspace (ctxt:Gort.Data.DataModel.IGortContext) (wsName:String) =
        try
            CauseQuery.GetPendingCauseForWorkspace(wsName, ctxt)
             |> Ok
        with
            | ex -> ("error in GetAllCausesForWorkspace: " + ex.Message ) |> Result.Error


    let GetCauseById (ctxt:Gort.Data.DataModel.IGortContext) (causeId:Guid) =
        try
            CauseQuery.GetCauseById(causeId, ctxt)
             |> Ok
        with
            | ex -> ("error in GetCauseById: " + ex.Message ) |> Result.Error


    let GetCauseTypeGroupAncestry (ctxt:Gort.Data.DataModel.IGortContext) (cauz:Gort.Data.DataModel.Cause) =
        try
            CauseQuery.GetCauseTypeGroupAncestry(cauz, ctxt)
             |> Ok
        with
            | ex -> ("error in GetCauseTypeGroupAncestry: " + ex.Message ) |> Result.Error


    let GetParamType (cauz:Gort.Data.DataModel.ParamTypeName) (ctxt:Gort.Data.DataModel.IGortContext) =
        try
            CauseQuery.GetParamType(cauz, ctxt)
             |> Ok
        with
            | ex -> ("error in GetParamType: " + ex.Message ) |> Result.Error


    let GetCauseDispatcherInfo (ctxt:Gort.Data.DataModel.IGortContext) (causeId:Guid) =
        result {
            let! cause = GetCauseById  ctxt causeId
            let! cestry = GetCauseTypeGroupAncestry ctxt cause
            let pth = cestry |> Array.map(fun ctg -> ctg.Name)
            return (cause, pth);
        }

    let RunNextCause (ctxt:Gort.Data.DataModel.IGortContext) (wsName:String)=
        result {
            let! nextCause = GetPendingCauseForWorkspace ctxt wsName
            let! cz, cestry = GetCauseDispatcherInfo ctxt nextCause.CauseId 
            let! dex = ctrRoot.RunCause cz (cestry |> Array.toList) ctxt
            return dex
        }