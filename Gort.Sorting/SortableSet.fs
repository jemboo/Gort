namespace global

type sortableSetFormat =
     | SsfArrayRoll of rolloutFormat
     | SsfBitStriped of expandBitSets


module SortableSetFormat =
    let makeBitStriped expandBitSets =
        sortableSetFormat.SsfBitStriped expandBitSets

    let makeRollout (rolloutFormat:rolloutFormat) =
        sortableSetFormat.SsfArrayRoll rolloutFormat



type sortableSet = 
    | ArrayRoll of sortableSetId * rollout * symbolSetSize
    | BitStriped of sortableSetId * uint64Roll * sortableCount

 
module SortableSet =

    let getSymbolSetSize (sortableSet:sortableSet) = 
        match sortableSet with
        | ArrayRoll (_, _, c) -> c
        | BitStriped (_, _, _)  -> 2uL |> SymbolSetSize.createNr


    let getSortableCount (sortableSet:sortableSet) = 
        match sortableSet with
        | ArrayRoll (_, r, _) -> r |> Rollout.getArrayCount
                                |> ArrayCount.value
                                |> SortableCount.create
        | BitStriped (_, _, sc)  -> sc


    let getOrder (sortableSet:sortableSet) = 
        match sortableSet with
        | ArrayRoll (_, r, _) -> r |> Rollout.getArrayLength
                                |> ArrayLength.value
                                |> Order.createNr

        | BitStriped (_, u64, _)  -> u64 |> Uint64Roll.getArrayLength
                                       |> ArrayLength.value
                                       |> Order.createNr

    let makeArrayRoll (sortableSetRId:sortableSetId)
                      (symbolSetSize: symbolSetSize) 
                      (rollout:rollout) =
        (sortableSetRId, rollout, symbolSetSize) |> sortableSet.ArrayRoll


    let makeBitStriped  (sortableSetRId:sortableSetId)
                        (uint64Roll:uint64Roll) =
        let sortableCount = uint64Roll |> Uint64Roll.getUsedStripes
                                       |> SortableCount.create
        (sortableSetRId, 
          uint64Roll, sortableCount) |> sortableSet.BitStriped


    let toSortableIntsArrays (sortableSet:sortableSet) =
        let order = sortableSet |> getOrder
        let symbolSetSize = getSymbolSetSize sortableSet
        match sortableSet with
        | ArrayRoll (_, r, c) -> r |> Rollout.toIntArraySeq
                                |> Seq.map(fun ia -> SortableInts.make order symbolSetSize ia)
        | BitStriped (_, r64, sc) -> r64 |> Uint64Roll.asBitStripedToIntArraySeq
                                      |> Seq.map(fun ia -> SortableInts.make order symbolSetSize ia)


    let fromSortableIntsArrays (sortableSetRId:sortableSetId)
                               (sortableSetFormat:sortableSetFormat)
                               (order:order)
                               (symbolSetSize:symbolSetSize)
                               (sortableIntsSeq:seq<sortableInts>) =
        let _expando e =
            if (ExpandBitSets.value e) then
                sortableIntsSeq |> SortableInts.expandToBitVersions
                                |> Seq.map(fun sints -> sints.values)
            else
                sortableIntsSeq |> Seq.map(fun sints -> sints.values)

        let arrayLength = order |> Order.value |> ArrayLength.createNr
                            
        match sortableSetFormat with
        | SsfArrayRoll rollfmt ->  
            (_expando (false |> ExpandBitSets.create)) 
                    |> Rollout.fromIntArraySeq rollfmt arrayLength
                    |> Result.map(makeArrayRoll sortableSetRId symbolSetSize)

        | SsfBitStriped expandBitsets -> 
            (_expando expandBitsets) 
                    |> Uint64Roll.saveIntArraysAsBitStriped arrayLength
                    |> Result.map(makeBitStriped sortableSetRId)


    let fromBitPack (sortableSetRId:int)
                    (sortableSetFormat:sortableSetFormat)
                    (order:order)
                    (symbolSetSize:symbolSetSize)
                    (bitPk:bitPack) =
        
        let arrayLen = order |> Order.value |> ArrayLength.createNr
        let _expando e =
            result {
                let! array2d = bitPk |> BitPack.toIntArrays arrayLen
                let sortableInts = array2d |> Array.map(SortableInts.make order symbolSetSize)

                let adjustedInts = 
                    if (ExpandBitSets.value e) then
                        sortableInts |> SortableInts.expandToBitVersions
                                     |> Seq.map(fun sints -> sints.values)
                    else
                        sortableInts |> Seq.map(fun sints -> sints.values)

                let! bitStriped = adjustedInts |> Uint64Roll.saveIntArraysAsBitStriped arrayLen
                return bitStriped |> makeBitStriped (sortableSetRId |> SortableSetId.create)
            }

        match sortableSetFormat with
        | SsfArrayRoll rollfmt -> bitPk |> Rollout.fromBitPack arrayLen
                                        |> Result.map(makeArrayRoll 
                                           (sortableSetRId |> SortableSetId.create) symbolSetSize)
        | SsfBitStriped expandBitsets -> expandBitsets |> _expando


    let toBitPack (sortableSet:sortableSet) =
        let order = sortableSet |> getOrder
        let symbolSetSize = getSymbolSetSize sortableSet
        match sortableSet with
        | ArrayRoll (_, r, c) ->  r |> Rollout.toBitPack symbolSetSize
        | BitStriped (_, r64, sc) -> r64 |> Uint64Roll.toBitPack symbolSetSize


    let makeAllBits (sortableSetId:sortableSetId)
                    (sortableSetFormat:sortableSetFormat)
                    (order:order) =
        let symbolSetSize = 2uL |> SymbolSetSize.createNr
        let sortableInts = SortableInts.makeAllBits order
        fromSortableIntsArrays   sortableSetId sortableSetFormat 
                                 order symbolSetSize sortableInts


    let makeOrbits (sortableSetId:sortableSetId)
                   (sortableSetFormat:sortableSetFormat)
                   (maxCount:sortableCount option) 
                   (perm:permutation) = 
        let order = perm |> Permutation.getOrder
        let symbolSetSize = order |> Order.value
                            |> uint64
                            |> SymbolSetSize.createNr
        let sortableInts = SortableInts.makeOrbits maxCount perm
        fromSortableIntsArrays sortableSetId sortableSetFormat order symbolSetSize sortableInts
        

    let makeSortedStacks (sortableSetId:sortableSetId)
                         (sortableSetFormat:sortableSetFormat)
                         (orderStack:order[]) = 
        let stackedOrder = Order.add orderStack
        let symbolSetSize = 2uL |> SymbolSetSize.createNr
        let sortableInts = SortableInts.makeSortedStacks orderStack
        fromSortableIntsArrays sortableSetId sortableSetFormat stackedOrder symbolSetSize sortableInts


    let makeRandomPermutation (sortableSetId:sortableSetId)
                              (sortableSetFormat:sortableSetFormat)
                              (order:order)
                              (sortableCount:sortableCount) 
                              (rando:IRando) =
        let symbolSetSize = order |> Order.value
                            |> uint64
                            |> SymbolSetSize.createNr
        let sortableInts = SortableCount.makeSeq sortableCount 
                            |> Seq.map(fun i -> SortableInts.makeRandomPermutation order rando)
        fromSortableIntsArrays sortableSetId sortableSetFormat order symbolSetSize sortableInts


    let makeRandomBits (sortableSetId:sortableSetId)
                       (sortableSetFormat:sortableSetFormat)
                       (order:order)
                       (pctOnes:float)
                       (sortableCount:sortableCount) 
                       (rando:IRando) =
        let symbolSetSize = order |> Order.value
                            |> uint64
                            |> SymbolSetSize.createNr
        let sortableInts = SortableCount.makeSeq sortableCount 
                             |> Seq.map(fun i -> SortableInts.makeRandomBits order pctOnes rando)
        fromSortableIntsArrays sortableSetId sortableSetFormat order 
                               symbolSetSize sortableInts


    let makeRandomSymbols (sortableSetId:sortableSetId)
                          (sortableSetFormat:sortableSetFormat)
                          (order:order)
                          (symbolSetSize:symbolSetSize) 
                          (sortableCount:sortableCount) 
                          (rando:IRando) =
        let sortableInts = SortableCount.makeSeq sortableCount 
                              |> Seq.map(fun i -> SortableInts.makeRandomSymbols order symbolSetSize rando)
        fromSortableIntsArrays sortableSetId sortableSetFormat order symbolSetSize sortableInts


    let switchReduce (sortableSetId:sortableSetId)
                     (sortableSetFormat:sortableSetFormat)
                     (sortableSet:sortableSet)
                     (sorter:sorter) =
        let sortableInts = Seq.empty<sortableInts>
        let order = sortableSet |> getOrder
        let symbolSetSize = sortableSet |> getSymbolSetSize
        fromSortableIntsArrays sortableSetId sortableSetFormat order symbolSetSize sortableInts
