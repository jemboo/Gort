namespace Gort.Sorting.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type SortableSetFixture () =

    [<TestMethod>]
    member this.makeAllBits () =
        let order = Order.createNr 10

        let ssFmtRu8 = SortableSetFormat.makeRollout rolloutFormat.RfU8
        let ssFmtRu16 = SortableSetFormat.makeRollout rolloutFormat.RfU16
        let ssFmtRI32 = SortableSetFormat.makeRollout rolloutFormat.RfI32
        let ssFmtRu64 = SortableSetFormat.makeRollout rolloutFormat.RfU64
        let ssFmtBs = SortableSetFormat.makeBitStriped(false |> ExpandBitSets.create)

        let ssRu8 = SortableSet.makeAllBits ssFmtRu8 order |> Result.ExtractOrThrow
        let ssRu16 = SortableSet.makeAllBits ssFmtRu16 order |> Result.ExtractOrThrow
        let ssRI32 = SortableSet.makeAllBits ssFmtRI32 order |> Result.ExtractOrThrow
        let ssRu64 = SortableSet.makeAllBits ssFmtRu64 order |> Result.ExtractOrThrow
        let ssBs = SortableSet.makeAllBits ssFmtBs order |> Result.ExtractOrThrow

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
        let order = Order.createNr 10
        let seed = RandomSeed.create 1123
        let randy = Rando.create rngType.Lcg (seed)
        let perm = Permutation.createRandom order randy
        let maxCount = Some (SortableCount.create 12)

        let ssFmtRu8 = SortableSetFormat.makeRollout rolloutFormat.RfU8
        let ssFmtRu16 = SortableSetFormat.makeRollout rolloutFormat.RfU16
        let ssFmtRI32 = SortableSetFormat.makeRollout rolloutFormat.RfI32
        let ssFmtRu64 = SortableSetFormat.makeRollout rolloutFormat.RfU64
        let ssFmtBs = SortableSetFormat.makeBitStriped(true |> ExpandBitSets.create)

        let ssRu8 = SortableSet.makeOrbits ssFmtRu8 maxCount perm |> Result.ExtractOrThrow
        let ssRu16 = SortableSet.makeOrbits ssFmtRu16 maxCount perm |> Result.ExtractOrThrow
        let ssRI32 = SortableSet.makeOrbits ssFmtRI32 maxCount perm |> Result.ExtractOrThrow
        let ssRu64 = SortableSet.makeOrbits ssFmtRu64 maxCount perm |> Result.ExtractOrThrow
        let ssBs = SortableSet.makeOrbits ssFmtBs maxCount perm |> Result.ExtractOrThrow

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
        let orderStack = [Order.create 8; Order.create 4; Order.create 2; Order.create 2]
                          |> Result.sequence
                          |> Result.ExtractOrThrow
                          |> List.toArray

        let ssFmtRu8 = SortableSetFormat.makeRollout rolloutFormat.RfU8
        let ssFmtRu16 = SortableSetFormat.makeRollout rolloutFormat.RfU16
        let ssFmtRI32 = SortableSetFormat.makeRollout rolloutFormat.RfI32
        let ssFmtRu64 = SortableSetFormat.makeRollout rolloutFormat.RfU64
        let ssFmtBs = SortableSetFormat.makeBitStriped(false |> ExpandBitSets.create)

        let ssRu8 = SortableSet.makeSortedStacks ssFmtRu8 orderStack |> Result.ExtractOrThrow
        let ssRu16 = SortableSet.makeSortedStacks ssFmtRu16 orderStack |> Result.ExtractOrThrow
        let ssRI32 = SortableSet.makeSortedStacks ssFmtRI32 orderStack |> Result.ExtractOrThrow
        let ssRu64 = SortableSet.makeSortedStacks ssFmtRu64 orderStack |> Result.ExtractOrThrow
        let ssBs = SortableSet.makeSortedStacks ssFmtBs orderStack |> Result.ExtractOrThrow

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
        let sortableCount = SortableCount.create 10
        let _randy () =
            Rando.create rngType.Lcg (123 |> RandomSeed.create)


        let ssFmtRu8 = SortableSetFormat.makeRollout rolloutFormat.RfU8
        let ssFmtRu16 = SortableSetFormat.makeRollout rolloutFormat.RfU16
        let ssFmtRI32 = SortableSetFormat.makeRollout rolloutFormat.RfI32
        let ssFmtRu64 = SortableSetFormat.makeRollout rolloutFormat.RfU64
        let ssFmtBs = SortableSetFormat.makeBitStriped(true |> ExpandBitSets.create)

        let ssRu8 = SortableSet.makeRandomPermutation ssFmtRu8 order sortableCount (_randy()) |> Result.ExtractOrThrow
        let ssRu16 = SortableSet.makeRandomPermutation ssFmtRu16 order sortableCount (_randy())|> Result.ExtractOrThrow
        let ssRI32 = SortableSet.makeRandomPermutation ssFmtRI32 order sortableCount (_randy()) |> Result.ExtractOrThrow
        let ssRu64 = SortableSet.makeRandomPermutation ssFmtRu64 order sortableCount (_randy()) |> Result.ExtractOrThrow
        let ssBs = SortableSet.makeRandomPermutation ssFmtBs order sortableCount (_randy()) |> Result.ExtractOrThrow

        let srtIntsRu8 = ssRu8 |> SortableSet.toSortableIntsArrays |> Seq.toArray
        let srtIntsRu16 = ssRu16 |> SortableSet.toSortableIntsArrays |> Seq.toArray
        let srtIntsRI32 = ssRI32 |> SortableSet.toSortableIntsArrays |> Seq.toArray
        let srtIntsRu64 = ssRu64 |> SortableSet.toSortableIntsArrays |> Seq.toArray
        let srtIntsBs = ssBs |> SortableSet.toSortableIntsArrays |> Seq.toArray

        Assert.IsTrue(CollectionProps.areEqual srtIntsRu8 srtIntsRu16);
        Assert.IsTrue(CollectionProps.areEqual srtIntsRu8 srtIntsRI32);
        Assert.IsTrue(CollectionProps.areEqual srtIntsRu8 srtIntsRu64);
        Assert.IsTrue(srtIntsBs.Length > srtIntsRu8.Length);