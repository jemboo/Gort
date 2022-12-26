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


type sorterUniformMutatorDto =
    { mutFuncType: string; mutationRate: float; switchCountPfx:int; switchCountFinal:int }

module SorterUniformMutatorDto =

    let fromDto (dto: sorterUniformMutatorDto) =
        let switchCtPfx = 
            if (dto.switchCountPfx > 0) then
                dto.switchCountPfx |> SwitchCount.create|> Some
            else None
        let switchCtFinal = 
            if (dto.switchCountFinal > 0) then
                dto.switchCountFinal |> SwitchCount.create|> Some
            else None
        result {
            let srtrMutRat = dto.mutationRate |> MutationRate.create
            let! sumt =
                match dto.mutFuncType with
                | (nameof sorterUniformMutatorType.Switch) ->
                    sorterUniformMutatorType.Switch|> Ok
                | (nameof sorterUniformMutatorType.Stage) ->
                    sorterUniformMutatorType.Stage |> Ok
                | (nameof sorterUniformMutatorType.StageRfl) ->
                    sorterUniformMutatorType.StageRfl |> Ok
                | _ -> sprintf "%s not matched" dto.mutFuncType |> Error

            return SorterUniformMutator.create switchCtPfx switchCtFinal sumt srtrMutRat
        }


    let fromJson (jstr: string) =
        result {
            let! dto = Json.deserialize<sorterUniformMutatorDto> jstr
            return! fromDto dto
        }


    let toDto (sum: sorterUniformMutator) =
        let sumType = sum |> SorterUniformMutator.getSorterUniformMutatorType
        let mutRateVal = sum |> SorterUniformMutator.getMutationRate
                             |> MutationRate.value
        { 
          sorterUniformMutatorDto.mutFuncType = string sumType
          mutationRate = mutRateVal 
          switchCountPfx = match (sum |> SorterUniformMutator.getPrefixSwitchCount) with
                           | Some ct -> ct |> SwitchCount.value
                           | None -> 0
          switchCountFinal = match (sum |> SorterUniformMutator.getFinalSwitchCount) with
                           | Some ct -> ct |> SwitchCount.value
                           | None -> 0
        }


    let toJson (sorterUniformMutato: sorterUniformMutator) = 
        sorterUniformMutato |> toDto |> Json.serialize
