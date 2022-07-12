// For more information see https://aka.ms/fsharp-console-apps
open System
open Gort.Data.DataModel;
open Gort.Data.Instance;
open Gort.Data.Instance.CauseBuilder;
open Gort.Data.Instance.SeedParams;
open WorkspaceBuilder

[<EntryPoint>]
let main argv =
    let ctx = new GortContext()
    let res = LoadRun ctx "WksRnd" 
    let yab = res |> Result.ExtractOrThrow

    printfn "%s %A" "Hi Pongki" res
    //Console.Read() |> ignore
    0
