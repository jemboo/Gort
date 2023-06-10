namespace global
open System


module WsCfgs = 

    let useParall = true |> UseParallel.create

    let rngGen1 = RngGen.createLcg (12544 |> RandomSeed.create)
    let rngGen2 = RngGen.createLcg (72574 |> RandomSeed.create)
    let rngGen3 = RngGen.createLcg (82584 |> RandomSeed.create)
    let rngGen4 = RngGen.createLcg (1544 |> RandomSeed.create)
    let rngGen5 = RngGen.createLcg (7254 |> RandomSeed.create)
    let rngGen6 = RngGen.createLcg (8284 |> RandomSeed.create)
    
    let rngGensCreate = [|rngGen2;|]
    let rngGensMutate = [|rngGen1; rngGen2; rngGen3; rngGen4; rngGen5; rngGen6;|]


    let orders = [|14;|] |> Array.map(Order.createNr)
    
    let mutRates = [| 0.10; 0.15; 0.20;|] 
                    |> Array.map(MutationRate.create)
                    


    let switchGenModes =
        [|
            switchGenMode.StageSymmetric; 
            switchGenMode.Switch; 
            switchGenMode.Stage
        |]



    ////********  SortableSet  ****************

    let allSortableSetCfgs () =
        [| 
          for ordr in orders do
            SortableSetCertainCfg.makeAllBitsReducedOneStage ordr
            |> sortableSetCfg.Certain

          //for ordr in orders do
          //  sortableSetCfgCertain.All_Bits ordr
          //  |> sortableSetCfg.Certain
        |]



    //********  SorterSet  ****************

    let sorterCountBase = 1

    let sorterCounts (order:order) = 
        match (order |> Order.value) with
        | 14 -> sorterCountBase * 50 |> SorterCount.create
        | 16 -> sorterCountBase * 5  |> SorterCount.create
        | 18 -> sorterCountBase * 1  |> SorterCount.create
        | 20 -> sorterCountBase * 25   |> SorterCount.create
        | 22 -> sorterCountBase * 5    |> SorterCount.create
        | 24 -> sorterCountBase        |> SorterCount.create
        | _ -> failwith "not handled"


    let sorterCountBase2 = 5

    let sorterCounts2 (order:order) = 
        match (order |> Order.value) with
        | 14 -> sorterCountBase2 * 20 |> SorterCount.create
        | 16 -> sorterCountBase2 * 5  |> SorterCount.create
        | 18 -> sorterCountBase2 * 1  |> SorterCount.create
        | 20 -> sorterCountBase2 * 25   |> SorterCount.create
        | 22 -> sorterCountBase2 * 5    |> SorterCount.create
        | 24 -> sorterCountBase2        |> SorterCount.create
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
        =
        SorterSetRndCfg.create 
            order 
            rngGen 
            switchGenMode
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
        |]


    //********  SorterSetMutate  ****************


    let allSorterSetMutatedFromRndCfgs () =
        [| 
            for ordr in orders do
             for genMode in switchGenModes do
                 for mutRate in mutRates do
                    SorterSetMutatedFromRndCfg.create
                        ordr
                        rngGen1
                        genMode
                        (SwitchCount.orderTo999SwitchCount ordr)
                        (sorterCounts ordr)
                        rngGen2
                        (sorterCounts2 ordr)
                        mutRate
        |]



    //********  SorterSetSelfAppend  ****************


    let allSorterSetSelfAppendCfgs () =
        [| 
            for ordr in orders do
             for genMode in switchGenModes do
                    SorterSetSelfAppendCfg.create
                        ordr
                        rngGen1
                        genMode
                        (SwitchCount.orderTo999SwitchCount ordr)
                        (sorterCounts ordr)
        |]



    ////********  sorterSetRnd_EvalAllBitsCfg  ****************

    let allSorterSetRnd_EvalAllBitsCfg () =
         [|
            for ordr in orders do
             for genMode in switchGenModes do
                 for mutRate in mutRates do
                    SorterSetRnd_EvalAllBitsCfg.create
                        ordr
                        rngGen1
                        genMode
                        (SwitchCount.orderTo999SwitchCount ordr)
                        (sorterCounts ordr)
                        sorterEvalMode.DontCheckSuccess
                        (1 |> StageCount.create)
         |]




    ////********  SsMfr_EvalAllBitsCfg  ****************

    let allSsMfr_EvalAllBitsCfg () =
         [|
            for ordr in orders do
             for genMode in switchGenModes do
                 for mutRate in mutRates do
                    SsMfr_EvalAllBitsCfg.create
                        ordr
                        rngGen1
                        genMode
                        (SwitchCount.orderTo999SwitchCount ordr)
                        (sorterCounts ordr)
                        rngGen2
                        (sorterCounts ordr)
                        mutRate
                        sorterEvalMode.DontCheckSuccess
                        (1 |> StageCount.create)
         |]



    ////********  SsAfr_EvalAllBitsCfg  ****************

    let allSsAfr_EvalAllBitsCfg () =
         [|
            for ordr in orders do
             for genMode in switchGenModes do
                    SsAfr_EvalAllBitsCfg.create
                        ordr
                        rngGen1
                        genMode
                        (StageCount.toSwitchCount ordr (1 |> StageCount.create ))
                        (sorterCounts ordr)
                        sorterEvalMode.GetSortedSetCount
                        (0 |> StageCount.create)
         |]





    ////********  sorterSetRnd_EvalAllBits_ReportCfg  ****************

    let allSorterSetEvalReportCfgs () =
        [| 
            for ordr in orders do
              SorterSetRnd_EvalAllBits_ReportCfg.create
                [|ordr|]
                rngGensCreate
                switchGenModes
                SwitchCount.orderTo999SwitchCount
                sorterCounts
                sorterEvalMode.DontCheckSuccess
                (1 |> StageCount.create)
                sorterEvalReport.Full
                (sprintf "%s_%d" "Jennifer" (ordr |> Order.value))
        |]




    ////********  ssMfr_EvalAllBits_ReportCfg  ****************

    let allssMfrEvalReportCfgs () =
        [| 
            for ordr in orders do
                for mutR in mutRates do
                  SsMfr_EvalAllBits_ReportCfg.create
                    [|ordr|]
                    rngGensCreate
                    switchGenModes
                    SwitchCount.orderTo999SwitchCount
                    sorterCounts
                    rngGensMutate
                    sorterCounts
                    [|mutR|]
                    sorterEvalMode.DontCheckSuccess
                    (1 |> StageCount.create)
                    sorterEvalReport.Full
                    (sprintf "%s_%d" "Jennifer" (ordr |> Order.value))
        |]


    ////********  ssAfr_EvalAllBits_ReportCfg  ****************

    let allssAfrEvalReportCfgs () =
        [| 
            for ordr in orders do
                  SsAfr_EvalAllBits_ReportCfg.create
                    [|ordr|]
                    rngGensCreate
                    switchGenModes
                    (fun ordr -> 2 |> StageCount.create |> StageCount.toSwitchCount ordr)
                    sorterCounts
                    sorterEvalMode.GetSortedSetCount
                    (1 |> StageCount.create)
                    sorterEvalReport.Full
                    (sprintf "%s_%d" "Jennifer" (ordr |> Order.value))
        |]




    ////********  ssMfr_EvalAllBitsMerge_ReportCfg  ****************

    let allssMfrEvalMergeReportCfgs () =
        [| 
            for ordr in orders do
                for mutR in mutRates do
                  SsMfr_EvalAllBitsMerge_ReportCfg.create
                    [|ordr|]
                    rngGensCreate
                    switchGenModes
                    SwitchCount.orderTo999SwitchCount
                    sorterCounts
                    rngGensMutate
                    sorterCounts2
                    [|mutR|]
                    sorterEvalMode.DontCheckSuccess
                    (1 |> StageCount.create)
                    sorterEvalReport.Full
                    (sprintf "%s_%d" "Jennifer" (ordr |> Order.value))
        |]