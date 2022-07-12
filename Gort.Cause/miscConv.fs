namespace global
open System
open System.Text
open Gort.Data.Utils
open Gort.Data.DataModel

module miscConv =

    let RndGenTypeFromInt (ival:int) =
        try
            let rgt = DataTypeExt.RandGenTypeFromInt ival
            rgt |> Ok
        with
            | ex -> ("error in RndGenTypeFromInt: " + ex.Message ) |> Error 


    let RndGenTypeToRngType (tVal:RandGenType) =
        try
            match tVal with
            | RandGenType.Lcg -> rngType.Lcg |> Ok
            | RandGenType.Clr -> rngType.Net |> Ok
            | _ -> "RndGenType not handled" |> Error
        with
            | ex -> ("error in RndGenTypeToRngType: " + ex.Message ) |> Error


    let RngTypeToRandGenType (tVal:rngType) =
        try
            match tVal with
            | rngType.Lcg -> RandGenType.Lcg |> Ok
            | rngType.Net -> RandGenType.Clr |> Ok
            | _ -> "rngType not handled" |> Error
        with
            | ex -> ("error in RngTypeToRandGenType: " + ex.Message ) |> Error 