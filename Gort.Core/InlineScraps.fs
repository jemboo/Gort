namespace global
open System
open Microsoft.FSharp.Core

module InlineScraps =

    //let inline yabba< ^a when ^a : (static member (<<<) : ^a * int -> ^a)> (qua:^a) i =
    //    qua &&& (1 <<< i) <> 0

    //let inline yab< ^a > (qua:^a) i =
    //    qua &&& (1 <<< i) <> 0


    //let inline yabba (qua: ^a when ^a : (static member isset : int -> bool)) =
    //            qua.isset 

    let inline descendants name (xml:^x) =
            (^x : (member Descendants : int -> seq<int>) (xml,name))

    let inline descendants2 name (xml:^x) =
            (^x : (member isset : int -> bool) (xml,name))

    let inline descendants3 (xml:^x) name  =
            (^x : (member isset : int -> bool) (xml,name))

    let inline descendants4 (ibts:^x when ^x : (member isset : int -> bool)) dex =
            (^x : (member isset : int -> bool) (ibts, dex))

    let inline walk_the_creature_2 (creature:^a when ^a:(member Walk : unit -> unit)) =
                (^a : (member Walk : unit -> unit) creature)

    let inline walk_the_creature (creature:^a when ^a:(member Walk2 : int->int)) =
                creature
                //(^a : (member Walk2 : int->int) creature 5)

    let inline yabb3 (qua: ^a when ^a : (static member (<<<) : ^a * int -> ^a)) =
        qua <<< 3


    let inline dabba (qua: ^a when ^a : (static member (<<<) : ^a * int -> ^a) and ^a : (static member (>>>) : ^a * int -> ^a)) =
        qua <<< 3


    let inline dodo (qua: ^a when ^a : (static member (<<<) : ^a * int -> ^a) and ^a : (static member (&&&) : ^a * ^a -> ^a)) =
        qua <<< 3

    
    //let inline writeStripeO< ^a when ^a: comparison> 
    //                                (oneThresh:^a)  
    //                                (values:^a[]) 
    //                                (stripePos:int)
    //                                (stripedArray:uint64[]) =
    //    for i = 0 to values.Length - 1 do
    //        if values.[i] >= oneThresh then
    //            stripedArray.[i] <- 
    //                    stripedArray.[i].set stripePos
            
            
    //let inline writeStripeArrayO< ^a when ^a: comparison> 
    //                                (oneThresh:^a)  
    //                                (ord:order) 
    //                                (aValues:^a[][]) =
    //    let stripedArray = Array.zeroCreate<uint64> (Order.value ord)
    //    for i = 0 to aValues.Length - 1 do
    //        writeStripeO oneThresh aValues.[i] i stripedArray
    //    stripedArray


    //let inline toStripeArraysO< ^a when ^a: comparison> 
    //                                (oneThresh:^a)  
    //                                (ord:order) 
    //                                (aSeq:^a[] seq) =
    //     aSeq |> Seq.chunkBySize 64
    //          |> Seq.map(writeStripeArrayO oneThresh ord)