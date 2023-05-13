namespace global
open System


module WsSorterSetEvalReport
       =
    let reportFileName = "Summary_Report"

    let mutable headerWritten = false
    let makeReport (cfg:sorterSetEvalReportCfg) 
        =
        if not headerWritten then
            headerWritten <-
                WsFile.writeToFile 
                    wsFile.SorterEvalReport
                    reportFileName 
                    (SorterSetEvalReportCfg.getReportHeader cfg)
                |> Result.ExtractOrThrow


        let repLines =
                SorterSetEvalReportCfg.getReportLines 
                    WsSorterSetEval.getSorterSetEval
                    cfg

        WsFile.appendLines
                wsFile.SorterEvalReport
                reportFileName 
                repLines


    let makeEm () =
        WsCfgs.allSorterSetEvalReportCfgs ()
        |> Array.map(makeReport)