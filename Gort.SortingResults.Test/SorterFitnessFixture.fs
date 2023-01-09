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
        let sorterCt = SorterCount.create 50
        let rndGn = RngGen.createLcg (123 |> RandomSeed.create)
        let randy = rndGn |> Rando.fromRngGen

        let sorterSt = SorterSet.createRandomSwitches sorterSetId sorterCt ordr [||] switchCt rndGn
        let rolloutFormt = rolloutFormat.RfBs64
        let sortableStId = SortableSetId.create 123

        let sortableSt =
            SortableSet.makeAllBits sortableStId rolloutFormt ordr |> Result.ExtractOrThrow

        let sorterEvls =
            SorterSetEval.evalSorters 
                sorterEvalMode.DontCheckSuccess 
                sortableSt 
                (sorterSt |> SorterSet.getSorters) 
                useParalll


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

        let stageWgt = 0.2 |> StageWeight.create
        let ranker ss = ss |> SorterFitness.fromSpeed stageWgt
        let ranky = sorterSpeedBins 
                        |> SorterSpeedBin.orderSorterPhenotypesBySliceAndSpeed ranker
                        |> Seq.toArray
        let bestSorterIds = ranky |> Seq.take(10) |> Seq.map(fst) |> Seq.toArray

        let winningSorters = sorterSt 
                                |> SorterSet.getSortersById (20 |> SorterCount.create) bestSorterIds
                                |> Seq.toArray
                                
        let mutationRate = MutationRate.create 0.05
        let sorterMutator = SorterUniformMutator.create 
                             None None sorterUniformMutatorType.Switch mutationRate

        let sorterSetMut = SorterSet.createMutationSet 
                                winningSorters sorterCt ordr sorterMutator sorterSetId randy
       
        let sorterEvls2 =
            SorterSetEval.evalSorters 
                sorterEvalMode.DontCheckSuccess 
                sortableSt 
                (sorterSetMut |> SorterSet.getSorters) 
                useParalll


        let sorterSpeedBins2 = sorterEvls2
                                    |> SorterSpeedBin.fromSorterEvals
                                    |> Seq.toArray

        let ranky2 = sorterSpeedBins2 
                        |> SorterSpeedBin.orderSorterPhenotypesBySliceAndSpeed ranker
                        |> Seq.toArray
        Assert.AreEqual(standings.Length, sorterCt |> SorterCount.value)





    [<TestMethod>]
    member this.evo() =
        let sorterSetId = Guid.NewGuid() |> SorterSetId.create
        let useParalll = true |> UseParallel.create
        let ordr = 16 |> Order.createNr
        let switchCt = SwitchCount.orderTo900SwitchCount ordr
        let sorterCt = SorterCount.create 500
        let rndGn = RngGen.createLcg (123 |> RandomSeed.create)
        let randy = rndGn |> Rando.fromRngGen
        let rolloutFormt = rolloutFormat.RfBs64
        let sortableStId = SortableSetId.create 123
        let stageWgt = 0.2 |> StageWeight.create
        let ranker ss = ss |> SorterFitness.fromSpeed stageWgt
        let mutationRate = MutationRate.create 0.01
        let sorterMutator = SorterUniformMutator.create 
                                 None None sorterUniformMutatorType.Switch mutationRate

        let sortableSt =
            SortableSet.makeAllBits sortableStId rolloutFormt ordr |> Result.ExtractOrThrow

        let mutable sorterSt = SorterSet.createRandomSwitches sorterSetId sorterCt ordr [||] switchCt rndGn

        let _repo (gen:int) (sorterSpeedBins:sorterSpeedBin array) = 
            let phenoBins = sorterSpeedBins 
                            |> SorterSpeedBin.toSorterPhenotypeBins
                            |> Seq.toArray

            let mutable dex = 0
            while dex < phenoBins.Length do
                Console.WriteLine (phenoBins.[dex] |> SorterPhenotypeBin.report (gen.ToString()))
                dex <- dex + 1


        let _doGen (gen:int) (sst:sorterSet) =
            let sorterEvls =
                SorterSetEval.evalSorters 
                    sorterEvalMode.DontCheckSuccess 
                    sortableSt 
                    (sst |> SorterSet.getSorters)
                    useParalll

            let sorterSpeedBins = sorterEvls 
                                        |> SorterSpeedBin.fromSorterEvals
                                        |> Seq.toArray
            if gen % 200 = 0 then
                _repo gen sorterSpeedBins

            let ranky = sorterSpeedBins 
                            |> SorterSpeedBin.orderSorterPhenotypesBySliceAndSpeed ranker
                            |> Seq.toArray
            let bestSorterIds = ranky |> Seq.take(400) |> Seq.map(fst) |> Seq.toArray

            let winningSorters = sst 
                                    |> SorterSet.getSortersById (400 |> SorterCount.create) bestSorterIds
                                    |> Seq.toArray
            SorterSet.createMutationSet 
                      winningSorters sorterCt ordr sorterMutator sorterSetId randy
       

        //let ss1 = _doGen 0 sorterSt
        //let ss2 = _doGen 1 ss1
        //let ss3 = _doGen 2 ss2
        //let ss4 = _doGen 3 ss3
        //let ss5 = _doGen 4 ss4
        //let ss6 = _doGen 5 ss5
        //let ss7 = _doGen 6 ss6




        let mutable gen = 0
        while gen < 2 do
            sorterSt <- _doGen gen sorterSt
            gen <- gen + 1
            
        Assert.AreEqual(1,1)