namespace Gort.Sorting.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type SortableSetFixture() =

    [<TestMethod>]
    member this.makeAllBits() =

        let order = Order.createNr 10
        let ssRecId = Guid.NewGuid() |> SortableSetId.create
        let ssFmtRu8 = rolloutFormat.RfU8
        let ssFmtRu16 = rolloutFormat.RfU16
        let ssFmtRI32 = rolloutFormat.RfI32
        let ssFmtRu64 = rolloutFormat.RfU64
        let ssFmtBs = rolloutFormat.RfBs64

        let ssRu8 = SortableSet.makeAllBits ssRecId ssFmtRu8 order |> Result.ExtractOrThrow

        let ssRu16 =
            SortableSet.makeAllBits ssRecId ssFmtRu16 order |> Result.ExtractOrThrow

        let ssRI32 =
            SortableSet.makeAllBits ssRecId ssFmtRI32 order |> Result.ExtractOrThrow

        let ssRu64 =
            SortableSet.makeAllBits ssRecId ssFmtRu64 order |> Result.ExtractOrThrow

        let ssBs = 
            SortableSet.makeAllBits ssRecId ssFmtBs order |> Result.ExtractOrThrow

        let srtIntsRu8 = ssRu8 |> SortableSet.toSortableIntsArrays |> Seq.toArray
        let srtIntsRu16 = ssRu16 |> SortableSet.toSortableIntsArrays |> Seq.toArray
        let srtIntsRI32 = ssRI32 |> SortableSet.toSortableIntsArrays |> Seq.toArray
        let srtIntsRu64 = ssRu64 |> SortableSet.toSortableIntsArrays |> Seq.toArray
        let srtIntsBs = ssBs |> SortableSet.toSortableIntsArrays |> Seq.toArray

        Assert.IsTrue(CollectionProps.areEqual srtIntsRu8 srtIntsRu16)
        Assert.IsTrue(CollectionProps.areEqual srtIntsRu8 srtIntsRI32)
        Assert.IsTrue(CollectionProps.areEqual srtIntsRu8 srtIntsRu64)
        Assert.IsTrue(CollectionProps.areEqual srtIntsRu8 srtIntsBs)


    [<TestMethod>]
    member this.makeOrbit() =
        let ssRecId = Guid.NewGuid() |> SortableSetId.create
        let order = Order.createNr 10
        let seed = RandomSeed.create 1123
        let randy = Rando.create rngType.Lcg (seed)
        let perm = Permutation.createRandom order randy
        let maxCount = Some(SortableCount.create 12)

        let ssRI32 =
            SortableSet.makeOrbits ssRecId maxCount perm |> Result.ExtractOrThrow

        let srtIntsRI32 = ssRI32 |> SortableSet.toSortableIntsArrays |> Seq.toArray

        Assert.IsTrue(srtIntsRI32.Length > 0)


    [<TestMethod>]
    member this.makeSortedStacks() =
        let ord = Order.createNr 16
        let ssRecId = Guid.NewGuid() |> SortableSetId.create

        let orderStack =
            [ Order.create 8; Order.create 4; Order.create 2; Order.create 2 ]
            |> Result.sequence
            |> Result.ExtractOrThrow
            |> List.toArray

        let ssFmtRu8 = rolloutFormat.RfU8
        let ssFmtRu16 = rolloutFormat.RfU16
        let ssFmtRI32 = rolloutFormat.RfI32
        let ssFmtRu64 = rolloutFormat.RfU64
        let ssFmtBs = rolloutFormat.RfBs64

        let ssRu8 =
            SortableSet.makeSortedStacks ssRecId ssFmtRu8 orderStack
            |> Result.ExtractOrThrow

        let ssRu16 =
            SortableSet.makeSortedStacks ssRecId ssFmtRu16 orderStack
            |> Result.ExtractOrThrow

        let ssRI32 =
            SortableSet.makeSortedStacks ssRecId ssFmtRI32 orderStack
            |> Result.ExtractOrThrow

        let ssRu64 =
            SortableSet.makeSortedStacks ssRecId ssFmtRu64 orderStack
            |> Result.ExtractOrThrow

        let ssBs =
            SortableSet.makeSortedStacks ssRecId ssFmtBs orderStack |> Result.ExtractOrThrow

        let srtIntsRu8 = ssRu8 |> SortableSet.toSortableIntsArrays |> Seq.toArray
        let srtIntsRu16 = ssRu16 |> SortableSet.toSortableIntsArrays |> Seq.toArray
        let srtIntsRI32 = ssRI32 |> SortableSet.toSortableIntsArrays |> Seq.toArray
        let srtIntsRu64 = ssRu64 |> SortableSet.toSortableIntsArrays |> Seq.toArray
        let srtIntsBs = ssBs |> SortableSet.toSortableIntsArrays |> Seq.toArray

        Assert.IsTrue(CollectionProps.areEqual srtIntsRu8 srtIntsRu16)
        Assert.IsTrue(CollectionProps.areEqual srtIntsRu8 srtIntsRI32)
        Assert.IsTrue(CollectionProps.areEqual srtIntsRu8 srtIntsRu64)
        Assert.IsTrue(CollectionProps.areEqual srtIntsRu8 srtIntsBs)


    [<TestMethod>]
    member this.makeRandom() =
        let order = Order.createNr 16
        let ssRecId = Guid.NewGuid() |> SortableSetId.create
        let sortableCount = SortableCount.create 10

        let _randy () =
            Rando.create rngType.Lcg (123 |> RandomSeed.create)

        let ssRI32 =
            SortableSet.makeRandomPermutation order sortableCount (_randy ()) ssRecId
            |> Result.ExtractOrThrow


        let srtIntsRI32 = ssRI32 |> SortableSet.toSortableIntsArrays |> Seq.toArray

        Assert.IsTrue(srtIntsRI32.Length > 0)


    [<TestMethod>]
    member this.makeSetOfSortableSetOrbit() =
        let order = Order.createNr 32
        let sOfSortableSetId = Guid.NewGuid() |> SetOfSortableSetId.create
        let sortableSetCount = SortableSetCount.create 100
        let maxSortableCount = None // SortableCount.create 10 |> Some
        let ssFmtRu8 = rolloutFormat.RfU8
        let rngGn = RngGen.create rngType.Lcg (67123 |> RandomSeed.create)

        let orbis = 
            SetOfSortableSet.makeRandomOrbits
              sOfSortableSetId
              sortableSetCount
              order
              maxSortableCount
              rngGn

        orbis |> SetOfSortableSet.getSummaryReports
              |> Seq.iter(Console.WriteLine)

        Assert.IsTrue(11 = 11)