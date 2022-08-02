namespace global
open System



type rolloutO = private { byteWidth:byteWidth; chunkCount:symbolCount; order:order; data:byte[] }
module RolloutO =

    let indexLength (rollout:rolloutO) =
        (ByteWidth.value rollout.byteWidth) * (Order.value rollout.order)

    let getByteWidth (rollout:rolloutO) =
        rollout.byteWidth

    let getChunkCount (rollout:rolloutO) =
        rollout.chunkCount

    let getOrder (rollout:rolloutO) =
        rollout.order

    let getData (rollout:rolloutO) =
        rollout.data

    let create (byteWidth:byteWidth) (chunkCount:symbolCount) (order:order) =
        {
            rolloutO.byteWidth = byteWidth; chunkCount = chunkCount; order=order;
            data = Array.zeroCreate<byte> ((ByteWidth.value byteWidth) * 
                                           (SymbolCount.value chunkCount) * 
                                           (Order.value order))
        }

    let init (byteWidth:byteWidth) 
             (chunkCount:symbolCount) 
             (order:order) 
             (data:byte[]) =
        let bytesNeeded = (ByteWidth.value byteWidth) * (SymbolCount.value chunkCount) * (Order.value order)
        if (bytesNeeded <> data.Length) then
            "data size is incorrect" |> Error
        else
            {
                rolloutO.byteWidth = byteWidth; chunkCount = chunkCount; order=order;
                data = data
            } |> Ok


    let getUint8s (rollIndex:int) (dataOut:uint8[]) (rollout:rolloutO) =
        let internalOffset = (indexLength rollout) * rollIndex
        match (ByteWidth.value rollout.byteWidth) with
        | x when (x = 1)  ->
            Buffer.BlockCopy(rollout.data, internalOffset, dataOut, 0, (indexLength rollout))
        | x when (x = 2)  -> 
            "invalid byteWidth for getUint8s" |> failwith
        | x when (x = 4)  ->
            "invalid byteWidth for getUint8s" |> failwith
        | x when (x = 8)  -> 
            "invalid byteWidth for getUint8s" |> failwith
        | _ ->  "invalid byteWidth" |> failwith


    let setUint8s (rollIndex:int) (dataIn:uint8[]) (rollout:rolloutO) =
        let internalOffset = (indexLength rollout) * rollIndex
        match (ByteWidth.value rollout.byteWidth) with
        | x when (x = 1)  ->
            Buffer.BlockCopy(dataIn, 0, rollout.data, internalOffset, (indexLength rollout))
        | x when (x = 2)  -> 
            "invalid byteWidth for setUint8s" |> failwith
        | x when (x = 4)  ->
            "invalid byteWidth for setUint8s" |> failwith
        | x when (x = 8)  -> 
            "invalid byteWidth for setUint8s" |> failwith
        | _ ->  "invalid byteWidth" |> failwith


    let getUint16s (rollIndex:int) (dataOut:uint16[]) (rollout:rolloutO) =
        let internalOffset = (indexLength rollout) * rollIndex
        match (ByteWidth.value rollout.byteWidth) with
        | x when (x = 1)  ->
            "invalid byteWidth for getUint16s" |> failwith
        | x when (x = 2)  -> 
             Buffer.BlockCopy(rollout.data, internalOffset, dataOut, 0, (indexLength rollout))
        | x when (x = 4)  ->
            "invalid byteWidth for getUint16s" |> failwith
        | x when (x = 8)  -> 
            "invalid byteWidth for getUint16s" |> failwith
        | _ ->  "invalid byteWidth" |> failwith


    let setUint16s (rollIndex:int) (dataIn:uint16[]) (rollout:rolloutO) =
        let internalOffset = (indexLength rollout) * rollIndex
        match (ByteWidth.value rollout.byteWidth) with
        | x when (x = 1)  ->
            "invalid byteWidth for setUint16s" |> failwith
        | x when (x = 2)  -> 
            Buffer.BlockCopy(dataIn, 0, rollout.data, internalOffset, (indexLength rollout))
        | x when (x = 4)  ->
            "invalid byteWidth for setUint16s" |> failwith
        | x when (x = 8)  -> 
            "invalid byteWidth for setUint16s" |> failwith
        | _ ->  "invalid byteWidth" |> failwith


    let getUint32s (rollIndex:int) (dataOut:uint32[]) (rollout:rolloutO) =
        let internalOffset = (indexLength rollout) * rollIndex
        match (ByteWidth.value rollout.byteWidth) with
        | x when (x = 1)  ->
            "invalid byteWidth for Uint32" |> failwith
        | x when (x = 2)  -> 
            "invalid byteWidth for Uint32" |> failwith
        | x when (x = 4)  ->
            Buffer.BlockCopy(rollout.data, internalOffset, dataOut, 0, (indexLength rollout))
        | x when (x = 8)  -> 
            "invalid byteWidth for Uint32" |> failwith
        | _ ->  "invalid byteWidth" |> failwith


    let setUint32s (rollIndex:int) (dataIn:uint32[]) (rollout:rolloutO) =
        let internalOffset = (indexLength rollout) * rollIndex
        match (ByteWidth.value rollout.byteWidth) with
        | x when (x = 1)  ->
            "invalid byteWidth for setUint32s" |> failwith
        | x when (x = 2)  -> 
            "invalid byteWidth for setUint32s" |> failwith
        | x when (x = 4)  ->
            Buffer.BlockCopy(dataIn, 0, rollout.data, internalOffset, (indexLength rollout))
        | x when (x = 8)  -> 
            "invalid byteWidth for setUint32s" |> failwith
        | _ ->  "invalid byteWidth" |> failwith


    let getUint64s (rollIndex:int) (dataOut:uint64[]) (rollout:rolloutO) =
        let internalOffset = (indexLength rollout) * rollIndex
        match (ByteWidth.value rollout.byteWidth) with
        | x when (x = 1)  ->
            "invalid byteWidth for Uint64" |> failwith
        | x when (x = 2)  -> 
            "invalid byteWidth for Uint64" |> failwith
        | x when (x = 4)  ->
            "invalid byteWidth for Uint64" |> failwith
        | x when (x = 8)  -> 
            Buffer.BlockCopy(rollout.data, internalOffset, dataOut, 0, (indexLength rollout))
        | _ ->  "invalid byteWidth" |> failwith


    let setUint64s (rollIndex:int) (dataIn:uint64[]) (rollout:rolloutO) =
        let internalOffset = (indexLength rollout) * rollIndex
        match (ByteWidth.value rollout.byteWidth) with
        | x when (x = 1)  ->
            "invalid byteWidth for setUint64s" |> failwith
        | x when (x = 2)  -> 
            "invalid byteWidth for setUint64s" |> failwith
        | x when (x = 4)  ->
            "invalid byteWidth for setUint64s" |> failwith
        | x when (x = 8)  -> 
            Buffer.BlockCopy(dataIn, 0, rollout.data, internalOffset, (indexLength rollout))
        | _ ->  "invalid byteWidth" |> failwith


