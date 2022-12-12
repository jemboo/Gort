﻿namespace global

open System



type sorterSetPerfReportMode =
    | Speed
    | Perf


type sorterPhenotypeSpeed =
    private
        { sorterSpeedBn: sorterSpeedBin
          sortrs: sorter[]
          sortrPhenotypeId: sorterPhenotypeId }


module SorterPhenotypeSpeed =

    let fromSorterSpeeds (sorterSpeeds: seq<sorterSpeedEval>) =
        let _makeBinFromSamePhenotypes 
                (spId: sorterPhenotypeId) 
                (sorterSpeeds: seq<sorterSpeedEval>) =
            let memA = sorterSpeeds |> Seq.toArray
            { 
              sorterPhenotypeSpeed.sorterSpeedBn = memA.[0].sorterSpeedBn
              sorterPhenotypeSpeed.sortrs = memA |> Array.map (fun sp -> sp.sortr)
              sorterPhenotypeSpeed.sortrPhenotypeId = spId
            }

        sorterSpeeds
        |> Seq.groupBy (fun sp -> sp.sortrPhenotypeId)
        |> Seq.map (fun (spId, mbrs) -> _makeBinFromSamePhenotypes spId mbrs)


    let getSorterSpeedBin (sorterSpeedBn: sorterPhenotypeSpeed) = sorterSpeedBn.sorterSpeedBn

    let getUsedSwitchCount (sorterSpeedBn: sorterPhenotypeSpeed) =
        sorterSpeedBn.sorterSpeedBn |> SorterSpeedBin.getSwitchCount

    let getUsedStageCount (sorterSpeedBn: sorterPhenotypeSpeed) =
        sorterSpeedBn.sorterSpeedBn |> SorterSpeedBin.getStageCount

    let getSorters (sorterSpeedBn: sorterPhenotypeSpeed) = sorterSpeedBn.sortrs

    let getSorterPhenotypeId (sorterSpeedBn: sorterPhenotypeSpeed) = sorterSpeedBn.sortrPhenotypeId



type sorterPhenotypeSpeedsForSpeedBin =
    private
        { sorterSpeedBn: sorterSpeedBin
          sorterPhenotypeSpeedBn: sorterPhenotypeSpeed[] }


module SorterPhenotypeSpeedsForSpeedBin =

    let getSpeedBin (perfR: sorterPhenotypeSpeedsForSpeedBin) = perfR.sorterSpeedBn

    let getUsedSwitchCount (perfR: sorterPhenotypeSpeedsForSpeedBin) =
        perfR.sorterSpeedBn |> SorterSpeedBin.getSwitchCount

    let getUsedStageCount (perfR: sorterPhenotypeSpeedsForSpeedBin) =
        perfR.sorterSpeedBn |> SorterSpeedBin.getStageCount

    let getSorterCount (perfR: sorterPhenotypeSpeedsForSpeedBin) =
        perfR.sorterPhenotypeSpeedBn
        |> Array.sumBy (fun spsb -> spsb.sortrs.Length)
        |> SorterCount.create

    let getPhenotypeCount (perfR: sorterPhenotypeSpeedsForSpeedBin) = perfR.sorterPhenotypeSpeedBn.Length

    // Similar bins have the same switch and stage counts
    let mergeSimilarBins
        (sorterSpeedBinReprtA: sorterPhenotypeSpeedsForSpeedBin)
        (sorterSpeedBinReprtB: sorterPhenotypeSpeedsForSpeedBin)
        =
        { sorterPhenotypeSpeedsForSpeedBin.sorterSpeedBn = sorterSpeedBinReprtA.sorterSpeedBn
          sorterPhenotypeSpeedsForSpeedBin.sorterPhenotypeSpeedBn =
            Array.append sorterSpeedBinReprtA.sorterPhenotypeSpeedBn sorterSpeedBinReprtB.sorterPhenotypeSpeedBn }

    let fromSorterPhenotypeSpeedBin (sorterPhenotypeSpeedBn: sorterPhenotypeSpeed) =
        { sorterPhenotypeSpeedsForSpeedBin.sorterSpeedBn = sorterPhenotypeSpeedBn.sorterSpeedBn
          sorterPhenotypeSpeedsForSpeedBin.sorterPhenotypeSpeedBn = [| sorterPhenotypeSpeedBn |] }


    let fromSorterPhenotpeSpeedBins (sorterSpeedBns: seq<sorterPhenotypeSpeed>) =
        sorterSpeedBns
        |> Seq.map (fromSorterPhenotypeSpeedBin)
        |> Seq.groupBy (fun spb -> spb |> getSpeedBin)
        |> Seq.map (fun gp -> gp |> snd |> Seq.reduce (mergeSimilarBins))


    let fromSorterSpeeds (sorterSpeeds: seq<sorterSpeedEval>) =
        sorterSpeeds
        |> SorterPhenotypeSpeed.fromSorterSpeeds
        |> fromSorterPhenotpeSpeedBins



