namespace global
open Gort.DataStore.DataModel
open Gort.DataStore.CauseBuild

module Load =

    let LoadCauseBuilder (czBuilder:CauseBuildBase)
                         (ctxt:IGortContext2) =

        let _addWorkspace (ws:Workspace) =
            let wsi = ctxt.Workspace.Find(ws.WorkspaceId);
            if (wsi = null) then
                ctxt.Workspace.Add(ws) |> ignore

        let _addParam (pram:Param) =
            let prami = ctxt.Param.Find(pram.ParamId);
            if (prami = null) then
                ctxt.Param.Add(pram) |> ignore

        let _addCause (cz:Cause) =
            let czi = ctxt.Cause.Find(cz.CauseId);
            if (czi = null) then
                ctxt.Cause.Add(cz) |> ignore

        let _addCauseParam (czP:CauseParam) =
            let czPi = ctxt.CauseParam.Find(czP.ParamId);
            if (czPi = null) then
                ctxt.CauseParam.Add(czP) |> ignore
        try
            _addWorkspace czBuilder.Workspace
            _addCause czBuilder.Cause
            czBuilder.Params |> Seq.iter(_addParam)
            czBuilder.CauseParams |> Seq.iter(_addCauseParam)
            ctxt.SaveChanges() |> Ok
        with
            | ex -> ("error in bytesFromIntArray: " + ex.Message ) |> Result.Error 