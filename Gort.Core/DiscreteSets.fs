namespace global



type bitSet = private { values: bool[] }

module BitSet =

    let create (avs: bool[]) = { bitSet.values = avs }

    let create8 (b: uint8[]) =
        create (b |> Array.map (fun v -> v > 0uy)) |> Result.Ok

    let create16 (b: uint16[]) =
        create (b |> Array.map (fun v -> v > 0us)) |> Result.Ok

    let zeroCreate (count: int) =
        { bitSet.values = Array.zeroCreate count }

    let getValues (bitSet: bitSet) = bitSet.values

    let copy (v: bitSet) =
        { bitSet.values = Array.copy (v.values) }

    let isSorted (bitSet: bitSet) =
        CollectionProps.isSorted_inline bitSet.values

    let isZero (ibs: bitSet) = ibs.values |> Array.forall ((=) false)

    let fromInteger (ord: order) (intVers: int) =
        { bitSet.values = (intVers |> uint64) |> ByteUtils.uint64ToBoolArray ord }

    let toInteger (arrayVers: bitSet) =
        ByteUtils.boolArrayToInt arrayVers.values

    let fromUint64 (ord: order) (intVers: uint64) =
        { bitSet.values = intVers |> ByteUtils.uint64ToBoolArray ord }

    let toUint64 (bitSet: bitSet) =
        ByteUtils.boolArrayToUint64 bitSet.values

    let allBitsAsSeq (ord: order) =
        { 0 .. (1 <<< (Order.value ord)) - 1 } |> Seq.map (fun i -> fromInteger ord i)

    let allBitsAsArray (ord: order) =
        Array.init (1 <<< (Order.value ord)) (fromInteger ord)


    //*************************************************************
    //***************  byte conversions  **************************
    //*************************************************************

    //let fromBytes (ord:order) (data:byte[]) =
    //    result {

    //         let! ints =  ByteArray.convertBytesToInts data
    //         return create ints
    //    }

    //let toBytes (perm:bitSet) =
    //    ByteArray.convertIntsToBytes (perm.values)

    //*************************************************************
    //***************    IRando dependent   ***********************
    //*************************************************************

    let createRandom (order: order) (rando: IRando) =
        let perm =
            Array.init (Order.value order) (fun _ ->
                let q = rando.NextFloat
                if (q > 0.5) then true else false)

        { bitSet.values = perm }

    let createRandoms (order: order) (rnd: IRando) =
        seq {
            while true do
                yield createRandom order rnd
        }



type intSet = private { values: int[] }

module IntSet =

    let create (avs: int[]) = { intSet.values = avs }

    let create8 (b: uint8[]) =
        create (b |> Array.map (int)) |> Result.Ok

    let create16 (b: uint16[]) =
        create (b |> Array.map (int)) |> Result.Ok

    let zeroCreate (count: int) =
        { intSet.values = Array.zeroCreate count }

    let getValues (intSet: intSet) = intSet.values

    let copy (intSet: intSet) =
        { intSet.values = Array.copy (intSet.values) }

    let isSorted (intSet: intSet) =
        CollectionProps.isSorted_inline intSet.values

    let isTwoCycle (ist: intSet) = CollectionProps.isTwoCycle ist.values

    let isZero (ibs: intSet) = ibs.values |> Array.forall ((=) 0)

    let fromInteger (ord: order) (intVers: int) =
        { intSet.values = (intVers |> uint64) |> ByteUtils.uint64To2ValArray ord 0 1 }

    let toInteger (arrayVers: intSet) (oneThresh: int) =
        ByteUtils.thresholdArrayToInt arrayVers.values oneThresh

    let fromUint64 (ord: order) (intVers: uint64) =
        { intSet.values = intVers |> ByteUtils.uint64To2ValArray ord 0 1 }

    let toUint64 (intSt: intSet) (oneThresh: int) =
        ByteUtils.thresholdArrayToUint64 intSt.values oneThresh

    let allBitsAsSeq (ord: order) =
        { 0 .. (1 <<< (Order.value ord)) - 1 } |> Seq.map (fun i -> fromInteger ord i)

    let allBitsAsArray (ord: order) =
        Array.init (1 <<< (Order.value ord)) (fromInteger ord)


//*************************************************************
//***************  byte conversions  **************************
//*************************************************************

//let fromBytes (data:byte[]) =
//    result {

//         let! ints =  ByteArray.convertBytesToInts data
//         return create ints
//    }

//let toBytes (intSet:intSet) =
//    ByteArray.convertIntsToBytes (intSet.values)

//*************************************************************
//***************    IRando dependent   ***********************
//*************************************************************

//let createRandom (order:order) (rando:IRando) =
//    let perm =
//        Array.init (Order.value order)
//                    (fun _ -> let q = rando.NextFloat
//                              if (q > 0.5) then 1 else 0 )
//    {intSet.values = perm }

//let createRandoms (order:order)
//                  (rnd:IRando) =
//    seq { while true do
//            yield createRandom order rnd }


type intSet16 = private { values: uint16[] }

