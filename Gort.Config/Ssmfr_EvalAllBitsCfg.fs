﻿namespace global

type ssmfr_EvalAllBitsCfg = 
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


module Ssmfr_EvalAllBitsCfg 
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

    let getMutationRate (cfg: ssmfr_EvalAllBitsCfg) = 
            cfg.mutationRate

    let getOrder (cfg: ssmfr_EvalAllBitsCfg) = 
            cfg.order

    let getRngGenCreate (cfg: ssmfr_EvalAllBitsCfg) = 
            cfg.rngGenCreate

    let getSorterEvalMode  (cfg: ssmfr_EvalAllBitsCfg) = 
            cfg.sorterEvalMode

    let getStagePrefixCount  (cfg: ssmfr_EvalAllBitsCfg) = 
            cfg.stagePrefixCount

    let getSwitchGenMode (cfg: ssmfr_EvalAllBitsCfg) = 
            cfg.switchGenMode

    let getSwitchCount (cfg: ssmfr_EvalAllBitsCfg) = 
            cfg.switchCount

    let getSortableSetCfg
            (cfg:ssmfr_EvalAllBitsCfg)
        =
        cfg.order |> SortableSetCertainCfg.makeAllBitsReducedOneStage

    let getSorterSetCfg 
            (cfg:ssmfr_EvalAllBitsCfg)
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
            (cfg: ssmfr_EvalAllBitsCfg) 
        = 
        [|
          (cfg |> getSorterSetCfg) :> obj;
          (cfg |> getSortableSetCfg) :> obj;
          (cfg |> getSorterEvalMode) :> obj;
        |] |> GuidUtils.guidFromObjs
           |> SorterSetEvalId.create


    let getFileName
            (cfg:ssmfr_EvalAllBitsCfg) 
        =
        cfg |> getId 
            |> SorterSetEvalId.value 
            |> string


    let makeSorterSetEval
            (up:useParallel)
            (cfg: ssmfr_EvalAllBitsCfg)
            (sortableSetCfgRet: sortableSetCfg->Result<sortableSet,string>)
            (sorterSetCfgRet: sorterSetMutatedFromRndCfg->Result<sorterSet,string>)
        =
        result {
            let! sorterSet = sorterSetCfgRet (cfg |> getSorterSetCfg)
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
             