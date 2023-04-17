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


    [<TestMethod>]
    member this.enumNchooseM() =
        let n = 8
        let m = 5
        let res = Combinatorics.enumNchooseM n m |> Seq.map (List.toArray) |> Seq.toList
        Assert.IsTrue(res |> Seq.forall (CollectionProps.isSorted))
        Assert.AreEqual(res.Length, 56)

