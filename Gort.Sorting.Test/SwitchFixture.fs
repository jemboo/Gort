namespace Gort.Sorting.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open System.Diagnostics

[<TestClass>]
type SwitchFixture() =

    [<TestMethod>]
    member this.switchMap() =
        let rndy = Rando.fromRngGen (RngGen.lcgFromNow ())
        let sw1 = {switch.low = 21; switch.hi = 21} // = 252
        let sw2 = {switch.low = 360; switch.hi = 360} // = 65340
        let sw1Dex = Switch.toIndex sw2
        let dd = sw1Dex |> uint16
        for i = 0 to 100 do
            let swDex = rndy.NextPositiveInt % 100000
            let swtch = Switch.fromIndex swDex
            let swDexBack = Switch.toIndex swtch
            Assert.AreEqual(swDex, swDexBack)


    [<TestMethod>]
    member this.lowOverlapping() =
        let degSrc = Order.createNr 16
        let rndy = Rando.fromRngGen (RngGen.lcgFromNow ())

        let switches =
            Switch.rndNonDegenSwitchesOfDegree degSrc rndy 
            |> Seq.take 10 
            |> Seq.toArray

        for i = 0 to switches.Length - 1 do
            let sw = switches.[i]

            let lowFriends =
                Switch.lowOverlapping degSrc sw.low |> Switch.fromSwitchIndexes |> Seq.toArray

            for j = 0 to lowFriends.Length - 1 do
                let fr = lowFriends.[j]
                Assert.AreEqual(lowFriends.[j].low, sw.low)


    [<TestMethod>]
    member this.hiOverlapping() =
        let degSrc = Order.createNr 16
        let rndy = Rando.fromRngGen (RngGen.lcgFromNow ())

        let switches =
            Switch.rndNonDegenSwitchesOfDegree degSrc rndy 
                |> Seq.take 10 
                |> Seq.toArray

        for i = 0 to switches.Length - 1 do
            let sw = switches.[i]

            let hiFriends =
                Switch.hiOverlapping degSrc sw.hi 
                    |> Switch.fromSwitchIndexes |> Seq.toArray

            for j = 0 to hiFriends.Length - 1 do
                let fr = hiFriends.[j]
                Assert.AreEqual(hiFriends.[j].hi, sw.hi)


    [<TestMethod>]
    member this.allMasks() =
        let degSrc = Order.createNr 16
        let degDest = Order.createNr 12
        let srtGreen = RefSorter.createRefSorter RefSorter.End16 |> Result.ExtractOrThrow

        let subSorters =
            Switch.allMasks degSrc degDest (srtGreen |> Sorter.getSwitches) |> Seq.toArray

        let hist = subSorters |> CollectionOps.histogram (fun a -> a.Length)

        hist
        |> Map.toSeq
        |> Seq.iter (fun (k, v) -> Debug.WriteLine(sprintf "%d\t%d" k v))

        Assert.IsTrue(hist.Count > 0)


    [<TestMethod>]
    member this.rndMasks() =
        let rndy = Rando.fromRngGen (RngGen.lcgFromNow ())
        let degSrc = Order.createNr 16
        let degDest = Order.createNr 12
        let srtGreen = RefSorter.createRefSorter RefSorter.End16 |> Result.ExtractOrThrow

        let subSorters =
            Switch.rndMasks degSrc degDest (srtGreen |> Sorter.getSwitches) rndy
            |> Seq.take (100)
            |> Seq.toArray

        let hist = subSorters |> CollectionOps.histogram (fun a -> a.Length)

        hist
        |> Map.toSeq
        |> Seq.iter (fun (k, v) -> Debug.WriteLine(sprintf "%d\t%d" k v))

        Assert.IsTrue(hist.Count > 0)
