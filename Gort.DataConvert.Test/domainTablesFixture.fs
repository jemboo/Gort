namespace Gort.DataConvert.Test
open Microsoft.VisualStudio.TestTools.UnitTesting
open Gort.DataStore.DataModel

[<TestClass>]
type domainTablesFixture () =

    let makeCause () =
        let causeR = new CauseR()
        causeR.Category <- "Category"
        causeR.Index <- 1
        causeR


    [<TestMethod>]
    member this.rngGenToRandGenR() =
        let causeR = makeCause()
        let causePath = "causePath"
        let seedV = 123
        let seed = seedV |> RandomSeed.create
        let rngT = rngType.Lcg
        let rngGen = { rngGen.rngType = rngT; seed = seed }
        let rndGenR = DomainTables.rngGenToComponentR rngGen causeR causePath
        let rngGenBack = rndGenR |> DomainTables.componentRToRngGen |> Result.ExtractOrThrow
        Assert.AreEqual(rngGen, rngGenBack)


    [<TestMethod>]
    member this.sorterMutatorToComponentR() =
        let causeR = makeCause()
        let causePath = "causePath"
        let stageMutRate = 0.7 |> StageMutationRate.create
        let sum = SorterUniformMutator.mutateByStage stageMutRate |> sorterMutator.Uniform
        let compR = DomainTables.sorterMutatorToComponentR sum causeR causePath
        let sumBackR = compR |> DomainTables.componentRToSorterMutator
        let sumBack = sumBackR |> Result.ExtractOrThrow
        let compRBack = DomainTables.sorterMutatorToComponentR sumBack causeR causePath
        Assert.AreEqual(compR.Json, compRBack.Json)