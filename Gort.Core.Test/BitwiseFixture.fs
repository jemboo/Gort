namespace Gort.Core.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type BitwiseFixture () =

    [<TestMethod>]
    member this.allSorted_uL () =
        let ba = Bitwise.allSorted_uL
        Assert.IsTrue(ba.Length = 64);


    [<TestMethod>]
    member this.allSorted_uL2 () =
        let ba = Bitwise.IdMap_ints
        Assert.IsTrue(ba.Length = 65);


    [<TestMethod>]
    member this.isSorted () =
        let uLun = 12412uL;
        let uLsrted = 2047uL;
        Assert.IsFalse(Bitwise.isSorted uLun);
        Assert.IsTrue(Bitwise.isSorted uLsrted);


    [<TestMethod>]
    member this.toIntArray () =
        let uLun = [|0; 0; 1; 0; 1; 1; 0|]
        let dg = Degree.create uLun.Length
        let uLRep = Bitwise.fromIntArray uLun 1
        let aBack = uLRep |> Bitwise.toIntArray dg
        Assert.AreEqual(uLun |> Array.toList, aBack |> Array.toList)
        Assert.IsTrue(true);


    [<TestMethod>]
    member this.mergeUpSeq () =
        let hiDegree = 5
        let lowDegree = 3
        let mergeUpCt = hiDegree * lowDegree
        let lowBand = Bitwise.allSorted_uL |> List.take lowDegree
        let hiBand = Bitwise.allSorted_uL |> List.take hiDegree
        let mergedSet = Bitwise.mergeUpSeq (Degree.create lowDegree) 
                                             lowBand hiBand |> Seq.toArray
        Assert.AreEqual(mergedSet.Length, mergeUpCt)


    [<TestMethod>]
    member this.cratesFor () =
        let wak = Bitwise.cratesFor 64 512
        let rak = Bitwise.cratesFor 64 513
        let yak = Bitwise.cratesFor 64 575
        Assert.AreEqual(wak, 8)
        Assert.AreEqual(rak, 9)
        Assert.AreEqual(yak, 9)


    [<TestMethod>]
    member this.allBitPackForDegree () =
        let deg = Degree.create 3
        let bitStrings = Bitwise.allBitPackForDegree deg |> Result.ExtractOrThrow
        Assert.AreEqual(Degree.value deg, Degree.value deg )
        Assert.AreEqual(bitStrings.Length, Degree.binExp deg )


    [<TestMethod>]
    member this.bitPackedtoBitStriped () =
        let deg = Degree.create 8
        let bpa = Bitwise.allBitPackForDegree deg |> Result.ExtractOrThrow
        let bsa = bpa |> Bitwise.bitPackedtoBitStriped deg |> Result.ExtractOrThrow
        let bpaBack = bsa |> Bitwise.bitStripedToBitPacked deg bpa.Length |> Result.ExtractOrThrow
        Assert.AreEqual(bpa |> Array.toList, bpaBack |> Array.toList)

        let deg2 = Degree.create 16
        let randy = Rando.fromRngGen (RngGen.lcgFromNow ())
        let bpa2 = Array.init 100 (fun _ -> RndGen.rndBitsUint64 deg2 randy)
        let bsa2 = bpa2 |> Bitwise.bitPackedtoBitStriped deg2 |> Result.ExtractOrThrow
        let bpaBack2 = bsa2 |> Bitwise.bitStripedToBitPacked deg2 bpa2.Length |> Result.ExtractOrThrow

        Assert.AreEqual(bpa2 |> Array.toList, bpaBack2 |> Array.toList)



    [<TestMethod>]
    member this.bitPackedtoBitStriped2D () =
        let deg2 = Degree.create 16
        let randy = Rando.fromRngGen (RngGen.lcgFromNow ())
        let bpa2 = Array.init 1000 (fun _ -> RndGen.rndBitsUint64 deg2 randy)
        let bsa2 = bpa2 |> Bitwise.bitPackedtoBitStriped2D deg2 |> Result.ExtractOrThrow
        Assert.IsTrue(bsa2.Length > 1)


    [<TestMethod>]
    member this.filterByPickList () =
        let data = [|0uL; 1uL; 2uL; 3uL; 4uL; 5uL;|]
        let picks = [|true; false; true; true; false; true;|]
        let expected = [|0uL; 2uL; 3uL; 5uL;|]
        let filtered = Bitwise.filterByPickList data picks
                        |> Result.ExtractOrThrow
        Assert.AreEqual(expected |> Array.toList, filtered |> Array.toList);
        

        
    [<TestMethod>]
    member this.rolloutAndSortedFilter () =
        let deg = Degree.create 8
        let bpa = Bitwise.allBitPackForDegree deg |> Result.ExtractOrThrow
        let bsa = bpa |> Bitwise.bitPackedtoBitStriped deg |> Result.ExtractOrThrow
        let bpaBack = bsa |> Bitwise.bitStripedToBitPacked deg bpa.Length |> Result.ExtractOrThrow
        let srtedBack = bpaBack |> Array.filter(fun srtbl -> srtbl |> Bitwise.isSorted |> not)

        Assert.AreEqual(1, 1);
