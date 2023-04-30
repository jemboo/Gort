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
            (cfg: sorterSetEvalCfg)
        =
        SorterSetEval.make
            (getSorterSetEvalId cfg)
            (getSorterEvalMode cfg)
            (cfg |> getSorterSetCfg |> sorterSetRet)
            (cfg |> getSortableSetCfg |> sortableSetCfgRet)
        