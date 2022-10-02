namespace Gort.SortingResults.Test

open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type SorterSetReportingFixture () =


    [<TestMethod>]
    member this.sorterSpeedBins() =
        let mutable i = 0
        let maxW = 60
        let rndy = Rando.fromRngGen (RngGen.createLcg (4213 |> RandomSeed.create))
        while i < 1000 do 
            let switchCt = 
                maxW |> (%) rndy.NextPositiveInt
                     |> SwitchCount.create
            let stageCt = 
                (switchCt |> SwitchCount.value |> (+) 1) 
                |> (%) rndy.NextPositiveInt
                |> StageCount.create

            let sorterSpeedBn = 
                SorterSpeedBin.create
                    switchCt
                    stageCt

            let sorterSpeedBnIndex = 
                SorterSpeedBin.getIndexOfBin sorterSpeedBn

            let sorterSpeedBnBack = 
                SorterSpeedBin.getBinFromIndex sorterSpeedBnIndex
            Assert.AreEqual(sorterSpeedBn, sorterSpeedBnBack);
            i <- i + 1


    [<TestMethod>]
    member this.getBins() =
        let ordr = 16 |> Order.createNr
        let switchCt = SwitchCount.orderTo900SwitchCount ordr
        let sorterCt = SorterCount.create 2000
        let rnGn = RngGen.createLcg (123 |> RandomSeed.create)
        let sorterSt = SorterSet.createRandomSwitches
                        ordr
                        [||]
                        switchCt
                        sorterCt
                        rnGn
        let rolloutFormt = rolloutFormat.RfBs64
        let sortableStId = SortableSetId.create 123
        let sortableSt = SortableSet.makeAllBits 
                            sortableStId 
                            rolloutFormt
                            ordr
                         |> Result.ExtractOrThrow

        let sorterEvalMod = sorterEvalMode.SorterSpeed
        let useParalll = true |> UseParallel.create
        let sorterEvls, errs = 
                SorterSetEval.eval
                        sorterEvalMod
                        sortableSt
                        sorterSt
                        useParalll




        Assert.AreEqual(1, 1);
