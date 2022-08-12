namespace global
open Microsoft.FSharp.Core
open Gort.DataStore.DataModel

 
type sortableSetVersion = | AllBitsForOrder | Orbit | SortedStack 
                            | RandomPermutation | RandomBits | RandomSymbols

module SortableSetVersion =
    let parse (strRep:string) =
        match strRep with
        | nameof sortableSetVersion.AllBitsForOrder -> 
            sortableSetVersion.AllBitsForOrder |> Ok
        | nameof sortableSetVersion.Orbit -> 
            sortableSetVersion.Orbit |> Ok
        | nameof sortableSetVersion.SortedStack -> 
            sortableSetVersion.SortedStack |> Ok
        | nameof sortableSetVersion.RandomPermutation -> 
            sortableSetVersion.RandomPermutation |> Ok
        | nameof sortableSetVersion.RandomBits -> 
            sortableSetVersion.RandomBits |> Ok
        | nameof sortableSetVersion.RandomSymbols -> 
            sortableSetVersion.RandomSymbols |> Ok
        | _ -> sprintf "%s not handled" strRep |> Error

    let makeGeneratedSortableSetR 
            (causeR:CauseR) 
            (causePath:string) 
            (version:string) 
            (json:string) =
        let sortableSetR = new SortableSetR();
        sortableSetR.CauseR <- causeR
        sortableSetR.CausePath <- causePath
        sortableSetR.Json <- json
        sortableSetR.Version <- version
        sortableSetR