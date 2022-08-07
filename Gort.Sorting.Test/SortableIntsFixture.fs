namespace Gort.Sorting.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type SortableIntsFixture () =

    [<TestMethod>]
    member this.fromSortableIntsU8() =
        let order = Order.create 3 |> Result.ExtractOrThrow
        let arOfIntAr = [| [|111;112;113|]; [|221;12;13|]; [|222;22;23|]; [|131;32;33|]; |]
        let sortableCount = arOfIntAr.Length |> SortableCount.create
        let symbolSetSize = 255 |> uint64 |> SymbolSetSize.createNr
        let siInt = arOfIntAr |> Array.map(SortableInts.make order symbolSetSize)
        let ssfU8 = rolloutFormat.RfU16 |> sortableSetFormat.SsfArrayRoll
        let sortableSet = SortableSet.fromSortableIntsArrays ssfU8 order symbolSetSize siInt
                           |> Result.ExtractOrThrow
        let arOfIntArBack = sortableSet |> SortableSet.toSortableIntsArrays 
                                        |> Seq.map(SortableInts.getValues)
                                        |> Seq.toArray

        Assert.IsTrue(CollectionProps.areEqual arOfIntAr arOfIntArBack);

    
    [<TestMethod>]
    member this.fromSortableIntsU16() =
        let order = Order.create 3 |> Result.ExtractOrThrow
        let arOfIntAr = [| [|1111;11112;11113|]; [|2211;12;13|]; [|2221;22;23|]; [|5531;32;33|]; |]
        let sortableCount = arOfIntAr.Length |> SortableCount.create
        let symbolSetSize = 5553 |> uint64 |> SymbolSetSize.createNr
        let siInt = arOfIntAr |> Array.map(SortableInts.make order symbolSetSize)
        let ssfU16 = rolloutFormat.RfU16 |> sortableSetFormat.SsfArrayRoll
        let sortableSet = SortableSet.fromSortableIntsArrays ssfU16 order symbolSetSize siInt
                           |> Result.ExtractOrThrow
        let arOfIntArBack = sortableSet |> SortableSet.toSortableIntsArrays 
                                        |> Seq.map(SortableInts.getValues)
                                        |> Seq.toArray

        Assert.IsTrue(CollectionProps.areEqual arOfIntAr arOfIntArBack);


    [<TestMethod>]
    member this.fromSortableIntsI32() =
        let order = Order.create 3 |> Result.ExtractOrThrow
        let arOfIntAr = [| [|1111;11112;11113|]; [|2211;12;13|]; [|2221;22;23|]; [|5555531;32;33|]; |]
        let sortableCount = arOfIntAr.Length |> SortableCount.create
        let symbolSetSize = 555553133 |> uint64 |> SymbolSetSize.createNr
        let siInt = arOfIntAr |> Array.map(SortableInts.make order symbolSetSize)
        let ssfI32 = rolloutFormat.RfI32 |> sortableSetFormat.SsfArrayRoll
        let sortableSet = SortableSet.fromSortableIntsArrays ssfI32 order symbolSetSize siInt
                           |> Result.ExtractOrThrow
        let arOfIntArBack = sortableSet |> SortableSet.toSortableIntsArrays 
                                        |> Seq.map(SortableInts.getValues)
                                        |> Seq.toArray

        Assert.IsTrue(CollectionProps.areEqual arOfIntAr arOfIntArBack);


    [<TestMethod>]
    member this.fromSortableIntsU64() =
        let order = Order.create 3 |> Result.ExtractOrThrow
        let arOfIntAr = [| [|1111;11112;11113|]; [|2211;12;13|]; [|2221;22;23|]; [|5555531;32;33|]; |]
        let sortableCount = arOfIntAr.Length |> SortableCount.create
        let symbolSetSize = 555553133 |> uint64 |> SymbolSetSize.createNr
        let siInt = arOfIntAr |> Array.map(SortableInts.make order symbolSetSize)
        let ssfU64 = rolloutFormat.RfU64 |> sortableSetFormat.SsfArrayRoll
        let sortableSet = SortableSet.fromSortableIntsArrays ssfU64 order symbolSetSize siInt
                           |> Result.ExtractOrThrow
        let arOfIntArBack = sortableSet |> SortableSet.toSortableIntsArrays 
                                        |> Seq.map(SortableInts.getValues)
                                        |> Seq.toArray

        Assert.IsTrue(CollectionProps.areEqual arOfIntAr arOfIntArBack);


    [<TestMethod>]
    member this.fromSortableIntsStriped() =
        let order = Order.create 3 |> Result.ExtractOrThrow
        let arOfIntAr = [| [|1;0;1|]; [|1;0;0|]; [|0;0;1|]; [|1;1;1|]; |]
        let symbolSetSize = 2 |> uint64 |> SymbolSetSize.createNr
        let siInt = arOfIntAr |> Array.map(SortableInts.make order symbolSetSize)
        let ssfStriped = sortableSetFormat.SsfBitStriped (false |> ExpandBitSets.create)
        let sortableSet = SortableSet.fromSortableIntsArrays ssfStriped order symbolSetSize siInt
                           |> Result.ExtractOrThrow
        let scFromSs  = sortableSet |> SortableSet.getSortableCount |> SortableCount.value
        let arOfIntArBack = sortableSet |> SortableSet.toSortableIntsArrays 
                                        |> Seq.map(SortableInts.getValues)
                                        |> Seq.take(scFromSs)
                                        |> Seq.toArray

        Assert.IsTrue(CollectionProps.areEqual arOfIntAr arOfIntArBack);


    [<TestMethod>]
    member this.fromAllSortableIntsStriped() =
        let order = Order.create 15 |> Result.ExtractOrThrow
        let symbolSetSize = 2uL |> SymbolSetSize.createNr
        let sortableInts = SortableInts.makeAllBits order
        let arOfIntAr = sortableInts
                         |> Seq.map(SortableInts.getValues)
                         |> Seq.toArray
        let ssfStriped = sortableSetFormat.SsfBitStriped (false |> ExpandBitSets.create)
        let sortableSet = SortableSet.fromSortableIntsArrays 
                                ssfStriped 
                                order 
                                symbolSetSize
                                sortableInts
                           |> Result.ExtractOrThrow
        let scFromSs  = sortableSet |> SortableSet.getSortableCount |> SortableCount.value
        let arOfIntArBack = sortableSet |> SortableSet.toSortableIntsArrays 
                                        |> Seq.map(SortableInts.getValues)
                                        |> Seq.take(arOfIntAr.Length)
                                        |> Seq.toArray

        Assert.IsTrue(CollectionProps.areEqual arOfIntAr arOfIntArBack);


    [<TestMethod>]
    member this.allBitVersions() =
        let order = Order.create 5 |> Result.ExtractOrThrow
        let symbolSetSize = SymbolSetSize.createNr 5uL
        let intArr = [|0;1;3;2;4|]
        let sortableInts = SortableInts.make order symbolSetSize intArr
        let bitExp = sortableInts |> SortableInts.allBitVersions
                                  |> Seq.toArray

        Assert.IsTrue(CollectionProps.areEqual bitExp.Length ((Order.value order) + 1));


    [<TestMethod>]
    member this.allBitVersionsSeq() =
        let order = Order.create 5 |> Result.ExtractOrThrow
        let symbolSetSize = SymbolSetSize.createNr 5uL
        let intArrSeq = [| [|0;1;2;3;4|]; [|4;3;2;1;0|] |]
        let sortableIntsSeq = intArrSeq |> Seq.map(SortableInts.make order symbolSetSize)
        let bitExp = sortableIntsSeq |> SortableInts.expandToBitVersions
                                     |> Seq.toArray

        Assert.IsTrue(CollectionProps.areEqual bitExp.Length (2 * (Order.value order)));