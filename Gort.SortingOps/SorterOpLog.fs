namespace global
open SysExt

type sorterOpTrack = 
        | NoGrouping of sorterId * sortableSetId * rollout * switchBySortableTrack
        | BySwitch of sorterId * sortableSetId * rollout * switchUseTrack


module SorterOpTrack =

    let getRollout (sortingResults:sorterOpTrack) = 
        match sortingResults with
        | NoGrouping (_, _, rollout, _) -> rollout
        | BySwitch (_, _, rollout, _ ) -> rollout

    let getSorterId (sortingResults:sorterOpTrack) = 
        match sortingResults with
        | NoGrouping (sorterId, _, _, _) -> sorterId
        | BySwitch (sorterId, _, _, _ ) -> sorterId

    let isSorted (sortingResults:sorterOpTrack) = 
        sortingResults |> getRollout |> Rollout.isSorted
