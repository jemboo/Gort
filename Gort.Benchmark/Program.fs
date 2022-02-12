open System
open BenchmarkDotNet.Running

[<EntryPoint>]
let main argv =

    let summary = BenchmarkRunner.Run<ArrayFormatBench>()
    printfn "%A" summary
    Console.Read() |> ignore
    0
