namespace global

type sorterSetMutatorCfg = 
    private
        { 
          order: order
          switchGenMode: switchGenMode
          rngGenMutate: rngGen
          sorterCountMutated: sorterCount
          mutationRate:mutationRate
        }

module SorterSetMutatorCfg =
    let create (order:order)
               (switchGenMode:switchGenMode)
               (rngGenMutate:rngGen)
               (sorterCountMutated:sorterCount)
               (mutationRate:mutationRate)
        =
        {
            order=order;
            switchGenMode=switchGenMode;
            rngGenMutate=rngGenMutate;
            sorterCountMutated=sorterCountMutated;
            mutationRate=mutationRate
        }

    let getOrder (cfg: sorterSetMutatorCfg) =
            cfg.order

    let getSwitchGenMode 
            (cfg: sorterSetMutatorCfg) 
        =
            cfg.switchGenMode

    let getMutationRate (cfg: sorterSetMutatorCfg) =
            cfg.mutationRate

    let getSorterCountMutated (cfg: sorterSetMutatorCfg) =
            cfg.sorterCountMutated

    let getId 
            (cfg: sorterSetMutatorCfg) 
        = 
        [|
          "sorterSetMutatorCfg" :> obj;
           cfg :> obj;
        |] |> GuidUtils.guidFromObjs
           |> SorterSetId.create


    let getConfigName 
            (rdsg:sorterSetMutatorCfg) 
        =
        sprintf "%d_%s_%f"
            (rdsg |> getOrder |> Order.value)
            (rdsg |> getSwitchGenMode |> string)
            (rdsg |> getMutationRate |> MutationRate.value )


    let getSorterSetMutator 
            (cfg:sorterSetMutatorCfg) 
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