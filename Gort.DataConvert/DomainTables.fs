namespace global
open Microsoft.FSharp.Core
open Gort.DataStore.DataModel


module DomainTables =
    let checkComponentR (componentR:ComponentR) 
                        (categoryName:string) 
                        (versionName:string) =

        match (componentR.Category, componentR.Version) with
        | (c, v) when (c = categoryName && v = versionName) -> true |> Ok
        | (_, _) -> (sprintf "%s %s do not match" categoryName versionName) |> Error



    let rngGenToComponentR (rg:rngGen) 
                           (causeR:CauseR) 
                           (causePath:string) =
        let cereal = rg |> RngGenDto.toDto |> Json.serialize
        let componentR = new ComponentR()
        componentR.CauseR <- causeR
        componentR.CausePath <- causePath
        componentR.Category <- nameof Versions.RandGen
        componentR.Version <- Versions.RandGen.rndGen
        componentR.Json <- cereal
        componentR


    let componentRToRngGen (componentR:ComponentR) =
        result {
            let chk = checkComponentR componentR (nameof Versions.RandGen) Versions.RandGen.rndGen
            let! dto = componentR.Json |> Json.deserialize<rngGenDto>
            let! rg = ( dto |> RngGenDto.fromDto)
            return rg
        }

    let sorterMutatorToComponentR (sm:sorterMutator) 
                                  (causeR:CauseR) 
                                  (causePath:string) =
        let componentR = new ComponentR()
        componentR.CauseR <- causeR
        componentR.CausePath <- causePath
        componentR.Category <- nameof Versions.SorterMutator
        let (vers, cereal) =
            match sm with
            | Uniform sum ->
                let c = sum |> SorterUniformMutatorDto.toJson
                (Versions.SorterMutator.uniform, c)

        componentR.Version <- vers
        componentR.Json <- cereal
        componentR


    let componentRToSorterMutator (componentR:ComponentR) =
        result {
            match componentR.Version with
            | Versions.SorterMutator.uniform ->
                return! componentR.Json  |> SorterUniformMutatorDto.fromJson
                                         |> Result.map(sorterMutator.Uniform)
            | _ -> 
               return! (sprintf "%s is not matched" componentR.Version) |> Error
        }


    let bitPackToBitPackR (bitPack:bitPack) =
        let bitPackR = new BitPackR();
        let dd = bitPack |> BitPack.getData
        bitPackR.BitsPerSymbol <- bitPack |> BitPack.getBitWidth 
                                          |> BitsPerSymbol.value
        bitPackR.SymbolCount <- bitPack |> BitPack.getSymbolCount 
                                        |> SymbolCount.value
        bitPackR.Data <- bitPack |> BitPack.getData
        bitPackR


    let bitPackRToBitPack (bitPackR:BitPackR) =
        result {
            let! bitsPerSymbol = bitPackR.BitsPerSymbol |> BitsPerSymbol.create
            let! symbolCount = bitPackR.SymbolCount |> SymbolCount.create
            let data = bitPackR.Data
            let bitPack = BitPack.create bitsPerSymbol symbolCount data
            return bitPack
        }

    let rec sortableSetRIdToSortableSet (ctxt:IGortContext2) (sortableSetRId:int) =

        let _rngGenLookup (ctxt:IGortContext2) (rndGenId:int) =
            result {
                let! rndGenR = DbLookup.GetRandGenRById ctxt rndGenId
                return! componentRToRngGen rndGenR
            }

        let _sortableSetLookup (ctxt:IGortContext2) (sortableSetRId:int) =
            result {
                let! order = 8 |> Order.create
                let fmt = rolloutFormat.RfI32 |> sortableSetFormat.SsfArrayRoll
                return! SortableSet.makeAllBits 
                    (sortableSetRId |> SortableSetId.create) fmt order 
            }

        let _sorterLookup (ctxt:IGortContext2) (sorterId:int) =
            result {
                let! order = 8 |> Order.create
                return Sorter.fromSwitches order Seq.empty<switch>
            }
        
        let _bitpackLookup (ctxt:IGortContext2) (bitpackId:int) =
            result {
                let! bitpackR = DbLookup.GetBitPackRById ctxt bitpackId
                return! bitPackRToBitPack bitpackR
            }

        result {
            let! sortableSetR = DbLookup.GetSortableSetRById ctxt sortableSetRId
            let! ssVers = sortableSetR.Version |> SortableSetVersion.parse

            return! match ssVers with
                    | AllBitsForOrder -> 
                            SortableSetAllBitsDto.fromJson sortableSetR.Json
                    | Orbit -> 
                            SortableSetOrbitDto.fromJson sortableSetR.Json
                    | SortedStack -> 
                            SortableSetSortedStacksDto.fromJson sortableSetR.Json
                    | RandomPermutation -> 
                            SortableSetRandomPermutationDto.fromJson sortableSetR.Json 
                                                                     (_rngGenLookup ctxt)
                    | RandomBits -> 
                            SortableSetRandomBitsDto.fromJson sortableSetR.Json 
                                                              (_rngGenLookup ctxt)
                    | RandomSymbols -> 
                            SortableSetRandomSymbolsDto.fromJson sortableSetR.Json 
                                                                 (_rngGenLookup ctxt)
                    | Explicit -> 
                            SortableSetExplicitDto.fromJson sortableSetR.Json 
                                                              (_bitpackLookup ctxt)
                    | SwitchReduced -> 
                            SortableSetSwitchReducedDto.fromJson sortableSetR.Json 
                                                                 (sortableSetRIdToSortableSet ctxt)
                                                                 (_sorterLookup ctxt)
        }