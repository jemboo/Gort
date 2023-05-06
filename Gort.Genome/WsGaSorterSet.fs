namespace global
open System


module WsGaSorterSet
       =
    let localFolder = "GaSorterSets"

    let appendLines (fileName:string) (data: string seq) =
        WsCommon.appendLines localFolder fileName data

    let writeToFile (fileName:string) (data: string) =
        WsCommon.writeToFile localFolder fileName data

    let readAllText (fileName:string) =
        WsCommon.readAllText localFolder fileName

    let readAllLines (fileName:string) =
        WsCommon.readAllLines localFolder fileName

    //let localFolder = "SorterSet_Eval_Report"
    //let reportHeaderPfx = "eval_id\torder\tsorter_type\tsortable_type"

    //let writeToFile (fileName:string) (data: string) =
    //    TextIO.writeToFile "txt" (Some WsCommon.wsRootDir) localFolder fileName data

    //let appendLines (fileName:string) (data: string seq) =
    //    TextIO.appendLines "txt" (Some WsCommon.wsRootDir) localFolder fileName data

    //let readAllText (fileName:string) =
    //    TextIO.readAllText "txt" (Some WsCommon.wsRootDir) localFolder fileName

    //let readAllLines (fileName:string) =
    //    TextIO.readAllLines "txt" (Some WsCommon.wsRootDir) localFolder fileName


    //let getSortableSet cfg =
    //        WsBinarySortableSets.getSortableSet cfg
    //        |> Result.ExtractOrThrow


    //let getSorterSet cfg =
    //        WsSorterSets.getSorterSet cfg
    //        |> Result.ExtractOrThrow


    //let runConfig (cfg:sorterSetEvalReportCfg) 
    //    =
    //    let sorterSetEvalCfg = 
    //        cfg 
    //        |> SorterSetEvalReportCfg.getSorterSetEvalCfg

    //    let eval_id = 
    //        sorterSetEvalCfg
    //        |> SorterSetEvalCfg.getSorterSetEvalId
    //        |> SorterSetEvalId.value
    //        |> string

    //    let order = 
    //        sorterSetEvalCfg
    //        |> SorterSetEvalCfg.getOrder
    //        |> Order.value

    //    let sorterSetCfgName = 
    //        sorterSetEvalCfg 
    //        |> SorterSetEvalCfg.getSorterSetCfg
    //        |> SorterSetCfg.getCfgName

    //    let sortableSetCfgName = 
    //        sorterSetEvalCfg 
    //        |> SorterSetEvalCfg.getSortableSetCfg
    //        |> SortableSetCfg.getCfgName
             
    //    let sorterSetEval = 
    //        WsSorterSetEval.getSorterSetEval sorterSetEvalCfg
    //        |> Result.ExtractOrThrow


    //    let linePfx = 
    //        sprintf "%s\t%d\t%s\t%s"
    //            eval_id
    //            order
    //            sorterSetCfgName
    //            sortableSetCfgName

    //    let repLines = 
    //        sorterSetEval 
    //        |> SorterSetEval.getSorterEvals
    //        |> Array.map(SorterEval.report linePfx)

    //    appendLines (cfg |> SorterSetEvalReportCfg.getReportFileName ) repLines


    //let orders = [|16; 18|] |> Array.map(Order.createNr)
    //let genModes = [switchGenMode.StageSymmetric; 
    //                switchGenMode.Switch; 
    //                switchGenMode.Stage]

    //let allCfgs () =
    //    [| 
    //        for cfg in WsSorterSetEval.allCfgs() do
    //            SorterSetEvalReportCfg.create
    //                cfg
    //                localFolder
    //                sorterSetEvalReportType.PerfBins
    //    |]


    //let makeEm () =
    //    let okRes =
    //        writeToFile localFolder (SorterEval.reportHeader reportHeaderPfx)
    //        |> Result.ExtractOrThrow
    //    allCfgs ()
    //    |> Array.map(runConfig)