namespace Gort.SortingResults.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type SortingEvalFixture () =

    let getResultsFromAllBits 
            (sorterOpTrackMode:sorterOpTrackMode)
            (rolloutFormat:rolloutFormat) =
        let order = Order.create 8 |> Result.ExtractOrThrow
        let sortableSetId = 123 |> SortableSetId.create
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

        let sorterOpOutput = 
            SortingRollout.makeSorterOpOutput
                                sorterOpTrackMode
                                sortableSet
                                goodSorter
            |> Result.ExtractOrThrow
 
        goodSorter, sorterOpOutput


    let getResultsOfRandomPermutations 
            (sorterOpTrackMode:sorterOpTrackMode)
            (rolloutFormat:rolloutFormat) =
        let order = Order.create 16 |> Result.ExtractOrThrow
        let sortableSetId = 123 |> SortableSetId.create
        let switchCount = SwitchCount.orderToRecordSwitchCount order
        let sortableCount = 4000 |> SortableCount.create
        
        let rando = Rando.create 
                        rngType.Lcg
                        (1233 |> RandomSeed.create)
        let failingSorter = 
            Sorter.randomSwitches 
                order
                (Seq.empty)
                switchCount
                rando

        let sortableSet = 
            SortableSet.makeRandomPermutation
                                sortableSetId
                                rolloutFormat
                                order
                                sortableCount
                                rando
            |> Result.ExtractOrThrow


        let sorterOpOutput = 
            SortingRollout.makeSorterOpOutput
                                sorterOpTrackMode
                                sortableSet
                                failingSorter
            |> Result.ExtractOrThrow
 
        failingSorter, sorterOpOutput



    [<TestMethod>]
    member this.switchUseCounters() =
        let sotmSwitchUses = sorterOpTrackMode.SwitchUses
        let sotmSwitchTrack = sorterOpTrackMode.SwitchTrack
         
        let sortableSetFormat_RfI32 = rolloutFormat.RfI32
        let sortableSetFormat_RfU16 = rolloutFormat.RfU16 
        let sortableSetFormat_RfU8 = rolloutFormat.RfU8 
        let sortableSetFormat_RfBs64 = rolloutFormat.RfBs64

        let srtr, res_RfU8_su =  getResultsFromAllBits sotmSwitchUses sortableSetFormat_RfU8
        Assert.IsTrue(res_RfU8_su |> SorterOpOutput.isSorted);
        let sot_RfU8_su = res_RfU8_su |> SorterOpOutput.getSorterOpTracker
        let suCt_RfU8_su = sot_RfU8_su |> SwitchUseCounters.fromSorterOpTracker
        let switches_RfU8_su = suCt_RfU8_su |> SwitchUseCounters.getUsedSwitchesFromSorter srtr

        let srtr, res_RfU16_su =  getResultsFromAllBits sotmSwitchUses sortableSetFormat_RfU16
        Assert.IsTrue(res_RfU16_su |> SorterOpOutput.isSorted);
        let sot_RfU16_su = res_RfU16_su |> SorterOpOutput.getSorterOpTracker
        let suCt_RfU16_su = sot_RfU16_su |> SwitchUseCounters.fromSorterOpTracker
        let switches_RfU16_su = suCt_RfU16_su |> SwitchUseCounters.getUsedSwitchesFromSorter srtr
        Assert.IsTrue(CollectionProps.areEqual switches_RfU8_su switches_RfU16_su)


        let srtr, res_RfI32_su =  getResultsFromAllBits sotmSwitchUses sortableSetFormat_RfI32
        Assert.IsTrue(res_RfI32_su |> SorterOpOutput.isSorted);
        let sot_RfI32_su = res_RfI32_su |> SorterOpOutput.getSorterOpTracker
        let suCt_RfI32_su = sot_RfI32_su |> SwitchUseCounters.fromSorterOpTracker
        let switches_RfI32_su = suCt_RfI32_su |> SwitchUseCounters.getUsedSwitchesFromSorter srtr
        Assert.IsTrue(CollectionProps.areEqual switches_RfU8_su switches_RfI32_su)


        let srtr, res_RfBs64_su =  getResultsFromAllBits sotmSwitchUses sortableSetFormat_RfBs64
        Assert.IsTrue(res_RfBs64_su |> SorterOpOutput.isSorted);
        let sot_RfBs64_su = res_RfBs64_su |> SorterOpOutput.getSorterOpTracker
        let suCt_RfBs64_su = sot_RfBs64_su |> SwitchUseCounters.fromSorterOpTracker
        let switches_RfBs64_su = suCt_RfBs64_su |> SwitchUseCounters.getUsedSwitchesFromSorter srtr
        Assert.IsTrue(CollectionProps.areEqual switches_RfU8_su switches_RfBs64_su)
        Assert.AreEqual(1, 1);


    [<TestMethod>]
    member this.refineSortableSetOfPermutations() =
        let sotmSwitchUses = sorterOpTrackMode.SwitchUses
        let sotmSwitchTrack = sorterOpTrackMode.SwitchTrack
         
        let sortableSetFormat_RfI32 = rolloutFormat.RfI32
        let sortableSetFormat_RfU16 = rolloutFormat.RfU16 
        let sortableSetFormat_RfU8 = rolloutFormat.RfU8 
        let sortableSetFormat_RfBs64 = rolloutFormat.RfBs64

        let sortr, res_RfU8_su = 
            getResultsOfRandomPermutations sotmSwitchUses sortableSetFormat_RfU8

        let origSet_RfU8_su =
            res_RfU8_su 
            |> SorterOpOutput.getSortableSet
            |> SortableSet.getRollout
            |> Rollout.toIntArrays
            |> Seq.toArray

        let refinedSet_RfU8_su =
            res_RfU8_su 
            |> SorterOpOutput.getRefinedSortableSet
            |> Result.ExtractOrThrow
            |> Rollout.toIntArrays
            |> Seq.toArray

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
            |> SorterPhenotypePerf.fromSorterPerfs
            |> Seq.toArray

        let totalSortersInBins = 
            sorterPerfBns
            |> Array.map(fun spb -> spb |> SorterPhenotypePerf.getSorters)
            |> Array.concat
            |> Array.length
                            
        Assert.AreEqual (totalSortersInSorterPrfs, totalSortersInBins)


        let sorterPerfBinReprt = 
            sorterPerfBns 
            |> Array.map(SorterPhenotypePerfsForSpeedBin.fromSorterPerfBin)
            |> Seq.toArray
            
        Assert.AreEqual (totalSortersInSorterPrfs, totalSortersInBins)