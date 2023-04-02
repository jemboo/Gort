namespace Gort.Core.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type CombinatoricsFixture() =

    [<TestMethod>]
    member this.enumerateMwithN() =
        let expectedLen = 20
        let yab = Combinatorics.enumerateMwithN 6 3
                    |> Seq.toArray
        Assert.AreEqual(yab.Length, expectedLen)

