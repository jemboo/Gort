namespace global

type ss_ShcRunCfg = 
    private
        { 
          order: order
          switchGenMode: switchGenMode
          rngGenMutate: rngGen
          sorterCountMutate: sorterCount
          mutationRate:mutationRate
          sorterEvalMode: sorterEvalMode
          stagePrefixCount: stageCount
          stageWeight:stageWeight
          temp:temp
          generationCount:generation
          ssInitId:sorterSetId
        }


module Ss_ShcRunCfg 
    =
    let create (order:order)
               (switchGenMode:switchGenMode)
               (rngGenMutate:rngGen)
               (sorterCountMutate:sorterCount)
               (mutationRate:mutationRate)
               (sorterEvalMode: sorterEvalMode)
               (stagePrefixCount: stageCount)
               (stageWeight: stageWeight)
               (temp: temp)
               (generationCount: generation)
               (ssInitId: sorterSetId)
        =
        {
            order=order;
            switchGenMode=switchGenMode;
            rngGenMutate=rngGenMutate;
            sorterCountMutate=sorterCountMutate;
            mutationRate=mutationRate
            sorterEvalMode=sorterEvalMode
            stagePrefixCount=stagePrefixCount;
            stageWeight=stageWeight
            temp=temp
            generationCount=generationCount;
            ssInitId=ssInitId;
        }

    let getMutationRate (cfg: ss_ShcRunCfg) = 
            cfg.mutationRate

    let getOrder (cfg: ss_ShcRunCfg) = 
            cfg.order

    let getSorterEvalMode  (cfg: ss_ShcRunCfg) = 
            cfg.sorterEvalMode

    let getStagePrefixCount  (cfg: ss_ShcRunCfg) = 
            cfg.stagePrefixCount

    let getSwitchGenMode (cfg: ss_ShcRunCfg) = 
            cfg.switchGenMode

    let getSortableSetCfg
            (cfg:ss_ShcRunCfg)
        =
        cfg.order |> SortableSetCertainCfg.makeAllBitsReducedOneStage


    let getlId
            (cfg: ss_ShcRunCfg) 
        = 
        [|
          "ss_ShcCfg" :> obj;
           cfg :> obj;
        |] |> GuidUtils.guidFromObjs
           |> SorterSetEvalId.create


    let getFileName
            (cfg:ss_ShcRunCfg) 
        =
        cfg |> getlId 
            |> SorterSetEvalId.value 
            |> string


    let getSs_GenStepCfg
            (cfg:ss_ShcRunCfg)
        =
        Ss_GenStepCfg.create 
            cfg.order
            cfg.switchGenMode
            cfg.rngGenMutate
            cfg.sorterCountMutate
            cfg.mutationRate
            cfg.stagePrefixCount
            cfg.stageWeight
            cfg.temp







             