namespace global
open System

module Comby =

    let identity (degree:int) = [|0 .. degree-1|] 

    // converts a density distr to a cumulative distr.
    let toCumulative (startVal:float) (weights:float[])  =
        weights |> Array.scan (fun cum cur -> cum + cur) startVal

    // product map composition: a(b()). See
    // CombyFixture.compIntArraysNr for example
    let compIntArraysNr (a:array<int>) 
                        (b:array<int>) 
                        (p:array<int>) =
        for i = 0 to a.Length - 1 do
            p.[i] <- a.[b.[i]]


    let compIntArrays (a:array<int>)
                      (b:array<int>) =
        try
            let product = Array.zeroCreate a.Length
            compIntArraysNr a b product 
            product |> Ok
        with
            | ex -> ("error in compIntArrays: " + ex.Message ) 
                    |> Result.Error



    let inverseMapArrayNr (a:array<int>)
                          (a_out:array<int>) =
        for i = 0 to a.Length - 1 do
            a_out.[a.[i]] <- i
        a_out


    let inverseMapArray (a:array<int>) =
      try
          let aInv = Array.zeroCreate a.Length
          inverseMapArrayNr a aInv |> Ok
      with
        | ex -> ("error in inverseMapArray: " + ex.Message ) 
                |> Result.Error


    // a_conj * a_core * (a_conj ^ -1)
    let conjIntArraysR (a_conj:array<int>)  
                       (a_core:array<int>) =
        result {
            let! a_conj_inv = inverseMapArray a_conj
            let! rhs = compIntArrays a_core a_conj_inv
            return! compIntArrays a_conj rhs
        }

    let conjIntArraysNr (a_conj:array<int>) 
                        (a_core:array<int>)  
                        (a_out:array<int>) =
        for i = 0 to a_conj.Length - 1 do
            a_out.[a_conj.[i]] <- a_conj.[a_core.[i]]
        a_out

    // a_conj * a_core * (a_conj ^ -1)
    let conjIntArrays (a_conj:array<int>) 
                      (a_core:array<int>)  =
        try
            let a_out = Array.zeroCreate a_conj.Length
            conjIntArraysNr a_conj a_core a_out |> Ok
        with
          | ex -> ("error in conjIntArrays: " + ex.Message ) 
                  |> Result.Error



    let isTwoCycle (a:int[]) =
        result {
            let! comp = compIntArrays a a
            return comp = identity (comp.Length)
        }



    let makeMonoTwoCycle (degree:degree) 
                         (aDex:int) 
                         (bDex:int) =
        Array.init (Degree.value degree) (fun i -> 
            if   (i = aDex) then bDex
            elif (i = bDex) then aDex
            else i)

    let makeAllMonoTwoCycles (degree:degree) =
        seq {for i = 0 to (Degree.value(degree) - 1) do
                for j = 0 to i - 1 do
                    yield makeMonoTwoCycle degree i j}
        

    // generates all int[] of length m, made by drawing m
    // items out of [0 .. (n-1)] without replacement, with the m items
    // always being ordered from smallest to largest.
    let enumNchooseM (n:int) (m:int) =
        let maxVal = n - 1

        let newMaxf l r =  
            let rh, rt = 
                match r with
                | rh::rt ->  (rh, rt)
                | [] -> failwith  "boo boo"
            rh + 2 + ( l |> List.length )

        let rightPack l r = 
            let rh, rt = 
                match r with
                | rh::rt ->  (rh, rt)
                | [] -> failwith  "boo boo"
            let curMax = newMaxf l r
            let rhS = rh + 1
            if (curMax = maxVal) then 
              [(rhS + 1) .. maxVal], rhS, rt
                else 
              [], curMax, [(curMax - 1) .. -1 .. rhS]@rt

        let rec makeNext (lhs:int list) 
                         (c:int)  
                         (rhs:int list) =
            let maxShift = match lhs with
                            | a::_ -> a - 1
                            | _ -> maxVal
            match lhs, c, rhs with
            | l, md, [] when md = maxShift -> None
            | l, md, r when md < maxShift -> Some (l, md + 1, r) 
            | l, md, rh::rt when (md = maxShift ) && 
                                 (rh = (maxShift - 1)) -> 
                        makeNext (md::l) rh  rt
            | l, md, r when (md = maxShift ) -> Some (rightPack l r)
            | l, md, r when md = maxShift -> Some (md::l, md + 1, r) 
            | _, _, _ -> None

        let mutable proceed = true
        let mutable curTup = Some ([], m - 1, [(m - 2) .. -1 .. 0])
        seq { while proceed do
                  let a, b, c = curTup |> Option.get
                  yield a@(b::c) |> List.sort 
                  curTup <- makeNext a b c
                  proceed <- (curTup |> Option.isSome)
            }