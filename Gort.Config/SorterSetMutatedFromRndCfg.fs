namespace global

open System

type sorterSetMutatedFromRndCfg = 
    private
        { 
          order: order
          rngGenCreate: rngGen
          switchGenMode: switchGenMode
          switchCount: switchCount
          sorterCountCreate: sorterCount
          rngGenMutate: rngGen
          sorterCountMutate: sorterCount
          mutationRate:mutationRate
        }


module SorterSetMutatedFromRndCfg =
    let create (order:order)
               (rngGenCreate:rngGen)
               (switchGenMode:switchGenMode)
               (switchCount:switchCount)
               (sorterCountCreate:sorterCount)
               (rngGenMutate:rngGen)
               (sorterCountMutate:sorterCount)
               (mutationRate:mutationRate)
        =
        {
            order=order;
            rngGenCreate=rngGenCreate;
            switchGenMode=switchGenMode;
            switchCount=switchCount;
            sorterCountCreate=sorterCountCreate;
            rngGenMutate=rngGenMutate;
            sorterCountMutate=sorterCountMutate;
            mutationRate=mutationRate
        }

    let getOrder (cfg: sorterSetMutatedFromRndCfg) = 
            cfg.order

    let getRngGenCreate (cfg: sorterSetMutatedFromRndCfg) = 
            cfg.rngGenCreate

    let getSwitchGenMode (cfg: sorterSetMutatedFromRndCfg) = 
            cfg.switchGenMode

    let getSwitchCount (cfg: sorterSetMutatedFromRndCfg) = 
            cfg.switchCount

    let getSorterSetOriginalCfg (cfg:sorterSetMutatedFromRndCfg)
        =
        SorterSetRndCfg.create 
            cfg.order
            cfg.rngGenCreate
            cfg.switchGenMode
            [||]
            cfg.switchCount
            cfg.sorterCountCreate


    let getOriginalSorterSetId (cfg: sorterSetMutatedFromRndCfg) 
        = 
        cfg  |> getSorterSetOriginalCfg |> SorterSetRndCfg.getSorterSetId


    let getMutatedSorterSetId 
            (cfg: sorterSetMutatedFromRndCfg) 
        = 
        [|
          (cfg.GetType()) :> obj;
          cfg :> obj;
        |] |> GuidUtils.guidFromObjs
           |> SorterSetId.create


    let getMutatedSorterSetFileName
            (cfg: sorterSetMutatedFromRndCfg) 
        =
        cfg |> getMutatedSorterSetId |> SorterSetId.value |> string


    let getSorterSetMutator (cfg:sorterSetMutatedFromRndCfg) 
        =
        let sorterUniformMutator = 
            SorterUniformMutator.create
                    None
                    None
                    cfg.switchGenMode
                    cfg.mutationRate
            |> sorterMutator.Uniform

        SorterSetMutator.load
            sorterUniformMutator
            (Some cfg.sorterCountMutate)
            cfg.rngGenMutate


    let getParentMapId  (cfg: sorterSetMutatedFromRndCfg) 
        = 
        [|
          (cfg |> getOriginalSorterSetId) :> obj;
          (cfg |> getMutatedSorterSetId) :> obj;
        |] |> GuidUtils.guidFromObjs
           |> SorterParentMapId.create


    let getParentMapFileName
            (cfg: sorterSetMutatedFromRndCfg) 
        =
        cfg |> getParentMapId |> SorterParentMapId.value |> string


    let makeMutantSorterSetAndParentMap 
            (lookup: sorterSetRndCfg -> Result<sorterSet, string>)
            (mutCfg: sorterSetMutatedFromRndCfg)
        =
        let parentCfg = 
            mutCfg 
            |> getSorterSetOriginalCfg

        result {
            let! parentSorterSet = lookup parentCfg
            let! parentMap, mutantSet = 
                    parentSorterSet |>
                        SorterSetMutator.createMutantSorterSetAndParentMap
                            (mutCfg |> getSorterSetMutator)

            return parentMap, mutantSet
        }     
