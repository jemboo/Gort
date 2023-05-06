namespace global

type gaSorterSetCfgType =
    | PerfBins


type gaSorterSetShcCfg = 
    private
        { 
          sorterSetEvalCfg: sorterSetEvalCfg
          generations:generation
          mutationRate: mutationRate
          reportFileName: string
          summaryReportFreq: int
          fullReportFreq: int
        }


module gaSorterSetShcCfg 
        =
    let create 
            (sorterSetEvalCfg: sorterSetEvalCfg)
            (generations: generation)
            (mutationRate: mutationRate)
            (reportFileName:string)
            (summaryReportFreq:int)
            (fullReportFreq:int)
        =
        {
            sorterSetEvalCfg=sorterSetEvalCfg;
            generations=generations;
            mutationRate=mutationRate;
            reportFileName=reportFileName;
            summaryReportFreq=summaryReportFreq;
            fullReportFreq=fullReportFreq;
        }


    let getSorterSetEvalCfg  (cfg: gaSorterSetShcCfg) 
        =
        cfg.sorterSetEvalCfg


    let getGenerations  (cfg: gaSorterSetShcCfg) 
        =
        cfg.generations


    let getMutationRate  (cfg: gaSorterSetShcCfg) 
        =
        cfg.mutationRate


    let getReportFileName  (cfg: gaSorterSetShcCfg) 
        =
        cfg.reportFileName


    let getSummaryReportFreq  (cfg: gaSorterSetShcCfg) 
        =
        cfg.summaryReportFreq


    let getFullReportFreq  (cfg: gaSorterSetShcCfg) 
        =
        cfg.fullReportFreq
