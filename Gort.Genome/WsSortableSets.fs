namespace global
open System


module WsSortableSets = 

    let saveSortableSet 
            (cfg:sortableSetCfg)
            (sst:sortableSet) 
        =
        let fileName = cfg |> SortableSetCfg.getFileName 
        WsFile.writeToFile wsFile.SortableSet fileName (sst |> SortableSetDto.toJson )


    let loadSortableSet (cfg:sortableSetCfg) =
          result {
            let! txtD = WsFile.readAllText  wsFile.SortableSet
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
        WsCfgs.allSortableSetCfgs ()
        |> Array.map(getSortableSet)
