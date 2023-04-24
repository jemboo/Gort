namespace global
open System
open Microsoft.FSharp.Core

type rolloutDto =
    { rolloutfmt: string
      arrayLength: int
      bitsPerSymbol: int
      base64: string }

module RolloutDto =

    let fromDto (dto: rolloutDto) =
        result {
            let! rft = dto.rolloutfmt |> RolloutFormat.fromString
            let! bps = dto.bitsPerSymbol |> BitsPerSymbol.create
            let! arrayLen = dto.arrayLength |> ArrayLength.create
            let! bites = ByteUtils.fromBase64 dto.base64
            let bitpk = bites |> BitPack.fromBytes bps
            return! bitpk |> Rollout.fromBitPack rft arrayLen
        }

    let fromJson (jstr: string) =
        result {
            let! dto = Json.deserialize<rolloutDto> jstr
            return! fromDto dto
        }

    let toDto
        (rollout: rollout)
         =
         None

        //{ sortableSetExplicitTableDto.sortableSetId = sortableSetId |> SortableSetId.value
        //  order = order |> Order.value
        //  symbolSetSize = symbolSetSz |> SymbolSetSize.value |> int
        //  bitPackRId = bitPackRId
        //  fmt = rolloutFormt |> RolloutFormat.toString }

    let toJson
        (rollout: rollout)
        =
        rollout
        |> toDto
        |> Json.serialize

