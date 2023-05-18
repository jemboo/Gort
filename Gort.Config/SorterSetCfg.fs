namespace global
      

//module SorterSetCfg =

//    let getSorterSetId 
//            (ssCfg: sorterSetCfg) 
//        = 
//        match ssCfg with
//        | RndDenovo rdssCfg -> 
//            rdssCfg |> SorterSetRndCfg.getSorterSetId
//        | RndDenovoMutated cfg -> 
//            cfg |> SorterSetMutatedFromRndCfg.getMutatedSorterSetId


//    let makeSorterSet
//            (ssCfg: sorterSetCfg) 
//            (lookup: sorterSetRndCfg -> Result<sorterSet, string> option)
//        = 
//        match ssCfg with
//        | RndDenovo cCfg -> 
//            cCfg |> SorterSetRndCfg.makeSorterSet
//        | RndDenovoMutated cfg -> 
//            cfg |> SorterSetMutatedFromRndCfg.getMutatedSorterSetId



//    let getOrder
//            (ssCfg: sorterSetCfg) 
//        = 
//        match ssCfg with
//        | RndDenovo cCfg -> 
//            cCfg |> SorterSetRndCfg.getOrder
//        | RndDenovoMutated cfg -> 
//            cfg |> SorterSetMutatedFromRndCfg.getOrder



//    let getSorterSetCt
//            (ssCfg: sorterSetCfg) 
//        = 
//        match ssCfg with
//        | RndDenovo cCfg -> 
//            cCfg |> SorterSetRndCfg.getSorterCount
//        | RndDenovoMutated cfg -> 
//            cfg |> SorterSetMutatedFromRndCfg.getSorterCountMutated



//    let getCfgName
//            (ssCfg: sorterSetCfg) 
//        = 
//        match ssCfg with
//        | RndDenovo cCfg -> 
//            cCfg |> SorterSetRndCfg.getConfigName
//        | RndDenovoMutated cfg -> 
//            cfg |> SorterSetMutatedFromRndCfg.getConfigName



//    let getFileName
//            (ssCfg: sorterSetCfg) 
//        = 
//        match ssCfg with
//        | RndDenovo cCfg -> 
//            cCfg |> SorterSetRndCfg.getFileName
//        | RndDenovoMutated cfg -> 
//            cfg |> SorterSetMutatedFromRndCfg.getMutatedSorterSetFileName

