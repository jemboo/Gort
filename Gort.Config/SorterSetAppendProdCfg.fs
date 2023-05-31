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


    let makeSorterSet
            (save: string -> sorterSet -> Result<bool, string>)
            (rdsg: sorterSetAppendProdCfg) 
        =
        let sorterStIdBase = getBaseId rdsg
        let sorterStId = getId rdsg
        let randy = rdsg.rngGen |> Rando.fromRngGen
        let nextRng () =
            randy |> Rando.nextRngGen
        result {
            let ssBase =
                match rdsg.switchGenMode with
                | Switch -> 
                    SorterSet.createRandomSwitches
                        sorterStIdBase
                        rdsg.sorterCount
                        rdsg.order
                        []
                        rdsg.switchCount
                        nextRng

                | Stage -> 
                    SorterSet.createRandomStages2
                        sorterStIdBase
                        rdsg.sorterCount
                        rdsg.order
                        []
                        rdsg.switchCount
                        nextRng

                | StageSymmetric -> 
                    SorterSet.createRandomSymmetric
                        sorterStIdBase
                        rdsg.sorterCount
                        rdsg.order
                        []
                        rdsg.switchCount
                        nextRng

            let mapping, ssRet = 
                sorterStId 
                        |> SorterSet.createAppendSet 
                            (ssBase |> SorterSet.getSorters)
                            rdsg.order

            let! wasSaved = save (rdsg |> getFileName) ssRet
            return ssRet
        }


    let getSorterSet
            (lookup: string -> Result<sorterSet, string>)
            (save: string -> sorterSet -> Result<bool, string>)
            (rdsg: sorterSetAppendProdCfg)
        =
        result {
            let loadRes  = 
                result {
                    let! mut = lookup (rdsg |> getFileName)
                    return mut
                }

            match loadRes with
            | Ok mut -> return mut
            | Error _ -> return! (makeSorterSet save rdsg)
        }