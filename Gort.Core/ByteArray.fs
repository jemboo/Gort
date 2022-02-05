namespace global
open System
open SysExt
open Microsoft.FSharp.Core
open System.Security.Cryptography
open System.IO



module ByteArray =

    /// ***********************************************************
    /// ******** uint64 mapping from byte arrays ******************
    /// ***********************************************************

    let getUint64fromBytes (offset:int) (blob:byte[]) =
        try
            BitConverter.ToUInt64(blob, offset) |> Ok
        with
            | ex -> ("error in uInt64FromBytes: " + ex.Message ) |> Result.Error


    let getUint64arrayFromBytes (blob:byte[]) (arrayLen:int) (offset:int) =
        try
            let rollout = Array.zeroCreate<uint64> arrayLen
            for i = 0 to (arrayLen - 1) do
                rollout.[i] <- BitConverter.ToUInt64(blob, i * 8 + offset)
            rollout |> Ok
        with
            | ex -> ("error in uInt64sFromBytes: " + ex.Message ) |> Result.Error


    let mapUint64arrayToBytes (uintA:uint64[]) (offset:int) (blob:byte[]) =
        try
            for i = 0 to (uintA.Length - 1) do
                let bA = BitConverter.GetBytes uintA.[i]
                let blobOff = i * 8
                blob.[blobOff + offset] <- bA.[0]
                blob.[blobOff + offset + 1] <- bA.[1]
                blob.[blobOff + offset + 2] <- bA.[2]
                blob.[blobOff + offset + 3] <- bA.[3]
                blob.[blobOff + offset + 4] <- bA.[4]
                blob.[blobOff + offset + 5] <- bA.[5]
                blob.[blobOff + offset + 6] <- bA.[6]
                blob.[blobOff + offset + 7] <- bA.[7]
            blob |> Ok
        with
            | ex -> ("error in bytesFromUint64s: " + ex.Message ) |> Result.Error


    let mapUint64toBytes (uintV:uint64) (offset:int) (blob:byte[]) =
        try
            let bA = BitConverter.GetBytes uintV
            blob.[offset] <- bA.[0]
            blob.[offset + 1] <- bA.[1]
            blob.[offset + 2] <- bA.[2]
            blob.[offset + 3] <- bA.[3]
            blob.[offset + 4] <- bA.[4]
            blob.[offset + 5] <- bA.[5]
            blob.[offset + 6] <- bA.[6]
            blob.[offset + 7] <- bA.[7]
            blob |> Ok
        with
            | ex -> ("error in bytesFromUint64: " + ex.Message ) |> Result.Error


    /// ***********************************************************
    /// ******** uint32 mapping from byte arrays ******************
    /// ***********************************************************

    let getUint32FromBytes (offset:int) (blob:byte[]) =
        try
            BitConverter.ToUInt32(blob, offset) |> Ok
        with
            | ex -> ("error in uInt32FromBytes: " + ex.Message ) |> Result.Error


    let getUint32arrayFromBytes (blob:byte[]) (arrayLen:int) (offset:int) =
        try
            let rollout = Array.zeroCreate<uint32> arrayLen
            for i = 0 to (arrayLen - 1) do
                rollout.[i] <- BitConverter.ToUInt32(blob, i * 4 + offset)
            rollout |> Ok
        with
            | ex -> ("error in uInt32sFromBytes: " + ex.Message ) |> Result.Error


    let mapUint32arrayToBytes (uintA:uint32[]) (offset:int) (blob:byte[]) =
        try
            for i = 0 to (uintA.Length - 1) do
                let bA = BitConverter.GetBytes uintA.[i]
                let blobOff = i * 4
                blob.[blobOff + offset] <- bA.[0]
                blob.[blobOff + offset + 1] <- bA.[1]
                blob.[blobOff + offset + 2] <- bA.[2]
                blob.[blobOff + offset + 3] <- bA.[3]
            blob |> Ok
        with
            | ex -> ("error in bytesFromUint32s: " + ex.Message ) |> Result.Error


    let mapUint32toBytes (uintV:uint32) (offset:int) (blob:byte[]) =
        try
            let bA = BitConverter.GetBytes uintV
            blob.[offset] <- bA.[0]
            blob.[offset + 1] <- bA.[1]
            blob.[offset + 2] <- bA.[2]
            blob.[offset + 3] <- bA.[3]
            blob |> Ok
        with
            | ex -> ("error in bytesFromUint32: " + ex.Message ) |> Result.Error


    /// ***********************************************************
    /// ******** uint16 mapping from byte arrays ******************
    /// ***********************************************************

    let getUint16FromBytes (offset:int) (blob:byte[]) =
        try
            BitConverter.ToUInt16(blob, offset) |> Ok
        with
            | ex -> ("error in uInt16FromBytes: " + ex.Message ) |> Result.Error


    let getUint16arrayFromBytes (blob:byte[]) (arrayLen:int) (offset:int) =
        try
            let rollout = Array.zeroCreate<uint16> arrayLen
            for i = 0 to (arrayLen - 1) do
                rollout.[i] <- BitConverter.ToUInt16(blob, i * 2 + offset)
            rollout |> Ok
        with
            | ex -> ("error in uInt16sFromBytes: " + ex.Message ) |> Result.Error


    let mapUint16arrayToBytes (uintA:uint16[]) (offset:int) (blob:byte[]) =
        try
            for i = 0 to (uintA.Length - 1) do
                let bA = BitConverter.GetBytes uintA.[i]
                let blobOff = i * 2
                blob.[blobOff + offset] <- bA.[0]
                blob.[blobOff + offset + 1] <- bA.[1]
            blob |> Ok
        with
            | ex -> ("error in bytesFromUint16s: " + ex.Message ) |> Result.Error


    let mapUint16toBytes (uintV:uint16) (offset:int) (blob:byte[]) =
        try
            let bA = BitConverter.GetBytes uintV
            blob.[offset] <- bA.[0]
            blob.[offset + 1] <- bA.[1]
            blob |> Ok
        with
            | ex -> ("error in bytesFromUint16: " + ex.Message ) |> Result.Error



    /// ***********************************************************
    /// ******** uint8 mapping from byte arrays *******************
    /// ***********************************************************

    let getUint8FromBytes (offset:int) (blob:byte[]) =
        try
              blob.[offset] |> uint8 |> Ok
        with
            | ex -> ("error in uInt8FromBytes: " + ex.Message ) |> Result.Error


    let getUint8arrayFromBytes (blob:byte[]) 
                               (arrayLen:int) 
                               (offset:int) =
        try
            let rollout = Array.zeroCreate<uint8> arrayLen
            for i = 0 to (arrayLen - 1) do
                rollout.[i] <- blob.[i + offset]
            rollout |> Ok
        with
            | ex -> ("error in uInt8sFromBytes: " + ex.Message ) |> Result.Error


    let mapUint8arrayToBytes (uintA:uint8[]) 
                             (offset:int) 
                             (blob:byte[]) =
        try
            for i = 0 to (uintA.Length - 1) do
                blob.[offset + i] <- uintA.[i]
            blob |> Ok
        with
            | ex -> ("error in mapUint8arrayToBytes: " + ex.Message ) |> Result.Error


    let mapUint8toBytes (uintV:uint8) 
                        (offset:int) 
                        (blob:byte[]) =
        try
            blob.[offset] <- uintV
            blob |> Ok
        with
            | ex -> ("error in bytesFromUint8: " + ex.Message ) |> Result.Error



