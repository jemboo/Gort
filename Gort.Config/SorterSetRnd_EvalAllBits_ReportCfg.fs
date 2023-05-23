namespace global
open System

type sorterEvalReport = 
     | Full
     | SpeedBin


type sorterEvalReportId = private SorterEvalReportId of Guid

module SorterEvalReportId =
    let value (SorterEvalReportId v) = v
    let create id = SorterEvalReportId id


type sorterSetRnd_EvalAllBits_ReportCfg = 
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


module SorterSetRnd_EvalAllBits_ReportCfg
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

    let getOrders (cfg: sorterSetRnd_EvalAllBits_ReportCfg) = 
            cfg.orders

    let getRngGenCreates (cfg: sorterSetRnd_EvalAllBits_ReportCfg) = 
            cfg.rngGenCreates

    let getSorterEvalMode  (cfg: sorterSetRnd_EvalAllBits_ReportCfg) = 
            cfg.sorterEvalMode

    let getStagePrefixCount  (cfg: sorterSetRnd_EvalAllBits_ReportCfg) = 
            cfg.stagePrefixCount

    let getSwitchGenModes (cfg: sorterSetRnd_EvalAllBits_ReportCfg) = 
            cfg.switchGenModes

    let getSwitchCounts (cfg: sorterSetRnd_EvalAllBits_ReportCfg) = 
            cfg.switchCounts

    let getSorterEvalReport (cfg: sorterSetRnd_EvalAllBits_ReportCfg) = 
            cfg.sorterEvalReport


    let getSorterSetRnd_EvalAllBitsCfg
            (order:order)
            (switchGenMode:switchGenMode)
            (rngGen:rngGen)
            (cfg:sorterSetRnd_EvalAllBits_ReportCfg)
        =
        SorterSetRnd_EvalAllBitsCfg.create 
            order
            rngGen
            switchGenMode
            (cfg.switchCounts order)
            (cfg.sorterCountCreates order)
            cfg.sorterEvalMode
            cfg.stagePrefixCount


    let getReportFileName
            (cfg:sorterSetRnd_EvalAllBits_ReportCfg) 
        =
        cfg.reportFileName


    let getReportHeader ()
        =
        SorterEval.reportHeader
            "eval_id\torder\tswitch_gen\tsortable_type"

    let getReportLines 
            (sorterSetEvalRet: sorterSetRnd_EvalAllBitsCfg->Result<sorterSetEval,string>)
            (cfg: sorterSetRnd_EvalAllBits_ReportCfg)
            (order:order)
            (switchGenMode:switchGenMode)
            (rngGen:rngGen)
        =
        let sorterSetEvalCfg = 
            cfg 
            |> getSorterSetRnd_EvalAllBitsCfg
                order
                switchGenMode
                rngGen

        let eval_id = 
            sorterSetEvalCfg
            |> SorterSetRnd_EvalAllBitsCfg.getSorterSetEvalId
            |> SorterSetEvalId.value
            |> string

        let order = 
            sorterSetEvalCfg
            |> SorterSetRnd_EvalAllBitsCfg.getOrder
            |> Order.value

        let switchGen = 
            sorterSetEvalCfg
            |> SorterSetRnd_EvalAllBitsCfg.getSwitchGenMode
            |> string

        let sortableSetCfgName = 
            sorterSetEvalCfg 
            |> SorterSetRnd_EvalAllBitsCfg.getSortableSetCertainCfg
            |> SortableSetCertainCfg.getConfigName

        let linePfx = 
            sprintf "%s\t%d\t%s\t%s"
                eval_id
                order
                switchGen
                sortableSetCfgName
             
        result {
            let! sorterSetEval = 
                sorterSetEvalRet sorterSetEvalCfg

            return 
                sorterSetEval 
                |> SorterSetEval.getSorterEvals
                |> Array.map(SorterEval.report linePfx)
        }


    let makeSorterSetEvalReport
            (cfg: sorterSetRnd_EvalAllBits_ReportCfg)
            (sortableSetCfgRet: sorterSetRnd_EvalAllBitsCfg->Result<sorterSetEval,string>)
        =
             seq {
                    for ordr in cfg.orders do
                        for sgm in cfg.switchGenModes do
                            for rgn in cfg.rngGenCreates do
                                getReportLines
                                    sortableSetCfgRet
                                    cfg
                                    ordr
                                    sgm
                                    rgn
                                                    
                 }
               