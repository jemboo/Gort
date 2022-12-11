namespace global
open System
open Microsoft.FSharp.Core

type sorterDto = { id:Guid; order:int; switches:byte[] }

module SorterDto =

    let fromDto (dto:sorterDto) =
        result {
            let! order = dto.order |> Order.create
            let bps = order |> Switch.bitsPerSymbolRequired
            let bitPck = BitPack.fromBytes bps dto.switches
            let switches = Switch.fromBitPack bitPck
            let sorterId = dto.id |> SorterId.create
            return Sorter.fromSwitches sorterId order switches
        }


    let fromJson (jstr: string) =
        result {
            let! dto = Json.deserialize<sorterDto> jstr
            return! fromDto dto
        }


    let toDto (sortr: sorter) =
        { 
          sorterDto.id = sortr |> Sorter.getSorterId |> SorterId.value;
          order =  sortr |> Sorter.getOrder |> Order.value;
          switches = sortr |> Sorter.toByteArray
        }


    let toJson (sortr: sorter) =
        sortr |> toDto |> Json.serialize
