namespace global

type sortableSetFormat =
     | SsfArrayRoll of rolloutFormat
     | SsfBitStriped


module SortableSetFormat =

    let makeBitStriped =
        sortableSetFormat.SsfBitStriped

    let makeRollout (rolloutFormat:rolloutFormat) =
        sortableSetFormat.SsfArrayRoll rolloutFormat


type sortableSet = 
    | ArrayRoll of sortableSetId * rollout * symbolSetSize
    | BitStriped of sortableSetId * uint64Roll * sortableCount

 
module SortableSet =

    let getSymbolSetSize (sortableSet:sortableSet) = 
        match sortableSet with
        | ArrayRoll (_, _, c) -> c
        | BitStriped (_, _, _)  -> 
                2uL |> SymbolSetSize.createNr


    let getSortableCount (sortableSet:sortableSet) = 
        match sortableSet with
        | ArrayRoll (_, r, _) -> 
              r |> Rollout.getArrayCount
                |> ArrayCount.value
                |> SortableCount.create
        | BitStriped (_, _, sc)  -> sc


    let getSortableSetId (sortableSet:sortableSet) = 
        match sortableSet with
        | ArrayRoll (ssid, _, _) -> ssid
        | BitStriped (ssid, _, sc)  -> ssid


    let getRollout (sortableSet:sortableSet) = 
        match sortableSet with
        | ArrayRoll (_, r, _) -> r |> Ok
        | BitStriped (_, _, _)  -> 
                sprintf "BitStriped does not have a rollout" |> Error


    let getOrder (sortableSet:sortableSet) = 
        match sortableSet with
        | ArrayRoll (_, rollout, _) ->
                rollout |> Rollout.getArrayLength
                        |> ArrayLength.value
                        |> Order.createNr

        | BitStriped (_, u64Roll, _) -> 
                u64Roll |> Uint64Roll.getArrayLength
                        |> ArrayLength.value
                        |> Order.createNr

    let makeArrayRoll (sortableSetRId:sortableSetId)
                      (symbolSetSize: symbolSetSize) 
                      (rollout:rollout) =
        (sortableSetRId, rollout, symbolSetSize) |> sortableSet.ArrayRoll


    let makeBitStriped (sortableSetId:sortableSetId)
                       (uint64Roll:uint64Roll) =
        let sortableCount = uint64Roll 
                            |> Uint64Roll.getUsedStripes
                            |> SortableCount.create
        (sortableSetId, uint64Roll, sortableCount) 
        |> sortableSet.BitStriped


    let fromSortableBoolArrays (sortableSetId:sortableSetId)
                               (sortableSetFormat:sortableSetFormat)
                               (order:order)
                               (sortableBoolSeq:seq<sortableBools>) =
        let arrayLength = order |> Order.value |> ArrayLength.createNr
        let symbolSetSize = 2uL |> SymbolSetSize.createNr
        match sortableSetFormat with
        | SsfArrayRoll rollfmt ->  
            sortableBoolSeq 
                    |> Seq.map(fun sints -> sints.values)
                    |> Rollout.fromBoolArrays rollfmt arrayLength
                    |> Result.map(makeArrayRoll sortableSetId symbolSetSize)
        | SsfBitStriped -> 
            sortableBoolSeq 
                    |> Seq.map(fun sints -> sints.values)
                    |> Uint64Roll.fromBoolArraysAsBitStriped arrayLength
                    |> Result.map(makeBitStriped sortableSetId)


    let toSortableIntsArrays (sortableSet:sortableSet) =
        let order = sortableSet |> getOrder
        let symbolSetSize = getSymbolSetSize sortableSet
        match sortableSet with
        | ArrayRoll (_, r, _) -> 
                r |> Rollout.toIntArraySeq
                  |> Seq.map(fun ia -> SortableInts.make order symbolSetSize ia)
        | BitStriped (_, r64, _) -> 
                r64 |> Uint64Roll.asBitStripedToIntArrays
                    |> Seq.map(fun ia -> SortableInts.make order symbolSetSize ia)


    let fromSortableIntsArrays (sortableSetId:sortableSetId)
                               (sortableSetFormat:sortableSetFormat)
                               (order:order)
                               (symbolSetSize:symbolSetSize)
                               (sortableIntsSeq:seq<sortableInts>) =
        let arrayLength = order |> Order.value |> ArrayLength.createNr
        match sortableSetFormat with
        | SsfArrayRoll rollfmt ->  
            sortableIntsSeq 
                    |> Seq.map(fun sints -> sints.values)
                    |> Rollout.fromIntArrays rollfmt arrayLength
                    |> Result.map(makeArrayRoll sortableSetId symbolSetSize)
        | SsfBitStriped -> 
            sortableIntsSeq 
                    |> SortableBools.expandToSortableBits
                    |> Seq.map(fun sints -> sints.values)
                    |> Uint64Roll.fromBoolArraysAsBitStriped arrayLength
                    |> Result.map(makeBitStriped sortableSetId)



    let fromBitPack (sortableSetId:sortableSetId)
                    (sortableSetFormat:sortableSetFormat)
                    (order:order)
                    (symbolSetSize:symbolSetSize)
                    (bitPk:bitPack) =
        let arrayLen = order |> Order.value |> ArrayLength.createNr
        let _expando =
            result {
                let! array2d = bitPk |> BitPack.toIntArrays arrayLen
                let sortableInts = array2d |> Array.map(SortableInts.make order symbolSetSize)

                let expandedBoolArrays = 
                        sortableInts |> SortableBools.expandToSortableBits
                                     |> Seq.map(fun sints -> sints.values)

                let! uint64roll = expandedBoolArrays 
                                  |> Uint64Roll.fromBoolArraysAsBitStriped arrayLen

                return uint64roll |> makeBitStriped sortableSetId
            }

        match sortableSetFormat with
        | SsfArrayRoll rolloutFormat -> 
                bitPk |> Rollout.fromBitPack rolloutFormat arrayLen
                      |> Result.map(makeArrayRoll sortableSetId symbolSetSize)
        | SsfBitStriped  -> _expando


    let toBitPack (sortableSet:sortableSet) =
        match sortableSet with
        | ArrayRoll (_, rollout, symbolSetSize) ->  
                rollout |> Rollout.toBitPack symbolSetSize
        | BitStriped (_, uint64roll, sc) -> 
                uint64roll |> Uint64Roll.asBitStripedtoBitPack


    let makeAllBits (sortableSetId:sortableSetId)
                    (sortableSetFormat:sortableSetFormat)
                    (order:order) =
        let sortableBits = SortableBools.makeAllBits order
        fromSortableBoolArrays  sortableSetId 
                                sortableSetFormat 
                                order
                                sortableBits


    let makeOrbits (sortableSetId:sortableSetId)
                   (sortableSetFormat:sortableSetFormat)
                   (maxCount:sortableCount option) 
                   (perm:permutation) = 
        let order = perm |> Permutation.getOrder
        let symbolSetSize = order |> Order.value
                            |> uint64
                            |> SymbolSetSize.createNr
        let sortableInts = SortableInts.makeOrbits maxCount perm
        fromSortableIntsArrays sortableSetId 
                               sortableSetFormat 
                               order 
                               symbolSetSize 
                               sortableInts
        

    let makeSortedStacks (sortableSetId:sortableSetId)
                         (sortableSetFormat:sortableSetFormat)
                         (orderStack:order[]) = 
        let stackedOrder = Order.add orderStack
        let sortableBits = SortableBools.makeSortedStacks orderStack
        fromSortableBoolArrays sortableSetId 
                               sortableSetFormat 
                               stackedOrder
                               sortableBits


    let makeRandomPermutation (sortableSetId:sortableSetId)
                              (sortableSetFormat:sortableSetFormat)
                              (order:order)
                              (sortableCount:sortableCount) 
                              (rando:IRando) =
        let symbolSetSize = order |> Order.value
                                  |> uint64
                                  |> SymbolSetSize.createNr
        let sortableInts = SortableCount.makeSeq sortableCount 
                            |> Seq.map(fun _ -> SortableInts.makeRandomPermutation order rando)
        fromSortableIntsArrays sortableSetId 
                               sortableSetFormat 
                               order 
                               symbolSetSize 
                               sortableInts


    let makeRandomBits (sortableSetId:sortableSetId)
                       (sortableSetFormat:sortableSetFormat)
                       (order:order)
                       (pctTrue:float)
                       (sortableCount:sortableCount) 
                       (rando:IRando) =
        let sortableBools = SortableCount.makeSeq sortableCount 
                             |> Seq.map(fun _ -> SortableBools.makeRandomBits order pctTrue rando)
        fromSortableBoolArrays sortableSetId 
                               sortableSetFormat 
                               order
                               sortableBools


    let makeRandomSymbols (sortableSetId:sortableSetId)
                          (sortableSetFormat:sortableSetFormat)
                          (order:order)
                          (symbolSetSize:symbolSetSize) 
                          (sortableCount:sortableCount) 
                          (rando:IRando) =
        let sortableInts = SortableCount.makeSeq sortableCount 
                              |> Seq.map(fun _ -> SortableInts.makeRandomSymbols order symbolSetSize rando)
        fromSortableIntsArrays sortableSetId
                               sortableSetFormat
                               order
                               symbolSetSize
                               sortableInts


    let switchReduce (sortableSetId:sortableSetId)
                     (sortableSetFormat:sortableSetFormat)
                     (sortableSet:sortableSet)
                     (sorter:sorter) =
        let sortableInts = Seq.empty<sortableInts>
        let order = sortableSet |> getOrder
        let symbolSetSize = sortableSet |> getSymbolSetSize
        fromSortableIntsArrays sortableSetId 
                               sortableSetFormat 
                               order 
                               symbolSetSize 
                               sortableInts