type bitPack = private { bitWidth:bitWidth; symbolCount:symbolCount; data:byte[] }
module BitPack =

    let getBitWidth (bitPack:bitPack) =
        bitPack.bitWidth

    let getSymbolCount (bitPack:bitPack) =
        bitPack.symbolCount

    let getData (bitPack:bitPack) =
        bitPack.data



type uInt8Roll = private { arrayCount:arrayCount; arrayLength:arrayLength; data:uint8[] }
module Uint8Roll =

    let getArrayCount (uInt8Roll:uInt8Roll) =
        uInt8Roll.arrayCount

    let getArrayLength (uInt8Roll:uInt8Roll) =
        uInt8Roll.arrayLength

    let getData (uInt8Roll:uInt8Roll) =
        uInt8Roll.data

    let fromBitPack (arrayLength:arrayLength) (bitPack:bitPack) =
        result {
            let! arrayCount = ((bitPack.symbolCount |> SymbolCount.value) / 
                               (arrayLength |> ArrayLength.value))
                                |> ArrayCount.create
            let bitWidth = bitPack |> BitPack.getBitWidth
            let! byteWidth = 8 |> BitWidth.create
            let data = bitPack |> BitPack.getData 
                               |> ByteUtils.byteSeqToBits byteWidth
                               |> ByteUtils.bitSeqToBytes bitWidth
                               |> Seq.toArray

            return { uInt8Roll.arrayCount = arrayCount; arrayLength = arrayLength; data = data }
        }

    let toBitPack (symbolSetSize:symbolSetSize) (uInt8Roll:uInt8Roll) =
        result {
          let! bitWidth = symbolSetSize |> BitWidth.fromSymbolSetSize
          let! byteWidth = 8 |> BitWidth.create
          let! symbolCount = ((uInt8Roll.arrayCount |> ArrayCount.value) * 
                             (uInt8Roll.arrayLength |> ArrayLength.value))
                             |> SymbolCount.create
          let data = uInt8Roll.data |> ByteUtils.byteSeqToBits bitWidth
                                   |> ByteUtils.bitSeqToBytes byteWidth
                                   |> Seq.toArray
          return { bitPack.bitWidth = bitWidth; symbolCount = symbolCount; data = data }
        
        }

    let fromIntArraySeq (arrayLength:arrayLength) (arrayCount:arrayCount) 
                        (aas:seq<int[]>) =
        result {
            let uint8s = aas |> Seq.concat
                             |> Seq.map(uint8)
                             |> Seq.toArray
            let! res = CollectionProps.check2dArraySize arrayLength arrayCount uint8s
            let! bigA = uint8s |> ByteArray.convertUint8sToBytes
            return { uInt8Roll.arrayCount = arrayCount; arrayLength = arrayLength; data = bigA }
        }

    let toIntArraySeq (uInt8Roll:uInt8Roll) =
         uInt8Roll.data |> Seq.map(int)
                       |> Seq.chunkBySize(uInt8Roll.arrayLength |> ArrayLength.value)


