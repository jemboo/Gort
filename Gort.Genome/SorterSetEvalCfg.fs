namespace global

open System

type sorterSetEvalCfg = 
    private
        { 
          sortableSetCfg: sortableSetCfg
          sorterSetCfg: sorterSetCfg
          stagePrefixCount: stageCount
          sorterEvalMode: sorterEvalMode
        }


module SorterSetEvalCfg 
        =
    let create 
            (sortableSetCfg:sortableSetCfg)
            (sorterSetCfg:sorterSetCfg)
            (stagePrefixCount: stageCount)
            (sorterEvalMode: sorterEvalMode)
        =
        {
            sortableSetCfg=sortableSetCfg;
            sorterSetCfg=sorterSetCfg;
            stagePrefixCount=stagePrefixCount;
            sorterEvalMode=sorterEvalMode
        }

    let getOrder (cfg: sorterSetEvalCfg) = 
            cfg.sortableSetCfg 
            |> SortableSetCfg.getOrder

    let getSorterSetEvalId  (cfg: sorterSetEvalCfg) = 
        [|
          cfg :> obj;
        |] |> GuidUtils.guidFromObjs
           |> SorterSetEvalId.create


    let getSortableSetCfg (cfg: sorterSetEvalCfg) = 
            cfg.sortableSetCfg


    let getSorterSetCfg  (cfg: sorterSetEvalCfg) = 
            cfg.sorterSetCfg


    let getStagePrefixCount  (cfg: sorterSetEvalCfg) = 
            cfg.stagePrefixCount


    let getSorterEvalMode  (cfg: sorterSetEvalCfg) = 
            cfg.sorterEvalMode


    let getConfigName
            (cfg: sorterSetEvalCfg)
        =
        sprintf "%s__%s"
            (cfg |> getSortableSetCfg |> SortableSetCfg.getCfgName)
            (cfg |> getSorterSetCfg |> SorterSetCfg.getCfgName)


    let getFileName
            (cfg:sorterSetEvalCfg) 
        =
        sprintf 
            "%s_%s"
                (cfg |> getConfigName)
                ( [|cfg :> obj|] |> GuidUtils.guidFromObjs |> string)


    let getSorterSetEval
            (sortableSetCfgRet: sortableSetCfg->sortableSet)
            (sorterSetRet: sorterSetCfg->sorterSet)
            (up:useParallel)
            (cfg: sorterSetEvalCfg)
        =
        let ssEval = 
           SorterSetEval.make
            (getSorterSetEvalId cfg)
            (getSorterEvalMode cfg)
            (cfg |> getSorterSetCfg |> sorterSetRet)
            (cfg |> getSortableSetCfg |> sortableSetCfgRet)
            up

        let ordr = cfg |> getOrder
        let tCmod = cfg |> getStagePrefixCount
        SorterSetEval.load
            (ssEval |> SorterSetEval.getSorterSetEvalId)
            (ssEval |> SorterSetEval.getSorterSetlId)
            (ssEval |> SorterSetEval.getSortableSetId)
            (ssEval |> SorterSetEval.getSorterEvals  
                |> Array.map(SorterEval.modifyForPrefix ordr tCmod))
               