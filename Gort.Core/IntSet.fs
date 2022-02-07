namespace global


type intSet = private { values:int[] }
module IntSet =

    let create (avs:int[]) = 
        {intSet.values = avs}

    let create8 (b:uint8[]) =
        create (b |> Array.map(int)) |> Result.Ok

    let create16 (b:uint16[]) =
        create (b |> Array.map(int)) |> Result.Ok

    let zeroCreate (count:int) = 
        { intSet.values = Array.create count 0 }

    let copy (intSet:intSet) = 
        {intSet.values = Array.copy (intSet.values) }
    
    let isSorted (intSet:intSet) =
        CollectionProps.isSorted_inline intSet.values

    let isTwoCycle (ist:intSet) =
        CollectionProps.isTwoCycle ist.values

    let isZero (ibs:intSet) = 
        ibs.values |> Array.forall((=) 0)

    let fromInteger (dg:degree) (intVers:int) =
        { intSet.values = ByteUtils.intToIntArray dg (intVers |> uint64) }

    let toInteger (arrayVers:intSet) (oneThresh:int) =
        ByteUtils.intArrayToInt arrayVers.values oneThresh
                
    let fromUint64 (dg:degree) (intVal:uint64) =
        { intSet.values = ByteUtils.uint64toIntArray dg intVal }
                
    let toUint64 (intSt:intSet) (oneThresh:int) = 
        ByteUtils.intArrayToUint64 intSt.values oneThresh

    let allForAsSeq (dg:degree) =
        { 0 .. (1 <<< (Degree.value dg)) - 1 }
        |> Seq.map (fun i -> fromInteger dg i)

    let allForAsArray (dg:degree) =
        let order = (Degree.value dg)
        Array.init (1 <<< order) (fun i -> fromInteger dg i)


//*************************************************************
//***************  byte conversions****************************
//*************************************************************

    let makeFromBytes (dg:degree) (data:byte[]) = 
        ByteArray.makeFromBytes dg create8 create16 data


    let makeArrayFromBytes (dg:degree) (data:byte[]) = 
        ByteArray.makeArrayFromBytes dg create8 create16 data


    let toBytes (perm:intSet) =
        ByteArray.toBytes (perm.values)


    let arrayToBytes (perms:intSet[]) =
        ByteArray.arrayToBytes (perms |> Array.map(fun p -> p.values))



//*************************************************************
//***************    IRando dependent   ***********************
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
        {intSet8.values = avs} |> Ok

    let create16 (b:uint16[]) =
        create (b |> Array.map(int)) |> Result.Ok

    let zeroCreate (count:int) = 
        { intSet8.values = 
                Array.create count 0uy }

    let copy (intSet:intSet8) = 
        {intSet8.values = Array.copy (intSet.values) }


    let isZero (ibs:intSet8) = 
        ibs.values |> Array.forall((=) 0uy)


    let isSorted (intSet:intSet8) =
        CollectionProps.isSorted_inline intSet.values

    let isTwoCycle (is8:intSet8) =
        CollectionProps.isTwoCycle8 is8.values

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

    let fromInteger (dg:degree) (intVers:int) =
        { intSet8.values = ByteUtils.intToIntArray8 dg (intVers |> uint64) }
        
    let toInteger (arrayVers:intSet8) (oneThresh:int) =
        ByteUtils.intArrayToInt (arrayVers.values |> Array.map(int)) oneThresh
            
    let fromUint64 (dg:degree) (intVal:uint64) =
        { intSet8.values = ByteUtils.intToIntArray8 dg intVal}
            
    let toUint64 (intSt:intSet8) (oneThresh:uint8) = 
        ByteUtils.int8ArrayToUint64 intSt.values oneThresh

    let allForSeq (degree:degree) =
        let dv = Degree.value degree 
        {0 .. (1 <<< dv) - 1} |> Seq.map (fromInteger degree)


    let allForArray (degree:degree) =
        let order = (Degree.value degree)
        Array.init (1 <<< order) (fromInteger degree)



//*************************************************************
//***************  byte conversions****************************
//*************************************************************


    let makeFromBytes (dg:degree) (data:byte[]) = 
        ByteArray.makeFromBytes dg create8 create16 data


    let makeArrayFromBytes (dg:degree) (data:byte[]) = 
        ByteArray.makeArrayFromBytes dg create8 create16 data


    let toBytes (perm:intSet) =
        ByteArray.toBytes (perm.values)


    let arrayToBytes (perms:intSet[]) =
        ByteArray.arrayToBytes (perms |> Array.map(fun p -> p.values))



//*************************************************************
//***************    IRando dependent   ***********************
//*************************************************************


    let createRandom (degree:degree) (rando:IRando) = 
        let perm = 
            Array.init (Degree.value degree)
                        (fun _ -> let q = rando.NextFloat
                                  if (q > 0.5) then 1uy else 0uy )
        {intSet8.values = perm }


    let createRandoms (degree:degree) 
                      (rnd:IRando) =
        seq { while true do yield createRandom degree rnd }


