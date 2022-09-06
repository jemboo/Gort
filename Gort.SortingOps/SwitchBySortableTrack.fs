namespace global
open SysExt


type switchBySortableTrackStandard = private { useRoll:booleanRoll }
module SwitchBySortableTrackStandard = 
    let create (switchCount:switchCount) 
               (sortableCount:sortableCount) =

        let arrayLength = 
                switchCount 
                |> SwitchCount.value 
                |> ArrayLength.createNr

        let arrayCount = 
                sortableCount 
                |> SortableCount.value 
                |> ArrayCount.createNr

        let booleanRoll = BooleanRoll.create arrayCount arrayLength
        { switchBySortableTrackStandard.useRoll = booleanRoll } 

    let getUseRoll (switchingTrackArrayRoll:switchBySortableTrackStandard) =
        switchingTrackArrayRoll.useRoll

    let getData (switchingTrackArrayRoll:switchBySortableTrackStandard) =
        switchingTrackArrayRoll.useRoll
        |> BooleanRoll.getData

    let getSortableCount (switchingTrackArrayRoll:switchBySortableTrackStandard) =
        switchingTrackArrayRoll.useRoll |> BooleanRoll.getArrayCount
                                       |> ArrayCount.value
                                       |> SortableCount.create

    let getSwitchCount (switchingTrackArrayRoll:switchBySortableTrackStandard) =
        switchingTrackArrayRoll.useRoll |> BooleanRoll.getArrayLength
                                       |> ArrayLength.value
                                       |> SwitchCount.create


type switchBySortableTrackBitStriped = private { useRoll:uint64Roll }

module SwitchBySortableTrackBitStriped =

    let create (switchCount:switchCount) 
               (sortableCount:sortableCount) = 

        let arrayCount = sortableCount 
                          |> SortableCount.value
                          |> ArrayCount.createNr

        let arrayLength = switchCount
                          |> SwitchCount.value
                          |> ArrayLength.createNr

        let useRoll = Uint64Roll.createEmptyStripedSet 
                            arrayLength
                            arrayCount

        { useRoll = useRoll }

    let getSortableCount (switchingTrackBitStriped:switchBySortableTrackBitStriped) =
        switchingTrackBitStriped.useRoll |> Uint64Roll.getArrayCount
                                       |> ArrayCount.value
                                       |> SortableCount.create

    let getSwitchCount (switchingTrackBitStriped:switchBySortableTrackBitStriped) =
        switchingTrackBitStriped.useRoll |> Uint64Roll.getArrayLength
                                       |> ArrayLength.value
                                       |> SwitchCount.create

    let getUseRoll (switchingTrackBitStriped:switchBySortableTrackBitStriped) =
        switchingTrackBitStriped.useRoll


    let getData (switchingTrackBitStriped:switchBySortableTrackBitStriped) =
        switchingTrackBitStriped.useRoll
        |> Uint64Roll.getData


type switchBySortableTrack =
     | ArrayRoll of switchBySortableTrackStandard
     | BitStriped of switchBySortableTrackBitStriped

module SwitchBySortableTrack = 
    let getRollingDataTrack (switchingTrack:switchBySortableTrack) = 
        match switchingTrack with
        | ArrayRoll switchingTrackArrayRoll ->
            switchingTrackArrayRoll |> SwitchBySortableTrackStandard.getData
        | BitStriped _ -> failwith "not implemented"