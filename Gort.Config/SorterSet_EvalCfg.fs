namespace global

type sorterSet_EvalCfg = 
     | Rnd of sorterSetRnd_EvalAllBitsCfg
     | RndMutated of ssmfr_EvalAllBitsCfg


module SorterSet_EvalCfg =

    let getFileName
            (cfg:sorterSet_EvalCfg) 
        =
        match cfg with
        | Rnd cCfg -> 
            cCfg |> SorterSetRnd_EvalAllBitsCfg.getFileName
        | RndMutated cfg -> 
            cfg |> Ssmfr_EvalAllBitsCfg.getFileName


    let makeSorterSetEval
            (sortableSetCfgRet: sortableSetCfg->Result<sortableSet,string>)
            (sorterSetCfgRet: sorterSetCfg->Result<sorterSet,string>)
            (saveSorterSetEval: string -> sorterSetEval -> Result<bool, string>)
            (up:useParallel)
            (cfg: sorterSet_EvalCfg)
        =
        match cfg with
        | Rnd rCfg -> 
            result {
                let fileName = cfg |> getFileName 
                let! ssEval = 
                        SorterSetRnd_EvalAllBitsCfg.makeSorterSetEval
                            up
                            rCfg
                            sortableSetCfgRet
                            sorterSetCfgRet

                let! resSs = ssEval |> saveSorterSetEval fileName
                return ssEval
            }
        | RndMutated rmCfg ->
            result {
                let fileName = cfg |> getFileName 
                let! ssEval = 
                        Ssmfr_EvalAllBitsCfg.makeSorterSetEval
                            up
                            rmCfg
                            sortableSetCfgRet
                            sorterSetCfgRet

                let! resSs = ssEval |> saveSorterSetEval fileName
                return ssEval
            }


    let getSorterSetEval
            (sortableSetCfgRet: sortableSetCfg->Result<sortableSet,string>)
            (sorterSetCfgRet: sorterSetCfg->Result<sorterSet,string>)
            (saveSorterSetEval: string -> sorterSetEval -> Result<bool, string>)
            (lookup: string -> Result<sorterSetEval, string>)
            (up:useParallel)
            (cfg: sorterSet_EvalCfg)
        =
        let fileName = cfg |> getFileName 
        result {
            let loadRes  = 
                result {
                    let! ssEval = lookup (cfg |> getFileName)
                    return ssEval
                }

            match loadRes with
            | Ok mut -> return mut
            | Error _ -> 
                return! 
                    makeSorterSetEval
                        sortableSetCfgRet
                        sorterSetCfgRet
                        saveSorterSetEval
                        up
                        cfg
        }