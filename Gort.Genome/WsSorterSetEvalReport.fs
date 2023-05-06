namespace global
open System


module WsSorterSetEvalReport
       =
    let useParall = true |> UseParallel.create

    let localFolder = "SorterSet_Eval_Report"
    let reportFileName = "Summary_Report"
    let reportHeaderPfx = "eval_id\torder\tsorter_type\tsortable_type"

    let appendLines (fileName:string) (data: string seq) =
        WsCommon.appendLines localFolder fileName data

    let writeToFile (fileName:string) (data: string) =
        WsCommon.writeToFile localFolder fileName data

    let readAllText (fileName:string) =
        WsCommon.readAllText localFolder fileName

    let readAllLines (fileName:string) =
        WsCommon.readAllLines localFolder fileName


    let makeReport (cfg:sorterSetEvalReportCfg) 
        =
        let sorterSetEvalCfg = 
            cfg 
            |> SorterSetEvalReportCfg.getSorterSetEvalCfg

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
            WsSorterSetEval.getSorterSetEval sorterSetEvalCfg
            |> Result.ExtractOrThrow


        let linePfx = 
            sprintf "%s\t%d\t%s\t%s"
                eval_id
                order
                sorterSetCfgName
                sortableSetCfgName

        let repLines = 
            sorterSetEval 
            |> SorterSetEval.getSorterEvals
            |> Array.map(SorterEval.report linePfx)

        appendLines reportFileName repLines


    let allCfgs () =
        [| 
            for cfg in WsSorterSetEval.allCfgs() do
                SorterSetEvalReportCfg.create
                    cfg
                    sorterSetEvalReportType.PerfBins
        |]


    let makeEm () =
        let okRes =
            writeToFile reportFileName (SorterEval.reportHeader reportHeaderPfx)
            |> Result.ExtractOrThrow
        allCfgs ()
        |> Array.map(makeReport)