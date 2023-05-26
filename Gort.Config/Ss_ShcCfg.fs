namespace global

type shcSubStep =
     | Mutate
     | Eval
     | Merge


type ss_ShcCfg = 
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
          sorterEvalMode: sorterEvalMode
          stagePrefixCount: stageCount
          stageWeight:stageWeight
          temp:temp
          generation:generation
          shcSubStep:shcSubStep
        }


module Ss_ShcCfg 
    =
    let create (order:order)
               (rngGenCreate:rngGen)
               (switchGenMode:switchGenMode)
               (switchCount:switchCount)
               (sorterCountCreate:sorterCount)
               (rngGenMutate:rngGen)
               (sorterCountMutate:sorterCount)
               (mutationRate:mutationRate)
               (sorterEvalMode: sorterEvalMode)
               (stagePrefixCount: stageCount)
               (stageWeight: stageWeight)
               (temp: temp)
               (generation: generation)
               (shcSubStep: shcSubStep)
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
            sorterEvalMode=sorterEvalMode
            stagePrefixCount=stagePrefixCount;
            stageWeight=stageWeight
            temp=temp
            generation=generation;
            shcSubStep=shcSubStep;
        }

    let getMutationRate (cfg: ss_ShcCfg) = 
            cfg.mutationRate

    let getOrder (cfg: ss_ShcCfg) = 
            cfg.order

    let getRngGenCreate (cfg: ss_ShcCfg) = 
            cfg.rngGenCreate

    let getSorterEvalMode  (cfg: ss_ShcCfg) = 
            cfg.sorterEvalMode

    let getStagePrefixCount  (cfg: ss_ShcCfg) = 
            cfg.stagePrefixCount

    let getSwitchGenMode (cfg: ss_ShcCfg) = 
            cfg.switchGenMode

    let getSwitchCount (cfg: ss_ShcCfg) = 
            cfg.switchCount



    let getSortableSetCfg
            (cfg:ss_ShcCfg)
        =
        cfg.order |> SortableSetCertainCfg.makeAllBitsReducedOneStage

    let getSorterSetMutatedFromRndCfg 
            (cfg:ss_ShcCfg)
        =
        SorterSetMutatedFromRndCfg.create 
            cfg.order
            cfg.rngGenCreate
            cfg.switchGenMode
            cfg.switchCount
            cfg.sorterCountCreate
            cfg.rngGenMutate
            cfg.sorterCountMutate
            cfg.mutationRate


    let getlId
            (cfg: ss_ShcCfg) 
        = 
        [|
          (cfg.GetType()) :> obj;
           cfg :> obj;
        |] |> GuidUtils.guidFromObjs
           |> SorterSetEvalId.create


    let getFileName
            (cfg:ss_ShcCfg) 
        =
        cfg |> getlId 
            |> SorterSetEvalId.value 
            |> string


    let makeSorterSetEval
            (up:useParallel)
            (cfg: ss_ShcCfg)
            (sortableSetCfgRet: sortableSetCfg->Result<sortableSet,string>)
            (sorterSetCfgRet: sorterSetMutatedFromRndCfg->Result<sorterSet,string>)
        =
        result {
            let! sorterSet = sorterSetCfgRet (cfg |> getSorterSetMutatedFromRndCfg)
            let! sortableSet = sortableSetCfgRet 
                                (cfg |> getSortableSetCfg
                                     |> sortableSetCfg.Certain   )
            let! ssEval = 
                   SorterSetEval.make
                        (getlId cfg)
                        (getSorterEvalMode cfg)
                        sorterSet
                        sortableSet
                        up

            let ordr = cfg |> getOrder
            let tCmod = cfg |> getStagePrefixCount
            return
                SorterSetEval.create
                    (ssEval |> SorterSetEval.getSorterSetEvalId)
                    (ssEval |> SorterSetEval.getSorterSetlId)
                    (ssEval |> SorterSetEval.getSortableSetId)
                    (ssEval |> SorterSetEval.getSorterEvals  
                        |> Array.map(SorterEval.modifyForPrefix ordr tCmod))
        }
             