//*************************************************************
//********  degree dependent byte array conversions  *********
//*************************************************************

    let fromUint8<'T> (dg:degree) 
                      (ctor8:byte[] -> Result<'T, string>) 
                      (data:byte[]) = 
        try
            if (data.Length) % (Degree.value dg) <> 0 then
                "data length is incorrect for degree" |> Error
            else
                result {
                    let! permsR =
                        data |> Array.chunkBySize (Degree.value dg) 
                             |> Array.map(ctor8)
                             |> Array.toList
                             |> Result.sequence
                    return permsR |> List.toArray
                }
        with
          | ex -> ("error in permsFromUint8: " + ex.Message ) 
                  |> Result.Error


    let fromUint16s (dg:degree) 
                    (ctor16:uint16[] -> Result<'T, string>) 
                    (data:byte[]) = 
        try
            if (data.Length) % (2 * (Degree.value dg)) <> 0 then
                "data length is incorrect for degree" |> Error
            else
                result {
                    let! u16s = getUint16arrayFromBytes data (data.Length / 2) 0
                    let! permsR =
                        u16s |> Array.chunkBySize (Degree.value dg)
                             |> Array.map(ctor16)
                             |> Array.toList
                             |> Result.sequence
                    return permsR |> List.toArray
                }
        with
          | ex -> ("error in permsFromUint8: " + ex.Message ) 
                  |> Result.Error


    let makeFromBytes (dg:degree) 
                      (ctor8:uint8[] -> Result<'T, string>)  
                      (ctor16:uint16[] -> Result<'T, string>) 
                      (data:byte[]) = 
        match (Degree.value dg) with
        | x when (x < 256)  ->
              result {
                        let! u8s = getUint8arrayFromBytes data data.Length 0
                        return! ctor8 u8s
              }
        | x when (x < 256 * 256)  -> 
              result {
                let! u16s = getUint16arrayFromBytes data (data.Length / 2) 0
                return! ctor16 u16s
              }
        | _ -> "invalid degree" |> Error


    let makeArrayFromBytes (dg:degree)
                           (ctor8:uint8[] -> Result<'T, string>)  
                           (ctor16:uint16[] -> Result<'T, string>) 
                           (data:byte[]) = 
        match (Degree.value dg) with
        | x when (x < 256)  -> fromUint8 dg ctor8 data
        | x when (x < 256 * 256)  -> fromUint16s dg ctor16 data
        | _ -> "invalid degree" |> Error


    let toBytes (inst:int[]) =
        match (inst.Length) with
        | x when (x < 256)  -> 
              result {
                let uint8Array = inst |> Array.map(uint8)
                let data = Array.zeroCreate<byte> inst.Length
                return! data |>  mapUint8arrayToBytes uint8Array 0
              }
        | x when (x < 256 * 256)  -> 
              result {
                  let uint16Array = inst |> Array.map(uint16)
                  let data = Array.zeroCreate<byte> (inst.Length * 2)
                  return! data |>  mapUint16arrayToBytes uint16Array 0
              }
        | _ -> "invalid degree" |> Error


    let arrayToBytes (insts:int[][]) =
                result {
                    let! wak = insts |> Array.map(toBytes)
                                     |> Array.toList
                                     |> Result.sequence
                    return wak |> Array.concat
                }



    /// ***********************************************************
    /// ******** degree mapping from byte arrays ******************
    /// ***********************************************************

    let bytesToDegree (data:byte[]) =
        result {
            let! v = getUint16FromBytes 0 data
            return! v |> int |> Degree.create
        }

    let bytesToDegreeArray (data:byte[]) =
        result {
            if data.Length % 2 <> 0 then
                return!   "incorrect byte format for degreeArrayFromBytes" |> Error
            else
                let! vs = data |> Array.chunkBySize 2
                               |> Array.map(getUint16FromBytes 0)
                               |> Array.toList
                               |> Result.sequence

                return! vs |> List.map(int >> Degree.create)
                           |> Result.sequence
        }

    let degreeToBytes (data:byte[]) (offset:int) (dg:degree) =
        result {
            let uint16Value = (Degree.value dg) |> uint16
            return! data |>  mapUint16toBytes uint16Value offset
        }

    let degreeArrayToBytes (data:byte[]) (offset:int) (dgs:degree[]) =
        result {
            let uint16Array = dgs |> Array.map(Degree.value >> uint16)
            return! data |>  mapUint16arrayToBytes uint16Array offset
        }
