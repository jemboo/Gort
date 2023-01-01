namespace global


type sorterPhenotypeCount = private SorterPhenotypeCount of int
module SorterPhenotypeCount =
    let value (SorterPhenotypeCount v) = v
    let create (spbCt: int) =
        spbCt |> SorterPhenotypeCount


type sorterPhenotypeBin =
    private
        { sorterSpeed: sorterSpeed
          sortrIds: sorterId[]
          sortrPhenotypeId: sorterPhenotypeId 
        }
module SorterPhenotypeBin =

    let fromSorterEvals (sorterEvals: seq<sorterEval>) =
        let _makeBinFromSamePhenotypes 
                (spId: sorterPhenotypeId) 
                (sorterSpeeds: seq<sorterEval>) =
            let memA = sorterSpeeds |> Seq.toArray
            { 
              sorterPhenotypeBin.sorterSpeed = memA.[0].sorterSpeed
              sorterPhenotypeBin.sortrIds = memA |> Array.map (fun sp -> sp.sortrId)
              sorterPhenotypeBin.sortrPhenotypeId = spId
            }

        sorterEvals
        |> Seq.groupBy (fun sp -> sp.sortrPhenotypeId)
        |> Seq.map (fun (spId, mbrs) -> _makeBinFromSamePhenotypes spId mbrs)


    let getSorterSpeed (sorterPhenotypeBin: sorterPhenotypeBin) = 
            sorterPhenotypeBin.sorterSpeed

    let getUsedSwitchCount (sorterPhenotypeBin: sorterPhenotypeBin) =
        sorterPhenotypeBin.sorterSpeed |> SorterSpeed.getSwitchCount

    let getUsedStageCount (sorterPhenotypeBin: sorterPhenotypeBin) =
        sorterPhenotypeBin.sorterSpeed |> SorterSpeed.getStageCount

    let getSorterIds (sorterPhenotypeBin: sorterPhenotypeBin) =
        sorterPhenotypeBin.sortrIds

    let getSorterPhenotypeId (sorterPhenotypeBin: sorterPhenotypeBin) = 
            sorterPhenotypeBin.sortrPhenotypeId

    let unRoll (sorterPhenotypeCt:sorterPhenotypeCount) 
               (sorterPhenotypeBn:sorterPhenotypeBin) = 
            sorterPhenotypeBn 
            |> getSorterIds
            |> Array.map(fun srtrId -> 
                (
                 sorterPhenotypeBn.sorterSpeed, 
                 srtrId, 
                 sorterPhenotypeBn.sortrPhenotypeId, 
                 sorterPhenotypeBn.sortrIds.Length |> SorterCount.create,
                 sorterPhenotypeCt
                )       )


type sorterSpeedBin =
    private
        { sorterSpeed: sorterSpeed
          sorterPhenotypeBns: sorterPhenotypeBin[]
        }
module SorterSpeedBin =

    let getSorterSpeed (sorterSpeedBn: sorterSpeedBin) = 
            sorterSpeedBn.sorterSpeed

    let getUsedSwitchCount (sorterSpeedBn: sorterSpeedBin) =
            sorterSpeedBn.sorterSpeed |> SorterSpeed.getSwitchCount

    let getUsedStageCount (sorterSpeedBn: sorterSpeedBin) =
            sorterSpeedBn.sorterSpeed |> SorterSpeed.getStageCount

    let getSorterCount (sorterSpeedBn: sorterSpeedBin) =
        sorterSpeedBn.sorterPhenotypeBns
        |> Array.sumBy (fun spsb -> spsb.sortrIds.Length)
        |> SorterCount.create

    let getPhenotypeBins (sorterSpeedBn: sorterSpeedBin) = 
            sorterSpeedBn.sorterPhenotypeBns

    // Similar bins have the same switch and stage counts
    let mergeSimilarBins
        (sorterSpeedBinA: sorterSpeedBin)
        (sorterSpeedBinB: sorterSpeedBin)  
        : sorterSpeedBin =
        { sorterSpeedBin.sorterSpeed = sorterSpeedBinA.sorterSpeed
          sorterSpeedBin.sorterPhenotypeBns =
            Array.append sorterSpeedBinA.sorterPhenotypeBns sorterSpeedBinB.sorterPhenotypeBns }


    let fromSorterPhenotypeBin (sorterPhenotypeBn: sorterPhenotypeBin) =
        { sorterSpeedBin.sorterSpeed = sorterPhenotypeBn.sorterSpeed
          sorterSpeedBin.sorterPhenotypeBns = [| sorterPhenotypeBn |] }


    let fromSorterPhenotpeBins (sorterPhenotypeBns: seq<sorterPhenotypeBin>) =
        sorterPhenotypeBns
        |> Seq.map (fromSorterPhenotypeBin)
        |> Seq.groupBy (fun spb -> spb |> getSorterSpeed)
        |> Seq.map (fun gp -> gp |> snd |> Seq.reduce (mergeSimilarBins))


    let toSorterPhenotypeBins (sorterSpeedBins: seq<sorterSpeedBin>) =
        sorterSpeedBins |> Seq.map(fun ssb -> getPhenotypeBins ssb)
                        |> Seq.concat
        
    let orderSorterPhenotypesBySliceAndSpeed 
                    (speedRanker: sorterSpeed -> float)
                    (sorterSpeedBins: seq<sorterSpeedBin>) =
        let orderedPhenotypeBins =
            sorterSpeedBins 
            |> Seq.sortByDescending(fun sb -> sb.sorterSpeed |> speedRanker)
            |> toSorterPhenotypeBins
            |> Seq.toArray

        let maxBinSz = orderedPhenotypeBins
                        |> Seq.map(fun pb -> pb.sortrIds.Length)
                        |> Seq.max
        seq {
            let mutable sizeDex = 0
            for sizeDex = 0 to (maxBinSz - 1) do
                let mutable binDex = 0
                for binDex = 0 to (orderedPhenotypeBins.Length - 1) do
                    if orderedPhenotypeBins.[binDex].sortrIds.Length >= sizeDex then
                        yield orderedPhenotypeBins.[binDex].sortrIds.[sizeDex], orderedPhenotypeBins.[binDex].sorterSpeed
                binDex <- binDex + 1
            sizeDex <- sizeDex + 1
        }


    let fromSorterEvals (sorterSpeeds: seq<sorterEval>) =
        sorterSpeeds
        |> SorterPhenotypeBin.fromSorterEvals
        |> fromSorterPhenotpeBins


