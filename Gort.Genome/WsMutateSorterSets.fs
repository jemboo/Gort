namespace global
open System


module WsMutateSorterSets = 

    let localFolder = "StandardSorterSets"

    let appendLines (fileName:string) (data: string seq) =
        WsCommon.appendLines localFolder fileName data

    let writeToFile (fileName:string) (data: string) =
        WsCommon.writeToFile localFolder fileName data

    let readAllText (fileName:string) =
        WsCommon.readAllText localFolder fileName

    let readAllLines (fileName:string) =
        WsCommon.readAllLines localFolder fileName


    let saveSorterSet
            (cfg:sorterSetCfg)
            (sst:sorterSet) 
        =
        writeToFile 
            (cfg |> SorterSetCfg.getFileName) 
            (sst |> SorterSetDto.toJson)


    let saveSorterSetParentMap
            (fileName:string)
            (sst:sorterParentMap) 
        =
        writeToFile 
            fileName
            (sst |> SorterParentMapDto.toJson)


    let loadSorterSet (cfg:sorterSetCfg) =
          result {
            let! txtD = readAllText  
                            (cfg |> SorterSetCfg.getFileName)
            return! txtD |> SorterSetDto.fromJson
          }


    let makeSorterSet 
            (cfg) 
        =
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


    let createMutantSorterSetAndParentMap 
            (mCfg: mutateSorterSetCfg)
        =
        let res = 
            result {
        
                let! parentMap, mutantSet = 
                    mCfg 
                    |> MutateSorterSetCfg.createMutantSorterSetAndParentMap
                            getSorterSet

                let mutantCfg = 
                    SorterSetCfgExplicit.create
                        (mutantSet |> SorterSet.getOrder)
                        (mutantSet |> SorterSet.getId)
                        "description"
                        (mutantSet |> SorterSet.getSorterCount)
                    |> sorterSetCfg.Explicit


                let! resPm = 
                    saveSorterSetParentMap
                        (parentMap |> MutateSorterSetCfg.getParentMapFileName )
                        parentMap

                let! resMs = 
                    saveSorterSet
                        mutantCfg
                        mutantSet


                return ()
            }
        ()
        


    let makeEm (errLogger: string -> unit) =
        WsCommon.allDenovoSorterSetCfgs ()
        |> Array.map(getSorterSet)
