namespace global

open System

type switchGenMode =
    | Switch
    | Stage
    | StageSymmetric


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

    let getSorterCount (rdsg: rndDenovoSorterSetCfg) = 
            rdsg.sorterCount

    let makeSorterSetId (rdsg: rndDenovoSorterSetCfg) = 
        [|
          rdsg.order :> obj;
          rdsg.rngGen;
          rdsg.switchGenMode;
          rdsg.switchPrefix;
          rdsg.switchCount;
          rdsg.sorterCount;
        |] |> GuidUtils.guidFromObjs
           |> SorterSetId.create


    let makeSorterSet (rdsg: rndDenovoSorterSetCfg) = 
        let sorterStId = makeSorterSetId rdsg
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
            





