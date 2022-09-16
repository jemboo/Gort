namespace Gort.SortingResults.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type SortingEvalFixture () =

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

        

    [<TestMethod>]
    member this.SorterPerfBinReport_fromSorterPerfBins() =
        let order = 8 |> Order.createNr
        let sortrA = RefSorter.goodRefSorterForOrder order
                       |> Result.ExtractOrThrow


        let successflA = true
        let unSuccessflA = false
        let switchCtA = 11 |> SwitchCount.create
        let switchCtB = 12 |> SwitchCount.create
        let switchCtC = 13 |> SwitchCount.create
        let switchCtD = 14 |> SwitchCount.create
        let switchCtE = 15 |> SwitchCount.create

        let stageCtA = 1 |> StageCount.create
        let stageCtB = 2 |> StageCount.create
        let stageCtC = 3 |> StageCount.create

        let switchSeqA = [|1;2;3;4;5|] 
                         |> Switch.fromSwitchIndexes
                         |> Seq.toArray

        let switchSeqB = [|11;12;13;14;15|] 
                         |> Switch.fromSwitchIndexes
                         |> Seq.toArray

        let switchSeqC = [|21;22;23;24;25|] 
                         |> Switch.fromSwitchIndexes
                         |> Seq.toArray

        let switchSeqD = [|31;12;13;14;15|] 
                         |> Switch.fromSwitchIndexes
                         |> Seq.toArray

        let switchSeqE = [|41;22;23;24;25|] 
                         |> Switch.fromSwitchIndexes
                         |> Seq.toArray


        let srtrPhenoTypeIdA = SorterPhenotypeId.create switchSeqA
        let srtrPhenoTypeIdB = SorterPhenotypeId.create switchSeqB
        let srtrPhenoTypeIdC = SorterPhenotypeId.create switchSeqC
        let srtrPhenoTypeIdD = SorterPhenotypeId.create switchSeqD
        let srtrPhenoTypeIdE = SorterPhenotypeId.create switchSeqE


        let sorterPrfAAA1 = 
            SorterPerf.make 
                switchCtA stageCtA successflA srtrPhenoTypeIdA sortrA

        let sorterPrfAAA2 = 
            SorterPerf.make 
                switchCtA stageCtA successflA srtrPhenoTypeIdA sortrA

        let sorterPrfABB = 
            SorterPerf.make 
                switchCtA stageCtB successflA srtrPhenoTypeIdB sortrA

        let sorterPrfABC = 
            SorterPerf.make 
                switchCtA stageCtB successflA srtrPhenoTypeIdC sortrA

        let sorterPrfBAD = 
            SorterPerf.make 
                switchCtB stageCtA successflA srtrPhenoTypeIdD sortrA

        let sorterPrfBAE = 
            SorterPerf.make 
                switchCtB stageCtA successflA srtrPhenoTypeIdE sortrA

        let sorterPrfs = seq { sorterPrfAAA1; sorterPrfAAA2; sorterPrfABB;
            sorterPrfABC; sorterPrfBAD; sorterPrfBAE; }

        let totalSortersInSorterPrfs = 
            sorterPrfs |> Seq.length
            
        let sorterPerfBns = 
            sorterPrfs
            |> SorterPerfBin.fromSorterPerfs
            |> Seq.toArray

        let totalSortersInBins = 
            sorterPerfBns
            |> Array.map(fun spb -> spb |> SorterPerfBin.getSorters)
            |> Array.concat
            |> Array.length
                            
        Assert.AreEqual (totalSortersInSorterPrfs, totalSortersInBins)


        let sorterPerfBinReprt = 
            sorterPerfBns 
            |> SorterPerfBinReport.fromSorterPerfBins
            |> Seq.toArray
            
        Assert.AreEqual (totalSortersInSorterPrfs, totalSortersInBins)