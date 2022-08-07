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


    //let inline yabba< ^a when ^a : (static member (<<<) : ^a * int -> ^a)> (qua:^a) i =
    //    qua &&& (1 <<< i) <> 0

    //let inline yab< ^a > (qua:^a) i =
    //    qua &&& (1 <<< i) <> 0


    //let inline yabba (qua: ^a when ^a : (static member isset : int -> bool)) =
    //            qua.isset 

    let inline descendants name (xml:^x) =
            (^x : (member Descendants : int -> seq<int>) (xml,name))

    let inline descendants2 name (xml:^x) =
            (^x : (member isset : int -> bool) (xml,name))

    let inline descendants3 (xml:^x) name  =
            (^x : (member isset : int -> bool) (xml,name))

    let inline descendants4 (ibts:^x when ^x : (member isset : int -> bool)) dex =
            (^x : (member isset : int -> bool) (ibts, dex))

    let inline walk_the_creature_2 (creature:^a when ^a:(member Walk : unit -> unit)) =
                (^a : (member Walk : unit -> unit) creature)

    let inline walk_the_creature (creature:^a when ^a:(member Walk2 : int->int)) =
                creature
                //(^a : (member Walk2 : int->int) creature 5)

    let inline yabb3 (qua: ^a when ^a : (static member (<<<) : ^a * int -> ^a)) =
        qua <<< 3


    let inline dabba (qua: ^a when ^a : (static member (<<<) : ^a * int -> ^a) and ^a : (static member (>>>) : ^a * int -> ^a)) =
        qua <<< 3


    let inline dodo (qua: ^a when ^a : (static member (<<<) : ^a * int -> ^a) and ^a : (static member (&&&) : ^a * ^a -> ^a)) =
        qua <<< 3



    let byteToBits (bitWidth:bitWidth) (v:byte) =
        let bw = bitWidth |> BitWidth.value
        seq { for i in 0 .. (bw - 1) -> v.isset i }


    let byteSeqToBits (bitWidth:bitWidth) (v:seq<byte>) =
        seq { for i in v do yield! byteToBits bitWidth i }


    let bitSeqToBytes (bitWidth:bitWidth) (bitsy:seq<bool>) =
        let bw = bitWidth |> BitWidth.value
        let _yab (_bs:seq<bool>) = 
            let mutable bRet = new byte()
            _bs |> Seq.iteri(fun dex v -> if v then (bRet <- bRet.set dex) |> ignore)
            bRet
        bitsy |> Seq.chunkBySize(bw)
              |> Seq.where(fun chunk -> chunk.Length = bw)
              |> Seq.map(_yab)


    let uint16ToBits (bitWidth:bitWidth) (v:uint16) =
        let bw = bitWidth |> BitWidth.value
        seq { for i in 0 .. (bw - 1) -> v.isset i }


    let uint16SeqToBits (bitWidth:bitWidth) (v:seq<uint16>) =
        let bw = bitWidth |> BitWidth.value
        seq { for i in v do yield! uint16ToBits bitWidth i }


    let bitSeqToUint16 (bitWidth:bitWidth) (bitsy:seq<bool>) =
        let bw = bitWidth |> BitWidth.value
        let _yab (_bs:seq<bool>) =
            let mutable bRet = new uint16()
            _bs |> Seq.iteri(fun dex v -> if v then (bRet <- bRet.set dex) |> ignore)
            bRet
        bitsy |> Seq.chunkBySize(bw)
              |> Seq.where(fun chunk -> chunk.Length = bw)
              |> Seq.map(_yab)


    let intToBits (bitWidth:bitWidth) (v:int) =
        let bw = bitWidth |> BitWidth.value
        seq { for i in 0 .. (bw - 1) -> v.isset i }


    let intSeqToBits (bitWidth:bitWidth) (v:seq<int>) =
        seq { for i in v do yield! intToBits bitWidth i }


    let bitSeqToInts (bitWidth:bitWidth) (bitsy:seq<bool>) =
        let bw = bitWidth |> BitWidth.value
        let _yab (_bs:seq<bool>) = 
            let mutable bRet = new int()
            _bs |> Seq.iteri(fun dex v -> if v then (bRet <- bRet.set dex) |> ignore)
            bRet
        bitsy |> Seq.chunkBySize(bw)
              |> Seq.where(fun chunk -> chunk.Length = bw)
              |> Seq.map(_yab)


    let uint64ToBits (bitWidth:bitWidth) (v:uint64) =
        let bw = bitWidth |> BitWidth.value
        seq { for i in 0 .. (bw - 1) -> v.isset i }


    let uint64SeqToBits (bitWidth:bitWidth) (v:seq<uint64>) =
        let bw = bitWidth |> BitWidth.value
        seq { for i in v do yield! uint64ToBits bitWidth i }


    let bitSeqToUint64 (bitWidth:bitWidth) (bitsy:seq<bool>) =
        let bw = bitWidth |> BitWidth.value
        let _yab (_bs:seq<bool>) =
            let mutable bRet = new uint64()
            _bs |> Seq.iteri(fun dex v -> if v then (bRet <- bRet.set dex) |> ignore)
            bRet
        bitsy |> Seq.chunkBySize(bw)
              |> Seq.where(fun chunk -> chunk.Length = bw)
              |> Seq.map(_yab)


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



    let uint64ToIntArray (order:order) (uint64:uint64) =
        Array.init (Order.value order) 
                   (fun dex -> if (uint64.isset dex) then 1 else 0)

    let allUint64s (symbolMod:int) (intVers:int[]) =
        let oneThresholds = seq { 0 .. (symbolMod - 1) }
        oneThresholds |> Seq.map(arrayToUint64 intVers)
        

    let toDistinctUint64s (symbolMod:int) (intVersions:int[] seq) =
        intVersions |> Seq.map(allUint64s symbolMod) 
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
            if values.[i] >= oneThresh then
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
         let sq = aSeq |> Seq.toArray
         aSeq |> Seq.chunkBySize 64
              |> Seq.map(writeStripeArray oneThresh ord)


    let createStripedArrayFromInts (ord:order) 
                                   (intSeq:int[] seq) =
        try
            intSeq |> toStripeArrays 1 ord 
                   |> Seq.concat
                   |> Seq.toArray |> Ok
        with
            | ex -> ("error in createStripedArray: " + ex.Message ) |> Result.Error


    let fromStripeArray<'a when 'a : equality> (zero_v:'a) 
                 (one_v:'a) (striped:uint64[]) =
        let order = striped.Length
        seq {
                for bit_pos = 0 to 63 do
                    yield Array.init 
                            order 
                            (fun stripe_dex -> 
                                if striped.[stripe_dex].get bit_pos 
                                    then one_v else zero_v)
            }


    let fromStripeArrays (zero_v:'a)
                         (one_v:'a)
                         (ord:order) 
                         (strSeq:uint64 seq) =
         strSeq |> Seq.chunkBySize (Order.value ord)
                |> Seq.map(fromStripeArray zero_v one_v)
                |> Seq.concat


    let usedStripeCount<'a when 'a : equality> (zero_v:'a ) (one_v:'a) (striped:uint64[]) =
        let _arrayIsZero (arr:'a[]) = 
            arr |> Array.forall(fun dexV -> dexV = zero_v)

        let _findFirstElement (cond:'a[]->bool) (arr:'a[][]) =
            let mutable carryOn = true
            let mutable dex = 0
            while (dex < (arr.Length - 1)) && carryOn do
                if cond arr.[dex] then
                    carryOn <- false
                else
                    dex <- dex + 1
            dex

        let stripeArrays = fromStripeArray zero_v one_v striped
                           |> Seq.toArray

        _findFirstElement _arrayIsZero stripeArrays