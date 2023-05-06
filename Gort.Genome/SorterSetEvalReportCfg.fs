namespace global

type sorterSetEvalReportType =
    | PerfBins


type sorterSetEvalReportCfg = 
    private
        { 
          sorterSetEvalCfg: sorterSetEvalCfg
          sorterSetEvalReportType: sorterSetEvalReportType
        }


module SorterSetEvalReportCfg 
        =
    let create 
            (sorterSetEvalCfg: sorterSetEvalCfg)
            (sorterSetEvalReportType: sorterSetEvalReportType)
        =
        {
            sorterSetEvalCfg=sorterSetEvalCfg;
            sorterSetEvalReportType=sorterSetEvalReportType;
        }

    let getSorterSetEvalCfg  (cfg: sorterSetEvalReportCfg) 
        = 
        cfg.sorterSetEvalCfg

    let getSorterSetEvalReportType 
            (cfg: sorterSetEvalReportCfg) 
        = 
            cfg.sorterSetEvalReportType
