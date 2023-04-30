namespace global
open System


module WsBinarySortableSets = 

    let binaryFolder = "BinarySortableSets"

    let appendLines (fileName:string) (data: string seq) =
        TextIO.appendLines "txt" (Some WsCommon.wsRootDir) binaryFolder fileName data

    let writeToFile (fileName:string) (data: string) =
        TextIO.writeToFile "txt" (Some WsCommon.wsRootDir) binaryFolder fileName data

    let readAllText (fileName:string) =
        TextIO.readAllText "txt" (Some WsCommon.wsRootDir) binaryFolder fileName

    let orders = [|16;18;|] |> Array.map(Order.createNr)

    let allCfgs () =
        [| 
          for ordr in orders do
            SortableSetCfgCertain.getStandardSwitchReducedOneStage ordr
            |> sortableSetCfg.Certain

          for ordr in orders do
            sortableSetCfgCertain.All_Bits ordr
            |> sortableSetCfg.Certain
        |]


    let getSortableSetId
            (ordr:order)
            (usePfx:bool)
        =
        if usePfx then
            SortableSetCfgCertain.getStandardSwitchReducedOneStage ordr
            |> SortableSetCfgCertain.getSortableSetId
        else
            sortableSetCfgCertain.All_Bits ordr
            |> SortableSetCfgCertain.getSortableSetId


    let fileNameFromSortableSet 
            (sst:sortableSet) 
        =
        sst |> SortableSet.getSortableSetId 
            |> SortableSetId.value 
            |> string
            |> FileName.create


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



    let runConfig (cfg) =
        let sortableSetR = SortableSetCfg.getSortableSet cfg

        let sortableSet = sortableSetR
                            |> Result.ExtractOrThrow

        let res = sortableSet |> saveSortableSet cfg
        ()


    let makeEm () =
        allCfgs ()
        |> Array.map(runConfig)
