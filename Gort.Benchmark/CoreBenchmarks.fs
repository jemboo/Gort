namespace global

open BenchmarkDotNet.Attributes
open System.Security.Cryptography
open System


//|          Method |   size |         Mean |        Error |       StdDev | Allocated |
//|---------------- |------- |-------------:|-------------:|-------------:|----------:|
//| arrayProductInt |     10 |     13.53 ns |     0.143 ns |     0.134 ns |         - |
//|    arrayProduct |     10 |     12.79 ns |     0.287 ns |     0.269 ns |         - |
//| arrayProductInt |    100 |     89.23 ns |     1.315 ns |     1.098 ns |         - |
//|    arrayProduct |    100 |     93.98 ns |     0.660 ns |     0.585 ns |         - |
//| arrayProductInt |   1000 |    760.74 ns |     8.464 ns |     7.917 ns |         - |
//|    arrayProduct |   1000 |    768.17 ns |     7.140 ns |     6.679 ns |         - |
//| arrayProductInt |  10000 |  7,553.64 ns |    57.999 ns |    51.414 ns |         - |
//|    arrayProduct |  10000 |  7,713.10 ns |   151.992 ns |   162.630 ns |         - |
//| arrayProductInt | 100000 | 75,923.11 ns | 1,458.919 ns | 1,996.985 ns |         - |
//|    arrayProduct | 100000 | 77,145.02 ns |   619.630 ns |   549.286 ns |         - |

[<MemoryDiagnoser>]
type ArrayCompBench() =

    [<Params(10, 100, 1000, 10000, 100000)>]
    member val size = 0 with get, set

    member val arrayA = [||] with get, set
    member val arrayB = [||] with get, set
    member val arrayC = [||] with get, set

    [<GlobalSetup>]
    member this.Setup() =
        this.arrayA <- Array.init this.size (fun dex -> (int dex))
        this.arrayB <- Array.init this.size (fun dex -> (int dex))
        this.arrayC <- Array.zeroCreate this.size


    //[<Benchmark>]
    //member this.arrayProductInt() =
    //    CollectionOps.arrayProductInt this.arrayA this.arrayB this.arrayC
    //    |> ignore


    [<Benchmark>]
    member this.arrayProduct() =
        CollectionOps.arrayProduct this.arrayA this.arrayB this.arrayC
        |> ignore



//|             Method |   size |          Mean |        Error |       StdDev |        Median |   Gen0 | Allocated |
//|------------------- |------- |--------------:|-------------:|-------------:|--------------:|-------:|----------:|
//|   distanceSquared2 |     10 |      12.12 ns |     0.166 ns |     0.155 ns |      12.10 ns |      - |         - |
//|    distanceSquared |     10 |      35.45 ns |     0.750 ns |     1.100 ns |      35.12 ns |      - |         - |
//|   distanceSquaredG |     10 |      11.02 ns |     0.077 ns |     0.068 ns |      11.04 ns |      - |         - |
//|  distanceSquaredGr |     10 |      15.50 ns |     0.221 ns |     0.207 ns |      15.60 ns |      - |         - |
//| distanceSquaredGrE |     10 |  15,708.25 ns |   298.195 ns |   278.932 ns |  15,694.75 ns | 0.0916 |     360 B |
//|   distanceSquared2 |    100 |     106.32 ns |     0.961 ns |     0.852 ns |     106.41 ns |      - |         - |
//|    distanceSquared |    100 |     244.24 ns |     2.884 ns |     2.557 ns |     244.57 ns |      - |         - |
//|   distanceSquaredG |    100 |     105.91 ns |     1.188 ns |     1.112 ns |     105.85 ns |      - |         - |
//|  distanceSquaredGr |    100 |     118.51 ns |     2.291 ns |     4.883 ns |     116.34 ns |      - |         - |
//| distanceSquaredGrE |    100 |  15,720.33 ns |   296.410 ns |   291.114 ns |  15,663.80 ns | 0.0916 |     360 B |
//|   distanceSquared2 |   1000 |     993.89 ns |    17.105 ns |    16.000 ns |     993.43 ns |      - |         - |
//|    distanceSquared |   1000 |   2,287.24 ns |    32.913 ns |    30.786 ns |   2,277.15 ns |      - |         - |
//|   distanceSquaredG |   1000 |     998.87 ns |    18.928 ns |    18.590 ns |     999.72 ns |      - |         - |
//|  distanceSquaredGr |   1000 |   1,077.77 ns |    20.182 ns |    22.432 ns |   1,074.35 ns |      - |         - |
//| distanceSquaredGrE |   1000 |  15,985.53 ns |   312.472 ns |   395.177 ns |  15,902.98 ns | 0.0916 |     360 B |
//|   distanceSquared2 |  10000 |  10,371.71 ns |   201.113 ns |   239.410 ns |  10,399.28 ns |      - |         - |
//|    distanceSquared |  10000 |  23,292.27 ns |   461.538 ns |   783.726 ns |  23,435.47 ns |      - |         - |
//|   distanceSquaredG |  10000 |  10,398.97 ns |   198.191 ns |   212.062 ns |  10,326.56 ns |      - |         - |
//|  distanceSquaredGr |  10000 |  10,997.44 ns |   198.680 ns |   165.907 ns |  11,010.31 ns |      - |         - |
//| distanceSquaredGrE |  10000 |  21,790.33 ns |   432.347 ns |   768.496 ns |  21,677.27 ns | 0.0916 |     360 B |
//|   distanceSquared2 | 100000 | 103,372.30 ns | 2,018.269 ns | 1,887.890 ns | 102,874.54 ns |      - |         - |
//|    distanceSquared | 100000 | 232,603.09 ns | 2,531.334 ns | 2,367.811 ns | 232,626.44 ns |      - |         - |
//|   distanceSquaredG | 100000 | 105,503.33 ns | 1,991.755 ns | 2,045.386 ns | 105,416.39 ns |      - |         - |
//|  distanceSquaredGr | 100000 | 110,162.25 ns | 1,850.448 ns | 2,056.768 ns | 110,025.69 ns |      - |         - |
//| distanceSquaredGrE | 100000 |  74,512.79 ns | 1,486.298 ns | 2,400.094 ns |  74,300.93 ns |      - |     360 B |


