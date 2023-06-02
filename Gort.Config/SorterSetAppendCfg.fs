namespace global


type sorterSetAppendCfg = 
    private
        { 
          order: order
          rngGen: rngGen
          switchGenMode: switchGenMode
          switchCount: switchCount
          sorterCountFactor: sorterCount
          sorterCount: sorterCount
        }


module SorterSetAppendCfg =
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

    let getProperties (rdsg: sorterSetAppendCfg) = 
        [|
            ("order", rdsg.order :> obj);
            ("rngGen", rdsg.rngGen :> obj);
            ("switchGenMode", rdsg.switchGenMode :> obj);
            ("switchCount", rdsg.switchCount :> obj);
            ("sorterCountFactor", rdsg.sorterCountFactor :> obj);
            ("sorterCount", rdsg.sorterCount :> obj);
        |]

    let getOrder (rdsg: sorterSetAppendCfg) = 
            rdsg.order

    let getRngGen (rdsg: sorterSetAppendCfg) = 
            rdsg.rngGen

    let getSwitchGenMode (rdsg: sorterSetAppendCfg) = 
            rdsg.switchGenMode

    let getSwitchCount (rdsg: sorterSetAppendCfg) = 
            rdsg.switchCount

    let getBaseId (cfg: sorterSetAppendCfg) 
        = 
        [|
          "sorterSetRndCfg" :> obj;
           cfg :> obj;
        |] |> GuidUtils.guidFromObjs
           |> SorterSetId.create

    let getId (cfg: sorterSetAppendCfg) 
        = 
        [|
          "sorterSetAppendCfg" :> obj;
           cfg :> obj;
        |] |> GuidUtils.guidFromObjs
           |> SorterSetId.create

    let getFileName (cfg:sorterSetAppendCfg) 
        =
        cfg |> getId |> SorterSetId.value |> string


    let getConfigName 
            (rdsg:sorterSetAppendCfg) 
        =
        sprintf "%d_%s"
            (rdsg |> getOrder |> Order.value)
            (rdsg |> getSwitchGenMode |> string)


    let getSorterCount (rdsg: sorterSetAppendCfg) 
        =
        rdsg.sorterCount


    let getSorterSetFactorCfg (cfg:sorterSetAppendCfg)
        =
        SorterSetRndCfg.create 
            cfg.order
            cfg.rngGen
            cfg.switchGenMode
            cfg.switchCount
            cfg.sorterCount

