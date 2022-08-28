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


type booleanRoll = private { arrayCount:arrayCount; arrayLength:arrayLength; data:bool[] }
module BooleanRoll =
    
    let create (arrayCount:arrayCount) 
               (arrayLength:arrayLength) =

        let dataLength =  (arrayCount |> ArrayCount.value) * 
                          (arrayLength |> ArrayLength.value)
        { arrayCount = arrayCount;
          arrayLength = arrayLength;
          data = Array.zeroCreate<bool> dataLength }

    let getArrayCount (booleanRoll:booleanRoll) =
        booleanRoll.arrayCount

    let getArrayLength (booleanRoll:booleanRoll) =
        booleanRoll.arrayLength

    let getData (booleanRoll:booleanRoll) =
        booleanRoll.data

    let copy (booleanRoll:booleanRoll) =
        {
           booleanRoll with
            data = booleanRoll.data |> Array.copy
        }

    let fromBitPack (arrayLength:arrayLength) (bitPack:bitPack) =
        result {
            let! arrayCount = ((bitPack.symbolCount |> SymbolCount.value) / 
                               (arrayLength |> ArrayLength.value))
                                |> ArrayCount.create
            let bitsPerSymbol = bitPack |> BitPack.getBitsPerSymbol
            let data = bitPack |> BitPack.getData 
                               |> ByteUtils.getAllBitsFromByteSeq
                               |> Seq.toArray

            return { booleanRoll.arrayCount = arrayCount; arrayLength = arrayLength; data = data }
        }

    let toBitPack (booleanRoll:booleanRoll) =
        result {
          let! bitsPerSymbol = 1 |> BitsPerSymbol.create
          let! symbolCount = ((booleanRoll.arrayCount |> ArrayCount.value) * 
                              (booleanRoll.arrayLength |> ArrayLength.value))
                             |> SymbolCount.create
          let data = booleanRoll.data
                     |> ByteUtils.storeBitSeqInBytes
                     |> Seq.toArray
          return { bitPack.bitsPerSymbol = bitsPerSymbol; symbolCount = symbolCount; data = data }
        
        }


    let fromBoolArraySeq (arrayLength:arrayLength) (boolAs:seq<bool[]>) =
        result {
            let bools = boolAs |> Seq.concat |> Seq.toArray
            let arrayCount = (bools.Length / (ArrayLength.value arrayLength)) 
                              |> ArrayCount.createNr
            let! res = CollectionProps.check2dArraySize arrayLength arrayCount bools
            return { booleanRoll.arrayCount = arrayCount; 
                     arrayLength = arrayLength; 
                     data = bools }
        }


    let fromIntArraySeq (arrayLength:arrayLength) (aas:seq<int[]>) =
        result {
            let boolA = aas |> Seq.concat
                             |> Seq.map(fun bv -> if (bv = 1) then true else false)
                             |> Seq.toArray
            let arrayCount = (boolA.Length / (ArrayLength.value arrayLength)) 
                              |> ArrayCount.createNr
            let! res = CollectionProps.check2dArraySize arrayLength arrayCount boolA
            return { booleanRoll.arrayCount = arrayCount; 
                     arrayLength = arrayLength; data = boolA }
        }


    let isSorted (booleanRoll:booleanRoll) =
        let mutable i=0
        let iIncr = booleanRoll.arrayLength |> ArrayLength.value
        let iMax = booleanRoll.data.Length
        let mutable looP = true
        while ((i < iMax) && looP) do
             looP <- CollectionProps.isSortedOffset booleanRoll.data i iIncr
             i<-i+iIncr
        looP

    let toIntArraySeq (booleanRoll:booleanRoll) =
         booleanRoll.data |> Seq.map(fun bv -> if bv then 1 else 0)
                       |> Seq.chunkBySize(booleanRoll.arrayLength |> ArrayLength.value)



