namespace global

type sortableSet = {id:sortableSetId; rollout:rollout}
 
module SortableSet =

    let makeAllBits (ord:order) (format:byteWidth) = 
        match (ByteWidth.value format) with
        | 1 -> 
            result {
                let! sortables = IntSet8.allForAsSeq ord
                                |> Seq.map(IntSet8.getValues)
                                |> Seq.concat
                                |> Seq.toArray
                                |> ByteArray.convertUint8sToBytes
                let! rollWdth = (ByteWidth.create 1)
                let! rollLen = (RollLength.create (Order.value ord))
                let! rollCt = (RollCount.create (1 <<< (Order.value ord)))
                let! rollout = sortables |> Rollout.init rollWdth rollLen rollCt
                let ssId = SortableSetId.create (GuidUtils.guidFromBytes sortables)
                return {sortableSet.id = ssId; rollout = rollout }
            }
        | 2 -> 
            result {
                let! sortables = IntSet16.allForAsSeq ord
                                 |> Seq.map(IntSet16.getValues)
                                 |> Seq.concat
                                 |> Seq.toArray
                                 |> ByteArray.convertUint16sToBytes
                let! rollWdth = (ByteWidth.create 2)
                let! rollLen = (RollLength.create (Order.value ord))
                let! rollCt = (RollCount.create (1 <<< (Order.value ord)))
                let! rollout = sortables |> Rollout.init rollWdth rollLen rollCt
                let ssId = SortableSetId.create (GuidUtils.guidFromBytes sortables)
                return {sortableSet.id = ssId; rollout = rollout }
            }
        | 8 -> 
            result {
                let stripeAs = IntSet8.allForAsSeq ord
                                |> Seq.map(IntSet8.getValues)
                                |> ByteUtils.toStripeArrays 1uy ord
                                |> Seq.toArray

                let! sortables = stripeAs
                                |> Array.concat
                                |> ByteArray.convertUint64sToBytes

                let! rollWdth = (ByteWidth.create 8)
                let! rollLen = (RollLength.create (Order.value ord))
                let! rollCt = (RollCount.create stripeAs.Length)
                let! rollout = sortables |> Rollout.init rollWdth rollLen rollCt
                let ssId = SortableSetId.create (GuidUtils.guidFromBytes sortables)
                return {sortableSet.id = ssId; rollout = rollout }
            }
        | _ -> "invalid format in makeAllBits" |> Error



    let makeOrbits (ord:order) (format:byteWidth) (perm:permutation) = 
        let orbit = Permutation.powers perm |> Seq.toArray
        match (ByteWidth.value format) with
        | 1 -> 
            result {
                let! norbi = orbit |> Array.map(Permutation.getArray)
                                   |> Array.concat
                                   |> Array.map(uint8)
                                   |> ByteArray.convertUint8sToBytes
                let! rollLen = (RollLength.create (Order.value ord))
                let! rollCt = (RollCount.create orbit.Length)
                let! rollout = norbi |> Rollout.init format rollLen rollCt
                let ssId = SortableSetId.create (GuidUtils.guidFromBytes norbi)
                return {sortableSet.id = ssId; rollout = rollout }
            }
        | 2 -> 
            result {
                let! norbi = orbit |> Array.map(Permutation.getArray)
                                   |> Array.concat
                                   |> Array.map(uint16)
                                   |> ByteArray.convertUint16sToBytes
                let! rollLen = (RollLength.create (Order.value ord))
                let! rollCt = (RollCount.create orbit.Length)
                let! rollout = norbi |> Rollout.init format rollLen rollCt
                let ssId = SortableSetId.create (GuidUtils.guidFromBytes norbi)
                return {sortableSet.id = ssId; rollout = rollout }
            }
        | 8 -> 
            result {
                let! norb = orbit |> Array.map(Permutation.getArray)
                                  |> ByteUtils.toDistinctUint64s
                                  |> Seq.toArray
                                  |> ByteUtils.uint64ArraytoBitStriped ord

                let! striped = norb |> ByteUtils.uint64ArraytoBitStriped ord
                let! sortables = striped |> ByteArray.convertUint64sToBytes

                let! rollLen = (RollLength.create (Order.value ord))
                let! rollCt = orbit.Length |> CollectionProps.cratesFor 64 |> RollCount.create
                let! rollout =  sortables |> Rollout.init format rollLen rollCt
                let ssId = SortableSetId.create (GuidUtils.guidFromBytes sortables)
                return {sortableSet.id = ssId; rollout = rollout }
            }
        | _ -> "invalid format in makeAllBits" |> Error


    let makeSortedStacks (ord:order) (format:byteWidth) (degStack:order[]) = 
        let stackTot = Order.add degStack
        if stackTot <> ord then
            "order list is incorrect" |> Error
        else
            match (ByteWidth.value format) with
            | 1 -> 
                result {
                    let stacked = CollectionOps.stackSortedBlocks degStack 1uy 0uy
                                    |> Seq.toArray
                    let! rBytes = stacked |> Array.concat |> ByteArray.convertUint8sToBytes
                    let! rollLen = (RollLength.create (Order.value ord))
                    let! rollCt = RollCount.create stacked.Length
                    let! rollout = rBytes |> Rollout.init format rollLen rollCt
                    let ssId = SortableSetId.create (GuidUtils.guidFromBytes rBytes)
                    return {sortableSet.id = ssId; rollout = rollout }
                }
            | 2 -> 
                result {
                    let stacked = CollectionOps.stackSortedBlocks degStack 1us 0us
                                    |> Seq.toArray
                    let! rBytes = stacked |> Array.concat |> ByteArray.convertUint16sToBytes
                    let! rollLen = (RollLength.create (Order.value ord))
                    let! rollCt = RollCount.create stacked.Length
                    let! rollout = rBytes |> Rollout.init format rollLen rollCt
                    let ssId = SortableSetId.create (GuidUtils.guidFromBytes rBytes)
                    return {sortableSet.id = ssId; rollout = rollout }
                }
            | 8 -> 
                result {
                    let stacked = CollectionOps.stackSortedBlocks degStack 1uy 0uy
                                    |> Seq.toArray
                    let stripedAs = stacked |> ByteUtils.toStripeArrays 1uy ord
                                            |> Seq.toArray
                    let! rBytes = stripedAs |> Array.concat |> ByteArray.convertUint64sToBytes
                    let! rollLen = (RollLength.create (Order.value ord))
                    let! rollCt = stacked.Length |> CollectionProps.cratesFor 64 |> RollCount.create
                    let! rollout = rBytes |> Rollout.init format rollLen rollCt
                    let ssId = SortableSetId.create (GuidUtils.guidFromBytes rBytes)
                    return {sortableSet.id = ssId; rollout = rollout }
                }
            | _ -> "invalid format in makeAllBits" |> Error