namespace global
open System
open BenchmarkDotNet.Attributes

//|          Method | sorterCt | ordr |       Mean |      Error |     StdDev |       Gen0 |      Gen1 |      Gen2 | Allocated |
//|---------------- |--------- |----- |-----------:|-----------:|-----------:|-----------:|----------:|----------:|----------:|
//| RfBs64DontCheck |        1 |   14 |   5.342 ms |  0.0674 ms |  0.0692 ms |   429.6875 |         - |         - |    1.3 MB |
//|     RfBs64Check |        1 |   14 |   7.592 ms |  0.1415 ms |  0.3075 ms |   671.8750 |         - |         - |   2.01 MB |
//|  RfI32DontCheck |        1 |   14 |  16.871 ms |  0.1907 ms |  0.1784 ms |   593.7500 |  187.5000 |  187.5000 |   2.14 MB |
//|      RfI32Check |        1 |   14 |  17.128 ms |  0.1515 ms |  0.1343 ms |   593.7500 |  187.5000 |  187.5000 |   2.14 MB |
//| RfBs64DontCheck |        1 |   16 |   8.938 ms |  0.0779 ms |  0.0608 ms |   546.8750 |         - |         - |   1.77 MB |
//|     RfBs64Check |        1 |   16 |  16.767 ms |  0.1819 ms |  0.1787 ms |  1500.0000 |         - |         - |   4.63 MB |
//|  RfI32DontCheck |        1 |   16 |  79.497 ms |  0.7701 ms |  0.6431 ms |   714.2857 |  285.7143 |  285.7143 |   5.64 MB |
//|      RfI32Check |        1 |   16 |  81.391 ms |  0.6769 ms |  0.5653 ms |   714.2857 |  285.7143 |  285.7143 |   5.65 MB |
//| RfBs64DontCheck |        1 |   18 |  20.388 ms |  0.3591 ms |  0.3359 ms |   687.5000 |         - |         - |   2.63 MB |
//|     RfBs64Check |        1 |   18 |  20.928 ms |  0.3607 ms |  0.3374 ms |   718.7500 |         - |         - |   2.75 MB |
//|  RfI32DontCheck |        1 |   18 | 307.068 ms |  3.4250 ms |  3.2037 ms |   500.0000 |         - |         - |  20.06 MB |
//|      RfI32Check |        1 |   18 | 306.490 ms |  4.2564 ms |  3.7732 ms |   500.0000 |         - |         - |  20.06 MB |
//| RfBs64DontCheck |        5 |   14 |   9.978 ms |  0.1133 ms |  0.0946 ms |  2031.2500 |  359.3750 |         - |   6.32 MB |
//|     RfBs64Check |        5 |   14 |  13.121 ms |  0.2305 ms |  0.2156 ms |  2921.8750 |  203.1250 |         - |   8.95 MB |
//|  RfI32DontCheck |        5 |   14 |  27.715 ms |  0.4303 ms |  0.4025 ms |  2906.2500 |  843.7500 |  812.5000 |  10.55 MB |
//|      RfI32Check |        5 |   14 |  28.056 ms |  0.3965 ms |  0.3515 ms |  2906.2500 |  843.7500 |  812.5000 |  10.56 MB |
//| RfBs64DontCheck |        5 |   16 |  15.763 ms |  0.3063 ms |  0.3528 ms |  2875.0000 |  125.0000 |   62.5000 |   8.87 MB |
//|     RfBs64Check |        5 |   16 |  31.022 ms |  0.4750 ms |  0.9809 ms |  7562.5000 |         - |         - |  23.13 MB |
//|  RfI32DontCheck |        5 |   16 | 110.167 ms |  2.0257 ms |  1.8948 ms |  2800.0000 |  200.0000 |  200.0000 |  28.22 MB |
//|      RfI32Check |        5 |   16 | 113.186 ms |  2.1544 ms |  2.1159 ms |  2800.0000 |  200.0000 |  200.0000 |  28.22 MB |
//| RfBs64DontCheck |        5 |   18 |  34.175 ms |  0.5129 ms |  0.4546 ms |  3533.3333 |         - |         - |  13.23 MB |
//|     RfBs64Check |        5 |   18 |  87.437 ms |  1.6654 ms |  1.9178 ms | 21571.4286 |         - |         - |  67.23 MB |
//|  RfI32DontCheck |        5 |   18 | 533.775 ms | 15.8555 ms | 46.5015 ms |  4000.0000 | 1000.0000 | 1000.0000 | 100.39 MB |
//|      RfI32Check |        5 |   18 | 520.840 ms | 17.2764 ms | 50.9398 ms |  4000.0000 | 1000.0000 | 1000.0000 | 100.39 MB |
//| RfBs64DontCheck |       10 |   14 |  17.696 ms |  0.3468 ms |  0.3406 ms |  3343.7500 |  281.2500 |  125.0000 |  12.56 MB |
//|     RfBs64Check |       10 |   14 |  24.850 ms |  0.4756 ms |  1.1394 ms |  5156.2500 |  312.5000 |  125.0000 |  17.83 MB |
//|  RfI32DontCheck |       10 |   14 |  54.995 ms |  0.8431 ms |  0.7474 ms |  4666.6667 |  777.7778 |  555.5556 |  21.02 MB |
//|      RfI32Check |       10 |   14 |  54.632 ms |  1.0807 ms |  1.8928 ms |  4888.8889 | 1222.2222 |  777.7778 |  21.02 MB |
//| RfBs64DontCheck |       10 |   16 |  30.330 ms |  0.3611 ms |  0.3378 ms |  5750.0000 |  468.7500 |  187.5000 |  17.52 MB |
//|     RfBs64Check |       10 |   16 |  63.589 ms |  1.2553 ms |  1.7183 ms | 15125.0000 |         - |         - |  46.04 MB |
//|  RfI32DontCheck |       10 |   16 | 180.376 ms |  2.3706 ms |  1.9796 ms |  5666.6667 |  333.3333 |  333.3333 |  56.21 MB |
//|      RfI32Check |       10 |   16 | 185.905 ms |  3.6295 ms |  4.7194 ms |  5666.6667 |  333.3333 |  333.3333 |  56.21 MB |
//| RfBs64DontCheck |       10 |   18 |  63.099 ms |  1.1930 ms |  1.1159 ms |  7000.0000 |         - |         - |  26.27 MB |
//|     RfBs64Check |       10 |   18 | 155.145 ms |  2.4240 ms |  2.2674 ms | 38666.6667 |         - |         - | 120.73 MB |
//|  RfI32DontCheck |       10 |   18 | 876.951 ms | 15.6154 ms | 33.6139 ms |  8000.0000 | 1000.0000 | 1000.0000 | 200.58 MB |
//|      RfI32Check |       10 |   18 | 889.960 ms | 17.2569 ms | 39.6505 ms |  8000.0000 | 1000.0000 | 1000.0000 | 200.57 MB |

