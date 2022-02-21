namespace global
open System
module IntSeries = 
    // returns true with an exp decreasing frequency
    let expoB (ticsPerLog:float) (value:int) = 
        if (ticsPerLog |> int) > value then
            true
        else 
            let logo = (ticsPerLog * Math.Log2 (value |> float)) |> int
            let logm = (ticsPerLog * Math.Log2 ((value - 1) |> float)) |> int
            logo > logm


    let logTics (ticsPerLog:float) (endVal:int) =
        seq {
            let mutable dex = 0
            while dex < endVal do
                if (expoB ticsPerLog dex) then yield dex
                dex <- dex + 1
        }

