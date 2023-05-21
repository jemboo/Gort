namespace global
open System

type sorterEvalReport = 
     | Full
     | SpeedBin


type sorterEvalReportId = private SorterEvalReportId of Guid

module SorterEvalReportId =
    let value (SorterEvalReportId v) = v
    let create id = SorterEvalReportId id


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
          sorterEvalReport : sorterEvalReport
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
               (sorterEvalReport: sorterEvalReport)
        =
        {
            order=order;
            rngGenCreate=rngGenCreate;
            switchGenMode=switchGenMode;
            switchCount=switchCount;
            sorterCountCreate=sorterCountCreate;
            sorterEvalMode=sorterEvalMode
            stagePrefixCount=stagePrefixCount;
            sorterEvalReport=sorterEvalReport
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

    let getSorterEvalReport (cfg: sorterSetRnd_EvalAllBitsCfg) = 
            cfg.sorterEvalReport

    let getSortableSetCertainCfg
            (cfg:sorterSetRnd_EvalAllBitsCfg)
        =
        cfg.order |> SortableSetCertainCfg.makeAllBitsReducedOneStage

    let getSorterSetRndCfg 
            (cfg:sorterSetRnd_EvalAllBitsCfg)
        =
        SorterSetRndCfg.create 
            cfg.order
            cfg.rngGenCreate
            cfg.switchGenMode
            [||]
            cfg.switchCount
            cfg.sorterCountCreate


    let getSorterSetMutatedFromRndCfg 
            (cfg:sorterSetRnd_EvalAllBitsCfg)
        =
        SorterSetRndCfg.create 
            cfg.order
            cfg.rngGenCreate
            cfg.switchGenMode
            [||]
            cfg.switchCount
            cfg.sorterCountCreate


    let getSorterSetEvalId
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
        cfg |> getSorterSetEvalId 
            |> SorterSetEvalId.value 
            |> string


    let makeSorterSetEval
            (up:useParallel)
            (cfg: sorterSetRnd_EvalAllBitsCfg)
            (sortableSetCfgRet: sortableSetCertainCfg->Result<sortableSet,string>)
            (sorterSetCfgRet: sorterSetRndCfg->Result<sorterSet,string>)
        =
        result {
            let! sorterSet = sorterSetCfgRet (cfg |> getSorterSetRndCfg)
            let! sortableSet = sortableSetCfgRet (cfg |> getSortableSetCertainCfg)
            let! ssEval = 
                   SorterSetEval.make
                        (getSorterSetEvalId cfg)
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
               