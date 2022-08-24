namespace Gort.Sorting.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type SortableSetFixture () =

    [<TestMethod>]
    member this.makeAllBits () =
        let order = Order.createNr 10
        let ssRecId = 123 |> SortableSetId.create
        let ssFmtRu8 = SortableSetFormat.makeRollout rolloutFormat.RfU8
        let ssFmtRu16 = SortableSetFormat.makeRollout rolloutFormat.RfU16
        let ssFmtRI32 = SortableSetFormat.makeRollout rolloutFormat.RfI32
        let ssFmtRu64 = SortableSetFormat.makeRollout rolloutFormat.RfU64
        let ssFmtBs = SortableSetFormat.makeBitStriped

        let ssRu8 = SortableSet.makeAllBits ssRecId ssFmtRu8 order |> Result.ExtractOrThrow
        let ssRu16 = SortableSet.makeAllBits ssRecId ssFmtRu16 order |> Result.ExtractOrThrow
        let ssRI32 = SortableSet.makeAllBits ssRecId ssFmtRI32 order |> Result.ExtractOrThrow
        let ssRu64 = SortableSet.makeAllBits ssRecId ssFmtRu64 order |> Result.ExtractOrThrow
        let ssBs = SortableSet.makeAllBits ssRecId ssFmtBs order |> Result.ExtractOrThrow

        let srtIntsRu8 = ssRu8 |> SortableSet.toSortableIntsArrays |> Seq.toArray
        let srtIntsRu16 = ssRu16 |> SortableSet.toSortableIntsArrays |> Seq.toArray
        let srtIntsRI32 = ssRI32 |> SortableSet.toSortableIntsArrays |> Seq.toArray
        let srtIntsRu64 = ssRu64 |> SortableSet.toSortableIntsArrays |> Seq.toArray
        let srtIntsBs = ssBs |> SortableSet.toSortableIntsArrays |> Seq.toArray

        Assert.IsTrue(CollectionProps.areEqual srtIntsRu8 srtIntsRu16);
        Assert.IsTrue(CollectionProps.areEqual srtIntsRu8 srtIntsRI32);
        Assert.IsTrue(CollectionProps.areEqual srtIntsRu8 srtIntsRu64);
        Assert.IsTrue(CollectionProps.areEqual srtIntsRu8 srtIntsBs);


    [<TestMethod>]
    member this.makeOrbit () =
        let ssRecId = 123 |> SortableSetId.create
        let order = Order.createNr 10
        let seed = RandomSeed.create 1123
        let randy = Rando.create rngType.Lcg (seed)
        let perm = Permutation.createRandom order randy
        let maxCount = Some (SortableCount.create 12)

        let ssFmtRu8 = SortableSetFormat.makeRollout rolloutFormat.RfU8
        let ssFmtRu16 = SortableSetFormat.makeRollout rolloutFormat.RfU16
        let ssFmtRI32 = SortableSetFormat.makeRollout rolloutFormat.RfI32
        let ssFmtRu64 = SortableSetFormat.makeRollout rolloutFormat.RfU64
        let ssFmtBs = SortableSetFormat.makeBitStriped

        let ssRu8 = SortableSet.makeOrbits ssRecId ssFmtRu8 maxCount perm |> Result.ExtractOrThrow
        let ssRu16 = SortableSet.makeOrbits ssRecId ssFmtRu16 maxCount perm |> Result.ExtractOrThrow
        let ssRI32 = SortableSet.makeOrbits ssRecId ssFmtRI32 maxCount perm |> Result.ExtractOrThrow
        let ssRu64 = SortableSet.makeOrbits ssRecId ssFmtRu64 maxCount perm |> Result.ExtractOrThrow
        let ssBs = SortableSet.makeOrbits ssRecId ssFmtBs maxCount perm |> Result.ExtractOrThrow

        let srtIntsRu8 = ssRu8 |> SortableSet.toSortableIntsArrays |> Seq.toArray
        let srtIntsRu16 = ssRu16 |> SortableSet.toSortableIntsArrays |> Seq.toArray
        let srtIntsRI32 = ssRI32 |> SortableSet.toSortableIntsArrays |> Seq.toArray
        let srtIntsRu64 = ssRu64 |> SortableSet.toSortableIntsArrays |> Seq.toArray
        let srtIntsBs = ssBs |> SortableSet.toSortableIntsArrays |> Seq.toArray

        Assert.IsTrue(CollectionProps.areEqual srtIntsRu8 srtIntsRu16);
        Assert.IsTrue(CollectionProps.areEqual srtIntsRu8 srtIntsRI32);
        Assert.IsTrue(CollectionProps.areEqual srtIntsRu8 srtIntsRu64);
        Assert.IsTrue(srtIntsBs.Length > srtIntsRu8.Length);

    [<TestMethod>]
    member this.makeSortedStacks() =
        let ord = Order.createNr 16
        let ssRecId = 123 |> SortableSetId.create
        let orderStack = [Order.create 8; Order.create 4; Order.create 2; Order.create 2]
                          |> Result.sequence
                          |> Result.ExtractOrThrow
                          |> List.toArray

        let ssFmtRu8 = SortableSetFormat.makeRollout rolloutFormat.RfU8
        let ssFmtRu16 = SortableSetFormat.makeRollout rolloutFormat.RfU16
        let ssFmtRI32 = SortableSetFormat.makeRollout rolloutFormat.RfI32
        let ssFmtRu64 = SortableSetFormat.makeRollout rolloutFormat.RfU64
        let ssFmtBs = SortableSetFormat.makeBitStriped

        let ssRu8 = SortableSet.makeSortedStacks ssRecId ssFmtRu8 orderStack |> Result.ExtractOrThrow
        let ssRu16 = SortableSet.makeSortedStacks ssRecId ssFmtRu16 orderStack |> Result.ExtractOrThrow
        let ssRI32 = SortableSet.makeSortedStacks ssRecId ssFmtRI32 orderStack |> Result.ExtractOrThrow
        let ssRu64 = SortableSet.makeSortedStacks ssRecId ssFmtRu64 orderStack |> Result.ExtractOrThrow
        let ssBs = SortableSet.makeSortedStacks ssRecId ssFmtBs orderStack |> Result.ExtractOrThrow

        let srtIntsRu8 = ssRu8 |> SortableSet.toSortableIntsArrays |> Seq.toArray
        let srtIntsRu16 = ssRu16 |> SortableSet.toSortableIntsArrays |> Seq.toArray
        let srtIntsRI32 = ssRI32 |> SortableSet.toSortableIntsArrays |> Seq.toArray
        let srtIntsRu64 = ssRu64 |> SortableSet.toSortableIntsArrays |> Seq.toArray
        let srtIntsBs = ssBs |> SortableSet.toSortableIntsArrays |> Seq.toArray

        Assert.IsTrue(CollectionProps.areEqual srtIntsRu8 srtIntsRu16);
        Assert.IsTrue(CollectionProps.areEqual srtIntsRu8 srtIntsRI32);
        Assert.IsTrue(CollectionProps.areEqual srtIntsRu8 srtIntsRu64);
        Assert.IsTrue(CollectionProps.areEqual srtIntsRu8 srtIntsBs);


    [<TestMethod>]
    member this.makeRandom() =
        let order = Order.createNr 16
        let ssRecId = 123 |> SortableSetId.create
        let sortableCount = SortableCount.create 10
        let _randy () =
            Rando.create rngType.Lcg (123 |> RandomSeed.create)


        let ssFmtRu8 = SortableSetFormat.makeRollout rolloutFormat.RfU8
        let ssFmtRu16 = SortableSetFormat.makeRollout rolloutFormat.RfU16
        let ssFmtRI32 = SortableSetFormat.makeRollout rolloutFormat.RfI32
        let ssFmtRu64 = SortableSetFormat.makeRollout rolloutFormat.RfU64
        let ssFmtBs = SortableSetFormat.makeBitStriped

        let ssRu8 = SortableSet.makeRandomPermutation ssRecId ssFmtRu8 order sortableCount (_randy()) |> Result.ExtractOrThrow
        let ssRu16 = SortableSet.makeRandomPermutation ssRecId ssFmtRu16 order sortableCount (_randy())|> Result.ExtractOrThrow
        let ssRI32 = SortableSet.makeRandomPermutation ssRecId ssFmtRI32 order sortableCount (_randy()) |> Result.ExtractOrThrow
        let ssRu64 = SortableSet.makeRandomPermutation ssRecId ssFmtRu64 order sortableCount (_randy()) |> Result.ExtractOrThrow
        let ssBs = SortableSet.makeRandomPermutation ssRecId ssFmtBs order sortableCount (_randy()) |> Result.ExtractOrThrow

        let srtIntsRu8 = ssRu8 |> SortableSet.toSortableIntsArrays |> Seq.toArray
        let srtIntsRu16 = ssRu16 |> SortableSet.toSortableIntsArrays |> Seq.toArray
        let srtIntsRI32 = ssRI32 |> SortableSet.toSortableIntsArrays |> Seq.toArray
        let srtIntsRu64 = ssRu64 |> SortableSet.toSortableIntsArrays |> Seq.toArray
        let srtIntsBs = ssBs |> SortableSet.toSortableIntsArrays |> Seq.toArray

        Assert.IsTrue(CollectionProps.areEqual srtIntsRu8 srtIntsRu16);
        Assert.IsTrue(CollectionProps.areEqual srtIntsRu8 srtIntsRI32);
        Assert.IsTrue(CollectionProps.areEqual srtIntsRu8 srtIntsRu64);
        Assert.IsTrue(srtIntsBs.Length > srtIntsRu8.Length);