namespace global
open Microsoft.FSharp.Core

type sortableSetFormatDto = {key:string; value:string}

module SortableSetFormat =
    let makeBitStriped expandBitSets =
        sortableSetFormat.SsfBitStriped expandBitSets

    let makeRollout (rolloutFormat:rolloutFormat) =
        sortableSetFormat.SsfArrayRoll rolloutFormat

    let toDto (ssf:sortableSetFormat) =
        match ssf with
        | sortableSetFormat.SsfArrayRoll rollfmt -> 
                        {sortableSetFormatDto.key = nameof sortableSetFormat.SsfArrayRoll;
                         value = (rollfmt |> RolloutFormat.toDto)}
        | sortableSetFormat.SsfBitStriped expBs ->
                        { sortableSetFormatDto.key = nameof sortableSetFormat.SsfBitStriped;
                          value = (expBs |> ExpandBitSets.value |> string)}

    let toJson (ssf:sortableSetFormat) =
        ssf |> toDto |> Json.serialize

    let fromDto (dto:sortableSetFormatDto) =
        result {
            if (dto.key = nameof sortableSetFormat.SsfArrayRoll) then
                let! rof = dto.value |> RolloutFormat.create
                return rof |> sortableSetFormat.SsfArrayRoll
            else
                let expB = dto.value |> bool.Parse |> ExpandBitSets.create
                return expB |> sortableSetFormat.SsfBitStriped
        }

    let fromJson (jstr:string) =
        result {
            let! dto = Json.deserialize<sortableSetFormatDto> jstr
            return! fromDto dto
        }


type sortableSetAllBitsDto = { sortableSetId:int; order:int; fmt:string }
module SortableSetAllBitsDto =

    let fromDto (dto:sortableSetAllBitsDto) =
        result {
            let! order = dto.order |> Order.create
            let! ssFormat = dto.fmt |> SortableSetFormat.fromJson
            return! SortableSet.makeAllBits (dto.sortableSetId |> SortableSetId.create)
                        ssFormat order
        }

    let fromJson (jstr:string) =
        result {
            let! dto = Json.deserialize<sortableSetAllBitsDto> jstr
            return! fromDto dto
        }

    let toDto (sortableSetId:sortableSetId) (ssfmt:sortableSetFormat) (ord:order) =
         { sortableSetAllBitsDto.sortableSetId = sortableSetId |> SortableSetId.value;
           order =  (ord |> Order.value);
           fmt = ssfmt |> SortableSetFormat.toJson }

    let toJson (sortableSetId:sortableSetId) (ssfmt:sortableSetFormat) (ord:order) =
        ord |> toDto sortableSetId ssfmt |> Json.serialize


type sortableSetOrbitDto = { sortableSetId:int; maxCount:int; permutation:int[]; fmt:string }
module SortableSetOrbitDto =

    let fromDto (dto:sortableSetOrbitDto) =
        result {
            let maxCt =
                match dto.maxCount with
                | v when v > 0 -> v |> SortableCount.create |> Some
                | _ -> None
            let! perm = dto.permutation |> Permutation.create
            let! ssFormat = dto.fmt |> SortableSetFormat.fromJson
            return! SortableSet.makeOrbits (dto.sortableSetId |> SortableSetId.create) 
                        ssFormat maxCt perm
        }

    let fromJson (jstr:string) =
        result {
            let! dto = Json.deserialize<sortableSetOrbitDto> jstr
            return! fromDto dto
        }

    let toDto (sortableSetId:sortableSetId) (ssfmt:sortableSetFormat) 
              (maxCount:int) (perm:permutation) =
         { sortableSetOrbitDto.sortableSetId = sortableSetId |> SortableSetId.value;
           maxCount = maxCount;
           sortableSetOrbitDto.permutation = perm |> Permutation.getArray
           fmt = ssfmt |> SortableSetFormat.toJson }

    let toJson (sortableSetId:sortableSetId) (ssfmt:sortableSetFormat) 
               (maxCount:int) (perm:permutation) =
        perm |> toDto sortableSetId ssfmt maxCount |> Json.serialize


type sortableSetSortedStacksDto = { sortableSetId:int; orderStack:int[]; fmt:string }
module SortableSetSortedStacksDto =

    let fromDto (dto:sortableSetSortedStacksDto) =
        result {
            let! orderStack = dto.orderStack |> Array.map(Order.create)
                                             |> Array.toList
                                             |> Result.sequence
            let! ssFormat = dto.fmt |> SortableSetFormat.fromJson
            return! SortableSet.makeSortedStacks (dto.sortableSetId |> SortableSetId.create)
                            ssFormat (orderStack |> List.toArray)
        }

    let fromJson (jstr:string) =
        result {
            let! dto = Json.deserialize<sortableSetSortedStacksDto> jstr
            return! fromDto dto
        }

    let toDto (sortableSetId:sortableSetId) (ssfmt:sortableSetFormat) 
              (orderStack:order[]) =
         { sortableSetSortedStacksDto.sortableSetId = sortableSetId |> SortableSetId.value;
           orderStack = orderStack |> Array.map(Order.value);
           fmt = ssfmt |> SortableSetFormat.toJson }

    let toJson (sortableSetId:sortableSetId) (ssfmt:sortableSetFormat) 
               (orderStack:order[]) =
        orderStack |> toDto sortableSetId ssfmt |> Json.serialize


