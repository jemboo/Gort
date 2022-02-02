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


//[<MemoryDiagnoser>]
//type BenchProd() =
//    let aCore = [|0 .. 31|]
//    let aConj = [|0 .. 31|]
//    let aOut = Array.zeroCreate<int> 32

//    let aCore16 = [|0us .. 31us|]
//    let aConj16 = [|0us .. 31us|]
//    let aOut16 = Array.zeroCreate<uint16> 32

//    [<Benchmark>]
//    member this.inverseMapArray() =
//        let ssR = Comby.intArrayProduct aCore (Array.zeroCreate aConj.Length)
                    
//        0

//    [<Benchmark>]
//    member this.intArrayProduct() =
//        let ssR = Comby.intArrayProduct aConj aCore (Array.zeroCreate aConj.Length)
//        0



//|               Method |      Mean |    Error |   StdDev |  Gen 0 | Allocated |
//|--------------------- |----------:|---------:|---------:|-------:|----------:|
//|          invertArray |  47.11 ns | 0.736 ns | 0.756 ns | 0.0705 |     304 B |
//|      arrayProductInt |  32.80 ns | 0.576 ns | 0.539 ns | 0.0352 |     152 B |
//| conjIntArraysNoAlloc |  20.58 ns | 0.322 ns | 0.301 ns |      - |         - |
//|       arrayProduct16 |  28.50 ns | 0.049 ns | 0.038 ns |      - |         - |
//|        arrayProduct8 |  28.95 ns | 0.496 ns | 0.464 ns |      - |         - |
//|     arrayProductIntR |  26.43 ns | 0.335 ns | 0.313 ns |      - |         - |
//|        conjIntArrays |  53.66 ns | 0.814 ns | 0.761 ns | 0.0352 |     152 B |
//|       conjIntArraysR | 324.93 ns | 6.525 ns | 6.103 ns | 0.1554 |     672 B |

[<MemoryDiagnoser>]
type BenchConj() =
    let aCore = [|0 .. 31|]
    let aConj = [|0 .. 31|]
    let aOut = Array.zeroCreate<int> 32

    let aCore16 = [|0us .. 31us|]
    let aConj16 = [|0us .. 31us|]
    let aOut16 = Array.zeroCreate<uint16> 32

    let aCore8 = [|0uy .. 31uy|]
    let aConj8 = [|0uy .. 31uy|]
    let aOut8 = Array.zeroCreate<uint8> 32


    [<Benchmark>]
    member this.invertArray() =
        let ssR = Comby.invertArray aCore (Array.zeroCreate aConj.Length)
                    |> Result.ExtractOrThrow
        0


    [<Benchmark>]
    member this.arrayProductInt() =
        let ssR = Comby.arrayProductInt aConj aCore (Array.zeroCreate aConj.Length)
        0


    [<Benchmark>]
    member this.conjIntArraysNoAlloc() =
        let ssR = Comby.arrayProductInt aConj aCore aOut
        0

    [<Benchmark>]
    member this.arrayProduct16() =
        let ssR = Comby.arrayProduct16 aConj16 aCore16 aOut16 //(Array.zeroCreate<uint16> aConj.Length)
        0


    [<Benchmark>]
    member this.arrayProduct8() =
        let ssR = Comby.arrayProduct8 aConj8 aCore8 aOut8 //(Array.zeroCreate<uint8> aConj.Length)
        0


    [<Benchmark>]
    member this.arrayProductIntR() =
        let ssR = Comby.arrayProductIntR aConj aCore aOut
                    |> Result.ExtractOrThrow
        0


    [<Benchmark>]
    member this.conjIntArrays() =
        let ssR = Comby.conjIntArrays aConj aCore 
                    |> Result.ExtractOrThrow
        0


    [<Benchmark>]
    member this.conjIntArraysR() =
        let ssR = Comby.conjIntArraysR aConj aCore
                    |> Result.ExtractOrThrow
        0




