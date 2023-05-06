namespace global
open System


module WsSorterSets = 

    let localFolder = "StandardSorterSets"

    let appendLines (fileName:string) (data: string seq) =
        WsCommon.appendLines localFolder fileName data

    let writeToFile (fileName:string) (data: string) =
        WsCommon.writeToFile localFolder fileName data

    let readAllText (fileName:string) =
        WsCommon.readAllText localFolder fileName

    let readAllLines (fileName:string) =
        WsCommon.readAllLines localFolder fileName


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
            (WsCommon.sorterCounts order)


    let allCfgs () =
        [| for ordr in WsCommon.orders do
             for genMode in WsCommon.switchGenModes do
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


    let makeConfig (cfg) =
        let sorterSet = SorterSetCfg.getSorterSet cfg
        let res = sorterSet |> saveStandardSorterSet cfg
        ()


    let makeEm () =
        allCfgs ()
        |> Array.map(makeConfig)