type sorterPopulationContext =
    private
        { 
          sorterSpeed: sorterSpeed
          sorterPrf: sorterPerf option
          sortrId: sorterId
          sortrPhenotypeId: sorterPhenotypeId 
          sorterCountForPhenotype: sorterCount
          phenotypeCountForSorterSpeed: sorterPhenotypeCount
        }


module SorterPopulationContext =

    let makeNoPerf (qt:sorterSpeed*sorterId*sorterPhenotypeId*sorterCount*sorterPhenotypeCount) =
        let ss, sid, spid, scfp, pcfss = qt
        {
            sorterPopulationContext.sorterSpeed = ss
            sorterPrf = None
            sortrId = sid
            sortrPhenotypeId = spid
            sorterCountForPhenotype = scfp
            phenotypeCountForSorterSpeed = pcfss
        }


    let fromSorterSpeedBins (sorterSpeedBns:sorterSpeedBin[]) =
        let _unRoll (bin:sorterSpeedBin) =
            bin |> SorterSpeedBin.getPhenotypeBins
                |> Array.map(SorterPhenotypeBin.unRoll 
                     (bin.sorterPhenotypeBns.Length |> SorterPhenotypeCount.create))
                |> Array.concat
                |> Array.map(makeNoPerf)

        sorterSpeedBns |> Array.map(_unRoll)
                       |> Array.concat


type stageWeight = private StageWeight of float
module StageWeight =
    let value (StageWeight v) = v
    let create vl = StageWeight vl


// larger fitness numbers are better
type sorterFitness = private SorterFitness of float
module SorterFitness =
    let value (SorterFitness v) = v
    let create (spbCt: float) =
        spbCt |> SorterFitness

    let fromSpeed (stageWght:stageWeight) (sorterSpd:sorterSpeed) = 
        (stageWght |> StageWeight.value) /
        (sorterSpd |> SorterSpeed.getStageCount |> StageCount.value |> float)
        +
        1.0 /
        (sorterSpd |> SorterSpeed.getSwitchCount |> SwitchCount.value |> float)



type selectionFraction = private SelectionFraction of float
module SelectionFraction =
    let value (SelectionFraction v) = v
    let create vl = SelectionFraction vl



//type sorterPhenotypePerf =
//    private
//        { sorterSpeedBn: sorterSpeedBin
//          sorterPrf: sorterPerf
//          sortrs: sorter[]
//          sortrPhenotypeId: sorterPhenotypeId }

//module SorterPhenotypePerf =

//    let fromSorterPerfs (sorterPerfs: seq<sorterPerfEval>) =
//        let _makeBinFromSamePhenotypes (spId: sorterPhenotypeId) (sorterPerfs: seq<sorterPerfEval>) =
//            let memA = sorterPerfs |> Seq.toArray
//            { sorterPhenotypePerf.sorterSpeedBn = memA.[0].sorterSpeedBn
//              sorterPhenotypePerf.sorterPrf = memA.[0].sorterPrf
//              sorterPhenotypePerf.sortrs = memA |> Array.map (fun sp -> sp.sortr)
//              sorterPhenotypePerf.sortrPhenotypeId = spId }

//        sorterPerfs
//        |> Seq.groupBy (fun sp -> sp.sortrPhenotypeId)
//        |> Seq.map (fun (spId, mbrs) -> _makeBinFromSamePhenotypes spId mbrs)


//    let getSorterPerf (perfBin: sorterPhenotypePerf) = perfBin.sorterPrf

