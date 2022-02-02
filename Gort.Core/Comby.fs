﻿namespace global
open System

module Comby =

    let identity (degree:int) = [|0 .. degree-1|] 

    let isIdentity (wh:int[]) =
        wh |> Collections.arrayEquals (identity wh.Length)

    // converts a density distr to a cumulative distr.
    let toCumulative (startVal:float) (weights:float[])  =
        weights |> Array.scan (fun cum cur -> cum + cur) startVal

    // product map composition: a(b()).
    let arrayProductInt (lhs:array<int>) 
                        (rhs:array<int>) 
                        (prod:array<int>) =
        for i = 0 to lhs.Length - 1 do
            prod.[i] <- lhs.[rhs.[i]]
        prod

    // product map composition: a(b()).
    let arrayProduct16 (lhs:array<uint16>) 
                       (rhs:array<uint16>) 
                       (prod:array<uint16>) =
           let dmax = lhs.Length |> uint16
           let mutable curdex = 0us
           while curdex < dmax do
               let i = curdex |> int
               prod.[i] <- lhs.[rhs.[i] |> int]
               curdex <- curdex + 1us
           prod

    // product map composition: a(b()).
    let arrayProduct8 (lhs:array<uint8>) 
                      (rhs:array<uint8>) 
                      (prod:array<uint8>) =
           let dmax = lhs.Length |> uint8
           let mutable curdex = 0uy
           while curdex < dmax do
               let i = curdex |> int
               prod.[i] <- lhs.[rhs.[i] |> int]
               curdex <- curdex + 1uy
           prod


    let arrayProductIntR (lhs:array<int>)
                         (rhs:array<int>) 
                         (prod:array<int>) =
        try
            arrayProductInt lhs rhs prod |> Ok
        with
            | ex -> ("error in compIntArrays: " + ex.Message ) 
                    |> Result.Error


    let invertArrayNr (a:array<int>)
                      (inv_out:array<int>) =
        for i = 0 to a.Length - 1 do
            inv_out.[a.[i]] <- i
        inv_out


    let invertArray (a:array<int>) 
                    (inv_out:array<int>) =   
      try
          invertArrayNr a inv_out |> Ok
      with
        | ex -> ("error in inverseMapArray: " + ex.Message ) 
                |> Result.Error


    // a_conj * a_core * (a_conj ^ -1)
    let conjIntArraysR (a_conj:array<int>)  
                       (a_core:array<int>) =
        result {
            let! a_conj_inv = invertArray a_conj (Array.zeroCreate a_core.Length)
            let! rhs = arrayProductIntR a_core a_conj_inv (Array.zeroCreate a_core.Length)
            return! arrayProductIntR a_conj rhs (Array.zeroCreate a_core.Length)
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
        


    let allPowers (a_core:array<int>) =
        seq {
            let mutable _continue = true
            let mutable a_cur = Array.copy a_core
            yield a_cur 
            while _continue do
                let a_next = Array.zeroCreate a_cur.Length
                a_cur <- arrayProductInt a_core a_cur a_next
                _continue <- not (isIdentity a_next)
                yield a_cur
        }

    
    let isPermutation (a:int[]) =
        let flags = Array.create a.Length true
        let mutable dex = 0
        let mutable _cont = true
        while _cont && (dex <  a.Length - 1) do
            let dv = a.[dex]
            _cont <- (dv > - 1) && (dv < a.Length) && flags.[dv]
            if _cont then flags.[dv] <- false
            dex <- dex + 1
        _cont


    let isTwoCycle (a:int[]) =
        let mutable dex = 0
        let mutable _cont = true
        while _cont && (dex <  a.Length - 1) do 
            let dv = a.[dex]
            _cont <- (dv > - 1) && (dv < a.Length) && (a.[dv] = dex)
            dex <- dex + 1
        _cont


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