namespace Gort.DataConvert.Test
open Microsoft.VisualStudio.TestTools.UnitTesting
open DomainTables

[<TestClass>]
type dtoFixture () =

    [<TestMethod>]
    member this.RngGenDto() =
        let rngGen = {  rngGen.rngType = rngType.Lcg; 
                        seed = RandomSeed.create 123  }
        let dto = RngGenDto.toDto rngGen
        let rngGenBack = RngGenDto.fromDto dto |> Result.ExtractOrThrow
        Assert.AreEqual(rngGen, rngGenBack)

        let rngGen2 = {  rngGen.rngType = rngType.Net; 
                         seed = RandomSeed.create 123  }
        let dto2 = RngGenDto.toDto rngGen2
        let rngGenBack2 = RngGenDto.fromDto dto2 |> Result.ExtractOrThrow
        Assert.AreEqual(rngGen2, rngGenBack2)

    [<TestMethod>]
    member this.sb() =
        let qua = nameof sortableSetVersion.Orbit
        Assert.IsTrue(true)

