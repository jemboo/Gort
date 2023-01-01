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

    let getSorterCount (sorterSet: sorterSet) = sorterSet.sorterMap.Count

    let getSorters (sorterSet: sorterSet) = sorterSet.sorterMap |> Map.values |> Seq.cast

    let getSortersById (maxCt:sorterCount) (ids: sorterId seq) (sorterSet: sorterSet) =
        ids |> Seq.map(fun d -> sorterSet.sorterMap.TryFind d)
            |> Seq.filter(fun ov -> ov |> Option.isSome)
            |> CollectionOps.takeUpto (maxCt |> SorterCount.value)

    let generateSorterIds (sorterStId:sorterSetId) =
        RandVars.rndGuidsLcg (sorterStId |> SorterSetId.value)
        |> Seq.map(SorterId.create)

    let load (id:sorterSetId) (order: order) (sorters: seq<sorter>) =
        let sorterMap =
            sorters |> Seq.map (fun s -> (s |> Sorter.getSorterId, s)) |> Map.ofSeq

        { sorterSet.id = id
          order = order
          sorterMap = sorterMap }

    let createEmpty = 
        load (Guid.Empty |> SorterSetId.create) (0 |> Order.createNr) (Seq.empty)

    let createRandom
        (sorterStId:sorterSetId)
        (sorterCt: sorterCount)
        (order: order)
        (wPfx: switch seq)
        (switchCount: switchCount)
        (sorterRndGen: sorterId -> order -> switch seq -> switchCount -> IRando -> sorter)
        (rnGen: rngGen) =
        generateSorterIds sorterStId
        |> Seq.map (fun sId -> sorterRndGen sId order wPfx switchCount (rnGen |> Rando.fromRngGen))
        |> Seq.take(sorterCt |> SorterCount.value)
        |> load sorterStId order 


    let createRandomSwitches
        (sorterStId:sorterSetId)
        (sorterCt: sorterCount)
        (order: order)
        (wPfx: switch seq)
        (switchCount: switchCount)
        (rnGen: rngGen)  =
        createRandom 
            sorterStId 
            sorterCt 
            order wPfx switchCount Sorter.randomSwitches rnGen


    let createRandomStages
        (sorterStId:sorterSetId)
        (sorterCt: sorterCount)
        (switchFreq: switchFrequency)
        (order: order)
        (wPfx: switch seq)
        (switchCount: switchCount)
        (rnGen: rngGen)  =
        createRandom 
            sorterStId 
            sorterCt 
            order wPfx switchCount (Sorter.randomStages switchFreq) rnGen


    let createRandomSymmetric
        (sorterStId:sorterSetId)
        (sorterCt: sorterCount)
        (order: order)
        (wPfx: switch seq)
        (switchCount: switchCount)
        (rnGen: rngGen) =
        createRandom 
            sorterStId 
            sorterCt 
            order wPfx switchCount Sorter.randomSymmetric rnGen


    let createRandomBuddies
        (sorterStId:sorterSetId)
        (sorterCt: sorterCount)
        (stageWindowSz: stageWindowSize)
        (order: order)
        (wPfx: switch seq)
        (switchCount: switchCount)
        (rnGen: rngGen)  =
        createRandom 
            sorterStId 
            sorterCt 
            order wPfx switchCount (Sorter.randomBuddies stageWindowSz) rnGen


    let createMutationSet 
        (sorterBase: sorter[]) 
        (sorterCt:sorterCount)
        (order: order) 
        (sorterMutatr: sorterUniformMutator) 
        (sorterStId:sorterSetId)
        (rnGen: rngGen) =
        let randy = rnGen |> Rando.fromRngGen
        
        //let newSorters = 
        //    sorterBase |> CollectionOps.infinteLoop
        //               |> Seq.take(sorterCt |> SorterCount.value)
        
        //load sorterStId order newSorters
        let _mutato dex id =
            let sortr = sorterBase.[dex % sorterBase.Length]
            sorterMutatr.mFunc sortr id randy
            
        generateSorterIds sorterStId
        |> Seq.mapi(_mutato)
        |> Seq.filter(Result.isOk)
        |> Seq.map(Result.ExtractOrThrow)
        |> Seq.take(sorterCt |> SorterCount.value)
        
        //(fun sId -> sorterRndGen sId order wPfx switchCount randy)
        //|> load sorterStId order 