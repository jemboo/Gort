namespace global
open System



// a permutation of the set {0, 1,.. (degree-1)}
type permutation = private {degree:degree; values:int[] }
module Permutation =
    let create (vals:int[]) =
            {permutation.degree=Degree.create vals.Length; values=vals }

    let identity (degree:degree) = 
        {degree=degree; values= Comby.identity (Degree.value degree)}

    let rotate (degree:degree) (dir:int) = 
        let d = (Degree.value degree)
        {degree=degree; values=Array.init d (fun i-> (i + dir) % d)}

    let arrayValues perm = perm.values
    let degree perm = perm.degree

    let isTwoCycle (perm:permutation) =
        Comby.isTwoCycle perm.values

    let inRange (degree:degree) (value:int) =
       ((value > -1) && (value < (Degree.value degree)))

    let inverse (p:permutation) =
        result {
            let! inv = Comby.inverseMapArray (p |> arrayValues)
            return create inv
        }

    let product (pA:permutation) (pB:permutation) =
        result {
            let! prod = Comby.compIntArrays 
                                (pA |> arrayValues)
                                (pB |> arrayValues)
            return create prod
        }

    let conjugate (pA:permutation) (conj:permutation) =
        result {
            let! res = Comby.conjIntArrays 
                                (pA |> arrayValues)
                                (conj |> arrayValues)
            return create res
        }

    let productR (pA:permutation) (pB:permutation) =
        if (Degree.value pA.degree) <> (Degree.value pB.degree) then
                Error (sprintf "degree %d <> degree %d:" 
                        (Degree.value pA.degree) (Degree.value pB.degree))
        else
            product pA pB |> Ok

    //let powers (maxPower:int) (perm:permutation)  =
    //    let mutable loop = true
    //    let mutable curPerm = perm
    //    let mutable curPow = 0
    //    seq { while loop do 
    //            yield curPerm
    //            curPerm <- product perm curPerm
    //            curPow <- curPow + 1
    //            if ((curPerm = perm) || (curPow > maxPower)) then
    //                loop <- false}

    //let cyclicGroup (degree:degree) = 
    //    let r1 = rotate degree 1
    //    powers ((Degree.value degree) - 1) r1 |> Seq.toList


    // IRando dependent

    let createRandom (degree:degree) (rnd:IRando) =
        let idArray = (identity degree) |> arrayValues  
        { degree=degree;
          values=(RndGen.fisherYatesShuffle rnd idArray |> Seq.toArray)}

    let createRandoms (degree:degree) (rnd:IRando) =
        Seq.initInfinite(fun _ -> createRandom degree rnd)





// a permutation of the set {0, 1,.. (degree-1)}, that is it's own inverse
type twoCyclePerm = private { degree:degree; values:int[] }
module TwoCyclePerm = 

    let identity (degree:int) = [|0 .. degree-1|] 