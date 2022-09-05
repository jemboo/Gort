namespace Gort.Sorting.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type SortableArraysFixture () =

    [<TestMethod>]
    member this.fromSortableIntsU8() =
        let order = Order.create 3 |> Result.ExtractOrThrow
        let arOfIntAr = [| [|111;112;113|]; [|221;12;13|]; [|222;22;23|]; [|131;32;33|]; |]
        let symbolSetSize = 255 |> uint64 |> SymbolSetSize.createNr
        let siInt = arOfIntAr |> Array.map(SortableIntArray.make order symbolSetSize)
        let rolloutFormt = rolloutFormat.RfU8
        let sortableSetId = 123 |> SortableSetId.create
        let sortableSet = 
            SortableSet.fromSortableIntArrays 
               sortableSetId
               rolloutFormt
               order
               symbolSetSize
               siInt
            |> Result.ExtractOrThrow

        let arOfIntArBack = 
            sortableSet 
            |> SortableSet.toSortableIntsArrays 
            |> Seq.map(SortableIntArray.getValues)
            |> Seq.toArray

        Assert.IsTrue(CollectionProps.areEqual arOfIntAr arOfIntArBack);

    
    [<TestMethod>]
    member this.fromSortableIntsU16() =
        let order = Order.create 3 |> Result.ExtractOrThrow
        let arOfIntAr = [| [|1111;11112;11113|]; [|2211;12;13|]; [|2221;22;23|]; [|5531;32;33|]; |]
        let symbolSetSize = 5553 |> uint64 |> SymbolSetSize.createNr
        let siInt = arOfIntAr |> Array.map(SortableIntArray.make order symbolSetSize)
        let rolloutFormt = rolloutFormat.RfI32
        let sortableSetId = 123 |> SortableSetId.create
        let sortableSet = 
            SortableSet.fromSortableIntArrays 
                sortableSetId 
                rolloutFormt 
                order 
                symbolSetSize 
                siInt
            |> Result.ExtractOrThrow

        let arOfIntArBack = 
            sortableSet 
            |> SortableSet.toSortableIntsArrays 
            |> Seq.map(SortableIntArray.getValues)
            |> Seq.toArray

        Assert.IsTrue(CollectionProps.areEqual arOfIntAr arOfIntArBack);


    [<TestMethod>]
    member this.fromSortableIntsI32() =
        let order = Order.create 3 |> Result.ExtractOrThrow
        let arOfIntAr = [| [|1111;11112;11113|]; [|2211;12;13|]; [|2221;22;23|]; [|5555531;32;33|]; |]
        let symbolSetSize = 555553133 |> uint64 |> SymbolSetSize.createNr
        let siInt = arOfIntAr |> Array.map(SortableIntArray.make order symbolSetSize)
        let rolloutFormt = rolloutFormat.RfI32
        let sortableSetId = 123 |> SortableSetId.create

        let sortableSet = 
            SortableSet.fromSortableIntArrays 
                    sortableSetId
                    rolloutFormt 
                    order 
                    symbolSetSize 
                    siInt
            |> Result.ExtractOrThrow

        let arOfIntArBack = 
            sortableSet 
            |> SortableSet.toSortableIntsArrays 
            |> Seq.map(SortableIntArray.getValues)
            |> Seq.toArray

        Assert.IsTrue(CollectionProps.areEqual arOfIntAr arOfIntArBack);


    [<TestMethod>]
    member this.fromSortableIntsU64() =
        let order = Order.create 3 |> Result.ExtractOrThrow
        let arOfIntAr = [| [|1111;11112;11113|]; [|2211;12;13|]; [|2221;22;23|]; [|5555531;32;33|]; |]
        let symbolSetSize = 555553133 |> uint64 |> SymbolSetSize.createNr
        let siInt = arOfIntAr |> Array.map(SortableIntArray.make order symbolSetSize)
        let rolloutFormt = rolloutFormat.RfU64
        let sortableSetId = 123 |> SortableSetId.create

        let sortableSet = 
            SortableSet.fromSortableIntArrays 
                sortableSetId
                rolloutFormt order symbolSetSize siInt
            |> Result.ExtractOrThrow

        let arOfIntArBack = 
            sortableSet 
            |> SortableSet.toSortableIntsArrays 
            |> Seq.map(SortableIntArray.getValues)
            |> Seq.toArray

        Assert.IsTrue(CollectionProps.areEqual arOfIntAr arOfIntArBack)


    [<TestMethod>]
    member this.fromSortableIntsStriped() =
        let order = Order.create 3 |> Result.ExtractOrThrow
        let arOfIntAr = [| [|1;0;1|]; [|1;0;0|]; [|0;0;1|]; [|1;1;1|]; |]
        let symbolSetSize = 2 |> uint64 |> SymbolSetSize.createNr
        let siInt = arOfIntAr |> Array.map(SortableIntArray.make order symbolSetSize)
        let rolloutFormt = rolloutFormat.RfBs64
        let sortableSetId = 123 |> SortableSetId.create

        let sortableSet = 
            SortableSet.fromSortableIntArrays 
                sortableSetId
                rolloutFormt 
                order 
                symbolSetSize 
                siInt
             |> Result.ExtractOrThrow

        let scFromSs = 
            sortableSet 
            |> SortableSet.getSortableCount 
            |> SortableCount.value

        let arOfIntArBack = 
            sortableSet 
            |> SortableSet.toSortableIntsArrays 
            |> Seq.map(SortableIntArray.getValues)
            |> Seq.take(scFromSs)
            |> Seq.toArray

        let sOrig = Set.ofArray arOfIntAr
        let sBack = Set.ofArray arOfIntArBack

        Assert.IsTrue(CollectionProps.areEqual sOrig sBack)


    [<TestMethod>]
    member this.fromAllSortableBoolsBitStriped() =
        let order = Order.create 8 |> Result.ExtractOrThrow
        let sortableCount = 100 |> SortableCount.create
        let randy = Rando.create rngType.Lcg (123 |> RandomSeed.create)
        let sortableBools = 
            SortableBoolArray.makeRandomBits order 0.5 randy
                |> Seq.take(sortableCount |> SortableCount.value)
                |> Seq.toArray

        let sortableBoolsArray = 
            sortableBools
            |> Seq.map(SortableBoolArray.getValues)
            |> Seq.toArray

        let rolloutFormt = rolloutFormat.RfBs64
        let sortableSetId = 123 |> SortableSetId.create
        let sortableSet = 
            SortableSet.fromSortableBoolArrays 
                sortableSetId
                rolloutFormt 
                order
                sortableBools
            |> Result.ExtractOrThrow

        let sortableBoolsArrayBack = 
            sortableSet 
                |> SortableSet.toSortableBoolSets
                |> Seq.map(SortableBoolArray.getValues)
                |> Seq.take(sortableBoolsArray.Length)
                |> Seq.toArray

        let sOrig = Set.ofArray sortableBoolsArray
        let sBack = Set.ofArray sortableBoolsArrayBack

        Assert.IsTrue(CollectionProps.areEqual sOrig sBack);


    [<TestMethod>]
    member this.allBooleanVersions() =
        let order = Order.create 7 |> Result.ExtractOrThrow
        let symbolSetSize = SymbolSetSize.createNr 5uL
        let intArr = [|0;1;1;3;2;2;4|]
        let uniqueElementCt = 
            intArr |> Set.ofArray |> Set.count
                
        let sortableInts = SortableIntArray.make order symbolSetSize intArr
        let bitExp =
            sortableInts 
            |> SortableBoolArray.allBooleanVersions
            |> Seq.toArray

        Assert.AreEqual(bitExp.Length, uniqueElementCt - 1);


    [<TestMethod>]
    member this.allBooleanVersionsSeq() =

        let order = Order.create 5 |> Result.ExtractOrThrow
        let symbolSetSize = SymbolSetSize.createNr 5uL
        let intArrSeq = [| [|0;1;2;3;4|]; [|4;3;2;1;0|] |]

        let sortableIntsSeq = 
            intArrSeq 
            |> Seq.map(SortableIntArray.make order symbolSetSize)

        let bitExp = 
            sortableIntsSeq 
            |> SortableBoolArray.expandToSortableBits
            |> Seq.toArray

        Assert.AreEqual(bitExp.Length, 2 * ((Order.value order) - 1));

        let intArrSeq2 = [| [|0;1;2;3;4|]; [|4;3;2;1;0|]; [|4;3;2;1;0|] |]
        let sortableIntsSeq2 = 
            intArrSeq2 
            |> Seq.map(SortableIntArray.make order symbolSetSize)

        let bitExp2 = 
            sortableIntsSeq2 
            |> SortableBoolArray.expandToSortableBits
            |> Seq.toArray

        Assert.AreEqual(bitExp.Length, bitExp2.Length);