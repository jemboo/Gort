namespace global

open SysExt
open Microsoft.FSharp.Core


module Combinatorics =
    // all the bool arrays of length m with n members true
    let enumerateMwithN (m:int) (n:int) =
        let maxVal = ( 1 <<< m ) - 1
        seq { for i = 0 to maxVal do
                if i.count_dense = n then
                    yield i.toBoolArray m }




    // generates all int[] of length m, made by drawing m
    // items out of [0 .. (n-1)] without replacement, with the m items
    // always being ordered from smallest to largest.
    let enumNchooseM (n: int) (m: int) =
        let maxVal = n - 1

        let _newMaxf l r =
            let rh, rt =
                match r with
                | rh :: rt -> (rh, rt)
                | [] -> failwith "boo boo"

            rh + 2 + (l |> List.length)

        let _rightPack l r =
            let rh, rt =
                match r with
                | rh :: rt -> (rh, rt)
                | [] -> failwith "boo boo"

            let curMax = _newMaxf l r
            let rhS = rh + 1
            if (curMax = maxVal) then
                [ (rhS + 1) .. maxVal ], rhS, rt
            else
                [], curMax, [ (curMax - 1) .. -1 .. rhS ] @ rt


        let rec _makeNext (lhs: int list) (c: int) (rhs: int list) =
            let maxShift =
                match lhs with
                | a :: _ -> a - 1
                | _ -> maxVal

            match lhs, c, rhs with
            | l, md, [] when md = maxShift -> None
            | l, md, r when md < maxShift -> Some(l, md + 1, r)
            | l, md, rh :: rt when (md = maxShift) && (rh = (maxShift - 1)) -> _makeNext (md :: l) rh rt
            | l, md, r when (md = maxShift) -> Some(_rightPack l r)
            | l, md, r when md = maxShift -> Some(md :: l, md + 1, r)
            | _, _, _ -> None


        let mutable proceed = true
        let mutable curTup = Some([], m - 1, [ (m - 2) .. -1 .. 0 ])
        seq {
            while proceed do
                let a, b, c = curTup |> Option.get
                yield a @ (b :: c) |> List.sort
                curTup <- _makeNext a b c
                proceed <- (curTup |> Option.isSome)
        }

