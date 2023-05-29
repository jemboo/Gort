namespace global

type sorterSetMutatedFromRndCfg = 
    private
        { 
          order: order
          rngGenCreate: rngGen
          switchGenMode: switchGenMode
          switchCount: switchCount
          sorterCountOriginal: sorterCount
          rngGenMutate: rngGen
          sorterCountMutated: sorterCount
          mutationRate:mutationRate
        }


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

    let getSorterSetParentCfg (cfg:sorterSetMutatedFromRndCfg)
        =
        SorterSetRndCfg.create 
            cfg.order
            cfg.rngGenCreate
            cfg.switchGenMode
            cfg.switchCount
            cfg.sorterCountOriginal


    let getSorterSetMutatorCfg (cfg:sorterSetMutatedFromRndCfg)
        =
        SorterSetMutatorCfg.create 
            cfg.order
            cfg.switchGenMode
            cfg.rngGenMutate
            cfg.sorterCountMutated
            cfg.mutationRate


    let getId
            (cfg: sorterSetMutatedFromRndCfg) 
        = 
        [|
          (cfg |> getSorterSetParentCfg |> SorterSetRndCfg.getId |> SorterSetId.value) :> obj;
          (cfg |> getSorterSetMutatorCfg) :> obj;
        |] |> GuidUtils.guidFromObjs
           |> SorterSetId.create


    let getFileName
            (cfg: sorterSetMutatedFromRndCfg) 
        =
        cfg |> getId |> SorterSetId.value |> string


    let getConfigName 
            (rdsg:sorterSetMutatedFromRndCfg) 
        =
        sprintf "%d_%s_%f"
            (rdsg |> getOrder |> Order.value)
            (rdsg |> getSwitchGenMode |> string)
            (rdsg |> getMutationRate |> MutationRate.value )


    let getSorterSetOriginalId (cfg: sorterSetMutatedFromRndCfg) 
        = 
        cfg |> getSorterSetParentCfg
            |> SorterSetRndCfg.getId


    let getSorterSetParentMapCfg (cfg:sorterSetMutatedFromRndCfg)
        =
        SorterSetParentMapCfg.create 
            (cfg |> getSorterSetOriginalId)
            (cfg |> getSorterCountOriginal)
            (cfg |> getId)
            (cfg |> getSorterCountMutated)


    let getSorterSetMutator (cfg:sorterSetMutatedFromRndCfg) 
        =
        cfg |> getSorterSetMutatorCfg 
            |> SorterSetMutatorCfg.getSorterSetMutator


    let makeMutantSorterSet
            (lookup: string -> Result<sorterSet, string>)
            (save: string -> sorterSet -> Result<bool, string>)
            (mutCfg: sorterSetMutatedFromRndCfg)
        =
        let parentFileName = 
            mutCfg 
            |> getSorterSetParentCfg
            |> SorterSetRndCfg.getFileName

        result {
            let! parentSorterSet = lookup parentFileName
            let! mutantSet = 
                    parentSorterSet |>
                        SorterSetMutator.createMutantSorterSetFromParentMap
                            (mutCfg 
                                |> getSorterSetParentMapCfg
                                |> SorterSetParentMapCfg.makeParentMap)
                            (mutCfg |> getSorterSetMutator)

            let mutantSetFileName = mutCfg |> getFileName
            let! res = save mutantSetFileName mutantSet
                            
            return mutantSet
        }


    let getMutantSorterSet
            (ssGet: string -> Result<sorterSet, string>)
            (save: string -> sorterSet -> Result<bool, string>)
            (mutCfg: sorterSetMutatedFromRndCfg)
        =
        result {
            let loadRes  = 
                result {
                    let! mut = ssGet (mutCfg |> getFileName)
                    return mut
                }

            match loadRes with
            | Ok mut -> return mut
            | Error _ -> return! (makeMutantSorterSet ssGet save mutCfg)
        }