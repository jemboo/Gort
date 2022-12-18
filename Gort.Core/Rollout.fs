namespace global

open System

type booleanRoll =
    private
        { arrayCount: arrayCount
          arrayLength: arrayLength
          data: bool[] }

module BooleanRoll =

    let create (arrayCount: arrayCount) (arrayLength: arrayLength) =

        let dataLength =
            (arrayCount |> ArrayCount.value) * (arrayLength |> ArrayLength.value)

        { arrayCount = arrayCount
          arrayLength = arrayLength
          data = Array.zeroCreate<bool> dataLength }

    let createEmpty =
        { arrayCount = (0 |> ArrayCount.createNr)
          arrayLength = (0 |> ArrayLength.createNr)
          data = Array.zeroCreate<bool> 0 }

    let getArrayCount (booleanRoll: booleanRoll) = booleanRoll.arrayCount

    let getArrayLength (booleanRoll: booleanRoll) = booleanRoll.arrayLength

    let getData (booleanRoll: booleanRoll) = booleanRoll.data

    let copy (booleanRoll: booleanRoll) =
        { booleanRoll with data = booleanRoll.data |> Array.copy }

    let fromBitPack (arrayLength: arrayLength) (bitPack: bitPack) =
        result {
            let! arrayCount =
                ((bitPack.symbolCount |> SymbolCount.value) / (arrayLength |> ArrayLength.value))
                |> ArrayCount.create

            let data =
                bitPack |> BitPack.getData |> ByteUtils.getAllBitsFromByteSeq |> Seq.toArray

            return
                { booleanRoll.arrayCount = arrayCount
                  arrayLength = arrayLength
                  data = data }
        }

    let toBitPack (booleanRoll: booleanRoll) =
        result {
            let! bitsPerSymbl = 1 |> BitsPerSymbol.create

            let! symbolCt =
                ((booleanRoll.arrayCount |> ArrayCount.value)
                 * (booleanRoll.arrayLength |> ArrayLength.value))
                |> SymbolCount.create

            let byteSeq, bitCt = booleanRoll.data |> ByteUtils.storeBitSeqInBytes
            let data = byteSeq |> Seq.toArray

            return BitPack.create bitsPerSymbl symbolCt 0 data
        }


    let fromBoolArrays (arrayLength: arrayLength) (boolAs: seq<bool[]>) =
        result {
            let bools = boolAs |> Seq.concat |> Seq.toArray

            let arrayCount =
                (bools.Length / (ArrayLength.value arrayLength)) |> ArrayCount.createNr

            return
                { booleanRoll.arrayCount = arrayCount
                  arrayLength = arrayLength
                  data = bools }
        }


    let fromIntArrays (arrayLength: arrayLength) (aas: seq<int[]>) =
        result {
            let boolA =
                aas
                |> Seq.concat
                |> Seq.map (fun bv -> if (bv > 0) then true else false)
                |> Seq.toArray

            let arrayCount =
                (boolA.Length / (ArrayLength.value arrayLength)) |> ArrayCount.createNr

            return
                { booleanRoll.arrayCount = arrayCount
                  arrayLength = arrayLength
                  data = boolA }
        }


    let toIntArrays (booleanRoll: booleanRoll) =
        booleanRoll.data
        |> Seq.map (fun bv -> if bv then 1 else 0)
        |> Seq.chunkBySize (booleanRoll.arrayLength |> ArrayLength.value)


    let toBoolArrays (booleanRoll: booleanRoll) =
        booleanRoll.data
        |> Seq.chunkBySize (booleanRoll.arrayLength |> ArrayLength.value)


    let uniqueMembers (booleanRoll: booleanRoll) =
        booleanRoll
        |> toBoolArrays
        |> Seq.distinct
        |> fromBoolArrays (booleanRoll |> getArrayLength)


    let uniqueUnsortedMembers (booleanRoll: booleanRoll) =
        booleanRoll
        |> toBoolArrays
        |> Seq.filter (fun ia -> not (CollectionProps.isSorted ia))
        |> Seq.distinct
        |> fromBoolArrays (booleanRoll |> getArrayLength)


    let isSorted (booleanRoll: booleanRoll) =
        let mutable i = 0
        let iIncr = booleanRoll.arrayLength |> ArrayLength.value
        let iMax = booleanRoll.data.Length
        let mutable looP = true

        while ((i < iMax) && looP) do
            looP <- CollectionProps.isSortedOffset booleanRoll.data i iIncr
            i <- i + iIncr
        looP




type uInt8Roll =
    private
        { arrayCount: arrayCount
          arrayLength: arrayLength
          data: uint8[] }