//    let getUsedSwitchCount (perfBin: sorterPhenotypePerf) =
//        perfBin.sorterSpeedBn |> SorterSpeedBin.getSwitchCount

//    let getUsedStageCount (perfBin: sorterPhenotypePerf) =
//        perfBin.sorterSpeedBn |> SorterSpeedBin.getStageCount

//    let getSorters (perfBin: sorterPhenotypePerf) = perfBin.sortrs

//    let getSorterPhenotypeId (perfBin: sorterPhenotypePerf) = perfBin.sortrPhenotypeId


//type sorterPhenotypePerfsForSpeedBin =
//    private
//        { sorterSpeedBn: sorterSpeedBin
//          failingPhenotypeBins: sorterPhenotypePerf[]
//          successfulPhenotypeBins: sorterPhenotypePerf[] }

//module SorterPhenotypePerfsForSpeedBin =
//    let getSpeedBin (perfR: sorterPhenotypePerfsForSpeedBin) = perfR.sorterSpeedBn

//    let getUsedSwitchCount (perfR: sorterPhenotypePerfsForSpeedBin) =
//        perfR.sorterSpeedBn |> SorterSpeedBin.getSwitchCount

//    let getUsedStageCount (perfR: sorterPhenotypePerfsForSpeedBin) =
//        perfR.sorterSpeedBn |> SorterSpeedBin.getStageCount

//    let getSucessCount (perfR: sorterPhenotypePerfsForSpeedBin) =
//        perfR.successfulPhenotypeBins |> Array.sumBy (fun spb -> spb.sortrs.Length)

//    let getFailureCount (perfR: sorterPhenotypePerfsForSpeedBin) =
//        perfR.failingPhenotypeBins |> Array.sumBy (fun spb -> spb.sortrs.Length)

//    let getTotalCount (perfR: sorterPhenotypePerfsForSpeedBin) =
//        (perfR |> getSucessCount) + (perfR |> getFailureCount)

//    let getSuccessfulPhenotypeCount (perfR: sorterPhenotypePerfsForSpeedBin) = perfR.successfulPhenotypeBins.Length

//    let getFailurePhenotypeCount (perfR: sorterPhenotypePerfsForSpeedBin) = perfR.failingPhenotypeBins.Length

//    let hasAFailure (perfR: sorterPhenotypePerfsForSpeedBin) = (getFailurePhenotypeCount perfR) > 0

//    let isSameBin (sorterPerfBn: sorterPhenotypePerfsForSpeedBin) 
//                  (sorterSpeedBn: sorterPhenotypeSpeedsForSpeedBin) =
//        ((sorterPerfBn |> getUsedStageCount) = 
//                    (sorterSpeedBn |> SorterPhenotypeSpeedsForSpeedBin.getUsedStageCount))
//        && 
//        ((sorterPerfBn |> getUsedSwitchCount) = 
//                (sorterSpeedBn |> SorterPhenotypeSpeedsForSpeedBin.getUsedSwitchCount))


//    let hasSameCount (sorterPerfBn: sorterPhenotypePerfsForSpeedBin) 
//                     (sorterSpeedBn: sorterPhenotypeSpeedsForSpeedBin) =
//        ((sorterPerfBn |> getTotalCount) = (sorterSpeedBn
//                                            |> SorterPhenotypeSpeedsForSpeedBin.getSorterCount
//                                            |> SorterCount.value))

//    let couldBeTheSame
//        (sorterPerfBn: sorterPhenotypePerfsForSpeedBin)
//        (sorterSpeedBn: sorterPhenotypeSpeedsForSpeedBin) =
//        (isSameBin sorterPerfBn sorterSpeedBn)
//        && (hasSameCount sorterPerfBn sorterSpeedBn)

//    // Similar bins have the same switch and stage counts
//    let mergeSimilarBins
//        (sorterPerfBinReprtA: sorterPhenotypePerfsForSpeedBin)
//        (sorterPerfBinReprtB: sorterPhenotypePerfsForSpeedBin) =
//        { sorterPhenotypePerfsForSpeedBin.sorterSpeedBn = sorterPerfBinReprtA.sorterSpeedBn
//          sorterPhenotypePerfsForSpeedBin.failingPhenotypeBins =
//            sorterPerfBinReprtA.failingPhenotypeBins
//            |> Array.append sorterPerfBinReprtB.failingPhenotypeBins
//          sorterPhenotypePerfsForSpeedBin.successfulPhenotypeBins =
//            sorterPerfBinReprtA.successfulPhenotypeBins
//            |> Array.append sorterPerfBinReprtB.successfulPhenotypeBins }


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





//module SortingReport =

//    let reportSpeedBins (sorterEvls: sorterEval[]) =
//        result {
//            let! speedBins =
//                sorterEvls
//                |> Array.map (SorterEval.getSorterSpeed)
//                |> Array.toList
//                |> Result.sequence

//            return
//                speedBins
//                |> SorterPhenotypeSpeed.fromSorterSpeeds
//                |> SorterPhenotypeSpeedsForSpeedBin.fromSorterPhenotpeSpeedBins
//        }


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