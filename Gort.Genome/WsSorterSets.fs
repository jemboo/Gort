namespace global
open System


module WsSorterSets = 

    let standardFolder = "StandardSorterSets"

    let writeToFile (fileName:string) (data: string) =
        TextIO.writeToFile "txt" (Some WsCommon.wsRootDir) standardFolder fileName data

    let appendLines (fileName:string) (data: string seq) =
        TextIO.appendLines "txt" (Some WsCommon.wsRootDir) standardFolder fileName data

    let readAllText (fileName:string) =
        TextIO.readAllText "txt" (Some WsCommon.wsRootDir) standardFolder fileName

    let readAllLines (fileName:string) =
        TextIO.readAllLines "txt" (Some WsCommon.wsRootDir) standardFolder fileName


    let sorterCt1 (order:order) = 
        match (order |> Order.value) with
        | 16 -> 5000 |> SorterCount.create
        | 18 -> 1500 |> SorterCount.create
        | 20 -> 500  |> SorterCount.create
        | 22 -> 150  |> SorterCount.create
        | 24 -> 50   |> SorterCount.create
        | _ -> failwith "not handled"


    let makeRdnDenovoCfg 
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
            (sorterCt1 order)


    let orders = [|16;18|] |> Array.map(Order.createNr)
    let genModes = [switchGenMode.StageSymmetric; 
                    switchGenMode.Switch; 
                    switchGenMode.Stage]

    let allCfgs () =
        [| for ordr in orders do
             for genMode in genModes do
                 for usePfx in [true;false] do
                    makeRdnDenovoCfg 
                        genMode
                        ordr
                        WsCommon.rngGen1
                        usePfx
                    |> sorterSetCfg.RndDenovo
        |]


    let saveStandardSorterSet
            (cfg:sorterSetCfg)
            (sst:sorterSet) 
        =
        writeToFile 
            (cfg |> SorterSetCfg.getFileName) 
            (sst |> SorterSetDto.toJson)


    let getSorterSet 
            (cfg:sorterSetCfg) 
        =
        match cfg with
        | RndDenovo rdnCfg ->
            let sorterFileName = 
                    (rdnCfg |> RndDenovoSorterSetCfg.getFileName)
            try
            result {
               let! txtD = readAllText sorterFileName
               return! txtD |> SorterSetDto.fromJson
            }
            with ex ->
                ("error in WsSorterSets.getSorterSet: " + ex.Message) |> Error


    let runConfig (cfg) =
        let sorterSet = SorterSetCfg.getSorterSet cfg
        let res = sorterSet |> saveStandardSorterSet cfg
        ()


    let makeEm () =
        allCfgs ()
        |> Array.map(runConfig)
