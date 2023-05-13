namespace global

open System

type mutateSorterSetCfg = 
    private
        { 
          sorterSetMutator: sorterSetMutator
          sorterSetParent: sorterSetCfg
        }


module MutateSorterSetCfg =
    let create (sorterSetMutator:sorterSetMutator)
               (sorterSetParent:sorterSetCfg)
        =
        {
            sorterSetMutator=sorterSetMutator;
            sorterSetParent=sorterSetParent;
        }


    let getSorterSetMutator (rdsg: mutateSorterSetCfg) = 
            rdsg.sorterSetMutator


    let getSorterSetParent (rdsg: mutateSorterSetCfg) = 
            rdsg.sorterSetParent


    let getParentMapFileName
            (pm:sorterParentMap) 
        =
        pm |> SorterParentMap.getId |> string


    let createMutantSorterSetAndParentMap 
            (lookup: sorterSetCfg -> Result<sorterSet, string>)
            (cfg: mutateSorterSetCfg)
        =
        let parentCfg = 
            cfg 
            |> getSorterSetParent

        result {
            let! parentSorterSet = lookup parentCfg
            let! parentMap, mutantSet = 
                    parentSorterSet |>
                        SorterSetMutator.createMutantSorterSetAndParentMap
                            (cfg |> getSorterSetMutator)

            return parentMap, mutantSet
        }
            