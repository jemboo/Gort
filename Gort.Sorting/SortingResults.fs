namespace global
open SysExt


type switchingLogArrayRoll = private { useRoll:booleanRoll }
module SwitchingLogArrayRoll = 
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
        { switchingLogArrayRoll.useRoll = booleanRoll } 

    let getUseRoll (switchingLogArrayRoll:switchingLogArrayRoll) =
        switchingLogArrayRoll.useRoll

    let getData (switchingLogArrayRoll:switchingLogArrayRoll) =
        switchingLogArrayRoll.useRoll
        |> BooleanRoll.getData

    let getSortableCount (switchingLogArrayRoll:switchingLogArrayRoll) =
        switchingLogArrayRoll.useRoll |> BooleanRoll.getArrayCount
                                       |> ArrayCount.value
                                       |> SortableCount.create

    let getSwitchCount (switchingLogArrayRoll:switchingLogArrayRoll) =
        switchingLogArrayRoll.useRoll |> BooleanRoll.getArrayLength
                                       |> ArrayLength.value
                                       |> SwitchCount.create

    let toSwitchUseCounts (switchingLogArrayRoll:switchingLogArrayRoll) =
        let switchCount = switchingLogArrayRoll |> getSwitchCount
                                         |> SwitchCount.value
        let useWeights = Array.zeroCreate switchCount
        let upDateSwU dex v =
            let swUdex = dex % switchCount
            if v then
                useWeights.[swUdex] <- useWeights.[swUdex] + 1

        let switchUseArrayRoll = switchingLogArrayRoll |> getData
        switchUseArrayRoll |> Array.iteri(fun dex v -> upDateSwU dex v)
        useWeights |> SwitchUseCounts.apply



type switchingLogBitStriped = private { useRoll:uint64Roll }

module SwitchingLogBitStriped =

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

    let getSortableCount (switchingLogBitStriped:switchingLogBitStriped) =
        switchingLogBitStriped.useRoll |> Uint64Roll.getArrayCount
                                       |> ArrayCount.value
                                       |> SortableCount.create

    let getSwitchCount (switchingLogBitStriped:switchingLogBitStriped) =
        switchingLogBitStriped.useRoll |> Uint64Roll.getArrayLength
                                       |> ArrayLength.value
                                       |> SwitchCount.create

    let getUseRoll (switchingLogBitStriped:switchingLogBitStriped) =
        switchingLogBitStriped.useRoll


    let getData (switchingLogBitStriped:switchingLogBitStriped) =
        switchingLogBitStriped.useRoll
        |> Uint64Roll.getData


    let toSwitchUseCounts (switchingLogBitStriped:switchingLogBitStriped) =
        let useRoll = switchingLogBitStriped 
                        |> getUseRoll
        let switchCt = useRoll
                        |> Uint64Roll.getArrayLength
                        |> ArrayLength.value
        
        let useFlags =  useRoll 
                        |> Uint64Roll.getData
                        |> Array.map(fun l -> l.count |> int)
                        |> CollectionOps.wrapAndSumCols switchCt

        useFlags |> SwitchUseCounts.apply



type switchingLog =
     | ArrayRoll of switchingLogArrayRoll
     | BitStriped of switchingLogBitStriped

module SwitchingLog = 
    let getRollingDataLog (switchingLog:switchingLog) = 
        match switchingLog with
        | ArrayRoll switchingLogArrayRoll ->
            switchingLogArrayRoll |> SwitchingLogArrayRoll.getData
        | BitStriped _ -> failwith "not implemented"

    let getSwitchUseCounts (switchingLog:switchingLog) = 
        match switchingLog with
        | ArrayRoll switchingLogArrayRoll ->
            switchingLogArrayRoll |> SwitchingLogArrayRoll.toSwitchUseCounts
        | BitStriped switchingLogBitStriped -> 
            switchingLogBitStriped |> SwitchingLogBitStriped.toSwitchUseCounts



type sorterResults = 
        | NoGrouping of sorterId * sortableSetId * rollout * switchingLog
        | BySwitch of sorterId * sortableSetId * rollout * switchUseTrack


module SorterResults =

    let getRollout (sortingResults:sorterResults) = 
        match sortingResults with
        | NoGrouping (_, _, rollout, _) -> rollout
        | BySwitch (_, _, rollout, _ ) -> rollout

    let getSorterId (sortingResults:sorterResults) = 
        match sortingResults with
        | NoGrouping (sorterId, _, _, _) -> sorterId
        | BySwitch (sorterId, _, _, _ ) -> sorterId

    let getSwitchUseCounts (sortingResults:sorterResults) = 
        match sortingResults with
        | NoGrouping (_, _, _, switchingLog) -> 
            switchingLog |> SwitchingLog.getSwitchUseCounts
        | BySwitch (_, _, _, switchUseTrack) -> 
            switchUseTrack |> SwitchUseTrack.getSwitchUseCounts


    let isSorted (sortingResults:sorterResults) = 
        sortingResults |> getRollout |> Rollout.isSorted
