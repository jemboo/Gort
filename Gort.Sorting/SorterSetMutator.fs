namespace global

open System

type sorterParentMap = 
        private {
        id: sorterParentMapId;
        mutantSetId:sorterSetId;
        parentSetId:sorterSetId;
        parentMap:Map<sorterId, sorterParentId> }

module SorterParentMap =

    let load
            (id:sorterParentMapId)
            (mutantSetId:sorterSetId)
            (parentSetId:sorterSetId)
            (parentMap:Map<sorterId, sorterParentId>)
        =
        {   
            id=id
            parentMap=parentMap
            mutantSetId=mutantSetId
            parentSetId=parentSetId
        }

    let getId
            (sorterParentMap:sorterParentMap) 
         =
         sorterParentMap.id


    let makeId
            (parentSetId:sorterSetId)
            (mutantSetId:sorterSetId)
        =
        [|parentSetId :> obj; mutantSetId :> obj|] 
        |> GuidUtils.guidFromObjs
        |> SorterParentMapId.create


    let getParentMap 
             (sorterParentMap:sorterParentMap) 
         =
         sorterParentMap.parentMap


    let getMutantSorterSetId
                (sorterParentMap:sorterParentMap) 
         =
         sorterParentMap.mutantSetId


    let getParentSorterSetId
                (sorterParentMap:sorterParentMap) 
         =
         sorterParentMap.parentSetId




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

    let getSorterCountFinal (sum: sorterSetMutator) = sum.sorterCountFinal

    let getMutantSorterSetId
            (sorterSetMutator:sorterSetMutator)
            (parentSetId:sorterSetId)
        =
        [|parentSetId :> obj; sorterSetMutator :> obj|] 
        |> GuidUtils.guidFromObjs
        |> SorterSetId.create


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
                    SorterParentMap.makeId
                        parentSetId
                        mutantSetId


            let parentMap =
                tupes 
                    |> Seq.map(fun (parentId, srtr) -> 
                          srtr |> Sorter.getSorterId, 
                          parentId |> SorterParentId.toSorterParentId )
                |> Map.ofSeq


            let sorterParentMap = 
                SorterParentMap.load
                    sorterParentMapId
                    mutantSetId
                    parentSetId
                    parentMap

            return  sorterParentMap, mutantSet

        }


type mutantSorterSetMap = 
    private {  
               mutantSetId:sorterSetId;
               parentSetId:sorterSetId;
               sorterSetMutator:sorterSetMutator;
               sorterParentMap:sorterParentMap }

module MutantSorterSetMap =

    let load
            (sorterSetMutator:sorterSetMutator) 
            (mutantSetId:sorterSetId)
            (parentSetId:sorterSetId)
            (sorterParentMap:sorterParentMap)
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



