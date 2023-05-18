namespace global

open System

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
        }

    let getOrder (cfg: ssmfr_EvalAllBitsCfg) = 
            cfg.order

    let getRngGenCreate (cfg: ssmfr_EvalAllBitsCfg) = 
            cfg.rngGenCreate

    let getSorterEvalMode  (cfg: ssmfr_EvalAllBitsCfg) = 
            cfg.sorterEvalMode

    let getSwitchGenMode (cfg: ssmfr_EvalAllBitsCfg) = 
            cfg.switchGenMode

    let getSwitchCount (cfg: ssmfr_EvalAllBitsCfg) = 
            cfg.switchCount

    let getSortableSetCertainCfg
            (cfg:ssmfr_EvalAllBitsCfg)
        =
        cfg.order |> sortableSetCertainCfg.All_Bits

    let getSorterSetMutatedFromRndCfg 
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