type uInt8Roll = private { arrayCount:arrayCount; arrayLength:arrayLength; data:uint8[] }
module Uint8Roll =

    let getArrayCount (uInt8Roll:uInt8Roll) =
        uInt8Roll.arrayCount

    let getArrayLength (uInt8Roll:uInt8Roll) =
        uInt8Roll.arrayLength

    let getData (uInt8Roll:uInt8Roll) =
        uInt8Roll.data

    let copy (uInt8Roll:uInt8Roll) = 
        {
            arrayCount = uInt8Roll.arrayCount;
            arrayLength = uInt8Roll.arrayLength;
            data = uInt8Roll.data |> Array.copy
        }

    let fromBitPack (arrayLength:arrayLength) (bitPack:bitPack) =
        result {
            let! arrayCount = ((bitPack.symbolCount |> SymbolCount.value) / 
                               (arrayLength |> ArrayLength.value))
                                |> ArrayCount.create
            let bitsPerSymbol = bitPack |> BitPack.getBitsPerSymbol
            let data = bitPack |> BitPack.getData 
                               |> ByteUtils.getAllBitsFromByteSeq
                               |> ByteUtils.bitsToSpBytePositions bitsPerSymbol
                               |> Seq.toArray

            return { uInt8Roll.arrayCount = arrayCount; arrayLength = arrayLength; data = data }
        }

    let toBitPack (symbolSetSize:symbolSetSize) (uInt8Roll:uInt8Roll) =
        result {
          let! bitsPerSymbol = symbolSetSize |> BitsPerSymbol.fromSymbolSetSize
          let! symbolCount = ((uInt8Roll.arrayCount |> ArrayCount.value) * 
                             (uInt8Roll.arrayLength |> ArrayLength.value))
                             |> SymbolCount.create
          let data = uInt8Roll.data |> ByteUtils.bitsFromSpBytePositions bitsPerSymbol
                                    |> ByteUtils.storeBitSeqInBytes
                                    |> Seq.toArray
          return { bitPack.bitsPerSymbol = bitsPerSymbol; symbolCount = symbolCount; data = data }
        
        }


    let fromBoolArraySeq (arrayLength:arrayLength) (aas:seq<bool[]>) =
        result {
            let uint8s = aas |> Seq.concat
                             |> Seq.map(fun tf -> if tf then 1uy else 0uy)
                             |> Seq.toArray

            let arrayCount = (uint8s.Length / (ArrayLength.value arrayLength)) 
                              |> ArrayCount.createNr
            let! res = CollectionProps.check2dArraySize arrayLength arrayCount uint8s
            return { uInt8Roll.arrayCount = arrayCount; arrayLength = arrayLength; data = uint8s }
        }


    let fromIntArraySeq (arrayLength:arrayLength) (aas:seq<int[]>) =
        result {
            let uint8s = aas |> Seq.concat
                             |> Seq.map(uint8)
                             |> Seq.toArray
            let arrayCount = (uint8s.Length / (ArrayLength.value arrayLength)) 
                              |> ArrayCount.createNr
            let! res = CollectionProps.check2dArraySize arrayLength arrayCount uint8s
            let! bigA = uint8s |> ByteArray.convertUint8sToBytes
            return { uInt8Roll.arrayCount = arrayCount; arrayLength = arrayLength; data = bigA }
        }


    let isSorted (uInt8Roll:uInt8Roll) =
        let mutable i=0
        let iIncr = uInt8Roll.arrayLength |> ArrayLength.value
        let iMax = uInt8Roll.data.Length
        let mutable looP = true
        while ((i < iMax) && looP) do
             looP <- CollectionProps.isSortedOffset uInt8Roll.data i iIncr
             i<-i+iIncr
        looP

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

    let copy (uInt16Roll:uInt16Roll) = 
        {
            arrayCount = uInt16Roll.arrayCount;
            arrayLength = uInt16Roll.arrayLength;
            data = uInt16Roll.data |> Array.copy
        }


    let fromBitPack (arrayLength:arrayLength) (bitPack:bitPack) =
        result {
            let! arrayCount = ((bitPack.symbolCount |> SymbolCount.value) / 
                               (arrayLength |> ArrayLength.value))
                                |> ArrayCount.create
            let bitsPerSymbol = bitPack |> BitPack.getBitsPerSymbol
            let data = bitPack |> BitPack.getData 
                               |> ByteUtils.getAllBitsFromByteSeq
                               |> ByteUtils.bitsToSpUint16Positions bitsPerSymbol
                               |> Seq.toArray

            return { uInt16Roll.arrayCount = arrayCount; arrayLength = arrayLength; data = data }
        }


    let toBitPack (symbolSetSize:symbolSetSize) (uInt16Roll:uInt16Roll) =
        result {
          let! bitsPerSymbol = symbolSetSize |> BitsPerSymbol.fromSymbolSetSize
          let! symbolCount = ((uInt16Roll.arrayCount |> ArrayCount.value) * 
                             (uInt16Roll.arrayLength |> ArrayLength.value))
                             |> SymbolCount.create
          let data = uInt16Roll.data |> ByteUtils.bitsFromSpUint16Positions bitsPerSymbol
                                     |> ByteUtils.storeBitSeqInBytes
                                     |> Seq.toArray
          return { bitPack.bitsPerSymbol = bitsPerSymbol; symbolCount = symbolCount; data = data }
        }

    let fromBoolArraySeq (arrayLength:arrayLength) (aas:seq<bool[]>) =
        result {
            let uint16s = aas |> Seq.concat
                              |> Seq.map(fun tf -> if tf then 0us else 1us)
                              |> Seq.toArray

            let arrayCount = (uint16s.Length / (ArrayLength.value arrayLength)) 
                              |> ArrayCount.createNr
            let! res = CollectionProps.check2dArraySize arrayLength arrayCount uint16s
            return { uInt16Roll.arrayCount = arrayCount; arrayLength = arrayLength; data = uint16s }
        }


    let fromIntArraySeq (arrayLength:arrayLength) (aas:seq<int[]>) =
        result {
            let uint16s = aas |> Seq.concat
                             |> Seq.map(uint16)
                             |> Seq.toArray

            let arrayCount = (uint16s.Length / (ArrayLength.value arrayLength)) 
                              |> ArrayCount.createNr
            let! res = CollectionProps.check2dArraySize arrayLength arrayCount uint16s
            return { uInt16Roll.arrayCount = arrayCount; arrayLength = arrayLength; data = uint16s }
        }


    let isSorted (uInt16Roll:uInt16Roll) =
        let mutable i=0
        let iIncr = uInt16Roll.arrayLength |> ArrayLength.value
        let iMax = uInt16Roll.data.Length
        let mutable looP = true
        while ((i < iMax) && looP) do
             looP <- CollectionProps.isSortedOffset uInt16Roll.data i iIncr
             i<-i+iIncr
        looP


    let toIntArraySeq (uInt16Roll:uInt16Roll) =
         uInt16Roll.data |> Seq.map(int)
                         |> Seq.chunkBySize(uInt16Roll.arrayLength |> ArrayLength.value)


