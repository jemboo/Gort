﻿namespace global
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



//|                              Method |        Mean |     Error |      StdDev |
//|------------------------------------ |------------:|----------:|------------:|
//| applySorterAndMakeSwitchUses_RfBs64 |    264.0 us |  11.25 us |    33.18 us |
//|applySorterAndMakeSwitchTrack_RfBs64 |    516.2 us |  16.55 us |    48.81 us |
//|  applySorterAndMakeSwitchUses_RfI32 | 17,685.1 us | 353.28 us |   866.59 us |
//| applySorterAndMakeSwitchTrack_RfI32 | 21,702.7 us | 417.33 us | 1,084.69 us |
//|   applySorterAndMakeSwitchUses_RfU8 | 16,560.4 us | 319.00 us |   313.30 us |
//|  applySorterAndMakeSwitchTrack_RfU8 | 19,457.1 us | 441.29 us | 1,301.15 us |

type BenchmarkSorterOnBp64() =
    let order = (Order.createNr 16 )
    let sorter16 = RefSorter.createRefSorter RefSorter.Green16 
                        |> Result.ExtractOrThrow

    let sortableSetId = 123 |> SortableSetId.create
    let sortableSetFormat_RfBs64 = rolloutFormat.RfBs64
    let sortableSet_RfBs64 = 
        SortableSet.makeAllBits
                            sortableSetId
                            sortableSetFormat_RfBs64
                            order
        |> Result.ExtractOrThrow

    let sortableSetFormat_RfI32= rolloutFormat.RfI32
    let sortableSet_RfI32 = 
        SortableSet.makeAllBits
                            sortableSetId
                            sortableSetFormat_RfI32
                            order
        |> Result.ExtractOrThrow



    let sortableSetFormat_RfU8= rolloutFormat.RfU8
    let sortableSet_RfU8 = 
        SortableSet.makeAllBits
                            sortableSetId
                            sortableSetFormat_RfU8
                            order
        |> Result.ExtractOrThrow


    [<Benchmark>]
    member this.applySorterAndMakeSwitchUses_RfBs64() =
        let sorterResults = 
            SortingRollout.applySorterAndMakeSwitchUses
                                sorter16
                                sortableSet_RfBs64
        sorterResults


    [<Benchmark>]
    member this.applySorterAndMakeSwitchTrack_RfBs64() =
        let sorterResults = 
            SortingRollout.applySorterAndMakeSwitchTrack
                                sorter16
                                sortableSet_RfBs64
        sorterResults


    [<Benchmark>]
    member this.applySorterAndMakeSwitchUses_RfI32() =
        let sorterResults = 
            SortingRollout.applySorterAndMakeSwitchUses
                                sorter16
                                sortableSet_RfI32
        sorterResults


    [<Benchmark>]
    member this.applySorterAndMakeSwitchTrack_RfI32() =
        let sorterResults = 
            SortingRollout.applySorterAndMakeSwitchTrack
                                sorter16
                                sortableSet_RfI32
        sorterResults



    [<Benchmark>]
    member this.applySorterAndMakeSwitchUses_RfU8() =
        let sorterResults = 
            SortingRollout.applySorterAndMakeSwitchUses
                                sorter16
                                sortableSet_RfU8
        sorterResults


    [<Benchmark>]
    member this.applySorterAndMakeSwitchTrack_RfU8() =
        let sorterResults = 
            SortingRollout.applySorterAndMakeSwitchTrack
                                sorter16
                                sortableSet_RfU8
        sorterResults