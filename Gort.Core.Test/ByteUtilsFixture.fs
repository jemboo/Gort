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
    member this.makeStripedArraysFromBoolArraysShort () =
        let order = 5 |> Order.createNr

        let boolArrays = 
            [|
                [|true; true; false; false; true|];
                [|false; true; false; false; false|];
                [|false; true; true; false; true|];
                [|true; true; false; false; true|];
                [|false; true; false; false; false|];
                [|false; true; true; false; true|];
                [|true; true; false; false; true|];
                [|false; true; false; false; false|];
                [|false; true; true; false; true|];
            |]
        let arrayCt = boolArrays.Length

        let bitStripes, count = ByteUtils.makeStripedArraysFromBoolArrays
                                    order
                                    boolArrays 
                                |> Result.ExtractOrThrow
                                
        Assert.AreEqual(arrayCt, count);

        let boolArrayBack = bitStripes 
                            |> ByteUtils.fromStripeArrays order
                            |> Seq.take arrayCt
                            |> Seq.toArray

        Assert.IsTrue(CollectionProps.areEqual boolArrays boolArrayBack)



    [<TestMethod>]
    member this.makeStripedArraysFromBoolArraysLong () =
        let order = 5 |> Order.createNr

        let boolArrays = [|
                        [|true; true; false; false; true|];
                        [|false; true; false; false; false|];
                        [|false; true; true; false; true|];
                        [|true; true; false; false; true|];
                        [|false; true; false; false; false|];
                        [|false; true; true; false; true|];
                        [|true; true; false; false; true|];
                        [|false; true; false; false; false|];
                        [|false; true; true; false; true|];
                        [|true; true; false; false; true|];
                        [|false; true; false; false; false|];
                        [|false; true; true; false; true|];
                        [|true; true; false; false; true|];
                        [|false; true; false; false; false|];
                        [|false; true; true; false; true|];
                        [|true; true; false; false; true|];
                        [|false; true; false; false; false|];
                        [|false; true; true; false; true|];
                        [|true; true; false; false; true|];
                        [|false; true; false; false; false|];
                        [|false; true; true; false; true|];
                        [|true; true; false; false; true|];
                        [|false; true; false; false; false|];
                        [|false; true; true; false; true|];
                        [|true; true; false; false; true|];
                        [|false; true; false; false; false|];
                        [|false; true; true; false; true|];
                        |]

        let arrayCt = boolArrays.Length

        let bitStripes, count = ByteUtils.makeStripedArraysFromBoolArrays
                                    order
                                    boolArrays 
                                |> Result.ExtractOrThrow
                                
        Assert.AreEqual(arrayCt, count);

        let boolArrayBack = bitStripes 
                            |> ByteUtils.fromStripeArrays order
                            |> Seq.take arrayCt
                            |> Seq.toArray

        Assert.IsTrue(CollectionProps.areEqual boolArrays boolArrayBack)






    [<TestMethod>]
    member this.usedStripeCountShort () =
        let order = 5 |> Order.createNr

        let boolArrays = [|
                        [|true; true; false; false; true|];
                        [|false; true; false; false; false|];
                        [|false; true; true; false; true|];
                        [|true; true; false; false; true|];
                        [|false; true; false; false; false|];
                        [|false; true; true; false; true|];
                        [|true; true; false; false; true|];
                        [|false; true; false; false; false|];
                        [|false; true; true; false; true|];
                        |]
        let arrayCt = boolArrays.Length

        let bitStripes, count = ByteUtils.makeStripedArraysFromBoolArrays
                                    order
                                    boolArrays 
                                |> Result.ExtractOrThrow
                                
        Assert.AreEqual(arrayCt, count);

        let usedStripeCt = bitStripes |> ByteUtils.usedStripeCount

        Assert.IsTrue(CollectionProps.areEqual arrayCt usedStripeCt)





    [<TestMethod>]
    member this.usedStripeCountLong () =
        let order = 5 |> Order.createNr

        let boolArrays = [|
                        [|true; true; false; false; true|];
                        [|false; true; false; false; false|];
                        [|false; true; true; false; true|];
                        [|true; true; false; false; true|];
                        [|false; true; false; false; false|];
                        [|false; true; true; false; true|];
                        [|true; true; false; false; true|];
                        [|false; true; false; false; false|];
                        [|false; true; true; false; true|];
                        [|true; true; false; false; true|];
                        [|false; true; false; false; false|];
                        [|false; true; true; false; true|];
                        [|true; true; false; false; true|];
                        [|false; true; false; false; false|];
                        [|false; true; true; false; true|];
                        [|true; true; false; false; true|];
                        [|false; true; false; false; false|];
                        [|false; true; true; false; true|];
                        [|true; true; false; false; true|];
                        [|false; true; false; false; false|];
                        [|false; true; true; false; true|];
                        [|true; true; false; false; true|];
                        [|false; true; false; false; false|];
                        [|false; true; true; false; true|];
                        [|true; true; false; false; true|];
                        [|false; true; false; false; false|];
                        [|false; true; true; false; true|];
                        |]
        let arrayCt = boolArrays.Length

        let bitStripes, count = ByteUtils.makeStripedArraysFromBoolArrays
                                    order
                                    boolArrays 
                                |> Result.ExtractOrThrow
                                
        Assert.AreEqual(arrayCt, count);

        let usedStripeCt = bitStripes |> ByteUtils.usedStripeCount

        Assert.IsTrue(CollectionProps.areEqual arrayCt usedStripeCt)







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
    member this.uint64ToIntArray () =
        let order = 44 |> Order.createNr
        let uint64In = 125436312321uL
        let arrayRep = ByteUtils.uint64ToIntArray order uint64In
        let uint64Back = ByteUtils.thresholdArrayToUint64 arrayRep 1
        Assert.AreEqual(uint64In, uint64Back);
