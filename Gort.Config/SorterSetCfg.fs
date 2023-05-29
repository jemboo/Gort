namespace global
      
type sorterSetCfg = 
     | RndDenovo of sorterSetRndCfg
     | RndDenovoMutated of sorterSetMutatedFromRndCfg

module SorterSetCfg =

    let getId 
            (ssCfg: sorterSetCfg) 
        = 
        match ssCfg with
        | RndDenovo rdssCfg -> 
            rdssCfg |> SorterSetRndCfg.getId
        | RndDenovoMutated cfg -> 
            cfg |> SorterSetMutatedFromRndCfg.getId


    let makeSorterSet
            (ssCfg: sorterSetCfg) 
            (lookup: (sorterSetCfg -> Result<sorterSet, string>) option)
        = 
        match ssCfg with
        | RndDenovo cCfg -> 
            cCfg |> SorterSetRndCfg.makeSorterSet |> Ok
        | RndDenovoMutated cfg -> 
            match lookup with
            | Some lk ->
                result {
                    let! parentSorterSet = lk ssCfg
                    let! mutantSet = 
                            parentSorterSet |>
                                SorterSetMutator.createMutantSorterSetFromParentMap
                                    (cfg 
                                        |> SorterSetMutatedFromRndCfg.getSorterSetParentMapCfg
                                        |> SorterSetParentMapCfg.makeParentMap)
                                    (cfg |> SorterSetMutatedFromRndCfg.getSorterSetMutator)
                            
                    return mutantSet
                }
            | None -> failwith "" 



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

