namespace global

open System
open System.Collections
open System.Runtime.CompilerServices
open SysExt


module CollectionProps =

    let rec compareAny (o1: obj) (o2: obj) =
        match (o1, o2) with
        | (:? IComparable as o1), (:? IComparable as o2) -> Some(compare o1 o2)
        | (:? IEnumerable as arr1), (:? IEnumerable as arr2) ->
            Seq.zip (arr1 |> Seq.cast) (arr2 |> Seq.cast)
            |> Seq.choose (fun (a, b) -> compareAny a b)
            |> Seq.skipWhile ((=) 0)
            |> Seq.tryHead
            |> Option.defaultValue 0
            |> Some
        | (:? ITuple as tup1), (:? ITuple as tup2) ->
            let tupleToSeq (tuple: ITuple) =
                seq {
                    for i in 0 .. tuple.Length do
                        yield tuple.[i]
                }

            compareAny (tupleToSeq tup1) (tupleToSeq tup2)
        | _ -> None

    let areEqual (o1: obj) (o2: obj) =
        match compareAny o1 o2 with
        | Some v -> v = 0
        | None -> false

    let arrayEquals<'a when 'a: equality> (lhs: 'a[]) (rhs: 'a[]) =
        (lhs.Length = rhs.Length) && lhs |> Array.forall2 (fun re le -> re = le) rhs

    //returns the last n items of the list in the original order
    let rec last n xs =
        if List.length xs <= n then xs else last n xs.Tail

    //returns the first n items of the list in the original order,
    //or all the items if it's shorter than n
    let first n (xs: 'a list) =
        let mn = min n xs.Length
        xs |> List.take mn

    // converts a density distr to a cumulative distr.
    let asCumulative (startVal: float) (weights: float[]) =
        weights |> Array.scan (fun cum cur -> cum + cur) startVal


    let cratesFor (itemsPerCrate: int) (items: int) =
        let fullCrates = items / itemsPerCrate
        let leftOvers = items % itemsPerCrate
        if (leftOvers = 0) then fullCrates else fullCrates + 1

    let check2dArraySize (arrayLength: arrayLength) (arrayCount: arrayCount) (data: 'T[]) =
        let expectedLen =
            (arrayLength |> ArrayLength.value) * (arrayCount |> ArrayCount.value)

        if (data.Length = expectedLen) then
            () |> Ok
        else
            sprintf
                "arrayLength:%d, arrayCount: %d data length: %d"
                (arrayLength |> ArrayLength.value)
                (arrayCount |> ArrayCount.value)
                data.Length
            |> Error


    let inline distanceSquared  (a: ^a[]) (b: ^a[]) =
        let mutable i = 0
        let mutable acc = zero_of a.[0]
        while (i < a.Length) do
            acc <- acc + (a.[i] - b.[i]) * (a.[i] - b.[i])
            i <- i + 1
        acc


    let inline distanceSquaredR  (a: ^a[]) (b: ^a[]) =
        try
            let mutable i = 0
            let mutable acc = zero_of a.[0]
            while (i < a.Length) do
                acc <- acc + (a.[i] - b.[i]) * (a.[i] - b.[i])
                i <- i + 1
            acc |> Ok
        with ex ->
            $"error in distanceSquared: { ex.Message }" |> Result.Error


    let inline isDistanceGtZero  (a: ^a[]) (b: ^a[]) =
        try
            let mutable i = 0
            let mutable acc = zero_of a.[0]
            while (i < a.Length) do
                acc <- acc + (a.[i] - b.[i]) * (a.[i] - b.[i])
                i <- i + 1
            (acc > (zero_of a.[0]) ) |> Ok
        with ex ->
            $"error in distanceSquared: { ex.Message }" |> Result.Error


    let inline isDistanceGtZeroC  (a: ^a[]) (b: ^a[]) =
        result {
            let! dist = distanceSquaredR a b
            return (dist > (zero_of a.[0]) )
        }


    let distanceSquared_int (a: int[]) (b: int[]) =
        let mutable i = 0
        let mutable acc = 0
        while (i < a.Length) do
            acc <- acc + (a.[i] - b.[i]) * (a.[i] - b.[i])
            i <- i + 1
        acc


    let distanceSquared_idiom (a: 'a[]) (b: 'a[]) =
        Array.fold2 (fun acc elem1 elem2 -> acc + (elem1 - elem2) * (elem1 - elem2)) 0 a b


    let distanceSquared_uL (a: array<uint64>) (b: array<uint64>) =
        Array.fold2 (fun acc elem1 elem2 -> acc + (elem1 - elem2) * (elem1 - elem2)) 0uL a b


    // Measured in bits (log base 2)
    let entropyBitsI (a: array<int>) =
        let f = 1.0 / Math.Log(2.0)
        let tot = float (a |> Array.sum)
        let fa = a |> Array.filter (fun i -> i > 0) |> Array.map (fun i -> (float i) / tot)
        let res = Array.fold (fun acc elem -> acc - elem * f * Math.Log(elem)) 0.0 fa
        res


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


    let fixedPointCount (a: int[]) =
        a |> Array.mapi (fun dex e -> if (dex = e) then 1 else 0) |> Array.reduce (+)


    let identity (order: int) = [| 0 .. order - 1 |]
    //let inline identity< ^a when ^a:(static member op_Explicit:^a-> int) and
    //                             ^a: (static member op_Explicit:int-> ^a) >
    //                (order: ^a) = 0 |> ^a

    let isIdentity (wh: int[]) = wh |> arrayEquals (identity wh.Length)

    let isPermutation (a: int[]) =
        let flags = Array.create a.Length true
        let mutable dex = 0
        let mutable _cont = true

        while _cont && (dex < a.Length - 1) do
            let dv = a.[dex]
            _cont <- (dv > - 1) && (dv < a.Length) && flags.[dv]
            if _cont then flags.[dv] <- false
            dex <- dex + 1
        _cont


    let isSorted_idiom (values: 'a[]) =
        seq { 1 .. (values.Length - 1) }
        |> Seq.forall (fun dex -> values.[dex] >= values.[dex - 1])


    let inline isSorted< ^a when ^a: comparison> (values: ^a[]) =
        let mutable i = 1
        let mutable looP = true
        while ((i < values.Length) && looP) do
            looP <- (values.[i - 1] <= values.[i])
            i <- i + 1
        looP


    let inline isSortedOffset< ^a when ^a: comparison> (baseValues: 'a[]) offset length =
        let mutable i = 1
        let mutable looP = true
        while ((i < length) && looP) do
            looP <- (baseValues.[i + offset - 1] <= baseValues.[i + offset])
            i <- i + 1
        looP


    let inline isTwoCycle< ^a when ^a: comparison and ^a:(static member op_Explicit:^a->int)> (values: ^a[]) =
        let mutable dex = 0
        let mutable _cont = true
        while _cont && (dex < values.Length - 1) do
            let dv = values.[dex] |> int
            _cont <- (dv > - 1) && (dv < values.Length) && (values.[dv] |> int = dex)
            dex <- dex + 1
        _cont


    // returns a sequence of items that occur more than once
    let itemsOccuringMoreThanOnce items =
        seq {
            let d = System.Collections.Generic.Dictionary()
            for i in items do
                match d.TryGetValue(i) with
                | false, _ -> d.[i] <- false // first observance
                | true, false ->
                    d.[i] <- true
                    yield i // second observance
                | true, true -> () // already seen at least twice
        }

    let unsortednessSquared_uL (a: array<uint64>) =
        distanceSquared_uL a [| 0uL .. ((uint64 a.Length) - 1uL) |]


    let unsortednessSquared_I (a: array<int>) =
        distanceSquared_idiom a [| 0 .. (a.Length - 1) |]