[<MemoryDiagnoser>]
type DistanceTest() =

    [<Params(10, 100, 1000, 10000, 100000)>]
    member val size = 0 with get, set

    member val arrayA = [||] with get, set
    member val arrayB = [||] with get, set
    member val arrayC = [||] with get, set

    [<GlobalSetup>]
    member this.Setup() =
        this.arrayA <- Array.init this.size (fun dex -> (int dex))
        this.arrayB <- Array.init this.size (fun dex -> (int dex) + 1)
        this.arrayC <- Array.init (this.size / 2) (fun dex -> (int dex) + 1)

    [<Benchmark>]
    member this.distanceSquared2() =
        let yow = CollectionProps.distanceSquared_int this.arrayA this.arrayB
        yow

    [<Benchmark>]
    member this.distanceSquared() =
        let yow = CollectionProps.distanceSquared_idiom this.arrayA this.arrayB
        yow

    [<Benchmark>]
    member this.distanceSquaredG() =
        let yow = CollectionProps.distanceSquared_generic this.arrayA this.arrayB
        yow

    [<Benchmark>]
    member this.distanceSquaredGr() =
        let yow = CollectionProps.distanceSquared_genericR this.arrayA this.arrayB
        yow

    [<Benchmark>]
    member this.distanceSquaredGrE() =
        let yow = CollectionProps.distanceSquared_genericR this.arrayA this.arrayC
        yow


//|              Method |   size |          Mean |        Error |       StdDev |        Median | Allocated |
//|-------------------- |------- |--------------:|-------------:|-------------:|--------------:|----------:|
//| distanceSquaredG_us |     10 |      12.67 ns |     0.160 ns |     0.150 ns |      12.65 ns |         - |
//| distanceSquaredG_uL |     10 |      10.12 ns |     0.166 ns |     0.139 ns |      10.08 ns |         - |
//|    distanceSquaredG |     10 |      10.14 ns |     0.216 ns |     0.202 ns |      10.15 ns |         - |
//| distanceSquaredG_us |   1000 |   1,302.51 ns |    17.012 ns |    15.081 ns |   1,297.59 ns |         - |
//| distanceSquaredG_uL |   1000 |     863.71 ns |    16.691 ns |    37.674 ns |     846.45 ns |         - |
//|    distanceSquaredG |   1000 |     858.35 ns |    17.095 ns |    39.279 ns |     846.02 ns |         - |
//| distanceSquaredG_us | 100000 | 126,937.30 ns | 2,382.214 ns | 3,708.821 ns | 125,911.99 ns |         - |
//| distanceSquaredG_uL | 100000 |  90,166.51 ns | 1,024.398 ns |   958.223 ns |  90,200.95 ns |         - |
//|    distanceSquaredG | 100000 |  87,386.45 ns | 1,211.828 ns | 1,074.254 ns |  87,326.81 ns |         - |