module Uint8Roll =

    let getArrayCount (uInt8Roll: uInt8Roll) = uInt8Roll.arrayCount

    let getArrayLength (uInt8Roll: uInt8Roll) = uInt8Roll.arrayLength

    let getData (uInt8Roll: uInt8Roll) = uInt8Roll.data

    let copy (uInt8Roll: uInt8Roll) =
        { arrayCount = uInt8Roll.arrayCount
          arrayLength = uInt8Roll.arrayLength
          data = uInt8Roll.data |> Array.copy }


    let fromBitPack (arrayLength: arrayLength) (bitPack: bitPack) =
        result {
            let! arrayCount =
                ((bitPack.symbolCount |> SymbolCount.value) / (arrayLength |> ArrayLength.value))
                |> ArrayCount.create
            let data =
                bitPack
                |> BitPack.getData
                |> ByteUtils.getAllBitsFromByteSeq
                |> ByteUtils.bitsToSpBytePositions 
                                (bitPack |> BitPack.getBitsPerSymbol)
                                (bitPack |> BitPack.getSymbolCount)
                |> Seq.toArray

            return
                { uInt8Roll.arrayCount = arrayCount
                  arrayLength = arrayLength
                  data = data }
        }


    let toBitPack (symbolSetSize: symbolSetSize) (uInt8Roll: uInt8Roll) =
        result {
            let bitsPerSymbl = symbolSetSize |> BitsPerSymbol.fromSymbolSetSize
            let! symbolCt =
                ((uInt8Roll.arrayCount |> ArrayCount.value)
                 * (uInt8Roll.arrayLength |> ArrayLength.value))
                |> SymbolCount.create

            let byteSeq, bitCt =
                uInt8Roll.data
                |> ByteUtils.bitsFromSpBytePositions bitsPerSymbl
                |> ByteUtils.storeBitSeqInBytes
            let data = byteSeq |> Seq.toArray
            return BitPack.create bitsPerSymbl symbolCt 0 data
        }


    let fromBoolArrays (arrayLength: arrayLength) (aas: seq<bool[]>) =
        result {
            let uint8s =
                aas |> Seq.concat |> Seq.map (fun tf -> if tf then 1uy else 0uy) |> Seq.toArray

            let arrayCount =
                (uint8s.Length / (ArrayLength.value arrayLength)) |> ArrayCount.createNr

            return
                { uInt8Roll.arrayCount = arrayCount
                  arrayLength = arrayLength
                  data = uint8s }
        }


    let fromIntArrays (arrayLength: arrayLength) (aas: seq<int[]>) =
        result {
            let uint8s = aas |> Seq.concat |> Seq.map (uint8) |> Seq.toArray

            let arrayCount =
                (uint8s.Length / (ArrayLength.value arrayLength)) |> ArrayCount.createNr

            let! bigA = uint8s |> ByteArray.convertUint8sToBytes

            return
                { uInt8Roll.arrayCount = arrayCount
                  arrayLength = arrayLength
                  data = bigA }
        }


    let toBoolArrays (uInt8Roll: uInt8Roll) =
        uInt8Roll.data
        |> Seq.map (fun bv -> bv > 0uy)
        |> Seq.chunkBySize (uInt8Roll.arrayLength |> ArrayLength.value)


    let toIntArrays (uInt8Roll: uInt8Roll) =
        uInt8Roll.data
        |> Seq.map (int)
        |> Seq.chunkBySize (uInt8Roll.arrayLength |> ArrayLength.value)


    let uniqueMembers (uInt8Roll: uInt8Roll) =
        uInt8Roll
        |> toIntArrays
        |> Seq.distinct
        |> fromIntArrays (uInt8Roll |> getArrayLength)


    let uniqueUnsortedMembers (uInt8Roll: uInt8Roll) =
        uInt8Roll
        |> toIntArrays
        |> Seq.filter (fun ia -> not (CollectionProps.isSorted ia))
        |> Seq.distinct
        |> fromIntArrays (uInt8Roll |> getArrayLength)


    let isSorted (uInt8Roll: uInt8Roll) =
        let mutable i = 0
        let iIncr = uInt8Roll.arrayLength |> ArrayLength.value
        let iMax = uInt8Roll.data.Length
        let mutable looP = true

        while ((i < iMax) && looP) do
            looP <- CollectionProps.isSortedOffset uInt8Roll.data i iIncr
            i <- i + iIncr

        looP



type uInt16Roll =
    private
        { arrayCount: arrayCount
          arrayLength: arrayLength
          data: uint16[] }

