namespace Gort.DataConvert.Test

open Microsoft.VisualStudio.TestTools.UnitTesting
open DomainTables
open System

[<TestClass>]
type sorterEvalDtoFixture() =


    [<TestMethod>]
    member this.sorterEvalDto() = 
        let sorterSpeed =
            SorterSpeed.create
                (11 |> SwitchCount.create)
                (12 |> StageCount.create)

        let sorterPerf = true |> sorterPerf.IsSuccessful
        let sorterPhenotypeId = SorterPhenotypeId.create (Guid.NewGuid())
        let sortableSetId = Guid.NewGuid() |> SortableSetId.create
        let sorterId = Guid.NewGuid() |> SorterId.create

        let sorterEval = 
            SorterEval.make
             ("errorM" |> Some)
             ([|1;2;3;|] |> SwitchUseCounts.make |> Some)
             (sorterSpeed |> Some)
             (sorterPerf |> Some)
             (sorterPhenotypeId |> Some)
             sortableSetId
             sorterId

        let sorterEvalJson = sorterEval |> SorterEvalDto.toJson
        let sorterEvalBack = sorterEvalJson 
                             |> SorterEvalDto.fromJson
                             |> Result.ExtractOrThrow
        Assert.IsTrue(CollectionProps.areEqual sorterEval sorterEvalBack)
