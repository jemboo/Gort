namespace Gort.SortingOps.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type SortingWithHistoryFixture () =

    [<TestMethod>]
    member this.IntsHist() =
        let order = Order.create 8 |> Result.ExtractOrThrow
        let randy = Rando.create rngType.Lcg (123 |> RandomSeed.create)
        let sortableInts = SortableIntArray.makeRandomPermutation order randy
        let goodSorter = RefSorter.goodRefSorterForOrder order |> Result.ExtractOrThrow
        let hist = SortingWithHistory.Ints.makeWithFullSorter goodSorter sortableInts
        Assert.AreEqual(hist.Length, 1 + SwitchCount.value goodSorter.switchCount)
        let result = hist.Item (hist.Length - 1)
        Assert.IsTrue(result |> SortableIntArray.isSorted)


    [<TestMethod>]
    member this.BoolsHist() =
        let order = Order.create 8 |> Result.ExtractOrThrow
        let randy = Rando.create rngType.Lcg (123 |> RandomSeed.create)
        let sortableInts = SortableBoolArray.makeRandomBits order 0.5 randy
                            |> Seq.head
        let goodSorter = RefSorter.goodRefSorterForOrder order |> Result.ExtractOrThrow
        let hist = SortingWithHistory.Bools.makeWithFullSorter goodSorter sortableInts
        Assert.AreEqual(hist.Length, 1 + SwitchCount.value goodSorter.switchCount)
        let result = hist.Item (hist.Length - 1)
        Assert.IsTrue(result |> SortableBoolArray.isSorted)


    [<TestMethod>]
    member this.TestMethodPassing () =
        Assert.IsTrue(true);
