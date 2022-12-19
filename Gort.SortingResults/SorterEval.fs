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



type sorterSpeed = private { switchCt:switchCount; stageCt:stageCount }

module SorterSpeed =
    let create (switchCt: switchCount) (stageCt: stageCount) =
        { switchCt = switchCt
          stageCt = stageCt }

    let getSwitchCount (sorterSpeedBn: sorterSpeed) = sorterSpeedBn.switchCt

    let getStageCount (sorterSpeedBn: sorterSpeed) = sorterSpeedBn.stageCt

    let toIndex (sorterSpeedBn: sorterSpeed) =
        let switchCtV = sorterSpeedBn.switchCt |> SwitchCount.value
        let stageCtV = sorterSpeedBn.stageCt |> StageCount.value
        ((switchCtV * (switchCtV + 1)) / 2) + stageCtV

    let fromIndex (index: int) =
        let indexFlt = (index |> float) + 1.0
        let p = (sqrt (1.0 + 8.0 * indexFlt) - 1.0) / 2.0
        let pfloor = Math.Floor(p)

        if (p = pfloor) then
            let stageCt = 1 |> (-) (int pfloor) |> StageCount.create
            let switchCt = 1 |> (-) (int pfloor) |> SwitchCount.create

            { sorterSpeed.switchCt = switchCt
              stageCt = stageCt }
        else
            let stageCt =
                (float index) - (pfloor * (pfloor + 1.0)) / 2.0 |> int |> StageCount.create

            let switchCt = (int pfloor) |> SwitchCount.create

            { sorterSpeed.switchCt = switchCt
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
        { sorterSpeed: sorterSpeed
          sorterPrf: sorterPerf option
          sortrPhenotypeId: sorterPhenotypeId
          sortrId: sorterId }



module SorterEval =
    let make (sorterSpeed: sorterSpeed) 
             (sorterPrf: sorterPerf option) 
             (sortrPhenotypeId: sorterPhenotypeId) 
             (sortrId: sorterId) =
        { sorterSpeed = sorterSpeed
          sorterPrf = sorterPrf
          sortrPhenotypeId = sortrPhenotypeId
          sortrId = sortrId }

    let getSorterSpeedBin (sorterEvl:sorterEval) =
        sorterEvl.sorterSpeed
    let getSorterPerf (sorterEvl:sorterEval) =
        sorterEvl.sorterPrf
    let getSortrPhenotypeId (sorterEvl:sorterEval) =
        sorterEvl.sortrPhenotypeId
    let getSorterId (sorterEvl:sorterEval) =
        sorterEvl.sortrId

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

        let sortrId = (sortr |> Sorter.getSorterId)

        result {
            let! sorterOpOutpt = _makeSorterOpOutput
            let! sorterSpeedBn, sorterPhenotypId = 
                 sorterOpOutpt |> SorterSpeed.fromSorterOpOutput

            match sorterPerfEvalMod with
                  | DontCheckSuccess -> 
                     return make sorterSpeedBn None sorterPhenotypId sortrId
                  | CheckSuccess -> 
                     let isSuccessfl = 
                        sorterOpOutpt 
                          |> SorterOpOutput.isSorted
                          |> sorterPerf.IsSuccessful
                          |> Option.Some
                     return make sorterSpeedBn isSuccessfl sorterPhenotypId sortrId
                  | GetSortedSetCount ->
                     let! sortedSetCt = 
                        sorterOpOutpt 
                         |> SorterOpOutput.getRefinedSortableCount
                         |> Result.map(sorterPerf.SortedSetSize)
                         |> Result.map(Option.Some)
                     return make sorterSpeedBn sortedSetCt sorterPhenotypId sortrId
        }