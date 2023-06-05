namespace global
open System
open Microsoft.FSharp.Core

type sorterSetConcatMapDto  = {
        id:Guid;
        sorterSetBaseId: Guid; 
        sorterSetConcatId: Guid; 
        parentMap:Map<Guid, Guid[]>;
    }

module SorterSetConcatMapDto =

    let fromDto (dto:sorterSetConcatMapDto) =
        result {
            let id = dto.id |> SorterSetConcatMapId.create            
            let sorterSetBaseId = dto.sorterSetBaseId |> SorterSetId.create
            let sorterSetConcatId = dto.sorterSetConcatId |> SorterSetId.create
            let parentMap = 
                    dto.parentMap
                    |> Map.toSeq
                    |> Seq.map(fun (p,m) -> 
                         (p |> SorterId.create, 
                          m |> Array.map(SorterId.create)))
                    |> Map.ofSeq

            return SorterSetConcatMap.load
                        id
                        sorterSetBaseId
                        sorterSetConcatId
                        parentMap
        }

    let fromJson (jstr: string) =
        result {
            let! dto = Json.deserialize<sorterSetConcatMapDto> jstr
            return! fromDto dto
        }

    let toDto (sorterSetConcatMap: sorterSetConcatMap) =
        {
            id = sorterSetConcatMap
                 |> SorterSetConcatMap.getId
                 |> SorterSetConcatMapId.value

            parentMap =
                sorterSetConcatMap 
                |> SorterSetConcatMap.getConcatMap
                |> Map.toSeq
                |> Seq.map(fun (p,m) -> 
                        (p |> SorterId.value, 
                         m |> Array.map( SorterId.value)))
                |> Map.ofSeq

            sorterSetConcatId =
                sorterSetConcatMap 
                |> SorterSetConcatMap.getSorterSetConcatId
                |> SorterSetId.value

            sorterSetBaseId =
                sorterSetConcatMap 
                |> SorterSetConcatMap.getSorterSetBaseId
                |> SorterSetId.value

        }

    let toJson (sorterSetConcatMap: sorterSetConcatMap) =
        sorterSetConcatMap |> toDto |> Json.serialize

