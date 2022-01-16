namespace global
open System
open SysExt

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




    let rollout (data:uint64[]) (dg:degree) =
        result {
            let arrayLen = (cratesFor data.Length 64) * (Degree.value dg)
            return Array.init<uint64> arrayLen (uint64)
        }


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