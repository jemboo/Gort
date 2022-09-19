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