namespace global
open System


module WsSorterSetMutate = 

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

    let saveSorterSetParentMapAsCsv
            (sst:sorterParentMap) 
        =
        let fileName = 
            sprintf "%s_%s"
                (sst |> SorterParentMap.getParentSorterSetId
                     |> SorterSetId.value |> string)
                (sst |> SorterParentMap.getMutantSorterSetId
                     |> SorterSetId.value |> string)
                     
        WsFile.appendLines wsFile.SorterSetMap
            fileName
            ["sorter_id\tparent_id"]
            |> ignore

        WsFile.appendLines wsFile.SorterSetMap
            fileName
            (sst |> SorterParentMap.getParentMap 
                 |> Map.toSeq
                 |> Seq.map(fun (s, p) -> 
                        sprintf "%s\t%s" 
                            (s |> SorterId.value |> string) 
                            (p |> SorterParentId.value |> string) ))
            |> ignore


    let loadSorterSetParentMap
            (fileName:string)
        =
        result {
            let! txtD = WsFile.readAllText  wsFile.SorterSetMap
                            fileName
            return! txtD |> SorterParentMapDto.fromJson
        }


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
                
                let parentSet = 
                    mCfg
                    |> SorterSetMutateCfg.getSorterSetCfgParent


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
                         mutantSet |> SorterSet.getOrder |> Order.value |> string,
                         parentSet |> SorterSetCfg.getSorterSetCt |> SorterCount.value  )
            }
        let yab = res |> Result.bimap
                            (fun (s, o, ct) -> Console.WriteLine (sprintf "%s\t%s\t%d" s o ct))
                            (fun s -> Console.WriteLine s)
                
        () //res
        


    let makeEm () =  //(errLogger: string -> unit) =
        let mapFileName = "b3751ad4-81f7-d262-11a8-464a757e3495"

        let pMapR = loadSorterSetParentMap mapFileName 
                    
        let pMap = pMapR |> Result.ExtractOrThrow
        saveSorterSetParentMapAsCsv pMap
        ()
        //let allcfgs = WsCfgs.allSorterSetMutateCfgs ()
        //WsCfgs.allSorterSetMutateCfgs ()
        //|> Array.map(createMutantSorterSetAndParentMap)
