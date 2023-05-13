namespace global
open System


module WsMutateSorterSets = 

    let saveSorterSet
            (cfg:sorterSetCfg)
            (sst:sorterSet) 
        =
        WsFile.writeToFile wsFile.SorterSet
            (cfg |> SorterSetCfg.getFileName) 
            (sst |> SorterSetDto.toJson)


    let saveSorterSetParentMap
            (fileName:string)
            (sst:sorterParentMap) 
        =
        WsFile.writeToFile wsFile.SorterSetMap
            fileName
            (sst |> SorterParentMapDto.toJson)


    let loadSorterSet (cfg:sorterSetCfg) =
          result {
            let! txtD = WsFile.readAllText  wsFile.SorterSet
                            (cfg |> SorterSetCfg.getFileName)
            return! txtD |> SorterSetDto.fromJson
          }


    let makeSorterSet 
            (cfg) 
        =
        result {
            let sorterSet = SorterSetCfg.makeSorterSet cfg
            let! res = sorterSet 
                       |> saveSorterSet cfg
            return sorterSet
        }


    let getSorterSet (cfg:sorterSetCfg) =
        result {
            let loadRes = loadSorterSet cfg
            match loadRes with
            | Ok ss -> return ss
            | Error _ -> return! (makeSorterSet cfg)
        }


    let createMutantSorterSetAndParentMap 
            (mCfg: sorterSetMutateCfg)
        =
        let res = 
            result {
        
                let! parentMap, mutantSet = 
                    mCfg 
                    |> SorterSetMutateCfg.createMutantSorterSetAndParentMap
                            getSorterSet

                let mutantCfg = 
                    SorterSetCfgExplicit.create
                        (mutantSet |> SorterSet.getOrder|> Some)
                        (mutantSet |> SorterSet.getId)
                        "description"
                        (mutantSet |> SorterSet.getSorterCount |> Some)
                    |> sorterSetCfg.Explicit


                let! resPm = 
                    saveSorterSetParentMap
                        (parentMap |> SorterSetMutateCfg.getParentMapFileName )
                        parentMap

                let! resMs = 
                    saveSorterSet
                        mutantCfg
                        mutantSet


                return ( mutantSet |> SorterSet.getId |> SorterSetId.value |> string,
                         mutantSet |> SorterSet.getOrder |> Order.value |> string)
            }
        let yab = res |> Result.bimap
                            (fun (s, o) -> Console.WriteLine (sprintf "%s\t%s" s o))
                            (fun s -> Console.WriteLine s)
                
        () //res
        


    let makeEm () =  //(errLogger: string -> unit) =
        let allcfgs = WsCfgs.allSorterSetMutateCfgs ()
        WsCfgs.allSorterSetMutateCfgs ()
        |> Array.map(createMutantSorterSetAndParentMap)
