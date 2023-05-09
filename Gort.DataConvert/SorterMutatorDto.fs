namespace global
open System
open Microsoft.FSharp.Core

type sorterUniformMutatorDto = 
    { 
      switchGenMode:string; 
      mutationRate:float; 
      switchCountPfx:int 
      switchCountFinal:int 
    }

module SorterUniformMutatorDto =

    let fromDto (dto:sorterUniformMutatorDto) =
        result {
            let! switchGenMode = Json.deserialize<switchGenMode> dto.switchGenMode
            let mutationRate = dto.mutationRate |> MutationRate.create
            let switchCountPfx = 
                match dto.switchCountPfx with
                | -1 -> None
                | x -> x |> SwitchCount.create |> Some
            let switchCountFinal = 
                match dto.switchCountFinal with
                | -1 -> None
                | x -> x |> SwitchCount.create |> Some

            return SorterUniformMutator.create
                        switchCountPfx
                        switchCountFinal
                        switchGenMode
                        mutationRate
        }


    let fromJson (jstr: string) =
        result {
            let! dto = Json.deserialize<sorterUniformMutatorDto> jstr
            return! fromDto dto
        }

    let toDto (sum: sorterUniformMutator) =
        { 
          switchCountPfx = 
            match (SorterUniformMutator.getSwitchCountPrefix sum) with
            | Some scp -> scp |> SwitchCount.value
            | None -> -1
          switchCountFinal = 
            match (SorterUniformMutator.getSwitchCountFinal sum) with
            | Some scp -> scp |> SwitchCount.value
            | None -> -1

          mutationRate =  sum |> SorterUniformMutator.getMutationRate |> MutationRate.value
          switchGenMode = sum |> SorterUniformMutator.getSwitchGenMode |> Json.serialize
          
        }

    let toJson (sortr: sorterUniformMutator) =
        sortr |> toDto |> Json.serialize


type sorterMutatorDto = 
    {
        smType:string
        cereal: string
    }

module SorterMutatorDto =

    let fromDto (dto:sorterMutatorDto) =
        result {
            return!
                match dto.smType with
                | "Uniform" -> 
                    dto.cereal 
                    |> SorterUniformMutatorDto.fromJson
                    |> Result.map(sorterMutator.Uniform)

                | _ -> "sorterMutator type not matched" |> Error
        }

    let fromJson (jstr: string) =
        result {
            let! dto = Json.deserialize<sorterMutatorDto> jstr
            return! fromDto dto
        }

    let toDto (sm: sorterMutator) =
          match sm with
          | Uniform sum ->
                {
                    smType = "Uniform"
                    cereal = sum |> SorterUniformMutatorDto.toJson
                }

    let toJson (sm: sorterMutator) =
        sm |> toDto |> Json.serialize

