namespace global

open System

type sorterSpeedBin = private {switchCt:switchCount; stageCt:stageCount}
module SorterSpeedBin =
    let create (switchCt: switchCount)
               (stageCt: stageCount) =
        {switchCt=switchCt; stageCt=stageCt}

    let getSwitchCount (sorterSpeedBn:sorterSpeedBin) =
        sorterSpeedBn.switchCt

    let getStageCount (sorterSpeedBn:sorterSpeedBin) =
        sorterSpeedBn.stageCt

    let getIndexOfBin (sorterSpeedBn:sorterSpeedBin) =
        let switchCtV = sorterSpeedBn.switchCt |> SwitchCount.value
        let stageCtV = sorterSpeedBn.stageCt |> StageCount.value
        ((switchCtV * (switchCtV + 1)) / 2)
        + stageCtV


    let getBinFromIndex (index:int) =
        let indexFlt = (index |> float) + 1.0
        let p = (sqrt(1.0 + 8.0 * indexFlt) - 1.0) / 2.0
        let pfloor = Math.Floor(p)
        if (p = pfloor) then
            let stageCt = 1 |> (-) (int pfloor) |> StageCount.create
            let switchCt = 1 |> (-) (int pfloor) |> SwitchCount.create
            {sorterSpeedBin.switchCt = switchCt; stageCt=stageCt}
        else
            let stageCt = 
                (float index) - (pfloor * (pfloor + 1.0)) / 2.0
                |> int 
                |> StageCount.create
            let switchCt = (int pfloor) |> SwitchCount.create
            {sorterSpeedBin.switchCt = switchCt; stageCt=stageCt}



type sorterSetPerfReportMode = 
    | Speed 
    | Perf


type sorterPhenotypeSpeed = private { 
        usedSwitchCount:switchCount;
        usedStageCount:stageCount;
        sortrs:sorter[];
        sortrPhenotypeId:sorterPhenotypeId;
    }


module SorterPhenotypeSpeed =
    
    let fromSorterSpeeds (sorterSpeeds:seq<sorterSpeed>) =
        let _makeBinFromSamePhenotypes
                (spId:sorterPhenotypeId) 
                (sorterSpeeds:seq<sorterSpeed>) =
            let memA = sorterSpeeds |> Seq.toArray
            {
                sorterPhenotypeSpeed.usedSwitchCount = memA.[0].usedSwitchCount
                sorterPhenotypeSpeed.usedStageCount = memA.[0].usedStageCount
                sorterPhenotypeSpeed.sortrs =
                                memA 
                                |> Array.map(fun sp -> sp.sortr)
                sorterPhenotypeSpeed.sortrPhenotypeId = spId
            }

        sorterSpeeds 
        |> Seq.groupBy(fun sp -> sp.sortrPhenotypeId)
        |> Seq.map(fun (spId, mbrs) -> _makeBinFromSamePhenotypes spId mbrs)


    let getUsedSwitchCount (sorterSpeedBn:sorterPhenotypeSpeed) =
        sorterSpeedBn.usedSwitchCount


    let getUsedStageCount (sorterSpeedBn:sorterPhenotypeSpeed) =
        sorterSpeedBn.usedStageCount


    let getSorters (sorterSpeedBn:sorterPhenotypeSpeed) =
        sorterSpeedBn.sortrs


    let getSorterPhenotypeId (sorterSpeedBn:sorterPhenotypeSpeed) =
        sorterSpeedBn.sortrPhenotypeId


type sorterSpeed2 = private { 
        usedSwitchCount:switchCount; 
        usedStageCount:stageCount;
        sorterPhenotypeSpeedBn:sorterPhenotypeSpeed[]
    }


