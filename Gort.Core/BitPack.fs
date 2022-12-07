namespace global

open System

type bitPack =
    private
        { bitsPerSymbol: bitsPerSymbol
          symbolCount: symbolCount
          offset:int
          data: byte[] }

module BitPack =

    let getBitsPerSymbol (bitPack: bitPack) = bitPack.bitsPerSymbol

    let getSymbolCount (bitPack: bitPack) = bitPack.symbolCount
    
    let getOffset (bitPack: bitPack) = bitPack.offset

    let getData (bitPack: bitPack) = 
        bitPack.data |> Seq.skip bitPack.offset

    let create (bitsPerSymbol: bitsPerSymbol) 
               (symbolCount: symbolCount)
               (offst:int)
               (data: byte[]) =
        { bitPack.bitsPerSymbol = bitsPerSymbol
          symbolCount = symbolCount;
          offset = offst;
          data = data }

    let changeOffset (bitPck:bitPack) (newOffset:int) =
        {bitPck with offset = newOffset }

    let fromBytes (bitsPerSymbol: bitsPerSymbol) 
                  (data: byte[]) =
        let bps = (bitsPerSymbol |> BitsPerSymbol.value)
        let bitCt = (data.Length * 8)
        let skud = bitCt % bps
        let symbolCt = ((bitCt - skud) / bps) |> SymbolCount.createNr
        create bitsPerSymbol symbolCt 0 data


    let toInts (bitPack: bitPack) =
        bitPack
        |> getData
        |> ByteUtils.getAllBitsFromByteSeq
        |> ByteUtils.bitsToSpIntPositions 
                    (bitPack |> getBitsPerSymbol)
                    (bitPack |> getSymbolCount)
        

    let toIntArrays (arrayLength: arrayLength) (bitPack: bitPack) =
        let arrayLen = (arrayLength |> ArrayLength.value)
        toInts bitPack 
        |> Seq.chunkBySize arrayLen
        |> Seq.filter (fun ba -> ba.Length = arrayLen)


    let fromInts (bitsPerSymbl: bitsPerSymbol) (ints: seq<int>) =
        let byteSeq, bitCt =
            ints
            |> ByteUtils.bitsFromSpIntPositions bitsPerSymbl
            |> ByteUtils.storeBitSeqInBytes

        let data = byteSeq |> Seq.toArray
        let symbolCt = bitCt / (BitsPerSymbol.value bitsPerSymbl) 
                       |> SymbolCount.createNr
        create bitsPerSymbl symbolCt 0 data


    let fromIntArrays (bitsPerSymbl: bitsPerSymbol) 
                      (intArrays: seq<int[]>) =
        fromInts bitsPerSymbl (intArrays |> Seq.concat)


    let toBoolArray (arrayLength: arrayLength) (bitPack: bitPack) =
        let arrayLen = (arrayLength |> ArrayLength.value)
        bitPack
        |> getData
        |> ByteUtils.getAllBitsFromByteSeq
        |> Seq.chunkBySize arrayLen
        |> Seq.filter (fun ba -> ba.Length = arrayLen)


    let toBoolArrays (arrayLength: arrayLength) (bitPack: bitPack) =
        let arrayLv = (arrayLength |> ArrayLength.value)
        bitPack
        |> getData
        |> ByteUtils.getAllBitsFromByteSeq
        |> Seq.chunkBySize arrayLv
        |> Seq.filter (fun ba -> ba.Length = arrayLv)


    let fromBools (boolArrays: seq<bool>) =
        let bitsPerSymbl = 1 |> BitsPerSymbol.createNr
        let byteSeq, bitCt = boolArrays |> ByteUtils.storeBitSeqInBytes
        let data = byteSeq |> Seq.toArray
        let symbolCt = bitCt |> SymbolCount.createNr
        create bitsPerSymbl symbolCt 0 data


    let fromBoolArrays (boolArrays: seq<bool[]>) =
        fromBools (boolArrays |> Seq.concat)



