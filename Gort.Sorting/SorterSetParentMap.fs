namespace global

open System

type sorterSetParentMap = 
        private {
        id: sorterSetParentMapId;
        childSetId:sorterSetId;
        parentSetId:sorterSetId;
        parentMap:Map<sorterId, sorterParentId> }

module SorterSetParentMap =

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
        [|parentSetId :> obj; childSetId :> obj|] 
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

