namespace global


type sorterSetRnd_EvalAllBitsCfg = 
    private
        { 
          order: order
          rngGenCreate: rngGen
          switchGenMode: switchGenMode
          switchCount: switchCount
          sorterCountCreate: sorterCount
          sorterEvalMode: sorterEvalMode
          stagePrefixCount: stageCount
        }


module SorterSetRnd_EvalAllBitsCfg 
    =
    let create (order:order)
               (rngGenCreate:rngGen)
               (switchGenMode:switchGenMode)
               (switchCount:switchCount)
               (sorterCountCreate:sorterCount)
               (sorterEvalMode: sorterEvalMode)
               (stagePrefixCount: stageCount)
        =
        {
            order=order;
            rngGenCreate=rngGenCreate;
            switchGenMode=switchGenMode;
            switchCount=switchCount;
            sorterCountCreate=sorterCountCreate;
            sorterEvalMode=sorterEvalMode
            stagePrefixCount=stagePrefixCount;
        }

    let getOrder (cfg: sorterSetRnd_EvalAllBitsCfg) = 
            cfg.order

    let getRngGenCreate (cfg: sorterSetRnd_EvalAllBitsCfg) = 
            cfg.rngGenCreate

    let getSorterEvalMode  (cfg: sorterSetRnd_EvalAllBitsCfg) = 
            cfg.sorterEvalMode

    let getStagePrefixCount  (cfg: sorterSetRnd_EvalAllBitsCfg) = 
            cfg.stagePrefixCount

    let getSwitchGenMode (cfg: sorterSetRnd_EvalAllBitsCfg) = 
            cfg.switchGenMode

    let getSwitchCount (cfg: sorterSetRnd_EvalAllBitsCfg) = 
            cfg.switchCount

    let getlId
            (cfg: sorterSetRnd_EvalAllBitsCfg) 
        = 
        [|
          (cfg.GetType()) :> obj;
           cfg :> obj;
        |] |> GuidUtils.guidFromObjs
           |> SorterSetEvalId.create


    let getFileName
            (cfg:sorterSetRnd_EvalAllBitsCfg) 
        =
        cfg |> getlId 
            |> SorterSetEvalId.value 
            |> string


    let getSortableSetCfg
            (cfg:sorterSetRnd_EvalAllBitsCfg)
        =
        cfg.order |> SortableSetCertainCfg.makeAllBitsReducedOneStage


    let getSorterSetCfg 
            (cfg:sorterSetRnd_EvalAllBitsCfg)
        =
        SorterSetRndCfg.create 
            cfg.order
            cfg.rngGenCreate
            cfg.switchGenMode
            [||]
            cfg.switchCount
            cfg.sorterCountCreate


    let makeSorterSetEval
            (up:useParallel)
            (cfg: sorterSetRnd_EvalAllBitsCfg)
            (sortableSetCfgRet: sortableSetCfg->Result<sortableSet,string>)
            (sorterSetCfgRet: sorterSetRndCfg->Result<sorterSet,string>)
        =
        result {
            let! sorterSet = sorterSetCfgRet (cfg |> getSorterSetCfg)
            let! sortableSet = sortableSetCfgRet (cfg |> getSortableSetCfg
                                                      |> sortableSetCfg.Certain )
            let! ssEval = 
                   SorterSetEval.make
                        (getlId cfg)
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
               