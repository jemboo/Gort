namespace global

open System

type sorterParentMap = 
        private {
        id: sorterParentMapId;
        childSetId:sorterSetId;
        parentSetId:sorterSetId;
        parentMap:Map<sorterId, sorterParentId> }

module SorterParentMap =

    let load
            (id:sorterParentMapId)
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
        [|parentSetId :> obj; childSetId :> obj|] 
        |> GuidUtils.guidFromObjs
        |> SorterParentMapId.create


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
            |> Seq.allPairs (childSetId |> SorterSet.generateSorterIds)
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



    let getId
            (sorterParentMap:sorterParentMap) 
         =
         sorterParentMap.id

    let getParentMap 
             (sorterParentMap:sorterParentMap) 
         =
         sorterParentMap.parentMap


    let getChildSorterSetId
                (sorterParentMap:sorterParentMap) 
         =
         sorterParentMap.childSetId


    let getParentSorterSetId
                (sorterParentMap:sorterParentMap) 
         =
         sorterParentMap.parentSetId