type uInt16Roll = private { arrayCount:arrayCount; arrayLength:arrayLength; data:uint16[] }
module Uint16Roll =

    let getArrayCount (uInt16Roll:uInt16Roll) =
        uInt16Roll.arrayCount

    let getArrayLength (uInt16Roll:uInt16Roll) =
        uInt16Roll.arrayLength

    let getData (uInt16Roll:uInt16Roll) =
        uInt16Roll.data

    let fromBitPack (arrayLength:arrayLength) (bitPack:bitPack) =
        result {
            let! arrayCount = ((bitPack.symbolCount |> SymbolCount.value) / 
                               (arrayLength |> ArrayLength.value))
                                |> ArrayCount.create
            let bitWidth = bitPack |> BitPack.getBitWidth
            let! byteWidth = 8 |> BitWidth.create
            let data = bitPack |> BitPack.getData 
                               |> ByteUtils.byteSeqToBits byteWidth
                               |> ByteUtils.bitSeqToUint16 bitWidth
                               |> Seq.toArray

            return { uInt16Roll.arrayCount = arrayCount; arrayLength = arrayLength; data = data }
        }

    let toBitPack (symbolSetSize:symbolSetSize) (uInt16Roll:uInt16Roll) =
        result {
          let! bitWidth = symbolSetSize |> BitWidth.fromSymbolSetSize
          let! byteWidth = 8 |> BitWidth.create
          let! symbolCount = ((uInt16Roll.arrayCount |> ArrayCount.value) * 
                             (uInt16Roll.arrayLength |> ArrayLength.value))
                             |> SymbolCount.create
          let data = uInt16Roll.data |> ByteUtils.uint16SeqToBits bitWidth
                                     |> ByteUtils.bitSeqToBytes byteWidth
                                     |> Seq.toArray
          return { bitPack.bitWidth = bitWidth; symbolCount = symbolCount; data = data }
        }

    let fromIntArraySeq (arrayLength:arrayLength) (arrayCount:arrayCount) 
                        (aas:seq<int[]>) =
        result {
            let uint16s = aas |> Seq.concat
                             |> Seq.map(uint16)
                             |> Seq.toArray
            let! res = CollectionProps.check2dArraySize arrayLength arrayCount uint16s
            return { uInt16Roll.arrayCount = arrayCount; arrayLength = arrayLength; data = uint16s }
        }

    let toIntArraySeq (uInt16Roll:uInt16Roll) =
         uInt16Roll.data |> Seq.map(int)
                         |> Seq.chunkBySize(uInt16Roll.arrayLength |> ArrayLength.value)



