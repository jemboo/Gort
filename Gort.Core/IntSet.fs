namespace global
open System



type intSet = private { values:int[] }
module IntSet =

    let create (avs:int[]) = 
        {intSet.values = avs}


    let zeroCreate (count:int) = 
        { intSet.values = 
                Array.create count 0 }


    let copy (intSet:intSet) = 
        {intSet.values = Array.copy (intSet.values) }


    let isZero (ibs:intSet) = 
        ibs.values |> Array.forall((=) 0)


    let isSorted (intSet:intSet) =
        CollectionProps.isSorted_inline intSet.values


    let sorted_O_1_Sequence (degree:degree) 
                            (onesCount:int) =
        let totalSize = (Degree.value degree)
        let numZeroes = totalSize - onesCount
        { intSet.values = Array.init totalSize 
                    (fun i -> if i< numZeroes then 0 else 1)}


    //Returns a bloclLen + 1 length array of IntBits
    // of all possible sorted 0-1 sequences of length degree
    let sorted_0_1_Sequences (degree:degree)  =
        seq { for i = 0 to (Degree.value degree) do 
                yield (sorted_O_1_Sequence degree i) }


    let stack (lowTohi: intSet seq) =
        lowTohi |> Seq.map(fun bs->bs.values)
                |> Seq.concat
                |> Seq.toArray
                |> create

    let comboStack (subSeqs: intSet[] seq) =
        let rec _cart LL =
            match LL with
            | [] -> Seq.singleton []
            | L::Ls -> seq {for x in L do for xs in _cart Ls -> x::xs}
        _cart (subSeqs |> Seq.toList) |> Seq.map(stack)


    let stackSortedBlocks (blockSizes:degree seq) =
        blockSizes |> Seq.map(sorted_0_1_Sequences >> Seq.toArray)
                   |> comboStack


//*************************************************************
//***************  byte conversions  **************************
//*************************************************************

    let create8 (b:uint8[]) =
        create (b |> Array.map(int)) |> Result.Ok

    let create16 (b:uint16[]) =
        create (b |> Array.map(int)) |> Result.Ok
    
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
                    let! u16s = Bitwise.getUint16arrayFromBytes data (data.Length / 2) 0
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
                        let! u8s = Bitwise.getUint8arrayFromBytes data data.Length 0
                        return! ctor8 u8s
              }
        | x when (x < 256 * 256)  -> 
              result {
                let! u16s = Bitwise.getUint16arrayFromBytes data (data.Length / 2) 0
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


    let toBytes (inst:intSet) =
        match (inst.values.Length) with
        | x when (x < 256)  -> 
              result {
                let uint8Array = inst.values |> Array.map(uint8)
                let data = Array.zeroCreate<byte> inst.values.Length
                return! data |>  Bitwise.mapUint8arrayToBytes uint8Array 0
              }
        | x when (x < 256 * 256)  -> 
              result {
                  let uint16Array = inst.values |> Array.map(uint16)
                  let data = Array.zeroCreate<byte> (inst.values.Length * 2)
                  return! data |>  Bitwise.mapUint16arrayToBytes uint16Array 0
              }
        | _ -> "invalid degree" |> Error


    let arrayToBytes (insts:intSet[]) =
                result {
                    let! wak = insts |> Array.map(toBytes)
                                     |> Array.toList
                                     |> Result.sequence
                    return wak |> Array.concat
                }


    let fromInteger (len:int) (intVers:int) =
        let bitLoc (loc:int) (intBits:int) =
            if (((1 <<< loc) &&& intBits) <> 0) then 1 else 0
        { intSet.values = 
                    Array.init len 
                               (fun i -> bitLoc i intVers) }

    let toInteger (arrayVers:intSet) =
        let mutable intRet = 0
        let _bump i =
            intRet <- intRet * 2
            if (arrayVers.values.[i] = 1) then
                intRet <- intRet + 1
        for i in (arrayVers.values.Length - 1) .. -1 .. 0 do
            _bump i
        intRet

                
    let fromUint64 (len:int) (intVers:int) =
        let bitLoc (loc:int) (intBits:int) =
            if (((1 <<< loc) &&& intBits) <> 0) then 1 else 0
        { intSet.values = 
                    Array.init len 
                                (fun i -> bitLoc i intVers) }
                
                
    let toUint64 (arrayVers:intSet) =
        let mutable intRet = 0UL
        let _bump i =
            intRet <- intRet * 2UL
            if (arrayVers.values.[i] = 1) then
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


