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
    { mutFuncType: string; mutationRate: float }

module SorterUniformMutatorDto =

    let fromDto (dto: sorterUniformMutatorDto) =
        result {
            let! res =
                match dto.mutFuncType with
                | (nameof sorterUniformMutatorType.Switch) ->
                    let swMr = dto.mutationRate |> MutationRate.create
                    SorterUniformMutator.mutateBySwitch swMr |> Ok
                | (nameof sorterUniformMutatorType.Stage) ->
                    let stMr = dto.mutationRate |> MutationRate.create
                    SorterUniformMutator.mutateByStage stMr |> Ok
                | (nameof sorterUniformMutatorType.StageRfl) ->
                    let stMr = dto.mutationRate |> MutationRate.create
                    SorterUniformMutator.mutateByStageRfl stMr |> Ok
                | _ -> sprintf "%s not matched" dto.mutFuncType |> Error

            return res
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
        { sorterUniformMutatorDto.mutFuncType = string sumType
          mutationRate = mutRateVal }


    let toJson (sorterUniformMutato: sorterUniformMutator) = 
        sorterUniformMutato |> toDto |> Json.serialize