type intRoll = private { arrayCount:arrayCount; arrayLength:arrayLength; data:int[] }
module IntRoll =

    let getArrayCount (uInt8Roll:intRoll) =
        uInt8Roll.arrayCount

    let getArrayLength (intRoll:intRoll) =
        intRoll.arrayLength

    let getData (uInt8Roll:intRoll) =
        uInt8Roll.data

    let fromBitPack (arrayLength:arrayLength) (bitPack:bitPack) =
        result {
            let! arrayCount = ((bitPack.symbolCount |> SymbolCount.value) / 
                               (arrayLength |> ArrayLength.value))
                                |> ArrayCount.create
            let bitWidth = bitPack |> BitPack.getBitWidth
            let! byteWidth = 8 |> BitWidth.create
            let data = bitPack |> BitPack.getData 
                               |> ByteUtils.byteSeqToBits byteWidth
                               |> ByteUtils.bitSeqToInts bitWidth
                               |> Seq.toArray

            return { intRoll.arrayCount = arrayCount; arrayLength = arrayLength; data = data }
        }

    let toBitPack (symbolSetSize:symbolSetSize) (intRoll:intRoll) =
        result {
          let! bitWidth = symbolSetSize |> BitWidth.fromSymbolSetSize
          let! byteWidth = 8 |> BitWidth.create
          let! symbolCount = ((intRoll.arrayCount |> ArrayCount.value) * 
                             (intRoll.arrayLength |> ArrayLength.value))
                             |> SymbolCount.create
          let data = intRoll.data |> ByteUtils.intSeqToBits bitWidth
                                  |> ByteUtils.bitSeqToBytes byteWidth
                                  |> Seq.toArray
          return { bitPack.bitWidth = bitWidth; symbolCount = symbolCount; data = data }
        
        }

    let fromIntArraySeq (arrayLength:arrayLength) (arrayCount:arrayCount) 
                        (aas:seq<int[]>) =
        result {
            let intA = aas |> Seq.concat
                           |> Seq.toArray
            let! res = CollectionProps.check2dArraySize arrayLength arrayCount intA
            return { intRoll.arrayCount = arrayCount; arrayLength = arrayLength; data = intA }
        }

    let toIntArraySeq (uInt8Roll:intRoll) =
         uInt8Roll.data |> Seq.chunkBySize(uInt8Roll.arrayLength |> ArrayLength.value)


