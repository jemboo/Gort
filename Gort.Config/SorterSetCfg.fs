namespace global
      
type sorterSetCfg = 
     | RndDenovo of sorterSetRndCfg
     | RndDenovoMutated of sorterSetMutatedFromRndCfg

module SorterSetCfg =

    let getProperties 
            (ssCfg: sorterSetCfg) 
        = 
        match ssCfg with
        | RndDenovo rdssCfg -> 
            rdssCfg |> SorterSetRndCfg.getProperties
        | RndDenovoMutated cfg -> 
            cfg |> SorterSetMutatedFromRndCfg.getProperties


    let getId 
            (ssCfg: sorterSetCfg) 
        = 
        match ssCfg with
        | RndDenovo rdssCfg -> 
            rdssCfg |> SorterSetRndCfg.getId
        | RndDenovoMutated cfg -> 
            cfg |> SorterSetMutatedFromRndCfg.getId

    let getOrder
            (ssCfg: sorterSetCfg) 
        = 
        match ssCfg with
        | RndDenovo cCfg -> 
            cCfg |> SorterSetRndCfg.getOrder
        | RndDenovoMutated cfg -> 
            cfg |> SorterSetMutatedFromRndCfg.getOrder


    let getSorterSetCt
            (ssCfg: sorterSetCfg) 
        = 
        match ssCfg with
        | RndDenovo cCfg -> 
            cCfg |> SorterSetRndCfg.getSorterCount
        | RndDenovoMutated cfg -> 
            cfg |> SorterSetMutatedFromRndCfg.getSorterCountMutated



    let getCfgName
            (ssCfg: sorterSetCfg) 
        = 
        match ssCfg with
        | RndDenovo cCfg -> 
            cCfg |> SorterSetRndCfg.getConfigName
        | RndDenovoMutated cfg -> 
            cfg |> SorterSetMutatedFromRndCfg.getConfigName


    let getFileName
            (ssCfg: sorterSetCfg) 
        = 
        match ssCfg with
        | RndDenovo cCfg -> 
            cCfg |> SorterSetRndCfg.getFileName
        | RndDenovoMutated cfg -> 
            cfg |> SorterSetMutatedFromRndCfg.getFileName


    let rec getSorterSet
            (save: string -> sorterSet -> Result<bool, string>)
            (sorterSetLookup: string -> Result<sorterSet, string>)
            (getParentMap: sorterSetParentMapCfg -> Result<sorterSetParentMap, string>)
            (ssCfg: sorterSetCfg) 
        = 
        match ssCfg with
        | RndDenovo cCfg -> 
            cCfg |> SorterSetRndCfg.getSorterSet sorterSetLookup save
        | RndDenovoMutated cfg -> 
            result {
                let parentCfg = 
                        cfg |> SorterSetMutatedFromRndCfg.getSorterSetParentCfg
                            |> sorterSetCfg.RndDenovo

                let! parentSorterSet = getSorterSet save sorterSetLookup getParentMap parentCfg
                let! parentMap = (cfg 
                                    |> SorterSetMutatedFromRndCfg.getSorterSetParentMapCfg
                                    |> getParentMap)

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

