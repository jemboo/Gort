namespace global
      
type sorterSetCfg = 
     | Rnd of sorterSetRndCfg
     | RndMutated of sorterSetMutatedFromRndCfg


module SorterSetCfg =

    let getProperties 
            (ssCfg: sorterSetCfg) 
        = 
        match ssCfg with
        | Rnd rdssCfg -> 
            rdssCfg |> SorterSetRndCfg.getProperties
        | RndMutated cfg -> 
            cfg |> SorterSetMutatedFromRndCfg.getProperties


    let getId 
            (ssCfg: sorterSetCfg) 
        = 
        match ssCfg with
        | Rnd rdssCfg -> 
            rdssCfg |> SorterSetRndCfg.getId
        | RndMutated cfg -> 
            cfg |> SorterSetMutatedFromRndCfg.getId

    let getOrder
            (ssCfg: sorterSetCfg) 
        = 
        match ssCfg with
        | Rnd cCfg -> 
            cCfg |> SorterSetRndCfg.getOrder
        | RndMutated cfg -> 
            cfg |> SorterSetMutatedFromRndCfg.getOrder


    let getSorterSetCt
            (ssCfg: sorterSetCfg) 
        = 
        match ssCfg with
        | Rnd cCfg -> 
            cCfg |> SorterSetRndCfg.getSorterCount
        | RndMutated cfg -> 
            cfg |> SorterSetMutatedFromRndCfg.getSorterCountMutated



    let getCfgName
            (ssCfg: sorterSetCfg) 
        = 
        match ssCfg with
        | Rnd cCfg -> 
            cCfg |> SorterSetRndCfg.getConfigName
        | RndMutated cfg -> 
            cfg |> SorterSetMutatedFromRndCfg.getConfigName


    let getFileName
            (ssCfg: sorterSetCfg) 
        = 
        match ssCfg with
        | Rnd cCfg -> 
            cCfg |> SorterSetRndCfg.getFileName
        | RndMutated cfg -> 
            cfg |> SorterSetMutatedFromRndCfg.getFileName


    let rec getSorterSet
            (save: string -> sorterSet -> Result<bool, string>)
            (sorterSetLookup: string -> Result<sorterSet, string>)
            (getParentMap: sorterSetParentMapCfg -> Result<sorterSetParentMap, string>)
            (ssCfg: sorterSetCfg) 
        = 
        match ssCfg with
        | Rnd cCfg -> 
            cCfg |> SorterSetRndCfg.getSorterSet sorterSetLookup save
        | RndMutated cfg -> 
            result {
                let parentCfg = 
                        cfg |> SorterSetMutatedFromRndCfg.getSorterSetParentCfg
                            |> sorterSetCfg.Rnd

                let! parentSorterSet = getSorterSet save sorterSetLookup getParentMap parentCfg
                let! parentMap = ( cfg 
                                   |> SorterSetMutatedFromRndCfg.getSorterSetParentMapCfg
                                   |> getParentMap )

                let mutantSorterSetFileName = (cfg |> SorterSetMutatedFromRndCfg.getFileName)
                let mutantSorterSetFinding = sorterSetLookup mutantSorterSetFileName

                match mutantSorterSetFinding with
                | Ok mutantSS -> return mutantSS
                | Error _ -> 
                    let! mutantSorterSet = 
                            parentSorterSet |>
                                SorterSetMutator.createMutantSorterSetFromParentMap
                                    parentMap
                                    (cfg |> SorterSetMutatedFromRndCfg.getSorterSetMutator)
                    let! isSaved = save mutantSorterSetFileName  mutantSorterSet
                    return mutantSorterSet
            }

