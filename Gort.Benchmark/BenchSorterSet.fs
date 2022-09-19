namespace global
open BenchmarkDotNet.Attributes

//|            Method |     Mean |   Error |  StdDev |      Gen0 |      Gen1 |      Gen2 | Allocated |
//|------------------ |---------:|--------:|--------:|----------:|----------:|----------:|----------:|
//|   evalSortableSet | 209.5 ms | 4.12 ms | 8.51 ms | 2666.6667 | 1333.3333 | 1333.3333 |  22.81 MB |
//| evalSortableSet_R | 202.8 ms | 4.03 ms | 8.94 ms | 2666.6667 | 1333.3333 | 1333.3333 |  22.81 MB |

//|            Method |     Mean |    Error |   StdDev |      Gen0 |      Gen1 |     Gen2 | Allocated |
//|------------------ |---------:|---------:|---------:|----------:|----------:|---------:|----------:|
//|   evalSortableSet | 57.90 ms | 1.149 ms | 1.856 ms | 2555.5556 | 1222.2222 | 777.7778 |  22.82 MB |
//| evalSortableSet_R | 58.90 ms | 1.176 ms | 2.150 ms | 2333.3333 |  888.8889 | 666.6667 |  22.83 MB |



//|            Method |     Mean |   Error |   StdDev |       Gen0 |      Gen1 |      Gen2 | Allocated |
//|------------------ |---------:|--------:|---------:|-----------:|----------:|----------:|----------:|
//|   evalSortableSet | 294.6 ms | 5.82 ms | 13.71 ms | 57000.0000 | 5000.0000 | 3000.0000 | 261.97 MB |
//| evalSortableSet_R | 303.7 ms | 6.06 ms | 15.32 ms | 55500.0000 | 3000.0000 | 2000.0000 | 261.92 MB |

[<MemoryDiagnoser>]
type BenchSorterSet() =
    let rnGen = RngGen.createLcg  (123 |> RandomSeed.create)
    let useParall = true |> UseParallel.create
    let order = (Order.createNr 18 )
    let switchCt = SwitchCount.orderTo900SwitchCount order
    let sorterCt = 100 |> SorterCount.create
    let sortableSetId = 123 |> SortableSetId.create
    let sortableSetFormat_RfBs64 = rolloutFormat.RfBs64
    let sorterSetEvalMod = sorterEvalMode.SorterSpeed
    let sortableSet_RfBs64 = 
        SortableSet.makeAllBits
                            sortableSetId
                            sortableSetFormat_RfBs64
                            order
        |> Result.ExtractOrThrow
    let sorterSt = SorterSet.createRandomSwitches
                    order
                    Seq.empty<switch>
                    switchCt
                    sorterCt
                    rnGen


    [<Benchmark>]
    member this.evalSortableSet() =
        let res = SorterSetEval.eval
                        sorterSetEvalMod
                        sortableSet_RfBs64
                        sorterSt
                        useParall
        res