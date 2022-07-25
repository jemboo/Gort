namespace global
open System

type chunkCount = private RollLength of int
module ChunkCount = 
    let value (RollLength v) = v
    let create (value:int) =
        if (value > 0) then value |> RollLength |> Ok
        else "RollLength must be greater than 0" |> Error




type rollout = private { byteWidth:byteWidth; chunkCount:chunkCount; order:order; data:byte[] }
module Rollout =

    let indexLength (rollout:rollout) =
        (ByteWidth.value rollout.byteWidth) * (Order.value rollout.order)

    let getByteWidth (rollout:rollout) =
        rollout.byteWidth

    let getChunkCount (rollout:rollout) =
        rollout.chunkCount

    let getOrder (rollout:rollout) =
        rollout.order

    let getData (rollout:rollout) =
        rollout.data

    let create (byteWidth:byteWidth) (chunkCount:chunkCount) (order:order) =
        {
            rollout.byteWidth = byteWidth; chunkCount = chunkCount; order=order;
            data = Array.zeroCreate<byte> ((ByteWidth.value byteWidth) * 
                                           (ChunkCount.value chunkCount) * 
                                           (Order.value order))
        }

    let init (byteWidth:byteWidth) 
             (chunkCount:chunkCount) 
             (order:order) 
             (data:byte[]) =
        let bytesNeeded = (ByteWidth.value byteWidth) * (ChunkCount.value chunkCount) * (Order.value order)
        if (bytesNeeded <> data.Length) then
            "data size is incorrect" |> Error
        else
            {
                rollout.byteWidth = byteWidth; chunkCount = chunkCount; order=order;
                data = data
            } |> Ok


    let getUint8s (rollIndex:int) (dataOut:uint8[]) (rollout:rollout) =
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


    let setUint8s (rollIndex:int) (dataIn:uint8[]) (rollout:rollout) =
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


    let getUint16s (rollIndex:int) (dataOut:uint16[]) (rollout:rollout) =
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


    let setUint16s (rollIndex:int) (dataIn:uint16[]) (rollout:rollout) =
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


    let getUint32s (rollIndex:int) (dataOut:uint32[]) (rollout:rollout) =
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


    let setUint32s (rollIndex:int) (dataIn:uint32[]) (rollout:rollout) =
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


    let getUint64s (rollIndex:int) (dataOut:uint64[]) (rollout:rollout) =
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


    let setUint64s (rollIndex:int) (dataIn:uint64[]) (rollout:rollout) =
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