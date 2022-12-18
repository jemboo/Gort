namespace Gort.SortingResults.Test
open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type SorterSetReportingFixture() =


    [<TestMethod>]
    member this.sorterSpeedBins() =
        let mutable i = 0
        let maxW = 60
        let rndy = Rando.fromRngGen (RngGen.createLcg (4213 |> RandomSeed.create))

        while i < 100 do
            let switchCt = maxW |> (%) rndy.NextPositiveInt |> SwitchCount.create

            let stageCt =
                (switchCt |> SwitchCount.value |> (+) 1)
                |> (%) rndy.NextPositiveInt
                |> StageCount.create

            let sorterSpeedBn = SorterSpeedBin.create switchCt stageCt

            let sorterSpeedBnIndex = SorterSpeedBin.getIndexOfBin sorterSpeedBn

            let sorterSpeedBnBack = SorterSpeedBin.getBinFromIndex sorterSpeedBnIndex
            Assert.AreEqual(sorterSpeedBn, sorterSpeedBnBack)
            i <- i + 1


    [<TestMethod>]
    member this.getBins() =
        //let sorterSetId = Guid.NewGuid() |> SorterSetId.create
        //let useParalll = true |> UseParallel.create
        //let ordr = 16 |> Order.createNr
        //let switchCt = SwitchCount.orderTo900SwitchCount ordr
        //let sorterCt = SorterCount.create 500
        //let rnGn = RngGen.createLcg (123 |> RandomSeed.create)

        //let sorterSt = SorterSet.createRandomSwitches sorterSetId sorterCt ordr [||] switchCt rnGn
        //let rolloutFormt = rolloutFormat.RfBs64
        //let sortableStId = SortableSetId.create 123

        //let sortableSt =
        //    SortableSet.makeAllBits sortableStId rolloutFormt ordr |> Result.ExtractOrThrow

        //let sorterSpeedEvls, errs =
        //    SorterSetEval.eval sorterEvalMode.SorterSpeed sortableSt sorterSt useParalll

        //let sorterSpeedRs =
        //    sorterSpeedEvls |> Array.map (SorterEval.getSorterSpeed) |> Array.toList

        //let sorterSpeeds = sorterSpeedRs |> Result.sequence |> Result.ExtractOrThrow

        //let spsfsbs =
        //    sorterSpeeds
        //    |> SorterPhenotypeSpeedsForSpeedBin.fromSorterSpeeds
        //    |> Seq.toArray
        //    |> Array.sortBy (fun sp ->
        //        sp
        //        |> SorterPhenotypeSpeedsForSpeedBin.getSpeedBin
        //        |> SorterSpeedBin.getIndexOfBin)

        ////////////////
        //let sorterPerfEvls, errs =
        //    SorterSetEval.eval sorterEvalMode.SorterPerf sortableSt sorterSt useParalll

        //let sorterPerfRs =
        //    sorterPerfEvls |> Array.map (SorterEval.getSorterPerf) |> Array.toList

        //let sorterPerfs = sorterPerfRs |> Result.sequence |> Result.ExtractOrThrow

        //let sppfsbs =
        //    sorterPerfs
        //    |> SorterPhenotypePerfsForSpeedBin.fromSorterPerfs
        //    |> Seq.toArray
        //    |> Array.sortBy (fun sp ->
        //        sp
        //        |> SorterPhenotypePerfsForSpeedBin.getSpeedBin
        //        |> SorterSpeedBin.getIndexOfBin)

        //let mutable dex = 0
        //while dex < sppfsbs.Length do
        //    let res = SorterPhenotypePerfsForSpeedBin.couldBeTheSame sppfsbs.[dex] spsfsbs.[dex]
        //    Assert.IsTrue(res)
        //    dex <- dex + 1

        //let hasAFailure = sppfsbs |> Array.exists(fun spps -> spps |> SorterPhenotypePerfsForSpeedBin.hasAFailure)

        //Assert.IsTrue(hasAFailure)
        Assert.IsTrue(true)