[<MemoryDiagnoser>]
type BenchSorterSet() =
    
    let sorterSetId = Guid.NewGuid() |> SorterSetId.create
    let rnGen = RngGen.createLcg (123 |> RandomSeed.create)
    let useParall = true |> UseParallel.create
    let sortableSetId = 123 |> SortableSetId.create


    [<Params(1, 5, 10)>]
    member val sorterCt = 0 with get, set

    [<Params(14, 16, 18 )>]
    member val ordr = 0 with get, set

    member val sortableSet_RfBs64 = SortableSet.createEmpty with get, set

    member val sortableSet_RfI32 = SortableSet.createEmpty with get, set

    member val sorterSet = SorterSet.createEmpty with get, set


    [<GlobalSetup>]
    member this.Setup() =
        let ordr = this.ordr |> Order.createNr
        let sorterCt = this.sorterCt |> SorterCount.create
        let switchCt = SwitchCount.orderTo900SwitchCount ordr
        this.sortableSet_RfBs64 <- 
                            SortableSet.makeAllBits 
                                sortableSetId 
                                rolloutFormat.RfBs64 
                                ordr
                            |> Result.ExtractOrThrow

        this.sortableSet_RfI32 <- 
                            SortableSet.makeAllBits 
                                sortableSetId 
                                rolloutFormat.RfI32 
                                ordr
                            |> Result.ExtractOrThrow

        this.sorterSet <- SorterSet.createRandomSwitches 
                            sorterSetId 
                            sorterCt 
                            ordr 
                            Seq.empty<switch> 
                            switchCt 
                            rnGen


    [<Benchmark>]
    member this.RfBs64DontCheck() =
        let res = SorterSetEval.eval 
                    sorterPerfEvalMode.DontCheckSuccess 
                    this.sortableSet_RfBs64 
                    this.sorterSet 
                    useParall
        res


    [<Benchmark>]
    member this.RfBs64Check() =
        let res = SorterSetEval.eval 
                    sorterPerfEvalMode.CheckSuccess 
                    this.sortableSet_RfBs64 
                    this.sorterSet 
                    useParall
        res


    [<Benchmark>]
    member this.RfI32DontCheck() =
        let res = SorterSetEval.eval 
                    sorterPerfEvalMode.DontCheckSuccess 
                    this.sortableSet_RfI32 
                    this.sorterSet 
                    useParall
        res


    [<Benchmark>]
    member this.RfI32Check() =
        let res = SorterSetEval.eval 
                    sorterPerfEvalMode.CheckSuccess 
                    this.sortableSet_RfI32 
                    this.sorterSet 
                    useParall
        res



