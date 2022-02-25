namespace Gort.Sorting.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open System.Diagnostics

[<TestClass>]
type SwitchFixture () =

    [<TestMethod>]
    member this.switchMap() =
        let rndy = Rando.fromRngGen (RngGen.lcgFromNow())
        let _nextSwitch = 
            Switch.switchMap.[rndy.NextPositiveInt % Switch.mapIndexUb]
        for i=0 to 100 do
            let sw = _nextSwitch
            let dex = Switch.getIndex sw
            let swBack = Switch.switchMap.[dex]
            Assert.AreEqual(sw, swBack)


    [<TestMethod>]
    member this.lowOverlapping() =
        let degSrc = Degree.createNr 16
        let rndy = Rando.fromRngGen (RngGen.lcgFromNow())
        let switches = 
            Switch.rndNonDegenSwitchesOfDegree degSrc rndy
            |> Seq.take 10 |> Seq.toArray
        for i=0 to switches.Length - 1 do
            let sw = switches.[i]
            let lowFriends = Switch.lowOverlapping degSrc sw.low
                             |> Switch.fromIndexes
                             |> Seq.toArray
            for j=0 to lowFriends.Length - 1 do
                let fr = lowFriends.[j]
                Assert.AreEqual(lowFriends.[j].low, sw.low)


    [<TestMethod>]
    member this.hiOverlapping() =
        let degSrc = Degree.createNr 16
        let rndy = Rando.fromRngGen (RngGen.lcgFromNow())
        let switches = 
            Switch.rndNonDegenSwitchesOfDegree degSrc rndy
            |> Seq.take 10 |> Seq.toArray
        for i=0 to switches.Length - 1 do
            let sw = switches.[i]
            let hiFriends = Switch.hiOverlapping degSrc sw.hi
                             |> Switch.fromIndexes
                             |> Seq.toArray
            for j=0 to hiFriends.Length - 1 do
                let fr = hiFriends.[j]
                Assert.AreEqual(hiFriends.[j].hi, sw.hi)


    [<TestMethod>]
    member this.allMasks() =
        let degSrc = Degree.createNr 16
        let degDest = Degree.createNr 12
        let srtGreen = RefSorter.createRefSorter RefSorter.End16
                       |> Result.ExtractOrThrow
        let subSorters = Switch.allMasks degSrc degDest srtGreen.switches
                         |> Seq.toArray

        let hist = subSorters |> CollectionOps.histogram (fun a -> a.Length)
        hist |> Map.toSeq |> Seq.iter(fun (k, v) -> Debug.WriteLine (sprintf "%d\t%d" k v) )
        Assert.IsTrue(hist.Count > 0)


    [<TestMethod>]
    member this.rndMasks() =
        let rndy = Rando.fromRngGen (RngGen.lcgFromNow())
        let degSrc = Degree.createNr 16
        let degDest = Degree.createNr 12
        let srtGreen = RefSorter.createRefSorter RefSorter.End16
                       |> Result.ExtractOrThrow
        let subSorters = Switch.rndMasks degSrc degDest srtGreen.switches rndy
                         |> Seq.take(100)
                         |> Seq.toArray

        let hist = subSorters |> CollectionOps.histogram (fun a -> a.Length)
        hist |> Map.toSeq |> Seq.iter(fun (k, v) -> Debug.WriteLine (sprintf "%d\t%d" k v) )
        Assert.IsTrue(hist.Count > 0)