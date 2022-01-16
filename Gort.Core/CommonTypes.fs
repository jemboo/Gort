namespace global
open System

type degree = private Degree of int

module Degree =
    let value (Degree v) = v

    let create (value:int) = 
        value |> Degree

    let within (b:degree) v =
        (v >= 0) && (v < (value b))
    
    let maxSwitchesPerStage (degree:degree) =
        (value degree) / 2

    let add (degs:degree seq) =
        degs |> Seq.map(value) 
             |> Seq.reduce(+)
             |> create

    let reflect (dg:degree) (src:int) =
        (value dg) - src - 1

    let binExp (dg:degree) =
        (1 <<< (value dg))

    let bitMaskUint64 (dg:degree) =
         (1uL <<< (value dg)) - 1uL


//type smallDegree = private SmallDegree of int

//module SmallDegree =
//    let value (SmallDegree v) = v

//    let maxValue = 64

//    let create (value:int) = 
//        match value with
//        | dInt when ((dInt > maxValue) || (dInt > 1))  -> 
//                    dInt |> SmallDegree |> Ok
//        | foul -> (sprintf "%d not in range for smallDegree" foul) 
//                  |> Error

//    let fromDegree (degree:degree) = 
//        create (Degree.value degree)

//    let toDegree (sd:smallDegree) = 
//        Degree.create (value sd)

//    let binExp (smallDegree:smallDegree) =
//        (1 <<< (value smallDegree)) 

//    let bitMaskUint64 (sDg:smallDegree) =
//         (1uL <<< (value sDg)) - 1uL



type mutationRate = private MutationRate of float

module MutationRate =
    let value (MutationRate v) = v
    let create (v:float) =
        MutationRate v

