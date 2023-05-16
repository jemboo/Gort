namespace global


type sorterSetRndCfg = 
    private
        { 
          order: order
          rngGen: rngGen
          switchGenMode: switchGenMode
          switchPrefix: switch[]
          switchCount: switchCount
          sorterCount: sorterCount
        }


module SorterSetRndCfg =
    let create (order:order)
               (rngGen:rngGen)
               (switchGenMode:switchGenMode)
               (switchPrefix:switch[])
               (switchCount:switchCount)
               (sorterCount:sorterCount)
        =
        {
            order=order;
            rngGen=rngGen;
            switchGenMode=switchGenMode;
            switchPrefix=switchPrefix;
            switchCount=switchCount;
            sorterCount=sorterCount;
        }

    let getOrder (rdsg: sorterSetRndCfg) = 
            rdsg.order

    let getRngGen (rdsg: sorterSetRndCfg) = 
            rdsg.rngGen

    let getSwitchGenMode (rdsg: sorterSetRndCfg) = 
            rdsg.switchGenMode

    let getSwitchPrefix (rdsg: sorterSetRndCfg) = 
            rdsg.switchPrefix

    let getSwitchCount (rdsg: sorterSetRndCfg) = 
            rdsg.switchCount

    let getSorterSetId 
            (cfg: sorterSetRndCfg) 
        = 
        [|
          (cfg.GetType()) :> obj;
           cfg :> obj;
        |] |> GuidUtils.guidFromObjs
           |> SorterSetId.create

    let getConfigName 
            (rdsg:sorterSetRndCfg) 
        =
        sprintf "%d_%s_%d"
            (rdsg |> getOrder |> Order.value)
            (rdsg |> getSwitchGenMode |> string)
            (rdsg |> getSwitchPrefix |> Array.length)


    let getFileName
            (cfg:sorterSetRndCfg) 
        =
        cfg |> getSorterSetId |> SorterSetId.value |> string


    let getSorterCount (rdsg: sorterSetRndCfg) = 
            rdsg.sorterCount


    let makeSorterSet (rdsg: sorterSetRndCfg) = 
        let sorterStId = getSorterSetId rdsg
        let randy = rdsg.rngGen |> Rando.fromRngGen
        let nextRng () =
            randy |> Rando.nextRngGen

        match rdsg.switchGenMode with
        | Switch -> 
            SorterSet.createRandomSwitches
                sorterStId
                rdsg.sorterCount
                rdsg.order
                rdsg.switchPrefix
                rdsg.switchCount
                nextRng

        | Stage -> 
            SorterSet.createRandomStages2
                sorterStId
                rdsg.sorterCount
                rdsg.order
                rdsg.switchPrefix
                rdsg.switchCount
                nextRng

        | StageSymmetric -> 
            SorterSet.createRandomSymmetric
                sorterStId
                rdsg.sorterCount
                rdsg.order
                rdsg.switchPrefix
                rdsg.switchCount
                nextRng
           