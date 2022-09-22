namespace global
open System

module SorterSetEval =
    
    let eval
        (sorterEvalMod:sorterEvalMode)
        (sortableSt:sortableSet)
        (sorterSt:sorterSet) 
        (useParallel:useParallel) =
        if (useParallel |> UseParallel.value) then
            sorterSt
            |> SorterSet.getSorters
            |> Seq.toArray
            |> Array.Parallel.map(SorterEval.evalSorterWithSortableSet sorterEvalMod sortableSt)
        else
            sorterSt
            |> SorterSet.getSorters
            |> Seq.toArray
            |> Array.map(SorterEval.evalSorterWithSortableSet sorterEvalMod sortableSt)


type sorterSpeedBin = private { 
        usedSwitchCount:switchCount; 
        usedStageCount:stageCount;
        sortrs:sorter[];
        sortrPhenotypeId:sorterPhenotypeId;
    }

module SorterSpeedBin =
    
    let fromSorterSpeeds (sorterSpeeds:seq<sorterSpeed>) =
        let _makeBinFromSamePhenotypes
                (spId:sorterPhenotypeId) 
                (sorterSpeeds:seq<sorterSpeed>) =
            let memA = sorterSpeeds |> Seq.toArray
            {
                sorterSpeedBin.usedSwitchCount = memA.[0].usedSwitchCount
                sorterSpeedBin.usedStageCount = memA.[0].usedStageCount
                sorterSpeedBin.sortrs =
                                memA 
                                |> Array.map(fun sp -> sp.sortr)
                sorterSpeedBin.sortrPhenotypeId = spId
            }

        sorterSpeeds 
        |> Seq.groupBy(fun sp -> sp.sortrPhenotypeId)
        |> Seq.map(fun (spId, mbrs) -> _makeBinFromSamePhenotypes spId mbrs)

    let getUsedSwitchCount (perfBin:sorterSpeedBin) =
        perfBin.usedSwitchCount


    let getUsedStageCount (perfBin:sorterSpeedBin) =
        perfBin.usedStageCount


    let getSorters (perfBin:sorterSpeedBin) =
        perfBin.sortrs


    let getSorterPhenotypeId (perfBin:sorterSpeedBin) =
        perfBin.sortrPhenotypeId


type sorterSpeedBinReport = private { 
        usedSwitchCount:switchCount; 
        usedStageCount:stageCount;
        sortrCount:sorterCount
        phenotypeCount:int;
    }


module SorterSpeedBinReport =

    let getUsedSwitchCount (perfR:sorterSpeedBinReport) =
        perfR.usedSwitchCount

    let getUsedStageCount (perfR:sorterSpeedBinReport) =
        perfR.usedStageCount

    let getSorterCount (perfR:sorterSpeedBinReport) =
        perfR.sortrCount

    let getPhenotypeCount (perfR:sorterSpeedBinReport) =
        perfR.phenotypeCount

    // Similar bins have the same switch and stage counts
    let mergeSimilarBins 
        (sorterSpeedBinReprtA:sorterSpeedBinReport) 
        (sorterSpeedBinReprtB:sorterSpeedBinReport) =
        { 
            sorterSpeedBinReport.usedSwitchCount = sorterSpeedBinReprtA.usedSwitchCount; 
            sorterSpeedBinReport.usedStageCount = sorterSpeedBinReprtA.usedStageCount;
            sorterSpeedBinReport.sortrCount = 
                sorterSpeedBinReprtA.sortrCount |> SorterCount.add sorterSpeedBinReprtB.sortrCount;
            sorterSpeedBinReport.phenotypeCount = 
                sorterSpeedBinReprtA.phenotypeCount + sorterSpeedBinReprtB.phenotypeCount;
        }


    let fromSorterSpeedBin (sorterSpeedBn:sorterSpeedBin) =
        { 
            sorterSpeedBinReport.usedSwitchCount = sorterSpeedBn.usedSwitchCount; 
            sorterSpeedBinReport.usedStageCount = sorterSpeedBn.usedStageCount;
            sorterSpeedBinReport.sortrCount = 
                sorterSpeedBn.sortrs.Length |> SorterCount.create;
            sorterSpeedBinReport.phenotypeCount = 1;
        }

    let fromSorterSpeedBins (sorterSpeedBns:seq<sorterSpeedBin>) =
        sorterSpeedBns
        |> Seq.map(fromSorterSpeedBin)
        |> Seq.groupBy(fun spb -> (spb, spb))
        |> Seq.map(fun gp -> gp |> snd |> Seq.reduce(mergeSimilarBins))



type sorterPerfBin = private { 
        usedSwitchCount:switchCount; 
        usedStageCount:stageCount;
        isSuccessful:bool
        sortrs:sorter[];
        sortrPhenotypeId:sorterPhenotypeId;
    }


