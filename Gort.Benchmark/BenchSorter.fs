namespace global
open System
open BenchmarkDotNet.Attributes


[<MemoryDiagnoser>]
type BenchMakeSortableStack() =
    let order = Order.create 16 |> Result.ExtractOrThrow

    let orders =
        [| Order.create 8; Order.create 4; Order.create 2; Order.create 2 |]
        |> Array.toList
        |> Result.sequence
        |> Result.ExtractOrThrow
        |> List.toArray

    let offset = 7



//|                              Method |        Mean |     Error |      StdDev |
//|------------------------------------ |------------:|----------:|------------:|
//| applySorterAndMakeSwitchUses_RfBs64 |    264.0 us |  11.25 us |    33.18 us |
//|applySorterAndMakeSwitchTrack_RfBs64 |    516.2 us |  16.55 us |    48.81 us |
//|  applySorterAndMakeSwitchUses_RfI32 | 17,685.1 us | 353.28 us |   866.59 us |
//| applySorterAndMakeSwitchTrack_RfI32 | 21,702.7 us | 417.33 us | 1,084.69 us |
//|   applySorterAndMakeSwitchUses_RfU8 | 16,560.4 us | 319.00 us |   313.30 us |
//|  applySorterAndMakeSwitchTrack_RfU8 | 19,457.1 us | 441.29 us | 1,301.15 us |



//|                                           Method |        Mean |     Error |      StdDev |
//|------------------------------------------------- |------------:|----------:|------------:|
//|              applySorterAndMakeSwitchUses_RfBs64 |    146.5 us |   2.89 us |     3.86 us |
//|             applySorterAndMakeSwitchTrack_RfBs64 |    622.4 us |  12.37 us |    36.48 us |
//|  applySorterAndMakeSwitchUses_checkSorted_RfBs64 | 13,138.7 us | 176.76 us |   165.34 us |
//| applySorterAndMakeSwitchTrack_checkSorted_RfBs64 | 26,447.1 us | 520.96 us | 1,075.87 us |



//|                                          Method |     Mean |    Error |   StdDev |
//|------------------------------------------------ |---------:|---------:|---------:|
//|              applySorterAndMakeSwitchUses_RfI32 | 18.92 ms | 0.378 ms | 0.728 ms |
//|             applySorterAndMakeSwitchTrack_RfI32 | 20.60 ms | 0.406 ms | 0.802 ms |
//|  applySorterAndMakeSwitchUses_checkSorted_RfI32 | 18.79 ms | 0.372 ms | 1.030 ms |
//| applySorterAndMakeSwitchTrack_checkSorted_RfI32 | 21.79 ms | 0.396 ms | 0.486 ms |



//|                                              Method |     Mean |    Error |   StdDev |
//|---------------------------------------------------- |---------:|---------:|---------:|
//|                  applySorterAndMakeSwitchUses_RfI32 | 17.33 ms | 0.344 ms | 0.844 ms |
//|                 applySorterAndMakeSwitchTrack_RfI32 | 20.56 ms | 0.408 ms | 0.682 ms |
//|  applySorterAndMakeSwitchUses_getSwitchUseCts_RfI32 | 17.34 ms | 0.344 ms | 0.798 ms |
//| applySorterAndMakeSwitchTrack_getSwitchUseCts_RfI32 | 27.96 ms | 0.456 ms | 0.426 ms |

//|----------------------------------------------------- |------------:|----------:|----------:|
//|                  applySorterAndMakeSwitchUses_RfBs64 |    144.8 us |   1.37 us |   1.15 us |
//|                 applySorterAndMakeSwitchTrack_RfBs64 |    591.6 us |  11.73 us |  34.59 us |
//|  applySorterAndMakeSwitchUses_getSwitchUseCts_RfBs64 |    142.9 us |   2.22 us |   2.08 us |
//| applySorterAndMakeSwitchTrack_getSwitchUseCts_RfBs64 | 17,712.3 us | 351.61 us | 756.87 us |


//|                                                 Method |        Mean |     Error |      StdDev |
//|------------------------------------------------------- |------------:|----------:|------------:|
//| applySorterAndMakeSwitchUses_getUsedSwitchCount_RfBs64 |    294.7 us |  11.00 us |    32.44 us |
//|                   applySorter_getUsedSwitchCount_RfI32 | 18,294.4 us | 362.74 us | 1,017.16 us |
//|         applySorterAndMakeSwitchUses_checkSorted_RfI32 | 19,120.9 us | 382.13 us |   908.16 us |
//|      applySorterAndMakeSwitchTrack_getUnsortedCt_RfI32 | 26,581.1 us | 523.21 us | 1,080.52 us |
//|       applySorterAndMakeSwitchTrack_checkSorted_RfBs64 | 20,640.8 us | 412.41 us |   922.41 us |
//|     applySorterAndMakeSwitchTrack_getUnsortedCt_RfBs64 |  7,348.6 us | 145.33 us |   276.51 us |


