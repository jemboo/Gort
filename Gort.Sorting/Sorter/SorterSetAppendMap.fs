namespace global


type sorterSetConcatMap = 
        private {
        id: sorterSetAppendMapId;
        sorterSetBaseId:sorterSetId;
        concatMap:Map<sorterId, sorterId[]> }

module SorterSetConcatMap 
        =
    let make
            (sorterSetBaseId:sorterSetId) 
            (concatMap:Map<sorterId, sorterId[]>)
        =
        { 
          id = concatMap 
                |> Map.toArray 
                |> Array.map(fun tup -> tup:> obj) 
                |> GuidUtils.guidFromObjs 
                |> SorterSetAppendMapId.create
          sorterSetBaseId = sorterSetBaseId
          concatMap = concatMap
        }

    //let makeId
    //        (sorterSetPfxId:sorterSetId)
    //        (sorterSetSfxId:sorterSetId)
    //    =
    //    [|
    //        "sorterSetAppendMap" :> obj
    //        sorterSetPfxId |> SorterSetId.value :> obj; 
    //        sorterSetSfxId |> SorterSetId.value :> obj
    //    |]
    //    |> GuidUtils.guidFromObjs
    //    |> SorterSetParentMapId.create


    //let getId (sum: sorterSetMutator) = sum.sorterMutator

    //let getSorterSetBaseId (sum: sorterSetMutator) = sum.rngGen

    //let getConcatMap (sum: sorterSetMutator) = 
    //     sum.sorterCountFinal

    //let getMutantSorterSetId
    //        (sorterSetMutator:sorterSetMutator)
    //        (parentSetId:sorterSetId)
    //    =
    //    [|  
    //        parentSetId :> obj;
    //        (sorterSetMutator |> getRngGen) :> obj;
    //        (sorterSetMutator
    //                |> getSorterMutator
    //                |> SorterMutator.getMutatorId):> obj
    //    |] 
    //    |> GuidUtils.guidFromObjs
    //    |> SorterSetId.create


    //let makeSorterParentMap
    //        (sorterSetMutator:sorterSetMutator)
    //        (parentSet:sorterSet)
    //    =
    //    let mutantSetId = 
    //        parentSet
    //        |> SorterSet.getId
    //        |> getMutantSorterSetId sorterSetMutator

    //    let childSorterCount = 
    //        match (sorterSetMutator |> getSorterCountFinal) with
    //        | Some sc -> sc
    //        | None -> parentSet |> SorterSet.getSorterCount

    //    let sorterParentMapId = 
    //            SorterSetParentMap.makeId
    //                (parentSet |> SorterSet.getId)
    //                mutantSetId

    //    let parentMap =
    //        parentSet
    //        |> SorterSet.getSorters
    //        |> Seq.map(Sorter.getSorterId >> SorterParentId.toSorterParentId)
    //        |> CollectionOps.infinteLoop
    //        |> Seq.allPairs (mutantSetId |> SorterSet.generateSorterIds)
    //        |> Seq.take (childSorterCount |> SorterCount.value)
    //        |> Map.ofSeq

    //    SorterSetParentMap.load
    //        sorterParentMapId
    //        mutantSetId
    //        (parentSet |> SorterSet.getId)
    //        parentMap



    //let createMutantSorterSetAndParentMap
    //        (sorterSetMutator:sorterSetMutator)
    //        (parentSet:sorterSet)
    //    =
    //    result {
    //        let parentSetId = parentSet |> SorterSet.getId
    //        let mutantSetId = parentSetId |> getMutantSorterSetId sorterSetMutator

    //        let randy = sorterSetMutator |> getRngGen |> Rando.fromRngGen
    //        let childSorterCount = 
    //            match (sorterSetMutator |> getSorterCountFinal) with
    //            | Some sc -> sc
    //            | None -> parentSet |> SorterSet.getSorterCount

    //        let! tupes = 
    //            SorterMutator.makeMutants
    //                (sorterSetMutator |> getSorterMutator)
    //                randy
    //                childSorterCount
    //                (parentSet |> SorterSet.getSorters)

    //        let mutantSet = 
    //                SorterSet.load
    //                    mutantSetId
    //                    (parentSet |> SorterSet.getOrder)
    //                    (tupes |> Seq.map(snd))


    //        let sorterParentMapId = 
    //                SorterSetParentMap.makeId
    //                    parentSetId
    //                    mutantSetId


    //        let parentMap =
    //            tupes 
    //                |> Seq.map(fun (parentId, srtr) -> 
    //                      srtr |> Sorter.getSorterId, 
    //                      parentId |> SorterParentId.toSorterParentId )
    //            |> Map.ofSeq


    //        let sorterParentMap = 
    //            SorterSetParentMap.load
    //                sorterParentMapId
    //                mutantSetId
    //                parentSetId
    //                parentMap

    //        return  sorterParentMap, mutantSet

    //    }


    //let createMutantSorterSetFromParentMap
    //        (sorterSetParentMap:sorterSetParentMap)
    //        (sorterSetMutator:sorterSetMutator)
    //        (sortersToMutate:sorterSet)
    //    =
    //    result {
    //        let! mutants = 
    //            SorterMutator.makeMutants2
    //                (sorterSetMutator |> getSorterMutator)
    //                (sorterSetMutator |> getRngGen |> Rando.fromRngGen)
    //                (sorterSetParentMap |> SorterSetParentMap.getParentMap)
    //                (sortersToMutate |> SorterSet.getSorters)


    //        return
    //            SorterSet.load
    //                (sorterSetParentMap |> SorterSetParentMap.getChildSorterSetId)
    //                (sortersToMutate |> SorterSet.getOrder)
    //                mutants
    //    }