namespace global
open System


type intSet = private { values:int[] }
module IntSet =

    let create (avs:int[]) = 
        {intSet.values = avs}

    let zeroCreate (count:int) = 
        { intSet.values = Array.create count 0 }

    let create8 (b:uint8[]) =
        create (b |> Array.map(int)) |> Result.Ok

    let create16 (b:uint16[]) =
        create (b |> Array.map(int)) |> Result.Ok

    let copy (intSet:intSet) = 
        {intSet.values = Array.copy (intSet.values) }

    let isZero (ibs:intSet) = 
        ibs.values |> Array.forall((=) 0)


    let fromInteger (dg:degree) (intVers:int) =
        { intSet.values = ByteUtils.intToIntArray dg intVers }

    let toInteger (arrayVers:intSet) (oneThresh:int) =
        ByteUtils.intArrayToInt arrayVers.values oneThresh
                
    let fromUint64 (dg:degree) (intVal:int) =
        { intSet.values = ByteUtils.intToIntArray dg intVal }
                
    let toUint64 (intSt:intSet) (oneThresh:int) = 
        ByteUtils.intArrayToUint64 intSt.values oneThresh

    let allForAsSeq (dg:degree) =
        { 0 .. (1 <<< (Degree.value dg)) - 1 }
        |> Seq.map (fun i -> fromInteger dg i)

    let allForAsArray (dg:degree) =
        let order = (Degree.value dg)
        Array.init (1 <<< order) (fun i -> ByteUtils.intToIntArray dg i)


//*************************************************************
//*************************************************************
//*************************************************************

    let createRandom (degree:degree) (rando:IRando) = 
        let perm = 
            Array.init (Degree.value degree)
                        (fun _ -> let q = rando.NextFloat
                                  if (q > 0.5) then 1 else 0 )
        {intSet.values = perm }


    let createRandoms (degree:degree) 
                      (rnd:IRando) =
        seq { while true do 
                yield createRandom degree rnd }




type intSet8 = private { values:uint8[] }
module IntSet8 =

    let create (avs:int[]) = 
        {intSet8.values = avs |> Array.map(uint8)}


    let create8 (avs:uint8[]) = 
        {intSet8.values = avs}

    let zeroCreate (count:int) = 
        { intSet8.values = 
                Array.create count 0uy }


    let copy (intSet:intSet8) = 
        {intSet8.values = Array.copy (intSet.values) }


    let isZero (ibs:intSet8) = 
        ibs.values |> Array.forall((=) 0uy)


    let isSorted (intSet:intSet8) =
        CollectionProps.isSorted_inline intSet.values


    let sorted_O_1_Sequence (degree:degree) 
                            (onesCount:int) =
        let totalSize = (Degree.value degree)
        let numZeroes = totalSize - onesCount
        { intSet8.values = Array.init totalSize 
                    (fun i -> if i< numZeroes then 0uy else 1uy)}

    //Returns a bloclLen + 1 length array of IntBits
    // of all possible sorted 0-1 sequences of length degree
    let sorted_0_1_Sequences (degree:degree)  =
        seq { for i = 0 to (Degree.value degree) do 
                yield (sorted_O_1_Sequence degree i) }


    let stack (lowTohi: intSet8 seq) =
        lowTohi |> Seq.map(fun bs->bs.values)
                |> Seq.concat
                |> Seq.toArray
                |> create8

    let comboStack (subSeqs: intSet8[] seq) =
        let rec _cart LL =
            match LL with
            | [] -> Seq.singleton []
            | L::Ls -> seq {for x in L do for xs in _cart Ls -> x::xs}
        _cart (subSeqs |> Seq.toList) |> Seq.map(stack)


    let stackSortedBlocks (blockSizes:degree seq) =
        blockSizes |> Seq.map(sorted_0_1_Sequences >> Seq.toArray)
                   |> comboStack


    let fromInteger (len:int) (intVers:int) =
        let bitLoc (loc:int) (intBits:int) =
            if (((1 <<< loc) &&& intBits) <> 0) then 1 else 0
        { intSet8.values = 
                    Array.init len 
                               (fun i -> bitLoc i intVers |> uint8) }


    let toInteger (arrayVers:intSet8) =
        let mutable intRet = 0
        let _bump i =
            intRet <- intRet * 2
            if (arrayVers.values.[i] = 1uy) then
                intRet <- intRet + 1
        for i in (arrayVers.values.Length - 1) .. -1 .. 0 do
            _bump i
        intRet

                
    let fromUint64 (len:int) (intVers:int) =
        let bitLoc (loc:int) (intBits:int) =
            if (((1 <<< loc) &&& intBits) <> 0) then 1 else 0
        { intSet8.values = 
            Array.init len (fun i -> bitLoc i intVers |> uint8) }
                
                
    let toUint64 (arrayVers:intSet8) =
        let mutable intRet = 0UL
        let _bump i =
            intRet <- intRet * 2UL
            if (arrayVers.values.[i] = 1uy) then
                intRet <- intRet + 1UL 
        for i in (arrayVers.values.Length - 1) .. -1 .. 0 do
            _bump i
        intRet


    let seqOfAllFor (degree:degree) =
        let dv = Degree.value degree 
        {0 .. (1 <<< dv) - 1}
        |> Seq.map (fun i -> fromInteger dv i)


    let arrayOfAllFor (degree:degree) =
        let order = (Degree.value degree)
        Array.init (1 <<< order) (fun i -> fromInteger order i)


    let createRandom (degree:degree) (rando:IRando) = 
        let perm = 
            Array.init (Degree.value degree)
                        (fun _ -> let q = rando.NextFloat
                                  if (q > 0.5) then 1uy else 0uy )
        {intSet8.values = perm }


    let createRandoms (degree:degree) 
                      (rnd:IRando) =
        seq { while true do 
                yield createRandom degree rnd }