type intRoll = private { arrayCount:arrayCount; arrayLength:arrayLength; data:int[] }
module IntRoll =

    let copy (intRoll:intRoll) = 
        {
            arrayCount = intRoll.arrayCount;
            arrayLength = intRoll.arrayLength;
            data = intRoll.data |> Array.copy
        }

    let updateData (data:int[]) (intRoll:intRoll)  =
        {intRoll with data=data}

    let getArrayCount (uInt8Roll:intRoll) =
        uInt8Roll.arrayCount

    let getArrayLength (intRoll:intRoll) =
        intRoll.arrayLength

    let getRolloutLength (intRoll:intRoll) =
        (intRoll.arrayCount |> ArrayCount.value) * 
        (intRoll.arrayLength |> ArrayLength.value)

    let getData (uInt8Roll:intRoll) =
        uInt8Roll.data

    let fromBitPack (arrayLength:arrayLength) (bitPack:bitPack) =
        result {
            let! arrayCount = ((bitPack.symbolCount |> SymbolCount.value) / 
                               (arrayLength |> ArrayLength.value))
                                |> ArrayCount.create
            let bitsPerSymbol = bitPack |> BitPack.getBitsPerSymbol
            let data = bitPack |> BitPack.getData 
                               |> ByteUtils.getAllBitsFromByteSeq
                               |> ByteUtils.bitsToSpIntPositions bitsPerSymbol
                               |> Seq.toArray

            return { intRoll.arrayCount = arrayCount; arrayLength = arrayLength; data = data }
        }

    let toBitPack (symbolSetSize:symbolSetSize) (intRoll:intRoll) =
        result {
          let! bitsPerSymbol = symbolSetSize |> BitsPerSymbol.fromSymbolSetSize
          let! symbolCount = ((intRoll.arrayCount |> ArrayCount.value) * 
                             (intRoll.arrayLength |> ArrayLength.value))
                             |> SymbolCount.create
          let data = intRoll.data |> ByteUtils.bitsFromSpIntPositions bitsPerSymbol
                                  |> ByteUtils.storeBitSeqInBytes
                                  |> Seq.toArray
          return { bitPack.bitsPerSymbol = bitsPerSymbol; symbolCount = symbolCount; data = data }
        
        }

    let fromBoolArraySeq (arrayLength:arrayLength) (aas:seq<bool[]>) =
        result {
            let uint8s = aas |> Seq.concat
                             |> Seq.map(fun tf -> if tf then 0 else 1)
                             |> Seq.toArray

            let arrayCount = (uint8s.Length / (ArrayLength.value arrayLength)) 
                              |> ArrayCount.createNr
            let! res = CollectionProps.check2dArraySize arrayLength arrayCount uint8s
            return { intRoll.arrayCount = arrayCount; arrayLength = arrayLength; data = uint8s }
        }

    let fromIntArraySeq (arrayLength:arrayLength) (aas:seq<int[]>) =
        result {
            let intA = aas |> Seq.concat
                           |> Seq.toArray
            let arrayCount = (intA.Length / (ArrayLength.value arrayLength)) 
                              |> ArrayCount.createNr
            let! res = CollectionProps.check2dArraySize arrayLength arrayCount intA
            return { intRoll.arrayCount = arrayCount; arrayLength = arrayLength; data = intA }
        }

    let isSorted (uInt8Roll:intRoll) =
        let mutable i=0
        let iIncr = uInt8Roll.arrayLength |> ArrayLength.value
        let iMax = uInt8Roll.data.Length
        let mutable looP = true
        while ((i < iMax) && looP) do
             looP <- CollectionProps.isSortedOffset uInt8Roll.data i iIncr
             i<-i+iIncr
        looP


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
    
    let stripeBlocksNeededForArrayCount (arrayCount:arrayCount) = 
        ( (ArrayCount.value arrayCount) + 63) / 64

    let createEmptyStripedSet (arrayLength:arrayLength) 
                              (arrayCount:arrayCount) =
        let blocksNeeded = stripeBlocksNeededForArrayCount arrayCount
        let dataLength = (ArrayLength.value arrayLength) * blocksNeeded
        let data = Array.zeroCreate<uint64> dataLength
        { arrayCount=arrayCount; arrayLength=arrayLength; data=data }


    let copy (uint64Roll:uint64Roll) = 
        {
            arrayCount = uint64Roll.arrayCount;
            arrayLength = uint64Roll.arrayLength;
            data = uint64Roll.data |> Array.copy
        }

    // *************** not bit striped **********************
    
    let fromBitPack (arrayLength:arrayLength) (bitPack:bitPack) =
        result {
            let! arrayCount = ((bitPack.symbolCount |> SymbolCount.value) / 
                               (arrayLength |> ArrayLength.value))
                                |> ArrayCount.create
            let bitsPerSymbol = bitPack |> BitPack.getBitsPerSymbol
            let data = bitPack |> BitPack.getData 
                               |> ByteUtils.getAllBitsFromByteSeq
                               |> ByteUtils.bitsToSpUint64Positions bitsPerSymbol
                               |> Seq.toArray

            return { uint64Roll.arrayCount = arrayCount; arrayLength = arrayLength; data = data }
        }


    let fromBoolArraySeq (arrayLength:arrayLength) (aas:seq<bool[]>) =
        result {
            let uint8s = aas |> Seq.concat
                             |> Seq.map(fun tf -> if tf then 0uL else 1uL)
                             |> Seq.toArray

            let arrayCount = (uint8s.Length / (ArrayLength.value arrayLength)) 
                              |> ArrayCount.createNr
            let! res = CollectionProps.check2dArraySize arrayLength arrayCount uint8s
            return { uint64Roll.arrayCount = arrayCount; arrayLength = arrayLength; data = uint8s }
        }


    let fromIntArrays (arrayLength:arrayLength) (aas:seq<int[]>) =
        result {
            let uint64s = aas |> Seq.concat
                              |> Seq.map(uint64)
                              |> Seq.toArray
            let arrayCount = (uint64s.Length / (ArrayLength.value arrayLength)) 
                              |> ArrayCount.createNr
            let! res = CollectionProps.check2dArraySize arrayLength arrayCount uint64s
            return { uint64Roll.arrayCount = arrayCount; arrayLength = arrayLength; data = uint64s }
        }


    let fromUint64Arrays (arrayLength:arrayLength) (aas:seq<uint64[]>) =
        result {
            let uint64s = aas |> Seq.concat
                              |> Seq.toArray
            let arrayCount = (uint64s.Length / (ArrayLength.value arrayLength)) 
                                |> ArrayCount.createNr
            let! res = CollectionProps.check2dArraySize arrayLength arrayCount uint64s
            return { uint64Roll.arrayCount = arrayCount; arrayLength = arrayLength; data = uint64s }
        }


    let toBitPack (symbolSetSize:symbolSetSize) (uint64Roll:uint64Roll) =
        result {
          let! bitsPerSymbol = symbolSetSize |> BitsPerSymbol.fromSymbolSetSize
          let! symbolCount = ((uint64Roll.arrayCount |> ArrayCount.value) * 
                              (uint64Roll.arrayLength |> ArrayLength.value))
                             |> SymbolCount.create
          let data = uint64Roll.data |> ByteUtils.bitsFromSpUint64Positions bitsPerSymbol
                                     |> ByteUtils.storeBitSeqInBytes
                                     |> Seq.toArray
          return { bitPack.bitsPerSymbol = bitsPerSymbol; symbolCount = symbolCount; data = data }
        }


    let isSorted (uint64Roll:uint64Roll) =
        let mutable i=0
        let iIncr = uint64Roll.arrayLength |> ArrayLength.value
        let iMax = uint64Roll.data.Length
        let mutable looP = true
        while ((i < iMax) && looP) do
             looP <- CollectionProps.isSortedOffset uint64Roll.data i iIncr
             i<-i+iIncr
        looP


    let toIntArrays (uint64Roll:uint64Roll) =
        uint64Roll.data |> Seq.map(int)
                        |> Seq.chunkBySize(uint64Roll.arrayLength |> ArrayLength.value)


    let toUint64Arrays (uint64Roll:uint64Roll) =
        uint64Roll.data |> Seq.chunkBySize(uint64Roll.arrayLength |> ArrayLength.value)


    
    // ****************   bit striped  **********************

    let getUsedStripes (uint64Roll:uint64Roll) =
        let len = uint64Roll.data.Length
        let arrayLen = uint64Roll.arrayLength |> ArrayLength.value
        let q = uint64Roll.data.[len - arrayLen .. len - 1]
        let lastStripes = q |> ByteUtils.usedStripeCount
        let ww = (len - arrayLen) / arrayLen
        (ww * 64) + lastStripes


    let fromBoolArraysAsBitStriped (arrayLength:arrayLength)
                                   (aas:seq<bool[]>) =
        result {
            let! order = arrayLength |> ArrayLength.value |> Order.create
            let! data = ByteUtils.makeStripedArraysFromBoolArrays order aas
            let! arrayCount = data.Length |> ArrayCount.create
            return { uint64Roll.arrayCount = arrayCount; arrayLength = arrayLength; data = data }
        }


    let asBitStripedtoBitPack (uint64Roll:uint64Roll) =
        result {
            let! order = uint64Roll.arrayLength |> ArrayLength.value |> Order.create
            let! symbolCount = uint64Roll.data |> ByteUtils.usedStripeCount 
                                            |> (*) (order |> Order.value)
                                            |> SymbolCount.create
            let data = ByteUtils.fromStripeArrays false true order uint64Roll.data
                        |> Seq.concat
                        |> ByteUtils.storeBitSeqInBytes
                        |> Seq.toArray
            let! bitsPerSymbol = 2 |> BitsPerSymbol.create
            return  { bitPack.bitsPerSymbol = bitsPerSymbol; symbolCount = symbolCount; data = data }
         }


    let asBitStripedToIntArrays (uint64Roll:uint64Roll) =
         let order = uint64Roll.arrayLength |> ArrayLength.value |> Order.createNr
         ByteUtils.fromStripeArrays 0 1 order uint64Roll.data



