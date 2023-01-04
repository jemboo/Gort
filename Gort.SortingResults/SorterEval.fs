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

    let makeEmpty (switchCount: switchCount) =
        { switchUseCounters.useCounts = Array.zeroCreate (switchCount |> SwitchCount.value) }

    let make (useCounts: int[]) =
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
        sorterOpTrackr |> SorterOpTracker.getSwitchUseCounts |> make

    let fromSorterOpOutput (sorterOpRes: sorterOpOutput) =
        sorterOpRes |> SorterOpOutput.getSorterOpTracker |> fromSorterOpTracker


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
            let switchesUsd =
                sorterOpOutpt
                |> SwitchUseCounters.fromSorterOpOutput
                |> SwitchUseCounters.getUsedSwitchesFromSorter sortr
                
            let usedSwitchCt = switchesUsd.Length |> SwitchCount.create
            let usedStageCt = (switchesUsd |> StageCover.getStageCount)
            let sortrPhenotypId = switchesUsd |> SorterPhenotypeId.create
            (create usedSwitchCt usedStageCt, sortrPhenotypId, switchesUsd |> SwitchesUsed.make) |> Ok
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
        { errorMessage: string option
          sorterSpeed: sorterSpeed option
          sorterPrf: sorterPerf option
          sortrPhenotypeId: sorterPhenotypeId option
          sortrId: sorterId }


module SorterEval =
    let make (errorMsg:string option)
             (sorterSpeed: sorterSpeed option) 
             (sorterPrf: sorterPerf option) 
             (sortrPhenotypeId: sorterPhenotypeId option) 
             (sortrId: sorterId) =
        { errorMessage = errorMsg
          sorterSpeed = sorterSpeed
          sorterPrf = sorterPrf
          sortrPhenotypeId = sortrPhenotypeId
          sortrId = sortrId }

    let getSorterSpeed (sorterEvl:sorterEval) =
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

        
        let sortrId = (sortr |> Sorter.getSorterId)
        let sorterOpOutput =
            SortingRollout.makeSorterOpOutput sorterOpTrackMode.SwitchUses sortableSt sortr

        match sorterOpOutput with
        | Ok output ->
            let res = output |> SorterSpeed.fromSorterOpOutput
            match res with
            | Ok (sorterSpeed, sorterPhenotypId, switchesUsd) ->
                match sorterPerfEvalMod with
                | DontCheckSuccess -> 
                    make None (Some sorterSpeed) None (Some sorterPhenotypId) sortrId
                | CheckSuccess -> 
                    let isSuccessfl = 
                        output 
                            |> SorterOpOutput.isSorted
                            |> sorterPerf.IsSuccessful
                            |> Option.Some
                    make None (Some sorterSpeed) isSuccessfl (Some sorterPhenotypId) sortrId
                | GetSortedSetCount ->
                    let sortedSetCt = output |> SorterOpOutput.getRefinedSortableCount
                    match sortedSetCt with
                    | Ok ct ->
                        let sct = ct |> sorterPerf.SortedSetSize |> Some
                        make None (Some sorterSpeed) sct (Some sorterPhenotypId) sortrId
                    | Error msg ->
                        make (Some msg) (Some sorterSpeed) None (Some sorterPhenotypId) sortrId

            | Error msg -> make (Some msg) None None None sortrId

        | Error msg -> make (Some msg) None None None sortrId

        //result {
        //    let! sorterOpOutpt = _makeSorterOpOutput
        //    let! sorterSpeed, sorterPhenotypId, switchesUsd = 
        //         sorterOpOutpt |> SorterSpeed.fromSorterOpOutput

        //    match sorterPerfEvalMod with
        //          | DontCheckSuccess -> 
        //             return make None (Some sorterSpeed) None (Some sorterPhenotypId) sortrId
        //          | CheckSuccess -> 
        //             let isSuccessfl = 
        //                sorterOpOutpt 
        //                  |> SorterOpOutput.isSorted
        //                  |> sorterPerf.IsSuccessful
        //                  |> Option.Some
        //             return make sorterSpeed isSuccessfl sorterPhenotypId sortrId
        //          | GetSortedSetCount ->
        //             let! sortedSetCt = 
        //                sorterOpOutpt 
        //                 |> SorterOpOutput.getRefinedSortableCount
        //                 |> Result.map(sorterPerf.SortedSetSize)
        //                 |> Result.map(Option.Some)
        //             return make sorterSpeed sortedSetCt sorterPhenotypId sortrId
        //}


