namespace global

// a permutation of the set {0, 1,.. (degree-1)}
type permutation = private {values:int[] }
module Permutation =

    let create (vals:int[]) =
        if CollectionProps.isPermutation vals then
             { permutation.values = vals} |> Ok
        else
        "not a permutation" |> Error

    let create8 (b:uint8[]) =
        create (b |> Array.map(int))

    let create16 (b:uint16[]) =
        create (b |> Array.map(int))

    let getArray (perm:permutation) = perm.values
        
    let getDegree (perm:permutation) =
        Degree.createNr perm.values.Length

    let rotate (degree:degree) (dir:int) = 
        let d = (Degree.value degree)
        { values=Array.init d (fun i-> (i + dir) % d)}


    let powers (perm:permutation)  =
        perm.values |> CollectionOps.allPowers
                    |> Seq.map(fun vs -> {permutation.values = vs})

    let conjugate (conj:permutation)  (pA:permutation) =
        result {
            let! res = CollectionOps.conjIntArrays 
                                (pA |> getArray)
                                (conj |> getArray)
            return create res
        }

    let cyclicGroup (degree:degree) = 
        let r1 = rotate degree 1
        powers r1 |> Seq.toArray

    let identity (degree:degree) = 
        { values= CollectionProps.identity (Degree.value degree)}

    let toIntSet (perm:permutation) =
        {intSet.values = perm.values}

    let toIntSet8 (perm:permutation) =
        {intSet8.values = perm.values |> Array.map(uint8)}

    let inRange (degree:degree) (value:int) =
       ((value > -1) && (value < (Degree.value degree)))

    let inverse (perm:permutation ) =
        let ia = Array.zeroCreate perm.values.Length
        { values = CollectionOps.invertArrayNr perm.values ia }

    let isSorted (perm:permutation) =
        CollectionProps.isSorted_inline perm.values

    let isTwoCycle (perm:permutation) =
        CollectionProps.isTwoCycle perm.values

    // will work without error if the permutations are the same size
    let productNr (lhs:permutation) 
                  (rhs:permutation) =
        { permutation.values =  
                CollectionOps.arrayProductInt
                    (lhs.values)
                    (rhs.values)
                    (Array.zeroCreate lhs.values.Length) }


    let product (lhs:permutation) 
                (rhs:permutation) =
        if (lhs.values.Length <> rhs.values.Length) then
            "permuation degrees dont match" |> Error
        else
            { permutation.values =  
                        CollectionOps.arrayProductInt
                            (lhs.values)
                            (rhs.values)
                            (Array.zeroCreate lhs.values.Length) } |> Ok


//*************************************************************
//***************  byte conversions  **************************
//*************************************************************


    //let makeFromBytes (dg:degree) (data:byte[]) = 
    //    ByteArray.makeFromBytes dg create8 create16 data


    //let makeArrayFromBytes (dg:degree) (data:byte[]) = 
    //    ByteArray.makeArrayFromBytes dg create8 create16 data


    //let toBytes (perm:permutation) =
    //    ByteArray.toBytes (perm.values)


    //let arrayToBytes (perms:permutation[]) =
    //    ByteArray.arrayToBytes (perms |> Array.map(fun p -> p.values))



//*************************************************************
//***************    IRando dependent   ***********************
//*************************************************************

    let createRandom (degree:degree) (rnd:IRando) =
        let idArray = (identity degree) |> getArray  
        { values=(RandGen.fisherYatesShuffle rnd idArray |> Seq.toArray)}

    let createRandoms (degree:degree) (rnd:IRando) =
        Seq.initInfinite(fun _ -> createRandom degree rnd)