type sorterPhenotypePerf =
    private
        { sorterSpeedBn: sorterSpeedBin
          sorterPrf: sorterPerf
          sortrs: sorter[]
          sortrPhenotypeId: sorterPhenotypeId }

module SorterPhenotypePerf =

    let fromSorterPerfs (sorterPerfs: seq<sorterPerfEval>) =
        let _makeBinFromSamePhenotypes (spId: sorterPhenotypeId) (sorterPerfs: seq<sorterPerfEval>) =
            let memA = sorterPerfs |> Seq.toArray
            { sorterPhenotypePerf.sorterSpeedBn = memA.[0].sorterSpeedBn
              sorterPhenotypePerf.sorterPrf = memA.[0].sorterPrf
              sorterPhenotypePerf.sortrs = memA |> Array.map (fun sp -> sp.sortr)
              sorterPhenotypePerf.sortrPhenotypeId = spId }

        sorterPerfs
        |> Seq.groupBy (fun sp -> sp.sortrPhenotypeId)
        |> Seq.map (fun (spId, mbrs) -> _makeBinFromSamePhenotypes spId mbrs)


    let getSorterPerf (perfBin: sorterPhenotypePerf) = perfBin.sorterPrf

    let getUsedSwitchCount (perfBin: sorterPhenotypePerf) =
        perfBin.sorterSpeedBn |> SorterSpeedBin.getSwitchCount

    let getUsedStageCount (perfBin: sorterPhenotypePerf) =
        perfBin.sorterSpeedBn |> SorterSpeedBin.getStageCount

    let getSorters (perfBin: sorterPhenotypePerf) = perfBin.sortrs

    let getSorterPhenotypeId (perfBin: sorterPhenotypePerf) = perfBin.sortrPhenotypeId


type sorterPhenotypePerfsForSpeedBin =
    private
        { sorterSpeedBn: sorterSpeedBin
          failingPhenotypeBins: sorterPhenotypePerf[]
          successfulPhenotypeBins: sorterPhenotypePerf[] }

