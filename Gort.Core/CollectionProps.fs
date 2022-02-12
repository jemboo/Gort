namespace global
open System


module CollectionProps =

    let arrayEquals<'a when 'a:equality> (lhs:'a[]) (rhs:'a[]) =
        (lhs.Length = rhs.Length) &&
        lhs |> Array.forall2 (fun re le -> re = le) rhs


    // converts a density distr to a cumulative distr.
    let asCumulative (startVal:float) (weights:float[])  =
        weights |> Array.scan (fun cum cur -> cum + cur) startVal


    let cratesFor (itemsPerCrate:int) (items:int) = 
        let fullCrates = items / itemsPerCrate
        let leftOvers = items % itemsPerCrate
        if (leftOvers = 0) then fullCrates else fullCrates + 1


    let distanceSquared (a:'a[]) 
                        (b:'a[]) =
        Array.fold2 (fun acc elem1 elem2 ->
            acc + (elem1 - elem2) * (elem1 - elem2)) 0 a b


    let distanceSquared_uL (a:array<uint64>) 
                           (b:array<uint64>) =
        Array.fold2 (fun acc elem1 elem2 ->
            acc + (elem1 - elem2) * (elem1 - elem2)) 0uL a b


    // Measured in bits (log base 2)
    let entropyBitsI (a:array<int>) =
        let f = 1.0 / Math.Log(2.0)
        let tot = float (a |> Array.sum)
        let fa = a  |> Array.filter(fun i->i>0)
                    |> Array.map (fun i->(float i) / tot)
        let res = Array.fold (fun acc elem -> 
                        acc - elem * f * Math.Log(elem)) 0.0 fa
        res


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



    let fixedPointCount (a:int[]) =
        a |> Array.mapi(fun dex e -> if (dex = e) then 1 else 0)
          |> Array.reduce(+)


    let identity (degree:int) = [|0 .. degree-1|] 

    let isIdentity (wh:int[]) =
        wh |> arrayEquals (identity wh.Length)

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


    let isSorted_idiom (values:'a[]) =
        seq { 1 .. (values.Length - 1) }
        |> Seq.forall(fun dex -> values.[dex] >= values.[dex - 1])


    let isSorted (values:'a[]) =
        let mutable i=1
        let mutable looP = true
        while ((i < values.Length) && looP) do
             looP <- (values.[i - 1] <= values.[i])
             i<-i+1
        looP


    let inline isSorted_inline< ^a when ^a: comparison>  (values:^a[]) =
        let mutable i=1
        let mutable looP = true
        while ((i < values.Length) && looP) do
                looP <- (values.[i - 1] <= values.[i])
                i<-i+1
        looP


    let isSorted_uL (values:uint64[]) =
        let mutable i=1
        let mutable looP = true
        while ((i < values.Length) && looP) do
             looP <- (values.[i - 1] <= values.[i])
             i<-i+1
        looP


    let inline isSortedOffset< ^a when ^a: comparison> (baseValues:'a[]) offset length =
        let mutable i=1
        let mutable looP = true
        while ((i < length) && looP) do
             looP <- (baseValues.[i+offset-1] <= baseValues.[i+offset])
             i<-i+1
        looP


    let isTwoCycle (a:int[]) =
        let mutable dex = 0
        let mutable _cont = true
        while _cont && (dex <  a.Length - 1) do 
            let dv = a.[dex]
            _cont <- (dv > - 1) && (dv < a.Length) && (a.[dv] = dex)
            dex <- dex + 1
        _cont


    let isTwoCycle8 (a:uint8[]) =
        let mutable dex = 0
        let mutable _cont = true
        while _cont && (dex <  a.Length - 1) do 
            let dv = a.[dex] |> int
            _cont <- (dv > - 1) && (dv < a.Length) && (a.[dv] = (dex |> uint8))
            dex <- dex + 1
        _cont


    let isTwoCycle16 (a:uint16[]) =
        let mutable dex = 0
        let mutable _cont = true
        while _cont && (dex <  a.Length - 1) do 
            let dv = a.[dex] |> int
            _cont <- (dv > - 1) && (dv < a.Length) && (a.[dv] = (dex |> uint16))
            dex <- dex + 1
        _cont


    let isTwoCycle32 (a:uint32[]) =
        let mutable dex = 0
        let mutable _cont = true
        while _cont && (dex <  a.Length - 1) do 
            let dv = a.[dex] |> int
            _cont <- (dv > - 1) && (dv < a.Length) && (a.[dv] = (dex |> uint32))
            dex <- dex + 1
        _cont


    let unsortednessSquared_uL (a:array<uint64>) =
        distanceSquared_uL a [|0uL .. ((uint64 a.Length ) - 1uL)|]


    let unsortednessSquared_I (a:array<int>) =
        distanceSquared a [|0 .. (a.Length - 1)|]






    //let inline distanceSquared_inline (a:^a[] when ^a:(member (+) : ^a -> ^a -> ^a) and ^a:(member (*) : ^a -> ^a -> ^a)) 
    //                                  (b:^a[] when ^a:(member (+) : ^a -> ^a -> ^a) and ^a:(member (*) : ^a -> ^a -> ^a))  =
    //           Array.fold2 (fun acc elem1 elem2 ->
    //               acc + (elem1 - elem2) * (elem1 - elem2)) 0 a b

