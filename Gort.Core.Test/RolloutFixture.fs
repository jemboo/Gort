namespace Gort.Core.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type RolloutFixture () =

    [<TestMethod>]
    member this.uInt8Roll () =
        let arOfIntAr = [|[|1;2;3|]; [|11;12;13|]; [|21;22;23|]; [|31;32;33|];|]
        let arrayLen = 3 |> ArrayLength.create |> Result.ExtractOrThrow
        let arrayCt = 4 |> ArrayCount.create |> Result.ExtractOrThrow
        let ssSize = 64uL |> SymbolSetSize.create |> Result.ExtractOrThrow

        let br = Uint8Roll.fromIntArraySeq arrayLen arOfIntAr
                    |> Result.ExtractOrThrow
        let arOfIntArBack = Uint8Roll.toIntArraySeq br |> Seq.toArray
        Assert.IsTrue(CollectionProps.areEqual arOfIntAr arOfIntArBack)

        let bitPack = br |> Uint8Roll.toBitPack ssSize |> Result.ExtractOrThrow
        let brB = bitPack |> Uint8Roll.fromBitPack arrayLen |> Result.ExtractOrThrow
        let arOfIntArBack2 = Uint8Roll.toIntArraySeq brB |> Seq.toArray
        Assert.IsTrue(CollectionProps.areEqual arOfIntAr arOfIntArBack2)

        Assert.IsTrue(true)


    [<TestMethod>]
    member this.uInt16Roll () =
        let arOfIntAr = [|[|1333;2;3|]; [|11;12;13|]; [|21;22;23|]; [|3331;32;33|];|]
        let arrayLen = 3 |> ArrayLength.create |> Result.ExtractOrThrow
        let arrayCt = 4 |> ArrayCount.create |> Result.ExtractOrThrow
        let ssSize = 4000uL |> SymbolSetSize.create |> Result.ExtractOrThrow

        let br = Uint16Roll.fromIntArraySeq arrayLen arOfIntAr
                    |> Result.ExtractOrThrow
        let arOfIntArBack = Uint16Roll.toIntArraySeq br |> Seq.toArray
        Assert.IsTrue(CollectionProps.areEqual arOfIntAr arOfIntArBack)

        let bitPack = br |> Uint16Roll.toBitPack ssSize |> Result.ExtractOrThrow
        let brB = bitPack |> Uint16Roll.fromBitPack arrayLen |> Result.ExtractOrThrow
        let arOfIntArBack2 = Uint16Roll.toIntArraySeq brB |> Seq.toArray
        Assert.IsTrue(CollectionProps.areEqual arOfIntAr arOfIntArBack2)

        Assert.IsTrue(true)



    [<TestMethod>]
    member this.intRoll () =
        let arOfIntAr = [|[|1111;11112;11113|]; [|2211;12;13|]; [|2221;22;23|]; [|5555531;32;33|];|]
        let arrayLen = 3 |> ArrayLength.create |> Result.ExtractOrThrow
        let arrayCt = 4 |> ArrayCount.create |> Result.ExtractOrThrow
        let ssSize = 16555531uL |> SymbolSetSize.create |> Result.ExtractOrThrow

        let br = IntRoll.fromIntArraySeq arrayLen arOfIntAr
                    |> Result.ExtractOrThrow
        let arOfIntArBack = IntRoll.toIntArraySeq br |> Seq.toArray
        Assert.IsTrue(CollectionProps.areEqual arOfIntAr arOfIntArBack)

        let bitPack = br |> IntRoll.toBitPack ssSize |> Result.ExtractOrThrow
        let brB = bitPack |> IntRoll.fromBitPack arrayLen |> Result.ExtractOrThrow
        let arOfIntArBack2 = IntRoll.toIntArraySeq brB |> Seq.toArray
        Assert.IsTrue(CollectionProps.areEqual arOfIntAr arOfIntArBack2)



    [<TestMethod>]
    member this.uint64Roll () =
        let arOfIntAr = [|[|1111;11112;11113|]; [|2211;12;13|]; [|2221;22;23|]; [|5555531;32;33|];|]
        let arrayLen = 3 |> ArrayLength.create |> Result.ExtractOrThrow
        let arrayCt = 4 |> ArrayCount.create |> Result.ExtractOrThrow
        let ssSize = 16555531uL |> SymbolSetSize.create |> Result.ExtractOrThrow

        let roll64 = Uint64Roll.fromIntArraySeq arrayLen arOfIntAr
                    |> Result.ExtractOrThrow
        let arOfIntArBack = Uint64Roll.toIntArraySeq roll64 |> Seq.toArray
        Assert.IsTrue(CollectionProps.areEqual arOfIntAr arOfIntArBack)

        let bitPack = roll64 |> Uint64Roll.toBitPack ssSize |> Result.ExtractOrThrow
        let brB = bitPack |> Uint64Roll.fromBitPack arrayLen |> Result.ExtractOrThrow
        let arOfIntArBack2 = Uint64Roll.toIntArraySeq brB |> Seq.toArray
        Assert.IsTrue(CollectionProps.areEqual arOfIntAr arOfIntArBack2)



    [<TestMethod>]
    member this.uint64Rollw64 () =
        let arOfIntAr = [|[|1111uL; 11112uL; 11113uL|]; [|2211uL; 12uL; 13uL|]; [|2221uL; 22uL; 23uL|]; [|19912345123456uL; 5658543211234567uL; 654321089766uL |]; |]
        let arrayLen = 3 |> ArrayLength.create |> Result.ExtractOrThrow
        let arrayCt = 4 |> ArrayCount.create |> Result.ExtractOrThrow
        let ssSize = (UInt64.MaxValue - 1uL) |> SymbolSetSize.create |> Result.ExtractOrThrow

        let roll64 = Uint64Roll.fromUint64ArraySeq arrayLen arOfIntAr
                      |> Result.ExtractOrThrow
        let arOfIntArBack = Uint64Roll.toUint64ArraySeq roll64 |> Seq.toArray
        Assert.IsTrue(CollectionProps.areEqual arOfIntAr arOfIntArBack)

        let bitPack = roll64 |> Uint64Roll.toBitPack ssSize |> Result.ExtractOrThrow
        let brB = bitPack |> Uint64Roll.fromBitPack arrayLen |> Result.ExtractOrThrow
        let arOfIntArBack2 = Uint64Roll.toUint64ArraySeq brB |> Seq.toArray
        Assert.IsTrue(CollectionProps.areEqual arOfIntAr arOfIntArBack2)




    [<TestMethod>]
    member this.fromIntArraySeqAsBitStriped () =
        let arOfIntAr = [|[|1; 0; 1|]; [|0; 1; 1|]; [|0; 0; 0|]; [|1; 1; 1 |];|]
        let arrayLen = 3 |> ArrayLength.create |> Result.ExtractOrThrow
        let arrayCt = 4 |> ArrayCount.create |> Result.ExtractOrThrow
        let ssSize = 16555531uL |> SymbolSetSize.create |> Result.ExtractOrThrow

        let roll64 = Uint64Roll.saveIntArraysAsBitStriped arrayLen arOfIntAr
                      |> Result.ExtractOrThrow
        let arOfIntArBack = Uint64Roll.asBitStripedToIntArraySeq roll64 |> Seq.toArray
        Assert.IsTrue(CollectionProps.areEqual arOfIntAr arOfIntArBack)