module SorterPhenotypePerfsForSpeedBin =
    let getSpeedBin (perfR: sorterPhenotypePerfsForSpeedBin) = perfR.sorterSpeedBn

    let getUsedSwitchCount (perfR: sorterPhenotypePerfsForSpeedBin) =
        perfR.sorterSpeedBn |> SorterSpeedBin.getSwitchCount

    let getUsedStageCount (perfR: sorterPhenotypePerfsForSpeedBin) =
        perfR.sorterSpeedBn |> SorterSpeedBin.getStageCount

    let getSucessCount (perfR: sorterPhenotypePerfsForSpeedBin) =
        perfR.successfulPhenotypeBins |> Array.sumBy (fun spb -> spb.sortrs.Length)

    let getFailureCount (perfR: sorterPhenotypePerfsForSpeedBin) =
        perfR.failingPhenotypeBins |> Array.sumBy (fun spb -> spb.sortrs.Length)

    let getTotalCount (perfR: sorterPhenotypePerfsForSpeedBin) =
        (perfR |> getSucessCount) + (perfR |> getFailureCount)

    let getSuccessfulPhenotypeCount (perfR: sorterPhenotypePerfsForSpeedBin) = perfR.successfulPhenotypeBins.Length

    let getFailurePhenotypeCount (perfR: sorterPhenotypePerfsForSpeedBin) = perfR.failingPhenotypeBins.Length

    let hasAFailure (perfR: sorterPhenotypePerfsForSpeedBin) = (getFailurePhenotypeCount perfR) > 0

    let isSameBin (sorterPerfBn: sorterPhenotypePerfsForSpeedBin) 
                  (sorterSpeedBn: sorterPhenotypeSpeedsForSpeedBin) =
        ((sorterPerfBn |> getUsedStageCount) = 
                    (sorterSpeedBn |> SorterPhenotypeSpeedsForSpeedBin.getUsedStageCount))
        && 
        ((sorterPerfBn |> getUsedSwitchCount) = 
                (sorterSpeedBn |> SorterPhenotypeSpeedsForSpeedBin.getUsedSwitchCount))


    let hasSameCount (sorterPerfBn: sorterPhenotypePerfsForSpeedBin) 
                     (sorterSpeedBn: sorterPhenotypeSpeedsForSpeedBin) =
        ((sorterPerfBn |> getTotalCount) = (sorterSpeedBn
                                            |> SorterPhenotypeSpeedsForSpeedBin.getSorterCount
                                            |> SorterCount.value))

    let couldBeTheSame
        (sorterPerfBn: sorterPhenotypePerfsForSpeedBin)
        (sorterSpeedBn: sorterPhenotypeSpeedsForSpeedBin) =
        (isSameBin sorterPerfBn sorterSpeedBn)
        && (hasSameCount sorterPerfBn sorterSpeedBn)

    // Similar bins have the same switch and stage counts
    let mergeSimilarBins
        (sorterPerfBinReprtA: sorterPhenotypePerfsForSpeedBin)
        (sorterPerfBinReprtB: sorterPhenotypePerfsForSpeedBin) =
        { sorterPhenotypePerfsForSpeedBin.sorterSpeedBn = sorterPerfBinReprtA.sorterSpeedBn
          sorterPhenotypePerfsForSpeedBin.failingPhenotypeBins =
            sorterPerfBinReprtA.failingPhenotypeBins
            |> Array.append sorterPerfBinReprtB.failingPhenotypeBins
          sorterPhenotypePerfsForSpeedBin.successfulPhenotypeBins =
            sorterPerfBinReprtA.successfulPhenotypeBins
            |> Array.append sorterPerfBinReprtB.successfulPhenotypeBins }


    //let fromSorterPerfBin (sorterPerfBn: sorterPhenotypePerf) =
    //    if (sorterPerfBn.isSuccessful) then
    //        { sorterPhenotypePerfsForSpeedBin.sorterSpeedBn = sorterPerfBn.sorterSpeedBn
    //          sorterPhenotypePerfsForSpeedBin.successfulPhenotypeBins = [| sorterPerfBn |]
    //          sorterPhenotypePerfsForSpeedBin.failingPhenotypeBins = [||] }
    //    else
    //        { sorterPhenotypePerfsForSpeedBin.sorterSpeedBn = sorterPerfBn.sorterSpeedBn
    //          sorterPhenotypePerfsForSpeedBin.successfulPhenotypeBins = [||]
    //          sorterPhenotypePerfsForSpeedBin.failingPhenotypeBins = [| sorterPerfBn |] }


    //let fromSorterPhenotypePerfBins (sorterPerfBns: seq<sorterPhenotypePerf>) =
    //    sorterPerfBns
    //    |> Seq.map (fromSorterPerfBin)
    //    |> Seq.groupBy (fun spb -> spb |> getSpeedBin)
    //    |> Seq.map (fun gp -> gp |> snd |> Seq.reduce (mergeSimilarBins))


    //let fromSorterPerfs (sorterPerfs: seq<sorterPerfEval>) =
    //    sorterPerfs
    //    |> SorterPhenotypePerf.fromSorterPerfs
    //    |> fromSorterPhenotypePerfBins





