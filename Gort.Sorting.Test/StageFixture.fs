namespace Gort.Sorting.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type StageFixture () =

    let fitIn (bins:int list list) (wha:int) = 
        match bins with
        | [] -> [[wha]]
        | h::t -> 
            if h.Length < 5 then
                (wha::h)::t
            else  [wha]::h::t

    let fitEmAll (tems:int seq) = 
        let mutable fits = []
        let temEnumer = tems.GetEnumerator()
        while temEnumer.MoveNext() do
            fits <- fitIn fits temEnumer.Current
        fits

    let placeIn (bins:int list list) (wha:int) = 
        match bins with
        | [] -> [[wha]]
        | h::t -> 
            if h.Length < 5 then
                (wha::h)::t
            else  [wha]::h::t

    let placeEmAll (tems:int seq) = 
        let mutable fits = []
        let temEnumer = tems.GetEnumerator()
        while temEnumer.MoveNext() do
            fits <- placeIn fits temEnumer.Current
        fits



    [<TestMethod>]
    member this.placeEmAllT () =
        let l = [1 .. 50]
        let c = l |> placeEmAll
        Assert.IsTrue(true);


    [<TestMethod>]
    member this.fitEmAllT () =
        let l = [1 .. 50]
        let c = l |> fitEmAll
        Assert.IsTrue(true);


    [<TestMethod>]
    member this.addStageIndexes () =
        let deg = Degree.createNr 32
        let l = [1 .. 200]
        let newRes = l |> Switch.fromIndexesNonDeg 
                  |> StageCover.addStageIndexes
                  |> Seq.groupBy(snd)
                  |> Seq.toArray

        let oldRes = l |> Switch.fromIndexesNonDeg 
                   |> Stage.fromSwitches deg
                   |> Seq.toArray


        Assert.IsTrue(newRes.Length < oldRes.Length);
