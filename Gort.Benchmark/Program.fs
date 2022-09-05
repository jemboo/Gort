open System
open BenchmarkDotNet.Running

[<EntryPoint>]
let main argv =

    let summary = BenchmarkRunner.Run<BenchmarkSorterOnBp64>()
    printfn "%A" summary
    Console.Read() |> ignore
    0
