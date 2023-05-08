namespace global


type sorterSetEvalReportCfgFull = 
    private
        {
          sorterSetEvalCfg: sorterSetEvalCfg
        }


module SorterSetEvalReportCfgFull 
        =
    let create 
            (sorterSetEvalCfg: sorterSetEvalCfg)
        =
        {
            sorterSetEvalCfg=sorterSetEvalCfg;
        }

    let getSorterSetEvalCfg  (cfg: sorterSetEvalReportCfgFull) 
        = 
        cfg.sorterSetEvalCfg


    let getReportHeader ()
        =
        SorterEval.reportHeader
            "eval_id\torder\tsorter_type\tsortable_type"


    let getReportLines 
            (sorterSetEvalRet: sorterSetEvalCfg->Result<sorterSetEval,string>)
            (cfg: sorterSetEvalReportCfgFull) 
        =
        let sorterSetEvalCfg = 
            cfg 
            |> getSorterSetEvalCfg

        let eval_id = 
            sorterSetEvalCfg
            |> SorterSetEvalCfg.getSorterSetEvalId
            |> SorterSetEvalId.value
            |> string

        let order = 
            sorterSetEvalCfg
            |> SorterSetEvalCfg.getOrder
            |> Order.value

        let sorterSetCfgName = 
            sorterSetEvalCfg 
            |> SorterSetEvalCfg.getSorterSetCfg
            |> SorterSetCfg.getCfgName

        let sortableSetCfgName = 
            sorterSetEvalCfg 
            |> SorterSetEvalCfg.getSortableSetCfg
            |> SortableSetCfg.getCfgName
             
        let sorterSetEval = 
            sorterSetEvalRet sorterSetEvalCfg
            |> Result.ExtractOrThrow


        let linePfx = 
            sprintf "%s\t%d\t%s\t%s"
                eval_id
                order
                sorterSetCfgName
                sortableSetCfgName

        sorterSetEval 
            |> SorterSetEval.getSorterEvals
            |> Array.map(SorterEval.report linePfx)



type sorterSetEvalReportCfg =
     | Full of sorterSetEvalReportCfgFull


module SorterSetEvalReportCfg =

    let createFull (sorterSetEvalCfg:sorterSetEvalCfg)
        =
        SorterSetEvalReportCfgFull.create sorterSetEvalCfg
        |> sorterSetEvalReportCfg.Full


    let getSorterSetEvalCfg  (cfg: sorterSetEvalReportCfg) 
        = 
        match cfg with
        | Full cfgFull ->
            SorterSetEvalReportCfgFull.getSorterSetEvalCfg
                            cfgFull


    let getReportHeader (cfg: sorterSetEvalReportCfg) 
        = 
        match cfg with
        | Full _ ->
            SorterSetEvalReportCfgFull.getReportHeader ()



    let getReportLines
            (sorterSetEvalRet: sorterSetEvalCfg->Result<sorterSetEval,string>)
            (cfg: sorterSetEvalReportCfg)

        = 
        match cfg with
        | Full cfgFull ->
            SorterSetEvalReportCfgFull.getReportLines
                    sorterSetEvalRet
                    cfgFull