type BenchmarkSorterOnBp64() =
    let order = (Order.createNr 16)
    let sortrId = (Guid.NewGuid()) |> SorterId.create
    let sorter16 = RefSorter.createRefSorter sortrId RefSorter.Green16 |> Result.ExtractOrThrow

    let sortableSetId = (Guid.NewGuid()) |> SortableSetId.create
    let sortableSetFormat_RfBs64 = rolloutFormat.RfBs64

    let sortableSet_RfBs64 =
        SortableSet.makeAllBits sortableSetId sortableSetFormat_RfBs64 order
        |> Result.ExtractOrThrow

    let sortableSetFormat_RfI32 = rolloutFormat.RfI32

    let sortableSet_RfI32 =
        SortableSet.makeAllBits sortableSetId sortableSetFormat_RfI32 order
        |> Result.ExtractOrThrow

    let sortableSetFormat_RfU8 = rolloutFormat.RfU8

    let sortableSet_RfU8 =
        SortableSet.makeAllBits sortableSetId sortableSetFormat_RfU8 order
        |> Result.ExtractOrThrow


    //[<Benchmark>]
    //member this.applySorterAndMakeSwitchUses_RfBs64() =
    //    let sorterOpOutput =
    //        SortingRollout.applySorterAndMakeSwitchUses
    //                            sorter16
    //                            sortableSet_RfBs64
    //    sorterOpOutput


    //[<Benchmark>]
    //member this.applySorterAndMakeSwitchTrack_RfBs64() =
    //    let sorterOpOutput =
    //        SortingRollout.applySorterAndMakeSwitchTrack
    //                            sorter16
    //                            sortableSet_RfBs64
    //    sorterOpOutput


    //[<Benchmark>]
    //member this.applySorterAndMakeSwitchUses_getUsedSwitchCount_RfBs64() =
    //    let sorterOpOutput =
    //        SortingRollout.applySorterAndMakeSwitchUses
    //                            sorter16
    //                            sortableSet_RfBs64
    //    let usedSwitchCt =
    //        sorterOpOutput
    //        |> SwitchUseCounters.fromSorterOpOutput
    //        |> SwitchUseCounters.getUsedSwitchCount
    //    sorterOpOutput, usedSwitchCt



    [<Benchmark>]
    member this.evalSorterWithSortableSet_getUsedSwitchCount_RfBs64() =
        let trackMode = sorterOpTrackMode.SwitchUses

        let sorterOpOutput =
            SortingRollout.makeSorterOpOutput trackMode sortableSet_RfBs64 sorter16
            |> Result.ExtractOrThrow

        let usedSwitchCt =
            sorterOpOutput
            |> SorterOpOutput.getSorterOpTracker
            |> SorterOpTracker.getSwitchUseCounts

        sorterOpOutput, usedSwitchCt


    [<Benchmark>]
    member this.evalSorterWithSortableSetR_getUsedSwitchCount_RfBs64() =
        let trackMode = sorterOpTrackMode.SwitchUses

        let sorterOpOutput =
            SortingRollout.makeSorterOpOutput trackMode sortableSet_RfBs64 sorter16
            |> Result.ExtractOrThrow

        let usedSwitchCt =
            sorterOpOutput
            |> SorterOpOutput.getSorterOpTracker
            |> SorterOpTracker.getSwitchUseCounts

        sorterOpOutput, usedSwitchCt



//[<Benchmark>]
//member this.applySorterAndMakeSwitchTrack_getSwitchUseCts_RfBs64() =
//    let sorterOpOutput =
//        SortingRollout.applySorterAndMakeSwitchTrack
//                            sorter16
//                            sortableSet_RfBs64
//    let usedSwitchCt =
//        sorterOpOutput
//        |> SwitchUseCounters.fromSorterOpOutput
//        |> SwitchUseCounters.getUsedSwitchCount
//    sorterOpOutput, usedSwitchCt



//[<Benchmark>]
//member this.applySorterAndMakeSwitchUses_checkSorted_RfBs64() =
//    let sorterOpOutput =
//        SortingRollout.applySorterAndMakeSwitchUses
//                            sorter16
//                            sortableSet_RfBs64
//    let isSorted = sorterOpOutput |> SorterOpOutput.isSorted
//    sorterOpOutput, isSorted