module IntSet16 =

    let create (avs: int[]) =
        { intSet16.values = avs |> Array.map (uint16) }

    let create16 (avs: uint16[]) = { intSet16.values = avs } |> Ok

    let create8 (b: uint8[]) =
        create (b |> Array.map (int)) |> Result.Ok

    let zeroCreate (count: int) =
        { intSet16.values = Array.zeroCreate count }

    let getValues (is16: intSet16) = is16.values

    let copy (intSet: intSet16) =
        { intSet16.values = Array.copy (intSet.values) }

    let isZero (ibs: intSet16) = ibs.values |> Array.forall ((=) 0us)

    let isSorted (intSet: intSet16) =
        CollectionProps.isSorted_inline intSet.values

    let isTwoCycle (is16: intSet16) =
        CollectionProps.isTwoCycle16 is16.values

    let sorted_O_1_Sequence (order: order) (onesCount: int) =
        let totalSize = (Order.value order)
        let numZeroes = totalSize - onesCount
        { intSet16.values = Array.init totalSize (fun i -> if i < numZeroes then 0us else 1us) }

    //Returns a bloclLen + 1 length array of IntBits
    // of all possible sorted 0-1 sequences of length order
    let sorted_0_1_Sequences (order: order) =
        seq {
            for i = 0 to (Order.value order) do
                yield (sorted_O_1_Sequence order i)
        }

    let fromInteger (ord: order) (intVers: int) =
        { intSet16.values = (intVers |> uint64) |> ByteUtils.uint64To2ValArray ord 0us 1us }

    let fromUint64 (ord: order) (intVers: uint64) =
        { intSet16.values = intVers |> ByteUtils.uint64To2ValArray ord 0us 1us }

    let toInteger (arrayVers: intSet16) (oneThresh: int) =
        ByteUtils.thresholdArrayToInt (arrayVers.values |> Array.map (int)) oneThresh

    let toUint64 (intSt: intSet16) (oneThresh: uint16) =
        ByteUtils.thresholdArrayToUint64 intSt.values oneThresh

    let allBitsAsSeq (order: order) =
        let dv = Order.value order
        { 0 .. (1 <<< dv) - 1 } |> Seq.map (fromInteger order)

    let allBitsAsArray (ord: order) =
        Array.init (1 <<< (Order.value ord)) (fromInteger ord)



//*************************************************************
//***************  byte conversions****************************
//*************************************************************

//let fromBytes (data:byte[]) =
//    result {

//         let! ints =  ByteArray.convertBytesToInts data
//         return create ints
//    }

//let toBytes (intSet16:intSet16) =
//    ByteArray.convertUint16sToBytes (intSet16.values)


//*************************************************************
//***************    IRando dependent   ***********************
//*************************************************************

//let createRandom (order:order) (rando:IRando) =
//    let perm =
//        Array.init (Order.value order)
//                    (fun _ -> let q = rando.NextFloat
//                              if (q > 0.5) then 1us else 0us )
//    {intSet16.values = perm }


//let createRandoms (order:order) (rnd:IRando) =
//    seq { while true do yield createRandom order rnd }



type intSet8 = private { values: uint8[] }

module IntSet8 =

    let create (avs: int[]) =
        { intSet8.values = avs |> Array.map (uint8) }

    let create8 (avs: uint8[]) = { intSet8.values = avs } |> Ok

    let create16 (b: uint16[]) =
        create (b |> Array.map (int)) |> Result.Ok

    let zeroCreate (count: int) =
        { intSet8.values = Array.zeroCreate count }

    let getValues (is8: intSet8) = is8.values

    let copy (intSet: intSet8) =
        { intSet8.values = Array.copy (intSet.values) }

    let isZero (ibs: intSet8) = ibs.values |> Array.forall ((=) 0uy)

    let isSorted (intSet: intSet8) =
        CollectionProps.isSorted_inline intSet.values

    let isTwoCycle (is8: intSet8) = CollectionProps.isTwoCycle8 is8.values

    let sorted_O_1_Sequence (order: order) (onesCount: int) =
        let totalSize = (Order.value order)
        let numZeroes = totalSize - onesCount
        { intSet8.values = Array.init totalSize (fun i -> if i < numZeroes then 0uy else 1uy) }

    //Returns a bloclLen + 1 length array of IntBits
    // of all possible sorted 0-1 sequences of length order
    let sorted_0_1_Sequences (order: order) =
        seq {
            for i = 0 to (Order.value order) do
                yield (sorted_O_1_Sequence order i)
        }

    let fromInteger (ord: order) (intVers: int) =
        { intSet8.values = (intVers |> uint64) |> ByteUtils.uint64To2ValArray ord 0uy 1uy }

    let fromUint64 (ord: order) (intVers: uint64) =
        { intSet8.values = intVers |> ByteUtils.uint64To2ValArray ord 0uy 1uy }

    let toInteger (arrayVers: intSet8) (oneThresh: int) =
        ByteUtils.thresholdArrayToInt (arrayVers.values |> Array.map (int)) oneThresh

    let toUint64 (intSt: intSet8) (oneThresh: uint8) =
        ByteUtils.thresholdArrayToUint64 intSt.values oneThresh

    let allBitsAsSeq (order: order) =
        let dv = Order.value order
        { 0 .. (1 <<< dv) - 1 } |> Seq.map (fromInteger order)

    let allBitsAsArray (ord: order) =
        Array.init (1 <<< (Order.value ord)) (fromInteger ord)



//*************************************************************
//***************  byte conversions****************************
//*************************************************************

//let fromBytes (data:byte[]) =
//    result {
//         let! ints =  ByteArray.convertBytesToUint8s data
//         return! create8 ints
//    }

//let toBytes (intSet8:intSet8) =
//    ByteArray.convertUint8sToBytes (intSet8.values)


//*************************************************************
//***************    IRando dependent   ***********************
//*************************************************************

//let createRandom (order:order) (rando:IRando) =
//    let perm =
//        Array.init (Order.value order)
//                    (fun _ -> let q = rando.NextFloat
//                              if (q > 0.5) then 1uy else 0uy )
//    {intSet8.values = perm }


//let createRandoms (order:order)
//                  (rnd:IRando) =
//    seq { while true do yield createRandom order rnd }
