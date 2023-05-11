namespace global
open System
open Microsoft.FSharp.Core

type sorterSetDto = { 
        id: Guid; 
        order:int; 
        sorterIds:Guid[]; 
        offsets:int[]; 
        symbolCounts:int[]; 
        switches:byte[] }

module SorterSetDto =

    let fromDto (dto:sorterSetDto) =
        result {
            let! order = dto.order |> Order.create
            let bps = order |> Switch.bitsPerSymbolRequired
            let switchArrayPacks = 
                    dto.switches 
                            |> CollectionOps.deBookMarkArray dto.offsets
                            |> Seq.map(BitPack.fromBytes bps)
                            |> Seq.toArray
            let sorterA = switchArrayPacks
                            |> Array.mapi(fun i pack ->    
                   Sorter.fromSwitches (dto.sorterIds.[i] |> SorterId.create)
                                       order    
                                       (Switch.fromBitPack pack))
            let sorterSetId = dto.id |> SorterSetId.create
            return SorterSet.load sorterSetId order sorterA
        }


    let fromJson (jstr: string) =
        result {
            let! dto = Json.deserialize<sorterSetDto> jstr
            return! fromDto dto
        }


    let toDto (sorterSt: sorterSet) =
        let sOrder = sorterSt |> SorterSet.getOrder
        let triple = sorterSt 
                     |> SorterSet.getSorters
                     |> Seq.map(fun s -> 
                        (s |> Sorter.getSorterId |> SorterId.value), 
                         s |> Sorter.toByteArray, 
                         s |> Sorter.getSwitches |> Array.length)
                     |> Seq.toArray
        let bookMarks, data = triple
                              |> Array.map(fun (_, sw, _) -> sw)
                              |> CollectionOps.bookMarkArrays
        {
            sorterSetDto.id = sorterSt |> SorterSet.getId |> SorterSetId.value;
            order =  sOrder |> Order.value;
            sorterIds = triple |> Array.map(fun (gu, _, _) -> gu);
            offsets = bookMarks;
            symbolCounts = triple |> Array.map(fun (_, _, sc) -> sc);
            switches = data;
        }


    let toJson (sorterSt: sorterSet) =
        sorterSt |> toDto |> Json.serialize


type sorterParentMapDto  = {
        id:Guid;
        parentMap:Map<Guid, Guid>; 
    }

module SorterParentMapDto =

    let fromDto (dto:sorterParentMapDto) =
        result {
            let id = dto.id |> SorterParentMapId.create
            let parentMap = 
                    dto.parentMap
                    |> Map.toSeq
                    |> Seq.map(fun (p,m) -> 
                         (p |> SorterId.create, m |> SorterParentId.create))
                    |> Map.ofSeq

            return SorterParentMap.load
                        id
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
        }

    let toJson (sorterParentMap: sorterParentMap) =
        sorterParentMap |> toDto |> Json.serialize



type mutantSorterSetDto = { 
        sorterSetIdMutant: Guid; 
        sorterSetIdParent: Guid; 
        sorterMutatorDto:sorterMutatorDto;
        sorterParentMapDto:sorterParentMapDto; }

module MutantSorterSetDto =

    let fromDto (dto:mutantSorterSetDto) =
        result {
            let sorterSetIdMutant = dto.sorterSetIdMutant |> SorterSetId.create
            let  sorterSetIdParent = dto.sorterSetIdParent |> SorterSetId.create
            let! sorterMutator = dto.sorterMutatorDto |> SorterMutatorDto.fromDto
            let! sorterParentMap = dto.sorterParentMapDto |> SorterParentMapDto.fromDto


            return MutantSorterSetMap.load
                        sorterMutator
                        sorterSetIdMutant
                        sorterSetIdParent
                        sorterParentMap
        }

    let fromJson (jstr: string) =
        result {
            let! dto = Json.deserialize<mutantSorterSetDto> jstr
            return! fromDto dto
        }

    let toDto (mutantSorterSet: mutantSorterSetMap) =
        {
            sorterSetIdMutant =
                mutantSorterSet 
                |> MutantSorterSetMap.getMutantSorterSetId
                |> SorterSetId.value

            sorterSetIdParent =
                mutantSorterSet 
                |> MutantSorterSetMap.getParentSorterSetId
                |> SorterSetId.value

            sorterMutatorDto =
                mutantSorterSet 
                |> MutantSorterSetMap.getSorterMutator
                |> SorterMutatorDto.toDto

            sorterParentMapDto =
                mutantSorterSet 
                |> MutantSorterSetMap.getSorterParentMap
                |> SorterParentMapDto.toDto
        }

    let toJson (mutantSorterSetMap: mutantSorterSetMap) =
        mutantSorterSetMap |> toDto |> Json.serialize
