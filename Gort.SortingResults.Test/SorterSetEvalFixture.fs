namespace Gort.SortingResults.Test
open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type SorterSetEvalFixture() =

    [<TestMethod>]
    member this.getResults() =
        let ordr = 16 |> Order.createNr
        let switchCt = SwitchCount.orderTo900SwitchCount ordr
        let sorterCt = SorterCount.create 2000
        let rnGn = RngGen.createLcg (123 |> RandomSeed.create)
        let sorterSetId = Guid.NewGuid() |> SorterSetId.create
        let sorterSt = SorterSet.createRandomSwitches sorterSetId sorterCt ordr [||] switchCt rnGn
        let rolloutFormt = rolloutFormat.RfBs64
        let sortableStId = SortableSetId.create 123

        let sortableSt =
            SortableSet.makeAllBits sortableStId rolloutFormt ordr |> Result.ExtractOrThrow

        let sorterEvalMod = sorterEvalMode.SorterSpeed
        let useParalll = true |> UseParallel.create

        let sorterEvls, errs =
            SorterSetEval.eval sorterEvalMod sortableSt sorterSt useParalll

        Assert.AreEqual(sorterEvls.Length, (sorterCt |> SorterCount.value))


    [<TestMethod>]
    member this.tt() =


        Assert.AreEqual(1, 1)
