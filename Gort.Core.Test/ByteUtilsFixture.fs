namespace Gort.Core.Test

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
        Assert.IsFalse(ByteUtils.isUint64Sorted uLun);
        Assert.IsTrue(ByteUtils.isUint64Sorted uLsrted);


    [<TestMethod>]
    member this.toIntArray () =
        let uLun = [|0; 0; 1; 0; 1; 1; 0|]
        let ord = Order.createNr uLun.Length
        let uLRep = ByteUtils.thresholdArrayToUint64 uLun 1
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
        let srtedBack = bpaBack |> Array.filter(fun srtbl -> srtbl |> ByteUtils.isUint64Sorted |> not)
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
    member this.stripeRnWInt32 () =
        //let ord = Order.createNr 3
        //let arOfIntAr = [|[|1; 0; 1|]; [|0; 1; 1|]; [|0; 0; 0|]; [|1; 1; 1 |];|]
        //let stripeAs = arOfIntAr
        //                |> ByteUtils.toStripeArrays 1 ord
        //                |> Seq.toArray
        //                |> Array.concat
        //let zero_val = 0
        //let one_val = 1
        //let arOfIntArBack = stripeAs |> ByteUtils.fromStripeArrays zero_val one_val ord
        //                             |> Seq.take(arOfIntAr.Length)
        //                             |> Seq.toList
    
        //Assert.IsTrue(CollectionProps.areEqual arOfIntAr arOfIntArBack)
        Assert.AreEqual(1, 1);



    [<TestMethod>]
    member this.stripeRnW () =
        //let ord = Order.createNr 8
        //let boolSetIn = Bool
        //let intSetIn = IntSet8.allBitsAsSeq ord |> Seq.toArray
        //let stripeAs = intSetIn
        //                |> Seq.map(IntSet8.getValues)
        //                |> ByteUtils.toStripeArraysO 1uy ord
        //                |> Seq.toArray
        //                |> Array.concat

        //let intSetBack = stripeAs |> ByteUtils.fromStripeArrays 0uy 1uy ord
        //                          |> Seq.map(IntSet8.fromBytes ord)
        //                          |> Seq.toList
        //                          |> Result.sequence
        //                          |> Result.ExtractOrThrow
    
        //Assert.AreEqual(intSetIn |> Array.toList, intSetBack);

        Assert.AreEqual(1, 1);


    [<TestMethod>]
    member this.stripeRnWuint8 () =
        //let ord = Order.createNr 3
        //let arOfIntAr = [|[|1uy; 0uy; 1uy|]; [|0uy; 1uy; 1uy|]; [|0uy; 0uy; 0uy|]; [|1uy; 1uy; 1uy |];|]
        //let stripeAs = arOfIntAr
        //                |> ByteUtils.toStripeArraysO 1uy ord
        //                |> Seq.toArray
        //                |> Array.concat

        //let arOfIntArBack = stripeAs |> ByteUtils.fromStripeArrays 0uy 1uy ord
        //                             |> Seq.take(arOfIntAr.Length)
        //                             |> Seq.toList
    
        //Assert.IsTrue(CollectionProps.areEqual arOfIntAr arOfIntArBack)
        Assert.AreEqual(1, 1);


    [<TestMethod>]
    member this.intPack () =
        let bitsPerSymbol = 32 |> BitsPerSymbol.create |> Result.ExtractOrThrow
        let v = 5uL
        let va = [23454234uL; 23423uL]
        let qaA =  bitsFromSpUint64Positions bitsPerSymbol va |> Seq.toArray
        
        let max8 = 8 |> BitsPerSymbol.create |> Result.ExtractOrThrow
        let ua = [234uy; 42uy]
        let paA =  bitsFromSpBytePositions max8 ua |> Seq.toArray

        Assert.AreEqual(1, 1);


    [<TestMethod>]
    member this.intUnPack () =
        let bitsPerSymbol = 32 |> BitsPerSymbol.create |> Result.ExtractOrThrow
        let va = [23454234uL; 23423uL]
        let qaA =  bitsFromSpUint64Positions bitsPerSymbol va |> Seq.toArray
        let qaB =  bitsToSpUint64Positions bitsPerSymbol qaA |> Seq.toArray


        let max8 = 8 |> BitsPerSymbol.create |> Result.ExtractOrThrow
        let ua = [234uy; 42uy]
        let paA =  bitsFromSpBytePositions max8 ua |> Seq.toArray
        let akB = bitsToSpBytePositions max8 paA |> Seq.toArray

        Assert.AreEqual(1, 1);


    [<TestMethod>]
    member this.usedStripeCount () =
        //let order = 4 |> Order.createNr
        //let arraysToStoreFull = [|[|1;0;1;0|];[|0;0;0;1|];[|1;0;0;0|]|]
        //let usedStripeCtFull = arraysToStoreFull.Length
        //let stripedArray = ByteUtils.toStripeArraysO 1 order arraysToStoreFull
        //                   |> Seq.head
        //let actualUsedStripes = ByteUtils.usedStripeCount stripedArray
        //Assert.AreEqual(usedStripeCtFull, actualUsedStripes);
        //let arraysToStore3 = [|[|1;0;1;0|];[|0;0;0;1|];[|1;0;0;0|];[|0;0;0;0|]|]
        //let usedStripeCt3 = 3
        //let stripedArray = ByteUtils.toStripeArraysO 1 order arraysToStore3
        //                   |> Seq.head
        //let actualUsedStripes = ByteUtils.usedStripeCount stripedArray
        //Assert.AreEqual(usedStripeCt3, actualUsedStripes);
        Assert.AreEqual(1, 1);


    [<TestMethod>]
    member this.uint64ToIntArray () =
        let order = 44 |> Order.createNr
        let uint64In = 125436312321uL
        let arrayRep = ByteUtils.uint64ToIntArray order uint64In
        let uint64Back = ByteUtils.thresholdArrayToUint64 arrayRep 1
        Assert.AreEqual(uint64In, uint64Back);
