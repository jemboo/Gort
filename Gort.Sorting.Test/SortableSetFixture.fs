namespace Gort.Sorting.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type SortableSetFixture () =

    [<TestMethod>]
    member this.makeAllBits () =
        let dg = Degree.create 16 |> Result.ExtractOrThrow
        let byteWidth8 = ByteWidth.create 1 |> Result.ExtractOrThrow
        let byteWidth16 = ByteWidth.create 2 |> Result.ExtractOrThrow
        let byteWidth64 = ByteWidth.create 8 |> Result.ExtractOrThrow
        let res8 = SortableSet.makeAllBits dg byteWidth8 |> Result.ExtractOrThrow
        let rs16 = SortableSet.makeAllBits dg byteWidth16 |> Result.ExtractOrThrow
        let rs64 = SortableSet.makeAllBits dg byteWidth64 |> Result.ExtractOrThrow

        Assert.IsTrue(res8.rollout |> Rollout.getRollWidth |> ByteWidth.value = 1);
        Assert.IsTrue(rs16.rollout |> Rollout.getRollWidth |> ByteWidth.value = 2);
        Assert.IsTrue(rs64.rollout |> Rollout.getRollWidth |> ByteWidth.value = 8);


    [<TestMethod>]
    member this.makeOrbit () =
        let dg = Degree.create 16 |> Result.ExtractOrThrow
        let randy = Rando.create rngType.Lcg (RandomSeed.fromNow())
        let perm = Permutation.createRandom dg randy

        let byteWidth8 = ByteWidth.create 1 |> Result.ExtractOrThrow
        let byteWidth16 = ByteWidth.create 2 |> Result.ExtractOrThrow
        let byteWidth64 = ByteWidth.create 8 |> Result.ExtractOrThrow
        let res8 = SortableSet.makeOrbits dg byteWidth8 perm |> Result.ExtractOrThrow
        let rs16 = SortableSet.makeOrbits dg byteWidth16 perm |> Result.ExtractOrThrow
        let rs64 = SortableSet.makeOrbits dg byteWidth64 perm |> Result.ExtractOrThrow

        Assert.IsTrue(true);


    [<TestMethod>]
    member this.makeSortedStacks() =
        let degree = Degree.create 16 |> Result.ExtractOrThrow
        let degGrp = [|Degree.create 8; Degree.create 4; Degree.create 2; Degree.create 2|]
                      |> Array.toList
                      |> Result.sequence
                      |> Result.ExtractOrThrow
                      |> List.toArray
        let byteWidth8 = ByteWidth.create 1 |> Result.ExtractOrThrow
        let byteWidth16 = ByteWidth.create 2 |> Result.ExtractOrThrow
        let byteWidth64 = ByteWidth.create 8 |> Result.ExtractOrThrow

        let res8 = SortableSet.makeSortedStacks degree byteWidth8 degGrp
                     |> Result.ExtractOrThrow
        let res16 = SortableSet.makeSortedStacks degree byteWidth8 degGrp
                     |> Result.ExtractOrThrow
        let res64 = SortableSet.makeSortedStacks degree byteWidth8 degGrp
                     |> Result.ExtractOrThrow

        Assert.IsTrue(true);