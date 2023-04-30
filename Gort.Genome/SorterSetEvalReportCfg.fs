namespace global

open System

type sorterSetEvalReportType =
    | PerfBins


type sorterSetEvalReportCfg = 
    private
        { 
          reportFileName: string
          sorterSetEvalCfg: sorterSetEvalCfg
          sorterSetEvalReportType: sorterSetEvalReportType
        }


module SorterSetEvalReportCfg 
        =
    let create 
            (sorterSetEvalCfg: sorterSetEvalCfg)
            (reportFileName:string)
            (sorterSetEvalReportType: sorterSetEvalReportType)
        =
        {
            sorterSetEvalCfg=sorterSetEvalCfg;
            reportFileName=reportFileName;
            sorterSetEvalReportType=sorterSetEvalReportType;
        }


    let getReportFileName  (cfg: sorterSetEvalReportCfg) 
        = 
        cfg.reportFileName

    let getSorterSetEvalCfg  (cfg: sorterSetEvalReportCfg) 
        = 
        cfg.sorterSetEvalCfg

    let getSorterSetEvalReportType 
            (cfg: sorterSetEvalReportCfg) 
        = 
            cfg.sorterSetEvalReportType


    //let getSorterSetCfg  (cfg: sorterSetEvalReportCfg) = 
    //        cfg.sorterSetCfg


    //let getSorterEvalMode  (cfg: sorterSetEvalReportCfg) = 
    //        cfg.sorterEvalMode


    //let getConfigName
    //        (cfg: sorterSetEvalReportCfg)
    //    =
    //    sprintf "%s__%s"
    //        (cfg |> getSortableSetCfg |> SortableSetCfg.getCfgName)
    //        (cfg |> getSorterSetCfg |> SorterSetCfg.getCfgName)


    let getReportLines
            (cfg:sorterSetEvalReportCfg) 
            (sorterSetEval:sorterSetEval)
        = ""
        //sprintf 
        //    "%s_%s"
        //        (cfg |> getConfigName)
        //        ( [|cfg :> obj|] |> GuidUtils.guidFromObjs |> string)


    //let getSorterSetEval
    //        (cfg: sorterSetEvalReportCfg)
    //        (sortableSetCfgRet: sortableSetCfg->sortableSet)
    //        (sorterSetRet: sorterSetCfg->sorterSet)
    //    =
    //    SorterSetEval.make
    //        (getSorterSetEvalId cfg)
    //        (getSorterEvalMode cfg)
    //        (cfg |> getSorterSetCfg |> sorterSetRet)
    //        (cfg |> getSortableSetCfg |> sortableSetCfgRet)
        