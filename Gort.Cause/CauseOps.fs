namespace global
open System
open System.Text

module CauseOps =

    let GetAllCausesForWorkspace (wsName:String) (ctxt:Gort.Data.DataModel.IGortContext) =
        try
            Gort.Data.Utils.MetaDataUtils.GetAllCausesForWorkspace(wsName, ctxt)
             |> Ok
        with
            | ex -> ("error in GetAllCausesForWorkspace: " + ex.Message ) |> Result.Error


    let GetCauseById (causeId:Guid) (ctxt:Gort.Data.DataModel.IGortContext) =
        try
            Gort.Data.Utils.MetaDataUtils.GetCauseById(causeId, ctxt)
             |> Ok
        with
            | ex -> ("error in GetCauseById: " + ex.Message ) |> Result.Error


    let GetCauseTypeGroupAncestry (cauz:Gort.Data.DataModel.Cause) (ctxt:Gort.Data.DataModel.IGortContext) =
        try
            Gort.Data.Utils.MetaDataUtils.GetCauseTypeGroupAncestry(cauz, ctxt)
             |> Ok
        with
            | ex -> ("error in GetCauseTypeGroupAncestry: " + ex.Message ) |> Result.Error


    let GetParamType (cauz:Gort.Data.DataModel.ParamTypeName) (ctxt:Gort.Data.DataModel.IGortContext) =
        try
            Gort.Data.Utils.MetaDataUtils.GetParamType(cauz, ctxt)
             |> Ok
        with
            | ex -> ("error in GetParamType: " + ex.Message ) |> Result.Error


    let GetCauseDispatcherInfo (causeId:Guid) (ctxt:Gort.Data.DataModel.IGortContext) =
        result {
            let! cause = GetCauseById causeId ctxt
            let! cestry = GetCauseTypeGroupAncestry cause ctxt
            let pth = cestry |> Array.map(fun ctg -> ctg.Name)
            return (cause, pth);
        }