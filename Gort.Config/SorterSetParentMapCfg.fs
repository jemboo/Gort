namespace global


type sorterSetParentMapCfg = 
    private
        { 
          parentSorterSetId: sorterSetId
          parentSorterSetCount: sorterCount
          childSorterSetId: sorterSetId
          childSorterSetCount: sorterCount
        }


module SorterSetParentMapCfg =
    let create 
            (parentSorterSetId:sorterSetId)
            (parentSorterSetCount:sorterCount)
            (childSorterSetId:sorterSetId)
            (childSorterSetCount:sorterCount)
        =
        {
            parentSorterSetId=parentSorterSetId;
            parentSorterSetCount=parentSorterSetCount;
            childSorterSetId=childSorterSetId;
            childSorterSetCount=childSorterSetCount;
        }

    let getParentSorterSetId (cfg: sorterSetParentMapCfg) = 
            cfg.parentSorterSetId


    let getChildSorterSetId (cfg: sorterSetParentMapCfg) = 
            cfg.childSorterSetId


    let getParentSorterSetCount (cfg: sorterSetParentMapCfg) = 
            cfg.parentSorterSetCount


    let getChildSorterSetCount (cfg: sorterSetParentMapCfg) = 
            cfg.childSorterSetCount


    let getId (cfg:sorterSetParentMapCfg) 
        = 
        [|
          (cfg.GetType()) :> obj;
           cfg :> obj;
        |] |> GuidUtils.guidFromObjs
           |> SorterSetParentMapId.create

    let getFileName
            (cfg: sorterSetParentMapCfg) 
        =
        cfg |> getId |> SorterSetParentMapId.value |> string


    let makeParentMap 
            (cfg: sorterSetParentMapCfg) = 

        SorterSetParentMap.create
            (cfg |> getChildSorterSetId)
            (cfg |> getParentSorterSetId)
            (cfg |> getChildSorterSetCount)
            (cfg |> getParentSorterSetCount)    
  