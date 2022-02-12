namespace global
open System
open SysExt
open Microsoft.FSharp.Core




module ByteArray =

    /// ***********************************************************
    /// *************** int array mapping *************************
    /// ***********************************************************



    let mapIntArrays (src_offset:int) (src:int[]) (dest_offset:int) (dest:int[]) (src_Ct:int) =
            Buffer.BlockCopy(src, src_offset * 4, dest, dest_offset * 4, src_Ct * 4)

    let mapUint8Arrays (src_offset:int) (src:uint8[]) (dest_offset:int) (dest:uint8[]) (src_Ct:int) =
            Buffer.BlockCopy(src, src_offset, dest, dest_offset, src_Ct)

    let mapUint16Arrays (src_offset:int) (src:uint16[]) (dest_offset:int) (dest:uint16[]) (src_Ct:int) =
            Buffer.BlockCopy(src, src_offset * 2, dest, dest_offset * 2, src_Ct * 2)

    let mapUint32Arrays (src_offset:int) (src:uint32[]) (dest_offset:int) (dest:uint32[]) (src_Ct:int) =
            Buffer.BlockCopy(src, src_offset * 4, dest, dest_offset * 4, src_Ct * 4)

    let mapUint64Arrays (src_offset:int) (src:uint64[]) (dest_offset:int) (dest:uint64[]) (src_Ct:int) =
            Buffer.BlockCopy(src, src_offset * 8, dest, dest_offset * 8, src_Ct * 8)


    /// ***********************************************************
    /// ******** uint64 mapping from byte arrays ******************
    /// ***********************************************************

    let getUint64fromBytes (offset:int) (blob:byte[]) =
        try
            BitConverter.ToUInt64(blob, offset) |> Ok
        with
            | ex -> ("error in uInt64FromBytes: " + ex.Message ) |> Result.Error


    let mapBytesToUint64s (blob_offset:int) (uintA:uint64[]) (uintA_offset:int) (blobLen:int) (blob:byte[]) =
        try
            Buffer.BlockCopy(blob, blob_offset, uintA, uintA_offset*8, blobLen)
            uintA |> Ok
        with
            | ex -> ("error in uInt64sFromBytes: " + ex.Message ) |> Result.Error


    let convertBytesToUint64s (blob:byte[]) =
        try
            let uints = Array.zeroCreate<uint64> (blob.Length / 8)
            blob |> mapBytesToUint64s 0 uints 0 blob.Length
        with
            | ex -> ("error in convertBytesToUint64s: " + ex.Message ) |> Result.Error


    let mapUint64sToBytes (uintA_offset:int) (uint_ct:int) (blob:byte[]) (blob_offset:int) (uintA:uint64[]) =
        try
            Buffer.BlockCopy(uintA, uintA_offset*8, blob, blob_offset, uint_ct * 8) 
            blob |> Ok
        with
            | ex -> ("error in bytesFromUint64s: " + ex.Message ) |> Result.Error


    let convertUint64sToBytes (uintA:uint64[]) =
        try
            let blob = Array.zeroCreate<byte> (uintA.Length * 8)
            uintA |> mapUint64sToBytes 0 uintA.Length blob 0
        with
            | ex -> ("error in convertUint64sToBytes: " + ex.Message ) |> Result.Error



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



    let mapBytesToUint16s (blob_offset:int) (uintA:uint16[]) (uintA_offset:int) (uint_ct:int) (blob:byte[]) =
        try
            Buffer.BlockCopy(blob, blob_offset, uintA, uintA_offset * 2, uint_ct)
            uintA |> Ok
        with
            | ex -> ("error in uInt64sFromBytes: " + ex.Message ) |> Result.Error


    let convertBytesToUint16s (blob:byte[]) =
        try
            let uints = Array.zeroCreate<uint16> blob.Length
            blob |> mapBytesToUint16s 0 uints 0 blob.Length
        with
            | ex -> ("error in convertBytesToUint64s: " + ex.Message ) |> Result.Error


    //let getUint16arrayFromBytes (blob:byte[]) (arrayLen:int) (offset:int) =
    //    try
    //        let rollout = Array.zeroCreate<uint16> arrayLen
    //        for i = 0 to (arrayLen - 1) do
    //            rollout.[i] <- BitConverter.ToUInt16(blob, i * 2 + offset)
    //        rollout |> Ok
    //    with
    //        | ex -> ("error in uInt16sFromBytes: " + ex.Message ) |> Result.Error


    //let mapUint16arrayToBytes (uintA:uint16[]) (offset:int) (blob:byte[]) =
    //    try
    //        for i = 0 to (uintA.Length - 1) do
    //            let bA = BitConverter.GetBytes uintA.[i]
    //            let blobOff = i * 2
    //            blob.[blobOff + offset] <- bA.[0]
    //            blob.[blobOff + offset + 1] <- bA.[1]
    //        blob |> Ok
    //    with
    //        | ex -> ("error in bytesFromUint16s: " + ex.Message ) |> Result.Error


    //let mapUint16toBytes (uintV:uint16) (offset:int) (blob:byte[]) =
    //    try
    //        let bA = BitConverter.GetBytes uintV
    //        blob.[offset] <- bA.[0]
    //        blob.[offset + 1] <- bA.[1]
    //        blob |> Ok
    //    with
    //        | ex -> ("error in bytesFromUint16: " + ex.Message ) |> Result.Error



    let mapUint16sToBytes (uintA_offset:int) (uint_ct:int) (blob:byte[]) (blob_offset:int) (uintA:uint16[]) =
        try
            Buffer.BlockCopy(uintA, uintA_offset * 2, blob, blob_offset, uint_ct * 2) 
            blob |> Ok
        with
            | ex -> ("error in mapUint8sToBytes: " + ex.Message ) |> Result.Error


    let convertUint16sToBytes (uintA:uint16[]) =
        try
            let blob = Array.zeroCreate<byte> (uintA.Length)
            uintA |> mapUint16sToBytes 0 uintA.Length blob 0
        with
            | ex -> ("error in convertUint8sToBytes: " + ex.Message ) |> Result.Error




    /// ***********************************************************
    /// ******** uint8 mapping from byte arrays *******************
    /// ***********************************************************

    let getUint8FromBytes (offset:int) (blob:byte[]) =
        try
              blob.[offset] |> uint8 |> Ok
        with
            | ex -> ("error in uInt8FromBytes: " + ex.Message ) |> Result.Error


    let mapBytesToUint8s (blob_offset:int) (uintA:uint8[]) (uintA_offset:int) (uint_ct:int) (blob:byte[]) =
        try
            Buffer.BlockCopy(blob, blob_offset, uintA, uintA_offset, uint_ct)
            uintA |> Ok
        with
            | ex -> ("error in uInt64sFromBytes: " + ex.Message ) |> Result.Error


    let convertBytesToUint8s (blob:byte[]) =
        try
            let uints = Array.zeroCreate<uint8> blob.Length
            blob |> mapBytesToUint8s 0 uints 0 blob.Length
        with
            | ex -> ("error in convertBytesToUint64s: " + ex.Message ) |> Result.Error


    let mapUint8sToBytes (uintA_offset:int) (uint_ct:int) (blob:byte[]) (blob_offset:int) (uintA:uint8[]) =
        try
            Buffer.BlockCopy(uintA, uintA_offset, blob, blob_offset, uint_ct) 
            blob |> Ok
        with
            | ex -> ("error in mapUint8sToBytes: " + ex.Message ) |> Result.Error


    let convertUint8sToBytes (uintA:uint8[]) =
        try
            let blob = Array.zeroCreate<byte> (uintA.Length)
            uintA |> mapUint8sToBytes 0 uintA.Length blob 0
        with
            | ex -> ("error in convertUint8sToBytes: " + ex.Message ) |> Result.Error



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
                    let! u16s = convertBytesToUint16s data
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


    //let makeFromBytes (dg:degree) 
    //                  (ctor8:uint8[] -> Result<'T, string>)  
    //                  (ctor16:uint16[] -> Result<'T, string>) 
    //                  (data:byte[]) = 
    //    match (Degree.value dg) with
    //    | x when (x < 256)  ->
    //          result {
    //                    let! u8s = convertBytesToUint8s data
    //                    return! ctor8 u8s
    //          }
    //    | x when (x < 256 * 256)  -> 
    //          result {
    //            let! u16s = convertBytesToUint16s data
    //            return! ctor16 u16s
    //          }
    //    | _ -> "invalid degree" |> Error


    //let makeArrayFromBytes (dg:degree)
    //                       (ctor8:uint8[] -> Result<'T, string>)  
    //                       (ctor16:uint16[] -> Result<'T, string>) 
    //                       (data:byte[]) = 
    //    match (Degree.value dg) with
    //    | x when (x < 256)  -> fromUint8 dg ctor8 data
    //    | x when (x < 256 * 256)  -> fromUint16s dg ctor16 data
    //    | _ -> "invalid degree" |> Error


    //let toBytes (inst:int[]) =
    //    match (inst.Length) with
    //    | x when (x < 256)  -> 
    //          result {
    //            let uint8Array = inst |> Array.map(uint8)
    //            return! convertBytesToUint8s uint8Array
    //          }
    //    | x when (x < 256 * 256)  -> 
    //          result {
    //              let uint16Array = inst |> Array.map(uint16)
    //              let data = Array.zeroCreate<byte> (inst.Length * 2)
    //              return! data |>  mapUint16arrayToBytes uint16Array 0
    //          }
    //    | _ -> "invalid degree" |> Error


    //let arrayToBytes (insts:int[][]) =
    //            result {
    //                let! wak = insts |> Array.map(toBytes)
    //                                 |> Array.toList
    //                                 |> Result.sequence
    //                return wak |> Array.concat
    //            }



    /// ***********************************************************
    /// ******** degree list <-> byte arrays ******************
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
            return! [| uint16Value |] |> mapUint16sToBytes 0 1 data offset
        }

    let degreeArrayToBytes (data:byte[]) (offset:int) (dgs:degree[]) =
        result {
            let uint16Array = dgs |> Array.map(Degree.value >> uint16)
            return! uint16Array |>  mapUint16sToBytes 0 uint16Array.Length data offset
        }
