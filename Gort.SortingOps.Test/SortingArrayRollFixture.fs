namespace Gort.SortingOps.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type SortingArrayRollFixture () =

    [<TestMethod>]
    member this.ddd () =
        let order = Order.create 8 |> Result.ExtractOrThrow
        let randy = Rando.create rngType.Lcg (123 |> RandomSeed.create)
        let sortableSetId = 123 |> SortableSetId.create
        let sortableSetFormat = rolloutFormat.RfI32 |> SortableSetFormat.makeRollout
        let sortableCount = 10 |> SortableCount.create
        let goodSorter = RefSorter.goodRefSorterForOrder order |> Result.ExtractOrThrow

        let sortableSet = SortableSet.makeRandomPermutation 
                                sortableSetId
                                sortableSetFormat
                                order
                                sortableCount
                                randy
                           |> Result.ExtractOrThrow

        let rollout = sortableSet 
                            |> SortableSet.getRollout 
                            |> Result.ExtractOrThrow

        let symbolSetSize = sortableSet 
                            |> SortableSet.getSymbolSetSize

        let res = SortingArrayRoll.sorterWithNoSAG
                            goodSorter
                            sortableSetId
                            symbolSetSize
                            rollout



        Assert.IsTrue(true);


    [<TestMethod>]
    member this.TestMethodPassing () =
        Assert.IsTrue(true);
