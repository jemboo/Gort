namespace global
open Microsoft.FSharp.Core
open System.IO
open Newtonsoft.Json


module Json =

    type Marker = interface end
        
    let serialize obj = JsonConvert.SerializeObject obj
        
    let deserialize<'a> str :Result<'a, string> =
        try
            JsonConvert.DeserializeObject<'a> str |> Ok
        with
        | ex -> Result.Error ex.Message
        
    let deserializeOption<'a> str =
        match str with
        | Some s -> (deserialize<'a> s)
        | None -> Result.Error  "option was none"


module RngType =

    let toDto (rngt: rngType) =
        match rngt with
        | Lcg -> nameof rngType.Lcg
        | Net -> nameof rngType.Net
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
        {rngType=(RngType.toDto rngGen.rngType); 
         seed=RandomSeed.value rngGen.seed}

    let toJson (rngGen:rngGen) =
        rngGen |> toDto |> Json.serialize

