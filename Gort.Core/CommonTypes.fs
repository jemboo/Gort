namespace global
open System

type order = private Order of int

module Order =
    let value (Order v) = v
    let create (value:int) =
        if (value > 0) then value |> Order |> Ok
        else "order must be greater than 0" |> Error

    let create8 (value:int) =
        if (value > 0) && (value < 9) then value |> Order |> Ok
        else "order must be greater than 0 and less than 9" |> Error

    let createNr (value:int) = 
        value |> Order

    let within (b:order) v =
        (v >= 0) && (v < (value b))
    
    let maxSwitchesPerStage (order:order) =
        (value order) / 2

    let switchCount (order:order) =
        ((value order) * (value order - 1) )/ 2

    let add (degs:order seq) =
        degs |> Seq.map(value) 
             |> Seq.reduce(+)
             |> createNr

    let reflect (ord:order) (src:int) =
        (value ord) - src - 1

    let binExp (ord:order) =
        (1 <<< (value ord))

    let bitMaskUint64 (ord:order) =
         (1uL <<< (value ord)) - 1uL

    let bytesNeededFor (ord:order) =
        match (value ord) with
        | x when (x < 256)  -> 1
        | x when (x < 256 * 256)  -> 2
        | _  -> 4

    let twoSymbolOrderedArray (deg:order) (hiCt:int) (hiVal:'a) (loVal:'a) =
        [| for i in 0 .. ((value deg) - hiCt - 1) -> loVal; 
           for i in ((value deg) - hiCt) .. ((value deg) - 1)  -> hiVal |]

    // Returns a order + 1 length int array of
    // of all possible sorted 0-1 sequences of length order
    let allTwoSymbolOrderedArrays (deg:order) (hiVal:'a) (loVal:'a) =
        seq { for i = 0 to (value deg) do 
                yield (twoSymbolOrderedArray deg i hiVal loVal) }

    let allSortableAsInt (order:order) =
        try
            let itemCt = order |> binExp
            Array.init<int> itemCt (id) |> Ok
        with
            | ex -> ("error in allIntForDegree: " + ex.Message ) 
                    |> Result.Error

    let allSortableAsUint64 (order:order) =
        try
            let itemCt = order |> binExp
            Array.init<uint64> itemCt (uint64) |> Ok
        with
            | ex -> ("error in allUint64ForDegree: " + ex.Message ) 
                    |> Result.Error



type byteWidth = private ByteWidth of int
module ByteWidth = 
    let value (ByteWidth v) = v
    let create (value:int) =
        if (value = 1) || (value = 2) || (value = 4) || (value = 8) then value |> ByteWidth |> Ok
        else "byteWidth must be 1, 2, 4 or 8" |> Error


type mutationRate = private MutationRate of float
module MutationRate =
    let value (MutationRate v) = v
    let create (v:float) =
        MutationRate v

