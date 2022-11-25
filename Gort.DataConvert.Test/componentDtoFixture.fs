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


    [<TestMethod>]
    member this.SorterUniformMutatorDto() =
        let mutationRat = 0.55 |> MutationRate.create
        let stageRflMutato = SorterUniformMutator.mutateByStageRfl mutationRat
        let stageRflMutatorCereal = stageRflMutato |> SorterUniformMutatorDto.toJson
        let stageRflMutatoBack = stageRflMutatorCereal 
                                    |> SorterUniformMutatorDto.fromJson
                                    |> Result.ExtractOrThrow
        Assert.AreEqual(
            stageRflMutato |> SorterUniformMutator.getMutationRate,
            stageRflMutatoBack |> SorterUniformMutator.getMutationRate)

        Assert.AreEqual(
            stageRflMutato |> SorterUniformMutator.getSorterUniformMutatorType,
            stageRflMutatoBack |> SorterUniformMutator.getSorterUniformMutatorType)