module Uint16Roll =

    let getArrayCount (uInt16Roll: uInt16Roll) = uInt16Roll.arrayCount

    let getArrayLength (uInt16Roll: uInt16Roll) = uInt16Roll.arrayLength

    let getData (uInt16Roll: uInt16Roll) = uInt16Roll.data

    let copy (uInt16Roll: uInt16Roll) =
        { arrayCount = uInt16Roll.arrayCount
          arrayLength = uInt16Roll.arrayLength
          data = uInt16Roll.data |> Array.copy }

    let fromBitPack (arrayLength: arrayLength) (bitPack: bitPack) =
        result {
            let! arrayCount =
                ((bitPack.symbolCount |> SymbolCount.value) / (arrayLength |> ArrayLength.value))
                |> ArrayCount.create

            let data =
                bitPack
                |> BitPack.getData
                |> ByteUtils.getAllBitsFromByteSeq
                |> ByteUtils.bitsToSpUint16Positions 
                                (bitPack |> BitPack.getBitsPerSymbol)
                                (bitPack |> BitPack.getSymbolCount)
                |> Seq.toArray

            return
                { uInt16Roll.arrayCount = arrayCount
                  arrayLength = arrayLength
                  data = data }
        }


    let toBitPack (symbolSetSize: symbolSetSize) (uInt16Roll: uInt16Roll) =
        result {
            let bitsPerSymbl = symbolSetSize |> BitsPerSymbol.fromSymbolSetSize
            let! symbolCt =
                ((uInt16Roll.arrayCount |> ArrayCount.value)
                 * (uInt16Roll.arrayLength |> ArrayLength.value))
                |> SymbolCount.create

            let byteSeq, bitCt =
                uInt16Roll.data
                |> ByteUtils.bitsFromSpUint16Positions bitsPerSymbl
                |> ByteUtils.storeBitSeqInBytes
            let data = byteSeq |> Seq.toArray
            
            return BitPack.create bitsPerSymbl symbolCt 0 data
        }


    let fromBoolArrays (arrayLength: arrayLength) (aas: seq<bool[]>) =
        result {
            let uint16s =
                aas |> Seq.concat |> Seq.map (fun tf -> if tf then 1us else 0us) |> Seq.toArray

            let arrayCount =
                (uint16s.Length / (ArrayLength.value arrayLength)) |> ArrayCount.createNr

            return
                { uInt16Roll.arrayCount = arrayCount
                  arrayLength = arrayLength
                  data = uint16s }
        }


    let fromIntArrays (arrayLength: arrayLength) (aas: seq<int[]>) =
        result {
            let uint16s = aas |> Seq.concat |> Seq.map (uint16) |> Seq.toArray

            let arrayCount =
                (uint16s.Length / (ArrayLength.value arrayLength)) |> ArrayCount.createNr

            return
                { uInt16Roll.arrayCount = arrayCount
                  arrayLength = arrayLength
                  data = uint16s }
        }


    let toBoolArrays (uInt16Roll: uInt16Roll) =
        uInt16Roll.data
        |> Seq.map (fun bv -> bv > 0us)
        |> Seq.chunkBySize (uInt16Roll.arrayLength |> ArrayLength.value)


    let toIntArrays (uInt16Roll: uInt16Roll) =
        uInt16Roll.data
        |> Seq.map (int)
        |> Seq.chunkBySize (uInt16Roll.arrayLength |> ArrayLength.value)


    let uniqueMembers (uInt16Roll: uInt16Roll) =
        uInt16Roll
        |> toIntArrays
        |> Seq.distinct
        |> fromIntArrays (uInt16Roll |> getArrayLength)


    let uniqueUnsortedMembers (uInt16Roll: uInt16Roll) =
        uInt16Roll
        |> toIntArrays
        |> Seq.filter (fun ia -> not (CollectionProps.isSorted ia))
        |> Seq.distinct
        |> fromIntArrays (uInt16Roll |> getArrayLength)


    let isSorted (uInt16Roll: uInt16Roll) =
        let mutable i = 0
        let iIncr = uInt16Roll.arrayLength |> ArrayLength.value
        let iMax = uInt16Roll.data.Length
        let mutable looP = true

        while ((i < iMax) && looP) do
            looP <- CollectionProps.isSortedOffset uInt16Roll.data i iIncr
            i <- i + iIncr

        looP



type intRoll =
    private
        { arrayCount: arrayCount
          arrayLength: arrayLength
          data: int[] }

