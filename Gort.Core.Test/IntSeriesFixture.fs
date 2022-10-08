namespace Gort.Core.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type IntSeriesFixture() =

    [<TestMethod>]
    member this.IntSequence_expoB() =
        let aMax = 100
        let yab = Array.init aMax (fun dex -> (dex, IntSeries.expoB 5.0 dex))
        Assert.AreEqual(1, 1)

    [<TestMethod>]
    member this.IntSequence_logTics() =
        let ticsPerLog = 5.5
        let endVal = 1000
        let away = IntSeries.logTics ticsPerLog endVal |> Seq.toArray
        Assert.IsTrue(away.Length > 0)

    [<TestMethod>]
    member this.TestMethodPassing() = Assert.IsTrue(true)
