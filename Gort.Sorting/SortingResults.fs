namespace global
open SysExt


type switchingLogArrayRoll = private { useRoll:booleanRoll }


type switchingLogBitStriped = private { useRoll:uint64Roll }


type switchingLog =
     | ArrayRoll of switchingLogArrayRoll
     | BitStriped of switchingLogBitStriped


module SwitchingLogArrayRoll =

    let create (switchCount:switchCount) 
               (sortableCount:sortableCount) =

        let arrayLength = (SwitchCount.value switchCount)
                          |> ArrayLength.createNr
        let arrayCount = (SortableCount.value sortableCount)
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

    let toSwitchUses (switchingLogArrayRoll:switchingLogArrayRoll) =
        let switchCount = switchingLogArrayRoll |> getSwitchCount
                                         |> SwitchCount.value
        let useWeights = Array.zeroCreate switchCount
        let upDateSwU dex v =
            let swUdex = dex % switchCount
            if v then
                useWeights.[swUdex] <- useWeights.[swUdex] + 1

        let switchUseArrayRoll = switchingLogArrayRoll 
                                 |> getData
        switchUseArrayRoll |> Array.iteri(fun dex v -> upDateSwU dex v)
        useWeights |> SwitchUses.make



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

    let toSwitchUses (switchingLogBitStriped:switchingLogBitStriped) =
        let useRoll = switchingLogBitStriped 
                        |> getUseRoll
        let switchCt = useRoll
                        |> Uint64Roll.getArrayLength
                        |> ArrayLength.value
        
        let useFlags =  useRoll 
                        |> Uint64Roll.getData
                        |> Array.map(fun l -> l.count |> int)
                        |> CollectionOps.wrapAndSumCols switchCt

        useFlags |> SwitchUses.make


module SwitchingLog = 
    let getRollingDataLog (switchingLog:switchingLog) = 
        match switchingLog with
        | ArrayRoll switchingLogArrayRoll ->
            switchingLogArrayRoll |> SwitchingLogArrayRoll.getData
        | BitStriped _ -> failwith "not implemented"


type sorterResults = 
        | NoGrouping of sorter * sorterId * sortableSet * switchingLog
        | BySwitch of sorter * sorterId * sortableSet * switchUses



module SorterResults =

    let getSortableSet(sortingResults:sorterResults) = 
        match sortingResults with
        | NoGrouping (_, _, sortableSet, _) -> sortableSet
        | BySwitch (_, _, sortableSet, _ ) -> sortableSet

    let getSorter(sortingResults:sorterResults) = 
        match sortingResults with
        | NoGrouping (sorter, _, _, _) -> sorter
        | BySwitch (sorter, _, _, _ ) -> sorter