module IntRoll =

    let copy (intRoll: intRoll) =
        { arrayCount = intRoll.arrayCount
          arrayLength = intRoll.arrayLength
          data = intRoll.data |> Array.copy }

    let updateData (data: int[]) (intRoll: intRoll) = { intRoll with data = data }

    let getArrayCount (uInt8Roll: intRoll) = uInt8Roll.arrayCount

    let getArrayLength (intRoll: intRoll) = intRoll.arrayLength

    let getRolloutLength (intRoll: intRoll) =
        (intRoll.arrayCount |> ArrayCount.value)
        * (intRoll.arrayLength |> ArrayLength.value)

    let getData (uInt8Roll: intRoll) = uInt8Roll.data

    let fromBitPack (arrayLength: arrayLength) (bitPack: bitPack) =
        result {
            let! arrayCount =
                ((bitPack.symbolCount |> SymbolCount.value) / (arrayLength |> ArrayLength.value))
                |> ArrayCount.create
            let data =
                bitPack
                |> BitPack.getData
                |> ByteUtils.getAllBitsFromByteSeq
                |> ByteUtils.bitsToSpIntPositions 
                                (bitPack |> BitPack.getBitsPerSymbol)
                                (bitPack |> BitPack.getSymbolCount)
                |> Seq.toArray

            return
                { intRoll.arrayCount = arrayCount
                  arrayLength = arrayLength
                  data = data }
        }

    let toBitPack (symbolSetSize: symbolSetSize) (intRoll: intRoll) =
        result {
            let bitsPerSymbl = symbolSetSize |> BitsPerSymbol.fromSymbolSetSize

            let! symbolCt =
                ((intRoll.arrayCount |> ArrayCount.value)
                 * (intRoll.arrayLength |> ArrayLength.value))
                |> SymbolCount.create

            let byteSeq, bitCt =
                intRoll.data
                |> ByteUtils.bitsFromSpIntPositions bitsPerSymbl
                |> ByteUtils.storeBitSeqInBytes

            let data = byteSeq |> Seq.toArray
            return BitPack.create bitsPerSymbl symbolCt 0 data
        }

    let fromBoolArrays (arrayLength: arrayLength) (aas: seq<bool[]>) =
        result {
            let uint8s =
                aas |> Seq.concat |> Seq.map (fun tf -> if tf then 1 else 0) |> Seq.toArray
            let arrayCount =
                (uint8s.Length / (ArrayLength.value arrayLength)) |> ArrayCount.createNr
            return
                { intRoll.arrayCount = arrayCount
                  arrayLength = arrayLength
                  data = uint8s }
        }

    let fromIntArrays (arrayLength: arrayLength) (aas: seq<int[]>) =
        result {
            let intA = aas |> Seq.concat |> Seq.toArray
            let arrayCount =
                (intA.Length / (ArrayLength.value arrayLength)) |> ArrayCount.createNr
            return
                { intRoll.arrayCount = arrayCount
                  arrayLength = arrayLength
                  data = intA }
        }


    let toBoolArrays (intRoll: intRoll) =
        intRoll.data
        |> Seq.map (fun bv -> bv > 0)
        |> Seq.chunkBySize (intRoll.arrayLength |> ArrayLength.value)


    let toIntArrays (intRoll: intRoll) =
        intRoll.data |> Seq.chunkBySize (intRoll.arrayLength |> ArrayLength.value)


    let uniqueMembers (intRoll: intRoll) =
        intRoll
        |> toIntArrays
        |> Seq.distinct
        |> fromIntArrays (intRoll |> getArrayLength)


    let uniqueUnsortedMembers (intRoll: intRoll) =
        intRoll
        |> toIntArrays
        |> Seq.filter (fun ia -> not (CollectionProps.isSorted ia))
        |> Seq.distinct
        |> fromIntArrays (intRoll |> getArrayLength)


    let isSorted (intRoll: intRoll) =
        let mutable i = 0
        let iIncr = intRoll.arrayLength |> ArrayLength.value
        let iMax = intRoll.data.Length
        let mutable looP = true

        while ((i < iMax) && looP) do
            looP <- CollectionProps.isSortedOffset intRoll.data i iIncr
            i <- i + iIncr

        looP



type uint64Roll =
    private
        { arrayCount: arrayCount
          arrayLength: arrayLength
          data: uint64[] }

