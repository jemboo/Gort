namespace global
open System

type rollLength = private RollLength of int
module RollLength = 
    let value (RollLength v) = v
    let create (value:int) =
        if (value > 0) then value |> RollLength |> Ok
        else "RollLength must be greater than 0" |> Error


type rollCount = private RollCount of int
module RollCount = 
    let value (RollCount v) = v
    let create (value:int) =
        if (value > 0) then value |> RollCount |> Ok
        else "RollCount must be greater than 0" |> Error



type rollout = private { rollWidth:byteWidth; rollLength:rollLength; rollCount:rollCount; data:byte[] }
module Rollout =

    let byteLength (rollout:rollout) =
        (ByteWidth.value rollout.rollWidth) * (RollLength.value rollout.rollLength)

    let getRollWidth (rollout:rollout) =
        rollout.rollWidth

    let getRollLength (rollout:rollout) =
        rollout.rollLength

    let getRollCount (rollout:rollout) =
        rollout.rollCount

    let getData (rollout:rollout) =
        rollout.data

    let create (rollWidth:byteWidth) (rollLength:rollLength) (rollCount:rollCount) =
        {
            rollout.rollWidth = rollWidth; rollLength = rollLength; rollCount=rollCount;
            data = Array.zeroCreate<byte> ((ByteWidth.value rollWidth) * 
                                           (RollLength.value rollLength) * 
                                           (RollCount.value rollCount))
        }

    let init (rollWidth:byteWidth) (rollLength:rollLength) (rollCount:rollCount) (data:byte[]) =
        let bytesNeeded = (ByteWidth.value rollWidth) * (RollLength.value rollLength) * (RollCount.value rollCount)
        if (bytesNeeded <> data.Length) then
            "data size is incorrect" |> Error
        else
            {
                rollout.rollWidth = rollWidth; rollLength = rollLength; rollCount=rollCount;
                data = data
            } |> Ok


    let getUint8s (rollIndex:int) (dataOut:uint8[]) (rollout:rollout) =
        let internalOffset = (byteLength rollout) * rollIndex
        match (ByteWidth.value rollout.rollWidth) with
        | x when (x = 1)  ->
            Buffer.BlockCopy(rollout.data, internalOffset, dataOut, 0, (byteLength rollout))
        | x when (x = 2)  -> 
            "invalid rollWidth for getUint8s" |> failwith
        | x when (x = 4)  ->
            "invalid rollWidth for getUint8s" |> failwith
        | x when (x = 8)  -> 
            "invalid rollWidth for getUint8s" |> failwith
        | _ ->  "invalid rollWidth" |> failwith


    let setUint8s (rollIndex:int) (dataIn:uint8[]) (rollout:rollout) =
        let internalOffset = (byteLength rollout) * rollIndex
        match (ByteWidth.value rollout.rollWidth) with
        | x when (x = 1)  ->
            Buffer.BlockCopy(dataIn, 0, rollout.data, internalOffset, (byteLength rollout))
        | x when (x = 2)  -> 
            "invalid rollWidth for setUint8s" |> failwith
        | x when (x = 4)  ->
            "invalid rollWidth for setUint8s" |> failwith
        | x when (x = 8)  -> 
            "invalid rollWidth for setUint8s" |> failwith
        | _ ->  "invalid rollWidth" |> failwith


    let getUint16s (rollIndex:int) (dataOut:uint16[]) (rollout:rollout) =
        let internalOffset = (byteLength rollout) * rollIndex
        match (ByteWidth.value rollout.rollWidth) with
        | x when (x = 1)  ->
            "invalid rollWidth for getUint16s" |> failwith
        | x when (x = 2)  -> 
             Buffer.BlockCopy(rollout.data, internalOffset, dataOut, 0, (byteLength rollout))
        | x when (x = 4)  ->
            "invalid rollWidth for getUint16s" |> failwith
        | x when (x = 8)  -> 
            "invalid rollWidth for getUint16s" |> failwith
        | _ ->  "invalid rollWidth" |> failwith


    let setUint16s (rollIndex:int) (dataIn:uint16[]) (rollout:rollout) =
        let internalOffset = (byteLength rollout) * rollIndex
        match (ByteWidth.value rollout.rollWidth) with
        | x when (x = 1)  ->
            "invalid rollWidth for setUint16s" |> failwith
        | x when (x = 2)  -> 
            Buffer.BlockCopy(dataIn, 0, rollout.data, internalOffset, (byteLength rollout))
        | x when (x = 4)  ->
            "invalid rollWidth for setUint16s" |> failwith
        | x when (x = 8)  -> 
            "invalid rollWidth for setUint16s" |> failwith
        | _ ->  "invalid rollWidth" |> failwith


    let getUint32s (rollIndex:int) (dataOut:uint32[]) (rollout:rollout) =
        let internalOffset = (byteLength rollout) * rollIndex
        match (ByteWidth.value rollout.rollWidth) with
        | x when (x = 1)  ->
            "invalid rollWidth for Uint32" |> failwith
        | x when (x = 2)  -> 
            "invalid rollWidth for Uint32" |> failwith
        | x when (x = 4)  ->
            Buffer.BlockCopy(rollout.data, internalOffset, dataOut, 0, (byteLength rollout))
        | x when (x = 8)  -> 
            "invalid rollWidth for Uint32" |> failwith
        | _ ->  "invalid rollWidth" |> failwith


    let setUint32s (rollIndex:int) (dataIn:uint32[]) (rollout:rollout) =
        let internalOffset = (byteLength rollout) * rollIndex
        match (ByteWidth.value rollout.rollWidth) with
        | x when (x = 1)  ->
            "invalid rollWidth for setUint32s" |> failwith
        | x when (x = 2)  -> 
            "invalid rollWidth for setUint32s" |> failwith
        | x when (x = 4)  ->
            Buffer.BlockCopy(dataIn, 0, rollout.data, internalOffset, (byteLength rollout))
        | x when (x = 8)  -> 
            "invalid rollWidth for setUint32s" |> failwith
        | _ ->  "invalid rollWidth" |> failwith


    let getUint64s (rollIndex:int) (dataOut:uint64[]) (rollout:rollout) =
        let internalOffset = (byteLength rollout) * rollIndex
        match (ByteWidth.value rollout.rollWidth) with
        | x when (x = 1)  ->
            "invalid rollWidth for Uint64" |> failwith
        | x when (x = 2)  -> 
            "invalid rollWidth for Uint64" |> failwith
        | x when (x = 4)  ->
            "invalid rollWidth for Uint64" |> failwith
        | x when (x = 8)  -> 
            Buffer.BlockCopy(rollout.data, internalOffset, dataOut, 0, (byteLength rollout))
        | _ ->  "invalid rollWidth" |> failwith


    let setUint64s (rollIndex:int) (dataIn:uint64[]) (rollout:rollout) =
        let internalOffset = (byteLength rollout) * rollIndex
        match (ByteWidth.value rollout.rollWidth) with
        | x when (x = 1)  ->
            "invalid rollWidth for setUint64s" |> failwith
        | x when (x = 2)  -> 
            "invalid rollWidth for setUint64s" |> failwith
        | x when (x = 4)  ->
            "invalid rollWidth for setUint64s" |> failwith
        | x when (x = 8)  -> 
            Buffer.BlockCopy(dataIn, 0, rollout.data, internalOffset, (byteLength rollout))
        | _ ->  "invalid rollWidth" |> failwith