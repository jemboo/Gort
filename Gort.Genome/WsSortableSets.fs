namespace global
open System


module WsBinarySortableSets = 

    let binaryFolder = "BinarySortableSets"

    let writeData (fileName:string) (data: string seq) =
        TextIO.write "txt" (Some WsCommon.wsRootDir) binaryFolder fileName data

    let readData (fileName:string) =
        TextIO.read "txt" (Some WsCommon.wsRootDir) binaryFolder fileName

    let orders = [|16;18;|] |> Array.map(Order.createNr)

    let allCfgs () =
        [| 
          for ordr in orders do
            SortableSetCfgCertain.getStandardSwitchReducedOneStage ordr

          for ordr in orders do
            sortableSetCfgCertain.All_Bits ordr
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
        sst |> SortableSet.getSortableSetId |> SortableSetId.value 
            |> string |> FileName.create


    let saveSortableSet 
            (cfg:sortableSetCfgCertain)
            (sst:sortableSet) 
        =
        let fileName = cfg |> SortableSetCfgCertain.getFileName 

        let jsns = seq { sst |> SortableSetDto.toJson }
        writeData fileName jsns


    let runConfig (cfg) =
        let sortableSetR = SortableSetCfgCertain.getSortableSet cfg

        let sortableSet = sortableSetR
                            |> Result.ExtractOrThrow

        let res = sortableSet |> saveSortableSet cfg
        ()


    let makeEm () =
        allCfgs ()
        |> Array.map(runConfig)
