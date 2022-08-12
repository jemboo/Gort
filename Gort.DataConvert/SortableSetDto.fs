namespace global
open Microsoft.FSharp.Core
open System.IO
open Newtonsoft.Json
open System



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


type sortableSetAllBitsDto = { order:int; fmt:string }
module SortableSetAllBitsDto =

    let fromDto (dto:sortableSetAllBitsDto) =
        result {
            let! order = dto.order |> Order.create
            let! ssFormat = dto.fmt |> SortableSetFormat.fromJson
            return! SortableSet.makeAllBits ssFormat order
        }

    let fromJson (jstr:string) =
        result {
            let! dto = Json.deserialize<sortableSetAllBitsDto> jstr
            return! fromDto dto
        }

    let toDto (ssfmt:sortableSetFormat) (ord:order) =
         { sortableSetAllBitsDto.order =  (ord |> Order.value);
           fmt = ssfmt |> SortableSetFormat.toJson }

    let toJson (ssfmt:sortableSetFormat) (ord:order) =
        ord |> toDto ssfmt |> Json.serialize


type sortableSetOrbitDto = { maxCount:int; permutation:int[]; fmt:string }
module SortableSetOrbitDto =

    let fromDto (dto:sortableSetOrbitDto) =
        result {
            let maxCt =
                match dto.maxCount with
                | v when v > 0 -> v |> SortableCount.create |> Some
                | _ -> None
            let! perm = dto.permutation |> Permutation.create
            let! ssFormat = dto.fmt |> SortableSetFormat.fromJson
            return! SortableSet.makeOrbits ssFormat maxCt perm
        }

    let fromJson (jstr:string) =
        result {
            let! dto = Json.deserialize<sortableSetOrbitDto> jstr
            return! fromDto dto
        }

    let toDto (ssfmt:sortableSetFormat) (maxCount:int) (perm:permutation) =
         { sortableSetOrbitDto.maxCount = maxCount;
           sortableSetOrbitDto.permutation = perm |> Permutation.getArray
           fmt = ssfmt |> SortableSetFormat.toJson }

    let toJson (ssfmt:sortableSetFormat) (maxCount:int) (perm:permutation) =
        perm |> toDto ssfmt maxCount |> Json.serialize


type sortableSetSortedStacksDto = { orderStack:int[]; fmt:string }
module SortableSetSortedStacksDto =

    let fromDto (dto:sortableSetSortedStacksDto) =
        result {
            let! orderStack = dto.orderStack |> Array.map(Order.create)
                                             |> Array.toList
                                             |> Result.sequence
            let! ssFormat = dto.fmt |> SortableSetFormat.fromJson
            return! SortableSet.makeSortedStacks ssFormat (orderStack |> List.toArray)
        }

    let fromJson (jstr:string) =
        result {
            let! dto = Json.deserialize<sortableSetSortedStacksDto> jstr
            return! fromDto dto
        }

    let toDto (ssfmt:sortableSetFormat) (orderStack:order[]) =
         { sortableSetSortedStacksDto.orderStack = orderStack |> Array.map(Order.value);
           fmt = ssfmt |> SortableSetFormat.toJson }

    let toJson (ssfmt:sortableSetFormat) (orderStack:order[]) =
        orderStack |> toDto ssfmt |> Json.serialize


type sortableSetRandomPermutationDto = { order:int;
                  sortableCount:int; rngGenId:int; fmt:string }
module SortableSetRandomPermutationDto =

    let fromDto (dto:sortableSetRandomPermutationDto) (rnGenLookup: int -> Result<rngGen, string>) =
        result {
            let! rngGen = rnGenLookup dto.rngGenId
            let randy = Rando.fromRngGen rngGen
            let! order = dto.order |> Order.create
            let! ssFormat = dto.fmt |> SortableSetFormat.fromJson
            let sortableCt = dto.sortableCount |> SortableCount.create
            return! SortableSet.makeRandomPermutation ssFormat order sortableCt randy
        }

    let fromJson (jstr:string) (rnGenLookup: int -> Result<rngGen, string>) =
        result {
            let! dto = Json.deserialize<sortableSetRandomPermutationDto> jstr
            return! fromDto dto rnGenLookup
        }

    let toDto (ssfmt:sortableSetFormat) (ord:order) (sortableCt:sortableCount) 
              (rngId:int) =
         { sortableSetRandomPermutationDto.order = ord |> Order.value;
           sortableSetRandomPermutationDto.rngGenId = rngId;
           sortableSetRandomPermutationDto.sortableCount = (sortableCt |> SortableCount.value)
           fmt = ssfmt |> SortableSetFormat.toJson }

    let toJson (ssfmt:sortableSetFormat) (ord:order) (sortableCt:sortableCount) 
               (rngId:int) =
        rngId |> toDto ssfmt ord sortableCt |> Json.serialize



