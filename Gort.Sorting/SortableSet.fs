namespace global

type sortableSetO = {id:sortableSetId; rollout:rolloutO}
 
module SortableSetO =

    let toSortableIntArrays (sortableSet:sortableSetO) =
        //let byteW = sortableSet.rollout |> Rollout.byteLength |> Byt

        None
        
    let makeAllBits (order:order) (byteWidth:byteWidth) = 
        match (ByteWidth.value byteWidth) with
        | 1 -> 
            result {
                let! byteArray = IntSet8.allBitsAsSeq order
                                |> Seq.map(IntSet8.getValues)
                                |> Seq.concat
                                |> Seq.toArray
                                |> ByteArray.convertUint8sToBytes

                let! chunkCt = (SymbolCount.create (1 <<< (Order.value order)))
                let! rollout = byteArray |> RolloutO.init byteWidth chunkCt order
                let ssId = SortableSetId.create (GuidUtils.hashBytes byteArray)
                return {sortableSetO.id = ssId; rollout = rollout }
            }
        | 2 -> 
            result {
                let! byteArray = IntSet16.allBitsAsSeq order
                                 |> Seq.map(IntSet16.getValues)
                                 |> Seq.concat
                                 |> Seq.toArray
                                 |> ByteArray.convertUint16sToBytes

                let! chunkCt = (SymbolCount.create (1 <<< (Order.value order)))
                let! rollout = byteArray |> RolloutO.init byteWidth chunkCt order
                let ssId = SortableSetId.create (GuidUtils.hashBytes byteArray)
                return {sortableSetO.id = ssId; rollout = rollout }
            }
        | 8 -> 
            result {
                let stripeAs = IntSet8.allBitsAsSeq order
                                |> Seq.map(IntSet8.getValues)
                                |> ByteUtils.toStripeArrays 1uy order
                                |> Seq.toArray

                let! byteArray = stripeAs
                                |> Array.concat
                                |> ByteArray.convertUint64sToBytes

                let! rollLen = (SymbolCount.create stripeAs.Length)
                let! rollout = byteArray |> RolloutO.init byteWidth rollLen order
                let ssId = SortableSetId.create (GuidUtils.hashBytes byteArray)
                return {sortableSetO.id = ssId; rollout = rollout }
            }
        | _ -> "invalid format in makeAllBits" |> Error


    let makeOrbits (maxCount:sortableCount option) (byteWidth:byteWidth) 
                   (perm:permutation)  = 
        let intOpt = maxCount |> Option.map SortableCount.value
        let permA = Permutation.powers intOpt perm |> Seq.toArray
        let order = perm |> Permutation.getOrder
        match (ByteWidth.value byteWidth) with
        | 1 -> 
            result {
                let! norbi = permA |> Array.map(Permutation.getArray)
                                   |> Array.concat
                                   |> Array.map(uint8)
                                   |> ByteArray.convertUint8sToBytes
                let! rollCt = (SymbolCount.create permA.Length)
                let! rollout = norbi |> RolloutO.init byteWidth rollCt order
                let ssId = SortableSetId.create (GuidUtils.hashBytes norbi)
                return {sortableSetO.id = ssId; rollout = rollout }
            }
        | 2 -> 
            result {
                let! norbi = permA |> Array.map(Permutation.getArray)
                                   |> Array.concat
                                   |> Array.map(uint16)
                                   |> ByteArray.convertUint16sToBytes
                let! rollCt = (SymbolCount.create permA.Length)
                let! rollout = norbi |> RolloutO.init byteWidth rollCt order
                let ssId = SortableSetId.create (GuidUtils.hashBytes norbi)
                return {sortableSetO.id = ssId; rollout = rollout }
            }
        | 8 -> 
            result {
                let! norb = permA |> Array.map(Permutation.getArray)
                                  |> ByteUtils.toDistinctUint64s
                                  |> Seq.toArray
                                  |> ByteUtils.uint64ArraytoBitStriped order

                let! striped = norb |> ByteUtils.uint64ArraytoBitStriped order
                let! sortables = striped |> ByteArray.convertUint64sToBytes
                let! rollCt = permA.Length |> CollectionProps.cratesFor 64 |> SymbolCount.create
                let! rollout =  sortables |> RolloutO.init byteWidth rollCt order
                let ssId = SortableSetId.create (GuidUtils.hashBytes sortables)
                return {sortableSetO.id = ssId; rollout = rollout }
            }
        | _ -> "invalid format in makeOrbits" |> Error


    let makeSortedStacks (byteWidth:byteWidth) (degStack:order[]) = 
        let order = Order.add degStack
        match (ByteWidth.value byteWidth) with
        | 1 -> 
            result {
                let stacked = CollectionOps.stackSortedBlocks degStack 1uy 0uy
                                |> Seq.toArray
                let! rBytes = stacked |> Array.concat |> ByteArray.convertUint8sToBytes
                let! chunkCt = SymbolCount.create stacked.Length
                let! rollout = rBytes |> RolloutO.init byteWidth chunkCt order
                let ssId = SortableSetId.create (GuidUtils.hashBytes rBytes)
                return {sortableSetO.id = ssId; rollout = rollout }
            }
        | 2 -> 
            result {
                let stacked = CollectionOps.stackSortedBlocks degStack 1us 0us
                                |> Seq.toArray
                let! rBytes = stacked |> Array.concat |> ByteArray.convertUint16sToBytes
                let! chunkCt = SymbolCount.create stacked.Length
                let! rollout = rBytes |> RolloutO.init byteWidth chunkCt order
                let ssId = SortableSetId.create (GuidUtils.hashBytes rBytes)
                return {sortableSetO.id = ssId; rollout = rollout }
            }
        | 8 -> 
            result {
                let stacked = CollectionOps.stackSortedBlocks degStack 1uy 0uy
                                |> Seq.toArray
                let stripedAs = stacked |> ByteUtils.toStripeArrays 1uy order
                                        |> Seq.toArray
                let! rBytes = stripedAs |> Array.concat |> ByteArray.convertUint64sToBytes
                let! chunkCt = stacked.Length |> CollectionProps.cratesFor 64 |> SymbolCount.create
                let! rollout = rBytes |> RolloutO.init byteWidth chunkCt order
                let ssId = SortableSetId.create (GuidUtils.hashBytes rBytes)
                return {sortableSetO.id = ssId; rollout = rollout }
            }
        | _ -> "invalid format in makeSortedStacks" |> Error


    let makeRandom (order:order) 
                   (byteWidth:byteWidth)
                   (rando:IRando)
                   (sortableCount:sortableCount) =
        
        let sortableCt = sortableCount |> SortableCount.value
        let randPerms = Permutation.createRandoms order rando
                        |> Seq.take sortableCt
                        |> Seq.map(Permutation.getArray)
                        
        match (ByteWidth.value byteWidth) with
        | 1 -> 
            result {
                let! rBytes = randPerms |> Seq.concat
                                        |> Seq.map(uint8)
                                        |> Seq.toArray
                                        |> ByteArray.convertUint8sToBytes
                let! chunkCount = SymbolCount.create sortableCt
                let! rollout = rBytes |> RolloutO.init byteWidth chunkCount order
                let ssId = SortableSetId.create (GuidUtils.hashBytes rBytes)
                return {sortableSetO.id = ssId; rollout = rollout }
            }
        | 2 -> 
            result {
                let! rBytes = randPerms |> Seq.concat
                                        |> Seq.map(uint16)
                                        |> Seq.toArray
                                        |> ByteArray.convertUint16sToBytes
                let! chunkCount = SymbolCount.create sortableCt
                let! rollout = rBytes |> RolloutO.init byteWidth chunkCount order
                let ssId = SortableSetId.create (GuidUtils.hashBytes rBytes)
                return {sortableSetO.id = ssId; rollout = rollout }
            }
        | 8 -> 
            result {
                let! rBytes = randPerms |> ByteUtils.toDistinctUint64s
                                        |> Seq.toArray
                                        |> ByteUtils.uint64ArraytoBitStriped order

                let! striped = rBytes |> ByteUtils.uint64ArraytoBitStriped order
                let! sortables = striped |> ByteArray.convertUint64sToBytes
                let! chunkCount = rBytes.Length |> CollectionProps.cratesFor 64 |> SymbolCount.create
                let! rollout =  sortables |> RolloutO.init byteWidth chunkCount order
                let ssId = SortableSetId.create (GuidUtils.hashBytes sortables)
                return {sortableSetO.id = ssId; rollout = rollout }
            }
        | _ -> "invalid format in makeOrbits" |> Error

