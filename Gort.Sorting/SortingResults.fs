namespace global
open SysExt


type switchingLogArrayRoll = {
            switchCount:switchCount; 
            sortableCount:sortableCount; 
            useRoll:bool[]}


type switchingLogBitStriped = {
        switchCount:switchCount;
        sortableCount:sortableCount;
        sortableBlockCount:int;
        useRoll:uint64Roll }


type switchingLog =
     | ArrayRoll of switchingLogArrayRoll
     | BitStriped of switchingLogBitStriped


module SwitchingLogArrayRoll =

    let create (switchCount:switchCount) 
               (sortableCount:sortableCount) 
               (useRoll:bool[]) =

        let useRollLength = ((SwitchCount.value switchCount) * 
                             (SortableCount.value sortableCount)) 
        if (useRoll.Length <> useRollLength) then 
                failwith (sprintf "useRollLength %d is not correct" useRollLength)
        {   
            switchingLogArrayRoll.switchCount = switchCount;
            sortableCount = sortableCount;
            useRoll = useRoll 
        }

    let toSwitchUses (switchEvents:switchingLogArrayRoll) =
        let swCt = (SwitchCount.value switchEvents.switchCount)
        let useWeights = Array.zeroCreate swCt
        let upDateSwU dex v =
            let swUdex = dex % swCt
            if v then
                useWeights.[swUdex] <- useWeights.[swUdex] + 1

        let switchUseCounts = switchEvents.useRoll
        switchUseCounts |> Array.iteri(fun dex v -> upDateSwU dex v)
        useWeights |> SwitchUses.make



module SwitchingLogBitStriped =

    let create (switchCount:switchCount) 
               (sortableCount:sortableCount) = 

        let arrayCount = sortableCount 
                          |> SortableCount.value
                          |> ArrayCount.createNr
        let sortableBlockCount = arrayCount 
                                 |> Uint64Roll.stripeBlocksNeededForArrayCount
        let sortableBlockLength = switchCount
                                  |> SwitchCount.value
                                  |> ArrayLength.createNr

        let useRoll = Uint64Roll.createEmptyStripedSet 
                            sortableBlockLength
                            arrayCount

        {   switchCount = switchCount;
            sortableCount = sortableCount;
            sortableBlockCount = sortableBlockCount;
            useRoll = useRoll 
        }

    let toSwitchUses (switchEventRolloutBp64:switchingLogBitStriped) =
        let switchCt = (SwitchCount.value switchEventRolloutBp64.switchCount)
        let useFlags = switchEventRolloutBp64.useRoll 
                        |> Uint64Roll.getData
                        |> Array.map(fun l -> l.count |> int)
                        |> CollectionOps.wrapAndSumCols switchCt

        useFlags |> SwitchUses.make



type sortingResults = 
        | NoGrouping of sortableSet * switchingLog
        | BySwitch of sortableSet * switchUses



module SortingResults =
    let getSortableSet(sortingResults:sortingResults) = 
        match sortingResults with
        | NoGrouping (sortableSet, _) -> sortableSet
        | BySwitch (sortableSet, _ ) -> sortableSet


