namespace Gort.Core.Test

open System
open SysExt
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type CollectionPropsFixture () =

    [<TestMethod>]
    member this.areEqual () =
        let a1 = [| [|1;2;3;|]; [|1;2;3;|]; [|1;2;3;|] |]
        let a2 = [| [|1;2;3;|]; [|1;2;3;|]; [|1;2;3;|] |]
        let a3 = [| [|1;2;3;|]; [|1;2;2;|]; [|1;2;3;|] |]
        let c1 = CollectionProps.areEqual a1 a2 
        let c2 = CollectionProps.areEqual a1 a3 
        Assert.IsTrue(c1)
        Assert.IsFalse(c2)

    [<TestMethod>]
    member this.arrayEquals () =
        let a1 = [|1;2;3;|]
        let a2 = [|1;2;3;|]
        let a3 = [|1;2;4;|]
    
        Assert.IsTrue(CollectionProps.arrayEquals a1 a2)
        Assert.IsFalse(CollectionProps.arrayEquals a1 a3)


    [<TestMethod>]
    member this.asCumulative() =
        let testArray = [|2.0; 3.0; 4.0; 5.0; 6.0; 7.0; 8.0; 9.0; 10.0|]
        let startVal = 3.5
        let expectedResult = [|3.5; 5.5; 8.5; 12.5; 17.5; 23.5; 30.5; 38.5; 47.5; 57.5|]
        let actualResult = CollectionProps.asCumulative startVal testArray
        Assert.AreEqual(expectedResult.[8], actualResult.[8])


    [<TestMethod>]
    member this.enumNchooseM() =
        let n = 8
        let m = 5
        let res = CollectionProps.enumNchooseM n m 
                    |> Seq.map(List.toArray)
                    |> Seq.toList
        Assert.IsTrue(res |> Seq.forall(CollectionProps.isSorted))
        Assert.AreEqual(res.Length, 56)


    [<TestMethod>]
    member this.isIdentity() =
        let testArray = [|2; 3; 4; 5;|]
        let testArray2 = [|0; 1; 2; 3; 4; 5;|]
        Assert.IsFalse(CollectionProps.isIdentity testArray)
        Assert.IsTrue(CollectionProps.isIdentity testArray2)


    [<TestMethod>]
    member this.isPermutation () =
        let tc =  [|0;1;2;3;4;6;5|]
        let ntc = [|1;1;2;3;6;4;5|]
        let tc2 =  [|0;4;2;3;1;6;5|]
        let ntc2 = [|9;1;2;3;6;5;4|]
        let tc3 =  [|0;4;2;3;1;6;5|]
        let ntc3 = [|1;1;2;3;6;5;4|]

        Assert.IsTrue(CollectionProps.isPermutation tc);
        Assert.IsFalse(CollectionProps.isPermutation ntc);
        Assert.IsTrue(CollectionProps.isPermutation tc2);
        Assert.IsFalse(CollectionProps.isPermutation ntc2);
        Assert.IsTrue(CollectionProps.isPermutation tc3);
        Assert.IsFalse(CollectionProps.isPermutation ntc3);


    [<TestMethod>]
    member this.cratesFor () =
        let wak = CollectionProps.cratesFor 64 512
        let rak = CollectionProps.cratesFor 64 513
        let yak = CollectionProps.cratesFor 64 575
        Assert.AreEqual(wak, 8)
        Assert.AreEqual(rak, 9)
        Assert.AreEqual(yak, 9)


    [<TestMethod>]
    member this.isSorted_idiom () =
        let aSrted = [|0;1;2;3|]
        let aRes = aSrted |> CollectionProps.isSorted_idiom
        Assert.IsTrue(aRes)

        let aUnSrted = [|1; 2; 3; 0|]
        let aUnRes = aUnSrted |> CollectionProps.isSorted_idiom
        Assert.IsFalse(aUnRes)

        let aUnSrted2 = [|1; 2; 0; 3;|]
        let aUnRes2 = aUnSrted2 |> CollectionProps.isSorted_idiom
        Assert.IsFalse(aUnRes2)


    [<TestMethod>]
    member this.isSorted () =
        let aSrted = [|0;1;2;3|]
        let aRes = aSrted |> CollectionProps.isSorted
        Assert.IsTrue(aRes)

        let aUnSrted = [|1; 2; 3; 0|]
        let aUnRes = aUnSrted |> CollectionProps.isSorted
        Assert.IsFalse(aUnRes)

        let aUnSrted2 = [|1; 2; 0; 3;|]
        let aUnRes2 = aUnSrted2 |> CollectionProps.isSorted
        Assert.IsFalse(aUnRes2)


    [<TestMethod>]
    member this.isTwoCycle () =
        let tc =  [|0;1;2;3;4;6;5|]
        let ntc = [|0;1;2;3;6;4;5|]
        let tc2 =  [|0;4;2;3;1;6;5|]
        let ntc2 = [|9;1;2;3;6;5;4|]
        let tc3 =  [|0;4;2;3;1;6;5|]
        let ntc3 = [|1;1;2;3;6;5;4|]

        Assert.IsTrue(CollectionProps.isTwoCycle tc);
        Assert.IsFalse(CollectionProps.isTwoCycle ntc);
        Assert.IsTrue(CollectionProps.isTwoCycle tc2);
        Assert.IsFalse(CollectionProps.isTwoCycle ntc2);
        Assert.IsTrue(CollectionProps.isTwoCycle tc3);
        Assert.IsFalse(CollectionProps.isTwoCycle ntc3);


    //[<TestMethod>]
    //member this.conjugateIntArrays_preserves_twoCycle() =
    //    let order = Order.create 8
    //    let randy = Rando.fromRngGen (RngGen.lcgFromNow())
    //    let mutable i = 0
    //    while i<10 do
    //        let tc = RndGen.rndFullTwoCycleArray randy (Order.value order)
    //        let conjer = RndGen.conjIntArrayWsutation randy order
    //        let conj = Comby.conjIntArrays tc conjer
    //                    |> Result.ExtractOrThrow
    //        let isTc = Comby.isTwoCycle conj
    //                    |> Result.ExtractOrThrow
    //        Assert.IsTrue(isTc)
    //        i <- i+1