//[<Benchmark>]
//member this.applySorter_getUsedSwitchCount_RfI32() =
//    let sorterOpOutput =
//        SortingRollout.applySorterAndMakeSwitchUses
//                            sorter16
//                            sortableSet_RfI32

//    let usedSwitchCt =
//        sorterOpOutput
//        |> SwitchUseCounters.fromSorterOpOutput
//        |> SwitchUseCounters.getUsedSwitchCount
//    sorterOpOutput, usedSwitchCt


//[<Benchmark>]
//member this.applySorterAndMakeSwitchTrack_RfI32() =
//    let sorterOpOutput =
//        SortingRollout.applySorterAndMakeSwitchTrack
//                            sorter16
//                            sortableSet_RfI32
//    sorterOpOutput


//[<Benchmark>]
//member this.applySorterAndMakeSwitchUses_checkSorted_RfI32() =
//    let sorterOpOutput =
//        SortingRollout.applySorterAndMakeSwitchUses
//                            sorter16
//                            sortableSet_RfI32
//    let isSorted = sorterOpOutput |> SorterOpOutput.isSorted
//    sorterOpOutput, isSorted


//[<Benchmark>]
//member this.applySorterAndMakeSwitchUses_checkSorted_RfI32() =
//    let sorterOpOutput =
//        SortingRollout.applySorterAndMakeSwitchUses
//                            sorter16
//                            sortableSet_RfI32
//    let isSorted = sorterOpOutput |> SorterOpOutput.isSorted
//    sorterOpOutput, isSorted


//[<Benchmark>]
//member this.applySorterAndMakeSwitchTrack_getUnsortedCt_RfI32() =
//    let sorterOpOutput =
//        SortingRollout.applySorterAndMakeSwitchTrack
//                            sorter16
//                            sortableSet_RfI32
//    let unSortedCt = sorterOpOutput
//                     |> SorterOpOutput.getUnsortedCt
//                     |> Result.ExtractOrThrow
//    sorterOpOutput, unSortedCt


//[<Benchmark>]
//member this.applySorterAndMakeSwitchTrack_checkSorted_RfBs64() =
//    let sorterOpOutput =
//        SortingRollout.applySorterAndMakeSwitchTrack
//                            sorter16
//                            sortableSet_RfBs64
//    let isSorted = sorterOpOutput |> SorterOpOutput.isSorted
//    sorterOpOutput, isSorted


//[<Benchmark>]
//member this.applySorterAndMakeSwitchTrack_getUnsortedCt_RfBs64() =
//    let sorterOpOutput =
//        SortingRollout.applySorterAndMakeSwitchTrack
//                            sorter16
//                            sortableSet_RfBs64
//    let unSortedCt =
//        sorterOpOutput
//                |> SorterOpOutput.getRefinedSortableSet
//                |> Result.ExtractOrThrow
//    sorterOpOutput, unSortedCt



//[<Benchmark>]
//member this.applySorterAndMakeSwitchUses_getSwitchUseCts_RfI32() =
//    let sorterOpOutput =
//        SortingRollout.applySorterAndMakeSwitchUses
//                            sorter16
//                            sortableSet_RfI32
//    let usedSwitchCt =
//        sorterOpOutput
//        |> SwitchUseCounters.fromSorterOpOutput
//        |> SwitchUseCounters.getUsedSwitchCount
//    sorterOpOutput, usedSwitchCt


//[<Benchmark>]
//member this.applySorterAndMakeSwitchTrack_getSwitchUseCts_RfI32() =
//    let sorterOpOutput =
//        SortingRollout.applySorterAndMakeSwitchTrack
//                            sorter16
//                            sortableSet_RfI32
//    let usedSwitchCt =
//        sorterOpOutput
//        |> SwitchUseCounters.fromSorterOpOutput
//        |> SwitchUseCounters.getUsedSwitchCount
//    sorterOpOutput, usedSwitchCt



//[<Benchmark>]
//member this.applySorterAndMakeSwitchUses_RfU8() =
//    let sorterResults =
//        SortingRollout.applySorterAndMakeSwitchUses
//                            sorter16
//                            sortableSet_RfU8
//    sorterResults


//[<Benchmark>]
//member this.applySorterAndMakeSwitchTrack_RfU8() =
//    let sorterResults =
//        SortingRollout.applySorterAndMakeSwitchTrack
//                            sorter16
//                            sortableSet_RfU8
//    sorterResults
