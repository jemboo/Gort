namespace global

type ssAfr_EvalAllBitsCfg = 
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


module SsAfr_EvalAllBitsCfg 
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


    let getOrder (cfg: ssAfr_EvalAllBitsCfg) = 
            cfg.order

    let getRngGenCreate (cfg: ssAfr_EvalAllBitsCfg) = 
            cfg.rngGenCreate

    let getSorterEvalMode  (cfg: ssAfr_EvalAllBitsCfg) = 
            cfg.sorterEvalMode

    let getStagePrefixCount  (cfg: ssAfr_EvalAllBitsCfg) = 
            cfg.stagePrefixCount

    let getSwitchGenMode (cfg: ssAfr_EvalAllBitsCfg) = 
            cfg.switchGenMode

    let getSwitchCount (cfg: ssAfr_EvalAllBitsCfg) = 
            cfg.switchCount

    let getSortableSetCfg
            (cfg:ssAfr_EvalAllBitsCfg)
        =
        cfg.order |> SortableSetCertainCfg.makeAllBitsReducedOneStage

    let getSorterSetCfg 
            (cfg:ssAfr_EvalAllBitsCfg)
        =
        SorterSetSelfAppendCfg.create 
            cfg.order
            cfg.rngGenCreate
            cfg.switchGenMode
            cfg.switchCount
            cfg.sorterCountCreate


    let getId
            (cfg: ssAfr_EvalAllBitsCfg) 
        = 
        [|
          (cfg |> getSorterSetCfg |> SorterSetSelfAppendCfg.getSorterSetConcatId |> SorterSetId.value) :> obj;
          (cfg |> getSortableSetCfg |> SortableSetCertainCfg.getId |> SortableSetId.value) :> obj;
          (cfg |> getSorterEvalMode) :> obj;
        |] |> GuidUtils.guidFromObjs
           |> SorterSetEvalId.create


    let getFileName
            (cfg:ssAfr_EvalAllBitsCfg) 
        =
        cfg |> getId 
            |> SorterSetEvalId.value 
            |> string


    let makeSorterSetEval
            (up:useParallel)
            (cfg: ssAfr_EvalAllBitsCfg)
            (sortableSetCfgRet: sortableSetCfg->Result<sortableSet,string>)
            (sorterSetCfgRet: sorterSetCfg->Result<sorterSet,string>)
        =
        result {
            let! sorterSet = sorterSetCfgRet 
                                ( cfg |> getSorterSetCfg 
                                      |> sorterSetCfg.SelfAppend)
            let! sortableSet = sortableSetCfgRet 
                                ( cfg |> getSortableSetCfg
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
             