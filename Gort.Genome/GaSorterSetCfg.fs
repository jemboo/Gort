namespace global

type orgSelector =
        generation->sorterEval->Energy

// shc with sorter replicas acting independently
type gaSorterShcCfg = 
    private
        {
          sorterSetCfg:sorterSetCfg
          sortableSetCfg:sortableSetCfg
          sorterMutator: sorterMutator
          orgSelector: orgSelector
          generationCount:generation
          rngGen: rngGen
          reportFileName: string
          summaryReportFreq: int
          fullReportFreq: int
        }


module gaSorterSetShcCfg 
        =
    let create 
            (sorterSetCfg: sorterSetCfg)
            (sortableSetCfg: sortableSetCfg)
            (sorterMutator: sorterMutator)
            (orgSelector: orgSelector)
            (generationCount: generation)
            (rngGen: rngGen)
            (reportFileName:string)
            (summaryReportFreq:int)
            (fullReportFreq:int)
        =
        {
            sorterSetCfg=sorterSetCfg;
            sortableSetCfg=sortableSetCfg;
            sorterMutator=sorterMutator;
            orgSelector=orgSelector;
            generationCount=generationCount;
            rngGen=rngGen
            reportFileName=reportFileName;
            summaryReportFreq=summaryReportFreq;
            fullReportFreq=fullReportFreq;
        }


    let getSorterSetCfg  (cfg: gaSorterShcCfg) 
        =
        cfg.sorterSetCfg

    let getSortableSetCfg  (cfg: gaSorterShcCfg) 
        =
        cfg.sortableSetCfg

    let getSorterMutator  (cfg: gaSorterShcCfg) 
        =
        cfg.sorterMutator

    let getOrgSelector  (cfg: gaSorterShcCfg) 
        =
        cfg.orgSelector

    let getGenerationCount  (cfg: gaSorterShcCfg) 
        =
        cfg.generationCount

    let getRndGen  (cfg: gaSorterShcCfg) 
        =
        cfg.rngGen

    let getSummaryReportFreq  (cfg: gaSorterShcCfg) 
        =
        cfg.summaryReportFreq

    let getFullReportFreq  (cfg: gaSorterShcCfg) 
        =
        cfg.fullReportFreq
