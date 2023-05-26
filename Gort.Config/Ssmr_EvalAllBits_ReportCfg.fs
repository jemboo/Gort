namespace global
open System

type ssmfr_EvalAllBits_ReportCfg = 
    private
        { 
          orders: order[]
          rngGenCreates: rngGen[]
          switchGenModes: switchGenMode[]
          switchCounts: order -> switchCount
          sorterCountCreates:  order -> sorterCount
          rngGenMutates: rngGen[]
          sorterCountMutates:  order -> sorterCount
          mutationRates: mutationRate[]
          sorterEvalMode: sorterEvalMode
          stagePrefixCount: stageCount
          sorterEvalReport : sorterEvalReport
          reportFileName : string
        }


module Ssmfr_EvalAllBits_ReportCfg
    =
    let create (orders:order[])
               (rngGenCreates:rngGen[])
               (switchGenModes:switchGenMode[])
               (switchCounts: order -> switchCount)
               (sorterCountCreates:  order -> sorterCount)
               (rngGenMutates:rngGen[])
               (sorterCountMutates:  order -> sorterCount)
               (mutationRates:mutationRate[])
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
            rngGenMutates=rngGenMutates;
            sorterCountMutates=sorterCountMutates;
            mutationRates=mutationRates;
            sorterEvalMode=sorterEvalMode
            stagePrefixCount=stagePrefixCount;
            sorterEvalReport=sorterEvalReport
            reportFileName=reportFileName
        }

    let getOrders (cfg: ssmfr_EvalAllBits_ReportCfg) = 
            cfg.orders

    let getRngGenCreates (cfg: ssmfr_EvalAllBits_ReportCfg) = 
            cfg.rngGenCreates

    let getSorterEvalMode  (cfg: ssmfr_EvalAllBits_ReportCfg) = 
            cfg.sorterEvalMode

    let getStagePrefixCount  (cfg: ssmfr_EvalAllBits_ReportCfg) = 
            cfg.stagePrefixCount

    let getSwitchGenModes (cfg: ssmfr_EvalAllBits_ReportCfg) = 
            cfg.switchGenModes

    let getSwitchCounts (cfg: ssmfr_EvalAllBits_ReportCfg) = 
            cfg.switchCounts

    let getSorterEvalReport (cfg: ssmfr_EvalAllBits_ReportCfg) = 
            cfg.sorterEvalReport


    let getSsmfr_EvalAllBitsCfg
            (order:order)
            (switchGenMode:switchGenMode)
            (rngGenCreate:rngGen)
            (rngGenMutate:rngGen)
            (mutationRate:mutationRate)
            (cfg:ssmfr_EvalAllBits_ReportCfg)
        =
        Ssmfr_EvalAllBitsCfg.create 
            order
            rngGenCreate
            switchGenMode
            (cfg.switchCounts order)
            (cfg.sorterCountCreates order)
            rngGenMutate
            (cfg.sorterCountMutates order)
            mutationRate
            cfg.sorterEvalMode
            cfg.stagePrefixCount


    let getReportFileName
            (cfg:ssmfr_EvalAllBits_ReportCfg) 
        =
        cfg.reportFileName


    let getReportHeader ()
        =
        SorterEval.reportHeader
            "eval_id\torder\tswitch_gen\tsortable_type\tmutation_rate"

    let getReportLines 
            (sorterSetEvalRet: ssmfr_EvalAllBitsCfg->Result<sorterSetEval,string>)
            (cfg: ssmfr_EvalAllBits_ReportCfg)
            (order:order)
            (switchGenMode:switchGenMode)
            (rngGenCreate:rngGen)
            (rngGenMutate:rngGen)
            (mutationRate:mutationRate)
        =
        let sorterSetEvalCfg = 
            cfg 
            |> getSsmfr_EvalAllBitsCfg
                order
                switchGenMode
                rngGenCreate
                rngGenMutate
                mutationRate

        let eval_id = 
            sorterSetEvalCfg
            |> Ssmfr_EvalAllBitsCfg.getId
            |> SorterSetEvalId.value
            |> string


        let mutRate = 
            sorterSetEvalCfg
            |> Ssmfr_EvalAllBitsCfg.getMutationRate
            |> MutationRate.value

        let order = 
            sorterSetEvalCfg
            |> Ssmfr_EvalAllBitsCfg.getOrder
            |> Order.value

        let switchGen = 
            sorterSetEvalCfg 
            |> Ssmfr_EvalAllBitsCfg.getSorterSetCfg
            |> SorterSetMutatedFromRndCfg.getSwitchGenMode
            |> string

        let sortableSetCfgName = 
            sorterSetEvalCfg 
            |> Ssmfr_EvalAllBitsCfg.getSortableSetCfg
            |> SortableSetCertainCfg.getConfigName

        let linePfx = 
            sprintf "%s\t%d\t%s\t%s\t%f"
                eval_id
                order
                switchGen
                sortableSetCfgName
                mutRate

        result {
            let! sorterSetEval = 
                sorterSetEvalRet sorterSetEvalCfg

            return 
                sorterSetEval 
                |> SorterSetEval.getSorterEvals
                |> Array.map(SorterEval.report linePfx)
        }


    let makeSorterSetEvalReport
            (cfg: ssmfr_EvalAllBits_ReportCfg)
            (sortableSetCfgRet: ssmfr_EvalAllBitsCfg->Result<sorterSetEval,string>)
        =
             seq {
                    for ordr in cfg.orders do
                        for sgm in cfg.switchGenModes do
                            for rgnC in cfg.rngGenCreates do
                                for rgnM in cfg.rngGenMutates do
                                    for mutR in cfg.mutationRates do
                                        getReportLines
                                            sortableSetCfgRet
                                            cfg
                                            ordr
                                            sgm
                                            rgnC
                                            rgnM
                                            mutR
                                                    
                 }
               