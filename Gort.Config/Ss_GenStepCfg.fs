namespace global

open System


type ss_GenStepCfg = 
    private
        { 
          order: order
          switchGenMode: switchGenMode
          rngGenMutate: rngGen
          sorterCountMutate: sorterCount
          mutationRate:mutationRate
          stagePrefixCount: stageCount
          stageWeight:stageWeight
          temp:temp
        }


module Ss_GenStepCfg 
    =
    let create (order:order)
               (switchGenMode:switchGenMode)
               (rngGenMutate:rngGen)
               (sorterCountMutate:sorterCount)
               (mutationRate:mutationRate)
               (stagePrefixCount: stageCount)
               (stageWeight: stageWeight)
               (temp: temp)
        =
        {
            order=order;
            switchGenMode=switchGenMode;
            rngGenMutate=rngGenMutate;
            sorterCountMutate=sorterCountMutate;
            mutationRate=mutationRate
            stagePrefixCount=stagePrefixCount;
            stageWeight=stageWeight
            temp=temp
        }

    let getMutationRate (cfg: ss_GenStepCfg) = 
            cfg.mutationRate

    let getOrder (cfg: ss_GenStepCfg) = 
            cfg.order

    let getStagePrefixCount  (cfg: ss_GenStepCfg) = 
            cfg.stagePrefixCount

    let getSwitchGenMode (cfg: ss_GenStepCfg) = 
            cfg.switchGenMode

    let getSortableSetCfg
            (cfg:ss_GenStepCfg)
        =
        cfg.order |> SortableSetCertainCfg.makeAllBitsReducedOneStage


    let getSortableSetId
            (genCount:generation)
            (cfg: ss_GenStepCfg)
            (ssIdInit:sortableSetId)
        = 
        let mutable curGu = ssIdInit |> SortableSetId.value
        let mutable dex = 1
        while dex < (genCount |> Generation.value) do
          curGu <-         
                [|
                   "ss_ShcCfg" :> obj;
                   curGu :> obj;
                   cfg :> obj;
                |] |> GuidUtils.guidFromObjs
          dex <- dex + 1

        curGu |> SortableSetId.create


    let getFileName
            (genCount:generation)
            (cfg: ss_GenStepCfg)
            (ssIdInit:sortableSetId)
        =
        ssIdInit |> getSortableSetId genCount cfg
            |> SortableSetId.value 
            |> string


    let getSorterSetMutatorCfg 
            (cfg:ss_GenStepCfg)
        =
        SorterSetMutatorCfg.create 
            cfg.order
            cfg.switchGenMode
            cfg.rngGenMutate
            cfg.sorterCountMutate
            cfg.mutationRate




    //let makeNextGen
    //        (up:useParallel)
    //        (cfg: ss_GenStepCfg)
    //        (sorterSetGm1: sorterSet)
    //        (sortableSet: sortableSet)
    //    =
    //    result {
    //        let! ssEval = 
    //               SorterSetEval.make
    //                    (getlId cfg)
    //                    sorterEvalMode.DontCheckSuccess
    //                    sorterSetGm1
    //                    sortableSet
    //                    up

    //        let ordr = cfg |> getOrder
    //        let tCmod = cfg |> getStagePrefixCount
    //        return
    //            SorterSetEval.create
    //                (ssEval |> SorterSetEval.getSorterSetEvalId)
    //                (ssEval |> SorterSetEval.getSorterSetlId)
    //                (ssEval |> SorterSetEval.getSortableSetId)
    //                (ssEval |> SorterSetEval.getSorterEvals  
    //                    |> Array.map(SorterEval.modifyForPrefix ordr tCmod))
    //    }
             