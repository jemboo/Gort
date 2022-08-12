namespace global
open Microsoft.FSharp.Core
open Gort.DataStore.DataModel


module DomainTables =
        
    let rngGenToRandGenR (rg:rngGen) 
                         (causeId:int) 
                         (causePath:string) =
        let cereal = rg |> RngGenDto.toDto |> Json.serialize
        let randGen = new RandGenR()
        randGen.CauseId <- causeId
        randGen.CausePath <- causePath
        randGen.Version <- Versions.RandGen_rndGen
        randGen.Json <- cereal
        randGen


    let randGenRToRngGen (randGen:RandGenR) =
        result {

            if (randGen.Version <>  Versions.RandGen_rndGen) then
                let! res = (sprintf "randGen.Version is %s not %s" 
                               randGen.Version
                               Versions.RandGen_rndGen) 
                           |> Error
                return! res
            else
                let! dto = randGen.Json |> Json.deserialize<rngGenDto>
                let! rg = ( dto |> RngGenDto.fromDto)
                return rg
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

    type sortableSetVersion = | AllBitsForOrder | Orbit | SortedStack 
                              | RandomPermutation | RandomBits | RandomSymbols

    module SortableSetVersion =
        let parse (strRep:string) =
            match strRep with
            | nameof sortableSetVersion.AllBitsForOrder -> 
                sortableSetVersion.AllBitsForOrder |> Ok
            | nameof sortableSetVersion.Orbit -> 
                sortableSetVersion.Orbit |> Ok
            | nameof sortableSetVersion.SortedStack -> 
                sortableSetVersion.SortedStack |> Ok
            | nameof sortableSetVersion.RandomPermutation -> 
                sortableSetVersion.RandomPermutation |> Ok
            | nameof sortableSetVersion.RandomBits -> 
                sortableSetVersion.RandomBits |> Ok
            | nameof sortableSetVersion.RandomSymbols -> 
                sortableSetVersion.RandomSymbols |> Ok
            | _ -> sprintf "%s not handled" strRep |> Error

    let makeGeneratedSortableSetR 
            (causeR:CauseR) 
            (causePath:string) 
            (version:string) 
            (json:string) =
        let sortableSetR = new SortableSetR();
        sortableSetR.CauseR <- causeR
        sortableSetR.CausePath <- causePath
        sortableSetR.Json <- json
        sortableSetR.Version <- version
        sortableSetR


    let sortableSetRToSortableSet (ctxt:IGortContext2) (sortableSetR:SortableSetR)  =
        let _rngGenLookup (rndGenId:int) =
            result {
                let! rndGenR = DbLookup.GetRandGenRById ctxt rndGenId
                return! randGenRToRngGen rndGenR
            }

        result {
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
                                                             _rngGenLookup
            | RandomBits -> 
                    SortableSetRandomBitsDto.fromJson sortableSetR.Json 
                                                      _rngGenLookup
            | RandomSymbols -> 
                    SortableSetRandomSymbolsDto.fromJson sortableSetR.Json 
                                                         _rngGenLookup
        }
