namespace global
open System

type sorterSet = private { 
        id:sorterSetId; order:order; sorterMap:Map<sorterId, sorter>}

module SorterSet =

    let sorterCount (sorterSet:sorterSet) = 
            sorterSet.sorterMap.Count


    let fromSorters (id:sorterSetId)
                    (order:order) 
                    (sorters:seq<sorter>) =
        let sorterMap = 
                sorters 
                |> Seq.map(fun s-> (s |> Sorter.makeId, s))
                |> Map.ofSeq
        {
            sorterSet.id = id;
            order=order; 
            sorterMap = sorterMap
        }