[<MemoryDiagnoser>]
type DistanceTest2() =

    [<Params(10, 1000, 100000)>]
    member val size = 0 with get, set

    member val arrayA_us = [||] with get, set
    member val arrayB_us = [||] with get, set
    member val arrayA = [||] with get, set
    member val arrayB = [||] with get, set
    member val arrayA_uL = [||] with get, set
    member val arrayB_uL = [||] with get, set


    [<GlobalSetup>]
    member this.Setup() =
        this.arrayA_us <- Array.init this.size (fun dex -> (uint16 dex))
        this.arrayB_us <- Array.init this.size (fun dex -> (uint16 dex) + 1us)
        this.arrayA <- Array.init this.size (fun dex -> (int dex))
        this.arrayB <- Array.init this.size (fun dex -> (int dex) + 1)
        this.arrayA_uL <- Array.init this.size (fun dex -> (uint64 dex))
        this.arrayB_uL <- Array.init this.size (fun dex -> (uint64 dex) + 1UL)


    [<Benchmark>]
    member this.distanceSquaredG_us() =
        let yow = CollectionProps.distanceSquared_generic this.arrayA_us this.arrayB_us
        yow

    [<Benchmark>]
    member this.distanceSquaredG_uL() =
        let yow = CollectionProps.distanceSquared_generic this.arrayA_uL this.arrayB_uL
        yow

    [<Benchmark>]
    member this.distanceSquaredG() =
        let yow = CollectionProps.distanceSquared_generic this.arrayA this.arrayB
        yow




//|            Method |   size |         Mean |        Error |       StdDev |   Gen0 | Allocated |
//|------------------ |------- |-------------:|-------------:|-------------:|-------:|----------:|
//|  isDistanceGtZero |     10 |     12.59 ns |     0.155 ns |     0.145 ns |      - |         - |
//| isDistanceGtZeroC |     10 |    145.05 ns |     2.786 ns |     4.500 ns | 0.0076 |      24 B |
//|  isDistanceGtZero |    100 |     96.04 ns |     1.315 ns |     1.230 ns |      - |         - |
//| isDistanceGtZeroC |    100 |    225.49 ns |     2.355 ns |     1.966 ns | 0.0076 |      24 B |
//|  isDistanceGtZero |   1000 |    827.98 ns |     7.141 ns |     5.963 ns |      - |         - |
//| isDistanceGtZeroC |   1000 |  1,003.59 ns |    10.184 ns |     9.028 ns | 0.0076 |      24 B |
//|  isDistanceGtZero |  10000 |  8,269.85 ns |    86.082 ns |    80.521 ns |      - |         - |
//| isDistanceGtZeroC |  10000 |  9,432.98 ns |   171.390 ns |   176.005 ns |      - |      24 B |
//|  isDistanceGtZero | 100000 | 88,974.35 ns |   986.962 ns |   874.916 ns |      - |         - |
//| isDistanceGtZeroC | 100000 | 93,276.14 ns | 1,362.177 ns | 1,621.575 ns |      - |      24 B |

