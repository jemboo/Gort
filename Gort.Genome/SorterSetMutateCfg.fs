namespace global

open System

type sorterSetMutateCfg = 
    private
        { 
          sorterSetMutator: sorterSetMutator
          sorterSetCfgParent: sorterSetCfg
        }


module SorterSetMutateCfg =
    let create (sorterSetMutator:sorterSetMutator)
               (sorterSetParent:sorterSetCfg)
        =
        {
            sorterSetMutator=sorterSetMutator;
            sorterSetCfgParent=sorterSetParent;
        }


    let getSorterSetMutator (rdsg: sorterSetMutateCfg) = 
            rdsg.sorterSetMutator


    let getSorterSetCfgParent (rdsg: sorterSetMutateCfg) = 
            rdsg.sorterSetCfgParent


    let getParentMapFileName
            (pm:sorterParentMap) 
        =
        pm |> SorterParentMap.getId |> SorterParentMapId.value |> string


    let createMutantSorterSetAndParentMap 
            (lookup: sorterSetCfg -> Result<sorterSet, string>)
            (mutCfg: sorterSetMutateCfg)
        =
        let parentCfg = 
            mutCfg 
            |> getSorterSetCfgParent

        result {
            let! parentSorterSet = lookup parentCfg
            let! parentMap, mutantSet = 
                    parentSorterSet |>
                        SorterSetMutator.createMutantSorterSetAndParentMap
                            (mutCfg |> getSorterSetMutator)

            return parentMap, mutantSet
        }