type sortableSetRandomPermutationDto = 
                { sortableSetId:int; order:int;
                  sortableCount:int; rngGenId:int; fmt:string }
module SortableSetRandomPermutationDto =

    let fromDto (dto:sortableSetRandomPermutationDto) (rnGenLookup: int -> Result<rngGen, string>) =
        result {
            let! rngGen = rnGenLookup dto.rngGenId
            let randy = Rando.fromRngGen rngGen
            let! order = dto.order |> Order.create
            let! ssFormat = dto.fmt |> SortableSetFormat.fromJson
            let sortableCt = dto.sortableCount |> SortableCount.create
            return! SortableSet.makeRandomPermutation 
                    (dto.sortableSetId |> SortableSetId.create)
                    ssFormat order sortableCt randy
        }

    let fromJson (jstr:string) (rnGenLookup: int -> Result<rngGen, string>) =
        result {
            let! dto = Json.deserialize<sortableSetRandomPermutationDto> jstr
            return! fromDto dto rnGenLookup
        }

    let toDto (sortableSetId:sortableSetId) (ssfmt:sortableSetFormat) 
              (ord:order) (sortableCt:sortableCount) 
              (rngId:int) =
         { sortableSetRandomPermutationDto.sortableSetId = sortableSetId |> SortableSetId.value;
           sortableSetRandomPermutationDto.order = ord |> Order.value;
           sortableSetRandomPermutationDto.rngGenId = rngId;
           sortableSetRandomPermutationDto.sortableCount = (sortableCt |> SortableCount.value)
           fmt = ssfmt |> SortableSetFormat.toJson }

    let toJson (sortableSetId:sortableSetId) (ssfmt:sortableSetFormat) 
               (ord:order) (sortableCt:sortableCount) 
               (rngId:int) =
        rngId |> toDto sortableSetId ssfmt ord sortableCt |> Json.serialize



type sortableSetRandomBitsDto = {
                  sortableSetId:int;
                  order:int;  pctOnes:float;
                  sortableCount:int; rngGenId:int; fmt:string }

module SortableSetRandomBitsDto =

    let fromDto (dto:sortableSetRandomBitsDto)
                (rnGenLookup: int -> Result<rngGen, string>) =
        result {
            let! rngGen = rnGenLookup dto.rngGenId
            let randy = Rando.fromRngGen rngGen
            let! order = dto.order |> Order.create
            let! ssFormat = dto.fmt |> SortableSetFormat.fromJson
            let sortableCt = dto.sortableCount |> SortableCount.create
            return! SortableSet.makeRandomBits 
                       (dto.sortableSetId |> SortableSetId.create)
                       ssFormat 
                       order dto.pctOnes sortableCt randy
        }

    let fromJson (jstr:string) (rnGenLookup: int -> Result<rngGen, string>) =
        result {
            let! dto = Json.deserialize<sortableSetRandomBitsDto> jstr
            return! fromDto dto rnGenLookup
        }

    let toDto (sortableSetId:sortableSetId) (ssfmt:sortableSetFormat) 
              (ord:order) (pctOnes:float) 
              (sortableCt:sortableCount) (rngGenId:int) =
         { sortableSetRandomBitsDto.sortableSetId = sortableSetId |> SortableSetId.value;
           order = ord |> Order.value;
           pctOnes = pctOnes;
           sortableCount = sortableCt |> SortableCount.value;
           rngGenId = rngGenId;
           fmt = ssfmt |> SortableSetFormat.toJson }

    let toJson (sortableSetId:sortableSetId) 
               (ssfmt:sortableSetFormat) (ord:order) (pctOnes:float) 
               (sortableCt:sortableCount) (rngGenId:int) =
        rngGenId |> toDto sortableSetId ssfmt ord pctOnes sortableCt |> Json.serialize


type sortableSetRandomSymbolsDto = 
                { sortableSetId:int;
                  order:int; symbolSetSize:int; 
                  sortableCount:int; rngGenId:int; fmt:string }
