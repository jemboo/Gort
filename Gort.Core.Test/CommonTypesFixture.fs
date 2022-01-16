namespace Gort.Core.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type CommonTypesFixture () =

    [<TestMethod>]
    member this.Degree_maxSwitchesPerStage() =
        let degree = Degree.create 7
        let msw = 3
        let cc = Degree.maxSwitchesPerStage degree
        Assert.AreEqual(msw, cc)
    
    [<TestMethod>]
    member this.Degree_reflect() =
        let degree7 = Degree.create 7
        let degree6 = Degree.create 6
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
        Assert.IsTrue(true);
