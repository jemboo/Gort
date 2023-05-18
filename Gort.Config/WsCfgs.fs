namespace global
open System


module WsCfgs = 

    let useParall = true |> UseParallel.create

    let rngGen1 = RngGen.createLcg (12544 |> RandomSeed.create)
    let rngGen2 = RngGen.createLcg (72574 |> RandomSeed.create)
    let rngGen3 = RngGen.createLcg (82584 |> RandomSeed.create)

    let orders = [|16;|] |> Array.map(Order.createNr)

    let switchGenModes =
        [
            switchGenMode.StageSymmetric; 
            switchGenMode.Switch; 
            switchGenMode.Stage
        ]



    ////********  SortableSet  ****************

    let allSortableSetCfgs () =
        [| 
          for ordr in orders do
            SortableSetCertainCfg.makeStandardSwitchReducedOneStage ordr
            |> sortableSetCfg.Certain

          //for ordr in orders do
          //  sortableSetCfgCertain.All_Bits ordr
          //  |> sortableSetCfg.Certain
        |]



    //********  SorterSet  ****************

    let sorterCountBase = 10

    let sorterCounts (order:order) = 
        match (order |> Order.value) with
        | 14 -> sorterCountBase * 20 |> SorterCount.create
        | 16 -> sorterCountBase * 5  |> SorterCount.create
        | 18 -> sorterCountBase * 1  |> SorterCount.create
        | 20 -> sorterCountBase * 25   |> SorterCount.create
        | 22 -> sorterCountBase * 5    |> SorterCount.create
        | 24 -> sorterCountBase        |> SorterCount.create
        | _ -> failwith "not handled"



    //let sorterCounts (order:order) = 
    //    match (order |> Order.value) with
    //    | 14 -> sorterCountBase * 2000 |> SorterCount.create
    //    | 16 -> sorterCountBase * 500  |> SorterCount.create
    //    | 18 -> sorterCountBase * 100  |> SorterCount.create
    //    | 20 -> sorterCountBase * 25   |> SorterCount.create
    //    | 22 -> sorterCountBase * 5    |> SorterCount.create
    //    | 24 -> sorterCountBase        |> SorterCount.create
    //    | _ -> failwith "not handled"


    let makeSorterSetRndCfg 
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
                    
        SorterSetRndCfg.create 
            order rngGen switchGenMode pfx
            (SwitchCount.orderTo999SwitchCount order)
            (sorterCounts order)


    let allSorterSetRndCfgs () =
        [| for ordr in orders do
             for genMode in switchGenModes do
                 for usePfx in [false] do
                    makeSorterSetRndCfg 
                        genMode
                        ordr
                        rngGen1
                        usePfx
        |]


    //********  SorterSetMutate  ****************

    let mutRate = 0.2 |> MutationRate.create

    let allSorterSetMutateCfgs () =
        [| 
            for ordr in orders do
             for genMode in switchGenModes do
                 for usePfx in [false] do
                    SorterSetMutatedFromRndCfg.create
                        ordr
                        rngGen1
                        genMode
                        (SwitchCount.orderTo999SwitchCount ordr)
                        (sorterCounts ordr)
                        rngGen2
                        (sorterCounts ordr)
                        mutRate
        |]




    ////********  Ssmfr_EvalAllBitsCfg  ****************

    let allSsmfr_EvalAllBitsCfg () =
         [|
            for ordr in orders do
             for genMode in switchGenModes do
                 for usePfx in [false] do
                    Ssmfr_EvalAllBitsCfg.create
                        ordr
                        rngGen1
                        genMode
                        (SwitchCount.orderTo999SwitchCount ordr)
                        (sorterCounts ordr)
                        rngGen2
                        (sorterCounts ordr)
                        mutRate
         |]



    ////********  SorterSetEvalReport  ****************


    //let allSorterSetEvalReportCfgs () =
    //    [| 
    //        for cfg in allSorterSetEvalCfgs() do
    //            SorterSetEvalReportCfg.createFull
    //                cfg
    //    |]
