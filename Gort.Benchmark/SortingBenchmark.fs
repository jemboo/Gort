namespace global
open BenchmarkDotNet.Attributes


[<MemoryDiagnoser>]
type BenchMakeSorableStack() =
    let order = Order.create 16 |> Result.ExtractOrThrow
    let orders = [| Order.create 8; Order.create 4; Order.create 2; Order.create 2 |]
                  |> Array.toList
                  |> Result.sequence
                  |> Result.ExtractOrThrow
                  |> List.toArray
    let offset = 7


    //[<Benchmark>]
    //member this.makeStack() =
    //    let mutable byteStore = Array.zeroCreate<byte> (offset + 8)
    //    byteStore <- ByteArray.orderArrayToBytes byteStore offset orders
    //                 |> Result.ExtractOrThrow
    //    let orderSection = byteStore.[offset ..]
    //    let res = SortableSet.makeStackInts order orderSection
    //              |> Result.ExtractOrThrow
    //    ()


    //[<Benchmark>]
    //member this.makeStack8() =
    //    let mutable byteStore = Array.zeroCreate<byte> (offset + 8)
    //    byteStore <- ByteArray.orderArrayToBytes byteStore offset orders
    //                 |> Result.ExtractOrThrow
    //    let orderSection = byteStore.[offset ..]
    //    let res = SortableSet.makeStackInts8 order orderSection
    //              |> Result.ExtractOrThrow
    //    0