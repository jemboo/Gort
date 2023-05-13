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
            (wPfx: switch seq)
            (switchCount: switchCount)
            (rnGen: unit -> rngGen)   
        =
        let perms = permSeed 
                    |> Permutation.powers None
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


//type sorterParentMap = 
//        private {
//        id: sorterParentMapId;
//        parentMap:Map<sorterId, sorterParentId> }

//module SorterParentMap =

//    let load
//            (id:sorterParentMapId)
//            (parentMap:Map<sorterId, sorterParentId>)
//        =
//        {   id=id
//            parentMap=parentMap }

//    let getId
//            (sorterParentMap:sorterParentMap) 
//         =
//         sorterParentMap.id

//    let getParentMap 
//             (sorterParentMap:sorterParentMap) 
//         =
//         sorterParentMap.parentMap


//type sorterSetMutator = 
//    private
//        { 
//          sorterMutator: sorterMutator
//          sorterCountFinal: sorterCount Option
//          rngGen: rngGen
//        }

//module SorterSetMutator =

//    let load 
//            (sorterMutator:sorterMutator) 
//            (sorterCountFinal:sorterCount option) 
//            (rngGen:rngGen) 
//        =
//        { 
//          sorterMutator = sorterMutator
//          sorterCountFinal = sorterCountFinal
//          rngGen = rngGen
//        }

//    let getSorterMutator (sum: sorterSetMutator) = sum.sorterMutator

//    let getRngGen (sum: sorterSetMutator) = sum.rngGen

//    let getSorterCountFinal (sum: sorterSetMutator) = sum.sorterCountFinal

//    let createMutantSorterSetAndParentMap
//            (sorterSetMutator:sorterSetMutator)
//            (parentSet:sorterSet)
//        =
//        result {
//            let parentSetId = parentSet |> SorterSet.getId
//            let mutantSetId = 
//                [|parentSetId :> obj; sorterSetMutator :> obj|] 
//                |> GuidUtils.guidFromObjs
//                |> SorterSetId.create

//            let randy = sorterSetMutator |> getRngGen |> Rando.fromRngGen
//            let childSorterCount = 
//                match (sorterSetMutator |> getSorterCountFinal) with
//                | Some sc -> sc
//                | None -> parentSet |> SorterSet.getSorterCount

//            let! tupes = 
//                SorterMutator.makeMutants
//                    (sorterSetMutator |> getSorterMutator)
//                    randy
//                    childSorterCount
//                    (parentSet |> SorterSet.getSorters)

//            let mutantSet = 
//                    SorterSet.load
//                        mutantSetId
//                        (parentSet |> SorterSet.getOrder)
//                        (tupes |> Seq.map(snd))


//            let sorterParentMapId = 
//                [|parentSetId :> obj; mutantSetId :> obj|] 
//                |> GuidUtils.guidFromObjs
//                |> SorterParentMapId.create


//            let parentMap =
//                tupes 
//                    |> Seq.map(fun (parentId, srtr) -> 
//                          srtr |> Sorter.getSorterId, 
//                          parentId |> SorterParentId.toSorterParentId )
//                |> Map.ofSeq


//            let sorterParentMap = 
//                SorterParentMap.load
//                    sorterParentMapId
//                    parentMap

//            return  sorterParentMap, mutantSet

//        }


//type mutantSorterSetMap = 
//    private {  mutantSetId:sorterSetId;
//               parentSetId:sorterSetId;
//               sorterSetMutator:sorterSetMutator;
//               sorterParentMap:sorterParentMap }

//module MutantSorterSetMap =

//    let load
//            (sorterSetMutator:sorterSetMutator) 
//            (mutantSetId:sorterSetId)
//            (parentSetId:sorterSetId)
//            (sorterParentMap:sorterParentMap)
//        =
//        {
//            mutantSetId = mutantSetId;
//            parentSetId = parentSetId;
//            sorterSetMutator = sorterSetMutator;
//            sorterParentMap = sorterParentMap
//        }


//    let create
//            (sorterSetMutator:sorterSetMutator)
//            (parentSet:sorterSet)
//        =
//        result {
//            let! sorterParentMap, mutantSet = 
//                    SorterSetMutator.createMutantSorterSetAndParentMap
//                        sorterSetMutator
//                        parentSet

//            return ( load
//                          sorterSetMutator
//                          (mutantSet |> SorterSet.getId)
//                          (parentSet |> SorterSet.getId)
//                          sorterParentMap,
//                     mutantSet )
//         }


//    let getSorterSetMutator 
//                (mutantSorterSet:mutantSorterSetMap) 
//         =
//         mutantSorterSet.sorterSetMutator


//    let getMutantSorterSetId
//                (mutantSorterSet:mutantSorterSetMap) 
//         =
//         mutantSorterSet.mutantSetId


//    let getParentSorterSetId
//                (mutantSorterSet:mutantSorterSetMap) 
//         =
//         mutantSorterSet.parentSetId


//    let getSorterParentMap
//                (mutantSorterSet:mutantSorterSetMap) 
//         =
//         mutantSorterSet.sorterParentMap



