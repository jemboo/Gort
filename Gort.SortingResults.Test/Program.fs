open System
open System.IO

module Program =

    let oneSorterSet () = 
        let sorterSetId = Guid.NewGuid() |> SorterSetId.create
        let useParalll = true |> UseParallel.create
        let ordr = 16 |> Order.createNr
        let switchCt = SwitchCount.orderTo999SwitchCount ordr
        let sorterCt = SorterCount.create 100
        let rndGn = RngGen.createLcg (9912 |> RandomSeed.create)
        let randy = rndGn |> Rando.fromRngGen
        let switchFreq = 1.0 |> SwitchFrequency.create
        let coreTwoCyc = TwoCycle.evenMode ordr
       // let permSeed = Permutation.create [|7;9;4;0;8;2;6;5;11;13;15;1;3;10;12;14|]
        //let permSeed = Permutation.create [|9;7;4;0;8;12;6;5;11;13;15;2;3;10;1;14|]
       // let permSeed = Permutation.create [|1;7;12;0;8;4;6;5;11;13;15;2;3;10;9;14|]
        let permSeed = Permutation.create [|10;7;12;0;8;4;6;5;11;13;15;2;3;1;9;14|]
        let permSeed = Permutation.create [|13;7;12;0;8;4;6;5;11;10;15;2;3;1;9;14|]
        let permSeed = Permutation.create [|13;7;12;0;8;4;6;5;11;10;15;3;2;1;9;14|]
                        |> Result.ExtractOrThrow
       // let sorterSt = SorterSet.createRandomSwitches sorterSetId sorterCt ordr [||] switchCt rndGn
       // let sorterSt = SorterSet.createRandomStages sorterSetId sorterCt switchFreq ordr [||] switchCt rndGn
       // let sorterSt = SorterSet.createRandomStages2 sorterSetId sorterCt ordr [||] switchCt rndGn
       // let sorterSt = SorterSet.createRandomStagesCoConj sorterSetId sorterCt ordr [||] switchCt rndGn
       // let sorterSt = SorterSet.createRandomSymmetric sorterSetId sorterCt ordr [||] switchCt rndGn
       // let sorterSt = SorterSet.createRandomStagesSeparated sorterSetId sorterCt ordr 86 90 [||] switchCt rndGn
        let sorterSt = SorterSet.createRandomOrbitDraws sorterSetId sorterCt coreTwoCyc permSeed [||] switchCt rndGn

        let rolloutFormt = rolloutFormat.RfBs64
        let sortableStId = SortableSetId.create (Guid.NewGuid())

        let sortableSt =
            SortableSet.makeAllBits sortableStId rolloutFormt ordr |> Result.ExtractOrThrow

        let sorterEvls =
            SorterSetEval.evalSorters 
                sorterEvalMode.DontCheckSuccess 
                sortableSt 
                (sorterSt |> SorterSet.getSorters) 
                useParalll

        let gps = sorterEvls |> Array.groupBy(fun e -> e |> SorterEval.getSorterSpeed)

        let runName = "rndOrbitDraws6"

        let mutable dex = 0
        while dex < gps.Length do
            let reprt = 
                sprintf "%s\t%d\t%s"
                    runName
                    (snd gps.[dex]).Length
                    (gps.[dex] |> fst |> SorterSpeed.report) 
            Console.WriteLine(reprt)
            dex <- dex + 1



   let runSorterSetOnSortableSet 
        (sw:StreamWriter)
        (forSortables:sortableSet) 
        (forSorters:sortableSet)
        (switchCt:switchCount)
        (sorterBatchCt:sorterCount)
        (coreTwoCyc:twoCycle)
        (rngen:rngGen)
        (useParallel:useParallel)
        =
        let ssIdtemp, sorterPermRoot = forSorters |> SortableSet.getIdAndPermRoot
        let sorterSetId = ssIdtemp |> SortableSetId.value |> SorterSetId.create
        let sorterSetTag = forSorters |> SortableSet.makeTag
        let sortableSetTag = forSortables |> SortableSet.makeTag
        let sortableSetCt = forSortables |> SortableSet.getSortableCount
        let sorterSt = SorterSet.createRandomOrbitDraws 
                            sorterSetId 
                            sorterBatchCt 
                            coreTwoCyc 
                            sorterPermRoot 
                            [||] 
                            switchCt 
                            rngen


        let sorterEvls =
            SorterSetEval.evalSorters 
                sorterEvalMode.DontCheckSuccess 
                forSortables 
                (sorterSt |> SorterSet.getSorters) 
                useParallel

        let gps = sorterEvls |> Array.groupBy(fun e -> e |> SorterEval.getSorterSpeed)

        let mutable dex = 0
        while dex < gps.Length do
            let binCt = (snd gps.[dex]).Length
            let stCt = (gps.[dex] |> fst |> SorterSpeed.getStageCount0) 
            let swCt = (gps.[dex] |> fst |> SorterSpeed.getSwitchCount0) 
            let reprt = 
                sprintf "%s\t%s\t%d\t%d\t%d\t%d\t%d\t%d"
                    sorterSetTag
                    sortableSetTag
                    (sortableSetCt |> SortableCount.value)
                    binCt
                    stCt
                    swCt
                    (stCt * binCt)
                    (swCt * binCt)
            sw.WriteLine(reprt)
            dex <- dex + 1


    let setOfSorterSets (sw:StreamWriter) = 
        let useParalll = true |> UseParallel.create
        let order = Order.createNr 24
        let switchCt = SwitchCount.orderTo999SwitchCount order
        let sorterBatchCt = SorterCount.create 1000
        let coreTwoCyc = TwoCycle.evenMode order

        let sOfSortableSetId = Guid.NewGuid() |> SetOfSortableSetId.create
        let sortableSetCount = SortableSetCount.create 30
        let maxSortableCount = None // SortableCount.create 10 |> Some
        let ssFmtRu8 = rolloutFormat.RfU8
        let rngGnSorterSeeds = RngGen.create rngType.Lcg (11288 |> RandomSeed.create)
        //let rngGnSortableSeeds = RngGen.create rngType.Lcg (11236 |> RandomSeed.create)

        let sorterPermSeeds = 
                Permutation.createRandoms order (rngGnSorterSeeds |> Rando.fromRngGen)
                |> Seq.take(sortableSetCount |> SortableSetCount.value)
                |> Seq.toArray

        //let sortablePermSeeds = 
        //        Permutation.createRandoms order (rngGnSortableSeeds |> Rando.fromRngGen)
        //        |> Seq.take(sortableSetCount |> SortableSetCount.value)
        //        |> Seq.toArray


        let randySampler = RngGen.create rngType.Lcg (61123 |> RandomSeed.create)
                           |> Rando.fromRngGen

        let sorterOrbis = 
            SetOfSortableSet.makeOrbits
              sOfSortableSetId
              sortableSetCount
              ssFmtRu8
              maxSortableCount
              sorterPermSeeds


        let orderedSorterOrbis = sorterOrbis
                                 |> SetOfSortableSet.getSortableSets 
                                 |> Seq.sortBy(fun ss -> 
                                      ss |> SortableSet.getSortableCount)



        sw.WriteLine("SorterSet\tSortableSet\tSortableCt\tbinCt\tstages\tswitches\tStageT\tSwitchT")

        CollectionOps.cartesianProduct orderedSorterOrbis orderedSorterOrbis
                   |> Seq.iter(
                    fun tup -> 
                        runSorterSetOnSortableSet
                            sw
                            (fst tup) 
                            (snd tup) 
                            switchCt 
                            sorterBatchCt 
                            coreTwoCyc
                            (Rando.nextRngGen randySampler)
                            useParalll
                        sw.Flush()
                   )

        //orbis |> SetOfSortableSet.getSummaryReports
        //      |> Seq.iter(Console.WriteLine)







     [<EntryPoint>]
     let main argv = 
        let fileN = Guid.NewGuid() |> string
        let fileRoot = "C:\\GortFiles\\Orbis"
        let fileP = Path.Combine(fileRoot, fileN)
        use streamW = new StreamWriter(fileP)
        setOfSorterSets(streamW)

        Console.WriteLine("Hi ya") |> ignore
        0