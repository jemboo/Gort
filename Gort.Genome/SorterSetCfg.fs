namespace global


type rndDenovoSorterSetCfg = 
    private
        { 
          order: order
          rngGen: rngGen
          switchGenMode: switchGenMode
          switchPrefix: switch[]
          switchCount: switchCount
          sorterCount: sorterCount
        }


module RndDenovoSorterSetCfg =
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

    let getOrder (rdsg: rndDenovoSorterSetCfg) = 
            rdsg.order

    let getRngGen (rdsg: rndDenovoSorterSetCfg) = 
            rdsg.rngGen

    let getSwitchGenMode (rdsg: rndDenovoSorterSetCfg) = 
            rdsg.switchGenMode

    let getSwitchPrefix (rdsg: rndDenovoSorterSetCfg) = 
            rdsg.switchPrefix

    let getSwitchCount (rdsg: rndDenovoSorterSetCfg) = 
            rdsg.switchCount

    let getSorterSetId 
            (rdsg: rndDenovoSorterSetCfg) 
        = 
        [|
          rdsg :> obj;
        |] |> GuidUtils.guidFromObjs
           |> SorterSetId.create

    let getConfigName 
            (rdsg:rndDenovoSorterSetCfg) 
        =
        sprintf "%d_%s_%d"
            (rdsg |> getOrder |> Order.value)
            (rdsg |> getSwitchGenMode |> string)
            (rdsg |> getSwitchPrefix |> Array.length)


    let getFileName
            (cfg:rndDenovoSorterSetCfg) 
        =
        cfg |> getSorterSetId |> SorterSetId.value |> string
        //sprintf "%s_%s"
        //    (rdsg |> getConfigName)
        //    ( [|rdsg :> obj|] |> GuidUtils.guidFromObjs |> string)


    let getSorterCount (rdsg: rndDenovoSorterSetCfg) = 
            rdsg.sorterCount


    let makeSorterSet (rdsg: rndDenovoSorterSetCfg) = 
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
            

type sorterSetCfgExplicit = 
    private
        { 
          order: order option
          sorterSetId: sorterSetId
          description: string
          sorterCount: sorterCount option
        }


module SorterSetCfgExplicit =
    let create (order:order option)
               (sorterSetId:sorterSetId)
               (description:string)
               (sorterCount:sorterCount option)
        =
        {
            order=order;
            sorterSetId=sorterSetId;
            description=description;
            sorterCount=sorterCount;
        }

    let getOrder (rdsg: sorterSetCfgExplicit) = 
            match rdsg.order with
            | Some o -> o
            | None -> -1 |> Order.createNr

    let getSorterSetId (rdsg: sorterSetCfgExplicit) = 
            rdsg.sorterSetId

    let getDescription (rdsg: sorterSetCfgExplicit) = 
            rdsg.description

    let getConfigName 
            (rdsg:sorterSetCfgExplicit) 
        =
        sprintf "%s"
            (rdsg |> getDescription |> string)


    let getFileName
            (cfg:sorterSetCfgExplicit) 
        =
        cfg |> getSorterSetId |> SorterSetId.value |> string


    let getSorterCount (rdsg: sorterSetCfgExplicit) =
            match rdsg.sorterCount with
            | Some o -> o
            | None -> -1 |> SorterCount.create

    let makeSorterSet (rdsg: sorterSetCfgExplicit) = 
        failwith "not implemented"
            


type sorterSetCfg = 
     | RndDenovo of rndDenovoSorterSetCfg
     | Explicit of sorterSetCfgExplicit


module SorterSetCfg =

    let getSorterSetId 
            (ssCfg: sorterSetCfg) 
        = 
        match ssCfg with
        | RndDenovo rdssCfg -> 
            rdssCfg |> RndDenovoSorterSetCfg.getSorterSetId
        | Explicit eC ->
            eC |> SorterSetCfgExplicit.getSorterSetId


    let makeSorterSet
            (ssCfg: sorterSetCfg) 
        = 
        match ssCfg with
        | RndDenovo cCfg -> 
            cCfg |> RndDenovoSorterSetCfg.makeSorterSet
        | Explicit eC ->
            eC |> SorterSetCfgExplicit.makeSorterSet


    let getOrder
            (ssCfg: sorterSetCfg) 
        = 
        match ssCfg with
        | RndDenovo cCfg -> 
            cCfg |> RndDenovoSorterSetCfg.getOrder
        | Explicit eC ->
            eC |> SorterSetCfgExplicit.getOrder


    let getSorterSetCt
            (ssCfg: sorterSetCfg) 
        = 
        match ssCfg with
        | RndDenovo cCfg -> 
            cCfg |> RndDenovoSorterSetCfg.getSorterCount
        | Explicit eC ->
            eC |> SorterSetCfgExplicit.getSorterCount


    let getCfgName
            (ssCfg: sorterSetCfg) 
        = 
        match ssCfg with
        | RndDenovo cCfg -> 
            cCfg |> RndDenovoSorterSetCfg.getConfigName
        | Explicit eC ->
            eC |> SorterSetCfgExplicit.getConfigName


    let getFileName
            (ssCfg: sorterSetCfg) 
        = 
        match ssCfg with
        | RndDenovo cCfg -> 
            cCfg |> RndDenovoSorterSetCfg.getFileName
        | Explicit eC ->
            eC |> SorterSetCfgExplicit.getFileName