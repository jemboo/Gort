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
    | SorterOpOutput of sorterOpOutput


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
            | ex -> ("error in SorterSpeed.fromSorterOpOutput: " + ex.Message ) 
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
        try
            let sortr = 
                sortrOpResults |> SorterOpOutput.getSorter

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
            | ex -> ("error in SorterPerf.fromSorterOpOutput: " + ex.Message ) 
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

        let _makeSorterOpOutput = 
            SortingRollout.makeSorterOpOutput
                sorterOpTrackMode.SwitchUses
                sortableSt
                sortr

        match sorterEvalMod with
        | SorterSpeed ->
            result {
               let! sout = _makeSorterOpOutput
               let! sopSpeed = sout |> SorterSpeed.fromSorterOpOutput sortr
               return sopSpeed  |> sorterEval.Speed
            }
        | SorterPerf -> 
            result {
               let! sout = _makeSorterOpOutput
               let! sopSpeed = sout |> SorterPerf.fromSorterOpOutput
               return sopSpeed  |> sorterEval.Perf
            }
        | SorterOutput -> 
            result {
               let! sout = _makeSorterOpOutput
               let! sopSpeed = sout |> SorterOutput.fromSorterOpOutput
               return sopSpeed  |> sorterEval.Output
            }
        | SorterOpOutput -> 
            result {
               let! sout = _makeSorterOpOutput
               return sout  |> sorterEval.SorterOpOutput
            }
