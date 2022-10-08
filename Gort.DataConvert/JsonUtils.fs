namespace global

open Microsoft.FSharp.Core
open System.IO
open Newtonsoft.Json
open System
open Gort.DataStore.DataModel
open Gort.DataStore.CauseBuild

module Json =

    type Marker =
        interface
        end

    let serialize obj = JsonConvert.SerializeObject obj

    let deserialize<'a> str : Result<'a, string> =
        try
            JsonConvert.DeserializeObject<'a> str |> Ok
        with ex ->
            Result.Error ex.Message

    let deserializeOption<'a> str =
        match str with
        | Some s -> (deserialize<'a> s)
        | None -> Result.Error "option was none"
