namespace Gort.SortingOps.Test
open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type SortingRolloutFixture() =

    let getGoodRefResults (sorterOpTrackMode: sorterOpTrackMode) (rolloutFormat: rolloutFormat) =
        let order = Order.create 8 |> Result.ExtractOrThrow
        let sortableSetId = Guid.NewGuid() |> SortableSetId.create
        let sortrId = Guid.NewGuid() |> SorterId.create
        let goodSorter = RefSorter.goodRefSorterForOrder sortrId order |> Result.ExtractOrThrow

        let sortableSet =
            SortableSet.makeAllBits sortableSetId rolloutFormat order
            |> Result.ExtractOrThrow


        let sorterOpOutput =
            SortingRollout.makeSorterOpOutput sorterOpTrackMode sortableSet goodSorter

        sorterOpOutput


    let getIncompleteSortResults (sorterOpTrackMode: sorterOpTrackMode) (rolloutFormat: rolloutFormat) =
        let order = Order.create 8 |> Result.ExtractOrThrow
        let sortableSetId = Guid.NewGuid() |> SortableSetId.create
        let switchCount = SwitchCount.orderToRecordSwitchCount order
        let rando = Rando.create rngType.Lcg (1233 |> RandomSeed.create)
        let sortrId = Guid.NewGuid() |> SorterId.create
        let failingSorter = Sorter.randomSwitches order (Seq.empty) switchCount rando sortrId 

        let sortableSet =
            SortableSet.makeAllBits sortableSetId rolloutFormat order
            |> Result.ExtractOrThrow

        let sorterOpOutput =
            SortingRollout.makeSorterOpOutput sorterOpTrackMode sortableSet failingSorter

        sorterOpOutput


    [<TestMethod>]
    member this.evalSorterWithSortableSet() =
        let sotmSwitchUses = sorterOpTrackMode.SwitchUses
        let sotmSwitchTrack = sorterOpTrackMode.SwitchTrack

        let sortableSetFormat_RfI32 = rolloutFormat.RfI32
        let sortableSetFormat_RfU16 = rolloutFormat.RfU16
        let sortableSetFormat_RfU8 = rolloutFormat.RfU8
        let sortableSetFormat_RfBs64 = rolloutFormat.RfBs64

        let res_RfU8_su =
            getGoodRefResults sotmSwitchUses sortableSetFormat_RfU8 |> Result.ExtractOrThrow

        Assert.IsTrue(res_RfU8_su |> SorterOpOutput.isSorted)
        let sot_RfU8_su = res_RfU8_su |> SorterOpOutput.getSorterOpTracker
        let suCt_RfU8_su = sot_RfU8_su |> SorterOpTracker.getSwitchUseCounts


        let res_RfU16_su =
            getGoodRefResults sotmSwitchUses sortableSetFormat_RfU16
            |> Result.ExtractOrThrow

        Assert.IsTrue(res_RfU16_su |> SorterOpOutput.isSorted)
        let sot_RfU16_su = res_RfU16_su |> SorterOpOutput.getSorterOpTracker
        let suCt_RfU16_su = sot_RfU16_su |> SorterOpTracker.getSwitchUseCounts
        Assert.IsTrue(CollectionProps.areEqual suCt_RfU8_su suCt_RfU16_su)


        let res_RfI32_su =
            getGoodRefResults sotmSwitchUses sortableSetFormat_RfI32
            |> Result.ExtractOrThrow

        Assert.IsTrue(res_RfI32_su |> SorterOpOutput.isSorted)
        let sot_RfU32_su = res_RfI32_su |> SorterOpOutput.getSorterOpTracker
        let suCt_RfU32_su = sot_RfU32_su |> SorterOpTracker.getSwitchUseCounts
        Assert.IsTrue(CollectionProps.areEqual suCt_RfU8_su suCt_RfU32_su)


        let res_RfBs64_su =
            getGoodRefResults sotmSwitchUses sortableSetFormat_RfBs64
            |> Result.ExtractOrThrow

        Assert.IsTrue(res_RfBs64_su |> SorterOpOutput.isSorted)
        let sot_RfU64_su = res_RfBs64_su |> SorterOpOutput.getSorterOpTracker
        let suCt_RfU64_su = sot_RfU64_su |> SorterOpTracker.getSwitchUseCounts
        // true counts are not available
        //Assert.IsTrue(CollectionProps.areEqual suCt_RfU8_su suCt_RfU64_su)


        let res_RfU8_st =
            getGoodRefResults sotmSwitchTrack sortableSetFormat_RfU8
            |> Result.ExtractOrThrow

        Assert.IsTrue(res_RfU8_st |> SorterOpOutput.isSorted)
        let sot_RfU8_st = res_RfU8_st |> SorterOpOutput.getSorterOpTracker
        let suCt_RfU8_st = sot_RfU8_st |> SorterOpTracker.getSwitchUseCounts
        Assert.IsTrue(CollectionProps.areEqual suCt_RfU8_su suCt_RfU8_st)


        let res_RfU16_st =
            getGoodRefResults sotmSwitchTrack sortableSetFormat_RfU16
            |> Result.ExtractOrThrow

        Assert.IsTrue(res_RfU16_st |> SorterOpOutput.isSorted)
        let sot_RfU16_st = res_RfU16_st |> SorterOpOutput.getSorterOpTracker
        let suCt_RfU16_st = sot_RfU16_st |> SorterOpTracker.getSwitchUseCounts
        Assert.IsTrue(CollectionProps.areEqual suCt_RfU8_su suCt_RfU16_st)


        let res_RfI32_st =
            getGoodRefResults sotmSwitchTrack sortableSetFormat_RfI32
            |> Result.ExtractOrThrow

        Assert.IsTrue(res_RfI32_st |> SorterOpOutput.isSorted)
        let sot_RfU32_st = res_RfI32_st |> SorterOpOutput.getSorterOpTracker
        let suCt_RfU32_st = sot_RfU32_st |> SorterOpTracker.getSwitchUseCounts
        Assert.IsTrue(CollectionProps.areEqual suCt_RfU8_su suCt_RfU32_st)


        let res_RfBs64_st =
            getGoodRefResults sotmSwitchTrack sortableSetFormat_RfBs64
            |> Result.ExtractOrThrow

        Assert.IsTrue(res_RfBs64_st |> SorterOpOutput.isSorted)
        let sot_RfU64_st = res_RfBs64_st |> SorterOpOutput.getSorterOpTracker
        let suCt_RfU64_st = sot_RfU64_st |> SorterOpTracker.getSwitchUseCounts

        Assert.IsTrue(CollectionProps.areEqual suCt_RfU8_su suCt_RfU64_st)


        Assert.IsTrue(true)


    [<TestMethod>]
    member this.getRefinedSortableSet() =
        let sotmSwitchUses = sorterOpTrackMode.SwitchUses
        let sotmSwitchTrack = sorterOpTrackMode.SwitchTrack

        let sortableSetFormat_RfI32 = rolloutFormat.RfI32
        let sortableSetFormat_RfU16 = rolloutFormat.RfU16
        let sortableSetFormat_RfU8 = rolloutFormat.RfU8
        let sortableSetFormat_RfBs64 = rolloutFormat.RfBs64

        let res_RfU8_su =
            getIncompleteSortResults sotmSwitchUses sortableSetFormat_RfU8
            |> Result.ExtractOrThrow

        let refinedSet_RfU8_su =
            res_RfU8_su
            |> SorterOpOutput.getRefinedSortableSet
            |> Result.ExtractOrThrow
            |> Rollout.toBoolArrays
            |> Seq.toArray

        let res_RfU16_su =
            getIncompleteSortResults sotmSwitchUses sortableSetFormat_RfU16
            |> Result.ExtractOrThrow

        let refinedSet_RfU16_su =
            res_RfU16_su
            |> SorterOpOutput.getRefinedSortableSet
            |> Result.ExtractOrThrow
            |> Rollout.toBoolArrays
            |> Seq.toArray

        Assert.IsTrue(CollectionProps.areEqual refinedSet_RfU8_su refinedSet_RfU16_su)

        let res_RfU64_su =
            getIncompleteSortResults sotmSwitchUses sortableSetFormat_RfBs64
            |> Result.ExtractOrThrow

        let refinedSet_RfU64_su =
            res_RfU64_su
            |> SorterOpOutput.getRefinedSortableSet
            |> Result.ExtractOrThrow
            |> Rollout.toBoolArrays
            |> Seq.toArray

        Assert.IsTrue(CollectionProps.areEqual refinedSet_RfU8_su refinedSet_RfU64_su)



        //let res_RfU8_su =  getIncompleteSortResults sotmSwitchUses sortableSetFormat_RfU8
        //let refinedSet_RfU8_su =
        //res_RfU8_su |> SorterOpOutput.getRefinedSortableSet

        Assert.IsTrue(true)
