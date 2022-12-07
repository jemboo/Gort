namespace global
open System
open Microsoft.FSharp.Core

type sorterSetDto = { 
        id: Guid; order:int; sorterIds: Guid[]; 
        offsets: int[]; symbolCounts:int[]; switches: byte[] }

module SorterSetDto =

    let fromDto (dto:sorterSetDto) =
        result {
            let! order = dto.order |> Order.create
            let bps = order |> Switch.bitsPerSymbolRequired
            let sorterSetId = dto.id |> SorterSetId.create
            return SorterSet.load sorterSetId order
        }


    let fromJson (jstr: string) =
        result {
            let! dto = Json.deserialize<sorterSetDto> jstr
            return! fromDto dto
        }


    let toDto (sorterSt: sorterSet) =
        let sOrder = sorterSt |> SorterSet.getOrder
        { 
            sorterSetDto.id = sorterSt |> SorterSet.getId |> SorterSetId.value;
            order =  sOrder |> Order.value;
            sorterIds = [||];
            offsets = [||];
            symbolCounts = [||];
            switches = [||];
        }


    let toJson (sorterSt: sorterSet) =
        sorterSt |> toDto |> Json.serialize