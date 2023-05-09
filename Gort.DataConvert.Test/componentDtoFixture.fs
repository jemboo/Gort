namespace Gort.DataConvert.Test

open Microsoft.VisualStudio.TestTools.UnitTesting
open DomainTables

[<TestClass>]
type componentDtoFixture() =

    [<TestMethod>]
    member this.RngGenDto() =
        let rngGen = RngGen.create rngType.Lcg (RandomSeed.create 123)
        let dto = RngGenDto.toDto rngGen
        let rngGenBack = RngGenDto.fromDto dto |> Result.ExtractOrThrow
        Assert.AreEqual(rngGen, rngGenBack)

        let rngGen2 = RngGen.create rngType.Net (RandomSeed.create 123)

        let dto2 = RngGenDto.toDto rngGen2
        let rngGenBack2 = RngGenDto.fromDto dto2 |> Result.ExtractOrThrow
        Assert.AreEqual(rngGen2, rngGenBack2)