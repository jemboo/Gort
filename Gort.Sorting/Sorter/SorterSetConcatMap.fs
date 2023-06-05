namespace global


type sorterSetConcatMap = 
        private {
        id: sorterSetConcatMapId;
        sorterSetBaseId:sorterSetId;
        sorterSetConcatId:sorterSetId;
        concatMap:Map<sorterId, sorterId[]> }


module SorterSetConcatMap 
        =
    let load (id:sorterSetConcatMapId)
             (sorterSetBaseId:sorterSetId)
             (sorterSetConcatId:sorterSetId)
             (concatMap:Map<sorterId, sorterId[]>)
        =
        { 
          id = id
          sorterSetBaseId = sorterSetBaseId
          sorterSetConcatId = sorterSetConcatId
          concatMap = concatMap
        }


    let make (concats:sorterId[] seq)
             (sorterSetBaseId:sorterSetId)
             (sorterSetConcatId:sorterSetId)
        =
        let concatMapId = 
            [| 
                (sorterSetBaseId |> SorterSetId.value);
                (sorterSetConcatId |> SorterSetId.value);
            |] 
            |> Array.map(fun tup -> tup:> obj) 
            |> GuidUtils.guidFromObjs 
            |> SorterSetConcatMapId.create

        let memberIds = sorterSetConcatId |> SorterSet.generateSorterIds
        let concatMap = 
                concats
                |> Seq.zip memberIds
                |> Seq.toArray
                |> Map.ofSeq
        { 
          id = concatMapId
          sorterSetBaseId = sorterSetBaseId
          sorterSetConcatId = sorterSetConcatId
          concatMap = concatMap
        }


    let getId (sscm: sorterSetConcatMap) = sscm.id
    let getSorterSetBaseId (sscm: sorterSetConcatMap) = sscm.sorterSetBaseId
    let getSorterSetConcatId (sscm: sorterSetConcatMap) = sscm.sorterSetConcatId
    let getConcatMap (sscm: sorterSetConcatMap) = sscm.concatMap

    let createSorterSet
            (sorterSetbase: sorterSet)
            (sorterSetConcatMap:sorterSetConcatMap)
        =
        let _mergeEm mergeId sorterIds =
            result {
                let order = sorterSetbase |> SorterSet.getOrder
                let! sorters = 
                    sorterIds 
                        |> Array.map(fun id -> sorterSetbase |> SorterSet.getSorterByIdR id)
                        |> Array.toList
                        |> Result.sequence

                return Sorter.concatSwitches mergeId order sorters
            }

        result {
            let! mergedSorters =
                    sorterSetConcatMap
                    |> getConcatMap
                    |> Map.toSeq
                    |> Seq.map(fun (mergeId, cats) -> _mergeEm mergeId cats)
                    |> Seq.toList
                    |> Result.sequence

            return SorterSet.load
                        sorterSetConcatMap.sorterSetConcatId
                        (sorterSetbase |> SorterSet.getOrder)
                        mergedSorters
        }


    let createForAppendSet
            (sorterSetbaseId: sorterSetId)
            (sorterSetBaseCount:sorterCount)
            (sorterSetIdAppendSet:sorterSetId)
        =
        let baseSorterIds = 
            sorterSetbaseId 
                |> SorterSet.generateSorterIds
                |> Seq.take(SorterCount.value sorterSetBaseCount)
                |> Seq.toArray

        let idCCts = 
            CollectionOps.cartesianProduct baseSorterIds baseSorterIds
            |> Seq.map(fun (fst, snd) -> [|fst;snd|])

        make 
            idCCts 
            sorterSetbaseId
            sorterSetIdAppendSet
        

    let createSorterSetAppend
            (sorterSetbase: sorterSet)
            (sorterSetIdAppendSet:sorterSetId)
        =
        let sscm = 
            createForAppendSet 
                (sorterSetbase |> SorterSet.getId)
                (sorterSetbase |> SorterSet.getSorterCount)
                sorterSetIdAppendSet
        createSorterSet sorterSetbase sscm
