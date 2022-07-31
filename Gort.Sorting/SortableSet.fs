﻿namespace global

type sortableSet = {id:sortableSetId; rollout:rolloutO}
 
module SortableSet =

    let toSortableIntArrays (sortableSet:sortableSet) =
        //let byteW = sortableSet.rollout |> Rollout.byteLength |> Byt

        None
        
    let makeAllBits (order:order) (byteWidth:byteWidth) = 
        match (ByteWidth.value byteWidth) with
        | 1 -> 
            result {
                let! byteArray = IntSet8.allForAsSeq order
                                |> Seq.map(IntSet8.getValues)
                                |> Seq.concat
                                |> Seq.toArray
                                |> ByteArray.convertUint8sToBytes

                let! chunkCt = (SymbolCount.create (1 <<< (Order.value order)))
                let! rollout = byteArray |> RolloutO.init byteWidth chunkCt order
                let ssId = SortableSetId.create (GuidUtils.hashBytes byteArray)
                return {sortableSet.id = ssId; rollout = rollout }
            }
        | 2 -> 
            result {
                let! byteArray = IntSet16.allForAsSeq order
                                 |> Seq.map(IntSet16.getValues)
                                 |> Seq.concat
                                 |> Seq.toArray
                                 |> ByteArray.convertUint16sToBytes

                let! chunkCt = (SymbolCount.create (1 <<< (Order.value order)))
                let! rollout = byteArray |> RolloutO.init byteWidth chunkCt order
                let ssId = SortableSetId.create (GuidUtils.hashBytes byteArray)
                return {sortableSet.id = ssId; rollout = rollout }
            }
        | 8 -> 
            result {
                let stripeAs = IntSet8.allForAsSeq order
                                |> Seq.map(IntSet8.getValues)
                                |> ByteUtils.toStripeArrays 1uy order
                                |> Seq.toArray

                let! byteArray = stripeAs
                                |> Array.concat
                                |> ByteArray.convertUint64sToBytes

                let! rollLen = (SymbolCount.create stripeAs.Length)
                let! rollout = byteArray |> RolloutO.init byteWidth rollLen order
                let ssId = SortableSetId.create (GuidUtils.hashBytes byteArray)
                return {sortableSet.id = ssId; rollout = rollout }
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
                return {sortableSet.id = ssId; rollout = rollout }
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
                return {sortableSet.id = ssId; rollout = rollout }
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
                return {sortableSet.id = ssId; rollout = rollout }
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
                return {sortableSet.id = ssId; rollout = rollout }
            }
        | 2 -> 
            result {
                let stacked = CollectionOps.stackSortedBlocks degStack 1us 0us
                                |> Seq.toArray
                let! rBytes = stacked |> Array.concat |> ByteArray.convertUint16sToBytes
                let! chunkCt = SymbolCount.create stacked.Length
                let! rollout = rBytes |> RolloutO.init byteWidth chunkCt order
                let ssId = SortableSetId.create (GuidUtils.hashBytes rBytes)
                return {sortableSet.id = ssId; rollout = rollout }
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
                return {sortableSet.id = ssId; rollout = rollout }
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
                return {sortableSet.id = ssId; rollout = rollout }
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
                return {sortableSet.id = ssId; rollout = rollout }
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
                return {sortableSet.id = ssId; rollout = rollout }
            }
        | _ -> "invalid format in makeOrbits" |> Error