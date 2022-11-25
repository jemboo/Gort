namespace Gort.Core.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type GuidUtilsFixture() =

    [<TestMethod>]
    member this.fromUint32s() =
        let g1 = 1234ul
        let g2 = 123456ul
        let g3 = 12345678ul
        let g4 = 1234567891ul

        let gu = GuidUtils.fromUint32s g1 g2 g3 g4

        let intsBack = GuidUtils.toUint32s gu

        Assert.AreEqual(intsBack.[0], g1)
        Assert.AreEqual(intsBack.[1], g2)
        Assert.AreEqual(intsBack.[2], g3)
        Assert.AreEqual(intsBack.[3], g4)



    [<TestMethod>]
    member this.guidFromBytes() =
        let bytesIn =
            seq {
                10uy
                20uy
                30uy
                40uy
                11uy
                21uy
                31uy
                41uy
                12uy
                22uy
                32uy
                42uy
                13uy
                23uy
                33uy
                43uy
            }
            |> Seq.toList

        let guid = GuidUtils.guidFromBytes (bytesIn |> List.toArray)
        let bytesOut = guid.ToByteArray() |> Array.toList

        Assert.AreEqual(bytesIn, bytesOut)


    [<TestMethod>]
    member this.getGuidFromBytes() =
        let bytesIn =
            seq {
                10uy
                20uy
                30uy
                40uy
                11uy
                21uy
                31uy
                41uy
                12uy
                22uy
                32uy
                42uy
                13uy
                23uy
                33uy
                43uy
            }
            |> Seq.toList

        let guid =
            GuidUtils.getGuidFromBytes 0 (bytesIn |> List.toArray) |> Result.ExtractOrThrow

        let bytesOut = guid.ToByteArray() |> Array.toList

        Assert.AreEqual(bytesIn, bytesOut)



    [<TestMethod>]
    member this.getGuidFromBytes4() =
        let offset = 4

        let bytesIn =
            seq {
                10uy
                20uy
                30uy
                40uy
                10uy
                20uy
                30uy
                40uy
                11uy
                21uy
                31uy
                41uy
                12uy
                22uy
                32uy
                42uy
                13uy
                23uy
                33uy
                43uy
                13uy
                23uy
                33uy
                43uy
            }
            |> Seq.toList

        let guid =
            GuidUtils.getGuidFromBytes offset (bytesIn |> List.toArray)
            |> Result.ExtractOrThrow

        let bytesOut = guid.ToByteArray() |> Array.toList

        let pieceIn = bytesIn.[offset .. offset + 15]

        Assert.AreEqual(pieceIn, bytesOut)



    [<TestMethod>]
    member this.mapBytesToGuids() =
        let bytesIn =
            seq {
                10uy
                20uy
                30uy
                40uy
                11uy
                21uy
                31uy
                41uy
                12uy
                22uy
                32uy
                42uy
                13uy
                23uy
                33uy
                43uy
                10uy
                20uy
                30uy
                40uy
                11uy
                21uy
                31uy
                41uy
                12uy
                22uy
                32uy
                42uy
                13uy
                23uy
                33uy
                43uy
                14uy
                20uy
                30uy
                40uy
                15uy
                21uy
                31uy
                41uy
                16uy
                22uy
                32uy
                42uy
                17uy
                23uy
                33uy
                43uy
                13uy
                14uy
                15uy
                16uy
            }
            |> Seq.toArray

        let guidsIn =
            seq {
                Guid.Empty
                Guid.Empty
                Guid.Empty
            }
            |> Seq.toArray

        let guidOffset = 0
        let blobOffset = 2
        let guidCt = guidsIn.Length


        let resGu = GuidUtils.mapBytesToGuids blobOffset guidsIn guidOffset guidCt bytesIn
        let guidsOut = resGu |> Result.ExtractOrThrow

        let bytesZa = Array.zeroCreate bytesIn.Length

        let resBytes =
            GuidUtils.mapGuidsToBytes guidOffset guidCt bytesZa blobOffset guidsOut

        let bytesOut = resBytes |> Result.ExtractOrThrow

        let inChk = bytesIn.[16..47] |> Array.toList
        let outChk = bytesOut.[16..47] |> Array.toList

        Assert.IsTrue(CollectionProps.areEqual inChk outChk)
        Assert.AreEqual(inChk, outChk)



    [<TestMethod>]
    member this.convertBytesToGuids() =
        let bytesIn =
            seq {
                10uy
                20uy
                30uy
                40uy
                11uy
                21uy
                31uy
                41uy
                12uy
                22uy
                32uy
                42uy
                13uy
                23uy
                33uy
                43uy
                10uy
                20uy
                30uy
                40uy
                11uy
                21uy
                31uy
                41uy
                12uy
                22uy
                32uy
                42uy
                13uy
                23uy
                33uy
                43uy
                14uy
                20uy
                30uy
                40uy
                15uy
                21uy
                31uy
                41uy
                16uy
                22uy
                32uy
                42uy
                17uy
                23uy
                33uy
                43uy
            }
            |> Seq.toArray

        let guidsIn =
            seq {
                Guid.Empty
                Guid.Empty
                Guid.Empty
            }
            |> Seq.toArray

        let resGuis = GuidUtils.convertBytesToGuids bytesIn
        let guidsOut = resGuis |> Result.ExtractOrThrow
        let resBytes = guidsOut |> GuidUtils.convertGuidsToBytes
        let bytesOut = resBytes |> Result.ExtractOrThrow

        Assert.AreEqual(bytesIn |> Array.toList, bytesOut |> Array.toList)