[<MemoryDiagnoser>]
type BenchRollout() =
    let deg = Degree.createNr 20
    let bpa = Bitwise.allBitPackForDegree deg |> Result.ExtractOrThrow
    
    [<Benchmark>]
    member this.toBitStriped() =
        let bsa = bpa |> Bitwise.bitPackedtoBitStriped deg |> Result.ExtractOrThrow
        0

    [<Benchmark>]
    member this.toBitStriped2D() =
        let bsa = bpa |> Bitwise.bitPackedtoBitStriped2D deg |> Result.ExtractOrThrow
        0


[<MemoryDiagnoser>]
type PermutationBench() =

    let permA = Permutation.create [|0;1;2;3;4;5;6;7;8;9;10;11;12;13;14;15;16;17;18;19;20;21;22;23;24;25;26;27;28;29;30;31;|] |> Result.ExtractOrThrow
    let permB = Permutation.create [|10;9;2;3;4;5;6;7;8;1;0;11;12;13;14;15;16;17;18;19;20;21;22;23;24;25;26;27;28;29;30;31;|] |> Result.ExtractOrThrow

    [<Benchmark>]
    member this.product() =
        let bsa = Permutation.product permA permB |> Result.ExtractOrThrow
        0

    [<Benchmark>]
    member this.productNr() =
        let bsa = Permutation.productNr permA permB
        0

        
//|  Method |      Mean |     Error |    StdDev |    Gen 0 |    Gen 1 |    Gen 2 | Allocated |
//|-------- |----------:|----------:|----------:|---------:|---------:|---------:|----------:|
//|  Alloc8 |  6.919 us | 0.1323 us | 0.1238 us |  31.2424 |  31.2424 |  31.2424 |     98 KB |
//| Alloc16 | 10.918 us | 0.2178 us | 0.2674 us |  62.4847 |  62.4847 |  62.4847 |    195 KB |
//| Alloc32 | 22.582 us | 0.4483 us | 1.1491 us | 124.9695 | 124.9695 | 124.9695 |    391 KB |
//| Alloc64 | 59.300 us | 1.1651 us | 2.0098 us | 249.9390 | 249.9390 | 249.9390 |    781 KB |
[<MemoryDiagnoser>]
type ArrayAllocBench() =

    [<Benchmark>]
    member this.Alloc8() =
        let bsa = Array.zeroCreate<uint8> 100000
        0

    [<Benchmark>]
    member this.Alloc16() =
        let bsa = Array.zeroCreate<uint16> 100000
        0

    [<Benchmark>]
    member this.Alloc32() =
        let bsa = Array.zeroCreate<uint32> 100000
        0


    [<Benchmark>]
    member this.Alloc64() =
        let bsa = Array.zeroCreate<uint64> 100000
        0



//| Method |      Mean |    Error |   StdDev |    Gen 0 |    Gen 1 |    Gen 2 | Allocated |
//|------- |----------:|---------:|---------:|---------:|---------:|---------:|----------:|
//|  Init8 |  89.95 us | 1.050 us | 0.982 us |  31.1279 |  31.1279 |  31.1279 |     98 KB |
//| Init16 | 134.67 us | 2.679 us | 3.755 us |  62.2559 |  62.2559 |  62.2559 |    195 KB |
//| Init32 | 225.47 us | 3.467 us | 3.243 us | 124.7559 | 124.7559 | 124.7559 |    391 KB |
//| Init64 | 423.21 us | 4.887 us | 4.332 us | 249.5117 | 249.5117 | 249.5117 |    781 KB |

[<MemoryDiagnoser>]
type ArrayInitBench() =

    [<Benchmark>]
    member this.Init8() =
        let bsa = Array.init<uint8> 100000 (id >> uint8)
        0

    [<Benchmark>]
    member this.Init16() =
        let bsa = Array.init<uint16> 100000 (id >> uint16)
        0

    [<Benchmark>]
    member this.Init32() =
        let bsa = Array.init<uint32> 100000 (id >> uint32)
        0


    [<Benchmark>]
    member this.Init64() =
        let bsa = Array.init<uint64> 100000 (id >> uint64)
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