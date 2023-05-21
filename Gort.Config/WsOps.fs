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



    //********  SorterSetMutate  ****************

    let saveMutatedSorterSet
            (cfg:sorterSetMutatedFromRndCfg)
            (sst:sorterSet) 
        =
        WsFile.writeToFile wsFile.SorterSet
            (cfg |> SorterSetMutatedFromRndCfg.getMutatedSorterSetFileName) 
            (sst |> SorterSetDto.toJson)


    let loadMutatedSorterSet 
            (cfg:sorterSetMutatedFromRndCfg) =
          result {
            let! txtD = 
                WsFile.readAllText  
                    wsFile.SorterSet
                    (cfg |> SorterSetMutatedFromRndCfg.getMutatedSorterSetFileName)
            return! txtD |> SorterSetDto.fromJson
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

          
    //********  ParentMap  ****************
    
    let saveSorterParentMap
            (cfg:sorterSetMutatedFromRndCfg)
            (sst:sorterParentMap) 
        =
        WsFile.writeToFile wsFile.SorterSetMap
            (cfg |> SorterSetMutatedFromRndCfg.getParentMapFileName) 
            (sst |> SorterParentMapDto.toJson)



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


    let makeSorterSetParentMap
            (cfg:sorterSetMutatedFromRndCfg) 
        =
        result {
            let parentMap = 
                    SorterSetMutatedFromRndCfg.makeSorterSetParentMap
                        cfg

            let! resSs = parentMap |> saveSorterParentMap cfg
            return parentMap
        }


    let getParentMap
            (cfg:sorterSetMutatedFromRndCfg) 
        =
        result {
            let loadRes  = 
                result {
                    let! mut = loadSorterSetParentMap cfg
                    return mut
                }

            match loadRes with
            | Ok mut -> return mut
            | Error _ -> return! (makeSorterSetParentMap cfg)
        }

        
    

    //********  SorterSetMutate and ParentMap  ****************

    let getMutantSorterSetAndParentMap
            (cfg:sorterSetMutatedFromRndCfg) 
        =
        result {
            let! mutantSet = getMutantSorterSet cfg
            let! parentMap = getParentMap cfg

            return mutantSet, parentMap
        }



    //********  ssmfr_EvalAllBitsCfg  ****************

    let saveSorterSetEval
            (cfg:ssmfr_EvalAllBitsCfg)
            (sst:sorterSetEval) 
        =
        let fileName = cfg |> Ssmfr_EvalAllBitsCfg.getSorterSetEvalFileName 
        WsFile.writeToFile 
            wsFile.SorterSetEval 
            fileName 
            (sst |> SorterSetEvalDto.toJson )


    let loadSsmfr_EvalAllBitsCfg
            (cfg:ssmfr_EvalAllBitsCfg) 
          =
          result {
            let! txtD = 
                WsFile.readAllText  
                    wsFile.SorterSetEval
                    (cfg |> Ssmfr_EvalAllBitsCfg.getSorterSetEvalFileName)
            return! txtD |> SorterSetEvalDto.fromJson
          }



    let makeSsmfr_EvalAllBitsCfg
            (cfg:ssmfr_EvalAllBitsCfg) 
        =
        result {
            let! ssEval = 
                    Ssmfr_EvalAllBitsCfg.makeSorterSetEval
                        WsCfgs.useParall
                        cfg
                        getSortableSet
                        getMutantSorterSet

            let! resSs = ssEval |> saveSorterSetEval cfg
            return ssEval
        }


    let getSsmfr_EvalAllBits
            (cfg:ssmfr_EvalAllBitsCfg) 
        =
        result {
            let loadRes  = 
                result {
                    let! ssEval = loadSsmfr_EvalAllBitsCfg cfg
                    return ssEval
                }

            match loadRes with
            | Ok ssEval -> return ssEval
            | Error _ -> return! (makeSsmfr_EvalAllBitsCfg cfg)
        }





    let makeEm () =

        //WsCfgs.allSortableSetCfgs ()
        //|> Array.map(getSortableSet)


        WsCfgs.allSsmfr_EvalAllBitsCfg ()
        |> Array.map(getSsmfr_EvalAllBits)

        

        //WsCfgs.allSorterSetMutatedFromRndCfgs ()
        //|> Array.map(getMutantSorterSetAndParentMap)
        //WsCfgs.allSorterSetRndCfgs ()
        //|> Array.map(getSorterSet)