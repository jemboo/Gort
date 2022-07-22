namespace Gort.DataConvert.Test
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type TestClass () =

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