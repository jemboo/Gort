namespace global
open System
open SysExt
open Microsoft.FSharp.Core


module ByteArray =

    /// ***********************************************************
    /// *************** int< ^a >[] <->  int< ^a >[]  *************
    /// ***********************************************************

    let copyIntArray (src_offset:int) (src:int[]) (dest_offset:int) 
                     (dest:int[]) (src_Ct:int) =
            Buffer.BlockCopy(src, src_offset * 4, dest, dest_offset * 4, src_Ct * 4)

    let getIntArray(src_offset:int) (src_Ct:int) (src:int[]) =
        let aout = Array.zeroCreate<int> src_Ct
        copyIntArray src_offset src 0 aout src_Ct
        aout

    let copyUint8Array (src_offset:int) (src:uint8[]) (dest_offset:int) 
                       (dest:uint8[]) (src_Ct:int) =
            Buffer.BlockCopy(src, src_offset, dest, dest_offset, src_Ct)

    let getUintArray(src_offset:int) (src_Ct:int) (src:uint8[]) =
        let aout = Array.zeroCreate<uint8> src_Ct
        copyUint8Array src_offset src 0 aout src_Ct
        aout

    let copyUint16Array (src_offset:int) (src:uint16[]) (dest_offset:int) 
                        (dest:uint16[]) (src_Ct:int) =
            Buffer.BlockCopy(src, src_offset * 2, dest, dest_offset * 2, src_Ct * 2)

    let getUint16Array(src_offset:int) (src_Ct:int) (src:uint16[]) =
        let aout = Array.zeroCreate<uint16> src_Ct
        copyUint16Array src_offset src 0 aout src_Ct
        aout

    let copyUint32Array (src_offset:int) (src:uint32[]) (dest_offset:int) 
                        (dest:uint32[]) (src_Ct:int) =
            Buffer.BlockCopy(src, src_offset * 4, dest, dest_offset * 4, src_Ct * 4)

    let getUint32Array(src_offset:int) (src_Ct:int) (src:uint32[]) =
        let aout = Array.zeroCreate<uint32> src_Ct
        copyUint32Array src_offset src 0 aout src_Ct
        aout

    let copyUint64Array (src_offset:int) (src:uint64[]) (dest_offset:int) 
                        (dest:uint64[]) (src_Ct:int) =
            Buffer.BlockCopy(src, src_offset * 8, dest, dest_offset * 8, src_Ct * 8)

    let getUint64Array(src_offset:int) (src_Ct:int) (src:uint64[]) =
        let aout = Array.zeroCreate<uint64> src_Ct
        copyUint64Array src_offset src 0 aout src_Ct
        aout



    /// ***********************************************************
    /// ******************   int[] <-> byte[]  ********************
    /// ***********************************************************

    let getIntfromBytes (offset:int) (blob:byte[]) =
        try
            BitConverter.ToUInt64(blob, offset) |> Ok
        with
            | ex -> ("error in getIntfromBytes: " + ex.Message ) |> Result.Error


    let mapBytesToInts (blob_offset:int) (uintA:int[]) (uintA_offset:int) (blobLen:int) (blob:byte[]) =
        try
            Buffer.BlockCopy(blob, blob_offset, uintA, uintA_offset*4, blobLen)
            uintA |> Ok
        with
            | ex -> ("error in mapBytesToInts: " + ex.Message ) |> Result.Error


    let convertBytesToInts (blob:byte[]) =
        try
            let uints = Array.zeroCreate<int> (blob.Length / 4)
            blob |> mapBytesToInts 0 uints 0 blob.Length
        with
            | ex -> ("error in convertBytesToInts: " + ex.Message ) |> Result.Error


    let mapIntsToBytes (uintA_offset:int) (uint_ct:int) (blob:byte[]) 
                       (blob_offset:int) (uintA:int[]) =
        try
            Buffer.BlockCopy(uintA, uintA_offset*8, blob, blob_offset, uint_ct * 4) 
            blob |> Ok
        with
            | ex -> ("error in mapIntsToBytes: " + ex.Message ) |> Result.Error


    let convertIntsToBytes (uintA:int[]) =
        try
            let blob = Array.zeroCreate<byte> (uintA.Length * 4)
            uintA |> mapIntsToBytes 0 uintA.Length blob 0
        with
            | ex -> ("error in convertIntsToBytes: " + ex.Message ) |> Result.Error



    /// ***********************************************************
    /// ****************  uint64[] <-> byte[]   *******************
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
    /// ****************** uint32[] <-> byte[] ********************
    /// ***********************************************************

    let getUint32FromBytes (offset:int) (blob:byte[]) =
        try
            BitConverter.ToUInt32(blob, offset) |> Ok
        with
            | ex -> ("error in uInt32FromBytes: " + ex.Message ) |> Result.Error


    let mapBytesToUint32s (blob_offset:int) (uintA:uint32[]) (uintA_offset:int) (uint_ct:int) (blob:byte[]) =
        try
            Buffer.BlockCopy(blob, blob_offset, uintA, uintA_offset * 4, uint_ct)
            uintA |> Ok
        with
            | ex -> ("error in mapBytesToUint32s: " + ex.Message ) |> Result.Error


    let convertBytesToUint32s (blob:byte[]) =
        try
            let uints = Array.zeroCreate<uint32> (blob.Length / 4)
            blob |> mapBytesToUint32s 0 uints 0 blob.Length
        with
            | ex -> ("error in convertBytesToUint32s: " + ex.Message ) |> Result.Error

    
    let mapUint32sToBytes (uintA_offset:int) (uint_ct:int) (blob:byte[]) (blob_offset:int) (uintA:uint32[]) =
        try
            Buffer.BlockCopy(uintA, uintA_offset * 4, blob, blob_offset, uint_ct * 4) 
            blob |> Ok
        with
            | ex -> ("error in mapUint32sToBytes: " + ex.Message ) |> Result.Error


    let convertUint32sToBytes (uintA:uint32[]) =
        try
            let blob = Array.zeroCreate<byte> (uintA.Length * 4)
            uintA |> mapUint32sToBytes 0 uintA.Length blob 0
        with
            | ex -> ("error in convertUint32sToBytes: " + ex.Message ) |> Result.Error



    /// ***********************************************************
    /// ****************** uint16[] <-> byte[] ********************
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
            | ex -> ("error in mapBytesToUint16s: " + ex.Message ) |> Result.Error


    let convertBytesToUint16s (blob:byte[]) =
        try
            let uints = Array.zeroCreate<uint16> (blob.Length / 2)
            blob |> mapBytesToUint16s 0 uints 0 blob.Length
        with
            | ex -> ("error in convertBytesToUint16s: " + ex.Message ) |> Result.Error


    let mapUint16sToBytes (uintA_offset:int) (uint_ct:int) (blob:byte[]) (blob_offset:int) (uintA:uint16[]) =
        try
            Buffer.BlockCopy(uintA, uintA_offset * 2, blob, blob_offset, uint_ct * 2) 
            blob |> Ok
        with
            | ex -> ("error in mapUint16sToBytes: " + ex.Message ) |> Result.Error


    let convertUint16sToBytes (uintA:uint16[]) =
        try
            let blob = Array.zeroCreate<byte> (uintA.Length * 2)
            uintA |> mapUint16sToBytes 0 uintA.Length blob 0
        with
            | ex -> ("error in convertUint16sToBytes: " + ex.Message ) |> Result.Error




    /// ***********************************************************
    /// ******************  uint8[] <-> byte[] ********************
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
            | ex -> ("error in mapBytesToUint8s: " + ex.Message ) |> Result.Error


    let convertBytesToUint8s (blob:byte[]) =
        try
            let uints = Array.zeroCreate<uint8> blob.Length
            blob |> mapBytesToUint8s 0 uints 0 blob.Length
        with
            | ex -> ("error in convertBytesToUint8s: " + ex.Message ) |> Result.Error


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
//********  order dependent byte array conversions  ***********
//*************************************************************

    let fromUint8<'T> (ord:order) 
                      (ctor8:byte[] -> Result<'T, string>) 
                      (data:byte[]) = 
        try
            if (data.Length) % (Order.value ord) <> 0 then
                "data length is incorrect for order" |> Error
            else
                result {
                    let! permsR =
                        data |> Array.chunkBySize (Order.value ord) 
                             |> Array.map(ctor8)
                             |> Array.toList
                             |> Result.sequence
                    return permsR |> List.toArray
                }
        with
          | ex -> ("error in permsFromUint8: " + ex.Message ) 
                  |> Result.Error


    let fromUint16s (ord:order) 
                    (ctor16:uint16[] -> Result<'T, string>) 
                    (data:byte[]) = 
        try
            if (data.Length) % (2 * (Order.value ord)) <> 0 then
                "data length is incorrect for order" |> Error
            else
                result {
                    let! u16s = convertBytesToUint16s data
                    let! permsR =
                        u16s |> Array.chunkBySize (Order.value ord)
                             |> Array.map(ctor16)
                             |> Array.toList
                             |> Result.sequence
                    return permsR |> List.toArray
                }
        with
          | ex -> ("error in permsFromUint8: " + ex.Message ) 
                  |> Result.Error


    //let makeFromBytes (ord:order) 
    //                  (ctor8:uint8[] -> Result<'T, string>)  
    //                  (ctor16:uint16[] -> Result<'T, string>) 
    //                  (data:byte[]) = 
    //    match (Order.value ord) with
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
    //    | _ -> "invalid order" |> Error


    //let makeArrayFromBytes (ord:order)
    //                       (ctor8:uint8[] -> Result<'T, string>)  
    //                       (ctor16:uint16[] -> Result<'T, string>) 
    //                       (data:byte[]) = 
    //    match (Order.value ord) with
    //    | x when (x < 256)  -> fromUint8 ord ctor8 data
    //    | x when (x < 256 * 256)  -> fromUint16s ord ctor16 data
    //    | _ -> "invalid order" |> Error


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
    //    | _ -> "invalid order" |> Error


    //let arrayToBytes (insts:int[][]) =
    //            result {
    //                let! wak = insts |> Array.map(toBytes)
    //                                 |> Array.toList
    //                                 |> Result.sequence
    //                return wak |> Array.concat
    //            }



    /// ******************************************************
    /// ******** order list <-> byte arrays ******************
    /// ******************************************************

    let bytesToOrder (data:byte[]) =
        result {
            let! v = getUint16FromBytes 0 data
            return! v |> int |> Order.create
        }

    let bytesToOrderArray (data:byte[]) =
        result {
            if data.Length % 2 <> 0 then
                return!   "incorrect byte format for orderArrayFromBytes" |> Error
            else
                let! vs = data |> Array.chunkBySize 2
                               |> Array.map(getUint16FromBytes 0)
                               |> Array.toList
                               |> Result.sequence

                return! vs |> List.map(int >> Order.create)
                           |> Result.sequence
        }

    let orderToBytes (data:byte[]) (offset:int) (ord:order) =
        result {
            let uint16Value = (Order.value ord) |> uint16
            return! [| uint16Value |] |> mapUint16sToBytes 0 1 data offset
        }

    let orderArrayToBytes (data:byte[]) (offset:int) (dgs:order[]) =
        result {
            let uint16Array = dgs |> Array.map(Order.value >> uint16)
            return! uint16Array |>  mapUint16sToBytes 0 uint16Array.Length data offset
        }
