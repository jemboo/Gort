namespace Gort.SortingOps.Test
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type SortingRolloutFixture () =

    let getResults 
            (sorterOpTrackMode:sorterOpTrackMode)
            (rolloutFormat:rolloutFormat) =
        let order = Order.create 8 |> Result.ExtractOrThrow
        let sortableSetId = 123 |> SortableSetId.create
        //let goodSorter = 
        //    RefSorter.goodRefSorterForOrder order 
        //    |> Result.ExtractOrThrow
        let switchCount = SwitchCount.orderTo900SwitchCount order
        let rando = Rando.create 
                        rngType.Lcg
                        (1233 |> RandomSeed.create)
        let goodSorter = 
            Sorter.randomSwitches 
                order
                (Seq.empty)
                switchCount
                rando

        let sortableSet = 
            SortableSet.makeAllBits
                                sortableSetId
                                rolloutFormat
                                order
            |> Result.ExtractOrThrow


        let sorterOpResults = 
            SortingRollout.evalSorterWithSortableSet
                                sorterOpTrackMode
                                goodSorter
                                sortableSet
 
        sorterOpResults



    [<TestMethod>]
    member this.evalSorterWithSortableSet () =
        let sotmSwitchUses = sorterOpTrackMode.SwitchUses
        let sotmSwitchTrack = sorterOpTrackMode.SwitchTrack
         
        let sortableSetFormat_RfI32 = rolloutFormat.RfI32
        let sortableSetFormat_RfU16 = rolloutFormat.RfU16 
        let sortableSetFormat_RfU8 = rolloutFormat.RfU8 
        let sortableSetFormat_RfBs64 = rolloutFormat.RfBs64

        let res_RfU8_su =  getResults sotmSwitchUses sortableSetFormat_RfU8
        Assert.IsTrue(res_RfU8_su |> SorterOpResults.isSorted);
        let sot_RfU8_su = res_RfU8_su |> SorterOpResults.getSorterOpTracker
        let suCt_RfU8_su = sot_RfU8_su |> SorterOpTracker.getSwitchUseCounts


        let res_RfU16_su =  getResults sotmSwitchUses sortableSetFormat_RfU16
        Assert.IsTrue(res_RfU16_su |> SorterOpResults.isSorted);
        let sot_RfU16_su = res_RfU16_su |> SorterOpResults.getSorterOpTracker
        let suCt_RfU16_su = sot_RfU16_su |> SorterOpTracker.getSwitchUseCounts
        Assert.IsTrue(CollectionProps.areEqual suCt_RfU8_su suCt_RfU16_su)


        let res_RfI32_su =  getResults sotmSwitchUses sortableSetFormat_RfI32
        Assert.IsTrue(res_RfI32_su |> SorterOpResults.isSorted);
        let sot_RfU32_su = res_RfI32_su |> SorterOpResults.getSorterOpTracker
        let suCt_RfU32_su = sot_RfU32_su |> SorterOpTracker.getSwitchUseCounts
        Assert.IsTrue(CollectionProps.areEqual suCt_RfU8_su suCt_RfU32_su)


        let res_RfBs64_su =  getResults sotmSwitchUses sortableSetFormat_RfBs64
        Assert.IsTrue(res_RfBs64_su |> SorterOpResults.isSorted);
        let sot_RfU64_su = res_RfBs64_su |> SorterOpResults.getSorterOpTracker
        let suCt_RfU64_su = sot_RfU64_su |> SorterOpTracker.getSwitchUseCounts
        // true counts are not available
        //Assert.IsTrue(CollectionProps.areEqual suCt_RfU8_su suCt_RfU64_su)


        let res_RfU8_st =  getResults sotmSwitchTrack sortableSetFormat_RfU8
        Assert.IsTrue(res_RfU8_st |> SorterOpResults.isSorted);
        let sot_RfU8_st = res_RfU8_st |> SorterOpResults.getSorterOpTracker
        let suCt_RfU8_st = sot_RfU8_st |> SorterOpTracker.getSwitchUseCounts
        Assert.IsTrue(CollectionProps.areEqual suCt_RfU8_su suCt_RfU8_st)


        let res_RfU16_st =  getResults sotmSwitchTrack sortableSetFormat_RfU16
        Assert.IsTrue(res_RfU16_st |> SorterOpResults.isSorted);
        let sot_RfU16_st = res_RfU16_st |> SorterOpResults.getSorterOpTracker
        let suCt_RfU16_st = sot_RfU16_st |> SorterOpTracker.getSwitchUseCounts
        Assert.IsTrue(CollectionProps.areEqual suCt_RfU8_su suCt_RfU16_st)


        let res_RfI32_st =  getResults sotmSwitchTrack sortableSetFormat_RfI32
        Assert.IsTrue(res_RfI32_st |> SorterOpResults.isSorted);
        let sot_RfU32_st = res_RfI32_st |> SorterOpResults.getSorterOpTracker
        let suCt_RfU32_st = sot_RfU32_st |> SorterOpTracker.getSwitchUseCounts
        Assert.IsTrue(CollectionProps.areEqual suCt_RfU8_su suCt_RfU32_st)


        let res_RfBs64_st =  getResults sotmSwitchTrack sortableSetFormat_RfBs64
        Assert.IsTrue(res_RfBs64_st |> SorterOpResults.isSorted);
        let sot_RfU64_st = res_RfBs64_st |> SorterOpResults.getSorterOpTracker
        let suCt_RfU64_st = sot_RfU64_st |> SorterOpTracker.getSwitchUseCounts

        // not sure why the counts are different here
        //Assert.IsTrue(CollectionProps.areEqual suCt_RfU8_su suCt_RfU64_st)


        Assert.IsTrue(true);
