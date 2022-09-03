namespace Gort.Core.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type RolloutFixture () =

    [<TestMethod>]
    member this.uInt8Roll_toBitPack () =
        let arOfIntAr = [|[|1;2;3|]; [|11;12;13|]; [|21;22;23|]; [|31;32;33|];|]
        let arrayLen = 3 |> ArrayLength.create |> Result.ExtractOrThrow
        let arrayCt = 4 |> ArrayCount.create |> Result.ExtractOrThrow
        let ssSize = 64uL |> SymbolSetSize.create |> Result.ExtractOrThrow

        let br = Uint8Roll.fromIntArrays arrayLen arOfIntAr
                    |> Result.ExtractOrThrow
        let arOfIntArBack = Uint8Roll.toIntArrays br |> Seq.toArray
        Assert.IsTrue(CollectionProps.areEqual arOfIntAr arOfIntArBack)

        let bitPack = br |> Uint8Roll.toBitPack ssSize |> Result.ExtractOrThrow
        let brB = bitPack |> Uint8Roll.fromBitPack arrayLen |> Result.ExtractOrThrow
        let arOfIntArBack2 = Uint8Roll.toIntArrays brB |> Seq.toArray
        Assert.IsTrue(CollectionProps.areEqual arOfIntAr arOfIntArBack2)

        Assert.IsTrue(true)



    [<TestMethod>]
    member this.bs64Roll_fromBitPack () =
        let arrayLen = 5 |> ArrayLength.createNr

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
             
        let bitPack = boolArrays 
                        |> BitPack.fromBoolArrays
                        |> Result.ExtractOrThrow


        let bs64Roll = bitPack 
                         |> Bs64Roll.fromBitPack arrayLen 
                         |> Result.ExtractOrThrow


        let bitPackBack = bs64Roll
                            |> Bs64Roll.toBitPack
                            |> Result.ExtractOrThrow


        let boolArraysBack = bitPackBack 
                                |> BitPack.toBoolArrays arrayLen
                                |> Result.ExtractOrThrow

        Assert.IsTrue(CollectionProps.areEqual boolArrays boolArraysBack)



    [<TestMethod>]
    member this.uInt16Roll () =
        let arOfIntAr = [|[|1333;2;3|]; [|11;12;13|]; [|21;22;23|]; [|3331;32;33|];|]
        let arrayLen = 3 |> ArrayLength.create |> Result.ExtractOrThrow
        let arrayCt = 4 |> ArrayCount.create |> Result.ExtractOrThrow
        let ssSize = 4000uL |> SymbolSetSize.create |> Result.ExtractOrThrow

        let br = Uint16Roll.fromIntArrays arrayLen arOfIntAr
                    |> Result.ExtractOrThrow
        let arOfIntArBack = Uint16Roll.toIntArrays br |> Seq.toArray
        Assert.IsTrue(CollectionProps.areEqual arOfIntAr arOfIntArBack)

        let bitPack = br |> Uint16Roll.toBitPack ssSize |> Result.ExtractOrThrow
        let brB = bitPack |> Uint16Roll.fromBitPack arrayLen |> Result.ExtractOrThrow
        let arOfIntArBack2 = Uint16Roll.toIntArrays brB |> Seq.toArray
        Assert.IsTrue(CollectionProps.areEqual arOfIntAr arOfIntArBack2)

        Assert.IsTrue(true)



    [<TestMethod>]
    member this.intRoll () =
        let arOfIntAr = [|[|1111;11112;11113|]; [|2211;12;13|]; [|2221;22;23|]; [|5555531;32;33|];|]
        let arrayLen = 3 |> ArrayLength.create |> Result.ExtractOrThrow
        let arrayCt = 4 |> ArrayCount.create |> Result.ExtractOrThrow
        let ssSize = 16555531uL |> SymbolSetSize.create |> Result.ExtractOrThrow

        let br = IntRoll.fromIntArrays arrayLen arOfIntAr
                    |> Result.ExtractOrThrow
        let arOfIntArBack = IntRoll.toIntArrays br |> Seq.toArray
        Assert.IsTrue(CollectionProps.areEqual arOfIntAr arOfIntArBack)

        let bitPack = br |> IntRoll.toBitPack ssSize |> Result.ExtractOrThrow
        let brB = bitPack |> IntRoll.fromBitPack arrayLen |> Result.ExtractOrThrow
        let arOfIntArBack2 = IntRoll.toIntArrays brB |> Seq.toArray
        Assert.IsTrue(CollectionProps.areEqual arOfIntAr arOfIntArBack2)



    [<TestMethod>]
    member this.uint64Roll () =
        let arOfIntAr = 
            [|
                [|1111;11112;11113|]; 
                [|2211;12;13|]; 
                [|2221;22;23|]; 
                [|5555531;32;33|];
            |]
        let arrayLen = 3 |> ArrayLength.create |> Result.ExtractOrThrow
        let arrayCt = 4 |> ArrayCount.create |> Result.ExtractOrThrow
        let ssSize = 16555531uL |> SymbolSetSize.create |> Result.ExtractOrThrow

        let roll64 = Uint64Roll.fromIntArrays arrayLen arOfIntAr
                    |> Result.ExtractOrThrow
        let arOfIntArBack = Uint64Roll.toIntArrays roll64 |> Seq.toArray
        Assert.IsTrue(CollectionProps.areEqual arOfIntAr arOfIntArBack)

        let bitPack = roll64 |> Uint64Roll.toBitPack ssSize |> Result.ExtractOrThrow
        let brB = bitPack |> Uint64Roll.fromBitPack arrayLen |> Result.ExtractOrThrow
        let arOfIntArBack2 = Uint64Roll.toIntArrays brB |> Seq.toArray
        Assert.IsTrue(CollectionProps.areEqual arOfIntAr arOfIntArBack2)



    [<TestMethod>]
    member this.uint64Rollw64 () =
        let arOfIntAr = 
            [|
                [|1111uL; 11112uL; 11113uL|]; 
                [|2211uL; 12uL; 13uL|]; 
                [|2221uL; 22uL; 23uL|]; 
                [|19912345123456uL; 5658543211234567uL; 654321089766uL |]; 
             |]

        let arrayLen = 3 |> ArrayLength.create |> Result.ExtractOrThrow
        let ssSize = (UInt64.MaxValue - 1uL) |> SymbolSetSize.create |> Result.ExtractOrThrow

        let roll64 = Uint64Roll.fromUint64Arrays arrayLen arOfIntAr
                      |> Result.ExtractOrThrow
        let arOfIntArBack = Uint64Roll.toUint64Arrays roll64 |> Seq.toArray
        Assert.IsTrue(CollectionProps.areEqual arOfIntAr arOfIntArBack)

        let bitPack = roll64 |> Uint64Roll.toBitPack ssSize |> Result.ExtractOrThrow
        let brB = bitPack |> Uint64Roll.fromBitPack arrayLen |> Result.ExtractOrThrow
        let arOfIntArBack2 = Uint64Roll.toUint64Arrays brB |> Seq.toArray
        Assert.IsTrue(CollectionProps.areEqual arOfIntAr arOfIntArBack2)



    [<TestMethod>]
    member this.bs64Roll_fromBoolArraysShort () =
        let arrayLen = 5 |> ArrayLength.create |> Result.ExtractOrThrow
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

        let arrayCountIn = boolArrays |> Array.length |> ArrayCount.createNr 
        let bs64Roll = boolArrays |> Bs64Roll.fromBoolArrays arrayLen |> Result.ExtractOrThrow
        Assert.AreEqual(arrayCountIn, bs64Roll |> Bs64Roll.getArrayCount)
        let boolArraysBack = bs64Roll 
                             |> Bs64Roll.toBoolArrays
                             |> Seq.toArray

        Assert.IsTrue(CollectionProps.areEqual boolArrays boolArraysBack)




    [<TestMethod>]
    member this.bs64Roll_fromBoolArraysLong () =
        let arrayLen = 5 |> ArrayLength.create |> Result.ExtractOrThrow
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

        let arrayCountIn = boolArrays |> Array.length |> ArrayCount.createNr 
        let bs64Roll = boolArrays |> Bs64Roll.fromBoolArrays arrayLen |> Result.ExtractOrThrow
        Assert.AreEqual(arrayCountIn, bs64Roll |> Bs64Roll.getArrayCount)
        let boolArraysBack = bs64Roll 
                             |> Bs64Roll.toBoolArrays
                             |> Seq.toArray

        Assert.IsTrue(CollectionProps.areEqual boolArrays boolArraysBack)


    [<TestMethod>]
    member this.isSorted () =
    
        let arrayLen = 7 |> ArrayLength.createNr
        let rfB = rolloutFormat.RfB
        let rfU8 = rolloutFormat.RfU8
        let rfI16 = rolloutFormat.RfU16
        let rfI32 = rolloutFormat.RfI32

        let sortedArrays8 = [|
                             [|0; 1; 3; 4; 5; 6; 9|]; 
                             [|10; 21; 33; 44; 55; 66; 77|]; 
                             [|10; 21; 43; 64; 85; 96; 109|]; 
                             [|0; 0; 31; 41; 51; 61; 91|]; 
                           |]

        let sortedArrays = [|
                             [|0; 1; 3; 4; 5; 6; 9|]; 
                             [|110; 221; 333; 444; 5555; 6666; 7779|]; 
                             [|10; 21; 43; 64; 85; 96; 109|]; 
                             [|1; 11; 31; 41; 51; 61; 91|]; 
                           |]

        let unSortedArrays = [|
                                 [|0; 1; 3; 4; 5; 6; 9|]; 
                                 [|11110; 22221; 33333; 444; 55555; 66666; 777779|]; 
                                 [|10; 21; 43; 64; 85; 96; 109|]; 
                                 [|1; 11; 31; 0; 51; 61; 91|]; 
                             |]

        let unSortedArrays8 = [|
                                 [|0; 1; 3; 4; 5; 6; 9|]; 
                                 [|11; 21; 33; 4; 55; 66; 79|]; 
                                 [|10; 21; 43; 64; 85; 96; 109|]; 
                                 [|1; 11; 31; 0; 51; 61; 91|]; 
                             |]


        let rolloutU8 = Rollout.fromIntArrays rfU8 arrayLen sortedArrays8 |> Result.ExtractOrThrow
        let isSortedU8 = rolloutU8 |> Rollout.isSorted
        Assert.IsTrue(isSortedU8)

        let rolloutU16 = Rollout.fromIntArrays rfI16 arrayLen sortedArrays |> Result.ExtractOrThrow
        let isSortedU16 = rolloutU16 |> Rollout.isSorted
        Assert.IsTrue(isSortedU16)

        let rolloutB = Rollout.fromIntArrays rfB arrayLen sortedArrays |> Result.ExtractOrThrow
        let isSortedB = rolloutB |> Rollout.isSorted
        Assert.IsTrue(isSortedB)


        let unsortedRI32 = Rollout.fromIntArrays rfI32 arrayLen unSortedArrays |> Result.ExtractOrThrow
        let isUnSortedI32 = unsortedRI32 |> Rollout.isSorted
        Assert.IsFalse(isUnSortedI32)

        let unsortedR8 = Rollout.fromIntArrays rfU8 arrayLen unSortedArrays8 |> Result.ExtractOrThrow
        let isUnSortedU8 = unsortedR8 |> Rollout.isSorted
        Assert.IsFalse(isUnSortedU8)

        let unsortedRB = Rollout.fromIntArrays rfB arrayLen unSortedArrays |> Result.ExtractOrThrow
        let unSortedB = unsortedRB |> Rollout.isSorted
        Assert.IsFalse(unSortedB)