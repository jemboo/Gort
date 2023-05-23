﻿namespace global
open System


module WsCfgs = 

    let useParall = true |> UseParallel.create

    let rngGen1 = RngGen.createLcg (12544 |> RandomSeed.create)
    let rngGen2 = RngGen.createLcg (72574 |> RandomSeed.create)
    let rngGen3 = RngGen.createLcg (82584 |> RandomSeed.create)
    
    let rngGensCreate = [|rngGen1; rngGen2; rngGen3;|]
    let rngGensMutate = [|rngGen1; rngGen2; |]


    let orders = [|14;|] |> Array.map(Order.createNr)
    
    let mutRates = [| 0.02; 0.05; 0.1; 0.2;|] 
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
        | 14 -> sorterCountBase * 10 |> SorterCount.create
        | 16 -> sorterCountBase * 5  |> SorterCount.create
        | 18 -> sorterCountBase * 1  |> SorterCount.create
        | 20 -> sorterCountBase * 25   |> SorterCount.create
        | 22 -> sorterCountBase * 5    |> SorterCount.create
        | 24 -> sorterCountBase        |> SorterCount.create
        | _ -> failwith "not handled"


    let sorterCountBase2 = 20

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




    ////********  Ssmfr_EvalAllBitsCfg  ****************

    let allSsmfr_EvalAllBitsCfg () =
         [|
            for ordr in orders do
             for genMode in switchGenModes do
                 for mutRate in mutRates do
                    Ssmfr_EvalAllBitsCfg.create
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




    ////********  ssmfr_EvalAllBits_ReportCfg  ****************

    let allssmfrEvalReportCfgs () =
        [| 
            for ordr in orders do
                for mutR in mutRates do
                  Ssmfr_EvalAllBits_ReportCfg.create
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





    ////********  ssmfr_EvalAllBitsMerge_ReportCfg  ****************

    let allssmfrEvalMergeReportCfgs () =
        [| 
            for ordr in orders do
                for mutR in mutRates do
                  Ssmfr_EvalAllBitsMerge_ReportCfg.create
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