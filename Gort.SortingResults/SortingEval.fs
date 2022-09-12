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
            (sorter:sorter) 
            (switchUseCnters:switchUseCounters) =
        switchUseCnters
        |> getUseCounters
        |> Seq.mapi(fun i w -> i,w)
        |> Seq.filter(fun t -> (snd t) > 0 )
        |> Seq.map(fun t -> sorter.switches.[(fst t)])
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
        successful:bool
        sortrPhenotypeId:sorterPhenotypeId
        sortrId:sorterId
    }

module SorterPerf =

    let fromSorterOpResults
            (sortr:sorter)
            (sortrOpResults:sorterOpResults) =

        let usedSwitches = 
            sortrOpResults 
            |> SwitchUseCounters.fromSorterOpResults 
            |> SwitchUseCounters.getUsedSwitchesFromSorter sortr

        let sortrPhenotypeId = usedSwitches |> SorterPhenotypeId.create

        let usedStageCount = 
            (usedSwitches |> StageCover.getStageCount)
        { 
            sorterPerf.sortrId = sortrOpResults 
                        |> SorterOpResults.getSorterId ;
            sorterPerf.sortrPhenotypeId = sortrPhenotypeId;
            sorterPerf.usedSwitchCount = usedSwitches.Length 
                        |> SwitchCount.create;
            sorterPerf.usedStageCount = usedStageCount;
            sorterPerf.successful = sortrOpResults 
                        |> SorterOpResults.isSorted
        }

    let isSucessful (perf:sorterPerf) =
        perf.successful

    let getUsedSwitchCount (perf:sorterPerf) =
        perf.usedSwitchCount

    let getUsedStageCount (perf:sorterPerf) =
        perf.usedStageCount



//type sorterPerfBin = 
//    { 
//        usedSwitchCount:switchCount; 
//        usedStageCount:stageCount;
//        successCount:sorterCount;
//        failCount:sorterCount;
//    }

//module SorterPerfBin =
    
//    let fromSorterPerf (sorterPrf:sorterPerf) (srterId:sorterId)  =
//        if sorterPrf.successful then
//            {
//                sorterPerfBin.usedSwitchCount = sorterPrf.usedSwitchCount
//                sorterPerfBin.usedStageCount = sorterPrf.usedStageCount
//                sorterPerfBin.successCount = 1 |> SorterCount.create
//                sorterPerfBin.failCount = 0 |> SorterCount.create
//            }
//        else
//            {
//                sorterPerfBin.usedSwitchCount = sorterPrf.usedSwitchCount
//                sorterPerfBin.usedStageCount = sorterPrf.usedStageCount
//                sorterPerfBin.successCount = 0 |> SorterCount.create
//                sorterPerfBin.failCount = 1 |> SorterCount.create
//            }

//    let merge (bins:sorterPerfBin seq) =
//        let _makeKey (bin:sorterPerfBin) =
//            (bin.usedSwitchCount, bin.usedStageCount)
//        let _add (taggedBins:(switchCount*stageCount)*sorterPerfBin array) =
//            let (wc, tc), bins = taggedBins
//            let totSuccessCt = 
//                    bins 
//                        |> Seq.map(fun bin -> 
//                            bin.successCount |> SorterCount.value )
//                        |> Seq.fold (+) 0
//                        |> SorterCount.create

//            let totFailCt = 
//                    bins 
//                        |> Seq.map(fun bin -> 
//                            bin.failCount |> SorterCount.value )
//                        |> Seq.fold (+) 0
//                        |> SorterCount.create
//            {
//                sorterPerfBin.usedSwitchCount = wc;
//                sorterPerfBin.usedStageCount = tc;
//                sorterPerfBin.successCount = totSuccessCt;
//                sorterPerfBin.failCount = totFailCt;
//            }

//        bins 
//            |> Seq.toArray
//            |> Array.groupBy(_makeKey)
//            |> Seq.map(_add)


    
//    let fromSorterPerfs (perfs:sorterPerf seq) =
       
//        perfs |> Seq.map(fromSorterPerf)
//              |> merge


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

