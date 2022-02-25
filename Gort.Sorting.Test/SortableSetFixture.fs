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
        let seed = RandomSeed.create 1123
        let randy = Rando.create rngType.Lcg (seed)
        let perm = Permutation.createRandom dg randy

        let byteWidth8 = ByteWidth.create 1 |> Result.ExtractOrThrow
        let byteWidth16 = ByteWidth.create 2 |> Result.ExtractOrThrow
        let byteWidth64 = ByteWidth.create 8 |> Result.ExtractOrThrow
        let res8 = SortableSet.makeOrbits dg byteWidth8 perm |> Result.ExtractOrThrow
        let rs16 = SortableSet.makeOrbits dg byteWidth16 perm |> Result.ExtractOrThrow
        let rs64 = SortableSet.makeOrbits dg byteWidth64 perm |> Result.ExtractOrThrow
        
        Assert.AreEqual(res8.rollout |> Rollout.getRollCount |> RollCount.value, 12);
        Assert.AreEqual(rs16.rollout |> Rollout.getRollCount |> RollCount.value, 12);
        Assert.AreEqual(rs64.rollout |> Rollout.getRollCount |> RollCount.value, 1);


    [<TestMethod>]
    member this.makeSortedStacks() =
        let degree = Degree.create 16 |> Result.ExtractOrThrow
        let degGrp = [Degree.create 8; Degree.create 4; Degree.create 2; Degree.create 2]
                      |> Result.sequence
                      |> Result.ExtractOrThrow
                      |> List.toArray
        let byteWidth8 = ByteWidth.create 1 |> Result.ExtractOrThrow
        let byteWidth16 = ByteWidth.create 2 |> Result.ExtractOrThrow
        let byteWidth64 = ByteWidth.create 8 |> Result.ExtractOrThrow

        let res8 = SortableSet.makeSortedStacks degree byteWidth8 degGrp
                     |> Result.ExtractOrThrow
        let res16 = SortableSet.makeSortedStacks degree byteWidth16 degGrp
                     |> Result.ExtractOrThrow
        let res64 = SortableSet.makeSortedStacks degree byteWidth64 degGrp
                     |> Result.ExtractOrThrow

        Assert.AreEqual(res8.rollout |> Rollout.getRollCount |> RollCount.value, 405);
        Assert.AreEqual(res16.rollout |> Rollout.getRollCount |> RollCount.value, 405);
        Assert.AreEqual(res64.rollout |> Rollout.getRollCount |> RollCount.value, 7);