namespace global
open System


module WsBinarySortableSets = 

    let localFolder = "BinarySortableSets"

    let appendLines (fileName:string) (data: string seq) =
        WsCommon.appendLines localFolder fileName data

    let writeToFile (fileName:string) (data: string) =
        WsCommon.writeToFile localFolder fileName data

    let readAllText (fileName:string) =
        WsCommon.readAllText localFolder fileName

    let readAllLines (fileName:string) =
        WsCommon.readAllLines localFolder fileName


    let allCfgs () =
        [| 
          for ordr in WsCommon.orders do
            SortableSetCfgCertain.getStandardSwitchReducedOneStage ordr
            |> sortableSetCfg.Certain

          for ordr in WsCommon.orders do
            sortableSetCfgCertain.All_Bits ordr
            |> sortableSetCfg.Certain
        |]


    let saveSortableSet 
            (cfg:sortableSetCfg)
            (sst:sortableSet) 
        =
        let fileName = cfg |> SortableSetCfg.getFileName 
        writeToFile fileName (sst |> SortableSetDto.toJson )


    let getSortableSet 
            (cfg:sortableSetCfg) 
        =
        match cfg with
        | Certain cfgCert ->
            let sortableSetFileName =
                    (cfgCert |> SortableSetCfgCertain.getFileName)
            try
                let allText = readAllText sortableSetFileName |> Result.ExtractOrThrow
                let sortableSet = allText |> SortableSetDto.fromJson
                sortableSet
            with ex ->
                ("error in WsBinarySortableSets.getSortableSet: " + ex.Message) |> Error



    let makeSortableSet (cfg) =
        let sortableSetR = SortableSetCfg.getSortableSet cfg

        let sortableSet = sortableSetR
                            |> Result.ExtractOrThrow

        let res = sortableSet |> saveSortableSet cfg
        ()


    let makeEm () =
        allCfgs ()
        |> Array.map(makeSortableSet)
