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
        let dg = Degree.createNr uLun.Length
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
        let mergedSet = Bitwise.mergeUpSeq (Degree.createNr lowDegree) 
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
        let deg = Degree.createNr 3
        let bitStrings = Bitwise.allBitPackForDegree deg |> Result.ExtractOrThrow
        Assert.AreEqual(Degree.value deg, Degree.value deg )
        Assert.AreEqual(bitStrings.Length, Degree.binExp deg )


    [<TestMethod>]
    member this.bitPackedtoBitStriped () =
        let deg = Degree.createNr 8
        let bpa = Bitwise.allBitPackForDegree deg |> Result.ExtractOrThrow
        let bsa = bpa |> Bitwise.bitPackedtoBitStriped deg |> Result.ExtractOrThrow
        let bpaBack = bsa |> Bitwise.bitStripedToBitPacked deg bpa.Length |> Result.ExtractOrThrow
        Assert.AreEqual(bpa |> Array.toList, bpaBack |> Array.toList)

        let deg2 = Degree.createNr 16
        let randy = Rando.fromRngGen (RngGen.lcgFromNow ())
        let bpa2 = Array.init 100 (fun _ -> RndGen.rndBitsUint64 deg2 randy)
        let bsa2 = bpa2 |> Bitwise.bitPackedtoBitStriped deg2 |> Result.ExtractOrThrow
        let bpaBack2 = bsa2 |> Bitwise.bitStripedToBitPacked deg2 bpa2.Length |> Result.ExtractOrThrow

        Assert.AreEqual(bpa2 |> Array.toList, bpaBack2 |> Array.toList)



    [<TestMethod>]
    member this.bitPackedtoBitStriped2D () =
        let deg2 = Degree.createNr 16
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
        let deg = Degree.createNr 8
        let bpa = Bitwise.allBitPackForDegree deg |> Result.ExtractOrThrow
        let bsa = bpa |> Bitwise.bitPackedtoBitStriped deg |> Result.ExtractOrThrow
        let bpaBack = bsa |> Bitwise.bitStripedToBitPacked deg bpa.Length |> Result.ExtractOrThrow
        let srtedBack = bpaBack |> Array.filter(fun srtbl -> srtbl |> Bitwise.isSorted |> not)
        Assert.AreEqual(1, 1);


    [<TestMethod>]
    member this.blobToRolloutS64 () =
        let uintsA = [|19912345123456uL; 5658543211234567uL; 119987654321089766uL |]
        let offset = 3
        let expected = uintsA |> Array.toList
        let blobIn = Array.zeroCreate<byte> (offset +  uintsA.Length * 8)
        let blobIn = blobIn |> (Bitwise.mapUint64arrayToBytes uintsA offset) |> Result.ExtractOrThrow
        let uintsAback = Bitwise.getUint64arrayFromBytes blobIn offset uintsA.Length |> Result.ExtractOrThrow

        Assert.AreEqual(expected, uintsAback |> Array.toList );



    [<TestMethod>]
    member this.blobToRolloutS32 () =
        let uintsA = [|199126ul; 565823457ul; 11089766ul |]
        let offset = 3
        let expected = uintsA |> Array.toList
        let blobIn = Array.zeroCreate<byte> (offset + uintsA.Length * 4)
        let blob = blobIn |> (Bitwise.mapUint32arrayToBytes uintsA offset) |> Result.ExtractOrThrow
        let uintsAback = Bitwise.getUint32arrayFromBytes blob offset uintsA.Length |> Result.ExtractOrThrow

        Assert.AreEqual(expected, uintsAback |> Array.toList );


    [<TestMethod>]
    member this.blobToRolloutS16 () =
        let uintsA = [|1996us; 5457us; 11066us |]
        let offset = 3
        let expected = uintsA |> Array.toList
        let blobIn = Array.zeroCreate<byte> (offset +  uintsA.Length * 2)
        let blobIn = blobIn |> (Bitwise.mapUint16arrayToBytes uintsA offset) |> Result.ExtractOrThrow
        let uintsAback = Bitwise.getUint16arrayFromBytes blobIn offset uintsA.Length |> Result.ExtractOrThrow

        Assert.AreEqual(expected, uintsAback |> Array.toList );


    [<TestMethod>]
    member this.blobToRolloutS8 () =
        let uintsA = [|196uy; 57uy; 110uy |]
        let offset = 3
        let expected = uintsA |> Array.toList
        let blobIn = Array.zeroCreate<byte> (offset + uintsA.Length)
        let blob = blobIn |> Bitwise.mapUint8arrayToBytes uintsA offset |> Result.ExtractOrThrow
        let uintsAback = Bitwise.getUint8arrayFromBytes blob offset uintsA.Length |> Result.ExtractOrThrow

        Assert.AreEqual(expected, uintsAback |> Array.toList );


    [<TestMethod>]
    member this.blobToRollout64 () =
        let uintV = 19912345123456uL
        let offset = 3
        let blobIn = Array.zeroCreate<byte> (offset * 2 * 8)
        let blobIn = blobIn |> (Bitwise.mapUint64toBytes uintV offset) |> Result.ExtractOrThrow
        let uintAback = blobIn |> Bitwise.getUint64fromBytes offset |> Result.ExtractOrThrow

        Assert.AreEqual(uintV, uintAback);


    [<TestMethod>]
    member this.blobToRollout32 () =
        let uintV = 199126ul
        let offset = 3
        let blobIn = Array.zeroCreate<byte> (offset * 2  * 4)
        let blobIn = blobIn |> (Bitwise.mapUint32toBytes uintV offset) |> Result.ExtractOrThrow
        let uintVback = blobIn |> Bitwise.getUint32FromBytes offset |> Result.ExtractOrThrow

        Assert.AreEqual(uintV, uintVback );


    [<TestMethod>]
    member this.blobToRollout16 () =
        let uintV = 1996us
        let offset = 3
        let blobIn = Array.zeroCreate<byte> (offset * 2  * 2)
        let blobIn = blobIn |> (Bitwise.mapUint16toBytes uintV offset) |> Result.ExtractOrThrow
        let uintVback = blobIn |> Bitwise.getUint16FromBytes offset |> Result.ExtractOrThrow

        Assert.AreEqual(uintV, uintVback);


    [<TestMethod>]
    member this.blobToRollout8 () =
        let uintV = 196uy
        let offset = 3
        let blobIn = Array.zeroCreate<byte> (offset * 2)
        let blobIn = blobIn |> Bitwise.mapUint8toBytes uintV offset |> Result.ExtractOrThrow
        let uintVback = blobIn |> Bitwise.getUint8FromBytes offset |> Result.ExtractOrThrow

        Assert.AreEqual(uintV, uintVback);



    [<TestMethod>]
    member this.hashObjs () =
        let uintsA = [|196uy; 57uy; 110uy |]
        let ooga = "ooga"
        let booga = [|19912345123456uL; 5658543211234567uL; 119987654321089766uL |]

        let expected = ByteUtils.hashObjs [|uintsA;ooga;uintsA;booga;|]
        let expected2 = ByteUtils.hashObjs [|ooga; null|]
        let expected3 = ByteUtils.hashObjs [|ooga;|]
        
        Assert.AreEqual(1,1);