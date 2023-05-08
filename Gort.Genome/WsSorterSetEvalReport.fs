namespace global
open System


module WsSorterSetEvalReport
       =
    let useParall = true |> UseParallel.create

    let localFolder = "SorterSet_Eval_Report"
    let reportFileName = "Summary_Report"

    let appendLines (fileName:string) (data: string seq) =
        WsCommon.appendLines localFolder fileName data

    let writeToFile (fileName:string) (data: string) =
        WsCommon.writeToFile localFolder fileName data

    let readAllText (fileName:string) =
        WsCommon.readAllText localFolder fileName

    let readAllLines (fileName:string) =
        WsCommon.readAllLines localFolder fileName

    let mutable headerWritten = false
    let makeReport (cfg:sorterSetEvalReportCfg) 
        =
        if not headerWritten then
            headerWritten <-
                writeToFile reportFileName (SorterSetEvalReportCfg.getReportHeader cfg)
                |> Result.ExtractOrThrow

        let sorterSetEvalCfg = 
            cfg 
            |> SorterSetEvalReportCfg.getSorterSetEvalCfg

        let repLines =
                SorterSetEvalReportCfg.getReportLines 
                    WsSorterSetEval.getSorterSetEval
                    cfg

        appendLines reportFileName repLines


    let makeEm () =
        WsCommon.allSorterSetEvalReportCfgs ()
        |> Array.map(makeReport)