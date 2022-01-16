namespace global
open BenchmarkDotNet.Attributes
open System.Security.Cryptography
open System


//|          Method |      Mean |     Error |    StdDev | Allocated |
//|---------------- |----------:|----------:|----------:|----------:|
//|     isSorted_uL | 1.3540 ns | 0.0358 ns | 0.0318 ns |         - |
//| isSorted_inline | 0.2851 ns | 0.0346 ns | 0.0485 ns |         - |
//|      isSorted_c | 1.4347 ns | 0.0384 ns | 0.0340 ns |         - |



[<MemoryDiagnoser>]
type BenchIsSorted_Arrays() =
    [<Params(20, 100, 1000, 10000)>]
    member val size = 0 with get, set
    member val testArray = [||] with get, set

    [<GlobalSetup>]
    member this.Setup() =
        this.testArray = Array.init this.size (fun dex -> (uint64 dex)) |> ignore
        ()


    [<Benchmark>]
    member this.isSorted_idiom() =
        let ssR = Collections.isSorted_idiom this.testArray
        0

    
    [<Benchmark>]
    member this.isSorted_uL() =
        let ssR = Collections.isSorted_uL this.testArray
        0


    [<Benchmark>]
    member this.isSorted_inline() =
        let ssR = Collections.isSorted_inline this.testArray
        0


    [<Benchmark>]
    member this.isSorted() =
        let ssR = Collections.isSorted this.testArray
        0


//|           Method |  size |     Mean |    Error |   StdDev |  Gen 0 | Allocated |
//|----------------- |------ |---------:|---------:|---------:|-------:|----------:|
//| filterByPickList |    20 | 10.81 ns | 0.242 ns | 0.484 ns | 0.0056 |      24 B |
//| filterByPickList |   100 | 11.01 ns | 0.250 ns | 0.234 ns | 0.0056 |      24 B |
//| filterByPickList |  1000 | 10.09 ns | 0.181 ns | 0.169 ns | 0.0056 |      24 B |
//| filterByPickList | 10000 | 10.11 ns | 0.160 ns | 0.150 ns | 0.0056 |      24 B |


//|           Method |     Mean |     Error |    StdDev |  Gen 0 | Allocated |
//|----------------- |---------:|----------:|----------:|-------:|----------:|
//| filterByPickList | 1.625 us | 0.0314 us | 0.0374 us | 0.9327 |      4 KB |
//|           Method |     Mean |    Error |   StdDev |  Gen 0 | Allocated |
//|----------------- |---------:|---------:|---------:|-------:|----------:|
//| filterByPickList | 15.92 us | 0.300 us | 0.333 us | 9.1553 |     39 KB |
[<MemoryDiagnoser>]
type BenchIsSorted_filterByPickList () =
   // [<Params(20, 100, 1000, 10000)>]
   // member val size = 100000 with get, set
    let testArray = Array.init 100000 (fun dex -> (uint64 dex))
    let pickArray = Array.init 100000 (fun dex -> dex % 2 = 0)
    //member val testArray = [||] with get, set
    //member val pickArray = [||] with get, set

    //[<GlobalSetup>]
    //member this.Setup() =
    //    this.testArray = Array.init this.size (fun dex -> (uint64 dex)) |> ignore
    //    this.pickArray = Array.init this.size (fun dex -> dex % 2 = 0) |> ignore
    //    ()

    [<Benchmark>]
    member this.filterByPickList() =
        let ssR = Bitwise.filterByPickList testArray pickArray
                  |> Result.ExtractOrThrow
        0

    [<Benchmark>]
    member this.filterByPickList2() =
        let ssR = Bitwise.filterByPickList testArray pickArray
                  |> Result.ExtractOrThrow
        0



// degree 32
//|          Method |      Mean |    Error |   StdDev |    Median |  Gen 0 | Allocated |
//|---------------- |----------:|---------:|---------:|----------:|-------:|----------:|
//| inverseMapArray |  36.70 ns | 0.768 ns | 1.346 ns |  36.43 ns | 0.0352 |     152 B |
//|   compIntArrays |  36.37 ns | 0.749 ns | 1.209 ns |  35.75 ns | 0.0352 |     152 B |
//|   conjIntArrays |  53.81 ns | 0.382 ns | 0.299 ns |  53.80 ns | 0.0352 |     152 B |
//|  conjIntArraysR | 299.90 ns | 5.137 ns | 4.805 ns | 297.87 ns | 0.1183 |     512 B |
//| compIntArraysNr |  19.39 ns | 0.298 ns | 0.278 ns |  19.23 ns |      - |         - |
//| conjIntArraysNr |  26.14 ns | 0.291 ns | 0.272 ns |  26.01 ns |      - |         - |


[<MemoryDiagnoser>]
type BenchConj() =
    let degree = Degree.create 16
    //let aCore = [|0; 1; 2; 3; 4; 5; 6; 7; 8; 9; 10; 11; 12; 13; 14; 15;|]
    //let aConj = [|0; 1; 2; 3; 4; 5; 6; 7; 8; 9; 10; 11; 12; 13; 14; 15;|]
    let aCore = [|0; 1; 2; 3; 4; 5; 6; 7; 8; 9; 10; 11; 12; 13; 14; 15; 16; 17; 18; 19; 20; 21; 22; 23; 24; 25; 26; 27; 28; 29; 30; 31|]
    let aConj = [|0; 1; 2; 3; 4; 5; 6; 7; 8; 9; 10; 11; 12; 13; 14; 15; 16; 17; 18; 19; 20; 21; 22; 23; 24; 25; 26; 27; 28; 29; 30; 31|]
    let aOut = Array.zeroCreate<int> 32

    [<Benchmark>]
    member this.inverseMapArray() =
        let ssR = Comby.inverseMapArray aCore
                    |> Result.ExtractOrThrow
        0

    [<Benchmark>]
    member this.compIntArrays() =
        let ssR = Comby.compIntArrays aConj aCore
        0

    [<Benchmark>]
    member this.conjIntArrays() =
        let ssR = Comby.conjIntArrays aConj aCore
        0

    [<Benchmark>]
    member this.conjIntArraysR() =
        let ssR = Comby.conjIntArraysR aConj aCore
                    |> Result.ExtractOrThrow
        0

    [<Benchmark>]
    member this.compIntArraysNr() =
        let ssR = Comby.compIntArraysNr aConj aCore aOut
        0

    [<Benchmark>]
    member this.conjIntArraysNr() =
        let ssR = Comby.conjIntArraysNr aConj aCore aOut
        0



[<MemoryDiagnoser>]
type BenchRollout() =
    let deg = Degree.create 16
    let bpa = Bitwise.allBitPackForDegree deg |> Result.ExtractOrThrow
    
    [<Benchmark>]
    member this.toBitStriped() =
        let bsa = bpa |> Bitwise.bitPackedtoBitStriped deg |> Result.ExtractOrThrow
        0

    [<Benchmark>]
    member this.roundTrip() =
        let bsa = bpa |> Bitwise.bitPackedtoBitStriped deg |> Result.ExtractOrThrow
        let bpaBack = bsa |> Bitwise.bitStripedToBitPacked deg bpa.Length |> Result.ExtractOrThrow
        0



[<MemoryDiagnoser>]
type Md5VsSha256() =
    let N = 100000
    let data = Array.zeroCreate N
    let sha256 = SHA256.Create();
    let md5 = MD5.Create()

    member this.GetData =
        data

    [<Benchmark(Baseline = true)>]
    member this.Sha256() =
        sha256.ComputeHash(data)

    [<Benchmark>]
    member this.Md5() =
        md5.ComputeHash(data)