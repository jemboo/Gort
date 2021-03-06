namespace global

// a permutation of the set {0, 1,.. (order-1)}
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
        
    let getOrder (perm:permutation) =
        Order.createNr perm.values.Length

    let rotate (order:order) (dir:int) = 
        let d = (Order.value order)
        { values=Array.init d (fun i-> (i + dir) % d)}

    let powers (maxCount:int option) (perm:permutation) =
        let permSeq = 
            match maxCount with
            | Some mc -> perm.values |> CollectionOps.allPowersCapped mc
            | None -> perm.values |> CollectionOps.allPowers

        permSeq |> Seq.map(fun vs -> {permutation.values = vs})

    let conjugate (conj:permutation)  (pA:permutation) =
        result {
            let! res = CollectionOps.conjIntArrays 
                                (pA |> getArray)
                                (conj |> getArray)
            return create res
        }

    let cyclicGroup (order:order) = 
        let r1 = rotate order 1
        powers None r1 |> Seq.toArray

    let identity (order:order) = 
        { values= CollectionProps.identity (Order.value order)}

    let toIntSet (perm:permutation) =
        {intSet.values = perm.values}

    let toIntSet8 (perm:permutation) =
        {intSet8.values = perm.values |> Array.map(uint8)}

    let inRange (order:order) (value:int) =
       ((value > -1) && (value < (Order.value order)))

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
            "permuation orders dont match" |> Error
        else
            { permutation.values =  
                        CollectionOps.arrayProductInt
                            (lhs.values)
                            (rhs.values)
                            (Array.zeroCreate lhs.values.Length) } |> Ok


//*************************************************************
//***************  byte conversions  **************************
//*************************************************************


    //let makeFromBytes (ord:order) (data:byte[]) = 
    //    ByteArray.makeFromBytes ord create8 create16 data


    //let makeArrayFromBytes (ord:order) (data:byte[]) = 
    //    ByteArray.makeArrayFromBytes ord create8 create16 data


    //let toBytes (perm:permutation) =
    //    ByteArray.toBytes (perm.values)


    //let arrayToBytes (perms:permutation[]) =
    //    ByteArray.arrayToBytes (perms |> Array.map(fun p -> p.values))



//*************************************************************
//***************    IRando dependent   ***********************
//*************************************************************

    let createRandom (order:order) (rnd:IRando) =
        let idArray = (identity order) |> getArray  
        { values=(RandVars.fisherYatesShuffle rnd idArray |> Seq.toArray)}

    let createRandoms (order:order) (rnd:IRando) =
        Seq.initInfinite(fun _ -> createRandom order rnd)