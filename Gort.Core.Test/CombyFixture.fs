namespace Gort.Core.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type CombyFixture () =

    [<TestMethod>]
    member this.toCumulative() =
        let testArray = [|2.0; 3.0; 4.0; 5.0; 6.0; 7.0; 8.0; 9.0; 10.0|]
        let startVal = 3.5
        let expectedResult = [|3.5; 5.5; 8.5; 12.5; 17.5; 23.5; 30.5; 38.5; 47.5; 57.5|]
        let actualResult = Comby.toCumulative startVal testArray
        Assert.AreEqual(expectedResult.[8], actualResult.[8])

    [<TestMethod>]
    member this.inverseMapArray() =
        let degree = Degree.create 8 
        let randy = Rando.fromRngGen (RngGen.lcgFromNow())
        let mutable i = 0
        while i<100 do
            let bloke = RndGen.randomPermutation randy degree
            let inv = Comby.inverseMapArray bloke
                        |> Result.ExtractOrThrow
            let prod = Comby.compIntArrays bloke inv
                        |> Result.ExtractOrThrow
            Assert.IsTrue((prod = (Comby.identity (Degree.value degree))))
            i <- i+1


    [<TestMethod>]
    member this.compIntArraysNr() =
        let aA = [|2; 0; 4; 1; 3;|]
        let aB = [|4; 3; 1; 2; 0;|]
        let aABexp = [|3; 1; 0; 4; 2;|]
        let aBAexp = [|1; 4; 0; 3; 2;|]
        let aABact = [|0; 0; 0; 0; 0;|]
        let aBAact = [|0; 0; 0; 0; 0;|]
        Comby.compIntArraysNr aA aB aABact
        Comby.compIntArraysNr  aB aA aBAact
        let abExp = aABexp |> Array.toList
        let baExp = aBAexp |> Array.toList
        let abAct = aABact |> Array.toList
        let baAct = aBAact |> Array.toList
        Assert.AreEqual(abExp, abAct)
        Assert.AreEqual(baExp, baAct)


    [<TestMethod>]
    member this.conjIntArrays0() =
        let degree = Degree.create 8 
        let randy = Rando.fromRngGen (RngGen.lcgFromNow())
        let mutable i = 0
        while i<10 do
            let conjer = RndGen.randomPermutation randy degree
            let core1 = RndGen.randomPermutation randy degree
            let core2 = RndGen.randomPermutation randy degree
            let coreProd = Comby.compIntArrays core1 core2
                            |> Result.ExtractOrThrow 

            let conj1 = core1 |> Comby.conjIntArraysR conjer  
                              |> Result.ExtractOrThrow
            let conj2 = core2 |> Comby.conjIntArraysR conjer 
                              |> Result.ExtractOrThrow

            let prodOfConj = Comby.compIntArrays conj1 conj2
                                |> Result.ExtractOrThrow
                                |> Array.toList
            let coreProdConj = coreProd |> Comby.conjIntArraysR conjer
                                        |> Result.ExtractOrThrow
                                        |> Array.toList
            Assert.IsTrue((coreProdConj=prodOfConj))
            i <- i+1


    [<TestMethod>]
    member this.conjIntArrays() =
        let degree = Degree.create 16
        let randy = Rando.fromRngGen (RngGen.lcgFromNow())
        let mutable i = 0
        while i<10 do
            let conjer = RndGen.randomPermutation randy degree
            let core1 = RndGen.randomPermutation randy degree
            let core2 = RndGen.randomPermutation randy degree
            let coreProd = Comby.compIntArrays core1 core2
                            |> Result.ExtractOrThrow 

            let conj1 = core1 |> Comby.conjIntArrays conjer  
                                |> Result.ExtractOrThrow
            let conj2 = core2 |> Comby.conjIntArrays conjer 
                                |> Result.ExtractOrThrow

            let prodOfConj = Comby.compIntArrays conj1 conj2
                                |> Result.ExtractOrThrow
                                |> Array.toList
            let coreProdConj = coreProd |> Comby.conjIntArrays conjer
                                        |> Result.ExtractOrThrow
                                        |> Array.toList
            Assert.IsTrue((coreProdConj=prodOfConj))
            i <- i+1


    //[<TestMethod>]
    //member this.conjugateIntArrays_preserves_twoCycle() =
    //    let degree = Degree.create 8
    //    let randy = Rando.fromRngGen (RngGen.lcgFromNow())
    //    let mutable i = 0
    //    while i<10 do
    //        let tc = RndGen.rndFullTwoCycleArray randy (Degree.value degree)
    //        let conjer = RndGen.conjIntArrayWsutation randy degree
    //        let conj = Comby.conjIntArrays tc conjer
    //                    |> Result.ExtractOrThrow
    //        let isTc = Comby.isTwoCycle conj
    //                    |> Result.ExtractOrThrow
    //        Assert.IsTrue(isTc)
    //        i <- i+1


    [<TestMethod>]
    member this.enumNchooseM() =
        let n = 8
        let m = 5
        let res = Comby.enumNchooseM n m 
                    |> Seq.map(List.toArray)
                    |> Seq.toList
        Assert.IsTrue(res |> Seq.forall(Collections.isSorted))
        Assert.AreEqual(res.Length, 56)

    [<TestMethod>]
    member this.TestMethodPassing () =
        Assert.IsTrue(true);