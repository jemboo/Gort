namespace global
open System


module WsOps = 


    //********  SortableSet ****************

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
            let! sortableSet = SortableSetCfg.makeSortableSet cfg
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
        WsFile.writeToFile wsFile.SorterSetMap
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
            (cfg:sorterSetMutatedFromRndCfg) 
          =
          result {
            let! txtD = 
                WsFile.readAllText  
                    wsFile.SorterSetMap
                    (cfg |> SorterSetMutatedFromRndCfg.getParentMapFileName)
            return! txtD |> SorterParentMapDto.fromJson
          }


    let makeMutantSorterSet
            (cfg:sorterSetMutatedFromRndCfg) 
        =
        result {
            let! mutantSet = 
                    SorterSetMutatedFromRndCfg.makeMutantSorterSet
                        getSorterSet
                        cfg

            let! resSs = mutantSet |> saveMutatedSorterSet cfg
            return mutantSet
        }
        

    let getMutantSorterSet
            (cfg:sorterSetMutatedFromRndCfg) 
        =
        result {
            let loadRes  = 
                result {
                    let! mut = loadMutatedSorterSet cfg
                    return mut
                }

            match loadRes with
            | Ok mut -> return mut
            | Error _ -> return! (makeMutantSorterSet cfg)
        }


    let makeEm () =

        WsCfgs.allSortableSetCfgs ()
        |> Array.map(getSortableSet)

        //WsCfgs.allSorterSetMutateCfgs ()
        //|> Array.map(getMutantSorterSetAndParentMap)
        //WsCfgs.allSorterSetRndCfgs ()
        //|> Array.map(getSorterSet)