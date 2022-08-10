namespace global
open Microsoft.FSharp.Core
open Gort.DataStore.DataModel


module DomainTables =
        
    let rngGenToRandGenRecord (rg:rngGen) 
                              (causeId:int) 
                              (causePath:string) =
        let cereal = rg |> RngGenDto.toDto |> Json.serialize
        let randGen = new RandGen()
        randGen.CauseId <- causeId
        randGen.CausePath <- causePath
        randGen.Version <- Versions.RandGen_rndGen
        randGen.Cereal <- cereal
        randGen


    let randGenRecordToRngGen (randGen:RandGen) =
        result {

            if (randGen.Version <>  Versions.RandGen_rndGen) then
                let! res = (sprintf "randGen.Version is %s not %s" 
                               randGen.Version
                               Versions.RandGen_rndGen) 
                           |> Error
                return! res
            else
                let! dto = randGen.Cereal |> Json.deserialize<rngGenDto>
                let! rg = ( dto |> RngGenDto.fromDto)
                return rg
        }

    let bitPackToBitPackRecord (bitPack:bitPack) =
        let bitPackRecord = new BitPackRecord();
        let dd = bitPack |> BitPack.getData
        bitPackRecord.BitsPerSymbol <- bitPack |> BitPack.getBitWidth 
                                               |> BitsPerSymbol.value
        bitPackRecord.SymbolCount <- bitPack |> BitPack.getSymbolCount 
                                             |> SymbolCount.value
        bitPackRecord.Data <- bitPack |> BitPack.getData
        bitPackRecord


    let bitPackRecordToBitPack (bitPackR:BitPackRecord) =
        result {
            let! bitsPerSymbol = bitPackR.BitsPerSymbol |> BitsPerSymbol.create
            let! symbolCount = bitPackR.SymbolCount |> SymbolCount.create
            let data = bitPackR.Data
            let bitPack = BitPack.create bitsPerSymbol symbolCount data
            return bitPack
        }
