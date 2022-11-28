namespace Gort.Core.Test

open SysExt
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type CommonTypesFixture() =

    [<TestMethod>]
    member this.Degree_maxSwitchesPerStage() =
        let order = Order.createNr 7
        let msw = 3
        let cc = Order.maxSwitchesPerStage order
        Assert.AreEqual(msw, cc)


    [<TestMethod>]
    member this.Degree_reflect() =
        let order7 = Order.createNr 7
        let order6 = Order.createNr 6
        let d7r3 = 3 |> Order.reflect order7
        let d7r2 = 2 |> Order.reflect order7
        let d6r3 = 3 |> Order.reflect order6
        let d6r2 = 2 |> Order.reflect order6

        Assert.AreEqual(d7r3, 3)
        Assert.AreEqual(d7r2, 4)
        Assert.AreEqual(d6r3, 2)
        Assert.AreEqual(d6r2, 3)


    [<TestMethod>]
    member this.TestMethodPassing() =
        let ord = Order.createNr 8
        let sA = Order.twoSymbolOrderedArray ord 6 1us 0us
        let sAs = Order.allTwoSymbolOrderedArrays ord 1us 0us |> Seq.toArray
        Assert.AreEqual(sA.Length, (Order.value ord))
        Assert.AreEqual(sAs.Length, (Order.value ord) + 1)


    [<TestMethod>]
    member this.SymbolCount0ToByteWidth() =
        let sc = 9uL |> SymbolSetSize.createNr
        let bw =
            sc
            |> BitsPerSymbol.fromSymbolSetSize
            |> BitsPerSymbol.value
        Assert.AreEqual(4, bw)

    [<TestMethod>]
    member this.leftmost_index() =
        let randy = Rando.create rngType.Lcg (123 |> RandomSeed.create)
        let mutable i = 0

        while i < 100 do
            let nuRnd = randy.NextULong
            let leftDex = nuRnd.leftmost_index
            let bv = nuRnd.get (leftDex + 0)
            let bvS = nuRnd.get (leftDex + 1)
            Assert.IsTrue(bv)

            if (leftDex < 63) then
                Assert.IsFalse(bvS)
            i <- i + 1

        Assert.AreEqual(3, 3)
