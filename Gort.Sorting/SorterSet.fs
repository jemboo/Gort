namespace global

open System

type sorterSet =
    private
        { id: sorterSetId
          order: order
          sorterMap: Map<sorterId, sorter> }

module SorterSet =

    let getId (sorterSet: sorterSet) = sorterSet.id

    let getSorterCount (sorterSet: sorterSet) = sorterSet.sorterMap.Count

    let getSorters (sorterSet: sorterSet) = sorterSet.sorterMap.Values

    let generateSorterIds (sorterStId:sorterSetId) (sorterCt:sorterCount) =
        RandVars.rndGuidsLcg (sorterStId |> SorterSetId.value)
        |> Seq.map(SorterId.create)
        |> Seq.take(sorterCt |> SorterCount.value)

    let load (id:sorterSetId) (order: order) (sorters: seq<sorter>) =
        let sorterMap =
            sorters |> Seq.map (fun s -> (s |> Sorter.getSorterId, s)) |> Map.ofSeq

        { sorterSet.id = id
          order = order
          sorterMap = sorterMap }


    let createRandom
        (sorterStId:sorterSetId)
        (sorterCt: sorterCount)
        (order: order)
        (wPfx: switch seq)
        (switchCount: switchCount)
        (sorterRndGen: sorterId -> order -> switch seq -> switchCount -> IRando -> sorter)
        (rnGen: rngGen) =
        let randy = rnGen |> Rando.fromRngGen

        generateSorterIds sorterStId sorterCt
        |> Seq.map (fun sId -> sorterRndGen sId order wPfx switchCount randy)
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
