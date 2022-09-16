namespace global
open System
open SysExt


type sorterPhenotypeId = private SorterPhenotypeId of Guid
module SorterPhenotypeId =
    let value (SorterPhenotypeId v) = v
    let create (switches:seq<switch>) = 
        switches |> Seq.map(fun sw -> sw :> obj)
         |> GuidUtils.guidFromObjs
         |> SorterPhenotypeId

type switchUseCounters = private { useCounts:int[] }
module SwitchUseCounters =

    let make (switchCount:switchCount) =
        { switchUseCounters.useCounts = 
                Array.zeroCreate (switchCount |> SwitchCount.value)}


    let apply (useCounts:int[]) =
        { switchUseCounters.useCounts = useCounts}


    let getUseCounters (switchUseCounts:switchUseCounters) =
        switchUseCounts.useCounts


    let getUsedSwitchCount (switchUseCounts:switchUseCounters) =
        switchUseCounts.useCounts
        |> Seq.filter((<) 0)
        |> Seq.length
        |> SwitchCount.create


    let getUsedSwitchesFromSorter
            (sortr:sorter) 
            (switchUseCnters:switchUseCounters) =
        switchUseCnters
        |> getUseCounters
        |> Seq.mapi(fun i w -> i,w)
        |> Seq.filter(fun t -> (snd t) > 0 )
        |> Seq.map(fun t -> (sortr |> Sorter.getSwitches).[(fst t)])
        |> Seq.toArray


    let fromSorterOpResults (sorterOpRes:sorterOpResults) =
        sorterOpRes 
        |> SorterOpResults.getSorterOpTracker
        |> SorterOpTracker.getSwitchUseCounts
        |> apply


    let fromSorterOpTracker (sorterOpTrackr:sorterOpTracker) =
        sorterOpTrackr 
        |> SorterOpTracker.getSwitchUseCounts
        |> apply



type sorterPerf = private  { 
        usedSwitchCount:switchCount; 
        usedStageCount:stageCount;
        isSuccessful:bool
        sortrPhenotypeId:sorterPhenotypeId
        sortr:sorter
    }

module SorterPerf =
    let make (switchCt:switchCount) (stageCt:stageCount) 
             (isSuccessfl:bool) (sorterPhenotypId:sorterPhenotypeId)  
             (sortr:sorter)  =
        { 
            sorterPerf.sortr = sortr 
            sorterPerf.sortrPhenotypeId = sorterPhenotypId;
            sorterPerf.usedSwitchCount = switchCt 
            sorterPerf.usedStageCount = stageCt;
            sorterPerf.isSuccessful = isSuccessfl
        }

    let fromSorterOpResults
            (sortr:sorter)
            (sortrOpResults:sorterOpResults) =

        let usedSwitches = 
            sortrOpResults 
            |> SwitchUseCounters.fromSorterOpResults 
            |> SwitchUseCounters.getUsedSwitchesFromSorter sortr

        let sortr = 
            sortrOpResults |> SorterOpResults.getSorter
        let sortrPhenotypId = 
            usedSwitches |> SorterPhenotypeId.create
        let usedSwitchCt = 
            usedSwitches.Length |> SwitchCount.create;
        let usedStageCt = 
            (usedSwitches |> StageCover.getStageCount)
        let isSuccessfl = 
            sortrOpResults |> SorterOpResults.isSorted

        make usedSwitchCt usedStageCt isSuccessfl sortrPhenotypId sortr


    let getIsSucessful (perf:sorterPerf) =
        perf.isSuccessful

    let getUsedSwitchCount (perf:sorterPerf) =
        perf.usedSwitchCount

    let getUsedStageCount (perf:sorterPerf) =
        perf.usedStageCount

    let getSorter (perf:sorterPerf) =
        perf.sortr

    let getSortrPhenotypeId (perf:sorterPerf) =
        perf.sortrPhenotypeId


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
        successCount:sorterCount
        failCount:sorterCount
        successfulPhenotypeCount:int;
        failurePhenotypeCount:int;
    }

module SorterPerfBinReport =

    let getUsedSwitchCount (perfR:sorterPerfBinReport) =
        perfR.usedSwitchCount

    let getUsedStageCount (perfR:sorterPerfBinReport) =
        perfR.usedStageCount

    let getSucessCount (perfR:sorterPerfBinReport) =
        perfR.successCount

    let getDistinctSuccessfulPhenotypeCount (perfR:sorterPerfBinReport) =
        perfR.successfulPhenotypeCount

    let getSortrPhenotypeId (perfR:sorterPerfBinReport) =
        perfR.failurePhenotypeCount

    // Similar bins have the same switch and stage counts
    let mergeSimilarBins 
        (sorterPerfBinReprtA:sorterPerfBinReport) 
        (sorterPerfBinReprtB:sorterPerfBinReport) =
        { 
            sorterPerfBinReport.usedSwitchCount = sorterPerfBinReprtA.usedSwitchCount; 
            sorterPerfBinReport.usedStageCount = sorterPerfBinReprtA.usedStageCount;
            sorterPerfBinReport.successCount = 
                sorterPerfBinReprtA.successCount |> SorterCount.add sorterPerfBinReprtB.successCount;
            sorterPerfBinReport.failCount =
                sorterPerfBinReprtA.failCount |> SorterCount.add sorterPerfBinReprtB.failCount;
            sorterPerfBinReport.successfulPhenotypeCount = 
                sorterPerfBinReprtA.successfulPhenotypeCount + sorterPerfBinReprtB.successfulPhenotypeCount;
            sorterPerfBinReport.failurePhenotypeCount =
                sorterPerfBinReprtA.failurePhenotypeCount + sorterPerfBinReprtB.failurePhenotypeCount;
        }


    let fromSorterPerfBin (sorterPerfBn:sorterPerfBin) =
        let sortersInBin = sorterPerfBn.sortrs.Length |> SorterCount.create
        let zerSorters = 0 |> SorterCount.create
        if (sorterPerfBn.isSuccessful) then
            { 
                sorterPerfBinReport.usedSwitchCount = sorterPerfBn.usedSwitchCount; 
                sorterPerfBinReport.usedStageCount = sorterPerfBn.usedStageCount;
                sorterPerfBinReport.successCount = sortersInBin;
                sorterPerfBinReport.failCount = zerSorters;
                sorterPerfBinReport.successfulPhenotypeCount = 1;
                sorterPerfBinReport.failurePhenotypeCount = 0;
            }
            else
            { 
                sorterPerfBinReport.usedSwitchCount = sorterPerfBn.usedSwitchCount; 
                sorterPerfBinReport.usedStageCount = sorterPerfBn.usedStageCount;
                sorterPerfBinReport.successCount = zerSorters;
                sorterPerfBinReport.failCount = sortersInBin;
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

