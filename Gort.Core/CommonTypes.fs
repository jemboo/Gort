namespace global
open System

type degree = private Degree of int

module Degree =
    let value (Degree v) = v
    let create (value:int) =
        if (value > 0) then value |> Degree |> Ok
        else "degree must be greater than 0" |> Error

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


type mutationRate = private MutationRate of float

module MutationRate =
    let value (MutationRate v) = v
    let create (v:float) =
        MutationRate v

