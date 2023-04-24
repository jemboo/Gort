namespace global

open System

type sorterPhenotypeId = private SorterPhenotypeId of Guid

module SorterPhenotypeId =
    let value (SorterPhenotypeId v) = v

    let create (id: Guid) =
        id |> SorterPhenotypeId

    let createFromSwitches (switches: seq<switch>) =
        switches
        |> Seq.map (fun sw -> sw :> obj)
        |> GuidUtils.guidFromObjs
        |> SorterPhenotypeId

    
type switchesUsed = private SwitchesUsed of switch[]
module SwitchesUsed = 
    let make (switches: switch[]) =
        SwitchesUsed switches

    let get (sus: switchesUsed) =
        let (SwitchesUsed rv) = sus
        rv

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
            let switchUseCts = 
                   sorterOpOutpt
                   |> SorterOpOutput.getSorterOpTracker
                   |> SorterOpTracker.getSwitchUseCounts

            let switchesUsd =
                    switchUseCts
                    |> SwitchUseCounts.getUsedSwitchesFromSorter sortr
                
            let usedSwitchCt = switchesUsd.Length |> SwitchCount.create
            let usedStageCt = (switchesUsd |> StageCover.getStageCount)
            let sortrPhenotypId = switchesUsd |> SorterPhenotypeId.createFromSwitches
            (create usedSwitchCt usedStageCt, sortrPhenotypId, switchUseCts) |> Ok
        with ex ->
            (sprintf "error in SorterSpeed.fromSorterOpOutput: %s" ex.Message)
            |> Result.Error


    let report (sorterSpd : sorterSpeed option) =
        match sorterSpd with
        | Some ss -> sprintf "%d\t%d"
                         (ss |> getStageCount |> StageCount.value)
                         (ss |> getSwitchCount |> SwitchCount.value)
        | None -> "None\tNone"


    let getStageCount0 (sorterSpd : sorterSpeed option) =
        match sorterSpd with
        | Some ss -> (ss |> getStageCount |> StageCount.value)
        | None -> 0


    let getSwitchCount0 (sorterSpd : sorterSpeed option) =
        match sorterSpd with
        | Some ss -> (ss |> getSwitchCount |> SwitchCount.value)
        | None -> 0


type sorterPerf = | IsSuccessful of bool 
                  | SortedSetSize of sortableCount

module SorterPerf =
    let isSuccessful (sorterPrf:sorterPerf) (ordr:order) =
        match sorterPrf with
        | IsSuccessful bv -> bv
        | SortedSetSize ssz -> (SortableCount.value ssz) < (Order.value ordr) + 1


type sorterEvalMode = | DontCheckSuccess
                      | CheckSuccess 
                      | GetSortedSetCount

type sorterEval =
    private
        { errorMessage: string option
          switchUseCts: switchUseCounts option
          sorterSpeed: sorterSpeed option
          sorterPrf: sorterPerf option
          sortrPhenotypeId: sorterPhenotypeId option
          sortableSetId: sortableSetId
          sortrId: sorterId }


module SorterEval =
    let make (errorMsg:string option)
             (switchUseCts: switchUseCounts option) 
             (sorterSpeed: sorterSpeed option) 
             (sorterPrf: sorterPerf option) 
             (sortrPhenotypeId: sorterPhenotypeId option)
             (sortableStId: sortableSetId)
             (sortrId: sorterId) =
        { errorMessage = errorMsg
          switchUseCts = switchUseCts
          sorterSpeed = sorterSpeed
          sorterPrf = sorterPrf
          sortrPhenotypeId = sortrPhenotypeId
          sortableSetId = sortableStId
          sortrId = sortrId }

    let getErrorMessage (sorterEvl:sorterEval) =
        sorterEvl.errorMessage
    let getSwitchUseCounts (sorterEvl:sorterEval) =
        sorterEvl.switchUseCts
    let getSorterSpeed (sorterEvl:sorterEval) =
        sorterEvl.sorterSpeed
    let getSorterPerf (sorterEvl:sorterEval) =
        sorterEvl.sorterPrf
    let getSortrPhenotypeId (sorterEvl:sorterEval) =
        sorterEvl.sortrPhenotypeId
    let getSortableSetId (sorterEvl:sorterEval) =
        sorterEvl.sortableSetId
    let getSorterId (sorterEvl:sorterEval) =
        sorterEvl.sortrId

    let shouldRetest 
        (sorterEvalMod:sorterEvalMode) 
        (sorterEvl:sorterEval)
        : bool =
        if (sorterEvl.errorMessage |> Option.isSome)
            then false else
        match sorterEvalMod with
        | CheckSuccess -> (sorterEvl.sorterPrf |> Option.isNone)
        | DontCheckSuccess -> (sorterEvl.sorterSpeed |> Option.isNone)
        | GetSortedSetCount -> 
                match sorterEvl.sorterPrf with  
                | None -> true
                | Some perf -> 
                    match perf with
                    | IsSuccessful _ -> true
                    | SortedSetSize _ -> false


    let evalSorterWithSortableSet 
            (sorterPerfEvalMod: sorterEvalMode) 
            (sortableSt: sortableSet) 
            (sortr: sorter) 
            : sorterEval =

        let _addSorterToErrorResultCase sortr resA =
            match resA with
            | Ok rv -> rv |> Ok
            | Error es -> (sortr, es) |> Error

        
        let sortrId = (sortr |> Sorter.getSorterId)
        let sortableStId = (sortableSt |> SortableSet.getSortableSetId)
        let sorterOpOutput =
            SortingRollout.makeSorterOpOutput sorterOpTrackMode.SwitchUses sortableSt sortr

        match sorterOpOutput with
        | Ok output ->
            let res = output |> SorterSpeed.fromSorterOpOutput
            match res with
            | Ok (sorterSpeed, sorterPhenotypId, switchUseCts) ->
                match sorterPerfEvalMod with
                | DontCheckSuccess -> 
                    make None (Some switchUseCts) (Some sorterSpeed) None (Some sorterPhenotypId) sortableStId sortrId
                | CheckSuccess -> 
                    let isSuccessfl = 
                        output 
                        |> SorterOpOutput.isSorted
                        |> sorterPerf.IsSuccessful
                        |> Option.Some
                    make None (Some switchUseCts) (Some sorterSpeed) isSuccessfl (Some sorterPhenotypId) sortableStId sortrId
                | GetSortedSetCount ->
                    let sortedSetCt = output |> SorterOpOutput.getRefinedSortableCount
                    match sortedSetCt with
                    | Ok ct ->
                        let sct = ct |> sorterPerf.SortedSetSize |> Some
                        make None (Some switchUseCts) (Some sorterSpeed) sct (Some sorterPhenotypId) sortableStId sortrId
                    | Error msg ->
                        make (Some msg) (Some switchUseCts) (Some sorterSpeed) None (Some sorterPhenotypId) sortableStId sortrId

            | Error msg -> make (Some msg) None None None None sortableStId sortrId

        | Error msg -> make  (Some msg) None None None None sortableStId sortrId