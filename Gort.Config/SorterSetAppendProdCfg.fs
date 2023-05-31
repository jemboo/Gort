namespace global

open System


type sorterSetAppendProdCfg = 
    private
        { 
          order: order
          rngGen: rngGen
          switchGenMode: switchGenMode
          switchCount: switchCount
          sorterCount: sorterCount
        }


module SorterSetAppendProdCfg =
    let create (order:order)
               (rngGen:rngGen)
               (switchGenMode:switchGenMode)
               (switchCount:switchCount)
               (sorterCount:sorterCount)
        =
        {
            order=order;
            rngGen=rngGen;
            switchGenMode=switchGenMode;
            switchCount=switchCount;
            sorterCount=sorterCount;
        }

    let getProperties (rdsg: sorterSetAppendProdCfg) = 
        [|
            ("order", rdsg.order :> obj);
            ("rngGen", rdsg.rngGen :> obj);
            ("switchGenMode", rdsg.switchGenMode :> obj);
            ("switchCount", rdsg.switchCount :> obj);
            ("sorterCount", rdsg.sorterCount :> obj);
        |]

    let getOrder (rdsg: sorterSetAppendProdCfg) = 
            rdsg.order

    let getRngGen (rdsg: sorterSetAppendProdCfg) = 
            rdsg.rngGen

    let getSwitchGenMode (rdsg: sorterSetAppendProdCfg) = 
            rdsg.switchGenMode

    let getSwitchCount (rdsg: sorterSetAppendProdCfg) = 
            rdsg.switchCount

    let getBaseId (cfg: sorterSetAppendProdCfg) 
        = 
        [|
          "sorterSetRndCfg" :> obj;
           cfg :> obj;
        |] |> GuidUtils.guidFromObjs
           |> SorterSetId.create

    let getId (cfg: sorterSetAppendProdCfg) 
        = 
        [|
          "sorterSetAppendProdCfg" :> obj;
           cfg :> obj;
        |] |> GuidUtils.guidFromObjs
           |> SorterSetId.create

    let getFileName (cfg:sorterSetAppendProdCfg) 
        =
        cfg |> getId |> SorterSetId.value |> string


    let getConfigName 
            (rdsg:sorterSetAppendProdCfg) 
        =
        sprintf "%d_%s"
            (rdsg |> getOrder |> Order.value)
            (rdsg |> getSwitchGenMode |> string)


    let getSorterCount (rdsg: sorterSetAppendProdCfg) 
        =
            rdsg.sorterCount