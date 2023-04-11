namespace global
open System
open Microsoft.FSharp.Core

type sorterSpeedDto = {
    switchCt:int;
    stageCt:int
    }

module SorterSpeedDto =

    let fromDto (dto:sorterSpeedDto) =
        result {
            let switchCount = dto.switchCt |> SwitchCount.create
            let stageCount = dto.stageCt |> StageCount.create
            return SorterSpeed.create switchCount stageCount
        }

    let fromJson (jstr: string) =
        result {
            let! dto = Json.deserialize<sorterSpeedDto> jstr
            return! fromDto dto
        }

    let fromNullabelJson (jstr: string) =
        if jstr = null then None 
        else jstr |> fromJson |> Some


    let toDto (sorterSpeed:sorterSpeed) =
        {
            sorterSpeedDto.switchCt = sorterSpeed 
                                      |> SorterSpeed.getSwitchCount
                                      |> SwitchCount.value;
            stageCt =  sorterSpeed 
                                      |> SorterSpeed.getStageCount
                                      |> StageCount.value;
        }

    let toJson (sorterSpeed: sorterSpeed) =
        sorterSpeed |> toDto |> Json.serialize

    let ofOption (sorterSpeed: sorterSpeed option) =
        match sorterSpeed with
        | Some ss -> ss |> toJson
        | None -> null


type sorterPerfDto = {
    useSuccess:bool;
    isSuccessful:Nullable<bool>;
    sortedSetSize:Nullable<int>
    }

module SorterPerfDto =

    let fromDto (dto:sorterPerfDto) =
        result {
            if dto.useSuccess then
                return dto.isSuccessful.Value
                    |> sorterPerf.IsSuccessful
            else
                return dto.sortedSetSize.Value
                    |> SortableCount.create
                    |> sorterPerf.SortedSetSize
        }

    let fromJson (jstr: string) =
        result {
            let! dto = Json.deserialize<sorterPerfDto> jstr
            return! fromDto dto
        }

    let fromNullabelJson (jstr: string) =
        if jstr = null then None 
        else jstr |> fromJson |> Some

    let toDto (sorterPerf:sorterPerf) =
        match sorterPerf with
        | IsSuccessful bv -> 
            {
                sorterPerfDto.useSuccess = true;
                isSuccessful = bv |> Nullable;
                sortedSetSize = Nullable();
            }
        | SortedSetSize sc ->
            {
                sorterPerfDto.useSuccess = false;
                isSuccessful = Nullable();
                sortedSetSize = sc |> SortableCount.value |> Nullable;
            }

    let toJson (sorterPerf: sorterPerf) =
        sorterPerf |> toDto |> Json.serialize

    let ofOption (sorterPerf: sorterPerf option) =
        match sorterPerf with
        | Some ss -> ss |> toJson
        | None -> null


type sorterEvalDto = { 
        errorMessage: string;
        switchUseCts:int[]; 
        sorterSpeed:string; 
        sorterPrf:string; 
        sortrPhenotypeId:Nullable<Guid>; 
        sortableSetId:Guid;
        sortrId:Guid
     }

module SorterEvalDto =

    let fromDto (dto:sorterEvalDto) =
        result {
            let errorMessage = 
                match dto.errorMessage with
                | null -> None
                | msg -> msg |> Some
                
            let switchUseCts =
                if dto.switchUseCts.Length = 0 then
                    None 
                else
                    dto.switchUseCts 
                    |> SwitchUseCounts.make
                    |> Some

            let! sorterSpeed =
                if dto.sorterSpeed = null then
                    None |> Ok
                 else
                    dto.sorterSpeed 
                    |> SorterSpeedDto.fromJson
                    |> Result.map(Some)

            let! sorterPrf =
                if dto.sorterPrf = null then
                    None |> Ok
                 else
                    dto.sorterPrf
                    |> SorterPerfDto.fromJson
                    |> Result.map(Some)

            let sortrPhenotypeId =
                if dto.sortrPhenotypeId.HasValue then
                    dto.sortrPhenotypeId.Value
                        |> SorterPhenotypeId.create
                        |> Some
                else
                    None

            return SorterEval.make
                        errorMessage
                        switchUseCts
                        sorterSpeed
                        sorterPrf
                        sortrPhenotypeId
                        (dto.sortableSetId |> SortableSetId.create)
                        (dto.sortrId |> SorterId.create)
        }

        
    let fromJson (jstr: string) =
        result {
            let! dto = Json.deserialize<sorterEvalDto> jstr
            return! fromDto dto
        }


    let toDto(sorterEvl:sorterEval) =
        let errorMsg = sorterEvl |> SorterEval.getErrorMessage 
                                 |> StringUtil.OrNull
        let switchUseCts = sorterEvl |> SorterEval.getSwitchUseCounts
                                     |> SwitchUseCounts.ofOption
        {
            errorMessage = errorMsg;
            switchUseCts = switchUseCts; 
            sorterSpeed = sorterEvl |> SorterEval.getSorterSpeed |> SorterSpeedDto.ofOption
            sorterPrf = sorterEvl |> SorterEval.getSorterPerf |> SorterPerfDto.ofOption
            sortrPhenotypeId = sorterEvl |> SorterEval.getSortrPhenotypeId
                                         |> Option.map(SorterPhenotypeId.value)
                                         |> Option.toNullable
            sortableSetId = sorterEvl |> SorterEval.getSortableSetId
                                      |> SortableSetId.value
            sortrId = sorterEvl |> SorterEval.getSorterId |> SorterId.value
        }

    let toJson (sorterEvl:sorterEval) =
        sorterEvl |> toDto |> Json.serialize