namespace global
open System
open System.Text
open Gort.Data.Utils
open Gort.Data.DataModel

module miscConv =

    let RndGenTypeFromInt (ival:int) =
        try
            let rgt = MetaDataUtils.RndGenTypeFromInt ival
            rgt |> Ok
        with
            | ex -> ("error in RndGenTypeFromInt: " + ex.Message ) |> Error 


    let RndGenTypeToRngType (tVal:RndGenType) =
        try
            match tVal with
            | RndGenType.Lcg -> rngType.Lcg |> Ok
            | RndGenType.Clr -> rngType.Net |> Ok
            | _ -> "RndGenType not handled" |> Error
        with
            | ex -> ("error in RndGenTypeToRngType: " + ex.Message ) |> Error


    let RngTypeToRandGenType (tVal:rngType) =
        try
            match tVal with
            | rngType.Lcg -> RndGenType.Lcg |> Ok
            | rngType.Net -> RndGenType.Clr |> Ok
            | _ -> "rngType not handled" |> Error
        with
            | ex -> ("error in RngTypeToRandGenType: " + ex.Message ) |> Error 