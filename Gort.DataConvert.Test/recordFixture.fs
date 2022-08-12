namespace Gort.DataConvert.Test
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type recordFixture () =

    [<TestMethod>]
    member this.rngGenToRandGenRecord() =
        let rngGen = {
                        rngGen.rngType = rngType.Lcg;
                        seed = RandomSeed.create 123
                     }
        let causeId = 1
        let causePath = "causePath"
        let randGen = DomainTables.rngGenToRandGenR rngGen causeId causePath
        let rngGenBack = DomainTables.randGenRToRngGen randGen 
                            |> Result.ExtractOrThrow
        Assert.AreEqual(rngGen, rngGenBack)



    [<TestMethod>]
    member this.bitPackToBitPackR() =
        let arrayIn = [|1;12;123;1234;12345|]
        let symbolCount = arrayIn.Length |> SymbolCount.createNr
        let symbolSetSize = 22345uL |> SymbolSetSize.createNr
        let bitsPerSymbol = symbolSetSize |> BitsPerSymbol.fromSymbolSetSize
                                          |> Result.ExtractOrThrow

        let byteData = arrayIn |> ByteUtils.bitsFromSpIntPositions bitsPerSymbol
                               |> ByteUtils.storeBitSeqInBytes
                               |> Seq.toArray
        let bitPack = BitPack.create bitsPerSymbol symbolCount byteData

        let bitPackRecord = bitPack |> DomainTables.bitPackToBitPackR
        let bitPackBack = bitPackRecord |> DomainTables.bitPackRToBitPack
                                        |> Result.ExtractOrThrow
        Assert.AreEqual(bitPack, bitPackBack)

        let bpsFromRecord = bitPackRecord.BitsPerSymbol |> BitsPerSymbol.createNr
        let arrayBack = bitPackBack |> BitPack.getData 
                                    |> ByteUtils.getAllBitsFromByteSeq
                                    |> ByteUtils.bitsToSpIntPositions bpsFromRecord
                                    |> Seq.toArray

        Assert.IsTrue(CollectionProps.areEqual arrayIn arrayBack)