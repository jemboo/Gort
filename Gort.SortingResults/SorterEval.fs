﻿namespace global

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
            create usedSwitchCt usedStageCt |> Ok
        with ex ->
            (sprintf "error in SorterSpeedBin.fromSorterOpOutput: %s" ex.Message)
            |> Result.Error

type sorterPerf = IsSuccessful of bool | SortedSetSize of sortableCount

type sorterSpeedEval =
    private
        { sorterSpeedBn: sorterSpeedBin
          sortrPhenotypeId: sorterPhenotypeId
          sortr: sorter }


module SorterSpeedEval =
    let make (sorterSpeedBn: sorterSpeedBin)
             (sorterPhenotypId: sorterPhenotypeId) 
             (sortr: sorter) =
        { sorterSpeedEval.sortr = sortr
          sorterSpeedEval.sortrPhenotypeId = sorterPhenotypId
          sorterSpeedEval.sorterSpeedBn = sorterSpeedBn }

    let fromSorterOpOutput (sortr: sorter) 
                           (sorterOpOutpt: sorterOpOutput) =
        result {
            let usedSwitches =
                sorterOpOutpt
                |> SwitchUseCounters.fromSorterOpOutput
                |> SwitchUseCounters.getUsedSwitchesFromSorter sortr

            let sortr = sorterOpOutpt |> SorterOpOutput.getSorter
            let sortrPhenotypId = usedSwitches |> SorterPhenotypeId.create
            let! sorterSpeedBn = sorterOpOutpt 
                                |> SorterSpeedBin.fromSorterOpOutput

            return make sorterSpeedBn sortrPhenotypId sortr
        }

    let getSorterSpeedBn (sorterSpeedEvl: sorterSpeedEval) = sorterSpeedEvl.sorterSpeedBn

    let getSorter (sorterSpeedEvl: sorterSpeedEval) = sorterSpeedEvl.sortr

    let getSortrPhenotypeId (sorterSpeedEvl: sorterSpeedEval) = sorterSpeedEvl.sortrPhenotypeId



type sorterPerfEval =
    private
        { sorterSpeedBn: sorterSpeedBin
          isSuccessful: bool
          sortrPhenotypeId: sorterPhenotypeId
          sortr: sorter }


module SorterPerfEval =
    let make
        (sorterSpeedBn: sorterSpeedBin)
        (isSuccessfl: bool)
        (sorterPhenotypId: sorterPhenotypeId)
        (sortr: sorter)  =
        { 
          sorterPerfEval.sortr = sortr
          sorterPerfEval.sortrPhenotypeId = sorterPhenotypId
          sorterPerfEval.sorterSpeedBn = sorterSpeedBn
          sorterPerfEval.isSuccessful = isSuccessfl 
        }

    let fromSorterOpOutput 
            (sorterOpOutpt: sorterOpOutput) =

        let sortr = sorterOpOutpt |> SorterOpOutput.getSorter
        result {
            let usedSwitches =
                sorterOpOutpt
                |> SwitchUseCounters.fromSorterOpOutput
                |> SwitchUseCounters.getUsedSwitchesFromSorter sortr

            let sortrPhenotypId = usedSwitches |> SorterPhenotypeId.create
            let! sorterSpeedBn = sorterOpOutpt 
                                    |> SorterSpeedBin.fromSorterOpOutput
            let isSuccessfl = sorterOpOutpt |> SorterOpOutput.isSorted

            return make sorterSpeedBn isSuccessfl sortrPhenotypId sortr
        }

    let getIsSucessful (perf:sorterPerfEval) = perf.isSuccessful

    let getSorterSpeedBin (perf:sorterPerfEval) = perf.sorterSpeedBn

    let getSorter (perf:sorterPerfEval) = perf.sortr

    let getSortrPhenotypeId (perf:sorterPerfEval) = perf.sortrPhenotypeId



type sorterOutput =
    private
        { sortr: sorter
          refinedSortables: rollout }


type sorterEval =
    | Speed of sorterSpeedEval
    | Perf of sorterPerfEval
    | Output of sorterOutput
    | OpOutput of sorterOpOutput

module SorterOutput =

    let getSorter (sorterOutpt: sorterOutput) = sorterOutpt.sortr

    let getRefinedSortables (sorterOutpt: sorterOutput) = sorterOutpt.refinedSortables

    let getRefinedSortableCount (sorterOutpt: sorterOutput) =
        sorterOutpt.refinedSortables
        |> Rollout.getArrayLength
        |> ArrayLength.value
        |> SortableCount.create

    let fromSorterOpOutput (sorterOpOutpt: sorterOpOutput) =
        result {
            let! refs = sorterOpOutpt |> SorterOpOutput.getRefinedSortableSet

            return
                { sorterOutput.sortr = sorterOpOutpt |> SorterOpOutput.getSorter
                  sorterOutput.refinedSortables = refs }
        }


type sorterEvalMode =
    | SorterSpeed
    | SorterPerf
    | SorterOutput
    | SorterOpOutput


module SorterEval =

    let evalSorterWithSortableSet 
            (sorterEvalMod: sorterEvalMode) 
            (sortableSt: sortableSet) 
            (sortr: sorter) =

        let _addSorterToErrorResultCase sortr resA =
            match resA with
            | Ok rv -> rv |> Ok
            | Error es -> (sortr, es) |> Error

        let _makeSorterOpOutput =
            SortingRollout.makeSorterOpOutput sorterOpTrackMode.SwitchUses sortableSt sortr
            |> _addSorterToErrorResultCase sortr

        match sorterEvalMod with
        | SorterSpeed ->
            result {
                let! sout = _makeSorterOpOutput
                let! sorterSpeed = sout |> SorterSpeedEval.fromSorterOpOutput sortr 
                                        |> _addSorterToErrorResultCase sortr
                return sorterSpeed |> sorterEval.Speed
            }
        | SorterPerf ->
            result {
                let! sout = _makeSorterOpOutput
                let! sorterPerf = sout |> SorterPerfEval.fromSorterOpOutput 
                                       |> _addSorterToErrorResultCase sortr
                return sorterPerf |> sorterEval.Perf
            }
        | SorterOutput ->
            result {
                let! sout = _makeSorterOpOutput
                let! sorterOutput = sout |> SorterOutput.fromSorterOpOutput 
                                         |> _addSorterToErrorResultCase sortr

                return sorterOutput |> sorterEval.Output
            }
        | SorterOpOutput ->
            result {
                let! sorterOpOutput = _makeSorterOpOutput
                return sorterOpOutput |> sorterEval.OpOutput
            }


    let getSorterSpeed (sorterEvl: sorterEval) =
        match sorterEvl with
        | Speed ss -> ss |> Ok
        | _ -> "a sorterSpeed is required" |> Error


    let getSorterPerf (sorterEvl: sorterEval) =
        match sorterEvl with
        | Perf ss -> ss |> Ok
        | _ -> "a sorterPerf is required" |> Error


    let getSorterOutput (sorterEvl: sorterEval) =
        match sorterEvl with
        | Output ss -> ss |> Ok
        | _ -> "a sorterOutput is required" |> Error


    let getSorterOpOutput (sorterEvl: sorterEval) =
        match sorterEvl with
        | OpOutput ss -> ss |> Ok
        | _ -> "a sorterOpOutput is required" |> Error