type uint64Roll = private { arrayCount:arrayCount; arrayLength:arrayLength; data:uint64[] }
module Uint64Roll =

    let getArrayCount (uint64Roll:uint64Roll) =
        uint64Roll.arrayCount

    let getArrayLength (uint64Roll:uint64Roll) =
        uint64Roll.arrayLength

    let getData (uint64Roll:uint64Roll) =
        uint64Roll.data

    let fromBitPack (arrayLength:arrayLength) (bitPack:bitPack) =
        result {
            let! arrayCount = ((bitPack.symbolCount |> SymbolCount.value) / 
                               (arrayLength |> ArrayLength.value))
                                |> ArrayCount.create
            let bitWidth = bitPack |> BitPack.getBitWidth
            let! byteWidth = 8 |> BitWidth.create
            let data = bitPack |> BitPack.getData 
                               |> ByteUtils.byteSeqToBits byteWidth
                               |> ByteUtils.bitSeqToUint64 bitWidth
                               |> Seq.toArray

            return { uint64Roll.arrayCount = arrayCount; arrayLength = arrayLength; data = data }
        }

    let toBitPack (symbolSetSize:symbolSetSize) (uint64Roll:uint64Roll) =
        result {
          let! bitWidth = symbolSetSize |> BitWidth.fromSymbolSetSize
          let! byteWidth = 8 |> BitWidth.create
          let! symbolCount = ((uint64Roll.arrayCount |> ArrayCount.value) * 
                             (uint64Roll.arrayLength |> ArrayLength.value))
                             |> SymbolCount.create
          let data = uint64Roll.data |> ByteUtils.uint64SeqToBits bitWidth
                                     |> ByteUtils.bitSeqToBytes byteWidth
                                     |> Seq.toArray
          return { bitPack.bitWidth = bitWidth; symbolCount = symbolCount; data = data }
        
        }

    let fromIntArraySeq (arrayLength:arrayLength) (arrayCount:arrayCount) 
                        (aas:seq<int[]>) =
        result {
            let uint64s = aas |> Seq.concat
                              |> Seq.map(uint64)
                              |> Seq.toArray
            let! res = CollectionProps.check2dArraySize arrayLength arrayCount uint64s
            return { uint64Roll.arrayCount = arrayCount; arrayLength = arrayLength; data = uint64s }
        }

    let toIntArraySeq (uint64Roll:uint64Roll) =
        uint64Roll.data |> Seq.map(int)
                        |> Seq.chunkBySize(uint64Roll.arrayLength |> ArrayLength.value)


    let fromUint64ArraySeq (arrayLength:arrayLength) (arrayCount:arrayCount) 
                           (aas:seq<uint64[]>) =
        result {
            let uint64s = aas |> Seq.concat
                              |> Seq.toArray
            let! res = CollectionProps.check2dArraySize arrayLength arrayCount uint64s
            return { uint64Roll.arrayCount = arrayCount; arrayLength = arrayLength; data = uint64s }
        }

    let toUint64ArraySeq (uint64Roll:uint64Roll) =
        uint64Roll.data |> Seq.chunkBySize(uint64Roll.arrayLength |> ArrayLength.value)


    let fromIntArraySeqAsBitStriped (arrayLength:arrayLength) 
                                    (arrayCount:arrayCount) 
                                    (aas:seq<int[]>) =
        let oo = arrayLength |> ArrayLength.value |> Order.createNr
        let wab = aas |> Seq.toArray

        let yab = ByteUtils.toStripeArrays 1 oo aas |> Seq.toArray
        result {
            let order = arrayLength |> ArrayLength.value |> Order.createNr
            let data = ByteUtils.toStripeArrays 1 order aas
                                |> Seq.concat
                                |> Seq.toArray
            return { uint64Roll.arrayCount = arrayCount; arrayLength = arrayLength; data = data }
        }


    let asBitStripedToIntArraySeq (uint64Roll:uint64Roll) =
         let order = uint64Roll.arrayLength |> ArrayLength.value |> Order.createNr
         ByteUtils.fromStripeArrays 0 1 order uint64Roll.data













type rolloutFormat = | RfU8 | RfU16 | RfI32 | RfU64

module RolloutFormat =
    let fromBitWidth (bitWidth:bitWidth) =
        match (bitWidth |> BitWidth.value) with
        | bw when bw < 9 -> rolloutFormat.RfU8
        | bw when bw < 17 -> rolloutFormat.RfU16
        | bw when bw < 32 -> rolloutFormat.RfI32
        | _ -> rolloutFormat.RfU64

type rollout = 
    | U8 of uInt8Roll
    | U16 of uInt16Roll
    | I32 of intRoll
    | U64 of uint64Roll


