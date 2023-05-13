namespace global
open System


module WsSorterSets = 

    let saveSorterSet
            (cfg:sorterSetCfg)
            (sst:sorterSet) 
        =
        WsFile.writeToFile wsFile.SorterSet
            (cfg |> SorterSetCfg.getFileName) 
            (sst |> SorterSetDto.toJson)


    let loadSorterSet (cfg:sorterSetCfg) =
          result {
            let! txtD = WsFile.readAllText  wsFile.SorterSet
                            (cfg |> SorterSetCfg.getFileName)
            return! txtD |> SorterSetDto.fromJson
          }


    let makeSorterSet (cfg) =
        result {
            let sorterSet = SorterSetCfg.makeSorterSet cfg
            let! res = sorterSet |> saveSorterSet cfg
            return sorterSet
        }


    let getSorterSet (cfg:sorterSetCfg) =
        result {
            let loadRes = loadSorterSet cfg
            match loadRes with
            | Ok ss -> return ss
            | Error _ -> return! (makeSorterSet cfg)
        }


    let makeEm () =
        WsCfgs.allSorterSetCfgs ()
        |> Array.map(getSorterSet)
