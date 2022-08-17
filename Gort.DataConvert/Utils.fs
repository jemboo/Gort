namespace global
open Microsoft.FSharp.Core
open System.IO
open Newtonsoft.Json
open System
open Gort.DataStore.DataModel
open Gort.DataStore.CauseBuild

module Json =

    type Marker = interface end
        
    let serialize obj = JsonConvert.SerializeObject obj
        
    let deserialize<'a> str :Result<'a, string> =
        try
            JsonConvert.DeserializeObject<'a> str |> Ok
        with
        | ex -> Result.Error ex.Message
        
    let deserializeOption<'a> str =
        match str with
        | Some s -> (deserialize<'a> s)
        | None -> Result.Error  "option was none"



module DbLookup = 

    let GetBitPackRById (gortCtxt:IGortContext2) (bitPackRId:int)  = 
        try
            DbQueryUtils.GetBitPackRById(bitPackRId, gortCtxt) |> Ok
        with
            | ex -> ("error in GetBitPackById: " + ex.Message ) |> Result.Error


    let GetCauseById (gortCtxt:IGortContext2) (causeId:int) = 
        try
            DbQueryUtils.GetCauseById(causeId, gortCtxt) |> Ok
        with
            | ex -> ("error in GetCauseById: " + ex.Message ) |> Result.Error


    let GetRandGenRById (gortCtxt:IGortContext2) (rndGenId:int) = 
        try
            DbQueryUtils.GetRandGenRById(rndGenId, gortCtxt) |> Ok
        with
            | ex -> ("error in GetRandGenRById: " + ex.Message ) |> Result.Error

    let GetSortableSetRById (gortCtxt:IGortContext2) (sortableSetRId:int) = 
        try
            DbQueryUtils.GetSortableSetRById(sortableSetRId, gortCtxt) |> Ok
        with
            | ex -> ("error in GetRandGenRById: " + ex.Message ) |> Result.Error 