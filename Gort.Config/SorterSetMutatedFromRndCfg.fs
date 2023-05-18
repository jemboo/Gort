namespace global


module SorterSetMutatedFromRndCfg =
    let create (order:order)
               (rngGenCreate:rngGen)
               (switchGenMode:switchGenMode)
               (switchCount:switchCount)
               (sorterCountOriginal:sorterCount)
               (rngGenMutate:rngGen)
               (sorterCountMutated:sorterCount)
               (mutationRate:mutationRate)
        =
        {
            order=order;
            rngGenCreate=rngGenCreate;
            switchGenMode=switchGenMode;
            switchCount=switchCount;
            sorterCountOriginal=sorterCountOriginal;
            rngGenMutate=rngGenMutate;
            sorterCountMutated=sorterCountMutated;
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

    let getMutationRate (cfg: sorterSetMutatedFromRndCfg) =
            cfg.mutationRate

    let getSorterCountMutated (cfg: sorterSetMutatedFromRndCfg) =
            cfg.sorterCountMutated

    let getSorterCountOriginal (cfg: sorterSetMutatedFromRndCfg) =
            cfg.sorterCountOriginal


    let getSorterSetOriginalCfg (cfg:sorterSetMutatedFromRndCfg)
        =
        SorterSetRndCfg.create 
            cfg.order
            cfg.rngGenCreate
            cfg.switchGenMode
            [||]
            cfg.switchCount
            cfg.sorterCountOriginal


    let getSorterSetOriginalId (cfg: sorterSetMutatedFromRndCfg) 
        = 
        cfg |> getSorterSetOriginalCfg
            |> SorterSetRndCfg.getSorterSetId


    let getMutatedSorterSetId 
            (cfg: sorterSetMutatedFromRndCfg) 
        = 
        [|
          (cfg.GetType()) :> obj;
           cfg :> obj;
        |] |> GuidUtils.guidFromObjs
           |> SorterSetId.create

    let getParentMapCfg
            (cfg: sorterSetMutatedFromRndCfg) 
        = 
        SorterParentMap.create
            (cfg |> getMutatedSorterSetId)
            (cfg |> getSorterSetOriginalId)
            (cfg |> getSorterCountMutated)
            (cfg |> getSorterCountOriginal)



    let getConfigName 
            (rdsg:sorterSetMutatedFromRndCfg) 
        =
        sprintf "%d_%s_%f"
            (rdsg |> getOrder |> Order.value)
            (rdsg |> getSwitchGenMode |> string)
            (rdsg |> getMutationRate |> MutationRate.value )


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
            (Some cfg.sorterCountMutated)
            cfg.rngGenMutate


    let getParentMapId (cfg:sorterSetMutatedFromRndCfg) 
        =                     
        SorterParentMap.makeId
                        (cfg |> getSorterSetOriginalId)
                        (cfg |> getMutatedSorterSetId)


    let getParentMapFileName
            (cfg: sorterSetMutatedFromRndCfg) 
        =
        cfg |> getParentMapId |> SorterParentMapId.value |> string


    let makeMutantSorterSet
            (lookup: sorterSetRndCfg -> Result<sorterSet, string>)
            (mutCfg: sorterSetMutatedFromRndCfg)
        =
        let parentCfg = 
            mutCfg 
            |> getSorterSetOriginalCfg

        result {
            let! parentSorterSet = lookup parentCfg
            let! mutantSet = 
                    parentSorterSet |>
                        SorterSetMutator.createMutantSorterSetFromParentMap
                            (mutCfg |> getParentMapCfg)
                            (mutCfg |> getSorterSetMutator)
                            
            return mutantSet
        }  


    //let makeMutantSorterSetAndParentMap 
    //        (lookup: sorterSetRndCfg -> Result<sorterSet, string>)
    //        (mutCfg: sorterSetMutatedFromRndCfg)
    //    =
    //    let parentCfg = 
    //        mutCfg 
    //        |> getSorterSetOriginalCfg

    //    result {
    //        let! parentSorterSet = lookup parentCfg
    //        let! parentMap, mutantSet = 
    //                parentSorterSet |>
    //                    SorterSetMutator.createMutantSorterSetAndParentMap
    //                        (mutCfg |> getSorterSetMutator)

    //        return parentMap, mutantSet
    //    }     
