namespace Gort.Config.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting


[<TestClass>]
type TestClass () =

    [<TestMethod>]
    member this.TestIDsAreTheSame () =
        let order = 16 |> Order.createNr
        let orderA = 16 |> Order.createNr
        let rngenCreate = 126 |> RandomSeed.create |> RngGen.createLcg
        let rngenCreateA = 126 |> RandomSeed.create |> RngGen.createLcg
        let switchCt = 10 |> SwitchCount.create
        let sorterCtCreate = 100 |> SorterCount.create
        let rngenMutate = 126 |> RandomSeed.create |> RngGen.createLcg
        let sorterCtMutate = 100 |> SorterCount.create
        let mutationRate = 0.5 |> MutationRate.create

        let ssmfrc1 = SorterSetMutatedFromRndCfg.create
                        order
                        rngenCreate
                        switchGenMode.Stage
                        switchCt
                        sorterCtCreate
                        rngenMutate
                        sorterCtMutate
                        mutationRate

        let ssmfrc2 = SorterSetMutatedFromRndCfg.create
                        order
                        rngenCreate
                        switchGenMode.Stage
                        switchCt
                        sorterCtCreate
                        rngenMutate
                        sorterCtMutate
                        mutationRate


        let ssId1 = ssmfrc1 |> SorterSetMutatedFromRndCfg.getId
        let ssId2 = ssmfrc2 |> SorterSetMutatedFromRndCfg.getId

        Assert.AreEqual(ssId1, ssId2)


        let ssmfrcA = SorterSetMutatedFromRndCfg.create
                        orderA
                        rngenCreateA
                        switchGenMode.Stage
                        switchCt
                        sorterCtCreate
                        rngenMutate
                        sorterCtMutate
                        mutationRate



        let ssIdA = ssmfrcA |> SorterSetMutatedFromRndCfg.getId
        Assert.AreEqual(ssId1, ssIdA)



        let rngenCreateB = 1265 |> RandomSeed.create |> RngGen.createLcg

        let ssmfrcB = SorterSetMutatedFromRndCfg.create
                        orderA
                        rngenCreateB
                        switchGenMode.Stage
                        switchCt
                        sorterCtCreate
                        rngenMutate
                        sorterCtMutate
                        mutationRate


        let ssIdB = ssmfrcB |> SorterSetMutatedFromRndCfg.getId


        Assert.IsTrue(true);
