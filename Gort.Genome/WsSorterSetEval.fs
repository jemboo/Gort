namespace global
open System


module WsSorterSetEval 
        =

    let localFolder = "Eval_Standard_On_Binary"

    let appendLines (fileName:string) (data: string seq) =
        WsCommon.appendLines localFolder fileName data

    let writeToFile (fileName:string) (data: string) =
        WsCommon.writeToFile localFolder fileName data

    let readAllText (fileName:string) =
        WsCommon.readAllText localFolder fileName

    let readAllLines (fileName:string) =
        WsCommon.readAllLines localFolder fileName


    let getSortableSet (cfg:sortableSetCfg)
        =
        WsBinarySortableSets.getSortableSet cfg


    let getSorterSet (cfg:sorterSetCfg)
        =
        WsSorterSets.getSorterSet cfg


    let saveSortableSetEval
            (cfg:sorterSetEvalCfg)
            (sst:sorterSetEval) 
        =
        let fileName = cfg |> SorterSetEvalCfg.getFileName 
        writeToFile fileName (sst |> SorterSetEvalDto.toJson )


    let loadSortableSetEval (cfg:sorterSetEvalCfg) =
          result {
            let! txtD = readAllText  
                            (cfg |> SorterSetEvalCfg.getFileName)
            return! txtD |> SorterSetEvalDto.fromJson
          }


    let makeSorterSetEval (cfg:sorterSetEvalCfg) =
        result {
            let! sorterSetEval =
                    SorterSetEvalCfg.makeSorterSetEval
                        WsCommon.useParall
                        cfg
                        getSortableSet
                        getSorterSet

            let res = sorterSetEval 
                        |> saveSortableSetEval cfg
                        |> Result.map(ignore)
            return sorterSetEval
        }


    let getSorterSetEval 
            (cfg:sorterSetEvalCfg) 
        =
        result {
            let loadRes = loadSortableSetEval cfg
            match loadRes with
            | Ok ss -> return ss
            | Error _ -> return! (makeSorterSetEval cfg)
        }


    let makeEm () =
        WsCommon.allSorterSetEvalCfgs ()
        |> Array.map(getSorterSetEval)