module SorterSpeed2 =

    let getUsedSwitchCount (perfR:sorterSpeed2) =
        perfR.usedSwitchCount

    let getUsedStageCount (perfR:sorterSpeed2) =
        perfR.usedStageCount

    let getSorterCount (perfR:sorterSpeed2) =
        perfR.sorterPhenotypeSpeedBn
        |> Array.sumBy(fun spsb -> spsb.sortrs.Length)
        |> SorterCount.create

    let getPhenotypeCount (perfR:sorterSpeed2) =
        perfR.sorterPhenotypeSpeedBn.Length

    // Similar bins have the same switch and stage counts
    let mergeSimilarBins 
        (sorterSpeedBinReprtA:sorterSpeed2) 
        (sorterSpeedBinReprtB:sorterSpeed2) =
        { 
            sorterSpeed2.usedSwitchCount = sorterSpeedBinReprtA.usedSwitchCount; 
            sorterSpeed2.usedStageCount = sorterSpeedBinReprtA.usedStageCount;
            sorterSpeed2.sorterPhenotypeSpeedBn = 
                Array.append
                    sorterSpeedBinReprtA.sorterPhenotypeSpeedBn  
                    sorterSpeedBinReprtB.sorterPhenotypeSpeedBn;
        }

    let fromSorterPhenotypeSpeedBin (sorterPhenotypeSpeedBn:sorterPhenotypeSpeed) =
        { 
            sorterSpeed2.usedSwitchCount = sorterPhenotypeSpeedBn.usedSwitchCount; 
            sorterSpeed2.usedStageCount = sorterPhenotypeSpeedBn.usedStageCount;
            sorterSpeed2.sorterPhenotypeSpeedBn = [| sorterPhenotypeSpeedBn |]
        }

    let fromSorterPhenotpeSpeedBins (sorterSpeedBns:seq<sorterPhenotypeSpeed>) =
        sorterSpeedBns
        |> Seq.map(fromSorterPhenotypeSpeedBin)
        |> Seq.groupBy(fun spb -> (spb, spb))
        |> Seq.map(fun gp -> gp |> snd |> Seq.reduce(mergeSimilarBins))



type sorterPhenotypePerf = private { 
        usedSwitchCount:switchCount; 
        usedStageCount:stageCount;
        isSuccessful:bool
        sortrs:sorter[];
        sortrPhenotypeId:sorterPhenotypeId;
    }


module SorterPhenotypePerf =
    
    let fromSorterPerfs (sorterPerfs:seq<sorterPerf>) =
        let _makeBinFromSamePhenotypes
                (spId:sorterPhenotypeId) 
                (sorterPerfs:seq<sorterPerf>) =
            let memA = sorterPerfs |> Seq.toArray
            {
                sorterPhenotypePerf.usedSwitchCount = memA.[0].usedSwitchCount
                sorterPhenotypePerf.usedStageCount = memA.[0].usedStageCount
                sorterPhenotypePerf.isSuccessful = memA.[0].isSuccessful
                sorterPhenotypePerf.sortrs =
                                memA 
                                |> Array.map(fun sp -> sp.sortr)
                sorterPhenotypePerf.sortrPhenotypeId = spId
            }

        sorterPerfs 
        |> Seq.groupBy(fun sp -> sp.sortrPhenotypeId)
        |> Seq.map(fun (spId, mbrs) -> _makeBinFromSamePhenotypes spId mbrs)


    let getIsSucessful (perfBin:sorterPhenotypePerf) =
        perfBin.isSuccessful


    let getUsedSwitchCount (perfBin:sorterPhenotypePerf) =
        perfBin.usedSwitchCount


    let getUsedStageCount (perfBin:sorterPhenotypePerf) =
        perfBin.usedStageCount


    let getSorters (perfBin:sorterPhenotypePerf) =
        perfBin.sortrs


    let getSorterPhenotypeId (perfBin:sorterPhenotypePerf) =
        perfBin.sortrPhenotypeId


type sorterPerf2 = private { 
        usedSwitchCount:switchCount; 
        usedStageCount:stageCount;
        failingPhenotypeBins:sorterPhenotypePerf[]
        successfulPhenotypeBins:sorterPhenotypePerf[];
    }


