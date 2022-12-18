namespace global

open System

type sorterPhenotypeId = private SorterPhenotypeId of Guid

module SorterPhenotypeId =
    let value (SorterPhenotypeId v) = v

    let create (switches: seq<switch>) =
        switches
        |> Seq.map (fun sw -> sw :> obj)
        |> GuidUtils.guidFromObjs
        |> SorterPhenotypeId

    

type switchUseCounters = private { useCounts: int[] }

module SwitchUseCounters =

    let make (switchCount: switchCount) =
        { switchUseCounters.useCounts = Array.zeroCreate (switchCount |> SwitchCount.value) }

    let apply (useCounts: int[]) =
        { switchUseCounters.useCounts = useCounts }

    let getUseCounters (switchUseCountrs: switchUseCounters) = switchUseCountrs.useCounts

    let getUsedSwitchCount (switchUseCountrs: switchUseCounters) =
        switchUseCountrs.useCounts
        |> Seq.filter ((<) 0)
        |> Seq.length
        |> SwitchCount.create

    let getUsedSwitchesFromSorter (sortr: sorter) (switchUseCountrs: switchUseCounters) =
        switchUseCountrs
        |> getUseCounters
        |> Seq.mapi (fun i w -> i, w)
        |> Seq.filter (fun t -> (snd t) > 0)
        |> Seq.map (fun t -> (sortr |> Sorter.getSwitches).[(fst t)])
        |> Seq.toArray

    let fromSorterOpTracker (sorterOpTrackr: sorterOpTracker) =
        sorterOpTrackr |> SorterOpTracker.getSwitchUseCounts |> apply

    let fromSorterOpOutput (sorterOpRes: sorterOpOutput) =
        sorterOpRes |> SorterOpOutput.getSorterOpTracker |> fromSorterOpTracker



type sorterSpeedBin = private { switchCt:switchCount; stageCt:stageCount }

module SorterSpeedBin =
    let create (switchCt: switchCount) (stageCt: stageCount) =
        { switchCt = switchCt
          stageCt = stageCt }

    let getSwitchCount (sorterSpeedBn: sorterSpeedBin) = sorterSpeedBn.switchCt

    let getStageCount (sorterSpeedBn: sorterSpeedBin) = sorterSpeedBn.stageCt

    let getIndexOfBin (sorterSpeedBn: sorterSpeedBin) =
        let switchCtV = sorterSpeedBn.switchCt |> SwitchCount.value
        let stageCtV = sorterSpeedBn.stageCt |> StageCount.value
        ((switchCtV * (switchCtV + 1)) / 2) + stageCtV

    let getBinFromIndex (index: int) =
        let indexFlt = (index |> float) + 1.0
        let p = (sqrt (1.0 + 8.0 * indexFlt) - 1.0) / 2.0
        let pfloor = Math.Floor(p)

        if (p = pfloor) then
            let stageCt = 1 |> (-) (int pfloor) |> StageCount.create
            let switchCt = 1 |> (-) (int pfloor) |> SwitchCount.create

            { sorterSpeedBin.switchCt = switchCt
              stageCt = stageCt }
        else
            let stageCt =
                (float index) - (pfloor * (pfloor + 1.0)) / 2.0 |> int |> StageCount.create

            let switchCt = (int pfloor) |> SwitchCount.create

            { sorterSpeedBin.switchCt = switchCt
              stageCt = stageCt }


    let fromSorterOpOutput (sorterOpOutpt: sorterOpOutput) =
        let sortr = sorterOpOutpt |> SorterOpOutput.getSorter
        try
            let usedSwitches =
                sorterOpOutpt
                |> SwitchUseCounters.fromSorterOpOutput
                |> SwitchUseCounters.getUsedSwitchesFromSorter sortr

            let usedSwitchCt = usedSwitches.Length |> SwitchCount.create
            let usedStageCt = (usedSwitches |> StageCover.getStageCount)
            let sortrPhenotypId = usedSwitches |> SorterPhenotypeId.create
            (create usedSwitchCt usedStageCt, sortrPhenotypId) |> Ok
        with ex ->
            (sprintf "error in SorterSpeedBin.fromSorterOpOutput: %s" ex.Message)
            |> Result.Error


type sorterPerf = | IsSuccessful of bool 
                  | SortedSetSize of sortableCount

module SorterPerf =
    let isSuccessful (sorterPrf:sorterPerf) (ordr:order) =
        match sorterPrf with
        | IsSuccessful bv -> bv
        | SortedSetSize ssz -> (SortableCount.value ssz) < (Order.value ordr) + 1


type sorterPerfEvalMode = | DontCheckSuccess
                          | CheckSuccess 
                          | GetSortedSetCount


