namespace global

type ssMfr_EvalAllBitsCfg = 
    private
        { 
          order: order
          rngGenCreate: rngGen
          switchGenMode: switchGenMode
          switchCount: switchCount
          sorterCountCreate: sorterCount
          rngGenMutate: rngGen
          sorterCountMutate: sorterCount
          mutationRate:mutationRate
          sorterEvalMode: sorterEvalMode
          stagePrefixCount: stageCount
        }


module SsMfr_EvalAllBitsCfg 
    =
    let create (order:order)
               (rngGenCreate:rngGen)
               (switchGenMode:switchGenMode)
               (switchCount:switchCount)
               (sorterCountCreate:sorterCount)
               (rngGenMutate:rngGen)
               (sorterCountMutate:sorterCount)
               (mutationRate:mutationRate)
               (sorterEvalMode: sorterEvalMode)
               (stagePrefixCount: stageCount)
        =
        {
            order=order;
            rngGenCreate=rngGenCreate;
            switchGenMode=switchGenMode;
            switchCount=switchCount;
            sorterCountCreate=sorterCountCreate;
            rngGenMutate=rngGenMutate;
            sorterCountMutate=sorterCountMutate;
            mutationRate=mutationRate
            sorterEvalMode=sorterEvalMode
            stagePrefixCount=stagePrefixCount;
        }

    let getMutationRate (cfg: ssMfr_EvalAllBitsCfg) = 
            cfg.mutationRate

    let getOrder (cfg: ssMfr_EvalAllBitsCfg) = 
            cfg.order

    let getRngGenCreate (cfg: ssMfr_EvalAllBitsCfg) = 
            cfg.rngGenCreate

    let getSorterEvalMode  (cfg: ssMfr_EvalAllBitsCfg) = 
            cfg.sorterEvalMode

    let getStagePrefixCount  (cfg: ssMfr_EvalAllBitsCfg) = 
            cfg.stagePrefixCount

    let getSwitchGenMode (cfg: ssMfr_EvalAllBitsCfg) = 
            cfg.switchGenMode

    let getSwitchCount (cfg: ssMfr_EvalAllBitsCfg) = 
            cfg.switchCount

    let getSortableSetCfg
            (cfg:ssMfr_EvalAllBitsCfg)
        =
        cfg.order |> SortableSetCertainCfg.makeAllBitsReducedOneStage

    let getSorterSetCfg 
            (cfg:ssMfr_EvalAllBitsCfg)
        =
        SorterSetMutatedFromRndCfg.create 
            cfg.order
            cfg.rngGenCreate
            cfg.switchGenMode
            cfg.switchCount
            cfg.sorterCountCreate
            cfg.rngGenMutate
            cfg.sorterCountMutate
            cfg.mutationRate


    let getId
            (cfg: ssMfr_EvalAllBitsCfg) 
        = 
        [|
          (cfg |> getSorterSetCfg |> SorterSetMutatedFromRndCfg.getId |> SorterSetId.value) :> obj;
          (cfg |> getSortableSetCfg |> SortableSetCertainCfg.getId |> SortableSetId.value) :> obj;
          (cfg |> getSorterEvalMode) :> obj;
        |] |> GuidUtils.guidFromObjs
           |> SorterSetEvalId.create


    let getFileName
            (cfg:ssMfr_EvalAllBitsCfg) 
        =
        cfg |> getId 
            |> SorterSetEvalId.value 
            |> string


    let makeSorterSetEval
            (up:useParallel)
            (cfg: ssMfr_EvalAllBitsCfg)
            (sortableSetCfgRet: sortableSetCfg->Result<sortableSet,string>)
            (sorterSetCfgRet: sorterSetCfg->Result<sorterSet,string>)
        =
        result {
            let! sorterSet = sorterSetCfgRet (cfg |> getSorterSetCfg |> sorterSetCfg.RndMutated)
            let! sortableSet = sortableSetCfgRet 
                                ( cfg |> getSortableSetCfg
                                      |> sortableSetCfg.Certain   )
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
             