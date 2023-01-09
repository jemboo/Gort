namespace global

open SysExt


type sortableBySwitchTrackerStandard = private { useRoll: booleanRoll }

module SortableBySwitchTrackerStandard =
    let create (switchCount: switchCount) (sortableCount: sortableCount) =

        let arrayLength = switchCount |> SwitchCount.value |> ArrayLength.createNr

        let arrayCount = sortableCount |> SortableCount.value |> ArrayCount.createNr

        let booleanRoll = BooleanRoll.create arrayCount arrayLength
        { sortableBySwitchTrackerStandard.useRoll = booleanRoll }


    let getUseRoll (sortableBySwitchTrackerStandard: sortableBySwitchTrackerStandard) =
        sortableBySwitchTrackerStandard.useRoll


    let getData (sortableBySwitchTrackerStandard: sortableBySwitchTrackerStandard) =
        sortableBySwitchTrackerStandard.useRoll |> BooleanRoll.getData


    let getArrayLength (sortableBySwitchTrackerStandard: sortableBySwitchTrackerStandard) =
        sortableBySwitchTrackerStandard.useRoll |> BooleanRoll.getArrayLength


    let getArrayCount (sortableBySwitchTrackerStandard: sortableBySwitchTrackerStandard) =
        sortableBySwitchTrackerStandard.useRoll |> BooleanRoll.getArrayCount


    let getSortableBySwitchArrays (sortableBySwitchTrackerStandard: sortableBySwitchTrackerStandard) =
        let arrayLength = sortableBySwitchTrackerStandard |> getArrayLength

        sortableBySwitchTrackerStandard.useRoll
        |> BooleanRoll.getData
        |> Seq.chunkBySize (arrayLength |> ArrayLength.value)


    let getSortableCount (sortableBySwitchTrackerStandard: sortableBySwitchTrackerStandard) =
        sortableBySwitchTrackerStandard.useRoll
        |> BooleanRoll.getArrayCount
        |> ArrayCount.value
        |> SortableCount.create


    let getSwitchCount (sortableBySwitchTrackerStandard: sortableBySwitchTrackerStandard) =
        sortableBySwitchTrackerStandard.useRoll
        |> BooleanRoll.getArrayLength
        |> ArrayLength.value
        |> SwitchCount.create


    let getSwitchUseCounts (sortableBySwitchTrackerStandard: sortableBySwitchTrackerStandard) =
        let switchCt =
            sortableBySwitchTrackerStandard |> getSwitchCount |> SwitchCount.value

        let useFlags = sortableBySwitchTrackerStandard |> getData
        let switchUseCts = Array.zeroCreate<int> switchCt

        let mutable flagDex = 0

        while (flagDex < (useFlags.Length)) do
            if useFlags.[flagDex] then
                switchUseCts.[flagDex % switchCt] <- switchUseCts.[flagDex % switchCt] + 1

            flagDex <- flagDex + 1

        switchUseCts



type sortableBySwitchTrackerBitStriped = private { useRoll: bs64Roll }

module SortableBySwitchTrackerBitStriped =

    let create (switchCount: switchCount) (sortableCount: sortableCount) =

        let arrayCount = sortableCount |> SortableCount.value |> ArrayCount.createNr

        let arrayLength = switchCount |> SwitchCount.value |> ArrayLength.createNr

        let useRoll = Bs64Roll.createEmptyStripedSet arrayLength arrayCount

        { useRoll = useRoll }


    let getArrayLength (sbstbs: sortableBySwitchTrackerBitStriped) =
        sbstbs.useRoll |> Bs64Roll.getArrayLength

    let getData (sbstbs: sortableBySwitchTrackerBitStriped) = sbstbs.useRoll |> Bs64Roll.getData

    let getSortableBySwitchArrays (sbstbs: sortableBySwitchTrackerBitStriped) = sbstbs.useRoll |> Bs64Roll.toBoolArrays

    let getSortableCount (sbstbs: sortableBySwitchTrackerBitStriped) =
        sbstbs.useRoll
        |> Bs64Roll.getArrayCount
        |> ArrayCount.value
        |> SortableCount.create

    let getSwitchCount (sbstbs: sortableBySwitchTrackerBitStriped) =
        sbstbs |> getArrayLength |> ArrayLength.value |> SwitchCount.create


    let getSwitchUseCounts (sbstbs: sortableBySwitchTrackerBitStriped) =
        let switchCt = sbstbs |> getSwitchCount |> SwitchCount.value

        let switchUseCts = Array.zeroCreate<int> switchCt

        let _procChunk (useFlags: bool[]) =
            let mutable flagDex = 0

            while (flagDex < useFlags.Length) do
                if useFlags.[flagDex] then
                    switchUseCts.[flagDex] <- switchUseCts.[flagDex] + 1

                flagDex <- flagDex + 1

        let ignoreMe = sbstbs |> getSortableBySwitchArrays |> Seq.iter _procChunk

        switchUseCts



type sortableBySwitchTracker =
    | ArrayRoll of sortableBySwitchTrackerStandard
    | BitStriped of sortableBySwitchTrackerBitStriped


module SortableBySwitchTracker =

    let getSortableBySwitchArrays (sortableBySwitchTracker: sortableBySwitchTracker) =
        match sortableBySwitchTracker with
        | ArrayRoll switchBySortableTrackerStandard ->
            switchBySortableTrackerStandard
            |> SortableBySwitchTrackerStandard.getSortableBySwitchArrays
        | BitStriped switchBySortableTrackerBitStriped ->
            switchBySortableTrackerBitStriped
            |> SortableBySwitchTrackerBitStriped.getSortableBySwitchArrays


    let getSortableCount (sortableBySwitchTracker: sortableBySwitchTracker) =
        match sortableBySwitchTracker with
        | ArrayRoll switchBySortableTrackerStandard ->
            switchBySortableTrackerStandard
            |> SortableBySwitchTrackerStandard.getSortableCount
        | BitStriped switchBySortableTrackerBitStriped ->
            switchBySortableTrackerBitStriped
            |> SortableBySwitchTrackerBitStriped.getSortableCount


    let getSwitchCount (sortableBySwitchTracker: sortableBySwitchTracker) =
        match sortableBySwitchTracker with
        | ArrayRoll switchBySortableTrackerStandard ->
            switchBySortableTrackerStandard
            |> SortableBySwitchTrackerStandard.getSwitchCount
        | BitStriped switchBySortableTrackerBitStriped ->
            switchBySortableTrackerBitStriped
            |> SortableBySwitchTrackerBitStriped.getSwitchCount


    let getSwitchUseCounts (sortableBySwitchTracker: sortableBySwitchTracker) =
        match sortableBySwitchTracker with
        | ArrayRoll switchBySortableTrackerStandard ->
            switchBySortableTrackerStandard
            |> SortableBySwitchTrackerStandard.getSwitchUseCounts
            |> SwitchUseCounts.make
        | BitStriped switchBySortableTrackerBitStriped ->
            switchBySortableTrackerBitStriped
            |> SortableBySwitchTrackerBitStriped.getSwitchUseCounts
            |> SwitchUseCounts.make