type sortableSetFormat =
     | SsfArrayRoll of rolloutFormat
     | SsfBitStriped

module SortableSetFormat =
    let makeBitStriped () =
        sortableSetFormat.SsfBitStriped

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
                                |> Order.create
        | BitStriped (u64, sc)  -> u64 |> Uint64Roll.getArrayLength
                                       |> ArrayLength.value
                                       |> Order.create

    let makeArrayRoll (symbolSetSize: symbolSetSize) (rollout:rollout) =
        (rollout, symbolSetSize) |> sortableSet.ArrayRoll
        
    let makeBitStriped (sortableCount: sortableCount) (uint64Roll:uint64Roll) =
        (uint64Roll, sortableCount) |> sortableSet.BitStriped

    let toSortableIntsArrays (sortableSet:sortableSet) =
        let symbolSetSize = getSymbolSetSize sortableSet
        match sortableSet with
        | ArrayRoll (r, c) -> r |> Rollout.toIntArraySeq
                                |> Seq.map(fun ia -> SortableInts.make symbolSetSize ia)
        | BitStriped (r64, sc) -> r64 |> Uint64Roll.asBitStripedToIntArraySeq
                                      |> Seq.map(fun ia -> SortableInts.make symbolSetSize ia)


    let fromSortableIntsArrays (sortableSetFormat:sortableSetFormat)
                               (order:order)
                               (symbolSetSize:symbolSetSize)
                               (sortableCount:sortableCount)
                               (sortableIntsSeq:seq<sortableInts>) =
        let arrayLength = order |> Order.value |> ArrayLength.createNr
        let arrayCount = sortableCount |> SortableCount.value |> ArrayCount.createNr
        let intAseq = sortableIntsSeq |> Seq.map(fun sints -> sints.values)
        match sortableSetFormat with
        | SsfArrayRoll rollfmt ->  
            intAseq |> Rollout.fromIntArraySeq rollfmt arrayLength arrayCount
                    |> Result.map(makeArrayRoll symbolSetSize)

        | SsfBitStriped -> 
            intAseq |> Uint64Roll.saveIntArraysAsBitStriped arrayLength arrayCount
                    |> Result.map(makeBitStriped sortableCount)


    let makeAllBits (sortableSetFormat:sortableSetFormat)
                    (order:order) = 
        match sortableSetFormat with
        | SsfArrayRoll rollfmt  -> 
                match rollfmt with
                | RfU8 -> None
                | RfU16 -> None
                | RfI32 -> None
                | RfU64 -> None
        | SsfBitStriped -> None


    let makeOrbits (sortableSetFormat:sortableSetFormat)
                   (maxCount:sortableCount option) 
                   (perm:permutation) = 
        let intOpt = maxCount |> Option.map SortableCount.value
        let permA = Permutation.powers intOpt perm |> Seq.toArray
        let order = perm |> Permutation.getOrder
        match sortableSetFormat with
        | SsfArrayRoll rollfmt  -> 
                match rollfmt with
                | RfU8 -> None
                | RfU16 -> None
                | RfI32 -> None
                | RfU64 -> None
        | SsfBitStriped -> None


    let makeSortedStacks (sortableSetFormat:sortableSetFormat)
                         (degStack:order[]) = 
        let order = Order.add degStack
        let stacked = CollectionOps.stackSortedBlocks degStack 1uy 0uy
                       |> Seq.toArray
        match sortableSetFormat with
        | SsfArrayRoll rollfmt  -> 
                match rollfmt with
                | RfU8 -> None
                | RfU16 -> None
                | RfI32 -> None
                | RfU64 -> None
        | SsfBitStriped -> None


    let makeRandom (sortableSetFormat:sortableSetFormat)
                   (order:order)
                   (rando:IRando)
                   (sortableCount:sortableCount) =
        
        let sortableCt = sortableCount |> SortableCount.value
        let randPerms = Permutation.createRandoms order rando
                        |> Seq.take sortableCt
                        |> Seq.map(Permutation.getArray)
                        
        match sortableSetFormat with
        | SsfArrayRoll rollfmt  -> 
                match rollfmt with
                | RfU8 -> None
                | RfU16 -> None
                | RfI32 -> None
                | RfU64 -> None
        | SsfBitStriped -> None