namespace global

open System

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
            (rdsg:rndDenovoSorterSetCfg) 
        =
        sprintf "%s_%s"
            (rdsg |> getConfigName)
            ( [|rdsg :> obj|] |> GuidUtils.guidFromObjs |> string)


    let getSorterCount (rdsg: rndDenovoSorterSetCfg) = 
            rdsg.sorterCount


    let getSorterSet (rdsg: rndDenovoSorterSetCfg) = 
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
            


type sorterSetCfg = 
     | RndDenovo of rndDenovoSorterSetCfg


module SorterSetCfg =

    let getSorterSetId 
            (ssCfg: sorterSetCfg) 
        = 
        match ssCfg with
        | RndDenovo rdssCfg -> 
            rdssCfg |> RndDenovoSorterSetCfg.getSorterSetId


    let getSorterSet
            (ssCfg: sorterSetCfg) 
        = 
        match ssCfg with
        | RndDenovo cCfg -> 
            cCfg |> RndDenovoSorterSetCfg.getSorterSet


    let getOrder
            (ssCfg: sorterSetCfg) 
        = 
        match ssCfg with
        | RndDenovo cCfg -> 
            cCfg |> RndDenovoSorterSetCfg.getOrder


    let getCfgName
            (ssCfg: sorterSetCfg) 
        = 
        match ssCfg with
        | RndDenovo cCfg -> 
            cCfg |> RndDenovoSorterSetCfg.getConfigName


    let getFileName
            (ssCfg: sorterSetCfg) 
        = 
        match ssCfg with
        | RndDenovo cCfg -> 
            cCfg |> RndDenovoSorterSetCfg.getFileName