module Uint64Roll =

    let getArrayCount (uint64Roll: uint64Roll) = uint64Roll.arrayCount

    let getArrayLength (uint64Roll: uint64Roll) = uint64Roll.arrayLength

    let getData (uint64Roll: uint64Roll) = uint64Roll.data

    let stripeBlocksNeededForArrayCount (arrayCount: arrayCount) =
        ((ArrayCount.value arrayCount) + 63) / 64

    let copy (uint64Roll: uint64Roll) =
        { arrayCount = uint64Roll.arrayCount
          arrayLength = uint64Roll.arrayLength
          data = uint64Roll.data |> Array.copy }

    let fromBitPack (arrayLength: arrayLength) (bitPack: bitPack) =
        result {
            let! arrayCount =
                ((bitPack.symbolCount |> SymbolCount.value) / (arrayLength |> ArrayLength.value))
                |> ArrayCount.create

            let data =
                bitPack
                |> BitPack.getData
                |> ByteUtils.getAllBitsFromByteSeq
                |> ByteUtils.bitsToSpUint64Positions 
                                    ( bitPack |> BitPack.getBitsPerSymbol)
                                    ( bitPack |> BitPack.getSymbolCount)
                |> Seq.toArray

            return
                { uint64Roll.arrayCount = arrayCount
                  arrayLength = arrayLength
                  data = data }
        }


    let toBitPack (symbolSetSize: symbolSetSize) 
                  (uint64Roll: uint64Roll) =
        result {
            let bitsPerSymbl = symbolSetSize |> BitsPerSymbol.fromSymbolSetSize

            let! symbolCt =
                uint64Roll.arrayCount
                |> ArrayCount.value
                |> (*) (uint64Roll.arrayLength |> ArrayLength.value)
                |> SymbolCount.create

            let byteSeq, bitCt =
                uint64Roll.data
                |> ByteUtils.bitsFromSpUint64Positions bitsPerSymbl
                |> ByteUtils.storeBitSeqInBytes

            let data = byteSeq |> Seq.toArray
            return BitPack.create bitsPerSymbl symbolCt 0 data
        }


    let fromBoolArrays (arrayLength: arrayLength) (aas: seq<bool[]>) =
        result {
            let uint8s =
                aas |> Seq.concat |> Seq.map (fun tf -> if tf then 1uL else 0uL) |> Seq.toArray
            let arrayCount =
                (uint8s.Length / (ArrayLength.value arrayLength)) |> ArrayCount.createNr
            return
                { uint64Roll.arrayCount = arrayCount
                  arrayLength = arrayLength
                  data = uint8s }
        }


    let toBoolArrays (uint64Roll: uint64Roll) =
        uint64Roll.data
        |> Seq.map (fun bv -> bv > 0uL)
        |> Seq.chunkBySize (uint64Roll.arrayLength |> ArrayLength.value)


    let fromIntArrays (arrayLength: arrayLength) (aas: seq<int[]>) =
        result {
            let uint64s = aas |> Seq.concat |> Seq.map (uint64) |> Seq.toArray
            let arrayCount =
                (uint64s.Length / (ArrayLength.value arrayLength)) |> ArrayCount.createNr
            return
                { uint64Roll.arrayCount = arrayCount
                  arrayLength = arrayLength
                  data = uint64s }
        }


    let toIntArrays (uint64Roll: uint64Roll) =
        uint64Roll.data
        |> Seq.map (int)
        |> Seq.chunkBySize (uint64Roll.arrayLength |> ArrayLength.value)


    let toUint64Arrays (uint64Roll: uint64Roll) =
        uint64Roll.data |> Seq.chunkBySize (uint64Roll.arrayLength |> ArrayLength.value)


    let fromUint64Arrays (arrayLength: arrayLength) (aas: seq<uint64[]>) =
        result {
            let uint64s = aas |> Seq.concat |> Seq.toArray
            let arrayCount =
                (uint64s.Length / (ArrayLength.value arrayLength)) |> ArrayCount.createNr
            return
                { uint64Roll.arrayCount = arrayCount
                  arrayLength = arrayLength
                  data = uint64s }
        }


    let uniqueMembers (uint64Roll: uint64Roll) =
        uint64Roll
        |> toIntArrays
        |> Seq.distinct
        |> fromIntArrays (uint64Roll |> getArrayLength)


    let uniqueUnsortedMembers (uint64Roll: uint64Roll) =
        uint64Roll
        |> toIntArrays
        |> Seq.filter (fun ia -> not (CollectionProps.isSorted ia))
        |> Seq.distinct
        |> fromIntArrays (uint64Roll |> getArrayLength)


    let isSorted (uint64Roll: uint64Roll) =
        let mutable i = 0
        let iIncr = uint64Roll.arrayLength |> ArrayLength.value
        let iMax = uint64Roll.data.Length
        let mutable looP = true
        while ((i < iMax) && looP) do
            looP <- CollectionProps.isSortedOffset uint64Roll.data i iIncr
            i <- i + iIncr

        looP


type bs64Roll =
    private
        { arrayCount: arrayCount
          arrayLength: arrayLength
          data: uint64[] }

