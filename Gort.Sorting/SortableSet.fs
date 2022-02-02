namespace global
open System


//type sortableB64 = { degree:degree; sortables:uint64[] }
//type sortableB64Roll = { degree:degree; count:int; rollout:uint64[] }
type sortableSetImpl = 
    | Bp64 of uint64[]
    | Ints of permutation[]


type sortableSetGen =
    | Explicit of degree
    | AllBits of degree
    | CoreAndConj of twoCycle*permutation
    | Stack of List<degree>


type sortableSet = {gen:sortableSetGen; impl:sortableSetImpl}
    
module SortableSetGen =

    let makeAllBits (dg:int) =
        result {
            let! dg = Degree.create dg
            return sortableSetGen.AllBits dg
        }

    let makeCoreAndConj (twoCycle:int[]) (perm:int[]) =
        result {
            let! tc = TwoCycle.create twoCycle
            let! perm = Permutation.create perm
            if ((tc |> TwoCycle.getDegree) <> (perm |> Permutation.getDegree)) then
                return "degree does not match stack sum" |> Error
            else return sortableSetGen.CoreAndConj (tc, perm) |> Ok
        }

    let makeStack (dgs:int[]) =
        result {
            let! dgt = dgs |> Array.map(Degree.create)
                           |> Array.toList
                           |> Result.sequence
            return sortableSetGen.Stack dgt
        }

    let getDegree (ssgen:sortableSetGen) =
        match ssgen with
        | Explicit dg -> dg
        | AllBits dg -> dg
        | CoreAndConj (tc, _) -> TwoCycle.getDegree tc
        | Stack dgs -> Degree.add dgs



module SortableSet =

    let allBp64ForDegree (dg:degree) = 
        result {
            let! sortables = Bitwise.allBitPackForDegree dg
            let! rollout = sortables |> Bitwise.bitPackedtoBitStriped dg
            let gen = sortableSetGen.Explicit dg
            return { sortableSet.gen = gen; impl = rollout |> sortableSetImpl.Bp64 }
        }

    let makePermutationSet (dg:degree) (data:byte[]) = 
        result {
            let! perms = Permutation.makeArrayFromBytes dg data
            return { sortableSet.gen = sortableSetGen.Explicit dg; 
                     impl = perms |> sortableSetImpl.Ints }
        }

    let makeCoreAndConj (dg:degree) (data:byte[]) = 
        result {
            let arrayLen = (Degree.value dg) * (Degree.bytesNeededFor dg)
            let! tcA = data.[0 .. (arrayLen - 1)] |> Permutation.makeFromBytes dg
            let! permA = data.[arrayLen ..] |> Permutation.makeFromBytes dg
            let allConjugators = Permutation.powers permA |> Seq.toArray
            return None
        }

    //let stackSets (ssets:sortableSet list) =
    //    None

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