module SorterPerfBin =
    
    let fromSorterPerfs (sorterPerfs:seq<sorterPerf>) =
        let _makeBinFromSamePhenotypes
                (spId:sorterPhenotypeId) 
                (sorterPerfs:seq<sorterPerf>) =
            let memA = sorterPerfs |> Seq.toArray
            {
                sorterPerfBin.usedSwitchCount = memA.[0].usedSwitchCount
                sorterPerfBin.usedStageCount = memA.[0].usedStageCount
                sorterPerfBin.isSuccessful = memA.[0].isSuccessful
                sorterPerfBin.sortrs =
                                memA 
                                |> Array.map(fun sp -> sp.sortr)
                sorterPerfBin.sortrPhenotypeId = spId
            }

        sorterPerfs 
        |> Seq.groupBy(fun sp -> sp.sortrPhenotypeId)
        |> Seq.map(fun (spId, mbrs) -> _makeBinFromSamePhenotypes spId mbrs)


    let getIsSucessful (perfBin:sorterPerfBin) =
        perfBin.isSuccessful


    let getUsedSwitchCount (perfBin:sorterPerfBin) =
        perfBin.usedSwitchCount


    let getUsedStageCount (perfBin:sorterPerfBin) =
        perfBin.usedStageCount


    let getSorters (perfBin:sorterPerfBin) =
        perfBin.sortrs


    let getSorterPhenotypeId (perfBin:sorterPerfBin) =
        perfBin.sortrPhenotypeId


type sorterPerfBinReport = private { 
        usedSwitchCount:switchCount; 
        usedStageCount:stageCount;
        successfulSorters:sorter[]
        failingSorters:sorter[]
        successfulPhenotypeCount:int;
        failurePhenotypeCount:int;
    }


module SorterPerfBinReport =

    let getUsedSwitchCount (perfR:sorterPerfBinReport) =
        perfR.usedSwitchCount

    let getUsedStageCount (perfR:sorterPerfBinReport) =
        perfR.usedStageCount

    let getSucessCount (perfR:sorterPerfBinReport) =
        perfR.successfulSorters.Length

    let getFailureCount (perfR:sorterPerfBinReport) =
        perfR.failingSorters.Length

    let getSuccessfulPhenotypeCount (perfR:sorterPerfBinReport) =
        perfR.successfulPhenotypeCount

    let getFailurePhenotypeCount (perfR:sorterPerfBinReport) =
        perfR.failurePhenotypeCount

    // Similar bins have the same switch and stage counts
    let mergeSimilarBins 
        (sorterPerfBinReprtA:sorterPerfBinReport) 
        (sorterPerfBinReprtB:sorterPerfBinReport) =
        { 
            sorterPerfBinReport.usedSwitchCount = sorterPerfBinReprtA.usedSwitchCount; 
            sorterPerfBinReport.usedStageCount = sorterPerfBinReprtA.usedStageCount;
            sorterPerfBinReport.successfulSorters = 
                sorterPerfBinReprtA.successfulSorters |> Array.append sorterPerfBinReprtB.successfulSorters;
            sorterPerfBinReport.failingSorters =
                sorterPerfBinReprtA.failingSorters |> Array.append sorterPerfBinReprtB.failingSorters;
            sorterPerfBinReport.successfulPhenotypeCount = 
                sorterPerfBinReprtA.successfulPhenotypeCount + sorterPerfBinReprtB.successfulPhenotypeCount;
            sorterPerfBinReport.failurePhenotypeCount =
                sorterPerfBinReprtA.failurePhenotypeCount + sorterPerfBinReprtB.failurePhenotypeCount;
        }


    let fromSorterPerfBin (sorterPerfBn:sorterPerfBin) =
        if (sorterPerfBn.isSuccessful) then
            { 
                sorterPerfBinReport.usedSwitchCount = sorterPerfBn.usedSwitchCount; 
                sorterPerfBinReport.usedStageCount = sorterPerfBn.usedStageCount;
                sorterPerfBinReport.successfulSorters = sorterPerfBn.sortrs;
                sorterPerfBinReport.failingSorters = [||];
                sorterPerfBinReport.successfulPhenotypeCount = 1;
                sorterPerfBinReport.failurePhenotypeCount = 0;
            }
            else
            { 
                sorterPerfBinReport.usedSwitchCount = sorterPerfBn.usedSwitchCount; 
                sorterPerfBinReport.usedStageCount = sorterPerfBn.usedStageCount;
                sorterPerfBinReport.successfulSorters = [||];
                sorterPerfBinReport.failingSorters = sorterPerfBn.sortrs;
                sorterPerfBinReport.successfulPhenotypeCount = 0;
                sorterPerfBinReport.failurePhenotypeCount = 1;
            }

    let fromSorterPerfBins (sorterPerfBns:seq<sorterPerfBin>) =
        sorterPerfBns
        |> Seq.map(fromSorterPerfBin)
        |> Seq.groupBy(fun spb -> (spb, spb))
        |> Seq.map(fun gp -> gp |> snd |> Seq.reduce(mergeSimilarBins))













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

