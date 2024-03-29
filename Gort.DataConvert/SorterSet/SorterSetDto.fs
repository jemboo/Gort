﻿namespace global
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

