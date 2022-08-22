namespace global
open System

type sorterSet = private { 
        id:sorterSetId; order:order; sorterMap:Map<sorterId, sorter>}

module SorterSet =

    let sorterCount (sorterSet:sorterSet) = 
            sorterSet.sorterMap.Count


    let fromSorters (id:sorterSetId)
                    (order:order) 
                    (sorters:seq<sorter>) =
        let sorterMap = 
                sorters 
                |> Seq.map(fun s-> (s |> Sorter.makeId, s))
                |> Map.ofSeq
        {
            sorterSet.id = id;
            order=order; 
            sorterMap = sorterMap
        }


    let createRandom (order:order)
                     (wPfx: switch seq)
                     (switchCount:switchCount)
                     (sorterRndGen: order->switch seq->switchCount->IRando->sorter) 
                     (sorterCt:sorterCount) 
                     (rnGen:rngGen) 
                     (id:sorterSetId) =
        let randy = rnGen |> Rando.fromRngGen
        seq { 1 .. ( sorterCt |> SorterCount.value) }
        |> Seq.map( fun _ -> sorterRndGen order wPfx switchCount randy )
        |> fromSorters id order


    let createRandomSwitches
                     (order:order)
                     (wPfx: switch seq)
                     (switchCount:switchCount)
                     (sorterCt:sorterCount) 
                     (rnGen:rngGen) 
                     (id:sorterSetId) =
        createRandom order wPfx switchCount Sorter.randomSwitches sorterCt rnGen id


    let createRandomStages
                     (switchFreq:switchFrequency) 
                     (order:order)
                     (wPfx: switch seq)
                     (switchCount:switchCount)
                     (sorterCt:sorterCount)
                     (rnGen:rngGen) 
                     (id:sorterSetId) =
        createRandom order wPfx switchCount (Sorter.randomStages switchFreq) 
                     sorterCt rnGen id


    let createRandomSymmetric
                     (order:order)
                     (wPfx: switch seq)
                     (switchCount:switchCount)
                     (sorterCt:sorterCount)
                     (rnGen:rngGen) 
                     (id:sorterSetId) =
        createRandom order wPfx switchCount Sorter.randomSymmetric
                     sorterCt rnGen id


    let createRandomBuddies
                     (stageWindowSz:stageWindowSize) 
                     (order:order)
                     (wPfx: switch seq)
                     (switchCount:switchCount)
                     (sorterCt:sorterCount)
                     (rnGen:rngGen) 
                     (id:sorterSetId) =
        createRandom order wPfx switchCount (Sorter.randomBuddies stageWindowSz) 
                     sorterCt rnGen id