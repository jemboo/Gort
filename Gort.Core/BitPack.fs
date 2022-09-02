namespace global
open System

type bitPack = private { bitsPerSymbol:bitsPerSymbol; symbolCount:symbolCount; data:byte[] }
module BitPack =

    let getBitsPerSymbol (bitPack:bitPack) =
        bitPack.bitsPerSymbol

    let getSymbolCount (bitPack:bitPack) =
        bitPack.symbolCount

    let getData (bitPack:bitPack) =
        bitPack.data

    let create (bitsPerSymbol:bitsPerSymbol) (symbolCount:symbolCount) (data:byte[]) =
        { bitPack.bitsPerSymbol = bitsPerSymbol; symbolCount = symbolCount; data = data }

    let toIntArrays (arrayLength:arrayLength) (bitPack:bitPack) =
        result {
            let bitsPerSymbol = bitPack |> getBitsPerSymbol
            let allInts = bitPack |> getData
                                  |> ByteUtils.getAllBitsFromByteSeq
                                  |> ByteUtils.bitsToSpIntPositions bitsPerSymbol
                                  |> Seq.toArray
            return allInts |> Array.chunkBySize (arrayLength |> ArrayLength.value)
        }
        

    let fromIntArrays (symbolSetSize:symbolSetSize) 
                      (intArrays:seq<int[]>) =
        result {
            let! bitsPerSymbol = symbolSetSize |> BitsPerSymbol.fromSymbolSetSize
            let byteSeq, bitCt = 
                intArrays
                |> Seq.concat
                |> ByteUtils.bitsFromSpIntPositions bitsPerSymbol
                |> ByteUtils.storeBitSeqInBytes
            let data = byteSeq |> Seq.toArray
            let! symbolCount = bitCt / (BitsPerSymbol.value bitsPerSymbol)
                              |> SymbolCount.create
            return { bitPack.bitsPerSymbol = bitsPerSymbol; symbolCount = symbolCount; data = data }
        }

        
    let toBoolArrays (arrayLength:arrayLength) (bitPack:bitPack) =
        result {
            let arrayLv = (arrayLength |> ArrayLength.value)
            return bitPack 
                    |> getData
                    |> ByteUtils.getAllBitsFromByteSeq
                    |> Seq.chunkBySize arrayLv
                    |> Seq.filter(fun ba -> ba.Length = arrayLv)
                    |> Seq.toArray
        }

        
    let fromBoolArrays (boolArrays:seq<bool[]>) =
        result {
            let! bitsPerSymbol = 1 |> BitsPerSymbol.create
            let byteSeq, bitCt = 
                boolArrays
                |> Seq.concat
                |> ByteUtils.storeBitSeqInBytes
            let data = byteSeq |> Seq.toArray
            let! symbolCount = bitCt |> SymbolCount.create
            return { bitPack.bitsPerSymbol = bitsPerSymbol; symbolCount = symbolCount; data = data }
        } 