namespace global
open System


module Collections =

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


    let fixedPointCount (a:int[]) =
        a |> Array.mapi(fun dex e -> if (dex = e) then 1 else 0)
          |> Array.reduce(+)


    let distanceSquared (a:'a[]) 
                        (b:'a[]) =
        Array.fold2 (fun acc elem1 elem2 ->
            acc + (elem1 - elem2) * (elem1 - elem2)) 0 a b


    let distanceSquared_uL (a:array<uint64>) 
                           (b:array<uint64>) =
        Array.fold2 (fun acc elem1 elem2 ->
            acc + (elem1 - elem2) * (elem1 - elem2)) 0uL a b


    //let inline distanceSquared_inline (a:^a[] when ^a:(member (+) : ^a -> ^a -> ^a) and ^a:(member (*) : ^a -> ^a -> ^a)) 
    //                                  (b:^a[] when ^a:(member (+) : ^a -> ^a -> ^a) and ^a:(member (*) : ^a -> ^a -> ^a))  =
    //           Array.fold2 (fun acc elem1 elem2 ->
    //               acc + (elem1 - elem2) * (elem1 - elem2)) 0 a b


    let unsortednessSquared_uL (a:array<uint64>) =
        distanceSquared_uL a [|0uL .. ((uint64 a.Length ) - 1uL)|]

    let unsortednessSquared_I (a:array<int>) =
        distanceSquared a [|0 .. (a.Length - 1)|]


    // Measured in bits (log base 2)
    let entropyBitsI (a:array<int>) =
        let f = 1.0 / Math.Log(2.0)
        let tot = float (a |> Array.sum)
        let fa = a  |> Array.filter(fun i->i>0)
                    |> Array.map (fun i->(float i) / tot)
        let res = Array.fold (fun acc elem -> 
                        acc - elem * f * Math.Log(elem)) 0.0 fa
        res 