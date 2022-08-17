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


type sorterUniformMutatorDto = {sumType:string; mutationRate:float}

module SorterUniformMutatorDto =

    let fromDto (dto:sorterUniformMutatorDto) =
        result {
            let! res =
                match dto.sumType with
                | (nameof sorterUniformMutatorType.Switch) ->
                    let swMr = dto.mutationRate |> SwitchMutationRate.create
                    SorterUniformMutator.mutateBySwitch swMr |> Ok
                | (nameof sorterUniformMutatorType.Stage) -> 
                    let stMr = dto.mutationRate |> StageMutationRate.create
                    SorterUniformMutator.mutateByStage stMr |> Ok
                | (nameof sorterUniformMutatorType.StageRfl) -> 
                    let stMr = dto.mutationRate |> StageMutationRate.create
                    SorterUniformMutator.mutateByStageRfl stMr |> Ok
                | _ -> sprintf "%s not matched" dto.sumType |> Error

            return res
        }


    let fromJson (jstr:string) =
        result {
            let! dto = Json.deserialize<sorterUniformMutatorDto> jstr
            return! fromDto dto
        }


    let toDto (sum:sorterUniformMutator) =
        let sumType = sum |> SorterUniformMutator.getSorterUniformMutatorType
        let mutRateVal = sum |> SorterUniformMutator.getMutationRateVal
        { sorterUniformMutatorDto.sumType = string sumType;
          mutationRate = mutRateVal }


    let toJson (rngGen:sorterUniformMutator) =
        rngGen |> toDto |> Json.serialize


