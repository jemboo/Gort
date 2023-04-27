namespace global

open System

type sorterSetEvalCfg = 
    private
        { 
          sortableSetCfg: sortableSetCfg
          sorterSetCfg: sorterSetCfg
          sorterEvalMode: sorterEvalMode
        }


module SorterSetEvalCfg 
        =
    let create 
            (sortableSetCfg:sortableSetCfg)
            (sorterSetCfg:sorterSetCfg)
            (sorterEvalMode: sorterEvalMode)
        =
        {
            sortableSetCfg=sortableSetCfg;
            sorterSetCfg=sorterSetCfg;
            sorterEvalMode=sorterEvalMode
        }


    let getId (cfg: sorterSetEvalCfg) = 
            cfg.sortableSetCfg


    let getSorterSetEvalId  (cfg: sorterSetEvalCfg) = 
        [|
          cfg :> obj;
        |] |> GuidUtils.guidFromObjs
           |> SorterSetEvalId.create


    let getSortableSetCfg (cfg: sorterSetEvalCfg) = 
            cfg.sortableSetCfg


    let getSorterSetCfg  (cfg: sorterSetEvalCfg) = 
            cfg.sorterSetCfg


    let getSorterEvalMode  (cfg: sorterSetEvalCfg) = 
            cfg.sorterEvalMode


    let getConfigName
            (cfg: sorterSetEvalCfg)
        =
        sprintf "%s__%s"
            (cfg |> getSortableSetCfg |> SortableSetCfg.getCfgName)
            (cfg |> getSorterSetCfg |> SorterSetCfg.getCfgName)


    let getSorterSetPerf 
            (cfg: sorterSetEvalCfg)
            (sortableSetCfgRet: sortableSetCfg->sortableSet)
            (sorterSetRet: sorterSetCfg->sorterSet)
        =
        SorterSetEval.make
            (getSorterSetEvalId cfg)
            (getSorterEvalMode cfg)
            (cfg |> getSortableSetCfg |> sortableSetCfgRet)
            (cfg |> getSorterSetCfg |> sorterSetRet)
        