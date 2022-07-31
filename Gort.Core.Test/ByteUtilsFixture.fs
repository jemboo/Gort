namespace Gort.Core.Test

open System
open SysExt
open Microsoft.VisualStudio.TestTools.UnitTesting
open ByteUtils

[<TestClass>]
type ByteUtilsFixture () =

    [<TestMethod>]
    member this.allSorted_uL () =
        let ba = ByteUtils.allSorted_uL
        Assert.IsTrue(ba.Length = 64);


    [<TestMethod>]
    member this.allSorted_uL2 () =
        let ba = ByteUtils.IdMap_ints
        Assert.IsTrue(ba.Length = 65);


    [<TestMethod>]
    member this.isSorted () =
        let uLun = 12412uL;
        let uLsrted = 2047uL;
        Assert.IsFalse(ByteUtils.isSorted uLun);
        Assert.IsTrue(ByteUtils.isSorted uLsrted);


    [<TestMethod>]
    member this.toIntArray () =
        let uLun = [|0; 0; 1; 0; 1; 1; 0|]
        let ord = Order.createNr uLun.Length
        let uLRep = ByteUtils.arrayToUint64 uLun 1
        let aBack = uLRep |> ByteUtils.uint64To2ValArray ord 1 0
        Assert.AreEqual(uLun |> Array.toList, aBack |> Array.toList)
        Assert.IsTrue(true);


    [<TestMethod>]
    member this.mergeUpSeq () =
        let hiDegree = 5
        let lowDegree = 3
        let mergeUpCt = hiDegree * lowDegree
        let lowBand = ByteUtils.allSorted_uL |> List.take lowDegree
        let hiBand = ByteUtils.allSorted_uL |> List.take hiDegree
        let mergedSet = ByteUtils.mergeUpSeq (Order.createNr lowDegree) 
                                             lowBand hiBand |> Seq.toArray
        Assert.AreEqual(mergedSet.Length, mergeUpCt)


    [<TestMethod>]
    member this.allBitPackForDegree () =
        let deg = Order.createNr 3
        let bitStrings = Order.allSortableAsUint64 deg |> Result.ExtractOrThrow
        Assert.AreEqual(Order.value deg, Order.value deg )
        Assert.AreEqual(bitStrings.Length, Order.binExp deg )


    [<TestMethod>]
    member this.bitPackedtoBitStriped () =
        let deg = Order.createNr 8
        let bpa = Order.allSortableAsUint64 deg |> Result.ExtractOrThrow
        let bsa = bpa |> ByteUtils.uint64ArraytoBitStriped deg |> Result.ExtractOrThrow
        let bpaBack = bsa |> ByteUtils.bitStripedToUint64array deg bpa.Length |> Result.ExtractOrThrow
        Assert.AreEqual(bpa |> Array.toList, bpaBack |> Array.toList)

        let deg2 = Order.createNr 16
        let randy = Rando.fromRngGen (RngGen.lcgFromNow ())
        let bpa2 = Array.init 100 (fun _ -> RandVars.rndBitsUint64 deg2 randy)
        let bsa2 = bpa2 |> ByteUtils.uint64ArraytoBitStriped deg2 |> Result.ExtractOrThrow
        let bpaBack2 = bsa2 |> ByteUtils.bitStripedToUint64array deg2 bpa2.Length |> Result.ExtractOrThrow

        Assert.AreEqual(bpa2 |> Array.toList, bpaBack2 |> Array.toList)



    [<TestMethod>]
    member this.bitPackedtoBitStriped2D () =
        let deg2 = Order.createNr 16
        let randy = Rando.fromRngGen (RngGen.lcgFromNow ())
        let bpa2 = Array.init 1000 (fun _ -> RandVars.rndBitsUint64 deg2 randy)
        let bsa2 = bpa2 |> ByteUtils.uint64ArraytoBitStriped2D deg2 |> Result.ExtractOrThrow
        Assert.IsTrue(bsa2.Length > 1)


    [<TestMethod>]
    member this.rolloutAndSortedFilter () =
        let deg = Order.createNr 8
        let bpa = Order.allSortableAsUint64 deg |> Result.ExtractOrThrow
        let bsa = bpa |> ByteUtils.uint64ArraytoBitStriped deg |> Result.ExtractOrThrow
        let bpaBack = bsa |> ByteUtils.bitStripedToUint64array deg bpa.Length |> Result.ExtractOrThrow
        let srtedBack = bpaBack |> Array.filter(fun srtbl -> srtbl |> ByteUtils.isSorted |> not)
        Assert.AreEqual(1, 1);


    [<TestMethod>]
    member this.hashObjs () =
        let uintsA = [|196uy; 57uy; 110uy |]
        let ooga = "ooga"
        let booga = [|19912345123456uL; 5658543211234567uL; 119987654321089766uL |]

        let expected = ByteUtils.hashObjs [|uintsA;ooga;uintsA;booga;|]
        let expected2 = ByteUtils.hashObjs [|ooga; null|]
        let expected3 = ByteUtils.hashObjs [|ooga;|]
        
        Assert.AreEqual(1,1);


    [<TestMethod>]
    member this.stripeRnW () =
        let ord = Order.createNr 8
        let intSetIn = IntSet8.allForAsSeq ord |> Seq.toArray
        let stripeAs = intSetIn
                        |> Seq.map(IntSet8.getValues)
                        |> ByteUtils.toStripeArrays 1uy ord
                        |> Seq.toArray
                        |> Array.concat

        let intSetBack = stripeAs |> ByteUtils.fromStripeArrays 0uy 1uy ord
                                  |> Seq.map(IntSet8.fromBytes ord)
                                  |> Seq.toList
                                  |> Result.sequence
                                  |> Result.ExtractOrThrow
    
        Assert.AreEqual(intSetIn |> Array.toList, intSetBack)


    [<TestMethod>]
    member this.intPack () =
        let max64 = 32 |> BitWidth.create |> Result.ExtractOrThrow
        let v = 5uL
        let va = [23454234uL; 23423uL]
        let qa =  uint64ToBits max64 v |> Seq.toArray
        let qaA =  uint64SeqToBits max64 va |> Seq.toArray
        
        let max8 = 8 |> BitWidth.create |> Result.ExtractOrThrow
        let u = 5uy
        let ua = [234uy; 42uy]
        let pa =  byteToBits max8 u |> Seq.toArray
        let paA =  byteSeqToBits max8 ua |> Seq.toArray

        Assert.AreEqual(1, 1);


    [<TestMethod>]
    member this.intUnPack () =
        let max64 = 32 |> BitWidth.create |> Result.ExtractOrThrow
        let v = 5uL
        let va = [23454234uL; 23423uL]
        let qa =  uint64ToBits max64 v |> Seq.toArray
        let qaA =  uint64SeqToBits max64 va |> Seq.toArray
        let qaB =  bitSeqToUint64 max64 qaA |> Seq.toArray


        let max8 = 8 |> BitWidth.create |> Result.ExtractOrThrow
        let u = 5uy
        let ua = [234uy; 42uy]
        let pa =  byteToBits max8 u |> Seq.toArray
        let paA =  byteSeqToBits max8 ua |> Seq.toArray
        let akB = bitSeqToBytes max8 paA |> Seq.toArray

        Assert.AreEqual(1, 1);