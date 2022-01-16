open System
open BenchmarkDotNet.Running

[<EntryPoint>]
let main argv =

  //  BatchRuns.Proc BatchRuns.SayHi

    let summary = BenchmarkRunner.Run<BenchRollout>()
    printfn "%A" summary
    Console.Read() |> ignore
    0
