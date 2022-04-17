
namespace global
open Gort.Data


module Utils =

    let Save (ctxt:IGortContext) = 
        try
            ctxt.SaveChanges() |> Ok
        with
            | ex -> ("error in Utils.Save: " + ex.Message ) |> Result.Error


    let AddIntParam (ctxt:IGortContext) (ptn:ParamTypeName) (value:int) = 
        try
            let pt = MetaDataUtils.GetParamType(ptn, ctxt)

            None |> Ok //ctxt.PamType.
        with
            | ex -> ("error in Utils.GetParamType: " + ex.Message ) |> Result.Error