namespace Gort.SortingResults.Test

open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type SorterSetEvalFixture () =

    [<TestMethod>]
    member this.getResults() =
        let ordr = 16 |> Order.createNr
        let switchCt = SwitchCount.orderTo900SwitchCount ordr
        let sorterCt = SorterCount.create 20
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

        let sss = sorterEvls |> Seq.map(fun sev -> SorterSpeed.fromSorterOpOutput )

        Assert.AreEqual(sorterEvls.Length, (sorterCt |> SorterCount.value));


    [<TestMethod>]
    member this.tt() =


        Assert.AreEqual(1, 1);
