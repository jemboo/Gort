namespace global
open System
open Microsoft.FSharp.Core

type sorterSetParentMapDto  = {
        id:Guid;
        sorterSetIdMutant: Guid; 
        sorterSetIdParent: Guid; 
        parentMap:Map<Guid, Guid>;
    }

module SorterSetParentMapDto =

    let fromDto (dto:sorterSetParentMapDto) =
        result {
            let id = dto.id |> SorterSetParentMapId.create            
            let sorterSetIdMutant = dto.sorterSetIdMutant |> SorterSetId.create
            let  sorterSetIdParent = dto.sorterSetIdParent |> SorterSetId.create
            let parentMap = 
                    dto.parentMap
                    |> Map.toSeq
                    |> Seq.map(fun (p,m) -> 
                         (p |> SorterId.create, m |> SorterParentId.create))
                    |> Map.ofSeq

            return SorterSetParentMap.load
                        id
                        sorterSetIdMutant
                        sorterSetIdParent
                        parentMap
        }

    let fromJson (jstr: string) =
        result {
            let! dto = Json.deserialize<sorterSetParentMapDto> jstr
            return! fromDto dto
        }

    let toDto (sorterParentMap: sorterSetParentMap) =
        {
            id = sorterParentMap
                 |> SorterSetParentMap.getId
                 |> SorterSetParentMapId.value

            parentMap =
                sorterParentMap 
                |> SorterSetParentMap.getParentMap
                |> Map.toSeq
                |> Seq.map(fun (p,m) -> 
                        (p |> SorterId.value, m |> SorterParentId.value))
                |> Map.ofSeq

            sorterSetIdMutant =
                sorterParentMap 
                |> SorterSetParentMap.getChildSorterSetId
                |> SorterSetId.value

            sorterSetIdParent =
                sorterParentMap 
                |> SorterSetParentMap.getParentSorterSetId
                |> SorterSetId.value

        }

    let toJson (sorterParentMap: sorterSetParentMap) =
        sorterParentMap |> toDto |> Json.serialize
