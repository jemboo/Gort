namespace global
open System


module WsSorterSetEvalReport
       =
    let useParall = true |> UseParallel.create

    let sorterSetEvalReport = "SorterSet_Eval_Report"

    let writeToFile (fileName:string) (data: string) =
        TextIO.writeToFile "txt" (Some WsCommon.wsRootDir) sorterSetEvalReport fileName data

    let appendLines (fileName:string) (data: string seq) =
        TextIO.appendLines "txt" (Some WsCommon.wsRootDir) sorterSetEvalReport fileName data

    let readAllText (fileName:string) =
        TextIO.readAllText "txt" (Some WsCommon.wsRootDir) sorterSetEvalReport fileName

    let readAllLines (fileName:string) =
        TextIO.readAllLines "txt" (Some WsCommon.wsRootDir) sorterSetEvalReport fileName


    let getSortableSet cfg =
            WsBinarySortableSets.getSortableSet cfg
            |> Result.ExtractOrThrow


    let getSorterSet cfg =
            WsSorterSets.getSorterSet cfg
            |> Result.ExtractOrThrow


    let runConfig (cfg:sorterSetEvalReportCfg) 
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

        appendLines (cfg |> SorterSetEvalReportCfg.getReportFileName ) repLines


    let orders = [|16; 18|] |> Array.map(Order.createNr)
    let genModes = [switchGenMode.StageSymmetric; 
                    switchGenMode.Switch; 
                    switchGenMode.Stage]

    let allCfgs () =
        [| 
            for cfg in WsSorterSetEval.allCfgs() do
                SorterSetEvalReportCfg.create
                    cfg
                    sorterSetEvalReport
                    sorterSetEvalReportType.PerfBins
        |]


    let makeEm () =
        let okses =
            writeToFile sorterSetEvalReport (SorterEval.reportHeader "eval_id\torder\tsorter_type\tsortable_type")
            |> Result.ExtractOrThrow
        allCfgs ()
        |> Array.map(runConfig)