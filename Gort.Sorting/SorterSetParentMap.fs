namespace global

open System


type generation = private Generation of int

module Generation =
    let value (Generation v) = v
    let create dex = Generation dex
    let next (Generation v) = Generation (v + 1)


type sorterSetParentMap = 
        private {
        id: sorterSetParentMapId;
        childSetId:sorterSetId;
        parentSetId:sorterSetId;
        parentMap:Map<sorterId, sorterParentId> }

module SorterSetParentMap =

    let getId
            (sorterParentMap:sorterSetParentMap) 
         =
         sorterParentMap.id


    let getParentMap 
             (sorterParentMap:sorterSetParentMap) 
         =
         sorterParentMap.parentMap


    let getChildSorterSetId
                (sorterParentMap:sorterSetParentMap) 
         =
         sorterParentMap.childSetId


    let getParentSorterSetId
                (sorterParentMap:sorterSetParentMap) 
         =
         sorterParentMap.parentSetId

    let load
            (id:sorterSetParentMapId)
            (childSetId:sorterSetId)
            (parentSetId:sorterSetId)
            (parentMap:Map<sorterId, sorterParentId>)
        =
        {   
            id=id
            parentMap=parentMap
            childSetId=childSetId
            parentSetId=parentSetId
        }

    let makeId
            (parentSetId:sorterSetId)
            (childSetId:sorterSetId)
        =
        [|
            "sorterSetParentMap" :> obj
            parentSetId :> obj; childSetId :> obj
        |] 
        |> GuidUtils.guidFromObjs
        |> SorterSetParentMapId.create


    let create
            (childSetId:sorterSetId)
            (parentSetId:sorterSetId)
            (childSetCount:sorterCount)
            (parentSetCount:sorterCount)
        =
        let parentSorterIds = 
            parentSetId |> SorterSet.generateSorterIds
            |> Seq.map(SorterParentId.toSorterParentId)
            |> Seq.take (parentSetCount |> SorterCount.value)
            |> Seq.toArray

        let parentMap =
            parentSorterIds
            |> CollectionOps.infinteLoop
            |> Seq.zip (childSetId |> SorterSet.generateSorterIds)
            |> Seq.take (childSetCount |> SorterCount.value)
            |> Map.ofSeq

        let sorterParentMapId = 
                makeId
                   parentSetId
                   childSetId

        load
            sorterParentMapId
            childSetId
            parentSetId
            parentMap


    let makeMergeMap
            (ssParent:sorterSet) 
            (sspm:sorterSetParentMap)
        =
        ssParent 
        |> SorterSet.getSorters
        |> Seq.map(
            fun sorter -> 
                (
                    sorter |> Sorter.getSorterId,
                    sorter |> Sorter.getSorterId |> SorterParentId.toSorterParentId
                )
            )
        |> Seq.append
            (sspm |> getParentMap |> Map.toSeq)
        |> Map.ofSeq
        

    // adds self-mapping of the parent sorterId's
    let extendToParents
            (sspm:sorterSetParentMap)
        =
            sspm 
            |> getParentMap
            |> Map.values
            |> Seq.distinct
            |> Seq.map(fun v -> (v |> SorterParentId.toSorterId, v))
            |> Seq.append
                (sspm |> getParentMap |> Map.toSeq)
            |> Map.ofSeq



type sorterAncestry =
        private {
        sorterId: sorterId
        ancestors: List<(sorterId*generation)>
        }


module SorterAncestry =

    let getSorterId (sa:sorterAncestry) =
        sa.sorterId

    let getAncestors (sa:sorterAncestry) =
        sa.ancestors

    let create (sorterId:sorterId) =
        {
            sorterAncestry.sorterId = sorterId;
            ancestors = [(sorterId, Generation.create 0)]
        }

    let update 
            (sa:sorterAncestry) 
            (sorterId:sorterId) 
            (gen:generation) =
        {
            sorterAncestry.sorterId = sorterId;
            ancestors = (sorterId, gen) :: sa.ancestors
        }


type sorterSetAncestryId = private SorterSetAncestryId of Guid
module SorterSetAncestryId =
    let value (SorterSetAncestryId v) = v
    let create (v: Guid) = SorterSetAncestryId v


type sorterSetAncestry =
        private {
        id: sorterSetAncestryId;
        generation:generation;
        ancestorMap:Map<sorterId, sorterAncestry>
        }


module SorterSetAncestry =

    let getId (sa:sorterSetAncestry) =
        sa.id

    let getAncestorMap (sa:sorterSetAncestry) =
        sa.ancestorMap

    let create (id:sorterSetAncestryId) 
               (sorterIds:sorterId seq) 
        =
        {
            id = id;
            generation = 0 |> Generation.create; 
            ancestorMap = 
                sorterIds
                |> Seq.map(fun sid -> (sid, SorterAncestry.create sid))
                |> Map.ofSeq
        }

    let update 
            (newId:sorterSetAncestryId)
            (parentMap:Map<sorterId, sorterParentId>)
            (sorterSetAncestry:sorterSetAncestry) 
        =
        let nextGen = (sorterSetAncestry.generation |> Generation.next)

        let _update (sorterId, sorterParentId) =
            if (sorterId |> SorterId.value) = (sorterParentId |> SorterParentId.value) 
               then
                (sorterId, sorterSetAncestry.ancestorMap.[sorterId])
               else
                let parentAncestry = 
                    sorterSetAncestry.ancestorMap.[sorterParentId |> SorterParentId.toSorterId]
                (
                    sorterId,
                    SorterAncestry.update 
                        parentAncestry 
                        sorterId
                        nextGen
                  )

        let newAncestorMap = 
            parentMap
            |> Map.toSeq 
            |> Seq.map(_update)
            |> Map.ofSeq

        {
            id = newId;
            generation = nextGen; 
            ancestorMap = newAncestorMap
        }

