namespace global
open System


module WsCfgs = 

    let useParall = true |> UseParallel.create

    let rngGen1 = RngGen.createLcg (12544 |> RandomSeed.create)
    let rngGen2 = RngGen.createLcg (72574 |> RandomSeed.create)
    let rngGen3 = RngGen.createLcg (82584 |> RandomSeed.create)

    let orders = [|14;16;|] |> Array.map(Order.createNr)

    //********  SortableSet  ****************

    let switchGenModes =
        [
            switchGenMode.StageSymmetric; 
            switchGenMode.Switch; 
            switchGenMode.Stage
        ]


    let allSortableSetCfgs () =
        [| 
          for ordr in orders do
            SortableSetCfgCertain.makeStandardSwitchReducedOneStage ordr
            |> sortableSetCfg.Certain

          //for ordr in orders do
          //  sortableSetCfgCertain.All_Bits ordr
          //  |> sortableSetCfg.Certain
        |]



    //********  SorterSet  ****************

    let sorterCountBase = 10

    //let sorterCounts (order:order) = 
    //    match (order |> Order.value) with
    //    | 14 -> sorterCountBase * 20 |> SorterCount.create
    //    | 16 -> sorterCountBase * 5  |> SorterCount.create
    //    | 18 -> sorterCountBase * 1  |> SorterCount.create
    //    | 20 -> sorterCountBase * 25   |> SorterCount.create
    //    | 22 -> sorterCountBase * 5    |> SorterCount.create
    //    | 24 -> sorterCountBase        |> SorterCount.create
    //    | _ -> failwith "not handled"



    let sorterCounts (order:order) = 
        match (order |> Order.value) with
        | 14 -> sorterCountBase * 2000 |> SorterCount.create
        | 16 -> sorterCountBase * 500  |> SorterCount.create
        | 18 -> sorterCountBase * 100  |> SorterCount.create
        | 20 -> sorterCountBase * 25   |> SorterCount.create
        | 22 -> sorterCountBase * 5    |> SorterCount.create
        | 24 -> sorterCountBase        |> SorterCount.create
        | _ -> failwith "not handled"


    let makeRdnDenovoSorterSetCfg 
            (switchGenMode:switchGenMode)
            (order:order)
            (rngGen:rngGen)
            (usePfx:bool)
            =
        let pfx = 
            match usePfx with
            | true ->
                TwoCycle.evenMode order
                |> Switch.fromTwoCycle
                |> Seq.toArray
            | false ->
                [||]
                    
        RndDenovoSorterSetCfg.create 
            order rngGen switchGenMode pfx
           // (24 |> SwitchCount.create)
            (SwitchCount.orderTo999SwitchCount order)
            (sorterCounts order)


    let allDenovoSorterSetCfgs () =
        [| for ordr in orders do
             for genMode in switchGenModes do
                 for usePfx in [false] do
                    makeRdnDenovoSorterSetCfg 
                        genMode
                        ordr
                        rngGen1
                        usePfx
        |]

        
    let allSorterSetCfgs () =
        allDenovoSorterSetCfgs() |> Array.map(sorterSetCfg.RndDenovo)


    //********  SorterSetMutate  ****************

    let mutRate = 0.2 |> MutationRate.create

    let makeSorterSetMutator 
            (ssCfg:rndDenovoSorterSetCfg)
        =
        let sorterUnniformMutator = 
                SorterUniformMutator.create
                        None
                        None
                        (ssCfg |> RndDenovoSorterSetCfg.getSwitchGenMode)
                        mutRate
        SorterSetMutator.load
            (sorterUnniformMutator |> sorterMutator.Uniform)
            None //(2 |> SorterCount.create |> Some) 
            rngGen1


    let allSorterSetMutateCfgs () =
        [| for rdnSsCfg in allDenovoSorterSetCfgs () do
             let ssm = rdnSsCfg |> makeSorterSetMutator
             SorterSetMutateCfg.create 
                        ssm
                        (rdnSsCfg |> sorterSetCfg.RndDenovo)
        |]




    //********  SorterSetEval  ****************
    let gvs =
        [|
            ("b0bb7a8d-4571-d4e3-b0d9-a6bc65ecb3af", 14);
            ("801229f2-d660-6222-87ca-7faafdc13527", 14);
            ("916e6c98-150e-0d81-efdb-c1ee79340236", 14);
            ("00e53012-804e-2e4a-da85-f52ace11419b", 16);
            ("f8fe1365-b9ff-67ff-590e-5ff41c2745a8", 16);
            ("7ddd8406-5d78-ddcd-889d-69d7eab4cf3f", 16);
        |]



//b0bb7a8d-4571-d4e3-b0d9-a6bc65ecb3af    14
//801229f2-d660-6222-87ca-7faafdc13527    14
//916e6c98-150e-0d81-efdb-c1ee79340236    14
//00e53012-804e-2e4a-da85-f52ace11419b    16
//f8fe1365-b9ff-67ff-590e-5ff41c2745a8    16
//7ddd8406-5d78-ddcd-889d-69d7eab4cf3f    16

    let getSortableSetCfg (ssCfg:sorterSetCfg) =
        SortableSetCfgCertain.makeStandardSwitchReducedOneStage 
                            (ssCfg |> SorterSetCfg.getOrder)
        |> sortableSetCfg.Certain


    let wak = 
        gvs
        |> Array.map(fun (gs, ov) -> (Guid gs, Order.createNr ov))
        |> Array.map (
            fun (g, o) ->
                SorterSetCfgExplicit.create
                    (Some o)
                    (g |> SorterSetId.create)
                    (sprintf "%s_%d" (g|> string) (o |> Order.value) )
                    None
                |> sorterSetCfg.Explicit
                )
        |> Array.map(fun sorterSetCfg ->
                SorterSetEvalCfg.create
                    (getSortableSetCfg sorterSetCfg)
                    sorterSetCfg
                    (1 |> StageCount.create)
                    sorterEvalMode.DontCheckSuccess)

    //let allSorterSetEvalCfgs () =
    //    wak
    let allSorterSetEvalCfgs () =
         [|
           for ordr in orders do

              let sortableSetCfg =
                      SortableSetCfgCertain.makeStandardSwitchReducedOneStage 
                            ordr
                      |> sortableSetCfg.Certain

              for genMode in switchGenModes do

                 let sorterSetCfg = 
                        makeRdnDenovoSorterSetCfg
                            genMode
                            ordr
                            rngGen1
                            true
                      |> sorterSetCfg.RndDenovo

                 SorterSetEvalCfg.create
                    sortableSetCfg
                    sorterSetCfg
                    (1 |> StageCount.create)
                    sorterEvalMode.DontCheckSuccess
         |]



    //********  SorterSetEvalReport  ****************


    let allSorterSetEvalReportCfgs () =
        [| 
            for cfg in allSorterSetEvalCfgs() do
                SorterSetEvalReportCfg.createFull
                    cfg
        |]