module Bs64Roll =

    let getArrayCount (bs64Roll: bs64Roll) = bs64Roll.arrayCount

    let getArrayLength (bs64Roll: bs64Roll) = bs64Roll.arrayLength

    let getData (bs64Roll: bs64Roll) = bs64Roll.data

    let getDataArrayLength (bs64Roll: bs64Roll) = bs64Roll.data.Length

    let stripeBlocksNeededForArrayCount (arrayCount: arrayCount) =
        ((ArrayCount.value arrayCount) + 63) / 64

    let createEmptyStripedSet (arrayLength: arrayLength) (arrayCount: arrayCount) =
        let blocksNeeded = stripeBlocksNeededForArrayCount arrayCount
        let dataLength = (ArrayLength.value arrayLength) * blocksNeeded
        let data = Array.zeroCreate<uint64> dataLength

        { arrayCount = arrayCount
          arrayLength = arrayLength
          data = data }


    let copy (bs64Roll: bs64Roll) =
        { bs64Roll with data = bs64Roll.data |> Array.copy }


    let getUsedStripes (bs64Roll: bs64Roll) =
        let len = bs64Roll.data.Length
        let arrayLen = bs64Roll.arrayLength |> ArrayLength.value
        let q = bs64Roll.data.[len - arrayLen .. len - 1]
        let lastStripes = q |> ByteUtils.usedStripeCount
        let ww = (len - arrayLen) / arrayLen
        (ww * 64) + lastStripes


    let fromBoolArrays (arrayLength: arrayLength) (aas: seq<bool[]>) =
        result {
            let! order = arrayLength |> ArrayLength.value |> Order.create
            let! data, arrayCountV = ByteUtils.makeStripedArraysFromBoolArrays order aas
            let! arrayCount = arrayCountV |> ArrayCount.create

            return
                { bs64Roll.arrayCount = arrayCount
                  arrayLength = arrayLength
                  data = data }
        }


    let fromIntArrays (arrayLength: arrayLength) (aas: seq<int[]>) =
        let _aConv (ia: int[]) =
            Array.init ia.Length (fun dex -> ia[dex] = 1)

        fromBoolArrays arrayLength (aas |> Seq.map (_aConv))


    let fromBitPack (arrayLength: arrayLength) (bitPack: bitPack) =
        result {
            let bitsPerSymbol = bitPack |> BitPack.getBitsPerSymbol

            if (bitsPerSymbol |> BitsPerSymbol.value) <> 1 then
                return! sprintf "bitsPerSymbol must be 1" |> Error
            else
                return!
                    bitPack
                    |> BitPack.getData
                    |> ByteUtils.getAllBitsFromByteSeq
                    |> Seq.take (bitPack.symbolCount |> SymbolCount.value)
                    |> Seq.chunkBySize (arrayLength |> ArrayLength.value)
                    |> fromBoolArrays arrayLength
        }


    let toBitPack (bs64Roll: bs64Roll) =
        result {
            let! order = bs64Roll.arrayLength |> ArrayLength.value |> Order.create
            let! bitsPerSymbl = 1 |> BitsPerSymbol.create

            let! symbolCt =
                bs64Roll.arrayCount
                |> ArrayCount.value
                |> (*) (bs64Roll.arrayLength |> ArrayLength.value)
                |> SymbolCount.create

            let data, bitCt =
                ByteUtils.fromStripeArrays order bs64Roll.data
                |> Seq.take (bs64Roll.arrayCount |> ArrayCount.value)
                |> Seq.concat
                |> ByteUtils.storeBitSeqInBytes
            
            return BitPack.create bitsPerSymbl symbolCt 0 data
        }


    let toBoolArrays (bs64Roll: bs64Roll) =
        let order = bs64Roll.arrayLength |> ArrayLength.value |> Order.createNr

        ByteUtils.fromStripeArrays order bs64Roll.data
        |> Seq.take (bs64Roll |> getArrayCount |> ArrayCount.value)


    let uniqueMembers (bs64Roll: bs64Roll) =
        bs64Roll
        |> toBoolArrays
        |> Seq.distinct
        |> fromBoolArrays (bs64Roll |> getArrayLength)


    let uniqueUnsortedMembers (bs64Roll: bs64Roll) =
        bs64Roll
        |> toBoolArrays
        |> Seq.filter (fun ia -> not (CollectionProps.isSorted ia))
        |> Seq.distinct
        |> fromBoolArrays (bs64Roll |> getArrayLength)


    let isSorted (bs64Roll: bs64Roll) =
        bs64Roll
        |> toBoolArrays
        |> Seq.forall (fun ia -> (CollectionProps.isSorted ia))


    let toIntArrays (bs64Roll: bs64Roll) =
        let _aConv (ba: bool[]) =
            Array.init ba.Length (fun dex -> if ba[dex] then 1 else 0)

        let order = bs64Roll.arrayLength |> ArrayLength.value |> Order.createNr

        ByteUtils.fromStripeArrays order bs64Roll.data
        |> Seq.map (_aConv)
        |> Seq.take (bs64Roll |> getArrayCount |> ArrayCount.value)



type rolloutFormat =
    | RfB
    | RfU8
    | RfU16
    | RfI32
    | RfU64
    | RfBs64

module RolloutFormat =

    let toString (rf: rolloutFormat) =
        match rf with
        | rolloutFormat.RfB -> nameof rolloutFormat.RfB
        | rolloutFormat.RfU8 -> nameof rolloutFormat.RfU8
        | rolloutFormat.RfU16 -> nameof rolloutFormat.RfU16
        | rolloutFormat.RfI32 -> nameof rolloutFormat.RfI32
        | rolloutFormat.RfU64 -> nameof rolloutFormat.RfU64
        | rolloutFormat.RfBs64 -> nameof rolloutFormat.RfBs64


    let fromString str =
        match str with
        | nameof rolloutFormat.RfB -> rolloutFormat.RfB |> Ok
        | nameof rolloutFormat.RfU8 -> rolloutFormat.RfU8 |> Ok
        | nameof rolloutFormat.RfU16 -> rolloutFormat.RfU16 |> Ok
        | nameof rolloutFormat.RfI32 -> rolloutFormat.RfI32 |> Ok
        | nameof rolloutFormat.RfU64 -> rolloutFormat.RfU64 |> Ok
        | nameof rolloutFormat.RfBs64 -> rolloutFormat.RfBs64 |> Ok
        | _ -> Error(sprintf "no match for rolloutFormat: %s" str)



type rollout =
    | B of booleanRoll
    | U8 of uInt8Roll
    | U16 of uInt16Roll
    | I32 of intRoll
    | U64 of uint64Roll
    | Bs64 of bs64Roll


