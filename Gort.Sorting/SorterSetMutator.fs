namespace global

open System


type sorterSetMutator = 
    private
        { 
          sorterMutator: sorterMutator
          sorterCountFinal: sorterCount Option
          rngGen: rngGen
        }

module SorterSetMutator =

    let load
            (sorterMutator:sorterMutator) 
            (sorterCountFinal:sorterCount option) 
            (rngGen:rngGen) 
        =
        { 
          sorterMutator = sorterMutator
          sorterCountFinal = sorterCountFinal
          rngGen = rngGen
        }

    let getSorterMutator (sum: sorterSetMutator) = sum.sorterMutator

    let getRngGen (sum: sorterSetMutator) = sum.rngGen

    let getSorterCountFinal (sum: sorterSetMutator) = 
         sum.sorterCountFinal

    let getMutantSorterSetId
            (sorterSetMutator:sorterSetMutator)
            (parentSetId:sorterSetId)
        =
        [|  
            parentSetId :> obj;
            (sorterSetMutator |> getRngGen) :> obj;
            (sorterSetMutator
                    |> getSorterMutator
                    |> SorterMutator.getMutatorId):> obj
        |] 
        |> GuidUtils.guidFromObjs
        |> SorterSetId.create


    let makeSorterParentMap
            (sorterSetMutator:sorterSetMutator)
            (parentSet:sorterSet)
        =
        let mutantSetId = 
            parentSet
            |> SorterSet.getId
            |> getMutantSorterSetId sorterSetMutator

        let childSorterCount = 
            match (sorterSetMutator |> getSorterCountFinal) with
            | Some sc -> sc
            | None -> parentSet |> SorterSet.getSorterCount

        let sorterParentMapId = 
                SorterSetParentMap.makeId
                    (parentSet |> SorterSet.getId)
                    mutantSetId

        let parentMap =
            parentSet
            |> SorterSet.getSorters
            |> Seq.map(Sorter.getSorterId >> SorterParentId.toSorterParentId)
            |> CollectionOps.infinteLoop
            |> Seq.allPairs (mutantSetId |> SorterSet.generateSorterIds)
            |> Seq.take (childSorterCount |> SorterCount.value)
            |> Map.ofSeq

        SorterSetParentMap.load
            sorterParentMapId
            mutantSetId
            (parentSet |> SorterSet.getId)
            parentMap



    let createMutantSorterSetAndParentMap
            (sorterSetMutator:sorterSetMutator)
            (parentSet:sorterSet)
        =
        result {
            let parentSetId = parentSet |> SorterSet.getId
            let mutantSetId = parentSetId |> getMutantSorterSetId sorterSetMutator

            let randy = sorterSetMutator |> getRngGen |> Rando.fromRngGen
            let childSorterCount = 
                match (sorterSetMutator |> getSorterCountFinal) with
                | Some sc -> sc
                | None -> parentSet |> SorterSet.getSorterCount

            let! tupes = 
                SorterMutator.makeMutants
                    (sorterSetMutator |> getSorterMutator)
                    randy
                    childSorterCount
                    (parentSet |> SorterSet.getSorters)

            let mutantSet = 
                    SorterSet.load
                        mutantSetId
                        (parentSet |> SorterSet.getOrder)
                        (tupes |> Seq.map(snd))


            let sorterParentMapId = 
                    SorterSetParentMap.makeId
                        parentSetId
                        mutantSetId


            let parentMap =
                tupes 
                    |> Seq.map(fun (parentId, srtr) -> 
                          srtr |> Sorter.getSorterId, 
                          parentId |> SorterParentId.toSorterParentId )
                |> Map.ofSeq


            let sorterParentMap = 
                SorterSetParentMap.load
                    sorterParentMapId
                    mutantSetId
                    parentSetId
                    parentMap

            return  sorterParentMap, mutantSet

        }


    let createMutantSorterSetFromParentMap
            (sorterSetParentMap:sorterSetParentMap)
            (sorterSetMutator:sorterSetMutator)
            (sortersToMutate:sorterSet)
        =
        result {
            let! mutants = 
                SorterMutator.makeMutants2
                    (sorterSetMutator |> getSorterMutator)
                    (sorterSetMutator |> getRngGen |> Rando.fromRngGen)
                    (sorterSetParentMap |> SorterSetParentMap.getParentMap)
                    (sortersToMutate |> SorterSet.getSorters)


            return
                SorterSet.load
                    (sorterSetParentMap |> SorterSetParentMap.getChildSorterSetId)
                    (sortersToMutate |> SorterSet.getOrder)
                    mutants
        }




type mutantSorterSetMap = 
    private {  
               mutantSetId:sorterSetId;
               parentSetId:sorterSetId;
               sorterSetMutator:sorterSetMutator;
               sorterParentMap:sorterSetParentMap }

module MutantSorterSetMap =

    let load
            (sorterSetMutator:sorterSetMutator) 
            (mutantSetId:sorterSetId)
            (parentSetId:sorterSetId)
            (sorterParentMap:sorterSetParentMap)
        =
        {
            mutantSetId = mutantSetId;
            parentSetId = parentSetId;
            sorterSetMutator = sorterSetMutator;
            sorterParentMap = sorterParentMap
        }


    let create
            (sorterSetMutator:sorterSetMutator)
            (parentSet:sorterSet)
        =
        result {
            let! sorterParentMap, mutantSet = 
                    SorterSetMutator.createMutantSorterSetAndParentMap
                        sorterSetMutator
                        parentSet

            return ( load
                          sorterSetMutator
                          (mutantSet |> SorterSet.getId)
                          (parentSet |> SorterSet.getId)
                          sorterParentMap,
                     mutantSet )
         }


    let getSorterSetMutator 
                (mutantSorterSet:mutantSorterSetMap) 
         =
         mutantSorterSet.sorterSetMutator


    let getSorterParentMap
                (mutantSorterSet:mutantSorterSetMap) 
         =
         mutantSorterSet.sorterParentMap



