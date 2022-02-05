namespace global
open System
open SysExt
open Microsoft.FSharp.Core
open System.Security.Cryptography
open System.IO


module Bitwise =
    
    let bitMask_uL (bitCt:int) = 
        (1uL <<< bitCt) - 1uL


    let IdMap_ints = 
        [|for deg=0 to 64 do 
                yield if deg = 0 then [||] else [| 0 .. deg - 1 |] |]


    let allSorted_uL =
        [for deg=0 to 63 do 
                yield bitMask_uL deg]


    let isSorted (bitRep:uint64) = 
        allSorted_uL |> List.contains bitRep


    let noRepeats (items:uint64[]) =
        items |> Array.distinct


    let toIntArray (dg:degree) (data:uint64) = 
        Array.init (Degree.value dg) data.intAt


    let fromIntArray (data:int[]) (oneThresh:int)= 
        let mutable rv = 0uL
        data |> Array.iteri(fun dex v -> if (v >= oneThresh) then rv <- rv.set dex)
        rv


    let allBitVersions (intVers:int[]) =
        seq { 0 .. (intVers.Length - 1) } |> Seq.map(fromIntArray intVers)
        

    let toUniqueBitVersions (intVersions:int[] seq) =
        intVersions |> Seq.map(allBitVersions) 
                    |> Seq.concat
                    |> Seq.distinct


    let mergeUp (lowDegree:degree) (lowVal:uint64) (hiVal:uint64) =
        (hiVal <<< (Degree.value lowDegree)) &&& lowVal


    let mergeUpSeq (lowDegree:degree) (lowVals:uint64 seq) (hiVals:uint64 seq) =
        let _mh (lv:uint64) =
            hiVals |> Seq.map(mergeUp lowDegree lv)
        lowVals |> Seq.map(_mh) |> Seq.concat


    let cratesFor (itemsPerCrate:int) (items:int) = 
        let fullCrates = items / itemsPerCrate
        let leftOvers = items % itemsPerCrate
        if (leftOvers = 0) then fullCrates else fullCrates + 1


    let allBitPackForDegree (degree:degree) =
        try
            let itemCt = degree |> Degree.binExp
            Array.init<uint64> itemCt (uint64) |> Ok
        with
            | ex -> ("error in allBitPackForDegree: " + ex.Message ) 
                    |> Result.Error


    let bitPacktoBitStripe (dg:degree) 
                           (stripeArray:uint64[]) 
                           (stripedOffset:int) 
                           (bitPos:int) 
                           (packedBits:uint64) =
        for i = 0 to (Degree.value dg) - 1 do
            if packedBits.isset i then
                stripeArray.[stripedOffset + i] <- 
                                stripeArray.[stripedOffset + i].set bitPos


    let bitPackedtoBitStriped (dg:degree)
                              (packedArray:uint64[]) =
        try
            let stripedArrayLength = cratesFor 64 packedArray.Length * (Degree.value dg)
            let stripedArray = Array.zeroCreate stripedArrayLength
            for i = 0 to packedArray.Length - 1 do
                let stripedOffset = (i / 64) * (Degree.value dg)
                let bitPos = i % 64
                packedArray.[i] |> bitPacktoBitStripe dg stripedArray stripedOffset bitPos
            stripedArray |> Ok
        with
            | ex -> ("error in bitPackedtoBitStriped: " + ex.Message ) |> Result.Error



    let bitPackedtoBitStriped2D (dg:degree)
                                (packedArray:uint64[]) =
        try
            let stripedArrayLength = cratesFor 64 packedArray.Length
            let stripedArray = Array2D.zeroCreate<uint64> (Degree.value dg) stripedArrayLength
            let mutable i = 0
            let mutable block = - 1
            while i < packedArray.Length do
                let mutable stripe = 0
                block <- block + 1
                while ((stripe < 64) && (i < packedArray.Length)) do
                    for j = 0 to (Degree.value dg) - 1 do
                        if packedArray.[i].isset j then
                            stripedArray.[j, block] <- stripedArray.[j, block].set stripe
                    i <- i + 1
                    stripe <- stripe + 1
            stripedArray |> Ok
        with
            | ex -> ("error in bitPackedtoBitStriped: " + ex.Message ) |> Result.Error



    let bitStripeToBitPack (dg:degree)
                           (packedArray:uint64[])
                           (stripeLoad:int)
                           (stripedOffset:int) 
                           (stripeArray:uint64)  =
        let packedArrayBtPos = (stripedOffset % (Degree.value dg))
        let packedArrayRootOffset = (stripedOffset / (Degree.value dg)) * 64
        for i = 0 to stripeLoad - 1 do
            if (stripeArray.isset i) then
                 packedArray.[packedArrayRootOffset + i]  <-  
                    packedArray.[packedArrayRootOffset + i].set packedArrayBtPos


    let bitStripedToBitPacked (dg:degree)
                              (itemCount:int) 
                              (stripedArray:uint64[]) =
        try
            let packedArray = Array.zeroCreate itemCount
            for i = 0 to stripedArray.Length - 1 do
                let stripeLoad = Math.Min(64, itemCount - (i / (Degree.value dg)) * 64)
                stripedArray.[i] |> bitStripeToBitPack dg packedArray stripeLoad i
            packedArray |> Ok
        with
            | ex -> ("error in bitPackedtoBitStriped: " + ex.Message ) |> Result.Error


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


    let filterByPickList (data:'a[]) (picks:bool[]) =
        try
            let pickCount = picks |> Array.map(fun v -> if v then 1 else 0)
                                  |> Array.sum
            let filtAr = Array.zeroCreate pickCount
            let mutable newDex = 0
            for i = 0 to (data.Length - 1) do
                if picks.[i] then 
                    filtAr.[newDex] <- data.[i] 
                    newDex <- newDex + 1
            filtAr |> Ok
        with
            | ex -> ("error in filterByPickList: " + ex.Message ) |> Result.Error


    /// ***********************************************************
    /// ****** degree dependent maps to byte arrays ***************
    /// ***********************************************************




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






module ByteUtils =

    let structHash (o:obj) =
        let s = sprintf "%A" o
        let inputBytes = System.Text.Encoding.ASCII.GetBytes(s);
        let md5 = MD5.Create();
        md5.ComputeHash(inputBytes)


    let hashObjs (oes:obj seq) =
        use stream = new MemoryStream()
        use writer = new BinaryWriter(stream)
        oes |> Seq.iter(fun o -> writer.Write(sprintf "%A" o))
        let md5 = MD5.Create();
        md5.ComputeHash(stream.ToArray())
        

    let trueBitCount32 (u32:uint) =
        let mutable tc = 0
        for i in 0 .. 31 do
            let qua = (u32 &&& (1u <<< i)) > 0u
            if qua then
                tc <- tc + 1
        tc


    let trueBitCount64 (u64:uint64) =
        let mutable tc = 0
        for i in 0 .. 63 do
            let qua = (u64 &&& (1UL <<< i)) > 0UL
            if qua then
                tc <- tc + 1
        tc


    let trueBitIndexes64 (u64:uint64) =
        seq {
                for i in 0 .. 63 do
                    if (u64 &&& (1UL <<< i)) > 0UL then
                        yield i
            }


    let stripeWrite (uBits:uint64[]) 
                    (intBits:int[]) 
                    (pos:int) = 
        let one = (1UL <<< pos)
        let proc dex =
            if (intBits.[dex] > 0) then
                uBits.[dex] <- 
                            uBits.[dex] ||| one
    
        for i=0 to (uBits.Length - 1) do
            proc i


    let stripeRead (uBits:uint64[]) 
                   (pos:int) = 
        let one = (1UL <<< pos)
        let proc dex v =
            if ((uBits.[dex] &&& one) > 0UL) then
                1
            else 0
        uBits |> Array.mapi (proc)
