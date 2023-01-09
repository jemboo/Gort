namespace global
open System
open BenchmarkDotNet.Attributes



//| Method | sorterCt | ordr | srtablFormat |  evalMode |        Mean |    Error |    StdDev |      Median |       Gen0 |      Gen1 |      Gen2 | Allocated |
//|------- |--------- |----- |------------- |---------- |------------:|---------:|----------:|------------:|-----------:|----------:|----------:|----------:|
//|  paral |       20 |   16 |         BP64 |     check |   115.05 ms | 2.003 ms |  1.874 ms |   115.37 ms | 27800.0000 |  800.0000 |  600.0000 |  83.68 MB |
//| serial |       20 |   16 |         BP64 |     check |   306.17 ms | 3.493 ms |  2.917 ms |   305.85 ms | 27000.0000 |  500.0000 |  500.0000 |  83.69 MB |
//|  paral |       20 |   16 |         BP64 | dontCheck |    61.24 ms | 0.970 ms |  0.907 ms |    61.18 ms | 11555.5556 | 1000.0000 |  666.6667 |     35 MB |
//| serial |       20 |   16 |         BP64 | dontCheck |   177.05 ms | 4.225 ms | 12.054 ms |   171.69 ms | 10666.6667 |  666.6667 |  666.6667 |  34.99 MB |
//|  paral |       20 |   16 |          I32 |     check |   361.14 ms | 6.483 ms | 17.078 ms |   358.06 ms | 12000.0000 | 1000.0000 | 1000.0000 |  112.4 MB |
//| serial |       20 |   16 |          I32 |     check | 1,346.26 ms | 7.116 ms |  6.656 ms | 1,348.01 ms | 18000.0000 | 8000.0000 | 8000.0000 | 112.42 MB |
//|  paral |       20 |   16 |          I32 | dontCheck |   366.24 ms | 9.620 ms | 28.365 ms |   352.43 ms | 12000.0000 | 1000.0000 | 1000.0000 | 112.39 MB |
//| serial |       20 |   16 |          I32 | dontCheck | 1,315.71 ms | 6.130 ms |  5.734 ms | 1,314.76 ms | 18000.0000 | 8000.0000 | 8000.0000 | 112.42 MB |


[<MemoryDiagnoser>]
type BenchSorterSet() =

    [<Params(20)>]
    member val sorterCt = 0 with get, set

    [<Params(16)>]
    member val ordr = 0 with get, set

    [<Params("I32", "BP64" )>]
    member val srtablFormat = "" with get, set

    [<Params("check", "dontCheck" )>]
    member val evalMode = "" with get, set

    member val sortableSet = SortableSet.createEmpty with get, set

    member val sorterSet = SorterSet.createEmpty with get, set

    member val perfEvalMode = sorterEvalMode.CheckSuccess with get, set

    member val sortableFormat = rolloutFormat.RfI32 with get, set


    [<GlobalSetup>]
    member this.Setup() =
        let ordr = this.ordr |> Order.createNr
        let sorterCt = this.sorterCt |> SorterCount.create
        let switchCt = SwitchCount.orderTo900SwitchCount ordr

        if this.evalMode = "check" then
            this.perfEvalMode <- sorterEvalMode.CheckSuccess
        else
            this.perfEvalMode <- sorterEvalMode.DontCheckSuccess

        if this.srtablFormat = "BP64" then
            this.sortableFormat <- rolloutFormat.RfBs64 
        else
            this.sortableFormat <- rolloutFormat.RfI32 


        this.sortableSet <- SortableSet.makeAllBits 
                                (123 |> SortableSetId.create) 
                                this.sortableFormat 
                                ordr
                            |> Result.ExtractOrThrow


        this.sorterSet <- SorterSet.createRandomSwitches 
                            (Guid.NewGuid() |> SorterSetId.create)
                            sorterCt 
                            ordr 
                            Seq.empty<switch> 
                            switchCt 
                            (RngGen.createLcg (123 |> RandomSeed.create))


    [<Benchmark>]
    member this.paral() =
        let res = SorterSetEval.evalSorters 
                    this.perfEvalMode
                    this.sortableSet
                    (this.sorterSet |> SorterSet.getSorters) 
                    (true |> UseParallel.create)
        res


    [<Benchmark>]
    member this.serial() =
        let res = SorterSetEval.evalSorters 
                    this.perfEvalMode
                    this.sortableSet
                    (this.sorterSet |> SorterSet.getSorters)
                    (false |> UseParallel.create)
        res