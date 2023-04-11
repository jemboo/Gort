namespace global
open System

type sortableSet =
    private
        { sortableSetId: sortableSetId
          rollout: rollout
          symbolSetSize: symbolSetSize }

module SortableSet =

    let getSymbolSetSize (sortableSet: sortableSet) = sortableSet.symbolSetSize

    let getSortableCount (sortableSet: sortableSet) =
        sortableSet.rollout
        |> Rollout.getArrayCount
        |> ArrayCount.value
        |> SortableCount.create

    let getSortableSetId (sortableSet: sortableSet) = sortableSet.sortableSetId

    let getRollout (sortableSet: sortableSet) = sortableSet.rollout

    let getOrder (sortableSet: sortableSet) =
        sortableSet.rollout
        |> Rollout.getArrayLength
        |> ArrayLength.value
        |> Order.createNr

    let getSummaryReport (sortableSet:sortableSet)
        =
        sprintf "%s\t%d"
            (sortableSet |> getSortableSetId |> SortableSetId.value |> string)
            (sortableSet |> getRollout |> Rollout.getArrayCount |> ArrayCount.value)

    let makeTag (sortableSet:sortableSet)
        =
        sprintf "%s(%s)"
            ((sortableSet |> getRollout |> Rollout.getArrayCount |> ArrayCount.value).ToString("D4"))
            ((sortableSet |> getSortableSetId |> SortableSetId.value |> string).Substring(0,8))

    let getIdAndPermRoot 
        (sortableSet:sortableSet)
        =
        ( sortableSet |> getSortableSetId 
          ,
          sortableSet
          |> getRollout
          |> Rollout.toIntArrays
          |> Seq.head
          |> Permutation.createNr
        )


    let make
        (sortableSetRId: sortableSetId) 
        (symbolSetSize: symbolSetSize) 
        (rollout: rollout) 
        =
        { sortableSetId = sortableSetRId
          rollout = rollout
          symbolSetSize = symbolSetSize }


    let createEmpty = 
        make (Guid.Empty |> SortableSetId.create) (0UL |> SymbolSetSize.createNr) Rollout.createEmpty


    let fromSortableBoolArrays
        (sortableSetId: sortableSetId)
        (rolloutFormat: rolloutFormat)
        (order: order)
        (sortableBoolSeq: seq<sortableBoolArray>)
        =
        result {
            let! symbolSetSize = 2uL |> SymbolSetSize.create
            let! arrayLength = order |> Order.value |> ArrayLength.create
            let boolArraySeq = sortableBoolSeq |> Seq.map (SortableBoolArray.getValues)
            let! rollout = boolArraySeq |> Rollout.fromBoolArrays rolloutFormat arrayLength
            return make sortableSetId symbolSetSize rollout
        }


    let toSortableBoolArrays (sortableSet: sortableSet) =
        let order = sortableSet |> getOrder

        sortableSet
        |> getRollout
        |> Rollout.toBoolArrays
        |> Seq.map (SortableBoolArray.make order)


    let toSortableIntsArrays 
        (sortableSet: sortableSet) 
        =
        let order = sortableSet |> getOrder
        let symbolSetSize = getSymbolSetSize sortableSet
        sortableSet
        |> getRollout
        |> Rollout.toIntArrays
        |> Seq.map (SortableIntArray.make order symbolSetSize)


    let fromSortableIntArrays
        (sortableSetId: sortableSetId)
        (rolloutFormat: rolloutFormat)
        (order: order)
        (symbolSetSize: symbolSetSize)
        (sortableInts: seq<sortableIntArray>)
        =
        result {
            let arrayLength = order |> Order.value |> ArrayLength.createNr

            let! rollout =
                sortableInts
                |> Seq.map (fun sints -> sints.values)
                |> Rollout.fromIntArrays rolloutFormat arrayLength

            return make sortableSetId symbolSetSize rollout
        }


    let fromBitPack
        (sortableSetId: sortableSetId)
        (rolloutFormat: rolloutFormat)
        (order: order)
        (symbolSetSize: symbolSetSize)
        (bitPk: bitPack)
        =
        result {
            let arrayLength = order |> Order.value |> ArrayLength.createNr
            let! rollout = bitPk |> Rollout.fromBitPack rolloutFormat arrayLength
            return make sortableSetId symbolSetSize rollout
        }


    let toBitPack (sortableSet: sortableSet) =
        let symbolSetSize = sortableSet |> getSymbolSetSize
        sortableSet.rollout |> Rollout.toBitPack symbolSetSize


    let makeAllBits (sortableSetId: sortableSetId) (rolloutFormat: rolloutFormat) (order: order) =
        let sortableBits = SortableBoolArray.makeAllBits order
        fromSortableBoolArrays sortableSetId rolloutFormat order sortableBits


    let makeOrbits
        (rolloutFormat: rolloutFormat)
        (maxCount: sortableCount option)
        (perm: permutation)
        (sortableSetId: sortableSetId)
        =
        let order = perm |> Permutation.getOrder
        let symbolSetSize = order |> Order.value |> uint64 |> SymbolSetSize.createNr
        let sortableInts = SortableIntArray.makeOrbits maxCount perm
        fromSortableIntArrays sortableSetId rolloutFormat order symbolSetSize sortableInts


    let makeSortedStacks (sortableSetId: sortableSetId) (rolloutFormat: rolloutFormat) (orderStack: order[]) =
        let stackedOrder = Order.add orderStack
        let sortableBits = SortableBoolArray.makeSortedStacks orderStack
        fromSortableBoolArrays sortableSetId rolloutFormat stackedOrder sortableBits


    let makeRandomPermutation
        (rolloutFormat: rolloutFormat)
        (order: order)
        (sortableCount: sortableCount)
        (rando: IRando)
        (sortableSetId: sortableSetId)
        =
        let symbolSetSize = order |> Order.value |> uint64 |> SymbolSetSize.createNr

        let sortableInts =
            SortableCount.makeSeq sortableCount
            |> Seq.map (fun _ -> SortableIntArray.makeRandomPermutation order rando)

        fromSortableIntArrays sortableSetId rolloutFormat order symbolSetSize sortableInts


    let makeRandomBits
        (rolloutFormat: rolloutFormat)
        (order: order)
        (pctTrue: float)
        (sortableCount: sortableCount)
        (rando: IRando)
        (sortableSetId: sortableSetId)
        =
        let sortableBools =
            SortableBoolArray.makeRandomBits order pctTrue rando
            |> Seq.take (sortableCount |> SortableCount.value)

        fromSortableBoolArrays sortableSetId rolloutFormat order sortableBools


    let makeRandomSymbols
        (rolloutFormat: rolloutFormat)
        (order: order)
        (symbolSetSize: symbolSetSize)
        (sortableCount: sortableCount)
        (rando: IRando)
        (sortableSetId: sortableSetId)
        =
        let sortableInts =
            SortableCount.makeSeq sortableCount
            |> Seq.map (fun _ -> SortableIntArray.makeRandomSymbol order symbolSetSize rando)

        fromSortableIntArrays sortableSetId rolloutFormat order symbolSetSize sortableInts


    let switchReduce
        (sortableSetId: sortableSetId)
        (rolloutFormat: rolloutFormat)
        (sortableSet: sortableSet)
        (sorter: sorter)
        =
        let sortableInts = Seq.empty<sortableIntArray>
        let order = sortableSet |> getOrder
        let symbolSetSize = sortableSet |> getSymbolSetSize
        fromSortableIntArrays sortableSetId rolloutFormat order symbolSetSize sortableInts



