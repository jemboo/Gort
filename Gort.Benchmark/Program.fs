open System
open BenchmarkDotNet.Running

[<EntryPoint>]
let main argv =
    let summary = BenchmarkRunner.Run<ArrayCompBench>()
    let cls = new BenchmarkSorterOnBp64()
    //let summary = cls.applySorterAndMakeSwitchTrack_getUnsortedCt_RfBs64()
    printfn "%A" summary
    Console.Read() |> ignore
    0
