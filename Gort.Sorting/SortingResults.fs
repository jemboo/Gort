namespace global
open System


type switchEventRolloutInt = {
            switchCount:switchCount; 
            sortableCount:sortableCount; 
            useRoll:bool[]}


type switchEventRolloutBp64 = {
        switchCount:switchCount;
        sortableCount:sortableCount;
        sortableBlockCount:int;
        useRoll:uint64[] }


type switchEventRollout =
     | Int of switchEventRolloutInt
     | Bp64 of switchEventRolloutBp64


module SwitchEventRolloutInt =

    let create (switchCount:switchCount) 
               (sortableCount:sortableCount) 
               (useRoll:bool[]) =

        let useRollLength = ((SwitchCount.value switchCount) * 
                             (SortableCount.value sortableCount)) 
        if (useRoll.Length <> useRollLength) then 
                failwith (sprintf "useRollLength %d is not correct" useRollLength)
        {   
            switchEventRolloutInt.switchCount = switchCount;
            sortableCount = sortableCount;
            useRoll = useRoll 
        }

    let toSwitchUses (switchEvents:switchEventRolloutInt) =
        let swCt = (SwitchCount.value switchEvents.switchCount)
        let useWeights = Array.zeroCreate swCt
        let upDateSwU dex v =
            let swUdex = dex % swCt
            if v then
                useWeights.[swUdex] <- useWeights.[swUdex] + 1

        let switchUseCounts = switchEvents.useRoll
        switchUseCounts |> Array.iteri(fun dex v -> upDateSwU dex v)
        useWeights |> SwitchUses.make



module SwitchEventRolloutBp64 =
    let yab = None
    //let create (switchCount:SwitchCount) 
    //           (sortableCount:SortableCount) = 

    //    let blockCount = (SortableCount.value sortableCount) |> BitsP64.pBlocksFor
    //    let ur = BitsP64.zeroCreate
    //                          ((SwitchCount.value switchCount) * 
    //                           blockCount)

    //    {   switchCount = switchCount;
    //        sortableCount = sortableCount;
    //        sortableBlockCount = blockCount;
    //        useRoll = ur 
    //    }

    //let init (sortableCount:SortableCount)
    //         (weights:int[]) =
    //    let zz = create (SwitchCount.fromInt weights.Length) 
    //                    sortableCount
    //    weights |> Array.iteri(fun i _ -> 
    //                zz.useRoll.values.[i] <- weights.[i] |> uint64)
    //    zz


    //let toSwitchUses (switchEvents:switchEventRolloutBp64) =
    //    let switchCt = (SwitchCount.value switchEvents.switchCount)
    //    let weights = switchEvents.useRoll.values
    //                   |> Array.map(fun l -> ByteUtils.trueBitCount64 l )
    //                   |> CollectionUtils.wrapAndSumCols switchCt

    //    { switchUses.weights = weights }





type sortingResults = 
        | NoGrouping of sortableSet * switchEventRollout
        | BySwitch of sortableSet * switchUses



module SortingResults =
    let getSortableSet(sortingResults:sortingResults) = 
        match sortingResults with
        | NoGrouping (sortableSet, _) -> sortableSet
        | BySwitch (sortableSet, _ ) -> sortableSet