module SortingReport =

    let reportSpeedBins (sorterEvls: sorterEval[]) =
        result {
            let! speedBins =
                sorterEvls
                |> Array.map (SorterEval.getSorterSpeed)
                |> Array.toList
                |> Result.sequence

            return
                speedBins
                |> SorterPhenotypeSpeed.fromSorterSpeeds
                |> SorterPhenotypeSpeedsForSpeedBin.fromSorterPhenotpeSpeedBins
        }


    //let reportPerfBins (sorterEvls: sorterEval[]) =
    //    result {
    //        let! speedBins =
    //            sorterEvls
    //            |> Array.map (SorterEval.getSorterPerf)
    //            |> Array.toList
    //            |> Result.sequence

    //        return
    //            speedBins
    //            |> SorterPhenotypePerf.fromSorterPerfs
    //            |> SorterPhenotypePerfsForSpeedBin.fromSorterPhenotypePerfBins
    //    }









//let getMinMaxMeanOfSuccessful (perfM:sorterPerfBin -> double)
//                                (bins:sorterPerfBin seq)  =
//    use enumer = bins.GetEnumerator()
//    let mutable min = Double.MaxValue
//    let mutable max = Double.MinValue
//    let mutable total = 0.0
//    let mutable count = 0.0
//    while enumer.MoveNext() do
//        if enumer.Current.successCount > 0 then
//            let fct = (float enumer.Current.successCount)
//            let curM = perfM enumer.Current
//            if curM < min then
//                min <- curM
//            if curM > max then
//                max <- curM
//            count <- count + fct
//            total <- total + (curM * fct)
//    let mean = if count = 0.0 then 0.0 else total / count
//    (min, max, mean)


//let getMinMaxMeanOfFails (perfM:sorterPerfBin -> double)
//                            (bins:sorterPerfBin seq)  =
//    use enumer = bins.GetEnumerator()
//    let mutable min = Double.MaxValue
//    let mutable max = Double.MinValue
//    let mutable total = 0.0
//    let mutable count = 0.0
//    while enumer.MoveNext() do
//        if enumer.Current.failCount > 0 then
//            let fct = (float enumer.Current.failCount)
//            let curM = perfM enumer.Current
//            if curM < min then
//                min <- curM
//            if curM > max then
//                max <- curM
//            count <- count + fct
//            total <- total + (curM * fct)
//    let mean = if count = 0.0 then 0.0 else total / count
//    (min, max, mean)


//let getStdevOfSuccessful (perfM:sorterPerfBin -> double)
//                            (centroid:float)
//                            (bins:sorterPerfBin seq) =
//    use enumer = bins.GetEnumerator()
//    let mutable totalCt = 0.0
//    let mutable totalRds = 0.0
//    while enumer.MoveNext() do
//        if enumer.Current.successCount > 0 then
//            let binCt = (float enumer.Current.successCount)
//            let curM = perfM enumer.Current
//            totalCt <- totalCt + binCt
//            totalRds <- totalRds + (Math.Sqrt ((curM - centroid) * (curM - centroid))) * binCt

//    if (totalCt = 0.0) then 0.0 else (totalRds / totalCt)