type sorterEval =
    private
        { sorterSpeedBn: sorterSpeedBin
          sorterPrf: sorterPerf option
          sortrPhenotypeId: sorterPhenotypeId
          sortr: sorter }



module SorterEval =
    let make (sorterSpeedBn: sorterSpeedBin) 
             (sorterPrf: sorterPerf option) 
             (sortrPhenotypeId: sorterPhenotypeId) 
             (sortr: sorter) =
        { sorterSpeedBn = sorterSpeedBn
          sorterPrf = sorterPrf
          sortrPhenotypeId = sortrPhenotypeId
          sortr = sortr }

    let getSorterSpeedBin (sorterEvl:sorterEval) =
        sorterEvl.sorterSpeedBn
    let getSorterPerf (sorterEvl:sorterEval) =
        sorterEvl.sorterPrf
    let getSortrPhenotypeId (sorterEvl:sorterEval) =
        sorterEvl.sortrPhenotypeId
    let getSorter (sorterEvl:sorterEval) =
        sorterEvl.sortr

    let evalSorterWithSortableSet 
            (sorterPerfEvalMod: sorterPerfEvalMode) 
            (sortableSt: sortableSet) 
            (sortr: sorter) =

        let _addSorterToErrorResultCase sortr resA =
            match resA with
            | Ok rv -> rv |> Ok
            | Error es -> (sortr, es) |> Error

        let _makeSorterOpOutput =
            SortingRollout.makeSorterOpOutput sorterOpTrackMode.SwitchUses sortableSt sortr

        result {
            let! sorterOpOutpt = _makeSorterOpOutput
            let! sorterSpeedBn, sorterPhenotypId = 
                 sorterOpOutpt |> SorterSpeedBin.fromSorterOpOutput

            match sorterPerfEvalMod with
                  | DontCheckSuccess -> 
                     return make sorterSpeedBn None sorterPhenotypId sortr
                  | CheckSuccess -> 
                     let isSuccessfl = 
                        sorterOpOutpt 
                          |> SorterOpOutput.isSorted
                          |> sorterPerf.IsSuccessful
                          |> Option.Some
                     return make sorterSpeedBn isSuccessfl sorterPhenotypId sortr
                  | GetSortedSetCount ->
                     let! sortedSetCt = 
                        sorterOpOutpt 
                         |> SorterOpOutput.getRefinedSortableCount
                         |> Result.map(sorterPerf.SortedSetSize)
                         |> Result.map(Option.Some)
                     return make sorterSpeedBn sortedSetCt sorterPhenotypId sortr
        }


//    let evalSorterWithSortableSet 
//            (sorterEvalMod: sorterEvalMode) 
//            (sortableSt: sortableSet) 
//            (sortr: sorter) =

//        let _addSorterToErrorResultCase sortr resA =
//            match resA with
//            | Ok rv -> rv |> Ok
//            | Error es -> (sortr, es) |> Error

//        let _makeSorterOpOutput =
//            SortingRollout.makeSorterOpOutput sorterOpTrackMode.SwitchUses sortableSt sortr
//            |> _addSorterToErrorResultCase sortr

//        match sorterEvalMod with
//        | SorterSpeed ->
//            result {
//                let! sout = _makeSorterOpOutput
//                let! sorterSpeed = sout |> SorterSpeedEval.fromSorterOpOutput sortr 
//                                        |> _addSorterToErrorResultCase sortr
//                return sorterSpeed |> sorterEval.Speed
//            }
//        | SorterPerf m ->
//            result {
//                let! sout = _makeSorterOpOutput
//                let! sorterPerf = sout |> SorterPerfEval.fromSorterOpOutput <| m
//                                       |> _addSorterToErrorResultCase sortr
//                return sorterPerf |> sorterEval.Perf
//            }

//    let getSorterSpeed (sorterEvl: sorterEval) =
//        match sorterEvl with
//        | Speed ss -> ss |> Ok
//        | _ -> "a sorterSpeed is required" |> Error


//    let getSorterPerf (sorterEvl: sorterEval) =
//        match sorterEvl with
//        | Perf ss -> ss |> Ok
//        | _ -> "a sorterPerf is required" |> Error






//type sorterPerfEval =
//    private
//        { sorterSpeedBn: sorterSpeedBin
//          sorterPrf: sorterPerf
//          sortrPhenotypeId: sorterPhenotypeId
//          sortr: sorter }


//type sorterPerfEvalMode = | Success | SortedSetCount









//type sorterEvalMode =
//    | SorterSpeed
//    | SorterPerf of sorterPerfEvalMode


