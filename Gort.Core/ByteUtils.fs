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

    
    // creates a bit stream from a byte stream by selecting the first bitsPerSymbol bits.
    let bitsFromSpBytePositions (bitsPerSymbol:bitsPerSymbol) (byteSeq:seq<byte>) =
        let bw = bitsPerSymbol |> BitsPerSymbol.value
        let _byteToBits (v:byte) =
            seq { for i in 0 .. (bw - 1) -> v.isset i }
        seq { for bite in byteSeq do yield! _byteToBits bite }
    
    // Creates a bit stream from each bit in a byte stream
    let getAllBitsFromByteSeq (byteSeq:seq<byte>) =
        let _byteBits (v:byte) =
            seq { for i in 0 .. 7 -> v.isset i }
        seq { for bite in byteSeq do yield! _byteBits bite }

    // maps a bit stream to the first bitsPerSymbol in a generated stream of byte
    // The way this is used, the last chunk may padding - in this case it is smaller 
    // than bitsPerSymbol, and it is ignored
    let bitsToSpBytePositions (bitsPerSymbol:bitsPerSymbol) (bitsy:seq<bool>) =
        let bw = bitsPerSymbol |> BitsPerSymbol.value
        let _yab (_bs:seq<bool>) = 
            let mutable bRet = new byte()
            _bs |> Seq.iteri(fun dex v -> if v then (bRet <- bRet.set dex) |> ignore)
            bRet
        bitsy |> Seq.chunkBySize(bw)
              |> Seq.where(fun chunk -> chunk.Length = bw)
              |> Seq.map(_yab)

    // Creates a byte stream from a bit stream by filling each byte
    let storeBitSeqInBytes (bitsy:seq<bool>) =
        let _yab (_bs:seq<bool>) = 
            let mutable bRet = new byte()
            _bs |> Seq.iteri(fun dex v -> if v then (bRet <- bRet.set dex) |> ignore)
            bRet
        bitsy |> Seq.chunkBySize(8)
              |> Seq.map(_yab)


    // creates a bit stream from a uint16 stream by selecting the first bitsPerSymbol bits.
    let bitsFromSpUint16Positions (bitsPerSymbol:bitsPerSymbol) (v:seq<uint16>) =

        let bw = bitsPerSymbol |> BitsPerSymbol.value
        let _uint16ToBits (bitsPerSymbol:bitsPerSymbol) (v:uint16) =
            seq { for i in 0 .. (bw - 1) -> v.isset i }
        seq { for i in v do yield! _uint16ToBits bitsPerSymbol i }

    // maps a bit stream to the first bitsPerSymbol in a generated stream of uint16
    // The way this is used, the last chunk may padding - in this case it is smaller 
    // than bitsPerSymbol, and it is ignored
    let bitsToSpUint16Positions (bitsPerSymbol:bitsPerSymbol) (bitsy:seq<bool>) =
        let bw = bitsPerSymbol |> BitsPerSymbol.value
        let _yab (_bs:seq<bool>) =
            let mutable bRet = new uint16()
            _bs |> Seq.iteri(fun dex v -> if v then (bRet <- bRet.set dex) |> ignore)
            bRet
        bitsy |> Seq.chunkBySize(bw)
              |> Seq.where(fun chunk -> chunk.Length = bw)
              |> Seq.map(_yab)

    // creates a bit stream from a int stream by selecting the first bitsPerSymbol bits.
    let bitsFromSpIntPositions (bitsPerSymbol:bitsPerSymbol) (v:seq<int>) =
        let bw = bitsPerSymbol |> BitsPerSymbol.value
        let _intToBits (v:int) =
            seq { for i in 0 .. (bw - 1) -> v.isset i }
        seq { for i in v do yield! _intToBits i }


    // maps a bit stream to the first bitsPerSymbol in a generated stream of int
    // The way this is used, the last chunk may padding - in this case it is smaller 
    // than bitsPerSymbol, and it is ignored
    let bitsToSpIntPositions (bitsPerSymbol:bitsPerSymbol) (bitsy:seq<bool>) =
        let bw = bitsPerSymbol |> BitsPerSymbol.value
        let _yab (_bs:seq<bool>) = 
            let mutable bRet = new int()
            _bs |> Seq.iteri(fun dex v -> if v then (bRet <- bRet.set dex) |> ignore)
            bRet
        bitsy |> Seq.chunkBySize(bw)
              |> Seq.where(fun chunk -> chunk.Length = bw)
              |> Seq.map(_yab)

    
    // creates a bit stream from a uint64 stream by selecting the first bitsPerSymbol bits.
    let bitsFromSpUint64Positions (bitsPerSymbol:bitsPerSymbol) (v:seq<uint64>) =
        let bw = bitsPerSymbol |> BitsPerSymbol.value
        let _uint64ToBits (v:uint64) =
            seq { for i in 0 .. (bw - 1) -> v.isset i }
        seq { for i in v do yield! _uint64ToBits i }


    // maps a bit stream to the first bitsPerSymbol in a generated stream of uint64
    // The way this is used, the last chunk may padding - in this case it is smaller 
    // than bitsPerSymbol, and it is ignored
    let bitsToSpUint64Positions (bitsPerSymbol:bitsPerSymbol) 
                                       (bitsy:seq<bool>) =
        let bw = bitsPerSymbol |> BitsPerSymbol.value
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

    let isUint64Sorted (bitRep:uint64) = 
        allSorted_uL |> List.contains bitRep


    let inline uint64To2ValArray< ^a> (ord:order)
                                      (truVal:^a) (falseVal:^a)  
                                      (d64:uint64) = 
        Array.init (Order.value ord) 
                   (fun dex -> if (d64.get dex) then truVal else falseVal)

                   
    let inline uint64ToBoolArray (ord:order)
                                 (d64:uint64) = 
        Array.init (Order.value ord) 
                   (fun dex -> if (d64.get dex) then true else false)


    let inline thresholdArrayToInt< ^a when ^a: comparison>  (array:^a[]) (oneThresh:^a) =
        let mutable rv = 0
        array |> Array.iteri(fun dex v -> if (v >= oneThresh) then rv <- rv.set dex)
        rv

    let inline thresholdArrayToUint64< ^a when ^a: comparison>  (array:^a[]) (oneThresh:^a) =
        let mutable rv = 0uL
        array |> Array.iteri(fun dex v -> if (v >= oneThresh) then rv <- rv.set dex)
        rv


    let uint64ToIntArray (order:order) (uint64:uint64) =
        Array.init (Order.value order) 
                   (fun dex -> if (uint64.isset dex) then 1 else 0)

    let allUint64s (symbolMod:int) (intVers:int[]) =
        let oneThresholds = seq { 0 .. (symbolMod - 1) }
        oneThresholds |> Seq.map(thresholdArrayToUint64 intVers)
        

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

    // stripePos <=64
    // if values.Length = stripedArray.Length
    let writeStripeFromBitArray
                                (values:bool[]) 
                                (stripePos:int)
                                (stripedArray:uint64[]) =
        for i = 0 to values.Length - 1 do
            if values.[i] then
                stripedArray.[i] <- 
                        stripedArray.[i].set stripePos


    // for 2d arrays bool[a][b] where a <= 64 and b = order
    // returns uint64[b]
    let makeStripedArrayFrom2dBoolArray
                                    (ord:order) 
                                    (twoDvals:bool[][]) =
        let stripedArray = Array.zeroCreate<uint64> (Order.value ord)
        for i = 0 to twoDvals.Length - 1 do
            writeStripeFromBitArray twoDvals.[i] i stripedArray
        stripedArray


    // for 2d arrays bool[a][b] where a <= 64 and b = order
    // returns uint64[c] where c <= (a / 64)
    let makeStripedArrayFromBoolArrays
                       (ord:order) 
                       (boolSeq:bool[] seq) =

         let _makeStripedArrayFromBoolArray
                                        (ord:order) 
                                        (twoDvals:bool[][]) =
            let stripedArray = Array.zeroCreate<uint64> (Order.value ord)
            for i = 0 to twoDvals.Length - 1 do
                writeStripeFromBitArray twoDvals.[i] i stripedArray
            stripedArray

         boolSeq |> Seq.chunkBySize 64
              |> Seq.map(_makeStripedArrayFromBoolArray ord)



    let inline writeStripeO< ^a when ^a: comparison> 
                                    (oneThresh:^a)  
                                    (values:^a[]) 
                                    (stripePos:int)
                                    (stripedArray:uint64[]) =
        for i = 0 to values.Length - 1 do
            if values.[i] >= oneThresh then
                stripedArray.[i] <- 
                        stripedArray.[i].set stripePos
            
            
    let inline writeStripeArrayO< ^a when ^a: comparison> 
                                    (oneThresh:^a)  
                                    (ord:order) 
                                    (aValues:^a[][]) =
        let stripedArray = Array.zeroCreate<uint64> (Order.value ord)
        for i = 0 to aValues.Length - 1 do
            writeStripeO oneThresh aValues.[i] i stripedArray
        stripedArray


    let inline toStripeArraysO< ^a when ^a: comparison> 
                                    (oneThresh:^a)  
                                    (ord:order) 
                                    (aSeq:^a[] seq) =
         aSeq |> Seq.chunkBySize 64
              |> Seq.map(writeStripeArrayO oneThresh ord)


    let createStripedArrayFromIntArrays 
                                   (ord:order) 
                                   (intSeq:int[] seq) =
        try
            intSeq |> toStripeArraysO 1 ord 
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


    let usedStripeCount  (striped:uint64[]) =
        let _arrayIsNotZero (arr:bool[]) = 
            not (arr |> Array.forall(fun dexV -> dexV = false))

        let stripeArrays = fromStripeArray false true striped
                           |> Seq.toArray

        stripeArrays |> Seq.filter(_arrayIsNotZero) |> Seq.length
