namespace global


type sorterSetRndCfg = 
    private
        { 
          order: order
          rngGen: rngGen
          switchGenMode: switchGenMode
          switchCount: switchCount
          sorterCount: sorterCount
        }


module SorterSetRndCfg =
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

    let getProperties (rdsg: sorterSetRndCfg) = 
        [|
            ("order", rdsg.order :> obj);
            ("rngGen", rdsg.rngGen :> obj);
            ("switchGenMode", rdsg.switchGenMode :> obj);
            ("switchCount", rdsg.switchCount :> obj);
            ("sorterCount", rdsg.sorterCount :> obj);
        |]

    let getOrder (rdsg: sorterSetRndCfg) = 
            rdsg.order

    let getRngGen (rdsg: sorterSetRndCfg) = 
            rdsg.rngGen

    let getSwitchGenMode (rdsg: sorterSetRndCfg) = 
            rdsg.switchGenMode

    let getSwitchCount (rdsg: sorterSetRndCfg) = 
            rdsg.switchCount

    let getId 
            (cfg: sorterSetRndCfg) 
        = 
        [|
          "sorterSetRndCfg" :> obj;
           cfg :> obj;
        |] |> GuidUtils.guidFromObjs
           |> SorterSetId.create

    let getFileName
            (cfg:sorterSetRndCfg) 
        =
        cfg |> getId |> SorterSetId.value |> string


    let getConfigName 
            (rdsg:sorterSetRndCfg) 
        =
        sprintf "%d_%s"
            (rdsg |> getOrder |> Order.value)
            (rdsg |> getSwitchGenMode |> string)


    let getSorterCount (rdsg: sorterSetRndCfg) = 
            rdsg.sorterCount


    let makeSorterSet
            (save: string -> sorterSet -> Result<bool, string>)
            (rdsg: sorterSetRndCfg) 
        = 
        let sorterStId = getId rdsg
        let randy = rdsg.rngGen |> Rando.fromRngGen
        let nextRng () =
            randy |> Rando.nextRngGen
        result {
            let ssRet =
                match rdsg.switchGenMode with
                | Switch -> 
                    SorterSet.createRandomSwitches
                        sorterStId
                        rdsg.sorterCount
                        rdsg.order
                        []
                        rdsg.switchCount
                        nextRng

                | Stage -> 
                    SorterSet.createRandomStages2
                        sorterStId
                        rdsg.sorterCount
                        rdsg.order
                        []
                        rdsg.switchCount
                        nextRng

                | StageSymmetric -> 
                    SorterSet.createRandomSymmetric
                        sorterStId
                        rdsg.sorterCount
                        rdsg.order
                        []
                        rdsg.switchCount
                        nextRng
            let! wasSaved = save (rdsg |> getFileName) ssRet
            return ssRet
        }


    let getSorterSet
            (lookup: string -> Result<sorterSet, string>)
            (save: string -> sorterSet -> Result<bool, string>)
            (rdsg: sorterSetRndCfg)
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