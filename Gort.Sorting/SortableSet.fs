namespace global

type sortableSet = 
    private { sortableSetId:sortableSetId; 
              rollout:rollout; 
              symbolSetSize:symbolSetSize } 

module SortableSet =

    let getSymbolSetSize (sortableSet:sortableSet) = 
        sortableSet.symbolSetSize


    let getSortableCount (sortableSet:sortableSet) = 
        sortableSet.rollout
        |> Rollout.getArrayCount
        |> ArrayCount.value
        |> SortableCount.create


    let getSortableSetId (sortableSet:sortableSet) = 
        sortableSet.sortableSetId


    let getRollout (sortableSet:sortableSet) = 
        sortableSet.rollout

    let getOrder (sortableSet:sortableSet) = 
        sortableSet.rollout
        |> Rollout.getArrayLength
        |> ArrayLength.value
        |> Order.createNr


    let make (sortableSetRId:sortableSetId)
             (symbolSetSize: symbolSetSize) 
             (rollout:rollout) =
        { sortableSetId = sortableSetRId; 
          rollout = rollout; 
          symbolSetSize = symbolSetSize }


    let fromSortableBoolArrays (sortableSetId:sortableSetId)
                               (rolloutFormat:rolloutFormat)
                               (order:order)
                               (sortableBoolSeq:seq<sortableBoolArray>) =
         result {
            let! symbolSetSize = 2uL |> SymbolSetSize.create
            let! arrayLength = order |> Order.value |> ArrayLength.create
            let boolArraySeq = 
                    sortableBoolSeq
                    |> Seq.map(SortableBoolArray.getValues)
            let! rollout = 
                    boolArraySeq 
                    |> Rollout.fromBoolArrays rolloutFormat arrayLength
            return make sortableSetId symbolSetSize rollout
         }


    let toSortableBoolSets (sortableSet:sortableSet) =
        let order = sortableSet |> getOrder
        sortableSet 
            |> getRollout
            |> Rollout.toBoolArrays
            |> Seq.map(SortableBoolArray.make order)


    let toSortableIntsArrays (sortableSet:sortableSet) =
        let order = sortableSet |> getOrder
        let symbolSetSize = getSymbolSetSize sortableSet
        sortableSet 
            |> getRollout
            |> Rollout.toIntArrays
            |> Seq.map(SortableIntArray.make order symbolSetSize)


    let fromSortableIntArrays (sortableSetId:sortableSetId)
                              (rolloutFormat:rolloutFormat)
                              (order:order)
                              (symbolSetSize:symbolSetSize)
                              (sortableInts:seq<sortableIntArray>) =
        result {
            let arrayLength = order |> Order.value |> ArrayLength.createNr
            let! rollout =
                    sortableInts
                    |> Seq.map(fun sints -> sints.values)
                    |> Rollout.fromIntArrays rolloutFormat arrayLength
            return make sortableSetId symbolSetSize rollout
        }


    let fromBitPack (sortableSetId:sortableSetId)
                    (rolloutFormat:rolloutFormat)
                    (order:order)
                    (symbolSetSize:symbolSetSize)
                    (bitPk:bitPack) =
        result {
            let arrayLength = order |> Order.value |> ArrayLength.createNr
            let! rollout =
                    bitPk
                    |> Rollout.fromBitPack rolloutFormat arrayLength
            return make sortableSetId symbolSetSize rollout
        }


    let toBitPack (sortableSet:sortableSet) =
        let symbolSetSize = sortableSet |> getSymbolSetSize
        sortableSet.rollout
            |> Rollout.toBitPack symbolSetSize


    let makeAllBits (sortableSetId:sortableSetId)
                    (rolloutFormat:rolloutFormat)
                    (order:order) =
        let sortableBits = SortableBoolArray.makeAllBits order
        fromSortableBoolArrays  
                sortableSetId 
                rolloutFormat 
                order
                sortableBits


    let makeOrbits (sortableSetId:sortableSetId)
                   (rolloutFormat:rolloutFormat)
                   (maxCount:sortableCount option) 
                   (perm:permutation) = 
        let order = perm |> Permutation.getOrder
        let symbolSetSize = order |> Order.value
                            |> uint64
                            |> SymbolSetSize.createNr
        let sortableInts = SortableIntArray.makeOrbits maxCount perm
        fromSortableIntArrays 
                sortableSetId 
                rolloutFormat 
                order 
                symbolSetSize 
                sortableInts
        

    let makeSortedStacks (sortableSetId:sortableSetId)
                         (rolloutFormat:rolloutFormat)
                         (orderStack:order[]) = 
        let stackedOrder = Order.add orderStack
        let sortableBits = SortableBoolArray.makeSortedStacks orderStack
        fromSortableBoolArrays 
                sortableSetId 
                rolloutFormat 
                stackedOrder
                sortableBits


    let makeRandomPermutation (sortableSetId:sortableSetId)
                              (rolloutFormat:rolloutFormat)
                              (order:order)
                              (sortableCount:sortableCount) 
                              (rando:IRando) =
        let symbolSetSize = 
            order |> Order.value
                  |> uint64
                  |> SymbolSetSize.createNr

        let sortableInts = 
            SortableCount.makeSeq sortableCount 
                            |> Seq.map(fun _ -> 
                   SortableIntArray.makeRandomPermutation order rando)

        fromSortableIntArrays 
                sortableSetId 
                rolloutFormat 
                order 
                symbolSetSize 
                sortableInts


    let makeRandomBits (sortableSetId:sortableSetId)
                       (rolloutFormat:rolloutFormat)
                       (order:order)
                       (pctTrue:float)
                       (sortableCount:sortableCount) 
                       (rando:IRando) =
        let sortableBools = 
            SortableBoolArray.makeRandomBits order pctTrue rando
            |> Seq.take (sortableCount |> SortableCount.value)

        fromSortableBoolArrays
                sortableSetId 
                rolloutFormat 
                order
                sortableBools


    let makeRandomSymbols (sortableSetId:sortableSetId)
                          (rolloutFormat:rolloutFormat)
                          (order:order)
                          (symbolSetSize:symbolSetSize) 
                          (sortableCount:sortableCount) 
                          (rando:IRando) =
        let sortableInts = 
                SortableCount.makeSeq sortableCount 
                |> Seq.map(fun _ -> 
                    SortableIntArray.makeRandomSymbol order symbolSetSize rando)

        fromSortableIntArrays 
                sortableSetId
                rolloutFormat
                order
                symbolSetSize
                sortableInts


    let switchReduce (sortableSetId:sortableSetId)
                     (rolloutFormat:rolloutFormat)
                     (sortableSet:sortableSet)
                     (sorter:sorter) =
        let sortableInts = Seq.empty<sortableIntArray>
        let order = sortableSet |> getOrder
        let symbolSetSize = sortableSet |> getSymbolSetSize
        fromSortableIntArrays 
                sortableSetId 
                rolloutFormat 
                order 
                symbolSetSize 
                sortableInts
