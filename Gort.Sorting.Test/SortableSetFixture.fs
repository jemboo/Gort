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
        let res8 = SortableSetO.makeAllBits ord byteWidth8 |> Result.ExtractOrThrow
        let rs16 = SortableSetO.makeAllBits ord byteWidth16 |> Result.ExtractOrThrow
        let rs64 = SortableSetO.makeAllBits ord byteWidth64 |> Result.ExtractOrThrow

        Assert.IsTrue(res8.rollout |> RolloutO.getByteWidth |> ByteWidth.value = 1);
        Assert.IsTrue(rs16.rollout |> RolloutO.getByteWidth |> ByteWidth.value = 2);
        Assert.IsTrue(rs64.rollout |> RolloutO.getByteWidth |> ByteWidth.value = 8);


    [<TestMethod>]
    member this.makeOrbit () =
        let ord = Order.create 16 |> Result.ExtractOrThrow
        let seed = RandomSeed.create 1123
        let randy = Rando.create rngType.Lcg (seed)
        let perm = Permutation.createRandom ord randy

        let byteWidth8 = ByteWidth.create 1 |> Result.ExtractOrThrow
        let byteWidth16 = ByteWidth.create 2 |> Result.ExtractOrThrow
        let byteWidth64 = ByteWidth.create 8 |> Result.ExtractOrThrow
        let res8 = SortableSetO.makeOrbits None byteWidth8 perm  |> Result.ExtractOrThrow
        let rs16 = SortableSetO.makeOrbits None byteWidth16 perm |> Result.ExtractOrThrow
        let rs64 = SortableSetO.makeOrbits None byteWidth64 perm |> Result.ExtractOrThrow
        
        Assert.AreEqual(res8.rollout |> RolloutO.getChunkCount |> SymbolCount.value, 12);
        Assert.AreEqual(rs16.rollout |> RolloutO.getChunkCount |> SymbolCount.value, 12);
        Assert.AreEqual(rs64.rollout |> RolloutO.getChunkCount |> SymbolCount.value, 1);


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

        let res8 = SortableSetO.makeSortedStacks byteWidth8 degGrp
                     |> Result.ExtractOrThrow
        let res16 = SortableSetO.makeSortedStacks byteWidth16 degGrp
                     |> Result.ExtractOrThrow
        let res64 = SortableSetO.makeSortedStacks byteWidth64 degGrp
                     |> Result.ExtractOrThrow

        Assert.AreEqual(res8.rollout |> RolloutO.getChunkCount |> SymbolCount.value, 405);
        Assert.AreEqual(res16.rollout |> RolloutO.getChunkCount |> SymbolCount.value, 405);
        Assert.AreEqual(res64.rollout |> RolloutO.getChunkCount |> SymbolCount.value, 7);



    [<TestMethod>]
    member this.makeRandom() =
        let order = Order.create 16 |> Result.ExtractOrThrow
        let randy = Rando.create rngType.Lcg (123 |> RandomSeed.create)
        let sortableCt = 129 |> SortableCount.create
        let byteWidth8 = ByteWidth.create 1 |> Result.ExtractOrThrow
        let byteWidth16 = ByteWidth.create 2 |> Result.ExtractOrThrow
        let byteWidth64 = ByteWidth.create 8 |> Result.ExtractOrThrow

        let res8 = SortableSetO.makeRandom order byteWidth8 randy sortableCt
                    |> Result.ExtractOrThrow
        let res16 = SortableSetO.makeRandom order byteWidth16 randy sortableCt
                    |> Result.ExtractOrThrow
        let res64 = SortableSetO.makeRandom order byteWidth64 randy sortableCt
                    |> Result.ExtractOrThrow

        Assert.AreEqual(res8.rollout |> RolloutO.getChunkCount |> SymbolCount.value, 129);
        Assert.AreEqual(res16.rollout |> RolloutO.getChunkCount |> SymbolCount.value, 129);
        Assert.AreEqual(res64.rollout |> RolloutO.getChunkCount |> SymbolCount.value, 7);


    [<TestMethod>]
    member this.fromSortableInts() =
        let order = Order.create 3 |> Result.ExtractOrThrow
        let arOfIntAr = [| [|1111;11112;11113|]; [|2211;12;13|]; [|2221;22;23|]; [|5555531;32;33|]; |]
        let sortableCount = arOfIntAr.Length |> SortableCount.create
        let symbolSetSize = 555553133 |> uint64 |> SymbolSetSize.createNr
        let siInt = arOfIntAr |> Array.map(SortableInts.make symbolSetSize)
        let ssfU8 = rolloutFormat.RfI32 |> sortableSetFormat.SsfArrayRoll
        let sortableSet = SortableSet.fromSortableIntsArrays ssfU8 order symbolSetSize sortableCount siInt
                           |> Result.ExtractOrThrow
        let arOfIntArBack = sortableSet |> SortableSet.toSortableIntsArrays 
                                        |> Seq.map(SortableInts.value)
                                        |> Seq.toArray


        Assert.IsTrue(CollectionProps.areEqual arOfIntAr arOfIntArBack);