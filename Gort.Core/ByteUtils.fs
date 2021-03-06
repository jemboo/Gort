namespace global
open System
open SysExt
open Microsoft.FSharp.Core
open System.Security.Cryptography
open System.IO


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
        let md5 = MD5.Create()
        md5.ComputeHash(stream.ToArray())


    let IdMap_ints = 
        [|for deg=0 to 64 do 
                yield if deg = 0 then [||] else [| 0 .. deg - 1 |] |]

    let allSorted_uL =
        [for deg = 0 to 63 do yield (1uL <<< deg) - 1uL]

    let isSorted (bitRep:uint64) = 
        allSorted_uL |> List.contains bitRep


    let inline uint64To2ValArray< ^a> (ord:order)
                                      (truVal:^a) (falseVal:^a)  
                                      (d64:uint64) = 
        Array.init (Order.value ord) 
                   (fun dex -> if (d64.get dex) then truVal else falseVal)


    let intArrayToInt (data:int[]) (oneThresh:int) = 
        let mutable rv = 0
        data |> Array.iteri(fun dex v -> if (v >= oneThresh) then rv <- rv.set dex)
        rv

    let inline arrayToUint64< ^a when ^a: comparison>  (array:^a[]) (oneThresh:^a) =
        let mutable rv = 0uL
        array |> Array.iteri(fun dex v -> if (v >= oneThresh) then rv <- rv.set dex)
        rv

    let allUint64s  (intVers:int[]) =
        let oneThresholds = seq { 0 .. (intVers.Length - 1) }
        oneThresholds |> Seq.map(arrayToUint64 intVers)
        
    let toDistinctUint64s (intVersions:int[] seq) =
        intVersions |> Seq.map(allUint64s) 
                    |> Seq.concat
                    |> Seq.distinct

    let mergeUp (lowDegree:order) (lowVal:uint64) (hiVal:uint64) =
        (hiVal <<< (Order.value lowDegree)) &&& lowVal

    let mergeUpSeq (lowDegree:order) (lowVals:uint64 seq) (hiVals:uint64 seq) =
        let _mh (lv:uint64) =
            hiVals |> Seq.map(mergeUp lowDegree lv)
        lowVals |> Seq.map(_mh) |> Seq.concat



/// ***********************************************************
/// ***************  bitstriped <-> uint64  *******************
/// ***********************************************************

    let uint64toBitStripe (ord:order) 
                           (stripeArray:uint64[]) 
                           (stripedOffset:int) 
                           (bitPos:int) 
                           (packedBits:uint64) =
        for i = 0 to (Order.value ord) - 1 do
            if packedBits.isset i then
                stripeArray.[stripedOffset + i] <- 
                    stripeArray.[stripedOffset + i].set bitPos


    let uint64ArraytoBitStriped (ord:order)
                                (packedArray:uint64[]) =
        try
            let stripedArrayLength = CollectionProps.cratesFor 
                                        64 packedArray.Length * (Order.value ord)
            let stripedArray = Array.zeroCreate stripedArrayLength
            for i = 0 to packedArray.Length - 1 do
                let stripedOffset = (i / 64) * (Order.value ord)
                let bitPos = i % 64
                packedArray.[i] |> uint64toBitStripe ord stripedArray stripedOffset bitPos
            stripedArray |> Ok
        with
            | ex -> ("error in bitPackedtoBitStriped: " + ex.Message ) |> Result.Error


    let uint64ArraytoBitStriped2D (ord:order)
                                  (packedArray:uint64[]) =
        try
            let stripedArrayLength = CollectionProps.cratesFor 
                                        64 packedArray.Length
            let stripedArray = Array2D.zeroCreate<uint64> (Order.value ord) stripedArrayLength
            let mutable i = 0
            let mutable block = - 1
            while i < packedArray.Length do
                let mutable stripe = 0
                block <- block + 1
                while ((stripe < 64) && (i < packedArray.Length)) do
                    for j = 0 to (Order.value ord) - 1 do
                        if packedArray.[i].isset j then
                            stripedArray.[j, block] <- stripedArray.[j, block].set stripe
                    i <- i + 1
                    stripe <- stripe + 1
            stripedArray |> Ok
        with
            | ex -> ("error in uint64ArraytoBitStriped2D: " + ex.Message ) |> Result.Error


    let bitStripeToUint64 (ord:order)
                          (packedArray:uint64[])
                          (stripeLoad:int)
                          (stripedOffset:int) 
                          (stripeArray:uint64)  =
        let packedArrayBtPos = (stripedOffset % (Order.value ord))
        let packedArrayRootOffset = (stripedOffset / (Order.value ord)) * 64
        for i = 0 to stripeLoad - 1 do
            if (stripeArray.isset i) then
                 packedArray.[packedArrayRootOffset + i]  <-  
                    packedArray.[packedArrayRootOffset + i].set packedArrayBtPos


    let bitStripedToUint64array (ord:order)
                                (itemCount:int) 
                                (stripedArray:uint64[]) =
        try
            let packedArray = Array.zeroCreate itemCount
            for i = 0 to stripedArray.Length - 1 do
                let stripeLoad = Math.Min(64, itemCount - (i / (Order.value ord)) * 64)
                stripedArray.[i] |> bitStripeToUint64 ord packedArray stripeLoad i
            packedArray |> Ok
        with
            | ex -> ("error in bitStripedToUint64array: " + ex.Message ) |> Result.Error



/// ***********************************************************
/// ***************  bitstriped <-> 'a[]  *********************
/// ***********************************************************

    let inline writeStripe< ^a when ^a: comparison> 
                                    (oneThresh:^a)  
                                    (values:^a[]) 
                                    (stripePos:int)
                                    (stripedArray:uint64[]) =
        for i = 0 to values.Length - 1 do
            if values.[i] < oneThresh then
                stripedArray.[i] <- 
                        stripedArray.[i].set stripePos
            
            
    let inline writeStripeArray< ^a when ^a: comparison> 
                                    (oneThresh:^a)  
                                    (ord:order) 
                                    (aValues:^a[][]) =
        let stripedArray = Array.zeroCreate<uint64> (Order.value ord)
        for i = 0 to aValues.Length - 1 do
            writeStripe oneThresh aValues.[i] i stripedArray
        stripedArray


    let inline toStripeArrays< ^a when ^a: comparison> 
                                    (oneThresh:^a)  
                                    (ord:order) 
                                    (aSeq:^a[] seq) =
         aSeq |> Seq.chunkBySize 64
              |> Seq.map(writeStripeArray oneThresh ord)


    let fromStripeArray (zero_v:'a) (one_v:'a) (striped:uint64[])  =
        seq {
                for i = 0 to 63 do
                    yield Array.init 
                            striped.Length 
                            (fun dex -> if striped.[dex].get i then zero_v else one_v)
            }

    let fromStripeArrays (zero_v:'a)
                         (one_v:'a)
                         (ord:order) 
                         (strSeq:uint64 seq) =
         strSeq |> Seq.chunkBySize (Order.value ord)
                |> Seq.map(fromStripeArray zero_v one_v)
                |> Seq.concat