type sortableSetRandomBitsDto = { order:int;  pctOnes:float;
                  sortableCount:int; rngGenId:int; fmt:string }
module SortableSetRandomBitsDto =

    let fromDto (dto:sortableSetRandomBitsDto) (rnGenLookup: int -> Result<rngGen, string>) =
        result {
            let! rngGen = rnGenLookup dto.rngGenId
            let randy = Rando.fromRngGen rngGen
            let! order = dto.order |> Order.create
            let! ssFormat = dto.fmt |> SortableSetFormat.fromJson
            let sortableCt = dto.sortableCount |> SortableCount.create
            return! SortableSet.makeRandomBits ssFormat order dto.pctOnes sortableCt randy
        }

    let fromJson (jstr:string) (rnGenLookup: int -> Result<rngGen, string>) =
        result {
            let! dto = Json.deserialize<sortableSetRandomBitsDto> jstr
            return! fromDto dto rnGenLookup
        }

    let toDto (ssfmt:sortableSetFormat) (ord:order) (pctOnes:float) 
              (sortableCt:sortableCount) (rngGenId:int) =
         { sortableSetRandomBitsDto.order = ord |> Order.value;
           pctOnes = pctOnes;
           sortableCount = sortableCt |> SortableCount.value;
           rngGenId = rngGenId;
           fmt = ssfmt |> SortableSetFormat.toJson }

    let toJson (ssfmt:sortableSetFormat) (ord:order) (pctOnes:float) 
               (sortableCt:sortableCount) (rngGenId:int) =
        rngGenId |> toDto ssfmt ord pctOnes sortableCt |> Json.serialize


type sortableSetRandomSymbolsDto = 
                { order:int; symbolSetSize:int; 
                  sortableCount:int; rngGenId:int; fmt:string }
module SortableSetRandomSymbolsDto =

    let fromDto (dto:sortableSetRandomSymbolsDto) 
                (rnGenLookup: int -> Result<rngGen, string>) =
        result {
            let! rngGen = rnGenLookup dto.rngGenId
            let randy = Rando.fromRngGen rngGen
            let! order = dto.order |> Order.create
            let! ssFormat = dto.fmt |> SortableSetFormat.fromJson
            let! symbolSetSize = dto.sortableCount |> uint64 |> SymbolSetSize.create
            let sortableCt = dto.sortableCount |> SortableCount.create
            return! SortableSet.makeRandomSymbols ssFormat order symbolSetSize sortableCt randy
        }

    let fromJson (jstr:string) (rnGenLookup: int -> Result<rngGen, string>) =
        result {
            let! dto = Json.deserialize<sortableSetRandomSymbolsDto> jstr
            return! fromDto dto rnGenLookup
        }

    let toDto (ssfmt:sortableSetFormat) (order:order) (symbolSetSz:symbolSetSize) 
              (sortableCt:sortableCount) (rngGenId:int) =
         { sortableSetRandomSymbolsDto.order = order |> Order.value;
           symbolSetSize = symbolSetSz |> SymbolSetSize.value |> int;
           sortableCount = sortableCt |> SortableCount.value;
           rngGenId = rngGenId;
           fmt = ssfmt |> SortableSetFormat.toJson }

    let toJson (ssfmt:sortableSetFormat) (order:order) (symbolSetSz:symbolSetSize) 
               (sortableCt:sortableCount) (rngGenId:int) =
        rngGenId |> toDto ssfmt order symbolSetSz sortableCt|> Json.serialize