namespace global
open System

type ssMfr_EvalAllBits_ReportCfg = 
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


module SsMfr_EvalAllBits_ReportCfg
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

    let getOrders (cfg: ssMfr_EvalAllBits_ReportCfg) = 
            cfg.orders

    let getRngGenCreates (cfg: ssMfr_EvalAllBits_ReportCfg) = 
            cfg.rngGenCreates

    let getSorterEvalMode  (cfg: ssMfr_EvalAllBits_ReportCfg) = 
            cfg.sorterEvalMode

    let getStagePrefixCount  (cfg: ssMfr_EvalAllBits_ReportCfg) = 
            cfg.stagePrefixCount

    let getSwitchGenModes (cfg: ssMfr_EvalAllBits_ReportCfg) = 
            cfg.switchGenModes

    let getSwitchCounts (cfg: ssMfr_EvalAllBits_ReportCfg) = 
            cfg.switchCounts

    let getSorterEvalReport (cfg: ssMfr_EvalAllBits_ReportCfg) = 
            cfg.sorterEvalReport


    let getSsMfr_EvalAllBitsCfg
            (order:order)
            (switchGenMode:switchGenMode)
            (rngGenCreate:rngGen)
            (rngGenMutate:rngGen)
            (mutationRate:mutationRate)
            (cfg:ssMfr_EvalAllBits_ReportCfg)
        =
        SsMfr_EvalAllBitsCfg.create 
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
            (cfg:ssMfr_EvalAllBits_ReportCfg) 
        =
        cfg.reportFileName


    let getReportHeader ()
        =
        SorterEval.reportHeader
            "eval_id\torder\tswitch_gen\tsortable_type\tmutation_rate"

    let getReportLines 
            (sorterSetEvalRet: ssMfr_EvalAllBitsCfg->Result<sorterSetEval,string>)
            (cfg: ssMfr_EvalAllBits_ReportCfg)
            (order:order)
            (switchGenMode:switchGenMode)
            (rngGenCreate:rngGen)
            (rngGenMutate:rngGen)
            (mutationRate:mutationRate)
        =
        let ssMfr_EvalAllBitsCfg = 
            cfg 
            |> getSsMfr_EvalAllBitsCfg
                order
                switchGenMode
                rngGenCreate
                rngGenMutate
                mutationRate

        let eval_id = 
            ssMfr_EvalAllBitsCfg
            |> SsMfr_EvalAllBitsCfg.getId
            |> SorterSetEvalId.value
            |> string


        let mutRate = 
            ssMfr_EvalAllBitsCfg
            |> SsMfr_EvalAllBitsCfg.getMutationRate
            |> MutationRate.value

        let order = 
            ssMfr_EvalAllBitsCfg
            |> SsMfr_EvalAllBitsCfg.getOrder
            |> Order.value

        let switchGen = 
            ssMfr_EvalAllBitsCfg 
            |> SsMfr_EvalAllBitsCfg.getSorterSetCfg
            |> SorterSetMutatedFromRndCfg.getSwitchGenMode
            |> string

        let sortableSetCfgName = 
            ssMfr_EvalAllBitsCfg 
            |> SsMfr_EvalAllBitsCfg.getSortableSetCfg
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
                sorterSetEvalRet ssMfr_EvalAllBitsCfg

            return 
                sorterSetEval 
                |> SorterSetEval.getSorterEvals
                |> Array.map(SorterEval.report linePfx)
        }


    let makeSorterSetEvalReport
            (cfg: ssMfr_EvalAllBits_ReportCfg)
            (sorterSetEvalRet: ssMfr_EvalAllBitsCfg->Result<sorterSetEval,string>)
        =
             seq {
                    for ordr in cfg.orders do
                        for sgm in cfg.switchGenModes do
                            for rgnC in cfg.rngGenCreates do
                                for rgnM in cfg.rngGenMutates do
                                    for mutR in cfg.mutationRates do
                                        getReportLines
                                            sorterSetEvalRet
                                            cfg
                                            ordr
                                            sgm
                                            rgnC
                                            rgnM
                                            mutR
                                                    
                 }
               