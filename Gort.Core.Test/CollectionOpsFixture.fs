namespace Gort.Core.Test

open System
open SysExt
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type CollectionOpsFixture () =

    [<TestMethod>]
    member this.arrayProductInt() =
        let aA = [|2; 0; 4; 1; 3;|]
        let aB = [|4; 3; 1; 2; 0;|]
        let aABexp = [|3; 1; 0; 4; 2;|]
        let aBAexp = [|1; 4; 0; 3; 2;|]
        let aABact = [|0; 0; 0; 0; 0;|]
        let aBAact = [|0; 0; 0; 0; 0;|]
        CollectionOps.arrayProductInt aA aB aABact
        CollectionOps.arrayProductInt  aB aA aBAact
        let abExp = aABexp |> Array.toList
        let baExp = aBAexp |> Array.toList
        let abAct = aABact |> Array.toList
        let baAct = aBAact |> Array.toList
        Assert.AreEqual(abExp, abAct)
        Assert.AreEqual(baExp, baAct)


    [<TestMethod>]
    member this.arrayProductInt16() =
        let aA = [|2us; 0us; 4us; 1us; 3us;|]
        let aB = [|4us; 3us; 1us; 2us; 0us;|]
        let aABexp = [|3us; 1us; 0us; 4us; 2us;|]
       // let aBAexp = [|1; 4; 0; 3; 2;|]
        let aABactA = [|3us; 1us; 0us; 4us; 2us;|]
        let aABact = [|3us; 1us; 0us; 4us; 2us;|]
       // let aBAact = [|0; 0; 0; 0; 0;|]
        let aABact = CollectionOps.arrayProduct16 aA aB aABact
        //Comby.arrayProduct16 aB aA aBAact
        let abExp = aABexp |> Array.toList
        //let baExp = aBAexp |> Array.toList
        let abAct = aABact |> Array.toList
        //let baAct = aBAact |> Array.toList
        Assert.AreEqual(abExp, abAct)
       // Assert.AreEqual(baExp, baAct)



    [<TestMethod>]
    member this.filterByPickList () =
        let data = [|0uL; 1uL; 2uL; 3uL; 4uL; 5uL;|]
        let picks = [|true; false; true; true; false; true;|]
        let expected = [|0uL; 2uL; 3uL; 5uL;|]
        let filtered = CollectionOps.filterByPickList data picks
                        |> Result.ExtractOrThrow
        Assert.AreEqual(expected |> Array.toList, filtered |> Array.toList);


    [<TestMethod>]
    member this.inverseMapArray() =
        let degree = Degree.createNr 8 
        let randy = Rando.fromRngGen (RngGen.lcgFromNow())
        let mutable i = 0
        while i<100 do
            let bloke = RndGen.randomPermutation randy degree
            let inv = CollectionOps.invertArray bloke (Array.zeroCreate (Degree.value degree))
                        |> Result.ExtractOrThrow
            let prod = CollectionOps.arrayProductIntR bloke inv (Array.zeroCreate bloke.Length)
                        |> Result.ExtractOrThrow
            Assert.IsTrue((prod = (CollectionProps.identity (Degree.value degree))))
            i <- i+1


    [<TestMethod>]
    member this.conjIntArrays0() =
        let degree = Degree.createNr 8 
        let randy = Rando.fromRngGen (RngGen.lcgFromNow())
        let mutable i = 0
        while i<10 do
            let conjer = RndGen.randomPermutation randy degree
            let core1 = RndGen.randomPermutation randy degree
            let core2 = RndGen.randomPermutation randy degree
            let coreProd = CollectionOps.arrayProductIntR core1 core2 (Array.zeroCreate core1.Length)
                            |> Result.ExtractOrThrow 

            let conj1 = core1 |> CollectionOps.conjIntArraysR conjer  
                              |> Result.ExtractOrThrow
            let conj2 = core2 |> CollectionOps.conjIntArraysR conjer 
                              |> Result.ExtractOrThrow

            let prodOfConj = CollectionOps.arrayProductIntR conj1 conj2 (Array.zeroCreate conj1.Length)
                                |> Result.ExtractOrThrow
                                |> Array.toList
            let coreProdConj = coreProd |> CollectionOps.conjIntArraysR conjer
                                        |> Result.ExtractOrThrow
                                        |> Array.toList
            Assert.IsTrue((coreProdConj=prodOfConj))
            i <- i+1


    [<TestMethod>]
    member this.conjIntArrays() =
        let degree = Degree.createNr 16
        let randy = Rando.fromRngGen (RngGen.lcgFromNow())
        let mutable i = 0
        while i<10 do
            let conjer = RndGen.randomPermutation randy degree
            let core1 = RndGen.randomPermutation randy degree
            let core2 = RndGen.randomPermutation randy degree
            let coreProd = CollectionOps.arrayProductIntR core1 core2  (Array.zeroCreate core1.Length)
                            |> Result.ExtractOrThrow 

            let conj1 = core1 |> CollectionOps.conjIntArrays conjer  
                                |> Result.ExtractOrThrow
            let conj2 = core2 |> CollectionOps.conjIntArrays conjer 
                                |> Result.ExtractOrThrow

            let prodOfConj = CollectionOps.arrayProductIntR conj1 conj2  (Array.zeroCreate conj1.Length)
                                |> Result.ExtractOrThrow
                                |> Array.toList
            let coreProdConj = coreProd |> CollectionOps.conjIntArrays conjer
                                        |> Result.ExtractOrThrow
                                        |> Array.toList
            Assert.IsTrue((coreProdConj=prodOfConj))
            i <- i+1


    [<TestMethod>]
    member this.allPowers () =
        let tc =  [|1;2;3;4;5;6;0|]
        let orbit = CollectionOps.allPowers tc |> Seq.toArray
        Assert.AreEqual(orbit.Length, 7);


    [<TestMethod>]
    member this.takeUpto () =
        let a1 = [|1;2;3;4;5;6;7;8|]
        let yab = a1 |> CollectionOps.takeUpto 3 |> Seq.toArray
        let zab = a1 |> CollectionOps.takeUpto 30 |> Seq.toArray
        Assert.IsTrue(yab.Length = 3)
        Assert.IsTrue(zab.Length = 8)



