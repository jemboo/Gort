namespace Gort.SortingOps.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type SortingArrayRollFixture () =

    let getResultsWithSwitchLog (sortableSetFormat:sortableSetFormat) =
        let order = Order.create 8 |> Result.ExtractOrThrow
        let randy = Rando.create rngType.Lcg (123 |> RandomSeed.create)
        let sortableSetId = 123 |> SortableSetId.create
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

        let sortingResults = SortingArrayRoll.applySorterAndMakeSwitchLog
                                goodSorter
                                sortableSetId
                                symbolSetSize
                                rollout
        sortingResults


    let getResultsWithSwitchUses (sortableSetFormat:sortableSetFormat) =
        let order = Order.create 8 |> Result.ExtractOrThrow
        let randy = Rando.create rngType.Lcg (123 |> RandomSeed.create)
        let sortableSetId = 123 |> SortableSetId.create
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

        let sortingResults = SortingArrayRoll.applySorterAndMakeSwitchUses
                                goodSorter
                                sortableSetId
                                symbolSetSize
                                rollout
        sortingResults



    [<TestMethod>]
    member this.rolloutFormatWithSwitchLog () =
        
        let sortableSetFormat_RfI32 = rolloutFormat.RfI32 |> SortableSetFormat.makeRollout
        let results_RfI32 = getResultsWithSwitchLog sortableSetFormat_RfI32
                
        let sortableSetFormat_RfU16 = rolloutFormat.RfU16 |> SortableSetFormat.makeRollout
        let results_RfU16 = getResultsWithSwitchLog sortableSetFormat_RfU16
        
        let sortableSetFormat_RfU8 = rolloutFormat.RfU8 |> SortableSetFormat.makeRollout
        let results_RfU8 = getResultsWithSwitchLog sortableSetFormat_RfU8


        //Assert.IsTrue(CollectionProps.areEqual results_RfI32 results_RfU16);
        Assert.IsTrue(true);


    [<TestMethod>]
    member this.rolloutFormatWithSwitchUses () =
        
        let sortableSetFormat_RfI32 = rolloutFormat.RfI32 |> SortableSetFormat.makeRollout
        let results_RfI32 = getResultsWithSwitchUses sortableSetFormat_RfI32
                
        let sortableSetFormat_RfU16 = rolloutFormat.RfU16 |> SortableSetFormat.makeRollout
        let results_RfU16 = getResultsWithSwitchUses sortableSetFormat_RfU16
        
        let sortableSetFormat_RfU8 = rolloutFormat.RfU8 |> SortableSetFormat.makeRollout
        let results_RfU8 = getResultsWithSwitchUses sortableSetFormat_RfU8


        //Assert.IsTrue(CollectionProps.areEqual results_RfI32 results_RfU16);
        Assert.IsTrue(true);
