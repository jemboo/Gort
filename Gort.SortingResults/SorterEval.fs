namespace global
open System

type sorterPhenotypeId = private SorterPhenotypeId of Guid

type switchUseCounters = private { useCounts:int[] }

type sorterSpeed = private  { 
        usedSwitchCount:switchCount; 
        usedStageCount:stageCount;
        sortrPhenotypeId:sorterPhenotypeId
        sortr:sorter
    }


type sorterPerf = private  {
        usedSwitchCount:switchCount; 
        usedStageCount:stageCount;
        isSuccessful:bool
        sortrPhenotypeId:sorterPhenotypeId
        sortr:sorter
    }


type sorterOutput = private {
    sortr:sorter;
    refinedSortables:rollout
}


type sorterEval = 
    | Speed of sorterSpeed 
    | Perf of sorterPerf 
    | Output of sorterOutput
    | OpOutput of sorterOpOutput


module SorterPhenotypeId =
    let value (SorterPhenotypeId v) = v
    let create (switches:seq<switch>) = 
        switches |> Seq.map(fun sw -> sw :> obj)
         |> GuidUtils.guidFromObjs
         |> SorterPhenotypeId


module SwitchUseCounters =

    let make (switchCount:switchCount) =
        { switchUseCounters.useCounts = 
                Array.zeroCreate (switchCount |> SwitchCount.value)}


    let apply (useCounts:int[]) =
        { switchUseCounters.useCounts = useCounts}


    let getUseCounters (switchUseCountrs:switchUseCounters) =
        switchUseCountrs.useCounts


    let getUsedSwitchCount (switchUseCountrs:switchUseCounters) =
        switchUseCountrs.useCounts
        |> Seq.filter((<) 0)
        |> Seq.length
        |> SwitchCount.create


    let getUsedSwitchesFromSorter
            (sortr:sorter) 
            (switchUseCountrs:switchUseCounters) =
        switchUseCountrs
        |> getUseCounters
        |> Seq.mapi(fun i w -> i,w)
        |> Seq.filter(fun t -> (snd t) > 0 )
        |> Seq.map(fun t -> (sortr |> Sorter.getSwitches).[(fst t)])
        |> Seq.toArray


    let fromSorterOpTracker (sorterOpTrackr:sorterOpTracker) =
        sorterOpTrackr 
        |> SorterOpTracker.getSwitchUseCounts
        |> apply


    let fromSorterOpOutput (sorterOpRes:sorterOpOutput) =
        sorterOpRes 
        |> SorterOpOutput.getSorterOpTracker
        |> fromSorterOpTracker



module SorterSpeed =
    let make (switchCt:switchCount) (stageCt:stageCount) 
             (sorterPhenotypId:sorterPhenotypeId) (sortr:sorter)  =
        { 
            sorterSpeed.sortr = sortr 
            sorterSpeed.sortrPhenotypeId = sorterPhenotypId;
            sorterSpeed.usedSwitchCount = switchCt 
            sorterSpeed.usedStageCount = stageCt;
        }

    let fromSorterOpOutput
            (sortr:sorter)
            (sortrOpResults:sorterOpOutput) =
        try
            let usedSwitches = 
                sortrOpResults 
                |> SwitchUseCounters.fromSorterOpOutput 
                |> SwitchUseCounters.getUsedSwitchesFromSorter sortr

            let sortr = 
                sortrOpResults |> SorterOpOutput.getSorter
            let sortrPhenotypId = 
                usedSwitches |> SorterPhenotypeId.create
            let usedSwitchCt = 
                usedSwitches.Length |> SwitchCount.create;
            let usedStageCt = 
                (usedSwitches |> StageCover.getStageCount)

            make usedSwitchCt usedStageCt sortrPhenotypId sortr
            |> Ok
        with 
            | ex ->
                (sprintf "error in SorterSpeed.fromSorterOpOutput: %s" ex.Message)
                |> Result.Error 
            

    let getUsedSwitchCount (perf:sorterSpeed) =
        perf.usedSwitchCount

    let getUsedStageCount (perf:sorterSpeed) =
        perf.usedStageCount

    let getSorter (perf:sorterSpeed) =
        perf.sortr

    let getSortrPhenotypeId (perf:sorterSpeed) =
        perf.sortrPhenotypeId


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

    let fromSorterOpOutput
            (sortrOpResults:sorterOpOutput) =
        
        let sortr = 
            sortrOpResults |> SorterOpOutput.getSorter
        try
            let usedSwitches = 
                sortrOpResults 
                |> SwitchUseCounters.fromSorterOpOutput 
                |> SwitchUseCounters.getUsedSwitchesFromSorter sortr

            let sortrPhenotypId = 
                usedSwitches |> SorterPhenotypeId.create
            let usedSwitchCt = 
                usedSwitches.Length |> SwitchCount.create;
            let usedStageCt = 
                (usedSwitches |> StageCover.getStageCount)
            let isSuccessfl = 
                sortrOpResults |> SorterOpOutput.isSorted

            make usedSwitchCt usedStageCt isSuccessfl sortrPhenotypId sortr
                 |> Ok
        with 
            | ex -> 
                (sprintf "error in SorterPerf.fromSorterOpOutput: %s" ex.Message ) 
                |> Result.Error 


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


