namespace Gort.DataConvert.Test

open Microsoft.VisualStudio.TestTools.UnitTesting
open DomainTables
open System

[<TestClass>]
type sortableSetDtoFixture() =


    [<TestMethod>]
    member this.testOrbit() =
        let ssRecId = Guid.NewGuid() |> SortableSetId.create
        let order = Order.createNr 10
        let seed = RandomSeed.create 1123
        let randy = Rando.create rngType.Lcg (seed)
        let perm = Permutation.createRandom order randy
        let maxCount = Some(SortableCount.create 12)

        let ssRI32 =
            SortableSet.makeOrbits ssRecId maxCount perm |> Result.ExtractOrThrow

        let ssJson = ssRI32 |> SortableSetDto.toJson

        let ssBack = ssJson |> SortableSetDto.fromJson
                            |> Result.ExtractOrThrow

        let orbOrig = ssRI32 |> SortableSet.toSortableIntsArrays
                             |> Seq.map(SortableIntArray.getValues >> Array.toList)
                             |> Seq.toList

        let orbBack = ssBack |> SortableSet.toSortableIntsArrays
                        |> Seq.map(SortableIntArray.getValues >> Array.toList)
                        |> Seq.toList

        Assert.AreEqual(orbOrig, orbBack)
