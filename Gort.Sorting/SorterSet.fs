namespace global

open System

type sorterSet =
    private
        { id: sorterSetId
          order: order
          sorterMap: Map<sorterId, sorter> }

module SorterSet =

    let getSorterCount (sorterSet: sorterSet) = sorterSet.sorterMap.Count

    let getSorters (sorterSet: sorterSet) = sorterSet.sorterMap.Values

    let fromSorters (order: order) (sorters: seq<sorter>) =
        let sorterMap =
            sorters |> Seq.map (fun s -> (s |> Sorter.getSorterId, s)) |> Map.ofSeq

        { sorterSet.id = Guid.NewGuid() |> SorterSetId.create
          order = order
          sorterMap = sorterMap }


    let createRandom
        (order: order)
        (wPfx: switch seq)
        (switchCount: switchCount)
        (sorterRndGen: order -> switch seq -> switchCount -> IRando -> sorter)
        (sorterCt: sorterCount)
        (rnGen: rngGen) =
        let randy = rnGen |> Rando.fromRngGen

        seq { 1 .. (sorterCt |> SorterCount.value) }
        |> Seq.map (fun _ -> sorterRndGen order wPfx switchCount randy)
        |> fromSorters order


    let createRandomSwitches
        (order: order)
        (wPfx: switch seq)
        (switchCount: switchCount)
        (sorterCt: sorterCount)
        (rnGen: rngGen)  =
        createRandom order wPfx switchCount Sorter.randomSwitches sorterCt rnGen


    let createRandomStages
        (switchFreq: switchFrequency)
        (order: order)
        (wPfx: switch seq)
        (switchCount: switchCount)
        (sorterCt: sorterCount)
        (rnGen: rngGen)  =
        createRandom order wPfx switchCount (Sorter.randomStages switchFreq) sorterCt rnGen


    let createRandomSymmetric
        (order: order)
        (wPfx: switch seq)
        (switchCount: switchCount)
        (sorterCt: sorterCount)
        (rnGen: rngGen) =
        createRandom order wPfx switchCount Sorter.randomSymmetric sorterCt rnGen


    let createRandomBuddies
        (stageWindowSz: stageWindowSize)
        (order: order)
        (wPfx: switch seq)
        (switchCount: switchCount)
        (sorterCt: sorterCount)
        (rnGen: rngGen)  =
        createRandom order wPfx switchCount (Sorter.randomBuddies stageWindowSz) sorterCt rnGen