module Rollout =

    let getArrayLength (rollout: rollout) =
        match rollout with
        | B _uBRoll -> _uBRoll.arrayLength
        | U8 _uInt8Roll -> _uInt8Roll.arrayLength
        | U16 _uInt16Roll -> _uInt16Roll.arrayLength
        | I32 _intRoll -> _intRoll.arrayLength
        | U64 _uInt64Roll -> _uInt64Roll.arrayLength
        | Bs64 _bs64Roll -> _bs64Roll.arrayLength


    let getArrayCount (rollout: rollout) =
        match rollout with
        | B _uBRoll -> _uBRoll.arrayCount
        | U8 _uInt8Roll -> _uInt8Roll.arrayCount
        | U16 _uInt16Roll -> _uInt16Roll.arrayCount
        | I32 _intRoll -> _intRoll.arrayCount
        | U64 _uInt64Roll -> _uInt64Roll.arrayCount
        | Bs64 _bs64Roll -> _bs64Roll.arrayCount


    let copy (rollout: rollout) =
        match rollout with
        | B _uBRoll -> _uBRoll |> BooleanRoll.copy |> B
        | U8 _uInt8Roll -> _uInt8Roll |> Uint8Roll.copy |> U8
        | U16 _uInt16Roll -> _uInt16Roll |> Uint16Roll.copy |> U16
        | I32 _intRoll -> _intRoll |> IntRoll.copy |> I32
        | U64 _uInt64Roll -> _uInt64Roll |> Uint64Roll.copy |> U64
        | Bs64 _bs64Roll -> _bs64Roll |> Bs64Roll.copy |> Bs64


    let getRolloutLength (rollout: rollout) =
        (rollout |> getArrayCount |> ArrayCount.value)
        * (rollout |> getArrayLength |> ArrayLength.value)


    let fromBoolArrays (rolloutFormat: rolloutFormat) (arrayLength: arrayLength) (aas: seq<bool[]>) =
        match rolloutFormat with
        | RfB ->
            result {
                let! roll = BooleanRoll.fromBoolArrays arrayLength aas
                return roll |> rollout.B
            }

        | RfU8 ->
            result {
                let! roll = Uint8Roll.fromBoolArrays arrayLength aas
                return roll |> rollout.U8
            }
        | RfU16 ->
            result {
                let! roll = Uint16Roll.fromBoolArrays arrayLength aas
                return roll |> rollout.U16
            }
        | RfI32 ->
            result {
                let! roll = IntRoll.fromBoolArrays arrayLength aas
                return roll |> rollout.I32
            }
        | RfU64 ->
            result {
                let! roll = Uint64Roll.fromBoolArrays arrayLength aas
                return roll |> rollout.U64
            }
        | RfBs64 ->
            result {
                let! roll = Bs64Roll.fromBoolArrays arrayLength aas
                return roll |> rollout.Bs64
            }


    let fromIntArrays (rolloutFormat: rolloutFormat) (arrayLength: arrayLength) (aas: seq<int[]>) =
        match rolloutFormat with
        | RfB ->
            result {
                let! roll = BooleanRoll.fromIntArrays arrayLength aas
                return roll |> rollout.B
            }
        | RfU8 ->
            result {
                let! roll = Uint8Roll.fromIntArrays arrayLength aas
                return roll |> rollout.U8
            }
        | RfU16 ->
            result {
                let! roll = Uint16Roll.fromIntArrays arrayLength aas
                return roll |> rollout.U16
            }
        | RfI32 ->
            result {
                let! roll = IntRoll.fromIntArrays arrayLength aas
                return roll |> rollout.I32
            }
        | RfU64 ->
            result {
                let! roll = Uint64Roll.fromIntArrays arrayLength aas
                return roll |> rollout.U64
            }
        | RfBs64 ->
            result {
                let! roll = Bs64Roll.fromIntArrays arrayLength aas
                return roll |> rollout.Bs64
            }


    let createEmpty =
        BooleanRoll.createEmpty |> rollout.B


    let isSorted (rollout: rollout) =
        match rollout with
        | B _uBRoll -> _uBRoll |> BooleanRoll.isSorted
        | U8 _uInt8Roll -> _uInt8Roll |> Uint8Roll.isSorted
        | U16 _uInt16Roll -> _uInt16Roll |> Uint16Roll.isSorted
        | I32 _intRoll -> _intRoll |> IntRoll.isSorted
        | U64 _uInt64Roll -> _uInt64Roll |> Uint64Roll.isSorted
        | Bs64 _bs64Roll -> _bs64Roll |> Bs64Roll.isSorted


    let toIntArrays (rollout: rollout) =
        match rollout with
        | B _uBRoll -> _uBRoll |> BooleanRoll.toIntArrays
        | U8 _uInt8Roll -> _uInt8Roll |> Uint8Roll.toIntArrays
        | U16 _uInt16Roll -> _uInt16Roll |> Uint16Roll.toIntArrays
        | I32 _intRoll -> _intRoll |> IntRoll.toIntArrays
        | U64 _uInt64Roll -> _uInt64Roll |> Uint64Roll.toIntArrays
        | Bs64 _bs64Roll -> _bs64Roll |> Bs64Roll.toIntArrays


    let toBoolArrays (rollout: rollout) =
        match rollout with
        | B _uBRoll -> _uBRoll |> BooleanRoll.toBoolArrays
        | U8 _uInt8Roll -> _uInt8Roll |> Uint8Roll.toBoolArrays
        | U16 _uInt16Roll -> _uInt16Roll |> Uint16Roll.toBoolArrays
        | I32 _intRoll -> _intRoll |> IntRoll.toBoolArrays
        | U64 _uInt64Roll -> _uInt64Roll |> Uint64Roll.toBoolArrays
        | Bs64 _bs64Roll -> _bs64Roll |> Bs64Roll.toBoolArrays


    let uniqueMembers (rollout: rollout) =
        match rollout with
        | B _uBRoll -> _uBRoll |> BooleanRoll.uniqueMembers |> Result.map B
        | U8 _uInt8Roll -> _uInt8Roll |> Uint8Roll.uniqueMembers |> Result.map U8
        | U16 _uInt16Roll -> _uInt16Roll |> Uint16Roll.uniqueMembers |> Result.map U16
        | I32 _intRoll -> _intRoll |> IntRoll.uniqueMembers |> Result.map I32
        | U64 _uInt64Roll -> _uInt64Roll |> Uint64Roll.uniqueMembers |> Result.map U64
        | Bs64 _bs64Roll -> _bs64Roll |> Bs64Roll.uniqueMembers |> Result.map Bs64


    let uniqueUnsortedMembers (rollout: rollout) =
        match rollout with
        | B _uBRoll -> _uBRoll |> BooleanRoll.uniqueUnsortedMembers |> Result.map B
        | U8 _uInt8Roll -> _uInt8Roll |> Uint8Roll.uniqueUnsortedMembers |> Result.map U8
        | U16 _uInt16Roll -> _uInt16Roll |> Uint16Roll.uniqueUnsortedMembers |> Result.map U16
        | I32 _intRoll -> _intRoll |> IntRoll.uniqueUnsortedMembers |> Result.map I32
        | U64 _uInt64Roll -> _uInt64Roll |> Uint64Roll.uniqueUnsortedMembers |> Result.map U64
        | Bs64 _bs64Roll -> _bs64Roll |> Bs64Roll.uniqueUnsortedMembers |> Result.map Bs64



    let fromUInt64ArraySeq (rolloutFormat: rolloutFormat) (arrayLength: arrayLength) (aas: seq<uint64[]>) =

        let intSeq =
            aas
            |> Seq.concat
            |> Seq.map (int)
            |> Seq.chunkBySize (arrayLength |> ArrayLength.value)

        match rolloutFormat with
        | RfB ->
            result {
                let! roll = BooleanRoll.fromIntArrays arrayLength intSeq
                return roll |> rollout.B
            }
        | RfU8 ->
            result {
                let! roll = Uint8Roll.fromIntArrays arrayLength intSeq
                return roll |> rollout.U8
            }
        | RfU16 ->
            result {
                let! roll = Uint16Roll.fromIntArrays arrayLength intSeq
                return roll |> rollout.U16
            }
        | RfI32 ->
            result {
                let! roll = IntRoll.fromIntArrays arrayLength intSeq
                return roll |> rollout.I32
            }
        | RfU64 ->
            result {
                let! roll = Uint64Roll.fromUint64Arrays arrayLength aas
                return roll |> rollout.U64
            }
        | RfBs64 -> failwith "not implemented"



    let fromBitPack (rolloutFormat: rolloutFormat) 
                    (arrayLength: arrayLength) 
                    (bitPack: bitPack) =

        match rolloutFormat with
        | RfB ->
            result {
                let! roll = BooleanRoll.fromBitPack arrayLength bitPack
                return roll |> rollout.B
            }
        | RfU8 ->
            result {
                let! roll = Uint8Roll.fromBitPack arrayLength bitPack
                return roll |> rollout.U8
            }
        | RfU16 ->
            result {
                let! roll = Uint16Roll.fromBitPack arrayLength bitPack
                return roll |> rollout.U16
            }
        | RfI32 ->
            result {
                let! roll = IntRoll.fromBitPack arrayLength bitPack
                return roll |> rollout.I32
            }
        | RfU64 ->
            result {
                let! roll = Uint64Roll.fromBitPack arrayLength bitPack
                return roll |> rollout.U64
            }
        | RfBs64 ->
            result {
                let! roll = Bs64Roll.fromBitPack arrayLength bitPack
                return roll |> rollout.Bs64
            }


    let toBitPack (symbolSetSize: symbolSetSize) (rollout: rollout) =
        match rollout with
        | B _uBRoll -> _uBRoll |> BooleanRoll.toBitPack
        | U8 _uInt8Roll -> _uInt8Roll |> Uint8Roll.toBitPack symbolSetSize
        | U16 _uInt16Roll -> _uInt16Roll |> Uint16Roll.toBitPack symbolSetSize
        | I32 _intRoll -> _intRoll |> IntRoll.toBitPack symbolSetSize
        | U64 _uInt64Roll -> _uInt64Roll |> Uint64Roll.toBitPack symbolSetSize
        | Bs64 _bs64Roll -> _bs64Roll |> Bs64Roll.toBitPack
