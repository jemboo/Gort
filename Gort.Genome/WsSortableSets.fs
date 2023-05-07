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

    let saveSortableSet 
            (cfg:sortableSetCfg)
            (sst:sortableSet) 
        =
        let fileName = cfg |> SortableSetCfg.getFileName 
        writeToFile fileName (sst |> SortableSetDto.toJson )


    let loadSortableSet (cfg:sortableSetCfg) =
          result {
            let! txtD = readAllText  
                            (cfg |> SortableSetCfg.getFileName)
            return! txtD |> SortableSetDto.fromJson
          }


    let makeSortableSet (cfg:sortableSetCfg) =
        result {
            let! sortableSet = SortableSetCfg.getSortableSet cfg
            let res = sortableSet 
                        |> saveSortableSet cfg
                        |> Result.map(ignore)
            return sortableSet
        }


    let getSortableSet (cfg:sortableSetCfg) =
        result {
            let loadRes = loadSortableSet cfg
            match loadRes with
            | Ok ss -> return ss
            | Error _ -> return! (makeSortableSet cfg)
        }


    let makeEm () =
        WsCommon.allSortableSetCfgs ()
        |> Array.map(getSortableSet)