[<MemoryDiagnoser>]
type BenchSorterSet0() =
    let rnGen = RngGen.createLcg (123 |> RandomSeed.create)
    let useParall = true |> UseParallel.create
    let order = (Order.createNr 16)
    let switchCt = SwitchCount.orderTo900SwitchCount order
    let sorterCt = 50 |> SorterCount.create
    let sortableSetId = 123 |> SortableSetId.create
    let sortableSetFormat_RfBs64 = rolloutFormat.RfBs64
    let sortableSetFormat_RfI32 = rolloutFormat.RfI32
    let sorterSetEvalMod = sorterPerfEvalMode.DontCheckSuccess
    let sorterSetId = Guid.NewGuid() |> SorterSetId.create
    let sortableSet_RfBs64 =
        SortableSet.makeAllBits sortableSetId sortableSetFormat_RfBs64 order
        |> Result.ExtractOrThrow

    let sortableSet_RfI32 =
        SortableSet.makeAllBits sortableSetId sortableSetFormat_RfI32 order
        |> Result.ExtractOrThrow


    let sorterSt =
        SorterSet.createRandomSwitches sorterSetId sorterCt order Seq.empty<switch> switchCt rnGen


    [<Benchmark>]
    member this.evalSortableSet_RfBs64() =
        let res = SorterSetEval.eval sorterSetEvalMod sortableSet_RfBs64 sorterSt useParall
        res

    //[<Benchmark>]
    //member this.repSortableSet_RfBs64() =
    //    let sorterSpeedEvls, errs = 
    //            SorterSetEval.eval sorterSetEvalMod sortableSet_RfBs64 sorterSt useParall

    //    let sorterSpeedRs =
    //        sorterSpeedEvls |> Array.map (SorterEval.getSorterSpeedBin)

    //    //let sorterSpeeds = sorterSpeedRs 
    //    //                        |> Array.filter(fun rv -> rv |> Result.isOk)
    //    //                        |> Array.map(Result.ExtractOrThrow)

    //    //let spsfsbs =
    //    //    sorterSpeeds
    //    //    |> SorterPhenotypeSpeedsForSpeedBin.fromSorterSpeeds
    //    //    |> Seq.toArray
    //    //    |> Array.sortBy (fun sp ->
    //    //        sp
    //    //        |> SorterPhenotypeSpeedsForSpeedBin.getSpeedBin
    //    //        |> SorterSpeedBin.getIndexOfBin)

    //    sorterSpeedRs

    [<Benchmark>]
    member this.evalSortableSet_RfI32() =
        let res = SorterSetEval.eval sorterSetEvalMod sortableSet_RfI32 sorterSt useParall
        res
