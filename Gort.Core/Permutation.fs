namespace global
open System

// a permutation of the set {0, 1,.. (degree-1)}
type permutation = private {values:int[] }
module Permutation =

    let create (vals:int[]) =
        if Comby.isPermutation vals then
             { permutation.values = vals} |> Ok
        else
        "not a permutation" |> Error

    let getArray (perm:permutation) = perm.values

    let getDegree (perm:permutation) =
        Degree.createNr perm.values.Length

    let identity (degree:degree) = 
        { values= Comby.identity (Degree.value degree)}

    let permFrom8bits (b:byte[]) =
        create (b |> Array.map(int))

    let permFrom16bits (b:uint16[]) =
        create (b |> Array.map(int))

    let rotate (degree:degree) (dir:int) = 
        let d = (Degree.value degree)
        { values=Array.init d (fun i-> (i + dir) % d)}

    let isTwoCycle (perm:permutation) =
        Comby.isTwoCycle perm.values

    let inRange (degree:degree) (value:int) =
       ((value > -1) && (value < (Degree.value degree)))

    let inverse (p:permutation)
                (a_out:array<int>) =
        { values = Comby.invertArrayNr p.values a_out }

    // will work without error if the permutations are the same size
    let productNr (lhs:permutation) 
                  (rhs:permutation) =
        { permutation.values =  
                Comby.arrayProductInt
                    (lhs.values)
                    (rhs.values)
                    (Array.zeroCreate lhs.values.Length) }


    let product (lhs:permutation) 
                (rhs:permutation) =
        if (lhs.values.Length <> rhs.values.Length) then
            "permuation degrees dont match" |> Error
        else
            { permutation.values =  
                        Comby.arrayProductInt
                            (lhs.values)
                            (rhs.values)
                            (Array.zeroCreate lhs.values.Length) } |> Ok


    let conjugate (pA:permutation) (conj:permutation) =
        result {
            let! res = Comby.conjIntArrays 
                                (pA |> getArray)
                                (conj |> getArray)
            return create res
        }


    let powers (perm:permutation)  =
        perm.values |> Comby.allPowers
                    |> Seq.map(fun vs -> {permutation.values = vs})


    let cyclicGroup (degree:degree) = 
        let r1 = rotate degree 1
        powers r1 |> Seq.toArray


    let permArrayFromUint8 (dg:degree) (data:byte[]) = 
        try
            if (data.Length) % (Degree.value dg) <> 0 then
                "data length is incorrect for degree" |> Error
            else
                result {
                    let! permsR =
                        data |> Array.chunkBySize (Degree.value dg) 
                             |> Array.map(permFrom8bits)
                             |> Array.toList
                             |> Result.sequence
                    return permsR |> List.toArray
                }
        with
          | ex -> ("error in permsFromUint8: " + ex.Message ) 
                  |> Result.Error


    let permArrayFromUint16 (dg:degree) (data:byte[]) = 
        try
            if (data.Length) % (2 * (Degree.value dg)) <> 0 then
                "data length is incorrect for degree" |> Error
            else
                result {
                    let! u16s = Bitwise.getUint16arrayFromBytes data (data.Length / 2) 0
                    let! permsR =
                        u16s |> Array.chunkBySize (Degree.value dg)
                             |> Array.map(permFrom16bits)
                             |> Array.toList
                             |> Result.sequence
                    return permsR |> List.toArray
                }
        with
          | ex -> ("error in permsFromUint8: " + ex.Message ) 
                  |> Result.Error


    let makeArrayFromBytes (dg:degree) (data:byte[]) = 
        match (Degree.value dg) with
        | x when (x < 256)  -> permArrayFromUint8 dg data
        | x when (x < 256 * 256)  -> permArrayFromUint16 dg data
        | _ -> "invalid degree" |> Error


    let makeFromBytes (dg:degree) (data:byte[]) = 
        match (Degree.value dg) with
        | x when (x < 256)  -> permFrom8bits data
        | x when (x < 256 * 256)  -> 
              result {
                let! u16s = Bitwise.getUint16arrayFromBytes data (data.Length / 2) 0
                return! permFrom16bits u16s
              }
        | _ -> "invalid degree" |> Error


    // IRando dependent

    let createRandom (degree:degree) (rnd:IRando) =
        let idArray = (identity degree) |> getArray  
        { values=(RndGen.fisherYatesShuffle rnd idArray |> Seq.toArray)}

    let createRandoms (degree:degree) (rnd:IRando) =
        Seq.initInfinite(fun _ -> createRandom degree rnd)



// a permutation of the set {0, 1,.. (degree-1)}, that is it's own inverse
type twoCycle = private { values:int[] }
module TwoCycle = 

    let create (vals:int[]) =
        if Comby.isTwoCycle vals then
             { twoCycle.values = vals} |> Ok
        else
            "not a two cycle" |> Error

    let getArray (tc:twoCycle) = tc.values

    let getDegree (tc:twoCycle) =
        Degree.createNr tc.values.Length

    let identity (degree:int) = { twoCycle.values = [|0 .. degree-1|] }
    
    let twoCycleFrom8bits (b:byte[]) =
        create (b |> Array.map(int))

    let twoCycleFrom16bits (b:uint16[]) =
        create (b |> Array.map(int))

    let makeFromBytes (dg:degree) (data:byte[]) = 
        match (Degree.value dg) with
        | x when (x < 256)  -> twoCycleFrom8bits data
        | x when (x < 256 * 256)  -> 
              result {
                let! u16s = Bitwise.getUint16arrayFromBytes data (data.Length / 2) 0
                return! twoCycleFrom16bits u16s
              }
        | _ -> "invalid degree" |> Error

    let conjugate (tc:twoCycle) (perm:permutation) =
        { twoCycle.values =
            Comby.conjIntArraysNr (Permutation.getArray perm)
                                  (getArray tc)
                                  (Array.zeroCreate perm.values.Length)
        }