//let getRdsBetterWorseOfSuccessful (perfM:sorterPerfBin -> double)
//                                    (centroid:float)
//                                    (bins:sorterPerfBin seq) =
//    use enumer = bins.GetEnumerator()
//    let mutable totalCtBetter = 0.0
//    let mutable totalRdsBetter = 0.0
//    let mutable totalCtWorse = 0.0
//    let mutable totalRdsWorse = 0.0
//    while enumer.MoveNext() do
//        if enumer.Current.successCount > 0 then
//            let fct = (float enumer.Current.successCount)
//            let curM = perfM enumer.Current
//            if curM < centroid then
//                totalCtBetter <- totalCtBetter + fct
//                totalRdsBetter <- totalRdsBetter + (curM - centroid) * (curM - centroid) * fct
//            else
//                totalCtWorse <- totalCtWorse + fct
//                totalRdsWorse <- totalRdsWorse + (curM - centroid) * (curM - centroid) * fct
//    let bR = if (totalCtBetter = 0.0) then 0.0 else totalRdsBetter / totalCtBetter
//    let wR = if (totalCtWorse = 0.0) then 0.0 else totalRdsWorse / totalCtWorse
//    (bR, wR)


//let getRdsBetterWorseOfFails (perfM:sorterPerfBin -> double)
//                                (centroid:float)
//                                (bins:sorterPerfBin seq)  =
//    use enumer = bins.GetEnumerator()
//    let mutable totalCtBetter = 0.0
//    let mutable totalRdsBetter = 0.0
//    let mutable totalCtWorse = 0.0
//    let mutable totalRdsWorse = 0.0
//    while enumer.MoveNext() do
//        if enumer.Current.failCount > 0 then
//            let fct = (float enumer.Current.failCount)
//            let curM = perfM enumer.Current
//            if curM < centroid then
//                totalCtBetter <- totalCtBetter + fct
//                totalRdsBetter <- totalRdsBetter + (curM - centroid) * (curM - centroid) * fct
//            else
//                totalCtWorse <- totalCtWorse + fct
//                totalRdsWorse <- totalRdsWorse + (curM - centroid) * (curM - centroid) * fct
//    let bR = if (totalCtBetter = 0.0) then 0.0 else totalRdsBetter / totalCtBetter
//    let wR = if (totalCtWorse = 0.0) then 0.0 else totalRdsWorse / totalCtWorse
//    (bR, wR)



type stageWeight = private StageWeight of float

module StageWeight =
    let value (StageWeight v) = v
    let create id = Ok(StageWeight id)
    let fromFloat (id: float) = create id |> Result.ExtractOrThrow


type sorterSaving =
    | NotAny
    | All
    | Successful
    | Perf of stageWeight * sorterCount


module SorterFitness =

    let switchBased (order: order) (switchCount: switchCount) =
        let bestSwitch =
            SwitchCount.orderToRecordSwitchCount order |> SwitchCount.value |> float

        let scv = switchCount |> SwitchCount.value |> float
        (scv) / (bestSwitch) |> Energy.create


    let stageBased (order: order) (stageCount: stageCount) =
        let bestStage =
            StageCount.orderToRecordStageCount order |> StageCount.value |> float

        let scv = stageCount |> StageCount.value |> float
        (scv) / (bestStage) |> Energy.create

    let weighted (order: order) (stageWeight: stageWeight) (wCt: switchCount) (tCt: stageCount) =
        let wV = switchBased order wCt |> Energy.value
        let tV = stageBased order tCt |> Energy.value
        let tw = StageWeight.value stageWeight
        ((wV + tV * tw) / (tw + 1.0)) |> Energy.create


//let fromSorterPerf (order:order)
//                   (stageWeight:stageWeight)
//                   (perf:sorterPerf) =
//    let pv =
//        weighted order stageWeight
//                 perf.usedSwitchCount perf.usedStageCount

//    match perf.failCount with
//    | Some v -> if (SortableCount.value v) = 0
//                    then pv else Energy.failure
//    | None -> pv
