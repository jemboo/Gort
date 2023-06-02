namespace global


type sorterSetAppendMapCfg = 
    private
        { 
          pfxSorterSetId: sorterSetId
          sfxSorterSetId: sorterSetId
          pfxSorterSetCount: sorterCount
          sfxSorterSetCount: sorterCount
          appendedSorterSetId: sorterSetId
        }


module SorterSetAppendMapCfg =
    let create 
            (pfxSorterSetId:sorterSetId)
            (sfxSorterSetId:sorterSetId)
            (pfxSorterSetCount:sorterCount)
            (sfxSorterSetCount:sorterCount)
            (appendedSorterSetId:sorterSetId)
        =
        {
            pfxSorterSetId=pfxSorterSetId;
            sfxSorterSetId=sfxSorterSetId;
            pfxSorterSetCount=pfxSorterSetCount;
            sfxSorterSetCount=sfxSorterSetCount;
            appendedSorterSetId=appendedSorterSetId;
        }

    let getPfxSorterSetId (cfg: sorterSetAppendMapCfg) = 
            cfg.pfxSorterSetId

    let getSfxSorterSetId (cfg: sorterSetAppendMapCfg) = 
            cfg.sfxSorterSetId

    let getPfxSorterSetCount (cfg: sorterSetAppendMapCfg) = 
            cfg.pfxSorterSetCount

    let getSfxSorterSetCount (cfg: sorterSetAppendMapCfg) = 
            cfg.sfxSorterSetCount

    let getAppendedSorterSetId (cfg: sorterSetAppendMapCfg) = 
            cfg.appendedSorterSetId


    //let getId (cfg:sorterSetAppendMapCfg) 
    //    =
    //    SorterSetParentMap.makeId
    //        (cfg |> getPfxSorterSetId)
    //        (cfg |> getSfxSorterSetId)
    //        (cfg |> getParentSorterSetId)
    //        (cfg |> getChildSorterSetId)


    //let getFileName (cfg: sorterSetAppendMapCfg) 
    //    =
    //    cfg |> getId |> SorterSetAppendMapId.value |> string


    //let makeParentMap (cfg: sorterSetAppendMapCfg) 
    //    =
    //    SorterSetParentMap.create
    //        (cfg |> getChildSorterSetId)
    //        (cfg |> getParentSorterSetId)
    //        (cfg |> getChildSorterSetCount)
    //        (cfg |> getParentSorterSetCount)    
  