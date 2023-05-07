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
        switchUseCts:string; 
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
                
            let! switchUseCts =
                result {
                    if dto.switchUseCts.Length = 0 then
                        return None 
                    else
                        let! sparseA = dto.switchUseCts |> SparseIntArrayDto.fromJson

                        return sparseA
                                |> SparseArray.toArray
                                |> SwitchUseCounts.make
                                |> Some
                }

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
                                 |> StringUtil.nullOption
        let switchUseCts = sorterEvl |> SorterEval.getSwitchUseCounts
                                     |> SwitchUseCounts.ofOption
        {
            errorMessage = errorMsg;
            switchUseCts = switchUseCts |> SparseArray.fromArray 0 |> SparseIntArrayDto.toJson
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



type sorterSetEvalDto = { 
        sorterSetEvalId: Guid;
        sorterSetId:Guid; 
        sortableSetId:Guid; 
        sorterEvals:string[]; 
     }


module SorterSetEvalDto =

    let fromDto (dto:sorterSetEvalDto) =
        result {
            let sorterSetEvalId = 
                    dto.sorterSetEvalId
                    |> SorterSetEvalId.create

            let sorterSetId =
                    dto.sorterSetId
                    |> SorterSetId.create

            let sortableSetId =
                    dto.sortableSetId
                    |> SortableSetId.create

            let! sorterEvals =
                    dto.sorterEvals
                    |> Array.map(SorterEvalDto.fromJson)
                    |> Array.toList
                    |> Result.sequence

            return SorterSetEval.create
                        sorterSetEvalId
                        sorterSetId
                        sortableSetId
                        (sorterEvals |> List.toArray)
        }

        
    let fromJson (jstr: string) =
        result {
            let! dto = Json.deserialize<sorterSetEvalDto> jstr
            return! fromDto dto
        }


    let toDto (ssEvl:sorterSetEval) =
        {
            sorterSetEvalId = ssEvl |> SorterSetEval.getSorterSetEvalId |> SorterSetEvalId.value
            sorterSetId = ssEvl |> SorterSetEval.getSorterSetlId |> SorterSetId.value
            sortableSetId = ssEvl |> SorterSetEval.getSortableSetId |> SortableSetId.value
            sorterEvals = ssEvl |> SorterSetEval.getSorterEvals |> Array.map(SorterEvalDto.toJson)
        }

    let toJson (sorterSetEvl:sorterSetEval) =
        sorterSetEvl |> toDto |> Json.serialize


