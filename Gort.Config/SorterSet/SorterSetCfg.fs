namespace global
      
type sorterSetCfg = 
     | Rnd of sorterSetRndCfg
     | RndMutated of sorterSetMutatedFromRndCfg
     | SelfAppend of sorterSetSelfAppendCfg

module SorterSetCfg =

    let getProperties 
            (ssCfg: sorterSetCfg) 
        = 
        match ssCfg with
        | Rnd rdssCfg -> 
            rdssCfg |> SorterSetRndCfg.getProperties
        | RndMutated cfg -> 
            cfg |> SorterSetMutatedFromRndCfg.getProperties
        | SelfAppend cfg -> 
            cfg |> SorterSetSelfAppendCfg.getProperties

    let getId 
            (ssCfg: sorterSetCfg) 
        = 
        match ssCfg with
        | Rnd rdssCfg -> 
            rdssCfg |> SorterSetRndCfg.getId
        | RndMutated cfg -> 
            cfg |> SorterSetMutatedFromRndCfg.getId
        | SelfAppend cfg -> 
            cfg |> SorterSetSelfAppendCfg.getSorterSetConcatId


    let getOrder
            (ssCfg: sorterSetCfg) 
        = 
        match ssCfg with
        | Rnd cCfg -> 
            cCfg |> SorterSetRndCfg.getOrder
        | RndMutated cfg -> 
            cfg |> SorterSetMutatedFromRndCfg.getOrder
        | SelfAppend cfg -> 
            cfg |> SorterSetSelfAppendCfg.getOrder


    let getSorterSetCt
            (ssCfg: sorterSetCfg) 
        = 
        match ssCfg with
        | Rnd cCfg -> 
            cCfg |> SorterSetRndCfg.getSorterCount
        | RndMutated cfg -> 
            cfg |> SorterSetMutatedFromRndCfg.getSorterCount
        | SelfAppend cfg -> 
            cfg |> SorterSetSelfAppendCfg.getSorterSetConcatCount


    let getCfgName
            (ssCfg: sorterSetCfg) 
        = 
        match ssCfg with
        | Rnd cCfg -> 
            cCfg |> SorterSetRndCfg.getConfigName
        | RndMutated cfg -> 
            cfg |> SorterSetMutatedFromRndCfg.getConfigName
        | SelfAppend cfg -> 
            cfg |> SorterSetSelfAppendCfg.getConfigName


    let getFileName
            (ssCfg: sorterSetCfg) 
        = 
        match ssCfg with
        | Rnd cCfg -> 
            cCfg |> SorterSetRndCfg.getFileName
        | RndMutated cfg -> 
            cfg |> SorterSetMutatedFromRndCfg.getFileName
        | SelfAppend cfg -> 
            cfg |> SorterSetSelfAppendCfg.getSorterSetConcatFileName


    let rec getSorterSet
            (sorterSetSave: string -> sorterSet -> Result<bool, string>)
            (sorterSetLookup: string -> Result<sorterSet, string>)
            (getParentMap: sorterSetParentMapCfg -> Result<sorterSetParentMap, string>)
            (getConcatMap: sorterSetSelfAppendCfg -> Result<sorterSetConcatMap, string>)
            (ssCfg: sorterSetCfg) 
        = 
        match ssCfg with
        | Rnd rCfg -> 
            rCfg |> SorterSetRndCfg.getSorterSet sorterSetLookup sorterSetSave
        | RndMutated rmCfg -> 
            result {
                let parentCfg = 
                        rmCfg |> SorterSetMutatedFromRndCfg.getSorterSetParentCfg
                              |> sorterSetCfg.Rnd

                let! parentSorterSet = getSorterSet 
                                        sorterSetSave 
                                        sorterSetLookup 
                                        getParentMap
                                        getConcatMap
                                        parentCfg
                let! parentMap = ( rmCfg 
                                   |> SorterSetMutatedFromRndCfg.getSorterSetParentMapCfg
                                   |> getParentMap )

                let mutantSorterSetFileName = (rmCfg |> SorterSetMutatedFromRndCfg.getFileName)
                let mutantSorterSetFinding = sorterSetLookup mutantSorterSetFileName

                match mutantSorterSetFinding with
                | Ok mutantSS -> return mutantSS
                | Error _ -> 
                    let! mutantSorterSet = 
                            parentSorterSet |>
                                SorterSetMutator.createMutantSorterSetFromParentMap
                                    parentMap
                                    (rmCfg |> SorterSetMutatedFromRndCfg.getSorterSetMutator)
                    let! isSaved = sorterSetSave 
                                    mutantSorterSetFileName  
                                    mutantSorterSet
                    return mutantSorterSet
            }
        | SelfAppend saCfg ->
            result {
                let factorCfg = saCfg |> SorterSetSelfAppendCfg.getSorterSetFactorCfg
                let! ssFactor = factorCfg |> SorterSetRndCfg.getSorterSet sorterSetLookup sorterSetSave

                let selfAppendSSFileName =
                        saCfg |> SorterSetSelfAppendCfg.getSorterSetConcatFileName
                let selfAppendSSFinding = sorterSetLookup selfAppendSSFileName

                match selfAppendSSFinding with
                | Ok saSS -> return saSS
                | Error _ -> 
                    let! ssConcatMap = saCfg |> getConcatMap
                    let! selfAppendSS =       
                            SorterSetConcatMap.createSorterSetAppend
                                    ssFactor
                                    (saCfg |> SorterSetSelfAppendCfg.getSorterSetConcatId)
                    let! isSaved = sorterSetSave 
                                    (saCfg |> SorterSetSelfAppendCfg.getSorterSetConcatFileName)  
                                    selfAppendSS
                    return selfAppendSS
            }