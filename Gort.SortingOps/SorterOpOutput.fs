namespace global
open SysExt


type sorterOpOutput = private { 
        sortr:sorter; 
        sortableSt:sortableSet; 
        transformedRollout:rollout; 
        sorterOpTracker:sorterOpTracker; }

module SorterOpOutput = 
    
    let make (sortr:sorter) ( sortableSt:sortableSet) 
             (transRollout:rollout) (sorterOpTracker:sorterOpTracker) =
        { 
            sorterOpOutput.sortr = sortr; 
            sorterOpOutput.sortableSt = sortableSt; 
            sorterOpOutput.transformedRollout = transRollout; 
            sorterOpOutput.sorterOpTracker = sorterOpTracker; 
        }

    let getSorter (sorterOpOutput:sorterOpOutput) =
        sorterOpOutput.sortr

    let getSortableSet (sorterOpOutput:sorterOpOutput) =
        sorterOpOutput.sortableSt

    let getTransformedRollout (sorterOpOutput:sorterOpOutput) =
        sorterOpOutput.transformedRollout

    let getSorterOpTracker (sorterOpOutput:sorterOpOutput) =
        sorterOpOutput.sorterOpTracker

    let isSorted (sorterOpOutput:sorterOpOutput) =
        sorterOpOutput.transformedRollout |> Rollout.isSorted

    let getRefinedSortableSet (sorterOpOutput:sorterOpOutput) =
        sorterOpOutput.transformedRollout 
        |> Rollout.uniqueUnsortedMembers

    let getUnsortedCt (sorterOpOutput:sorterOpOutput) =
        sorterOpOutput.transformedRollout
        |> Rollout.uniqueUnsortedMembers
        |> Result.map Rollout.getArrayCount
