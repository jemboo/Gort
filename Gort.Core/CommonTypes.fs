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

    let sorted_O_1_Sequence (degree:degree) 
                            (onesCount:int) =
        let totalSize = (value degree)
        let numZeroes = totalSize - onesCount
        Array.init totalSize 
                   (fun i -> if i< numZeroes then 0 else 1)

    //Returns a degree + 1 length int array of
    // of all possible sorted 0-1 sequences of length degree
    let sorted_0_1_Sequences (degree:degree)  =
        seq { for i = 0 to (value degree) do 
                yield (sorted_O_1_Sequence degree i) }


    let allIntForDegree (degree:degree) =
        try
            let itemCt = degree |> binExp
            Array.init<int> itemCt (id) |> Ok
        with
            | ex -> ("error in allBitPackForDegree: " + ex.Message ) 
                    |> Result.Error

    let allUint64ForDegree (degree:degree) =
        try
            let itemCt = degree |> binExp
            Array.init<uint64> itemCt (uint64) |> Ok
        with
            | ex -> ("error in allBitPackForDegree: " + ex.Message ) 
                    |> Result.Error





type mutationRate = private MutationRate of float

module MutationRate =
    let value (MutationRate v) = v
    let create (v:float) =
        MutationRate v

