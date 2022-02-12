namespace Gort.Core.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type CommonTypesFixture () =

    [<TestMethod>]
    member this.Degree_maxSwitchesPerStage() =
        let degree = Degree.createNr 7
        let msw = 3
        let cc = Degree.maxSwitchesPerStage degree
        Assert.AreEqual(msw, cc)
    
    [<TestMethod>]
    member this.Degree_reflect() =
        let degree7 = Degree.createNr 7
        let degree6 = Degree.createNr 6
        let d7r3 = 3 |> Degree.reflect degree7
        let d7r2 = 2 |> Degree.reflect degree7
        let d6r3 = 3 |> Degree.reflect degree6
        let d6r2 = 2 |> Degree.reflect degree6

        Assert.AreEqual(d7r3, 3)
        Assert.AreEqual(d7r2, 4)
        Assert.AreEqual(d6r3, 2)
        Assert.AreEqual(d6r2, 3)



    [<TestMethod>]
    member this.TestMethodPassing () =
        let dg = Degree.createNr 8
        let sA = Degree.twoSymbolOrderedArray dg 6 1us 0us
        let sAs = Degree.allTwoSymbolOrderedArrays dg 1us 0us
                    |> Seq.toArray
        Assert.AreEqual(sA.Length, (Degree.value dg));
        Assert.AreEqual(sAs.Length, (Degree.value dg) + 1);
