namespace Gort.Core.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type ByteArrayFixture () =

    [<TestMethod>]
    member this.filterByPickList () =
        let data = [|0uL; 1uL; 2uL; 3uL; 4uL; 5uL;|]
        let picks = [|true; false; true; true; false; true;|]
        let expected = [|0uL; 2uL; 3uL; 5uL;|]
        let filtered = CollectionOps.filterByPickList data picks
                        |> Result.ExtractOrThrow
        Assert.AreEqual(expected |> Array.toList, filtered |> Array.toList);
        

    [<TestMethod>]
    member this.blobToRolloutS64 () =
        let uintsA = [|19912345123456uL; 5658543211234567uL; 119987654321089766uL |]
        let offset = 3
        let expected = uintsA |> Array.toList
        let blobIn = Array.zeroCreate<byte> (offset +  uintsA.Length * 8)
        let blobIn = blobIn |> (ByteArray.mapUint64arrayToBytes uintsA offset) |> Result.ExtractOrThrow
        let uintsAback = ByteArray.getUint64arrayFromBytes blobIn offset uintsA.Length |> Result.ExtractOrThrow

        Assert.AreEqual(expected, uintsAback |> Array.toList );



    [<TestMethod>]
    member this.blobToRolloutS32 () =
        let uintsA = [|199126ul; 565823457ul; 11089766ul |]
        let offset = 3
        let expected = uintsA |> Array.toList
        let blobIn = Array.zeroCreate<byte> (offset + uintsA.Length * 4)
        let blob = blobIn |> (ByteArray.mapUint32arrayToBytes uintsA offset) |> Result.ExtractOrThrow
        let uintsAback = ByteArray.getUint32arrayFromBytes blob offset uintsA.Length |> Result.ExtractOrThrow

        Assert.AreEqual(expected, uintsAback |> Array.toList );


    [<TestMethod>]
    member this.blobToRolloutS16 () =
        let uintsA = [|1996us; 5457us; 11066us |]
        let offset = 3
        let expected = uintsA |> Array.toList
        let blobIn = Array.zeroCreate<byte> (offset +  uintsA.Length * 2)
        let blobIn = blobIn |> (ByteArray.mapUint16arrayToBytes uintsA offset) |> Result.ExtractOrThrow
        let uintsAback = ByteArray.getUint16arrayFromBytes blobIn offset uintsA.Length |> Result.ExtractOrThrow

        Assert.AreEqual(expected, uintsAback |> Array.toList );


    [<TestMethod>]
    member this.blobToRolloutS8 () =
        let uintsA = [|196uy; 57uy; 110uy |]
        let offset = 3
        let expected = uintsA |> Array.toList
        let blobIn = Array.zeroCreate<byte> (offset + uintsA.Length)
        let blob = blobIn |> ByteArray.mapUint8arrayToBytes uintsA offset |> Result.ExtractOrThrow
        let uintsAback = ByteArray.getUint8arrayFromBytes blob offset uintsA.Length |> Result.ExtractOrThrow

        Assert.AreEqual(expected, uintsAback |> Array.toList );


    [<TestMethod>]
    member this.blobToRollout64 () =
        let uintV = 19912345123456uL
        let offset = 3
        let blobIn = Array.zeroCreate<byte> (offset * 2 * 8)
        let blobIn = blobIn |> (ByteArray.mapUint64toBytes uintV offset) |> Result.ExtractOrThrow
        let uintAback = blobIn |> ByteArray.getUint64fromBytes offset |> Result.ExtractOrThrow

        Assert.AreEqual(uintV, uintAback);


    [<TestMethod>]
    member this.blobToRollout32 () =
        let uintV = 199126ul
        let offset = 3
        let blobIn = Array.zeroCreate<byte> (offset * 2  * 4)
        let blobIn = blobIn |> (ByteArray.mapUint32toBytes uintV offset) |> Result.ExtractOrThrow
        let uintVback = blobIn |> ByteArray.getUint32FromBytes offset |> Result.ExtractOrThrow

        Assert.AreEqual(uintV, uintVback );


    [<TestMethod>]
    member this.blobToRollout16 () =
        let uintV = 1996us
        let offset = 3
        let blobIn = Array.zeroCreate<byte> (offset * 2  * 2)
        let blobIn = blobIn |> (ByteArray.mapUint16toBytes uintV offset) |> Result.ExtractOrThrow
        let uintVback = blobIn |> ByteArray.getUint16FromBytes offset |> Result.ExtractOrThrow

        Assert.AreEqual(uintV, uintVback);


    [<TestMethod>]
    member this.blobToRollout8 () =
        let uintV = 196uy
        let offset = 3
        let blobIn = Array.zeroCreate<byte> (offset * 2)
        let blobIn = blobIn |> ByteArray.mapUint8toBytes uintV offset |> Result.ExtractOrThrow
        let uintVback = blobIn |> ByteArray.getUint8FromBytes offset |> Result.ExtractOrThrow

        Assert.AreEqual(uintV, uintVback);


