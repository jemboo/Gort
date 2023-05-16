namespace global
open System


module WsSorterSetsRnd = 



    //********  SortableSet ****************

    let saveSortableSet 
            (cfg:sortableSetCfgCertain)
            (sst:sortableSet) 
        =
        let fileName = cfg |> SortableSetCfgCertain.getFileName 
        WsFile.writeToFile wsFile.SortableSet fileName (sst |> SortableSetDto.toJson )


    let loadSortableSet (cfg:sortableSetCfgCertain) =
          result {
            let! txtD = WsFile.readAllText  wsFile.SortableSet
                            (cfg |> SortableSetCfgCertain.getFileName)
            return! txtD |> SortableSetDto.fromJson
          }


    let makeSortableSet (cfg:sortableSetCfgCertain) =
        result {
            let! sortableSet = SortableSetCfgCertain.makeSortableSet cfg
            let res = sortableSet 
                        |> saveSortableSet cfg
                        |> Result.map(ignore)
            return sortableSet
        }


    let getSortableSet (cfg:sortableSetCfgCertain) =
        result {
            let loadRes = loadSortableSet cfg
            match loadRes with
            | Ok ss -> return ss
            | Error _ -> return! (makeSortableSet cfg)
        }


    //********  SorterSet ****************

    let saveSorterSet
            (cfg:sorterSetRndCfg)
            (sst:sorterSet) 
        =
        WsFile.writeToFile wsFile.SorterSet
            (cfg |> SorterSetRndCfg.getFileName) 
            (sst |> SorterSetDto.toJson)


    let loadSorterSet (cfg:sorterSetRndCfg) =
          result {
            let! txtD = WsFile.readAllText  wsFile.SorterSet
                            (cfg |> SorterSetRndCfg.getFileName)
            return! txtD |> SorterSetDto.fromJson
          }


    let makeSorterSet (cfg) =
        result {
            let sorterSet = SorterSetRndCfg.makeSorterSet cfg
            let! res = sorterSet |> saveSorterSet cfg
            return sorterSet
        }


    let getSorterSet (cfg:sorterSetRndCfg) =
        result {
            let loadRes = loadSorterSet cfg
            match loadRes with
            | Ok ss -> return ss
            | Error _ -> return! (makeSorterSet cfg)
        }



    //********  SorterSetMutate and ParentMap  ****************

    let saveMutatedSorterSet
            (cfg:sorterSetMutatedFromRndCfg)
            (sst:sorterSet) 
        =
        WsFile.writeToFile wsFile.SorterSet
            (cfg |> SorterSetMutatedFromRndCfg.getMutatedSorterSetFileName) 
            (sst |> SorterSetDto.toJson)


    let saveSorterParentMap
            (cfg:sorterSetMutatedFromRndCfg)
            (sst:sorterParentMap) 
        =
        WsFile.writeToFile wsFile.SorterSet
            (cfg |> SorterSetMutatedFromRndCfg.getParentMapFileName) 
            (sst |> SorterParentMapDto.toJson)


    let loadMutatedSorterSet 
            (cfg:sorterSetMutatedFromRndCfg) =
          result {
            let! txtD = 
                WsFile.readAllText  
                    wsFile.SorterSet
                    (cfg |> SorterSetMutatedFromRndCfg.getMutatedSorterSetFileName)
            return! txtD |> SorterSetDto.fromJson
          }


    let loadSorterSetParentMap
            (cfg:sorterSetMutatedFromRndCfg) =
          result {
            let! txtD = 
                WsFile.readAllText  
                    wsFile.SorterSet
                    (cfg |> SorterSetMutatedFromRndCfg.getParentMapFileName)
            return! txtD |> SorterParentMapDto.fromJson
          }


    let makeMutantSorterSetAndParentMap (cfg:sorterSetMutatedFromRndCfg) =
        result {
            let! parentMap, mutantSet = 
                SorterSetMutatedFromRndCfg.makeMutantSorterSetAndParentMap 
                    getSorterSet
                    cfg

            let! resSs = mutantSet |> saveMutatedSorterSet cfg
            let! resPm = parentMap |> saveSorterParentMap cfg
            return parentMap, mutantSet
        }
        

    let getMutantSorterSetAndParentMap 
            (cfg:sorterSetMutatedFromRndCfg) =
        result {
            let loadRes  = 
                result {
                    let! mut = loadMutatedSorterSet cfg
                    let! map = loadSorterSetParentMap cfg
                    return map, mut
                }

            match loadRes with
            | Ok (map, mut) -> return map, mut
            | Error _ -> return! (makeMutantSorterSetAndParentMap cfg)
        }


    let makeEm () =

        WsCfgs.allSortableSetCfgs ()
        |> Array.map(getSortableSet)

        //WsCfgs.allSorterSetMutateCfgs ()
        //|> Array.map(getMutantSorterSetAndParentMap)
        //WsCfgs.allSorterSetRndCfgs ()
        //|> Array.map(getSorterSet)