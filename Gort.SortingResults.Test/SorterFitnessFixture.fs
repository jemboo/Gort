namespace Gort.SortingResults.Test
open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type SorterSetReportingFixture() =


    [<TestMethod>]
    member this.sorterSpeedIndexes() =
        let mutable i = 0
        let maxW = 60
        let rndy = Rando.fromRngGen (RngGen.createLcg (4213 |> RandomSeed.create))

        while i < 100 do
            let switchCt = maxW |> (%) rndy.NextPositiveInt |> SwitchCount.create

            let stageCt =
                (switchCt |> SwitchCount.value |> (+) 1)
                |> (%) rndy.NextPositiveInt
                |> StageCount.create

            let sorterSpeedBn = SorterSpeed.create switchCt stageCt

            let sorterSpeedBnIndex = SorterSpeed.toIndex sorterSpeedBn

            let sorterSpeedBnBack = SorterSpeed.fromIndex sorterSpeedBnIndex
            Assert.AreEqual(sorterSpeedBn, sorterSpeedBnBack)
            i <- i + 1



    [<TestMethod>]
    member this.getBins() =
        let sorterSetId = Guid.NewGuid() |> SorterSetId.create
        let useParalll = true |> UseParallel.create
        let ordr = 16 |> Order.createNr
        let switchCt = SwitchCount.orderTo900SwitchCount ordr
        let sorterCt = SorterCount.create 500
        let rnGn = RngGen.createLcg (123 |> RandomSeed.create)

        let sorterSt = SorterSet.createRandomSwitches sorterSetId sorterCt ordr [||] switchCt rnGn
        let rolloutFormt = rolloutFormat.RfBs64
        let sortableStId = SortableSetId.create 123

        let sortableSt =
            SortableSet.makeAllBits sortableStId rolloutFormt ordr |> Result.ExtractOrThrow

        let sorterEvls, errs =
            SorterSetEval.eval sorterPerfEvalMode.DontCheckSuccess sortableSt sorterSt useParalll


        let sorterPhenotypeBins = sorterEvls 
                                    |> SorterPhenotypeBin.fromSorterEvals
                                    |> Seq.sortBy(SorterPhenotypeBin.getSorterPhenotypeId)
                                    |> Seq.toList

        let sorterSpeedBins = sorterEvls 
                                    |> SorterSpeedBin.fromSorterEvals
                                    |> Seq.toArray

        let sorterPhenotypeBinsBack = 
                            sorterSpeedBins 
                                    |> SorterSpeedBin.toSorterPhenotypeBins
                                    |> Seq.sortBy(SorterPhenotypeBin.getSorterPhenotypeId)
                                    |> Seq.toList

        Assert.IsTrue(CollectionProps.areEqual sorterPhenotypeBins sorterPhenotypeBinsBack)

        let standings = sorterSpeedBins |> SorterPopulationContext.fromSorterSpeedBins

        let stageWgth = 0.2 |> StageWeight.create
        let ranker ss = ss |> SorterFitness.fromSpeed stageWgth
        let ranky = sorterSpeedBins 
                        |> SorterSpeedBin.orderSorterPhenotypesBySliceAndSpeed ranker
                        |> Seq.toArray
        let bestRanks = ranky |> Seq.take(10) |> Seq.map(fst)
        let testIds = seq { Guid.NewGuid() |> SorterId.create } |> Seq.append bestRanks

        let winningSorters = sorterSt 
                                |> SorterSet.getSortersById (20 |> SorterCount.create) testIds
                                |> Seq.toArray
        Assert.AreEqual(standings.Length, sorterCt |> SorterCount.value)