module SortableSetRandomSymbolsDto =

    let fromDto (dto:sortableSetRandomSymbolsDto) 
                (rnGenLookup: int -> Result<rngGen, string>) =
        result {
            let! rngGen = rnGenLookup dto.rngGenId
            let randy = Rando.fromRngGen rngGen
            let! order = dto.order |> Order.create
            let! ssFormat = dto.fmt |> SortableSetFormat.fromJson
            let! symbolSetSize = dto.symbolSetSize |> uint64 |> SymbolSetSize.create
            let sortableCt = dto.sortableCount |> SortableCount.create
            return! SortableSet.makeRandomSymbols 
                        (dto.sortableSetId |> SortableSetId.create) ssFormat 
                        order symbolSetSize sortableCt randy
        }

    let fromJson (jstr:string) (rnGenLookup: int -> Result<rngGen, string>) =
        result {
            let! dto = Json.deserialize<sortableSetRandomSymbolsDto> jstr
            return! fromDto dto rnGenLookup
        }

    let toDto (sortableSetId:sortableSetId)
              (ssfmt:sortableSetFormat) (order:order) (symbolSetSz:symbolSetSize) 
              (sortableCt:sortableCount) (rngGenId:int) =
         { sortableSetRandomSymbolsDto.sortableSetId = sortableSetId |> SortableSetId.value;
           order = order |> Order.value;
           symbolSetSize = symbolSetSz |> SymbolSetSize.value |> int;
           sortableCount = sortableCt |> SortableCount.value;
           rngGenId = rngGenId;
           fmt = ssfmt |> SortableSetFormat.toJson }

    let toJson (sortableSetId:sortableSetId) 
               (ssfmt:sortableSetFormat) (order:order) (symbolSetSz:symbolSetSize) 
               (sortableCt:sortableCount) (rngGenId:int) =
        rngGenId |> toDto sortableSetId ssfmt order symbolSetSz sortableCt|> Json.serialize

type sortableSetExplicitDto = 
                { sortableSetId:int;
                  order:int; symbolSetSize:int; 
                  bitPackRId:int; fmt:string }

module SortableSetExplicitDto =

    let fromDto (dto:sortableSetExplicitDto) 
                (bitPackLookup: int -> Result<bitPack, string>) =
        result {
            let! bitPack = bitPackLookup dto.bitPackRId
            let! order = dto.order |> Order.create
            let! ssFormat = dto.fmt |> SortableSetFormat.fromJson
            let! symbolSetSize = dto.symbolSetSize |> uint64 |> SymbolSetSize.create
            return! SortableSet.fromBitPack dto.sortableSetId ssFormat order symbolSetSize bitPack
        }

    let fromJson (jstr:string) (bitPackLookup: int -> Result<bitPack, string>) =
        result {
            let! dto = Json.deserialize<sortableSetExplicitDto> jstr
            return! fromDto dto bitPackLookup
        }

    let toDto (sortableSetId:sortableSetId) (ssfmt:sortableSetFormat) 
              (order:order) (symbolSetSz:symbolSetSize) 
              (sortableCt:sortableCount) (bitPackRId:int) =
         { sortableSetExplicitDto.sortableSetId = sortableSetId |> SortableSetId.value;
           order = order |> Order.value;
           symbolSetSize = symbolSetSz |> SymbolSetSize.value |> int;
           bitPackRId = bitPackRId;
           fmt = ssfmt |> SortableSetFormat.toJson }

    let toJson (sortableSetId:sortableSetId) (ssfmt:sortableSetFormat) 
               (order:order) (symbolSetSz:symbolSetSize) 
               (sortableCt:sortableCount) (rngGenId:int) =
        rngGenId |> toDto sortableSetId ssfmt order symbolSetSz sortableCt|> Json.serialize



type sortableSetSwitchReducedDto = 
                { sortableSetId:int; sorterId:int; 
                  sortableSetSourceId:int; fmt:string }

module SortableSetSwitchReducedDto =

    let fromDto (dto:sortableSetSwitchReducedDto) 
                (sortableSetLookup: int -> Result<sortableSet, string>) 
                (sorterLookup: int -> Result<sorter, string>) =
        result {
            let! sortableSet = sortableSetLookup dto.sortableSetSourceId
            let! sorter = sorterLookup dto.sorterId
            let! ssFormat = dto.fmt |> SortableSetFormat.fromJson
            return! SortableSet.switchReduce (dto.sortableSetId |> SortableSetId.create) 
                        ssFormat sortableSet sorter 
        }

    let fromJson (jstr:string) (sortableSetLookup: int -> Result<sortableSet, string>) 
                 (sorterLookup: int -> Result<sorter, string>) =
        result {
            let! dto = Json.deserialize<sortableSetSwitchReducedDto> jstr
            return! fromDto dto sortableSetLookup sorterLookup
        }

    let toDto (sortableSetId:sortableSetId) (ssfmt:sortableSetFormat) 
              (sorterId:int) (sortableSetSourceId:int) =
         { sortableSetSwitchReducedDto.sorterId = sorterId;
           fmt = ssfmt |> SortableSetFormat.toJson;
           sortableSetId = sortableSetId |> SortableSetId.value;
           sortableSetSourceId=sortableSetSourceId}

    let toJson (sortableSetId:sortableSetId) (ssfmt:sortableSetFormat) 
               (sorterId:int) (sortableSetSourceId:int) =
         toDto sortableSetId ssfmt sorterId sortableSetSourceId |> Json.serialize