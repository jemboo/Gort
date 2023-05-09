namespace global

open Microsoft.FSharp.Core
open System.IO
open Newtonsoft.Json
open System


type sparseIntArrayDto = 
    { emptyVal:int; 
      length:int; 
      indexes:int[];
      values:int[] }

module SparseIntArrayDto =

    let fromDto (dto:sparseIntArrayDto) =
        result {
            return 
                SparseArray.create
                    dto.length
                    dto.indexes
                    dto.values
                    dto.emptyVal
        }

    let toDto 
            (sia:sparseArray<int>) 
        =
        { 
            emptyVal = sia |> SparseArray.getEmptyVal; 
            length = sia |> SparseArray.getLength; 
            indexes = sia |> SparseArray.getIndexes;
            values =  sia |> SparseArray.getValues
        }

        
    let fromJson (cereal:string) =
        result {
            let! dto = Json.deserialize<sparseIntArrayDto> cereal
            return! fromDto dto
        }

    let toJson (sia:sparseArray<int>) = 
        sia |> toDto |> Json.serialize



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
        | _ -> Error(sprintf "no match for RngType: %s" str)


type rngGenDto = { rngType: string; seed: int }

module RngGenDto =

    let fromDto (dto: rngGenDto) =
        result {
            let! typ = RngType.create dto.rngType
            let rs = RandomSeed.create dto.seed
            return RngGen.create typ rs
        }

    let fromJson (jstr: string) =
        result {
            let! dto = Json.deserialize<rngGenDto> jstr
            return! fromDto dto
        }

    let toDto (rngGen: rngGen) =
        { rngType = rngGen |> RngGen.getType |> RngType.toDto
          seed =  rngGen |> RngGen.getSeed |> RandomSeed.value }

    let toJson (rngGen: rngGen) = rngGen |> toDto |> Json.serialize