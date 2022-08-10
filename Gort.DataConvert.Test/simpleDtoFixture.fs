namespace Gort.DataConvert.Test
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type SimpleDtoFixture () =

    [<TestMethod>]
    member this.RngGenDto() =
        let rngGen = {
                        rngGen.rngType = rngType.Lcg; 
                        seed = RandomSeed.create 123
                     }
        let dto = RngGenDto.toDto rngGen
        let rngGenBack = RngGenDto.fromDto dto |> Result.ExtractOrThrow
        Assert.AreEqual(rngGen, rngGenBack)


    [<TestMethod>]
    member this.rngGenToRandGenRecord() =
        let rngGen = {
                        rngGen.rngType = rngType.Lcg;
                        seed = RandomSeed.create 123
                     }
        let causeId = 1
        let causePath = "causePath"
        let randGen = DomainTables.rngGenToRandGenRecord rngGen causeId causePath
        let rngGenBack = DomainTables.randGenRecordToRngGen randGen 
                            |> Result.ExtractOrThrow
        Assert.AreEqual(rngGen, rngGenBack)



    [<TestMethod>]
    member this.bitPackToBitPackRecord() =
        let arrayIn = [|1;12;123;1234;12345|]
        let symbolCount = arrayIn.Length |> SymbolCount.createNr
        let symbolSetSize = 22345uL |> SymbolSetSize.createNr
        let bitsPerSymbol = symbolSetSize |> BitsPerSymbol.fromSymbolSetSize
                                          |> Result.ExtractOrThrow

        let data = arrayIn |> ByteUtils.bitsFromSpIntPositions bitsPerSymbol
                           |> ByteUtils.storeBitSeqInBytes
                           |> Seq.toArray
        let bitPack = BitPack.create bitsPerSymbol symbolCount data

        let bitPackRecord = bitPack |> DomainTables.bitPackToBitPackRecord
        let bitPackBack = bitPackRecord |> DomainTables.bitPackRecordToBitPack
                                        |> Result.ExtractOrThrow
        Assert.AreEqual(bitPack, bitPackBack)

        let arrayBack = bitPackBack |> BitPack.getData 
                                    |> ByteUtils.getAllBitsFromByteSeq
                                    |> ByteUtils.bitsToSpIntPositions bitsPerSymbol
                                    |> Seq.toArray

        Assert.IsTrue(CollectionProps.areEqual arrayIn arrayBack)