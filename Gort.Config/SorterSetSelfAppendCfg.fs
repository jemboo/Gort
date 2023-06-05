namespace global


type sorterSetSelfAppendCfg = 
    private
        { 
          order: order
          rngGen: rngGen
          switchGenMode: switchGenMode
          switchCount: switchCount
          sorterCountFactor: sorterCount
          sorterCount: sorterCount
        }


module SorterSetSelfAppendCfg =
    let create (order:order)
               (rngGen:rngGen)
               (switchGenMode:switchGenMode)
               (switchCount:switchCount)
               (sorterCountFactor:sorterCount)
        =
        {
            order=order;
            rngGen=rngGen;
            switchGenMode=switchGenMode;
            switchCount=switchCount;
            sorterCountFactor=sorterCountFactor;
            sorterCount= 
                ((SorterCount.value sorterCountFactor)
                *
                (SorterCount.value sorterCountFactor))
                |> SorterCount.create;
        }

    let getProperties (rdsg: sorterSetSelfAppendCfg) = 
        [|
            ("order", rdsg.order :> obj);
            ("rngGen", rdsg.rngGen :> obj);
            ("switchGenMode", rdsg.switchGenMode :> obj);
            ("switchCount", rdsg.switchCount :> obj);
            ("sorterCountFactor", rdsg.sorterCountFactor :> obj);
            ("sorterCount", rdsg.sorterCount :> obj);
        |]

    let getOrder (rdsg: sorterSetSelfAppendCfg) = 
            rdsg.order

    let getRngGen (rdsg: sorterSetSelfAppendCfg) = 
            rdsg.rngGen

    let getSwitchGenMode (rdsg: sorterSetSelfAppendCfg) = 
            rdsg.switchGenMode

    let getSwitchCount (rdsg: sorterSetSelfAppendCfg) = 
            rdsg.switchCount

    let getSorterSetConcatId (cfg: sorterSetSelfAppendCfg) 
        = 
        [|
          "sorterSetSelfAppendCfg" :> obj;
           cfg :> obj;
        |] |> GuidUtils.guidFromObjs
           |> SorterSetId.create

    let getSorterSetConcatFileName (cfg:sorterSetSelfAppendCfg) 
        =
        cfg |> getSorterSetConcatId |> SorterSetId.value |> string


    let getConfigName 
            (rdsg:sorterSetSelfAppendCfg) 
        =
        sprintf "%d_%s"
            (rdsg |> getOrder |> Order.value)
            (rdsg |> getSwitchGenMode |> string)


    let getSorterSetConcatCount (rdsg: sorterSetSelfAppendCfg) 
        =
        rdsg.sorterCount


    let getSorterSetFactorCfg (cfg:sorterSetSelfAppendCfg)
        =
        SorterSetRndCfg.create 
            cfg.order
            cfg.rngGen
            cfg.switchGenMode
            cfg.switchCount
            cfg.sorterCount


    let getSorterSetFactorId (cfg: sorterSetSelfAppendCfg) 
        = 
        cfg |> getSorterSetFactorCfg |> SorterSetRndCfg.getId


    let makeSorterSetConcatMap
            (cfg:sorterSetSelfAppendCfg)
        =
        SorterSetConcatMap.createForAppendSet
                (cfg |> getSorterSetFactorId)
                cfg.sorterCountFactor
                (cfg |> getSorterSetConcatId)


