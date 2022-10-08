namespace global

open System
open System.Text
open Gort.Data.Utils
open Gort.Data.DataModel

module paramConv =

    let MutationTypeFromParam (pram: Param) =
        result {
            if (pram.ParamType.Name <> ParamTypeName.MutationType.ToString()) then
                let errMsg =
                    sprintf "%s is not %s" pram.ParamType.Name (ParamTypeName.MutationType.ToString())

                return! errMsg |> Error
            else
                let! rtv = pram.Value |> byteConv.intFromBytes
                return rtv |> enum<MutationType>
        }


    let NumberFormatFromParam (pram: Param) =
        result {
            if (pram.ParamType.Name <> ParamTypeName.NumberFormat.ToString()) then
                let errMsg =
                    sprintf "%s is not %s" pram.ParamType.Name (ParamTypeName.NumberFormat.ToString())

                return! errMsg |> Error
            else
                let! rtv = pram.Value |> byteConv.intFromBytes
                return rtv |> enum<NumberFormat>
        }


    let RandGenTypeFromParam (pram: Param) =
        result {
            if (pram.ParamType.Name <> ParamTypeName.RandGenType.ToString()) then
                let errMsg =
                    sprintf "%s is not %s" pram.ParamType.Name (ParamTypeName.RandGenType.ToString())

                return! errMsg |> Error
            else
                let! rtv = pram.Value |> byteConv.intFromBytes
                return rtv |> enum<RandGenType>
        }


    let SortableFormatFromParam (pram: Param) =
        result {
            if (pram.ParamType.Name <> ParamTypeName.SortableFormat.ToString()) then
                let errMsg =
                    sprintf "%s is not %s" pram.ParamType.Name (ParamTypeName.SortableFormat.ToString())

                return! errMsg |> Error
            else
                let! rtv = pram.Value |> byteConv.intFromBytes
                return rtv |> enum<SortableFormat>
        }


    let SortableSetRepFromParam (pram: Param) =
        result {
            if (pram.ParamType.Name <> ParamTypeName.SortableSetRep.ToString()) then
                let errMsg =
                    sprintf "%s is not %s" pram.ParamType.Name (ParamTypeName.SortableSetRep.ToString())

                return! errMsg |> Error
            else
                let! rtv = pram.Value |> byteConv.intFromBytes
                return rtv |> enum<SortableSetRep>
        }



    let SorterSaveModeFromParam (pram: Param) =
        result {
            if (pram.ParamType.Name <> ParamTypeName.SorterSaveMode.ToString()) then
                let errMsg =
                    sprintf "%s is not %s" pram.ParamType.Name (ParamTypeName.SorterSaveMode.ToString())

                return! errMsg |> Error
            else
                let! rtv = pram.Value |> byteConv.intFromBytes
                return rtv |> enum<SorterSaveMode>
        }



    let RandoFromRandGenRecordParam (ctxt: Gort.Data.DataModel.IGortContext) (pram: Param) =
        result {
            if (pram.ParamType.Name <> ParamTypeName.RandGenId.ToString()) then
                let errMsg =
                    sprintf "%s is not %s" pram.ParamType.Name (ParamTypeName.RandGenId.ToString())

                return! errMsg |> Error
            else
                let! rngId = pram.Value |> byteConv.guidFromBytes
                let! rngGen = gcOps.MakeRngGenFromRecord rngId ctxt
                return rngGen |> Rando.fromRngGen
        }
