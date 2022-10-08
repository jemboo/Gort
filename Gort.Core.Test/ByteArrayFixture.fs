namespace Gort.Core.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type ByteArrayFixture() =

    [<TestMethod>]
    member this.mapIntArrays() =
        let srcA = [| 199; 12345; 123456; 56585; 43211234; 567; 11998765; 432108; 9766 |]
        let destA = Array.zeroCreate<int> (srcA.Length)
        ByteArray.copyIntArray 0 srcA 0 destA (srcA.Length) //1  4 // |> Result.ExtractOrThrow
        Assert.AreEqual(srcA |> Array.toList, srcA |> Array.toList)


    [<TestMethod>]
    member this.getUint64fromBytes() =
        let uintV = 19912345123456uL
        let offset = 3
        let blobIn = Array.zeroCreate<byte> (offset + 8)

        let blobIn =
            [| uintV |]
            |> (ByteArray.mapUint64sToBytes 0 1 blobIn offset)
            |> Result.ExtractOrThrow

        let uintVback =
            blobIn |> ByteArray.getUint64fromBytes offset |> Result.ExtractOrThrow

        Assert.AreEqual(uintV, uintVback)


    [<TestMethod>]
    member this.filterByPickList() =
        let data = [| 0uL; 1uL; 2uL; 3uL; 4uL; 5uL |]
        let picks = [| true; false; true; true; false; true |]
        let expected = [| 0uL; 2uL; 3uL; 5uL |]
        let filtered = CollectionOps.filterByPickList data picks |> Result.ExtractOrThrow
        Assert.AreEqual(expected |> Array.toList, filtered |> Array.toList)


    [<TestMethod>]
    member this.blobToRolloutS64() =
        let uintsA = [| 19912345123456uL; 5658543211234567uL; 119987654321089766uL |]
        let blob = uintsA |> ByteArray.convertUint64sToBytes |> Result.ExtractOrThrow
        let uintsAback = blob |> ByteArray.convertBytesToUint64s |> Result.ExtractOrThrow
        let expected = uintsA |> Array.toList
        Assert.AreEqual(expected, uintsAback |> Array.toList)


    [<TestMethod>]
    member this.blobToRolloutS32() =
        let uintsA = [| 199126ul; 565823457ul; 11089766ul |]
        let blob = uintsA |> ByteArray.convertUint32sToBytes |> Result.ExtractOrThrow
        let uintsAback = blob |> ByteArray.convertBytesToUint32s |> Result.ExtractOrThrow
        let expected = uintsA |> Array.toList

        Assert.AreEqual(expected, uintsAback |> Array.toList)


    [<TestMethod>]
    member this.blobToRolloutS16() =
        let uintsA = [| 1996us; 5457us; 11066us |]
        let blob = uintsA |> ByteArray.convertUint16sToBytes |> Result.ExtractOrThrow
        let uintsAback = blob |> ByteArray.convertBytesToUint16s |> Result.ExtractOrThrow
        let expected = uintsA |> Array.toList

        Assert.AreEqual(expected, uintsAback |> Array.toList)


    [<TestMethod>]
    member this.blobToRolloutS8() =
        let uintsA = [| 196uy; 57uy; 110uy |]
        let blob = uintsA |> ByteArray.convertUint8sToBytes |> Result.ExtractOrThrow
        let uintsAback = blob |> ByteArray.convertBytesToUint8s |> Result.ExtractOrThrow
        let expected = uintsA |> Array.toList

        Assert.AreEqual(expected, uintsAback |> Array.toList)
