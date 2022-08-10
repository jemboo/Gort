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
    | ArrayRoll of rollout * symbolSetSize
    | BitStriped of uint64Roll * sortableCount

 
module SortableSet =

    let getSymbolSetSize (sortableSet:sortableSet) = 
        match sortableSet with
        | ArrayRoll (_, c) -> c
        | BitStriped (_, _)  -> 2uL |> SymbolSetSize.createNr


    let getSortableCount (sortableSet:sortableSet) = 
        match sortableSet with
        | ArrayRoll (r, c) -> r |> Rollout.getArrayCount
                                |> ArrayCount.value
                                |> SortableCount.create
        | BitStriped (_, sc)  -> sc


    let getOrder (sortableSet:sortableSet) = 
        match sortableSet with
        | ArrayRoll (r, c) -> r |> Rollout.getArrayLength
                                |> ArrayLength.value
                                |> Order.createNr

        | BitStriped (u64, sc)  -> u64 |> Uint64Roll.getArrayLength
                                       |> ArrayLength.value
                                       |> Order.createNr

    let makeArrayRoll (symbolSetSize: symbolSetSize) (rollout:rollout) =
        (rollout, symbolSetSize) |> sortableSet.ArrayRoll


    let makeBitStriped (uint64Roll:uint64Roll) =
        let sortableCount = uint64Roll |> Uint64Roll.getUsedStripes
                                       |> SortableCount.create
        (uint64Roll, sortableCount) |> sortableSet.BitStriped


    let toSortableIntsArrays (sortableSet:sortableSet) =
        let order = sortableSet |> getOrder
        let symbolSetSize = getSymbolSetSize sortableSet
        match sortableSet with
        | ArrayRoll (r, c) -> r |> Rollout.toIntArraySeq
                                |> Seq.map(fun ia -> SortableInts.make order symbolSetSize ia)
        | BitStriped (r64, sc) -> r64 |> Uint64Roll.asBitStripedToIntArraySeq
                                      |> Seq.map(fun ia -> SortableInts.make order symbolSetSize ia)


    let fromSortableIntsArrays (sortableSetFormat:sortableSetFormat)
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
                    |> Result.map(makeArrayRoll symbolSetSize)

        | SsfBitStriped expandBitsets -> 
            (_expando expandBitsets) 
                    |> Uint64Roll.saveIntArraysAsBitStriped arrayLength
                    |> Result.map(makeBitStriped)



    let makeAllBits (sortableSetFormat:sortableSetFormat)
                    (order:order) =
        let symbolSetSize = 2uL |> SymbolSetSize.createNr
        let sortableInts = SortableInts.makeAllBits order
        fromSortableIntsArrays sortableSetFormat order symbolSetSize sortableInts


    let makeOrbits (sortableSetFormat:sortableSetFormat)
                   (maxCount:sortableCount option) 
                   (perm:permutation) = 
        let order = perm |> Permutation.getOrder
        let symbolSetSize = order |> Order.value
                            |> uint64
                            |> SymbolSetSize.createNr
        let sortableInts = SortableInts.makeOrbits maxCount perm
        fromSortableIntsArrays sortableSetFormat order symbolSetSize sortableInts
        

    let makeSortedStacks (sortableSetFormat:sortableSetFormat)
                         (orderStack:order[]) = 
        let stackedOrder = Order.add orderStack
        let symbolSetSize = 2uL |> SymbolSetSize.createNr
        let sortableInts = SortableInts.makeSortedStacks orderStack
        fromSortableIntsArrays sortableSetFormat stackedOrder symbolSetSize sortableInts


    let makeRandomPermutation (sortableSetFormat:sortableSetFormat)
                              (order:order)
                              (sortableCount:sortableCount) 
                              (rando:IRando) =
        let symbolSetSize = order |> Order.value
                            |> uint64
                            |> SymbolSetSize.createNr
        let sortableInts = SortableCount.makeSeq sortableCount 
                            |> Seq.map(fun i -> SortableInts.makeRandomPermutation order rando)
        fromSortableIntsArrays sortableSetFormat order symbolSetSize sortableInts


    let makeRandomBits (sortableSetFormat:sortableSetFormat)
                       (order:order)
                       (pctOnes:float)
                       (sortableCount:sortableCount) 
                       (rando:IRando) =
        let symbolSetSize = order |> Order.value
                            |> uint64
                            |> SymbolSetSize.createNr
        let sortableInts = SortableCount.makeSeq sortableCount 
                             |> Seq.map(fun i -> SortableInts.makeRandomBits order pctOnes rando)
        fromSortableIntsArrays sortableSetFormat order symbolSetSize sortableInts


    let makeRandomSymbols (sortableSetFormat:sortableSetFormat)
                          (order:order)
                          (symbolSetSize:symbolSetSize) 
                          (sortableCount:sortableCount) 
                          (rando:IRando) =
        let sortableInts = SortableCount.makeSeq sortableCount 
                              |> Seq.map(fun i -> SortableInts.makeRandomSymbols order symbolSetSize rando)
        fromSortableIntsArrays sortableSetFormat order symbolSetSize sortableInts