//module SorterPerfEval =
//    let make
//        (sorterSpeedBn: sorterSpeedBin)
//        (sorterPrf: sorterPerf)
//        (sorterPhenotypId: sorterPhenotypeId)
//        (sortr: sorter)  =
//        { 
//          sorterPerfEval.sortr = sortr
//          sorterPerfEval.sortrPhenotypeId = sorterPhenotypId
//          sorterPerfEval.sorterSpeedBn = sorterSpeedBn
//          sorterPerfEval.sorterPrf = sorterPrf 
//        }


//    let fromSorterOpOutput (sorterOpOutpt:sorterOpOutput) 
//                           (sorterPerfEvalMod:sorterPerfEvalMode) =
//        let sortr = sorterOpOutpt |> SorterOpOutput.getSorter
//        result {
//            let usedSwitches =
//                sorterOpOutpt
//                |> SwitchUseCounters.fromSorterOpOutput
//                |> SwitchUseCounters.getUsedSwitchesFromSorter sortr

//            let sortrPhenotypId = usedSwitches |> SorterPhenotypeId.create
//            let! sorterSpeedBn = sorterOpOutpt 
//                                    |> SorterSpeedBin.fromSorterOpOutput

//            match sorterPerfEvalMod with
//                  | Success ->
//                    let isSuccessfl = sorterOpOutpt 
//                                        |> SorterOpOutput.isSorted
//                                        |> sorterPerf.IsSuccessful
//                    return make sorterSpeedBn isSuccessfl sortrPhenotypId sortr
//                  | SortedSetCount ->
//                    let! sortedSetCt = sorterOpOutpt 
//                                        |> SorterOpOutput.getRefinedSortableCount
//                                        |> Result.map(sorterPerf.SortedSetSize)
//                    return make sorterSpeedBn sortedSetCt sortrPhenotypId sortr
//        }


//    let getSorterPerfS (perf:sorterPerfEval) = perf.sorterPrf

//    let getSorterSpeedBin (perf:sorterPerfEval) = perf.sorterSpeedBn

//    let getSorter (perf:sorterPerfEval) = perf.sortr

//    let getSortrPhenotypeId (perf:sorterPerfEval) = perf.sortrPhenotypeId



//type sorterOutput =
//    private
//        { sortr: sorter
//          refinedSortables: rollout }


//type sorterEval =
//    | Speed of sorterSpeedEval
//    | Perf of sorterPerfEval


//module SorterOutput =

//    let getSorter (sorterOutpt: sorterOutput) = sorterOutpt.sortr

//    let getRefinedSortables (sorterOutpt: sorterOutput) = sorterOutpt.refinedSortables

//    let getRefinedSortableCount (sorterOutpt: sorterOutput) =
//        sorterOutpt.refinedSortables
//        |> Rollout.getArrayLength
//        |> ArrayLength.value
//        |> SortableCount.create

//    let fromSorterOpOutput (sorterOpOutpt: sorterOpOutput) =
//        result {
//            let! refs = sorterOpOutpt |> SorterOpOutput.getRefinedSortableSet

//            return
//                { sorterOutput.sortr = sorterOpOutpt |> SorterOpOutput.getSorter
//                  sorterOutput.refinedSortables = refs }
//        }


//module SorterEval =

//    let evalSorterWithSortableSet 
//            (sorterEvalMod: sorterEvalMode) 
//            (sortableSt: sortableSet) 
//            (sortr: sorter) =

//        let _addSorterToErrorResultCase sortr resA =
//            match resA with
//            | Ok rv -> rv |> Ok
//            | Error es -> (sortr, es) |> Error

//        let _makeSorterOpOutput =
//            SortingRollout.makeSorterOpOutput sorterOpTrackMode.SwitchUses sortableSt sortr
//            |> _addSorterToErrorResultCase sortr

//        match sorterEvalMod with
//        | SorterSpeed ->
//            result {
//                let! sout = _makeSorterOpOutput
//                let! sorterSpeed = sout |> SorterSpeedEval.fromSorterOpOutput sortr 
//                                        |> _addSorterToErrorResultCase sortr
//                return sorterSpeed |> sorterEval.Speed
//            }
//        | SorterPerf m ->
//            result {
//                let! sout = _makeSorterOpOutput
//                let! sorterPerf = sout |> SorterPerfEval.fromSorterOpOutput <| m
//                                       |> _addSorterToErrorResultCase sortr
//                return sorterPerf |> sorterEval.Perf
//            }

//    let getSorterSpeed (sorterEvl: sorterEval) =
//        match sorterEvl with
//        | Speed ss -> ss |> Ok
//        | _ -> "a sorterSpeed is required" |> Error


//    let getSorterPerf (sorterEvl: sorterEval) =
//        match sorterEvl with
//        | Perf ss -> ss |> Ok
//        | _ -> "a sorterPerf is required" |> Error
