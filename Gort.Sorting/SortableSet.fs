namespace global
open System


//type sortableB64 = { degree:degree; sortables:uint64[] }
//type sortableB64Roll = { degree:degree; count:int; rollout:uint64[] }
type sortableSet = 
    | Bp64 of degree * uint64[]
    | Ints of degree * int[]

module SortableSet =

    let allBp64ForDegree (dg:degree) = 
        result {
            let! sortables = Bitwise.allBitPackForDegree dg
            return (dg, sortables) |> sortableSet.Bp64
        }

    let stackSets (ssets:sortableSet list) =
        None

    //let allForDegree (degree:degree) = 
    //    result {
        
    //        let! sortables = Bitwise.allBitPackForDegree degree
    //        return  { 
    //                    sortableB64.degree = degree; 
    //                    sortableB64.sortables = sortables
    //                }
    //    }

    //let rndForDegree (degree:degree) (itemCt:int) (rnd:IRando) = 
    //    try
    //        { 
    //            sortableB64.degree = degree; 
    //            sortableB64.sortables = Array.init<uint64> 
    //                                        itemCt 
    //                                        (fun _ -> RndGen.rndBitsUint64 degree rnd)
    //        } |> Ok
    //    with
    //        | ex -> ("error in allBitPackForDegree: " + ex.Message ) |> Result.Error