type setOfSortableSet =
    private
        { setOfSortableSetId: setOfSortableSetId
          sortableSetMap: Map<sortableSetId, sortableSet> }


module SetOfSortableSet =

    let getSortableSetMap (setOfSortableSet: setOfSortableSet) =
        setOfSortableSet.sortableSetMap

    let getSetOfSortableSetId (setOfSortableSet: setOfSortableSet) = 
        setOfSortableSet.setOfSortableSetId

    let getSortableSets (setOfSortableSet: setOfSortableSet)
        =
        setOfSortableSet |> getSortableSetMap |> Map.values


    let getSummaryReports (setOfSortableSet: setOfSortableSet) 
        =
        setOfSortableSet |> getSortableSets |> Seq.map(SortableSet.getSummaryReport)


    let generateSortableSetIds 
        (setOfSortableSetId:setOfSortableSetId) 
        =
        RandVars.rndGuidsLcg (setOfSortableSetId |> SetOfSortableSetId.value)
        |> Seq.map(SortableSetId.create)


    let load (setOfSortableSetId:setOfSortableSetId) 
             (sortableSets: seq<sortableSet>) 
             =
        let sortableSetMap =
            sortableSets |> Seq.map (fun s -> (s |> SortableSet.getSortableSetId , s)) 
                         |> Map.ofSeq

        { setOfSortableSetId = setOfSortableSetId
          sortableSetMap = sortableSetMap }

    let create
        (setOfSortableSetId:setOfSortableSetId)
        (sortableSetCount: sortableSetCount)
        (sortableSetGen: sortableSetId -> sortableSet option)
        =
        generateSortableSetIds setOfSortableSetId
        |> Seq.map (fun sId -> sortableSetGen sId )
        |> Seq.filter(Option.isSome)
        |> Seq.map(Option.get)
        |> Seq.take(sortableSetCount |> SortableSetCount.value)
        |> load setOfSortableSetId


    let createRandom
        (setOfSortableSetId:setOfSortableSetId)
        (sortableSetCount: sortableSetCount)
        (rnGen: rngGen)
        (sortableSetRndGen: IRando -> sortableSetId -> sortableSet option)
        =
        let randy = rnGen |> Rando.fromRngGen
        let sortableSetGen = sortableSetRndGen randy
        create setOfSortableSetId sortableSetCount sortableSetGen


    let makeOrbits
        (setOfSortableSetId:setOfSortableSetId)
        (sortableSetCount: sortableSetCount)
        (rolloutFormat: rolloutFormat)
        (maxSortableCount: sortableCount option)
        (perms: permutation array)
        =
        let mutable dex = -1
        let getPerm () =
            dex <- dex + 1
            perms.[dex]

        let sortableSetGen (id:sortableSetId) = 
            SortableSet.makeOrbits rolloutFormat maxSortableCount (getPerm ()) id
            |> Result.toOption
        create setOfSortableSetId sortableSetCount sortableSetGen


    let makeRandomOrbits
        (setOfSortableSetId:setOfSortableSetId)
        (sortableSetCount: sortableSetCount)
        (order:order)
        (rolloutFormat: rolloutFormat)
        (maxSortableCount: sortableCount option)
        (rnGen: rngGen)
        =
        let randy = rnGen |> Rando.fromRngGen
        let perms = Permutation.createRandoms order randy
                    |> Seq.take(sortableSetCount |> SortableSetCount.value)
                    |> Seq.toArray
        makeOrbits
            setOfSortableSetId
            sortableSetCount
            rolloutFormat
            maxSortableCount
            perms