namespace global
open Microsoft.FSharp.Core
open System.IO
open Newtonsoft.Json
open System


module RngType =

    let toDto (rngt: rngType) =
        match rngt with
        | rngType.Lcg -> nameof rngType.Lcg
        | rngType.Net -> nameof rngType.Net
        | _ -> failwith (sprintf "no match for RngType: %A" rngt)

    let create str =
        match str with
        | nameof rngType.Lcg -> rngType.Lcg |> Ok
        | nameof rngType.Net -> rngType.Net |> Ok
        | _ -> Error (sprintf "no match for RngType: %s" str)


type rngGenDto = {rngType:string; seed:int}

module RngGenDto =

    let fromDto (dto:rngGenDto) =
        result {
            let! typ = RngType.create dto.rngType
            let rs = RandomSeed.create dto.seed
            return {rngGen.rngType=typ; seed=rs}
        }

    let fromJson (jstr:string) =
        result {
            let! dto = Json.deserialize<rngGenDto> jstr
            return! fromDto dto
        }

    let toDto (rngGen:rngGen) =
        { rngType=(RngType.toDto rngGen.rngType); 
          seed=RandomSeed.value rngGen.seed }

    let toJson (rngGen:rngGen) =
        rngGen |> toDto |> Json.serialize