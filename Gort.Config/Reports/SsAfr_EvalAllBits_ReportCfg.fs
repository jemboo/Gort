namespace global
open System

type ssAfr_EvalAllBits_ReportCfg = 
    private
        { 
          orders: order[]
          rngGenCreates: rngGen[]
          switchGenModes: switchGenMode[]
          switchCounts: order -> switchCount
          sorterCountCreates:  order -> sorterCount
          sorterEvalMode: sorterEvalMode
          stagePrefixCount: stageCount
          sorterEvalReport : sorterEvalReport
          reportFileName : string
        }


module SsAfr_EvalAllBits_ReportCfg
    =
    let create (orders:order[])
               (rngGenCreates:rngGen[])
               (switchGenModes:switchGenMode[])
               (switchCounts: order -> switchCount)
               (sorterCountCreates:  order -> sorterCount)
               (sorterEvalMode: sorterEvalMode)
               (stagePrefixCount: stageCount)
               (sorterEvalReport: sorterEvalReport)
               (reportFileName : string)
        =
        {
            orders=orders;
            rngGenCreates=rngGenCreates;
            switchGenModes=switchGenModes;
            switchCounts=switchCounts;
            sorterCountCreates=sorterCountCreates;
            sorterEvalMode=sorterEvalMode
            stagePrefixCount=stagePrefixCount;
            sorterEvalReport=sorterEvalReport
            reportFileName=reportFileName
        }

    let getOrders (cfg: ssAfr_EvalAllBits_ReportCfg) = 
            cfg.orders

    let getRngGenCreates (cfg: ssAfr_EvalAllBits_ReportCfg) = 
            cfg.rngGenCreates

    let getSorterEvalMode  (cfg: ssAfr_EvalAllBits_ReportCfg) = 
            cfg.sorterEvalMode

    let getStagePrefixCount  (cfg: ssAfr_EvalAllBits_ReportCfg) = 
            cfg.stagePrefixCount

    let getSwitchGenModes (cfg: ssAfr_EvalAllBits_ReportCfg) = 
            cfg.switchGenModes

    let getSwitchCounts (cfg: ssAfr_EvalAllBits_ReportCfg) = 
            cfg.switchCounts

    let getSorterEvalReport (cfg: ssAfr_EvalAllBits_ReportCfg) = 
            cfg.sorterEvalReport


    let getSsAfr_EvalAllBitsCfg
            (order:order)
            (rngGenCreate:rngGen)
            (switchGenMode:switchGenMode)
            (cfg:ssAfr_EvalAllBits_ReportCfg)
        =
        SsAfr_EvalAllBitsCfg.create 
            order
            rngGenCreate
            switchGenMode
            (cfg.switchCounts order)
            (cfg.sorterCountCreates order)
            cfg.sorterEvalMode
            cfg.stagePrefixCount


    let getSorterSetSelfAppendCfg
            (order:order)
            (rngGenCreate:rngGen)
            (switchGenMode:switchGenMode)
            (cfg:ssAfr_EvalAllBits_ReportCfg)
        =
        SorterSetSelfAppendCfg.create 
            order
            rngGenCreate
            switchGenMode
            (cfg.switchCounts order)
            (cfg.sorterCountCreates order)


    let getReportFileName
            (cfg:ssAfr_EvalAllBits_ReportCfg) 
        =
        cfg.reportFileName

    
    let getReportHeader ()
        =
        (SorterEval.reportHeader
                "eval_id\torder\tswitch_gen\tsortable_type\tsorter_pfx\tsorter_sfx")



    let getReportLine 
            (linePfx:string)
            (concatMap:Map<sorterId, sorterId[]>)
            (sorterEval:sorterEval)
        =
        let key = sorterEval |> SorterEval.getSorterId
        let sA = concatMap.[key] |> Array.map(SorterId.value >> string)
        let exPfx = sprintf "%s\t%s\t%s" linePfx sA.[0] sA.[1]
        SorterEval.report exPfx sorterEval


    let getReportLines 
            (sorterSetEvalRet: ssAfr_EvalAllBitsCfg->Result<sorterSetEval,string>)
            (sorterSetConcatMapRet: sorterSetSelfAppendCfg->Result<sorterSetConcatMap,string>)
            (cfg: ssAfr_EvalAllBits_ReportCfg)
            (order:order)
            (switchGenMode:switchGenMode)
            (rngGenCreate:rngGen)
        =

        let ssAfr_EvalAllBitsCfg = 
            cfg 
            |> getSsAfr_EvalAllBitsCfg
                order
                rngGenCreate
                switchGenMode

        let sorterSetSelfAppendCfg = 
            cfg 
            |> getSorterSetSelfAppendCfg
                order
                rngGenCreate
                switchGenMode

        let eval_id = 
            ssAfr_EvalAllBitsCfg
            |> SsAfr_EvalAllBitsCfg.getId
            |> SorterSetEvalId.value
            |> string

        let order = 
            ssAfr_EvalAllBitsCfg
            |> SsAfr_EvalAllBitsCfg.getOrder
            |> Order.value

        let switchGen = 
            ssAfr_EvalAllBitsCfg 
            |> SsAfr_EvalAllBitsCfg.getSorterSetCfg
            |> SorterSetSelfAppendCfg.getSwitchGenMode
            |> string

        let sortableSetCfgName =
            ssAfr_EvalAllBitsCfg 
            |> SsAfr_EvalAllBitsCfg.getSortableSetCfg
            |> SortableSetCertainCfg.getConfigName

        let linePfx = 
            sprintf "%s\t%d\t%s\t%s"
                eval_id
                order
                switchGen
                sortableSetCfgName


        result {

            let! sorterSetAppendedEval = 
                sorterSetEvalRet ssAfr_EvalAllBitsCfg

            let! sorterSetConcatMap =
                sorterSetConcatMapRet sorterSetSelfAppendCfg

            let concatMap = sorterSetConcatMap |> SorterSetConcatMap.getConcatMap

            return 
                sorterSetAppendedEval 
                |> SorterSetEval.getSorterEvals
                |> Array.map(getReportLine linePfx concatMap)
        }


    let makeSorterSetEvalReport
            (cfg: ssAfr_EvalAllBits_ReportCfg)
            (sorterSetEvalRet: ssAfr_EvalAllBitsCfg->Result<sorterSetEval,string>)
            (sorterSetAppendMapRet: sorterSetSelfAppendCfg->Result<sorterSetConcatMap,string>)
        =
             seq {
                    for ordr in cfg.orders do
                        for sgm in cfg.switchGenModes do
                            for rgnC in cfg.rngGenCreates do
                                        getReportLines
                                            sorterSetEvalRet
                                            sorterSetAppendMapRet
                                            cfg
                                            ordr
                                            sgm
                                            rgnC            
                 }
               

    //let makeSorterSetEvalReport
    //        (cfg: ssAfr_EvalAllBits_ReportCfg)
    //        (sorterSetEvalRet: ssAfr_EvalAllBitsCfg->Result<sorterSetEval,string>)
    //        (sorterSetAppendMapRet: sorterSetSelfAppendCfg->Result<sorterSetConcatMap,string>)
    //    =
    //         seq {
    //                for ordr in cfg.orders do
    //                    for sgm in cfg.switchGenModes do
    //                        for rgnC in cfg.rngGenCreates do
    //                                    getReportLines
    //                                        sorterSetEvalRet
    //                                        sorterSetAppendMapRet
    //                                        cfg
    //                                        ordr
    //                                        sgm
    //                                        rgnC            
    //             }
               