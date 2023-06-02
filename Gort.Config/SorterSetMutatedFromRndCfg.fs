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
          sorterCount: sorterCount
          mutationRate:mutationRate
        }


module SorterSetMutatedFromRndCfg =
    let create (order:order)
               (rngGenCreate:rngGen)
               (switchGenMode:switchGenMode)
               (switchCount:switchCount)
               (sorterCountOriginal:sorterCount)
               (rngGenMutate:rngGen)
               (sorterCount:sorterCount)
               (mutationRate:mutationRate)
        =
        {
            order=order;
            rngGenCreate=rngGenCreate;
            switchGenMode=switchGenMode;
            switchCount=switchCount;
            sorterCountOriginal=sorterCountOriginal;
            rngGenMutate=rngGenMutate;
            sorterCount=sorterCount;
            mutationRate=mutationRate
        }


    let getProperties (cfg: sorterSetMutatedFromRndCfg) = 
        [|
            ("order", cfg.order :> obj);
            ("rngGenCreate", cfg.rngGenCreate :> obj);
            ("switchGenMode", cfg.switchGenMode :> obj);
            ("switchCount", cfg.switchCount :> obj);
            ("sorterCountOriginal", cfg.sorterCountOriginal :> obj);
            ("rngGenMutate", cfg.rngGenMutate :> obj);
            ("sorterCount", cfg.sorterCount :> obj);
            ("mutationRate", cfg.mutationRate :> obj);
        |]


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

    let getSorterCount (cfg: sorterSetMutatedFromRndCfg) =
            cfg.sorterCount

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
            cfg.sorterCount
            cfg.mutationRate


    let getId (cfg: sorterSetMutatedFromRndCfg) 
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
            (cfg |> getSorterCount)


    let getSorterSetMutator (cfg:sorterSetMutatedFromRndCfg) 
        =
        cfg |> getSorterSetMutatorCfg 
            |> SorterSetMutatorCfg.getSorterSetMutator