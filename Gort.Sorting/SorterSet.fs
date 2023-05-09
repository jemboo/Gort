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

    let getSorters (sorterSet: sorterSet) = 
            sorterSet.sorterMap |> Map.values |> Seq.cast

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


    let load (id:sorterSetId) 
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
        (sorterMutatr: sorterUniformMutator) 
        (sorterStId:sorterSetId)
        (randy: IRando) 
        =
        let _mutato dex id =
            let sortr = sorterBase.[dex % sorterBase.Length]
            let muty = sorterMutatr.mutatorFunc sortr id randy
            muty

        generateSorterIds sorterStId
        |> Seq.mapi(_mutato)
        |> Seq.filter(Result.isOk)
        |> Seq.map(Result.ExtractOrThrow)
        |> Seq.take(sorterCt |> SorterCount.value)
        |> load sorterStId order





type mutantSorterSet = 
    private {  sorterSet:sorterSet;
               sorterMutator:sorterMutator;
               parentMap:Map<sorterId, sorterId> }
               // maps mutant sorterId's to parent sorterId's

module MutantSorterSet =
    
    let create 
            (sorterMutator:sorterMutator) 
            (randy:IRando)
            (sorterCount:sorterCount)
            (parents:sorter seq)
        =
        result {
            let! tupes = 
                SorterMutator.makeMutants
                    sorterMutator
                    randy
                    sorterCount
                    parents

            let sorterSet = 
                    SorterSet.load
                        ((Guid.NewGuid()) |> SorterSetId.create)
                        (parents |> Seq.head |> Sorter.getOrder)
                        (tupes |> Seq.map(snd))

            let parentMap =
                tupes 
                    |> Seq.map(fun (parentId, srtr) -> 
                        ( srtr |> Sorter.getSorterId), 
                          parentId )
                      |> Map.ofSeq
            return
                {
                    sorterSet = sorterSet;
                    sorterMutator = sorterMutator;
                    parentMap = parentMap
                }
        }