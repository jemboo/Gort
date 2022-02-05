namespace global
open BenchmarkDotNet.Attributes


[<MemoryDiagnoser>]
type BenchMakeSorableStack() =
    let degree = Degree.create 16 |> Result.ExtractOrThrow
    let degrees = [| Degree.create 8; Degree.create 4; Degree.create 2; Degree.create 2 |]
                  |> Array.toList
                  |> Result.sequence
                  |> Result.ExtractOrThrow
                  |> List.toArray
    let offset = 7


    [<Benchmark>]
    member this.makeStack() =
        let mutable byteStore = Array.zeroCreate<byte> (offset + 8)
        byteStore <- ByteArray.degreeArrayToBytes byteStore offset degrees
                     |> Result.ExtractOrThrow
        let degreeSection = byteStore.[offset ..]
        let res = SortableSet.makeStackInts degree degreeSection
                  |> Result.ExtractOrThrow
        ()


    [<Benchmark>]
    member this.makeStack8() =
        let mutable byteStore = Array.zeroCreate<byte> (offset + 8)
        byteStore <- ByteArray.degreeArrayToBytes byteStore offset degrees
                     |> Result.ExtractOrThrow
        let degreeSection = byteStore.[offset ..]
        let res = SortableSet.makeStackInts8 degree degreeSection
                  |> Result.ExtractOrThrow
        0