module SorterOutput =

    let getSorter (sorterOutpt:sorterOutput) =
        sorterOutpt.sortr

    let getRefinedSortables (sorterOutpt:sorterOutput) =
        sorterOutpt.refinedSortables

    let getRefinedSortableCount (sorterOutpt:sorterOutput) =
        sorterOutpt.refinedSortables
        |> Rollout.getArrayLength
        |> ArrayLength.value
        |> SortableCount.create

    let fromSorterOpOutput (sorterOpOutpt:sorterOpOutput) =
        result {
            let! refs = sorterOpOutpt |> SorterOpOutput.getRefinedSortableSet
            return
                {
                    sorterOutput.sortr = sorterOpOutpt |> SorterOpOutput.getSorter;
                    sorterOutput.refinedSortables = refs
                }
        }


type sorterEvalMode = 
    | SorterSpeed 
    | SorterPerf 
    | SorterOutput
    | SorterOpOutput


module SorterEval =

    let evalSorterWithSortableSet 
        (sorterEvalMod:sorterEvalMode)
        (sortableSt:sortableSet)
        (sortr:sorter) = 

        let _changeResultType sortr resA  = 
            match resA with
            | Ok rv -> rv |> Ok
            | Error es -> (sortr, es) |> Error

        let _makeSorterOpOutput = 
            SortingRollout.makeSorterOpOutput
                sorterOpTrackMode.SwitchUses
                sortableSt
                sortr
            |> _changeResultType sortr

        match sorterEvalMod with
        | SorterSpeed ->
            result {
               let! sout = _makeSorterOpOutput
               let! sorterSpeed = 
                        sout 
                        |> SorterSpeed.fromSorterOpOutput sortr
                        |> _changeResultType sortr
               return sorterSpeed  |> sorterEval.Speed
            }
        | SorterPerf -> 
            result {
               let! sout = _makeSorterOpOutput
               let! sorterPerf = 
                    sout 
                    |> SorterPerf.fromSorterOpOutput
                    |> _changeResultType sortr
               return sorterPerf  |> sorterEval.Perf
            }
        | SorterOutput -> 
            result {
               let! sout = _makeSorterOpOutput
               let! sorterOutput = 
                        sout 
                        |> SorterOutput.fromSorterOpOutput
                        |> _changeResultType sortr
                                        
               return sorterOutput  |> sorterEval.Output
            }
        | SorterOpOutput -> 
            result {
               let! sorterOpOutput = _makeSorterOpOutput
               return sorterOpOutput  |> sorterEval.OpOutput
            }


    let getSorterSpeed (sorterEvl:sorterEval) =
        match sorterEvl with
        | Speed ss -> ss |> Ok
        | _ -> "a sorterSpeed is required" |> Error


    let getSorterPerf (sorterEvl:sorterEval) =
        match sorterEvl with
        | Perf ss -> ss |> Ok
        | _ -> "a sorterPerf is required" |> Error


    let getSorterOutput (sorterEvl:sorterEval) =
        match sorterEvl with
        | Output ss -> ss |> Ok
        | _ -> "a sorterOutput is required" |> Error


    let getSorterOpOutput (sorterEvl:sorterEval) =
        match sorterEvl with
        | OpOutput ss -> ss |> Ok
        | _ -> "a sorterOpOutput is required" |> Error


//type sorterEval = 
//    | Speed of sorterSpeed 
//    | Perf of sorterPerf 
//    | Output of sorterOutput
//    | SorterOpOutput of sorterOpOutput


