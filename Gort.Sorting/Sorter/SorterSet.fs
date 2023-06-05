namespace global

open System

type sorterSet =
    private
        { id: sorterSetId
          order: order
          sorterMap: Map<sorterId, sorter> }

module SorterSet =

    let getId (sorterSet: sorterSet) = sorterSet.id

    let getOrder (sorterSet: sorterSet) = sorterSet.order

    let getSorterCount (sorterSet: sorterSet) =
            sorterSet.sorterMap.Count
            |> SorterCount.create

    let getSorters (sorterSet: sorterSet) = 
            sorterSet.sorterMap 
            |> Map.toArray
            |> Array.map(snd)
            |> Array.sortBy(Sorter.getSorterId)

    let getSortersById 
            (maxCt:sorterCount) 
            (ids: sorterId seq) 
            (sorterSet: sorterSet)
        =
        ids |> Seq.map(fun d -> sorterSet.sorterMap.TryFind d)
            |> Seq.filter(fun ov -> ov |> Option.isSome)
            |> Seq.map(fun ov -> ov |> Option.get)
            |> CollectionOps.takeUpto (maxCt |> SorterCount.value)
    

    let tryGetSorterById
            (id: sorterId) 
            (sorterSet: sorterSet)
        =
        sorterSet.sorterMap.TryFind id


    let getSorterByIdR
            (id: sorterId) 
            (sorterSet: sorterSet)
        =
        if sorterSet.sorterMap.ContainsKey id then
            sorterSet.sorterMap.[id] |> Ok
        else
            sprintf "%s not found" (id |> SorterId.value |> string)
            |> Error


    let generateSorterIds 
        (sorterStId:sorterSetId) 
        =
        RandVars.rndGuidsLcg (sorterStId |> SorterSetId.value)
        |> Seq.map(SorterId.create)


    let load 
            (id:sorterSetId) 
            (order: order) 
            (sorters: seq<sorter>) 
        =
        let sorterMap =
            sorters 
            |> Seq.map (fun s -> (s |> Sorter.getSorterId, s)) 
            |> Map.ofSeq

        { sorterSet.id = id
          order = order
          sorterMap = sorterMap }


    let createEmpty = 
        load (Guid.Empty |> SorterSetId.create) (0 |> Order.createNr) (Seq.empty)


    let create
            (sorterStId:sorterSetId)
            (sorterCt: sorterCount)
            (order: order)
            (sorterGen: sorterId -> sorter)
        =
        generateSorterIds sorterStId
        |> Seq.map (fun sId -> sorterGen sId)
        |> Seq.take(sorterCt |> SorterCount.value)
        |> load sorterStId order 


    let createMergedSorterSetId
            (lhs:sorterSetId)
            (rhs:sorterSetId) =
        [| 
            (lhs |> SorterSetId.value);
            (rhs |> SorterSetId.value);
        |] 
        |> Array.map(fun tup -> tup:> obj) 
        |> GuidUtils.guidFromObjs 
        |> SorterSetId.create


    let createMergedSorterSet
            (lhs:sorterSet)
            (rhs:sorterSet) =
        let mergeId = createMergedSorterSetId
                        (lhs |> getId)
                        (rhs |> getId)
        let mergedSorters = 
            (getSorters lhs)
            |> Array.append (getSorters rhs)
        load
            mergeId
            (lhs.order)
            mergedSorters


    let createRandom
            (sorterStId:sorterSetId)
            (sorterCt: sorterCount)
            (order: order)
            (rnGen: unit -> rngGen) 
            (sorterRndGen: (unit -> rngGen) -> sorterId -> sorter)
        =
        let sorterGen = sorterRndGen rnGen
        create sorterStId sorterCt order sorterGen


    let createRandomSwitches
            (sorterStId:sorterSetId)
            (sorterCt: sorterCount)
            (order: order)
            (wPfx: switch seq)
            (switchCount: switchCount)
            (rnGen: unit -> rngGen)  
        =
        createRandom 
            sorterStId
            sorterCt
            order
            rnGen
            (Sorter.randomSwitches order wPfx switchCount)


    let createRandomStages
            (sorterStId:sorterSetId)
            (sorterCt: sorterCount)
            (switchFreq: switchFrequency)
            (order: order)
            (wPfx: switch seq)
            (switchCount: switchCount)
            (rnGen: unit -> rngGen)   
        =
        createRandom 
            sorterStId
            sorterCt
            order
            rnGen
            (Sorter.randomStages order switchFreq wPfx switchCount)


    let createRandomStages2
            (sorterStId:sorterSetId)
            (sorterCt: sorterCount)
            (order: order)
            (wPfx: switch seq)
            (switchCount: switchCount)
            (rnGen: unit -> rngGen)  
        =
        createRandom 
            sorterStId 
            sorterCt 
            order 
            rnGen  
            (Sorter.randomStages2 order wPfx switchCount)


    let createRandomStagesCoConj
            (sorterStId:sorterSetId)
            (sorterCt: sorterCount)
            (order: order)
            (wPfx: switch seq)
            (switchCount: switchCount)
            (rnGen: unit -> rngGen) 
        =
        createRandom 
            sorterStId 
            sorterCt 
            order
            rnGen
            (Sorter.randomStagesCoConj order wPfx switchCount)


    let createRandomStagesSeparated
            (sorterStId:sorterSetId)
            (sorterCt: sorterCount)
            (order: order)
            (minSeparation: int)
            (maxSeparation: int)
            (wPfx: switch seq)
            (switchCount: switchCount)
            (rnGen: unit -> rngGen)   
        =
        createRandom 
            sorterStId
            sorterCt
            order
            rnGen 
            (Sorter.randomStagesSeparated minSeparation maxSeparation order wPfx switchCount)
            
    
    let createRandomOrbitDraws
            (sorterStId:sorterSetId)
            (sorterCt: sorterCount)
            (coreTc:twoCycle) 
            (permSeed:permutation)
            (maxOrbit:int option)
            (wPfx: switch seq)
            (switchCount: switchCount)
            (rnGen: unit -> rngGen)   
        =
        let perms = permSeed 
                    |> Permutation.powers maxOrbit
                    |> Seq.toArray
        let order = (coreTc |> TwoCycle.getOrder)
        createRandom 
            sorterStId
            sorterCt
            order
            rnGen
            (Sorter.randomPermutaionChoice coreTc perms order wPfx switchCount)


    let createRandomSymmetric
            (sorterStId:sorterSetId)
            (sorterCt: sorterCount)
            (order: order)
            (wPfx: switch seq)
            (switchCount: switchCount)
            (rnGen: unit -> rngGen) 
        =
        createRandom 
            sorterStId 
            sorterCt 
            order
            rnGen
            (Sorter.randomSymmetric order wPfx switchCount)


    let createRandomBuddies
            (sorterStId:sorterSetId)
            (sorterCt: sorterCount)
            (stageWindowSz: stageWindowSize)
            (order: order)
            (wPfx: switch seq)
            (switchCount: switchCount)
            (rnGen: unit -> rngGen)   
        =
        createRandom 
            sorterStId
            sorterCt
            order
            rnGen
            (Sorter.randomBuddies stageWindowSz order wPfx switchCount) 


    let createMutationSet 
            (sorterBase: sorter[]) 
            (sorterCt:sorterCount)
            (order: order) 
            (sorterMutatr: sorterMutator) 
            (sorterStId:sorterSetId)
            (randy: IRando) 
        =
        let _mutato dex id =
            let sortr = sorterBase.[dex % sorterBase.Length]
            let muty = 
                (sorterMutatr |> SorterMutator.getMutatorFunc) sortr id randy
            muty

        generateSorterIds sorterStId
        |> Seq.mapi(_mutato)
        |> Seq.filter(Result.isOk)
        |> Seq.map(Result.ExtractOrThrow)
        |> Seq.take(sorterCt |> SorterCount.value)
        |> load sorterStId order


    //// creates a Map<mergeId*(pfxId*sfxId)>
    //let createAppendSetMap
    //        (sorterStIdPfx:sorterSetId)
    //        (sorterCtPfx:sorterCount)
    //        (sorterStIdSfx:sorterSetId)
    //        (sorterCtSfx:sorterCount)
    //        (sorterStIdAppendSet:sorterSetId)
    //    =
    //    let sorterIdsPfx = 
    //            generateSorterIds sorterStIdPfx 
    //                |> Seq.take (sorterCtPfx |> SorterCount.value) 
    //                |> Seq.toArray

    //    let sorterIdsSfx = 
    //            generateSorterIds sorterStIdSfx 
    //                |> Seq.take (sorterCtSfx |> SorterCount.value) 
    //                |> Seq.toArray

    //    let sorterIdInputs = 
    //        CollectionOps.cartesianProduct sorterIdsPfx sorterIdsSfx
    //        |> Seq.toArray

    //    let sorterIdMerged = generateSorterIds sorterStIdAppendSet

    //    sorterIdInputs
    //    |> Seq.zip sorterIdMerged
    //    |> Seq.toArray
    //    |> Map.ofSeq


    //let createAppendSet 
    //        (sorterSetPfx: sorterSet)
    //        (sorterSetSfx: sorterSet)
    //        (sorterSetIdAppendSet:sorterSetId)
    //    =
    //    let appendSetMap = 
    //        createAppendSetMap
    //            (sorterSetPfx |> getId)
    //            (sorterSetPfx |> getSorterCount)
    //            (sorterSetSfx |> getId)
    //            (sorterSetSfx |> getSorterCount)
    //            sorterSetIdAppendSet

    //    let _mergeEm mergeId pfxId sfxId =
    //        let sPfx = sorterSetPfx |> tryGetSorterById pfxId
    //        let sSfx = sorterSetPfx |> tryGetSorterById sfxId
    //        match sPfx, sSfx with
    //        | Some pfx, Some sfx -> 
    //            pfx |> Sorter.appendSwitches mergeId (sfx |> Sorter.getSwitches) |> Ok
    //        | _ -> "sorter not found" |> Error


    //    let mergedSorters =
    //            appendSetMap 
    //            |> Map.toSeq
    //            |> Seq.map(fun (mergeId, (pfxId, sfxId)) -> _mergeEm mergeId pfxId sfxId)
    //            |> Seq.toList
    //            |> Result.sequence
    //    mergedSorters