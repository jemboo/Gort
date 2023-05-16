namespace global

open System

type ssmfr_EvalAllBitsCfg = 
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


module Ssmfr_EvalAllBitsCfg =
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

    let getOrder (cfg: ssmfr_EvalAllBitsCfg) = 
            cfg.order

    let getRngGenCreate (cfg: ssmfr_EvalAllBitsCfg) = 
            cfg.rngGenCreate

    let getSwitchGenMode (cfg: ssmfr_EvalAllBitsCfg) = 
            cfg.switchGenMode

    let getSwitchCount (cfg: ssmfr_EvalAllBitsCfg) = 
            cfg.switchCount

    let getSorterSetOriginalCfg (cfg:ssmfr_EvalAllBitsCfg)
        =
        SorterSetRndCfg.create 
            cfg.order
            cfg.rngGenCreate
            cfg.switchGenMode
            [||]
            cfg.switchCount
            cfg.sorterCountCreate


    let getOriginalSorterSetId (cfg: ssmfr_EvalAllBitsCfg) 
        = 
        cfg  |> getSorterSetOriginalCfg |> SorterSetRndCfg.getSorterSetId


    let getMutatedSorterSetId 
            (cfg: ssmfr_EvalAllBitsCfg) 
        = 
        [|
          (cfg.GetType()) :> obj;
          cfg :> obj;
        |] |> GuidUtils.guidFromObjs
           |> SorterSetId.create


    let getMutatedSorterSetFileName
            (cfg: ssmfr_EvalAllBitsCfg) 
        =
        cfg |> getMutatedSorterSetId |> SorterSetId.value |> string


    let getSorterSetMutator (cfg:ssmfr_EvalAllBitsCfg) 
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


    let getParentMapId  (cfg: ssmfr_EvalAllBitsCfg) 
        = 
        [|
          (cfg |> getOriginalSorterSetId) :> obj;
          (cfg |> getMutatedSorterSetId) :> obj;
        |] |> GuidUtils.guidFromObjs
           |> SorterParentMapId.create


    let getParentMapFileName
            (cfg: ssmfr_EvalAllBitsCfg) 
        =
        cfg |> getParentMapId |> SorterParentMapId.value |> string


    let makeMutantSorterSetAndParentMap 
            (lookup: sorterSetRndCfg -> Result<sorterSet, string>)
            (mutCfg: ssmfr_EvalAllBitsCfg)
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
