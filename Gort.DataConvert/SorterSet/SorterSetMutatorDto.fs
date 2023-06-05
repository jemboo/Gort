namespace global
open Microsoft.FSharp.Core


type sorterSetMutatorDto = { 
        sorterCountFinal: int; 
        sorterMutatorDto:sorterMutatorDto;
        rngGenDto:rngGenDto; }


module SorterSetMutatorDto =

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
