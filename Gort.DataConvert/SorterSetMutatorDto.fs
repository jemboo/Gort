namespace global
open System
open Microsoft.FSharp.Core

type sorterParentMapDto  = {
        id:Guid;
        parentMap:Map<Guid, Guid>;
        sorterSetIdMutant: Guid; 
        sorterSetIdParent: Guid; 
    }

module SorterParentMapDto =

    let fromDto (dto:sorterParentMapDto) =
        result {
            let id = dto.id |> SorterParentMapId.create            
            let sorterSetIdMutant = dto.sorterSetIdMutant |> SorterSetId.create
            let  sorterSetIdParent = dto.sorterSetIdParent |> SorterSetId.create
            let parentMap = 
                    dto.parentMap
                    |> Map.toSeq
                    |> Seq.map(fun (p,m) -> 
                         (p |> SorterId.create, m |> SorterParentId.create))
                    |> Map.ofSeq

            return SorterParentMap.load
                        id
                        sorterSetIdMutant
                        sorterSetIdParent
                        parentMap
        }

    let fromJson (jstr: string) =
        result {
            let! dto = Json.deserialize<sorterParentMapDto> jstr
            return! fromDto dto
        }

    let toDto (sorterParentMap: sorterParentMap) =
        {
            id = sorterParentMap
                 |> SorterParentMap.getId
                 |> SorterParentMapId.value

            parentMap =
                sorterParentMap 
                |> SorterParentMap.getParentMap
                |> Map.toSeq
                |> Seq.map(fun (p,m) -> 
                        (p |> SorterId.value, m |> SorterParentId.value))
                |> Map.ofSeq

            sorterSetIdMutant =
                sorterParentMap 
                |> SorterParentMap.getChildSorterSetId
                |> SorterSetId.value

            sorterSetIdParent =
                sorterParentMap 
                |> SorterParentMap.getParentSorterSetId
                |> SorterSetId.value

        }

    let toJson (sorterParentMap: sorterParentMap) =
        sorterParentMap |> toDto |> Json.serialize




type sorterSetMutatorDto = { 
        sorterCountFinal: int; 
        sorterMutatorDto:sorterMutatorDto;
        rngGenDto:rngGenDto; }


module MutantSorterSetDto =

    let fromDto (dto:sorterSetMutatorDto) =
        result {
            let! sorterMutator = dto.sorterMutatorDto |> SorterMutatorDto.fromDto
            let sorterCountFinal =
                match dto.sorterCountFinal with
                | -1 -> None
                | v -> v |> SorterCount.create |> Some
            let! rngGen = dto.rngGenDto |> RngGenDto.fromDto

            return SorterSetMutator.load
                        sorterMutator
                        sorterCountFinal
                        rngGen
        }

    let fromJson (jstr: string) =
        result {
            let! dto = Json.deserialize<sorterSetMutatorDto> jstr
            return! fromDto dto
        }

    let toDto (sorterSetMutator: sorterSetMutator) =
        {
            sorterCountFinal =
                match (sorterSetMutator |> SorterSetMutator.getSorterCountFinal) with
                | None -> -1
                | Some v -> v |> SorterCount.value

            sorterMutatorDto =
                sorterSetMutator 
                |> SorterSetMutator.getSorterMutator
                |> SorterMutatorDto.toDto

            rngGenDto =
                sorterSetMutator 
                |> SorterSetMutator.getRngGen
                |> RngGenDto.toDto
        }

    let toJson (sorterSetMutator: sorterSetMutator) =
        sorterSetMutator |> toDto |> Json.serialize



//type mutantSorterSetDto = { 
//        sorterSetIdMutant: Guid; 
//        sorterSetIdParent: Guid; 
//        sorterMutatorDto:sorterMutatorDto;
//        sorterParentMapDto:sorterParentMapDto; }


//module MutantSorterSetDto =

//    let fromDto (dto:mutantSorterSetDto) =
//        result {
//            let sorterSetIdMutant = dto.sorterSetIdMutant |> SorterSetId.create
//            let  sorterSetIdParent = dto.sorterSetIdParent |> SorterSetId.create
//            let! sorterMutator = dto.sorterMutatorDto |> SorterMutatorDto.fromDto
//            let! sorterParentMap = dto.sorterParentMapDto |> SorterParentMapDto.fromDto


//            return MutantSorterSetMap.load
//                        sorterMutator
//                        sorterSetIdMutant
//                        sorterSetIdParent
//                        sorterParentMap
//        }

//    let fromJson (jstr: string) =
//        result {
//            let! dto = Json.deserialize<mutantSorterSetDto> jstr
//            return! fromDto dto
//        }

//    let toDto (mutantSorterSet: mutantSorterSetMap) =
//        {
//            sorterSetIdMutant =
//                mutantSorterSet 
//                |> MutantSorterSetMap.getMutantSorterSetId
//                |> SorterSetId.value

//            sorterSetIdParent =
//                mutantSorterSet 
//                |> MutantSorterSetMap.getParentSorterSetId
//                |> SorterSetId.value

//            sorterMutatorDto =
//                mutantSorterSet 
//                |> MutantSorterSetMap.getSorterMutator
//                |> SorterMutatorDto.toDto

//            sorterParentMapDto =
//                mutantSorterSet 
//                |> MutantSorterSetMap.getSorterParentMap
//                |> SorterParentMapDto.toDto
//        }

//    let toJson (mutantSorterSetMap: mutantSorterSetMap) =
//        mutantSorterSetMap |> toDto |> Json.serialize