[<MemoryDiagnoser>]
type DistanceTest3() =

    [<Params(10, 100, 1000, 10000, 100000)>]
    member val size = 0 with get, set
    member val arrayA = [||] with get, set
    member val arrayB = [||] with get, set
    member val arrayC = [||] with get, set

    [<GlobalSetup>]
    member this.Setup() =
        this.arrayA <- Array.init this.size (fun dex -> (int dex))
        this.arrayB <- Array.init this.size (fun dex -> (int dex) + 1)

    [<Benchmark>]
    member this.isDistanceGtZero() =
        let yow = CollectionProps.isDistanceGtZero this.arrayA this.arrayB
        yow

    [<Benchmark>]
    member this.isDistanceGtZeroC() =
        let yow = CollectionProps.isDistanceGtZeroC this.arrayA this.arrayB
        yow



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
        let ssR = CollectionProps.isSorted_idiom this.testArray
        0


    [<Benchmark>]
    member this.isSorted_uL() =
        let ssR = CollectionProps.isSorted_uL this.testArray
        0


    [<Benchmark>]
    member this.isSorted_inline() =
        let ssR = CollectionProps.isSorted this.testArray
        0


    [<Benchmark>]
    member this.isSorted() =
        let ssR = CollectionProps.isSorted this.testArray
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
type BenchIsSorted_filterByPickList() =
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
        let ssR =
            CollectionOps.filterByPickList testArray pickArray |> Result.ExtractOrThrow

        0

    [<Benchmark>]
    member this.filterByPickList2() =
        let ssR =
            CollectionOps.filterByPickList testArray pickArray |> Result.ExtractOrThrow

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
    let aCore = [| 0..31 |]
    let aConj = [| 0..31 |]
    let aOut = Array.zeroCreate<int> 32

    let aCore16 = [| 0us .. 31us |]
    let aConj16 = [| 0us .. 31us |]
    let aOut16 = Array.zeroCreate<uint16> 32

    let aCore8 = [| 0uy .. 31uy |]
    let aConj8 = [| 0uy .. 31uy |]
    let aOut8 = Array.zeroCreate<uint8> 32


    [<Benchmark>]
    member this.invertArrayR() =
        let ssR =
            CollectionOps.invertArrayR aCore (Array.zeroCreate aConj.Length)
            |> Result.ExtractOrThrow

        0


    //[<Benchmark>]
    //member this.arrayProductInt() =
    //    let ssR = CollectionOps.arrayProductInt aConj aCore (Array.zeroCreate aConj.Length)
    //    0


    //[<Benchmark>]
    //member this.conjIntArraysNoAlloc() =
    //    let ssR = CollectionOps.arrayProductInt aConj aCore aOut
    //    0

    //[<Benchmark>]
    //member this.arrayProduct16() =
    //    let ssR = CollectionOps.arrayProduct16 aConj16 aCore16 aOut16 //(Array.zeroCreate<uint16> aConj.Length)
    //    0


    //[<Benchmark>]
    //member this.arrayProduct8() =
    //    let ssR = CollectionOps.arrayProduct8 aConj8 aCore8 aOut8 //(Array.zeroCreate<uint8> aConj.Length)
    //    0


    //[<Benchmark>]
    //member this.arrayProductIntR() =
    //    let ssR = CollectionOps.arrayProductIntR aConj aCore aOut |> Result.ExtractOrThrow
    //    0


    [<Benchmark>]
    member this.conjIntArrays() =
        let ssR = CollectionOps.conjIntArrays aConj aCore |> Result.ExtractOrThrow
        0


    [<Benchmark>]
    member this.conjIntArraysR() =
        let ssR = CollectionOps.conjIntArraysR aConj aCore |> Result.ExtractOrThrow
        0




[<MemoryDiagnoser>]
type BenchRollout() =
    let deg = Order.createNr 20
    let bpa = Order.allSortableAsUint64 deg |> Result.ExtractOrThrow

    [<Benchmark>]
    member this.toBitStriped() =
        let bsa = bpa |> ByteUtils.uint64ArraytoBitStriped deg |> Result.ExtractOrThrow
        0

    [<Benchmark>]
    member this.toBitStriped2D() =
        let bsa = bpa |> ByteUtils.uint64ArraytoBitStriped2D deg |> Result.ExtractOrThrow
        0


[<MemoryDiagnoser>]
type PermutationBench() =

    let permA =
        Permutation.create
            [| 0
               1
               2
               3
               4
               5
               6
               7
               8
               9
               10
               11
               12
               13
               14
               15
               16
               17
               18
               19
               20
               21
               22
               23
               24
               25
               26
               27
               28
               29
               30
               31 |]
        |> Result.ExtractOrThrow

    let permB =
        Permutation.create
            [| 10
               9
               2
               3
               4
               5
               6
               7
               8
               1
               0
               11
               12
               13
               14
               15
               16
               17
               18
               19
               20
               21
               22
               23
               24
               25
               26
               27
               28
               29
               30
               31 |]
        |> Result.ExtractOrThrow

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
type ArrayFormatBench() =
    let arrayLen = 100000
    let byteLen = arrayLen * 8
    let a64In = Array.init<uint64> arrayLen (uint64)

    [<Benchmark>]
    member this.MemCpy() =
        let bytesIn = Array.zeroCreate<byte> byteLen

        let bsa =
            ByteArray.mapUint64sToBytes 0 arrayLen bytesIn 0 a64In |> Result.ExtractOrThrow

        0




[<MemoryDiagnoser>]
type Md5VsSha256() =
    let N = 100000
    let data = Array.zeroCreate N
    let sha256 = SHA256.Create()
    let md5 = MD5.Create()

    member this.GetData = data

    [<Benchmark(Baseline = true)>]
    member this.Sha256() = sha256.ComputeHash(data)

    [<Benchmark>]
    member this.Md5() = md5.ComputeHash(data)
