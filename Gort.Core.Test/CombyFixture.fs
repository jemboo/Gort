namespace Gort.Core.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type CombyFixture () =


    [<TestMethod>]
    member this.isIdentity() =
        let testArray = [|2; 3; 4; 5;|]
        let testArray2 = [|0; 1; 2; 3; 4; 5;|]
        Assert.IsFalse(Comby.isIdentity testArray)
        Assert.IsTrue(Comby.isIdentity testArray2)


    [<TestMethod>]
    member this.toCumulative() =
        let testArray = [|2.0; 3.0; 4.0; 5.0; 6.0; 7.0; 8.0; 9.0; 10.0|]
        let startVal = 3.5
        let expectedResult = [|3.5; 5.5; 8.5; 12.5; 17.5; 23.5; 30.5; 38.5; 47.5; 57.5|]
        let actualResult = Comby.toCumulative startVal testArray
        Assert.AreEqual(expectedResult.[8], actualResult.[8])


    [<TestMethod>]
    member this.inverseMapArray() =
        let degree = Degree.createNr 8 
        let randy = Rando.fromRngGen (RngGen.lcgFromNow())
        let mutable i = 0
        while i<100 do
            let bloke = RndGen.randomPermutation randy degree
            let inv = Comby.invertArray bloke (Array.zeroCreate (Degree.value degree))
                        |> Result.ExtractOrThrow
            let prod = Comby.arrayProductIntR bloke inv (Array.zeroCreate bloke.Length)
                        |> Result.ExtractOrThrow
            Assert.IsTrue((prod = (Comby.identity (Degree.value degree))))
            i <- i+1


    [<TestMethod>]
    member this.arrayProductInt() =
        let aA = [|2; 0; 4; 1; 3;|]
        let aB = [|4; 3; 1; 2; 0;|]
        let aABexp = [|3; 1; 0; 4; 2;|]
        let aBAexp = [|1; 4; 0; 3; 2;|]
        let aABact = [|0; 0; 0; 0; 0;|]
        let aBAact = [|0; 0; 0; 0; 0;|]
        Comby.arrayProductInt aA aB aABact
        Comby.arrayProductInt  aB aA aBAact
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
        let aABact = Comby.arrayProduct16 aA aB aABact
        //Comby.arrayProduct16 aB aA aBAact
        let abExp = aABexp |> Array.toList
        //let baExp = aBAexp |> Array.toList
        let abAct = aABact |> Array.toList
        //let baAct = aBAact |> Array.toList
        Assert.AreEqual(abExp, abAct)
       // Assert.AreEqual(baExp, baAct)


    [<TestMethod>]
    member this.conjIntArrays0() =
        let degree = Degree.createNr 8 
        let randy = Rando.fromRngGen (RngGen.lcgFromNow())
        let mutable i = 0
        while i<10 do
            let conjer = RndGen.randomPermutation randy degree
            let core1 = RndGen.randomPermutation randy degree
            let core2 = RndGen.randomPermutation randy degree
            let coreProd = Comby.arrayProductIntR core1 core2 (Array.zeroCreate core1.Length)
                            |> Result.ExtractOrThrow 

            let conj1 = core1 |> Comby.conjIntArraysR conjer  
                              |> Result.ExtractOrThrow
            let conj2 = core2 |> Comby.conjIntArraysR conjer 
                              |> Result.ExtractOrThrow

            let prodOfConj = Comby.arrayProductIntR conj1 conj2 (Array.zeroCreate conj1.Length)
                                |> Result.ExtractOrThrow
                                |> Array.toList
            let coreProdConj = coreProd |> Comby.conjIntArraysR conjer
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
            let coreProd = Comby.arrayProductIntR core1 core2  (Array.zeroCreate core1.Length)
                            |> Result.ExtractOrThrow 

            let conj1 = core1 |> Comby.conjIntArrays conjer  
                                |> Result.ExtractOrThrow
            let conj2 = core2 |> Comby.conjIntArrays conjer 
                                |> Result.ExtractOrThrow

            let prodOfConj = Comby.arrayProductIntR conj1 conj2  (Array.zeroCreate conj1.Length)
                                |> Result.ExtractOrThrow
                                |> Array.toList
            let coreProdConj = coreProd |> Comby.conjIntArrays conjer
                                        |> Result.ExtractOrThrow
                                        |> Array.toList
            Assert.IsTrue((coreProdConj=prodOfConj))
            i <- i+1


    [<TestMethod>]
    member this.allPowers () =
        let tc =  [|1;2;3;4;5;6;0|]
        let orbit = Comby.allPowers tc |> Seq.toArray
        Assert.AreEqual(orbit.Length, 7);


    [<TestMethod>]
    member this.isTwoCycle () =
        let tc =  [|0;1;2;3;4;6;5|]
        let ntc = [|0;1;2;3;6;4;5|]
        let tc2 =  [|0;4;2;3;1;6;5|]
        let ntc2 = [|9;1;2;3;6;5;4|]
        let tc3 =  [|0;4;2;3;1;6;5|]
        let ntc3 = [|1;1;2;3;6;5;4|]

        Assert.IsTrue(Comby.isTwoCycle tc);
        Assert.IsFalse(Comby.isTwoCycle ntc);
        Assert.IsTrue(Comby.isTwoCycle tc2);
        Assert.IsFalse(Comby.isTwoCycle ntc2);
        Assert.IsTrue(Comby.isTwoCycle tc3);
        Assert.IsFalse(Comby.isTwoCycle ntc3);


    [<TestMethod>]
    member this.isPermutation () =
        let tc =  [|0;1;2;3;4;6;5|]
        let ntc = [|1;1;2;3;6;4;5|]
        let tc2 =  [|0;4;2;3;1;6;5|]
        let ntc2 = [|9;1;2;3;6;5;4|]
        let tc3 =  [|0;4;2;3;1;6;5|]
        let ntc3 = [|1;1;2;3;6;5;4|]

        Assert.IsTrue(Comby.isPermutation tc);
        Assert.IsFalse(Comby.isPermutation ntc);
        Assert.IsTrue(Comby.isPermutation tc2);
        Assert.IsFalse(Comby.isPermutation ntc2);
        Assert.IsTrue(Comby.isPermutation tc3);
        Assert.IsFalse(Comby.isPermutation ntc3);



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