namespace Gort.SortingResults.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type TestClass () =

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
 
        goodSorter, sorterOpResults



    [<TestMethod>]
    member this.switchUseCounters() =
        let sotmSwitchUses = sorterOpTrackMode.SwitchUses
        let sotmSwitchTrack = sorterOpTrackMode.SwitchTrack
         
        let sortableSetFormat_RfI32 = rolloutFormat.RfI32
        let sortableSetFormat_RfU16 = rolloutFormat.RfU16 
        let sortableSetFormat_RfU8 = rolloutFormat.RfU8 
        let sortableSetFormat_RfBs64 = rolloutFormat.RfBs64

        let srtr, res_RfU8_su =  getResults sotmSwitchUses sortableSetFormat_RfU8
        Assert.IsTrue(res_RfU8_su |> SorterOpResults.isSorted);
        let sot_RfU8_su = res_RfU8_su |> SorterOpResults.getSorterOpTracker
        let suCt_RfU8_su = sot_RfU8_su |> SwitchUseCounters.fromSorterOpTracker
        let switches_RfU8_su = suCt_RfU8_su |> SwitchUseCounters.getUsedSwitchesFromSorter srtr

        let srtr, res_RfU16_su =  getResults sotmSwitchUses sortableSetFormat_RfU16
        Assert.IsTrue(res_RfU16_su |> SorterOpResults.isSorted);
        let sot_RfU16_su = res_RfU16_su |> SorterOpResults.getSorterOpTracker
        let suCt_RfU16_su = sot_RfU16_su |> SwitchUseCounters.fromSorterOpTracker
        let switches_RfU16_su = suCt_RfU16_su |> SwitchUseCounters.getUsedSwitchesFromSorter srtr
        Assert.IsTrue(CollectionProps.areEqual switches_RfU8_su switches_RfU16_su)


        let srtr, res_RfBs64_su =  getResults sotmSwitchUses sortableSetFormat_RfBs64
        Assert.IsTrue(res_RfBs64_su |> SorterOpResults.isSorted);
        let sot_RfBs64_su = res_RfBs64_su |> SorterOpResults.getSorterOpTracker
        let suCt_RfBs64_su = sot_RfBs64_su |> SwitchUseCounters.fromSorterOpTracker
        let switches_RfBs64_su = suCt_RfBs64_su |> SwitchUseCounters.getUsedSwitchesFromSorter srtr
        Assert.IsTrue(CollectionProps.areEqual switches_RfU8_su switches_RfBs64_su)



        Assert.AreEqual(1, 1);

