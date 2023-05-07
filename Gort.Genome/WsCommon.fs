namespace global
open System


module WsCommon = 

    let useParall = true |> UseParallel.create

    let wsRootDir = "c:\\GortFiles2"
    let fileExt = "txt"


    let appendLines (localFolder:string) (fileName:string) (data: string seq) =
        TextIO.appendLines "txt" (Some wsRootDir) localFolder fileName data

    let writeToFile (localFolder:string)  (fileName:string) (data: string) =
        TextIO.writeToFile "txt" (Some wsRootDir) localFolder fileName data

    let readAllText (localFolder:string)  (fileName:string) =
        TextIO.readAllText "txt" (Some wsRootDir) localFolder fileName

    let readAllLines (localFolder:string)  (fileName:string) =
        TextIO.readAllLines "txt" (Some wsRootDir) localFolder fileName


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
            SortableSetCfgCertain.getStandardSwitchReducedOneStage ordr
            |> sortableSetCfg.Certain

          //for ordr in orders do
          //  sortableSetCfgCertain.All_Bits ordr
          //  |> sortableSetCfg.Certain
        |]



    //********  SorterSet  ****************

    let sorterCountBase = 2

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


    let allSorterSetCfgs () =
        [| for ordr in orders do
             for genMode in switchGenModes do
                 for usePfx in [false] do
                    makeRdnDenovoSorterSetCfg 
                        genMode
                        ordr
                        rngGen1
                        usePfx
                    |> sorterSetCfg.RndDenovo
        |]



    //********  SorterSetEval  ****************



    let allSorterSetEvalCfgs () =
        [| 
           for ordr in orders do
              for genMode in switchGenModes do
                 let sortableSetCfg =
                      SortableSetCfgCertain.getStandardSwitchReducedOneStage 
                            ordr
                      |> sortableSetCfg.Certain

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


