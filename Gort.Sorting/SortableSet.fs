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
    | BitStriped of sortableSetId * bs64Roll * sortableCount

 
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

        | BitStriped (_, bs64Roll, _) -> 
                bs64Roll |> Bs64Roll.getArrayLength
                         |> ArrayLength.value
                         |> Order.createNr

    let makeArrayRoll (sortableSetRId:sortableSetId)
                      (symbolSetSize: symbolSetSize) 
                      (rollout:rollout) =
        (sortableSetRId, rollout, symbolSetSize) |> sortableSet.ArrayRoll


    let makeBitStriped (sortableSetId:sortableSetId)
                       (bs64Roll:bs64Roll) =
        let sortableCount = bs64Roll 
                            |> Bs64Roll.getUsedStripes
                            |> SortableCount.create
        (sortableSetId, bs64Roll, sortableCount) 
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
                    |> Seq.map(fun sivs -> sivs.values)
                    |> Rollout.fromBoolArrays rollfmt arrayLength
                    |> Result.map(makeArrayRoll sortableSetId symbolSetSize)
        | SsfBitStriped ->
            sortableBoolSeq 
                    |> Seq.map(fun sbs -> sbs.values)
                    |> Bs64Roll.fromBoolArrays arrayLength
                    |> Result.map(makeBitStriped sortableSetId)


    let toSortableBoolSets (sortableSet:sortableSet) =
        let order = sortableSet |> getOrder
        match sortableSet with
        | ArrayRoll (_, r, _) -> 
                failwith "not implemented"
        | BitStriped (_, bs64Roll, _) -> 
                bs64Roll 
                |> Bs64Roll.toBoolArrays
                |> Seq.map(fun ia -> SortableBools.make order ia)


    let toSortableIntsArrays (sortableSet:sortableSet) =
        let order = sortableSet |> getOrder
        let symbolSetSize = getSymbolSetSize sortableSet
        match sortableSet with
        | ArrayRoll (_, r, _) -> 
                r |> Rollout.toIntArrays
                  |> Seq.map(fun ia -> SortableInts.make order symbolSetSize ia)
        | BitStriped (_, bs64Roll, _) -> 
                bs64Roll 
                |> Bs64Roll.toIntArrays
                |> Seq.map(fun ia -> SortableInts.make order symbolSetSize ia)


    let fromSortableIntArrays (sortableSetId:sortableSetId)
                              (sortableSetFormat:sortableSetFormat)
                              (order:order)
                              (symbolSetSize:symbolSetSize)
                              (sortableInts:seq<sortableInts>) =
        let arrayLength = order |> Order.value |> ArrayLength.createNr
        match sortableSetFormat with
        | SsfArrayRoll rollfmt ->  
            sortableInts
                    |> Seq.map(fun sints -> sints.values)
                    |> Rollout.fromIntArrays rollfmt arrayLength
                    |> Result.map(makeArrayRoll sortableSetId symbolSetSize)
        | SsfBitStriped -> 
            sortableInts
                    |> SortableBools.expandToSortableBits
                    |> Seq.map(fun sints -> sints.values)
                    |> Bs64Roll.fromBoolArrays arrayLength
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
                                  |> Bs64Roll.fromBoolArrays arrayLen

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
        | BitStriped (_, bs64roll, sc) -> 
                bs64roll |> Bs64Roll.toBitPack


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
        fromSortableIntArrays sortableSetId 
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
        fromSortableIntArrays sortableSetId 
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
        let sortableBools = SortableBools.makeRandomBits order pctTrue rando
                            |> Seq.take (sortableCount |> SortableCount.value)
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
                              |> Seq.map(fun _ -> SortableInts.makeRandomSymbol order symbolSetSize rando)
        fromSortableIntArrays sortableSetId
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
        fromSortableIntArrays sortableSetId 
                               sortableSetFormat 
                               order 
                               symbolSetSize 
                               sortableInts