module SorterPerf2 =

    let getUsedSwitchCount (perfR:sorterPerf2) =
        perfR.usedSwitchCount

    let getUsedStageCount (perfR:sorterPerf2) =
        perfR.usedStageCount

    let getSucessCount (perfR:sorterPerf2) =
        perfR.successfulPhenotypeBins
            |> Array.sumBy(fun spb -> spb.sortrs.Length)

    let getFailureCount (perfR:sorterPerf2) =
        perfR.failingPhenotypeBins
            |> Array.sumBy(fun spb -> spb.sortrs.Length)

    let getTotalCount (perfR:sorterPerf2) =
        (perfR |> getSucessCount) +
        (perfR |> getFailureCount)

    let getSuccessfulPhenotypeCount (perfR:sorterPerf2) =
        perfR.successfulPhenotypeBins.Length

    let getFailurePhenotypeCount (perfR:sorterPerf2) =
        perfR.failingPhenotypeBins.Length

    let isSameBin (sorterPerfBn:sorterPerf2)
                  (sorterSpeedBn:sorterSpeed2) =
           ((sorterPerfBn |> getUsedStageCount) =
           (sorterSpeedBn |> SorterSpeed2.getUsedStageCount))
           &&
           ((sorterPerfBn |> getUsedSwitchCount) =
           (sorterSpeedBn |> SorterSpeed2.getUsedSwitchCount))


    let hasSameCount (sorterPerfBn:sorterPerf2)
                     (sorterSpeedBn:sorterSpeed2) =
           ((sorterPerfBn |> getTotalCount) =
           (sorterSpeedBn |> SorterSpeed2.getSorterCount
                          |> SorterCount.value))

    let couldBeTheSame 
                  (sorterPerfBn:sorterPerf2)
                  (sorterSpeedBn:sorterSpeed2) =
           (isSameBin sorterPerfBn sorterSpeedBn)
           &&
           (hasSameCount sorterPerfBn sorterSpeedBn)

    // Similar bins have the same switch and stage counts
    let mergeSimilarBins 
        (sorterPerfBinReprtA:sorterPerf2) 
        (sorterPerfBinReprtB:sorterPerf2) =
        { 
            sorterPerf2.usedSwitchCount = sorterPerfBinReprtA.usedSwitchCount; 
            sorterPerf2.usedStageCount = sorterPerfBinReprtA.usedStageCount;
            sorterPerf2.failingPhenotypeBins = 
                sorterPerfBinReprtA.failingPhenotypeBins 
                    |> Array.append sorterPerfBinReprtB.failingPhenotypeBins;
            sorterPerf2.successfulPhenotypeBins =
                sorterPerfBinReprtA.successfulPhenotypeBins 
                    |> Array.append sorterPerfBinReprtB.successfulPhenotypeBins;
        }


    let fromSorterPerfBin (sorterPerfBn:sorterPhenotypePerf) =
        if (sorterPerfBn.isSuccessful) then
            { 
                sorterPerf2.usedSwitchCount = sorterPerfBn.usedSwitchCount; 
                sorterPerf2.usedStageCount = sorterPerfBn.usedStageCount;
                sorterPerf2.successfulPhenotypeBins = [| sorterPerfBn |];
                sorterPerf2.failingPhenotypeBins = [||];
            }
            else
            { 
                sorterPerf2.usedSwitchCount = sorterPerfBn.usedSwitchCount; 
                sorterPerf2.usedStageCount = sorterPerfBn.usedStageCount;
                sorterPerf2.successfulPhenotypeBins = [||];
                sorterPerf2.failingPhenotypeBins = [| sorterPerfBn |];
            }


    let fromSorterPhenotypePerfBins (sorterPerfBns:seq<sorterPhenotypePerf>) =
        sorterPerfBns
        |> Seq.map(fromSorterPerfBin)
        |> Seq.groupBy(fun spb -> (spb, spb))
        |> Seq.map(fun gp -> gp |> snd |> Seq.reduce(mergeSimilarBins))



module SortingReport = 

    let reportSpeedBins (sorterEvls:sorterEval[]) =
        result {
            let! speedBins = 
                sorterEvls
                |> Array.map(SorterEval.getSorterSpeed)
                |> Array.toList
                |> Result.sequence
            return speedBins 
                    |> SorterPhenotypeSpeed.fromSorterSpeeds
                    |> SorterSpeed2.fromSorterPhenotpeSpeedBins
        }


    let reportPerfBins (sorterEvls:sorterEval[]) =
        result {
            let! speedBins = 
                sorterEvls
                |> Array.map(SorterEval.getSorterPerf)
                |> Array.toList
                |> Result.sequence
            return speedBins 
                    |> SorterPhenotypePerf.fromSorterPerfs
                    |> SorterPerf2.fromSorterPhenotypePerfBins
        }









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
    let create id = Ok (StageWeight id)
    let fromFloat (id:float) = create id |> Result.ExtractOrThrow


type sorterSaving = 
        | NotAny
        | All
        | Successful
        | Perf of stageWeight*sorterCount


module SorterFitness =

    let switchBased (order:order) 
                    (switchCount:switchCount) = 
        let bestSwitch = SwitchCount.orderToRecordSwitchCount order 
                            |> SwitchCount.value |> float
        let scv = switchCount |> SwitchCount.value |> float
        (scv) / (bestSwitch) |> Energy.create


    let stageBased (order:order) 
                   (stageCount:stageCount) = 
        let bestStage = StageCount.orderToRecordStageCount order 
                            |> StageCount.value |> float
        let scv = stageCount |> StageCount.value |> float
        (scv) / (bestStage) |> Energy.create

    let weighted (order:order) 
                 (stageWeight:stageWeight) 
                 (wCt:switchCount)
                 (tCt:stageCount) =
        let wV = switchBased order wCt
                    |> Energy.value
        let tV = stageBased order tCt
                    |> Energy.value
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

