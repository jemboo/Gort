namespace global
open System


module WsSorterSetEval 
        =

    let getSortableSet (cfg:sortableSetCfg)
        =
        WsSortableSets.getSortableSet cfg


    let getSorterSet (cfg:sorterSetCfg)
        =
        WsSorterSets.getSorterSet cfg


    let saveSorterSetEval
            (cfg:sorterSetEvalCfg)
            (sst:sorterSetEval) 
        =
        let fileName = cfg |> SorterSetEvalCfg.getFileName 
        WsFile.writeToFile wsFile.SorterSetEval fileName (sst |> SorterSetEvalDto.toJson )


    let loadSorterSetEval (cfg:sorterSetEvalCfg) =
          result {
            let! txtD = WsFile.readAllText  wsFile.SorterSetEval
                            (cfg |> SorterSetEvalCfg.getFileName)
            return! txtD |> SorterSetEvalDto.fromJson
          }


    let makeSorterSetEval (cfg:sorterSetEvalCfg) =
        result {
            let! sorterSetEval =
                    SorterSetEvalCfg.makeSorterSetEval
                        WsCfgs.useParall
                        cfg
                        getSortableSet
                        getSorterSet

            let res = sorterSetEval 
                        |> saveSorterSetEval cfg
                        |> Result.map(ignore)
            return sorterSetEval
        }


    let getSorterSetEval 
            (cfg:sorterSetEvalCfg) 
        =
        result {
            let loadRes = loadSorterSetEval cfg
            match loadRes with
            | Ok ss -> return ss
            | Error _ -> return! (makeSorterSetEval cfg)
        }


    let makeEm () =
        WsCfgs.allSorterSetEvalCfgs ()
        |> Array.map(getSorterSetEval)