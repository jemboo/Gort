namespace global
open SysExt

type sorterOpTracker = 
     | SwitchUses of sortableBySwitchTracker
     | SwitchTrack of switchUseTracker


module SorterOpTracker =

   let getSwitchUseCounts (sorterOpTracker:sorterOpTracker) =
       match sorterOpTracker with
       | sorterOpTracker.SwitchUses sortableBySwitchTracker -> 
            sortableBySwitchTracker
            |> SortableBySwitchTracker.getSwitchUseCounts
       | sorterOpTracker.SwitchTrack switchUseTracker -> 
            switchUseTracker 
            |> SwitchUseTracker.getSwitchUseCounts



type sorterOpResults = private { 
        sortr:sorter; 
        sortableSt:sortableSet; 
        rollout:rollout; 
        sorterOpTracker:sorterOpTracker; }

module SorterOpResults = 
    
    let make (sortr:sorter) ( sortableSt:sortableSet) 
             (rollout:rollout) (sorterOpTracker:sorterOpTracker) =
        { 
            sorterOpResults.sortr = sortr; 
            sorterOpResults.sortableSt = sortableSt; 
            sorterOpResults.rollout = rollout; 
            sorterOpResults.sorterOpTracker = sorterOpTracker; 
        }

    let getSorter (sorterOpResults:sorterOpResults) =
        sorterOpResults.sortr

    let getRollout (sorterOpResults:sorterOpResults) =
        sorterOpResults.rollout

    let getSorterOpTracker (sorterOpResults:sorterOpResults) =
        sorterOpResults.sorterOpTracker

    let isSorted (sorterOpResults:sorterOpResults) =
        sorterOpResults.rollout |> Rollout.isSorted