type rolloutFormat = |RfB | RfU8 | RfU16 | RfI32 | RfU64

module RolloutFormat =

    let fromBitWidth (bitsPerSymbol:bitsPerSymbol) =
        match (bitsPerSymbol |> BitsPerSymbol.value) with
        | bw when bw < 9 -> rolloutFormat.RfU8
        | bw when bw < 17 -> rolloutFormat.RfU16
        | bw when bw < 32 -> rolloutFormat.RfI32
        | _ -> rolloutFormat.RfU64

    let toDto (rf: rolloutFormat) =
        match rf with
        | rolloutFormat.RfB -> nameof rolloutFormat.RfB
        | rolloutFormat.RfU8 -> nameof rolloutFormat.RfU8
        | rolloutFormat.RfU16 -> nameof rolloutFormat.RfU16
        | rolloutFormat.RfI32 -> nameof rolloutFormat.RfI32
        | rolloutFormat.RfU64 -> nameof rolloutFormat.RfU64

    let fromString str =
        match str with
        | nameof rolloutFormat.RfB -> rolloutFormat.RfB |> Ok
        | nameof rolloutFormat.RfU8 -> rolloutFormat.RfU8 |> Ok
        | nameof rolloutFormat.RfU16 -> rolloutFormat.RfU16 |> Ok
        | nameof rolloutFormat.RfI32 -> rolloutFormat.RfI32 |> Ok
        | nameof rolloutFormat.RfU64 -> rolloutFormat.RfU64 |> Ok
        | _ -> Error (sprintf "no match for rolloutFormat: %s" str)



