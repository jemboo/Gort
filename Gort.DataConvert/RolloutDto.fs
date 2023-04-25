namespace global
open System
open Microsoft.FSharp.Core

type rolloutDto =
    { rolloutfmt: string
      arrayLength: int
      arrayCount: int
      bitsPerSymbol: int
      base64: string }

module RolloutDto =

    let fromDto 
            (dto: rolloutDto) 
        =
        result {
            let! rft = dto.rolloutfmt |> RolloutFormat.fromString
            let! bps = dto.bitsPerSymbol |> BitsPerSymbol.create
            let! arrayLen = dto.arrayLength |> ArrayLength.create
            let! bites = ByteUtils.fromBase64 dto.base64
            let bitpk = bites |> BitPack.fromBytes bps
            return! bitpk |> Rollout.fromBitPack rft arrayLen
        }

    let fromJson 
            (jstr: string) 
        =
        result {
            let! dto = Json.deserialize<rolloutDto> jstr
            return! fromDto dto
        }

    let toDto
            (rollout: rollout)
         =
         let fmt = 
            rollout 
                |> Rollout.getRolloutFormat
                |> RolloutFormat.toString

         let arrayLen =
             rollout
                |> Rollout.getArrayLength
                |> ArrayLength.value

         let arrayCount =
             rollout
                |> Rollout.getArrayCount
                |> ArrayCount.value

         let bitsPerSymbol =
             rollout
                |> Rollout.getBitsPerSymbol
                |> BitsPerSymbol.value

         result {

            let! byteSeq = rollout |> Rollout.getDataBytes
            let data64 = byteSeq |> ByteUtils.toBase64

            return  { 
                rolloutfmt = fmt
                arrayLength = arrayLen
                arrayCount = arrayCount
                bitsPerSymbol = bitsPerSymbol
                base64 = data64 
            }
          }

    let toJson
        (rollout: rollout)
        =
        rollout
        |> toDto |> Result.ExtractOrThrow
        |> Json.serialize

