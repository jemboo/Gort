namespace global
open SysExt

type sorterOpTracker = 
     | NoGrouping of sortableBySwitchTracker
     | GroupBySwitch of switchUseTracker


module SorterOpTracker =

   let getSwitchUseCounts (sorterOpTracker:sorterOpTracker) =
       match sorterOpTracker with
       | sorterOpTracker.NoGrouping sortableBySwitchTracker -> 
            sortableBySwitchTracker
            |> SortableBySwitchTracker.getSwitchUseCounts
       | sorterOpTracker.GroupBySwitch switchUseTracker -> 
            switchUseTracker 
            |> SwitchUseTracker.getSwitchUseCounts



type sorterOpResults = private { sorterId:sorterId; sortableSetId:sortableSetId; 
        rollout:rollout; sorterOpTracker:sorterOpTracker; }

module SorterOpResults = 
    
    let make (sorterId:sorterId) ( sortableSetId:sortableSetId) 
             (rollout:rollout) (sorterOpTracker:sorterOpTracker) =
        { sorterId=sorterId; sortableSetId=sortableSetId; 
          rollout=rollout; sorterOpTracker=sorterOpTracker; }

    let getRollout (sorterOpResults:sorterOpResults) =
        sorterOpResults.rollout

    let getSorterOpTracker (sorterOpResults:sorterOpResults) =
        sorterOpResults.sorterOpTracker

    let isSorted (sorterOpResults:sorterOpResults) =
        sorterOpResults.rollout |> Rollout.isSorted