type rollout = 
    | B of booleanRoll
    | U8 of uInt8Roll
    | U16 of uInt16Roll
    | I32 of intRoll
    | U64 of uint64Roll


module Rollout =

    let getArrayLength (rollout:rollout) =
        match rollout with
        | B _uBRoll -> _uBRoll.arrayLength
        | U8 _uInt8Roll -> _uInt8Roll.arrayLength
        | U16 _uInt16Roll -> _uInt16Roll.arrayLength
        | I32 _intRoll -> _intRoll.arrayLength
        | U64 _uInt64Roll -> _uInt64Roll.arrayLength


    let getArrayCount (rollout:rollout) =
        match rollout with
        | B _uBRoll -> _uBRoll.arrayCount
        | U8 _uInt8Roll -> _uInt8Roll.arrayCount
        | U16 _uInt16Roll -> _uInt16Roll.arrayCount
        | I32 _intRoll -> _intRoll.arrayCount
        | U64 _uInt64Roll -> _uInt64Roll.arrayCount


    let copy (rollout:rollout) =
        match rollout with
        | B _uBRoll -> _uBRoll |> BooleanRoll.copy |> B
        | U8 _uInt8Roll -> _uInt8Roll |> Uint8Roll.copy |> U8
        | U16 _uInt16Roll -> _uInt16Roll |> Uint16Roll.copy |> U16
        | I32 _intRoll -> _intRoll |> IntRoll.copy |> I32
        | U64 _uInt64Roll -> _uInt64Roll |> Uint64Roll.copy |> U64
         

    let getRolloutLength (rollout:rollout) =
        (rollout |> getArrayCount |> ArrayCount.value) * 
        (rollout |> getArrayLength |> ArrayLength.value)


    let fromBoolArrays (rolloutFormat:rolloutFormat) 
                       (arrayLength:arrayLength) 
                       (aas:seq<bool[]>) =
        match rolloutFormat with
        | RfB -> result {
                        let! roll = BooleanRoll.fromBoolArraySeq arrayLength aas
                        return roll |> rollout.B
                    }

        | RfU8 -> result {
                        let! roll = Uint8Roll.fromBoolArraySeq arrayLength aas
                        return roll |> rollout.U8
                    }
        | RfU16 -> result {
                        let! roll = Uint16Roll.fromBoolArraySeq arrayLength aas
                        return roll |> rollout.U16
                    }
        | RfI32 -> result {
                        let! roll = IntRoll.fromBoolArraySeq arrayLength aas
                        return roll |> rollout.I32
                    }
        | RfU64 -> result {
                        let! roll = Uint64Roll.fromBoolArraySeq arrayLength aas
                        return roll |> rollout.U64
                    }


    let fromIntArrays (rolloutFormat:rolloutFormat) 
                      (arrayLength:arrayLength) 
                      (aas:seq<int[]>) =
        match rolloutFormat with
        | RfB -> result {
                let! roll = BooleanRoll.fromIntArraySeq arrayLength aas
                return roll |> rollout.B
            }
        | RfU8 -> result {
                        let! roll = Uint8Roll.fromIntArraySeq arrayLength aas
                        return roll |> rollout.U8
                    }
        | RfU16 -> result {
                        let! roll = Uint16Roll.fromIntArraySeq arrayLength aas
                        return roll |> rollout.U16
                    }
        | RfI32 -> result {
                        let! roll = IntRoll.fromIntArraySeq arrayLength aas
                        return roll |> rollout.I32
                    }
        | RfU64 -> result {
                        let! roll = Uint64Roll.fromIntArrays arrayLength aas
                        return roll |> rollout.U64
                    }


    let isSorted (rollout:rollout) =
        match rollout with
        | B _uBRoll -> _uBRoll |> BooleanRoll.isSorted
        | U8 _uInt8Roll -> _uInt8Roll |> Uint8Roll.isSorted
        | U16 _uInt16Roll -> _uInt16Roll |> Uint16Roll.isSorted
        | I32 _intRoll -> _intRoll |> IntRoll.isSorted
        | U64 _uInt64Roll -> _uInt64Roll |> Uint64Roll.isSorted


    let toIntArraySeq (rollout:rollout) =
        match rollout with
        | B _uBRoll -> _uBRoll |> BooleanRoll.toIntArraySeq
        | U8 _uInt8Roll -> _uInt8Roll |> Uint8Roll.toIntArraySeq
        | U16 _uInt16Roll -> _uInt16Roll |> Uint16Roll.toIntArraySeq
        | I32 _intRoll -> _intRoll |> IntRoll.toIntArraySeq
        | U64 _uInt64Roll -> _uInt64Roll |> Uint64Roll.toIntArrays


    let uniqueMembers (rollout:rollout) =
        rollout |> toIntArraySeq
                |> Seq.distinct


    let uniqueUnsortedMembers (rollout:rollout) =
        rollout |> uniqueMembers
                |> Seq.filter(fun ia -> not (CollectionProps.isSorted_inline ia))


    let fromUInt64ArraySeq (rolloutFormat:rolloutFormat) 
                           (arrayLength:arrayLength) 
                           (aas:seq<uint64[]>) =

        let intSeq = aas |> Seq.concat |> Seq.map(int) 
                         |> Seq.chunkBySize(arrayLength |> ArrayLength.value)
        match rolloutFormat with
        | RfB -> result {
                    let! roll = BooleanRoll.fromIntArraySeq arrayLength intSeq
                    return roll |> rollout.B
            }
        | RfU8 -> result {
                    let! roll = Uint8Roll.fromIntArraySeq arrayLength intSeq
                    return roll |> rollout.U8
                    }
        | RfU16 -> result {
                    let! roll = Uint16Roll.fromIntArraySeq arrayLength intSeq
                    return roll |> rollout.U16
                    }
        | RfI32 -> result {
                    let! roll = IntRoll.fromIntArraySeq arrayLength intSeq
                    return roll |> rollout.I32
                    }
        | RfU64 -> result {
                    let! roll = Uint64Roll.fromUint64Arrays arrayLength aas
                    return roll |> rollout.U64
                    }


    let fromBitPack (rolloutFormat:rolloutFormat) 
                    (arrayLength:arrayLength) 
                    (bitPack:bitPack) =
        match rolloutFormat with
        | RfB -> result {
                    let! roll = BooleanRoll.fromBitPack arrayLength bitPack
                    return roll |> rollout.B
                    }
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


    let toBitPack (symbolSetSize:symbolSetSize) 
                  (rollout:rollout) =
        match rollout with
        | B _uBRoll -> _uBRoll |> BooleanRoll.toBitPack
        | U8 _uInt8Roll -> _uInt8Roll |> Uint8Roll.toBitPack symbolSetSize
        | U16 _uInt16Roll -> _uInt16Roll |> Uint16Roll.toBitPack symbolSetSize
        | I32 _intRoll -> _intRoll |> IntRoll.toBitPack symbolSetSize
        | U64 _uInt64Roll -> _uInt64Roll |> Uint64Roll.toBitPack symbolSetSize
