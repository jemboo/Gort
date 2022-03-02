namespace Gort.Sorting.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type SortableSetFixture () =

    [<TestMethod>]
    member this.makeAllBits () =
        let ord = Order.create 16 |> Result.ExtractOrThrow
        let byteWidth8 = ByteWidth.create 1 |> Result.ExtractOrThrow
        let byteWidth16 = ByteWidth.create 2 |> Result.ExtractOrThrow
        let byteWidth64 = ByteWidth.create 8 |> Result.ExtractOrThrow
        let res8 = SortableSet.makeAllBits ord byteWidth8 |> Result.ExtractOrThrow
        let rs16 = SortableSet.makeAllBits ord byteWidth16 |> Result.ExtractOrThrow
        let rs64 = SortableSet.makeAllBits ord byteWidth64 |> Result.ExtractOrThrow

        Assert.IsTrue(res8.rollout |> Rollout.getRollWidth |> ByteWidth.value = 1);
        Assert.IsTrue(rs16.rollout |> Rollout.getRollWidth |> ByteWidth.value = 2);
        Assert.IsTrue(rs64.rollout |> Rollout.getRollWidth |> ByteWidth.value = 8);


    [<TestMethod>]
    member this.makeOrbit () =
        let ord = Order.create 16 |> Result.ExtractOrThrow
        let seed = RandomSeed.create 1123
        let randy = Rando.create rngType.Lcg (seed)
        let perm = Permutation.createRandom ord randy

        let byteWidth8 = ByteWidth.create 1 |> Result.ExtractOrThrow
        let byteWidth16 = ByteWidth.create 2 |> Result.ExtractOrThrow
        let byteWidth64 = ByteWidth.create 8 |> Result.ExtractOrThrow
        let res8 = SortableSet.makeOrbits ord byteWidth8 perm |> Result.ExtractOrThrow
        let rs16 = SortableSet.makeOrbits ord byteWidth16 perm |> Result.ExtractOrThrow
        let rs64 = SortableSet.makeOrbits ord byteWidth64 perm |> Result.ExtractOrThrow
        
        Assert.AreEqual(res8.rollout |> Rollout.getRollCount |> RollCount.value, 12);
        Assert.AreEqual(rs16.rollout |> Rollout.getRollCount |> RollCount.value, 12);
        Assert.AreEqual(rs64.rollout |> Rollout.getRollCount |> RollCount.value, 1);


    [<TestMethod>]
    member this.makeSortedStacks() =
        let order = Order.create 16 |> Result.ExtractOrThrow
        let degGrp = [Order.create 8; Order.create 4; Order.create 2; Order.create 2]
                      |> Result.sequence
                      |> Result.ExtractOrThrow
                      |> List.toArray
        let byteWidth8 = ByteWidth.create 1 |> Result.ExtractOrThrow
        let byteWidth16 = ByteWidth.create 2 |> Result.ExtractOrThrow
        let byteWidth64 = ByteWidth.create 8 |> Result.ExtractOrThrow

        let res8 = SortableSet.makeSortedStacks order byteWidth8 degGrp
                     |> Result.ExtractOrThrow
        let res16 = SortableSet.makeSortedStacks order byteWidth16 degGrp
                     |> Result.ExtractOrThrow
        let res64 = SortableSet.makeSortedStacks order byteWidth64 degGrp
                     |> Result.ExtractOrThrow

        Assert.AreEqual(res8.rollout |> Rollout.getRollCount |> RollCount.value, 405);
        Assert.AreEqual(res16.rollout |> Rollout.getRollCount |> RollCount.value, 405);
        Assert.AreEqual(res64.rollout |> Rollout.getRollCount |> RollCount.value, 7);