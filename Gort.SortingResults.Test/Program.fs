open System


module Program =

    let etup1 = 
          None

    [<EntryPoint>]
    let main argv = 
        let sorterSetId = Guid.NewGuid() |> SorterSetId.create
        let useParalll = true |> UseParallel.create
        let ordr = 16 |> Order.createNr
        let switchCt = SwitchCount.orderTo999SwitchCount ordr
        let sorterCt = SorterCount.create 100000
        let rndGn = RngGen.createLcg (9912 |> RandomSeed.create)
        let randy = rndGn |> Rando.fromRngGen
        let switchFreq = 1.0 |> SwitchFrequency.create

       // let sorterSt = SorterSet.createRandomSwitches sorterSetId sorterCt ordr [||] switchCt rndGn
       // let sorterSt = SorterSet.createRandomStages sorterSetId sorterCt switchFreq ordr [||] switchCt rndGn
       // let sorterSt = SorterSet.createRandomStages2 sorterSetId sorterCt ordr [||] switchCt rndGn
       // let sorterSt = SorterSet.createRandomStagesCoConj sorterSetId sorterCt ordr [||] switchCt rndGn
       // let sorterSt = SorterSet.createRandomSymmetric sorterSetId sorterCt ordr [||] switchCt rndGn
        let sorterSt = SorterSet.createRandomStagesSeparated sorterSetId sorterCt ordr 86 90 [||] switchCt rndGn
        let rolloutFormt = rolloutFormat.RfBs64
        let sortableStId = SortableSetId.create 123

        let sortableSt =
            SortableSet.makeAllBits sortableStId rolloutFormt ordr |> Result.ExtractOrThrow

        let sorterEvls =
            SorterSetEval.evalSorters 
                sorterEvalMode.DontCheckSuccess 
                sortableSt 
                (sorterSt |> SorterSet.getSorters) 
                useParalll

        let gps = sorterEvls |> Array.groupBy(fun e -> e |> SorterEval.getSorterSpeed)

        let runName = "rndStagesSep_86_90"

        let mutable dex = 0
        while dex < gps.Length do
            let reprt = 
                sprintf "%s\t%d\t%s"
                    runName
                    (snd gps.[dex]).Length
                    (gps.[dex] |> fst |> SorterSpeed.report) 
            Console.WriteLine(reprt)
            dex <- dex + 1















        Console.WriteLine("Hi ya") |> ignore
        0