module Rollout =

    let getArrayLength (rollout:rollout) =
        match rollout with
        | U8 _uInt8Roll -> _uInt8Roll.arrayLength
        | U16 _uInt16Roll -> _uInt16Roll.arrayLength
        | I32 _intRoll -> _intRoll.arrayLength
        | U64 _uInt64Roll -> _uInt64Roll.arrayLength


    let getArrayCount (rollout:rollout) =
        match rollout with
        | U8 _uInt8Roll -> _uInt8Roll.arrayCount
        | U16 _uInt16Roll -> _uInt16Roll.arrayCount
        | I32 _intRoll -> _intRoll.arrayCount
        | U64 _uInt64Roll -> _uInt64Roll.arrayCount


    let fromIntArraySeq (rolloutFormat:rolloutFormat) (arrayLength:arrayLength) 
                        (arrayCount:arrayCount) (aas:seq<int[]>) =

        match rolloutFormat with
        | RfU8 -> result {
                        let! roll = Uint8Roll.fromIntArraySeq arrayLength arrayCount aas
                        return roll |> rollout.U8
                    }
        | RfU16 -> result {
                        let! roll = Uint16Roll.fromIntArraySeq arrayLength arrayCount aas
                        return roll |> rollout.U16
                    }
        | RfI32 -> result {
                        let! roll = IntRoll.fromIntArraySeq arrayLength arrayCount aas
                        return roll |> rollout.I32
                    }
        | RfU64 -> result {
                        let! roll = Uint64Roll.fromIntArraySeq arrayLength arrayCount aas
                        return roll |> rollout.U64
                    }

    let toIntArraySeq (rollout:rollout) =
        match rollout with
        | U8 _uInt8Roll -> _uInt8Roll |> Uint8Roll.toIntArraySeq
        | U16 _uInt16Roll -> _uInt16Roll |> Uint16Roll.toIntArraySeq
        | I32 _intRoll -> _intRoll |> IntRoll.toIntArraySeq
        | U64 _uInt64Roll -> _uInt64Roll |> Uint64Roll.toIntArraySeq


    let fromUInt64ArraySeq (rolloutFormat:rolloutFormat) (arrayLength:arrayLength) 
                           (arrayCount:arrayCount) (aas:seq<uint64[]>) =

        let intSeq = aas |> Seq.concat |> Seq.map(int) 
                         |> Seq.chunkBySize(arrayLength |> ArrayLength.value)
        match rolloutFormat with
        | RfU8 -> result {
                        let! roll = Uint8Roll.fromIntArraySeq arrayLength arrayCount intSeq
                        return roll |> rollout.U8
                    }
        | RfU16 -> result {
                        let! roll = Uint16Roll.fromIntArraySeq arrayLength arrayCount intSeq
                        return roll |> rollout.U16
                    }
        | RfI32 -> result {
                        let! roll = IntRoll.fromIntArraySeq arrayLength arrayCount intSeq
                        return roll |> rollout.I32
                    }
        | RfU64 -> result {
                        let! roll = Uint64Roll.fromUint64ArraySeq arrayLength arrayCount aas
                        return roll |> rollout.U64
                    }


    let fromBitPackRf (rolloutFormat:rolloutFormat) (arrayLength:arrayLength) 
                      (bitPack:bitPack) =
        match rolloutFormat with
        | RfU8 -> result {
                        let! roll = Uint8Roll.fromBitPack arrayLength bitPack
                        return roll |> rollout.U8
                    }
        | RfU16 -> result {
                        let! roll = Uint16Roll.fromBitPack arrayLength bitPack
                        return roll |> rollout.U16
                    }
        | RfI32 -> result {
                        let! roll = IntRoll.fromBitPack arrayLength bitPack
                        return roll |> rollout.I32
                    }
        | RfU64 -> result {
                        let! roll = Uint64Roll.fromBitPack arrayLength bitPack
                        return roll |> rollout.U64
                    }


    let fromBitPack (arrayLength:arrayLength) (bitPack:bitPack) =
        let rolloutFormat = bitPack.bitWidth |> RolloutFormat.fromBitWidth
        fromBitPackRf rolloutFormat arrayLength bitPack


    let toBitPack (symbolSetSize:symbolSetSize) (rollout:rollout) =
        match rollout with
        | U8 _uInt8Roll -> _uInt8Roll |> Uint8Roll.toBitPack symbolSetSize
        | U16 _uInt16Roll -> _uInt16Roll |> Uint16Roll.toBitPack symbolSetSize
        | I32 _intRoll -> _intRoll |> IntRoll.toBitPack symbolSetSize
        | U64 _uInt64Roll -> _uInt64Roll |> Uint64Roll.toBitPack symbolSetSize
