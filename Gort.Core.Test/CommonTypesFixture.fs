namespace Gort.Core.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type CommonTypesFixture () =

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
    member this.TestMethodPassing () =
        let ord = Order.createNr 8
        let sA = Order.twoSymbolOrderedArray ord 6 1us 0us
        let sAs = Order.allTwoSymbolOrderedArrays ord 1us 0us
                    |> Seq.toArray
        Assert.AreEqual(sA.Length, (Order.value ord));
        Assert.AreEqual(sAs.Length, (Order.value ord) + 1);


    [<TestMethod>]
    member this.SymbolCountToByteWidth () =
        let sc = 9 |> SymbolSetSize.createNr
        let bw = sc |> BitWidth.fromSymbolSetSize
                    |> Result.ExtractOrThrow
                    |> BitWidth.value
        Assert.AreEqual(bw, 3);