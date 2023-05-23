namespace global
open System

type ssmfr_EvalAllBitsMerge_ReportCfg = 
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


module Ssmfr_EvalAllBitsMerge_ReportCfg
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

    let getOrders (cfg: ssmfr_EvalAllBitsMerge_ReportCfg) = 
            cfg.orders

    let getRngGenCreates (cfg: ssmfr_EvalAllBitsMerge_ReportCfg) = 
            cfg.rngGenCreates

    let getSorterEvalMode  (cfg: ssmfr_EvalAllBitsMerge_ReportCfg) = 
            cfg.sorterEvalMode

    let getStagePrefixCount  (cfg: ssmfr_EvalAllBitsMerge_ReportCfg) = 
            cfg.stagePrefixCount

    let getSwitchGenModes (cfg: ssmfr_EvalAllBitsMerge_ReportCfg) = 
            cfg.switchGenModes

    let getSwitchCounts (cfg: ssmfr_EvalAllBitsMerge_ReportCfg) = 
            cfg.switchCounts

    let getSorterEvalReport (cfg: ssmfr_EvalAllBitsMerge_ReportCfg) = 
            cfg.sorterEvalReport


    let getSsmfr_EvalAllBitsCfg
            (order:order)
            (rngGenCreate:rngGen)
            (switchGenMode:switchGenMode)
            (rngGenMutate:rngGen)
            (mutationRate:mutationRate)
            (cfg:ssmfr_EvalAllBitsMerge_ReportCfg)
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


    let getSorterSetRndEvalCfg 
            (order:order)
            (switchGenMode:switchGenMode)
            (rngGenCreate:rngGen)
            (cfg:ssmfr_EvalAllBitsMerge_ReportCfg)
        =
        SorterSetRnd_EvalAllBitsCfg.create 
            order
            rngGenCreate
            switchGenMode
            (cfg.switchCounts order)
            (cfg.sorterCountCreates order)
            cfg.sorterEvalMode
            cfg.stagePrefixCount



    let getReportFileName
            (cfg:ssmfr_EvalAllBitsMerge_ReportCfg) 
        =
        cfg.reportFileName


    //let getReportHeader ()
    //    =
    //    SorterEval.reportHeader
    //        (sprintf "%s\t%s"
    //                "eval_id_parent\torder\tswitch_gen\tsortable_type"
    //                "eval_id_mutant\tmutation_rate")

    
    let getReportHeader ()
        =
        sprintf "%s\t%s"
            (SorterEval.reportHeader
                "eval_id\torder\tswitch_gen\tsortable_type\tmutation_rate")
            (SorterEval.reportHeaderP "")

    let getReportLines 
            (sorterSetParentEvalRet: sorterSetRnd_EvalAllBitsCfg->Result<sorterSetEval,string>)
            (sorterSetMutantEvalRet: ssmfr_EvalAllBitsCfg->Result<sorterSetEval,string>)
            (sorterParentMapRet: sorterSetMutatedFromRndCfg->Result<sorterSetParentMap,string>)
            (cfg: ssmfr_EvalAllBitsMerge_ReportCfg)
            (order:order)
            (switchGenMode:switchGenMode)
            (rngGenCreate:rngGen)
            (rngGenMutate:rngGen)
            (mutationRate:mutationRate)
        =

        let sorterSetParentEvalCfg = 
            cfg 
            |> getSorterSetRndEvalCfg
                order
                switchGenMode
                rngGenCreate

        let sorterSetMutantEvalCfg = 
            cfg 
            |> getSsmfr_EvalAllBitsCfg
                order
                rngGenCreate
                switchGenMode
                rngGenMutate
                mutationRate


        let sorterSetMutatedFromRndCfg = 
            sorterSetMutantEvalCfg
            |> Ssmfr_EvalAllBitsCfg.getSorterSetMutatedFromRndCfg


        let eval_id = 
            sorterSetMutantEvalCfg
            |> Ssmfr_EvalAllBitsCfg.getSorterSetEvalId
            |> SorterSetEvalId.value
            |> string

        let mutRate = 
            sorterSetMutantEvalCfg
            |> Ssmfr_EvalAllBitsCfg.getMutationRate
            |> MutationRate.value

        let order = 
            sorterSetMutantEvalCfg
            |> Ssmfr_EvalAllBitsCfg.getOrder
            |> Order.value

        let switchGen = 
            sorterSetMutantEvalCfg 
            |> Ssmfr_EvalAllBitsCfg.getSorterSetMutatedFromRndCfg
            |> SorterSetMutatedFromRndCfg.getSwitchGenMode
            |> string

        let sortableSetCfgName =
            sorterSetMutantEvalCfg 
            |> Ssmfr_EvalAllBitsCfg.getSortableSetCertainCfg
            |> SortableSetCertainCfg.getConfigName

        let linePfx = 
            sprintf "%s\t%d\t%s\t%s\t%f"
                eval_id
                order
                switchGen
                sortableSetCfgName
                mutRate

        result {

            let! sorterSetParentEval = 
                sorterSetParentEvalRet sorterSetParentEvalCfg

            let ssParentMap = 
                sorterSetParentEval
                |> SorterSetEval.getSorterEvals
                |> Array.map(fun sev -> (sev |> SorterEval.getSorterId, sev))
                |> Map.ofSeq

            let! sorterSetMutantEval = 
                sorterSetMutantEvalRet sorterSetMutantEvalCfg

            let ssMutatedMap = 
                sorterSetMutantEval
                |> SorterSetEval.getSorterEvals
                |> Array.map(fun sev -> (sev |> SorterEval.getSorterId, sev))
                |> Map.ofSeq

            let! sorterSetParentMap = 
                sorterParentMapRet sorterSetMutatedFromRndCfg

            let parentMap = 
                sorterSetParentMap |> SorterSetParentMap.getParentMap

            let tupes =
                ssMutatedMap 
                |> Map.toSeq
                |> Seq.map(
                    fun (mId, mEv) -> 
                    (
                        mEv, 
                        ssParentMap.[parentMap.[mId] |> SorterParentId.toSorterId]
                    )
                    )
                |> Seq.toArray


            return
                tupes
                |> Array.map(
                    fun (mEv, pEv) -> 
                        sprintf "%s\t%s" 
                            (SorterEval.report linePfx mEv)
                            (SorterEval.report "" pEv)
                            )

            //return 
            //    sorterSetMutantEval 
            //    |> SorterSetEval.getSorterEvals
            //    |> Array.map(SorterEval.report linePfx)
        }


    let makeSorterSetEvalReport
            (cfg: ssmfr_EvalAllBitsMerge_ReportCfg)
            (sorterSetParentEvalRet: sorterSetRnd_EvalAllBitsCfg->Result<sorterSetEval,string>)
            (sortableSetMutantRet: ssmfr_EvalAllBitsCfg->Result<sorterSetEval,string>)
            (sorterParentMapRet: sorterSetMutatedFromRndCfg->Result<sorterSetParentMap,string>)
        =
             seq {
                    for ordr in cfg.orders do
                        for sgm in cfg.switchGenModes do
                            for rgnC in cfg.rngGenCreates do
                                for rgnM in cfg.rngGenMutates do
                                    for mutR in cfg.mutationRates do
                                        getReportLines
                                            sorterSetParentEvalRet
                                            sortableSetMutantRet
                                            sorterParentMapRet
                                            cfg
                                            ordr
                                            sgm
                                            rgnC
                                            rgnM
                                            mutR
                                                    
                 }
               