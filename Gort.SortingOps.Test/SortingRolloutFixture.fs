namespace Gort.SortingOps.Test

open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type SortingRolloutFixture () =

    let getResultsWithSwitchTrack (rolloutFormat:rolloutFormat) =
        let order = Order.create 8 |> Result.ExtractOrThrow
        let randy = Rando.create rngType.Lcg (123 |> RandomSeed.create)
        let sortableSetId = 123 |> SortableSetId.create
        let sortableCount = 100 |> SortableCount.create
        let goodSorter = 
            RefSorter.goodRefSorterForOrder 
                        order
            |> Result.ExtractOrThrow
        let sorterId = goodSorter |> Sorter.makeId

        let sortableSet = 
            SortableSet.makeRandomBits 
                                sortableSetId
                                rolloutFormat
                                order
                                0.5
                                sortableCount
                                randy
            |> Result.ExtractOrThrow


        let aas = sortableSet |> SortableSet.toSortableBoolSets
                              |> Seq.toArray

        let rollout = 
            sortableSet |> SortableSet.getRollout

        let sorterResults = 
            SortingRollout.applySorterAndMakeSwitchTrack
                                goodSorter
                                sorterId
                                sortableSetId
                                rollout


        let sortedSet = sorterResults
                        |> SorterOpTrack.getRollout
                        |> Rollout.toBoolArrays
                        |> Seq.toArray


        let isSorted = sorterResults |> SorterOpTrack.isSorted
        Assert.IsTrue(isSorted)


    let getResultsWithSwitchUses (rolloutFormat:rolloutFormat) =
        let order = Order.create 16 |> Result.ExtractOrThrow
        let randy = Rando.create rngType.Lcg (123 |> RandomSeed.create)
        
        let sortableSetId = 123 |> SortableSetId.create
        let sortableCount = 10 |> SortableCount.create
        let goodSorter = 
            RefSorter.goodRefSorterForOrder order 
            |> Result.ExtractOrThrow
        let sorterId = goodSorter |> Sorter.makeId
        let sortableSet = 
            SortableSet.makeAllBits
                                sortableSetId
                                rolloutFormat
                                order
            |> Result.ExtractOrThrow

        //let sortableSet = 
        //    SortableSet.makeRandomPermutation 
        //                        sortableSetId
        //                        rolloutFormat
        //                        order
        //                        sortableCount
        //                        randy
        //    |> Result.ExtractOrThrow

        let rollout = sortableSet 
                      |> SortableSet.getRollout

        let sorterResults = 
            SortingRollout.applySorterAndMakeSwitchUses
                                goodSorter
                                sorterId
                                sortableSetId
                                rollout

        let isSorted = sorterResults |> SorterOpTrack.isSorted
        Assert.IsTrue(isSorted)


    [<TestMethod>]
    member this.rolloutFormatWithSwitchTrack () =
        
        //let sortableSetFormat_RfI32 = rolloutFormat.RfI32
        //let results_RfI32 = getResultsWithSwitchTrack sortableSetFormat_RfI32
                
        //let sortableSetFormat_RfU16 = rolloutFormat.RfU16 
        //let results_RfU16 = getResultsWithSwitchTrack sortableSetFormat_RfU16
        
        //let sortableSetFormat_RfU8 = rolloutFormat.RfU8 
        //let results_RfU8 = getResultsWithSwitchTrack sortableSetFormat_RfU8

        let sortableSetFormat_RfBs64 = rolloutFormat.RfBs64
        let results_RfBs64 = getResultsWithSwitchTrack sortableSetFormat_RfBs64

        Assert.IsTrue(true);


    [<TestMethod>]
    member this.rolloutFormatWithSwitchUses () =
        
        //let sortableSetFormat_RfI32 = rolloutFormat.RfI32
        //let results_RfI32 = getResultsWithSwitchUses sortableSetFormat_RfI32
                
        //let sortableSetFormat_RfU16 = rolloutFormat.RfU16
        //let results_RfU16 = getResultsWithSwitchUses sortableSetFormat_RfU16
        
        //let sortableSetFormat_RfU8 = rolloutFormat.RfU8
        //let results_RfU8 = getResultsWithSwitchUses sortableSetFormat_RfU8

        let sortableSetFormat_RfBs64 = rolloutFormat.RfBs64
        let results_RfBs64 = getResultsWithSwitchUses sortableSetFormat_RfBs64
        Assert.IsTrue(true);



    [<TestMethod>]
    member this.yav () =
        let wak = [| 0 .. 8 .. 119 |]
        Assert.IsTrue(true);