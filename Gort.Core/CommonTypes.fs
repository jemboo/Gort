namespace global
open System

type degree = private Degree of int

module Degree =
    let value (Degree v) = v
    let create (value:int) =
        if (value > 0) then value |> Degree |> Ok
        else "degree must be greater than 0" |> Error

    let create8 (value:int) =
        if (value > 0) && (value < 9) then value |> Degree |> Ok
        else "degree must be greater than 0 and less than 9" |> Error

    let createNr (value:int) = 
        value |> Degree

    let within (b:degree) v =
        (v >= 0) && (v < (value b))
    
    let maxSwitchesPerStage (degree:degree) =
        (value degree) / 2

    let add (degs:degree seq) =
        degs |> Seq.map(value) 
             |> Seq.reduce(+)
             |> createNr

    let reflect (dg:degree) (src:int) =
        (value dg) - src - 1

    let binExp (dg:degree) =
        (1 <<< (value dg))

    let bitMaskUint64 (dg:degree) =
         (1uL <<< (value dg)) - 1uL

    let bytesNeededFor (dg:degree) =
        match (value dg) with
        | x when (x < 256)  -> 1
        | x when (x < 256 * 256)  -> 2
        | _  -> 4

    let twoSymbolOrderedArray (deg:degree) (hiCt:int) (hiVal:'a) (loVal:'a) =
        [| for i in 0 .. ((value deg) - hiCt - 1) -> loVal; 
           for i in ((value deg) - hiCt) .. ((value deg) - 1)  -> hiVal |]

    // Returns a degree + 1 length int array of
    // of all possible sorted 0-1 sequences of length degree
    let allTwoSymbolOrderedArrays (deg:degree) (hiVal:'a) (loVal:'a) =
        seq { for i = 0 to (value deg) do 
                yield (twoSymbolOrderedArray deg i hiVal loVal) }

    let allIntForDegree (degree:degree) =
        try
            let itemCt = degree |> binExp
            Array.init<int> itemCt (id) |> Ok
        with
            | ex -> ("error in allIntForDegree: " + ex.Message ) 
                    |> Result.Error

    let allUint64ForDegree (degree:degree) =
        try
            let itemCt = degree |> binExp
            Array.init<uint64> itemCt (uint64) |> Ok
        with
            | ex -> ("error in allUint64ForDegree: " + ex.Message ) 
                    |> Result.Error



type byteWidth = private ByteWidth of int
module ByteWidth = 
    let value (ByteWidth v) = v
    let create (value:int) =
        if (value = 1) || (value = 2) || (value = 4) || (value = 8) then value |> ByteWidth |> Ok
        else "RollWidth must be 1, 2, 4 or 8" |> Error


type mutationRate = private MutationRate of float

module MutationRate =
    let value (MutationRate v) = v
    let create (v:float) =
        MutationRate v

