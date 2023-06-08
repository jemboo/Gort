namespace global

type sorterSet_EvalCfg = 
     | Rnd of sorterSetRnd_EvalAllBitsCfg
     | RndMutated of ssMfr_EvalAllBitsCfg
     | RndAppended of ssAfr_EvalAllBitsCfg


module SorterSet_EvalCfg =

    let getId
            (cfg:sorterSet_EvalCfg)
        =
        match cfg with
        | Rnd rCfg -> 
            rCfg |> SorterSetRnd_EvalAllBitsCfg.getlId
        | RndMutated rmCfg ->
            rmCfg |> SsMfr_EvalAllBitsCfg.getId        
        | RndAppended amCfg ->
            amCfg |> SsAfr_EvalAllBitsCfg.getId

    let getFileName
            (cfg:sorterSet_EvalCfg) 
        =
        match cfg with
        | Rnd cCfg -> 
            cCfg |> SorterSetRnd_EvalAllBitsCfg.getFileName
        | RndMutated cfg -> 
            cfg |> SsMfr_EvalAllBitsCfg.getFileName
        | RndAppended amCfg ->
            amCfg |> SsAfr_EvalAllBitsCfg.getFileName

    let getOrder
            (cfg:sorterSet_EvalCfg) 
        =
        match cfg with
        | Rnd cCfg -> 
            cCfg |> SorterSetRnd_EvalAllBitsCfg.getOrder
        | RndMutated cfg -> 
            cfg |> SsMfr_EvalAllBitsCfg.getOrder
        | RndAppended amCfg ->
            amCfg |> SsAfr_EvalAllBitsCfg.getOrder

    let getSortableSetCfg
            (cfg:sorterSet_EvalCfg)
        =
        match cfg with
        | Rnd rCfg -> 
            rCfg |> SorterSetRnd_EvalAllBitsCfg.getSortableSetCfg
        | RndMutated rmCfg ->
            rmCfg |> SsMfr_EvalAllBitsCfg.getSortableSetCfg
        | RndAppended amCfg ->
            amCfg |> SsAfr_EvalAllBitsCfg.getSortableSetCfg


    let getSorterSetCfg
            (cfg:sorterSet_EvalCfg)
        =
        match cfg with
        | Rnd rCfg -> 
            rCfg |> SorterSetRnd_EvalAllBitsCfg.getSorterSetCfg
                 |> sorterSetCfg.Rnd
        | RndMutated rmCfg ->
            rmCfg |> SsMfr_EvalAllBitsCfg.getSorterSetCfg
                  |> sorterSetCfg.RndMutated
        | RndAppended amCfg ->
            amCfg |> SsAfr_EvalAllBitsCfg.getSorterSetCfg
                  |> sorterSetCfg.SelfAppend

    let getSorterEvalMode
            (cfg:sorterSet_EvalCfg)
        =
        match cfg with
        | Rnd rCfg -> 
            rCfg |> SorterSetRnd_EvalAllBitsCfg.getSorterEvalMode
        | RndMutated rmCfg ->
            rmCfg |> SsMfr_EvalAllBitsCfg.getSorterEvalMode
        | RndAppended amCfg ->
            amCfg |> SsAfr_EvalAllBitsCfg.getSorterEvalMode


    let getStagePrefixCount
            (cfg:sorterSet_EvalCfg)
        =
        match cfg with
        | Rnd rCfg -> 
            rCfg |> SorterSetRnd_EvalAllBitsCfg.getStagePrefixCount
        | RndMutated rmCfg ->
            rmCfg |> SsMfr_EvalAllBitsCfg.getStagePrefixCount
        | RndAppended amCfg ->
            amCfg |> SsAfr_EvalAllBitsCfg.getStagePrefixCount


    let makeSorterSetEval
            (sortableSetCfgRet: sortableSetCfg->Result<sortableSet,string>)
            (sorterSetCfgRet: sorterSetCfg->Result<sorterSet,string>)
            (up:useParallel)
            (cfg: sorterSet_EvalCfg)
        =
        result {
            let! sorterSet = sorterSetCfgRet (cfg |> getSorterSetCfg)
            let! sortableSet = sortableSetCfgRet (cfg |> getSortableSetCfg
                                                      |> sortableSetCfg.Certain )
            let! ssEval = 
                   SorterSetEval.make
                        (getId cfg)
                        (getSorterEvalMode cfg)
                        sorterSet
                        sortableSet
                        up

            let ordr = cfg |> getOrder
            let tCmod = cfg |> getStagePrefixCount
            return
                SorterSetEval.create
                    (ssEval |> SorterSetEval.getSorterSetEvalId)
                    (ssEval |> SorterSetEval.getSorterSetlId)
                    (ssEval |> SorterSetEval.getSortableSetId)
                    (ssEval |> SorterSetEval.getSorterEvals  
                        |> Array.map(SorterEval.modifyForPrefix ordr tCmod))
        }

    let getSorterSetEval
            (sortableSetCfgRet: sortableSetCfg->Result<sortableSet,string>)
            (sorterSetCfgRet: sorterSetCfg->Result<sorterSet,string>)
            (saveSorterSetEval: string -> sorterSetEval -> Result<bool, string>)
            (lookup: string -> Result<sorterSetEval, string>)
            (up:useParallel)
            (cfg: sorterSet_EvalCfg)
        =
        result {
            let loadRes  = 
                result {
                    let! ssEval = lookup (cfg |> getFileName)
                    return ssEval
                }

            match loadRes with
            | Ok mut -> return mut
            | Error _ -> 
                let! ssEval = 
                    makeSorterSetEval
                        sortableSetCfgRet
                        sorterSetCfgRet
                        up
                        cfg

                let! resSs = ssEval |> saveSorterSetEval (cfg |> getFileName)
                return ssEval
        }