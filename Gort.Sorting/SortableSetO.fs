namespace global
open System

type Burt = None

//type sortableSetImpl = 
//    | Bp64 of uint64[]
//    | Ints of intSet[]
//    | Ints8 of intSet8[]


//type sortableSetGen =
//    | AllBits of degree
//    | Explicit of degree
//    | Orbit of permutation
//    | Stack of List<degree>


//type sortableSet = {gen:sortableSetGen; impl:sortableSetImpl}
    
//module SortableSetGen =

//    let makeAllBits (dg:int) =
//        result {
//            let! dg = Degree.create dg
//            return sortableSetGen.AllBits dg
//        }

//    let makeOrbit (perm:int[]) =
//        result {
//            let! perm = Permutation.create perm
//            return sortableSetGen.Orbit perm |> Ok
//        }

//    let makeStack (dgs:int[]) =
//        result {
//            let! dgt = dgs |> Array.map(Degree.create)
//                           |> Array.toList
//                           |> Result.sequence
//            return sortableSetGen.Stack dgt
//        }

//    let getDegree (ssgen:sortableSetGen) =
//        match ssgen with
//        | Explicit dg -> dg
//        | AllBits dg -> dg
//        | Orbit perm -> Permutation.getDegree perm
//        | Stack dgs -> Degree.add dgs



//module SortableSet =

//    let makeAllBits (dg:degree) = 
//        result {
//            let! sortables = Degree.allUint64ForDegree dg
//            let! rollout = sortables |> ByteUtils.uint64ArraytoBitStriped dg
//            let gen = sortableSetGen.Explicit dg
//            return { sortableSet.gen = gen; impl = rollout |> sortableSetImpl.Bp64 }
//        }


//    let makeExplicitBp64 (dg:degree) (data:byte[]) = 
//        result {
//            let! perms = Permutation.makeArrayFromBytes dg data
//            return { sortableSet.gen = sortableSetGen.Explicit dg; 
//                     impl = perms |> Array.map(Permutation.toIntSet)
//                                  |> sortableSetImpl.Ints }
//        }


//    let makeOrbiInts (dg:degree) (data:byte[]) = 
//        result {
//            let arrayLen = (Degree.value dg) * (dg |> Degree.bytesNeededFor)
//            let! perm = data.[0 .. (arrayLen - 1)] |> Permutation.makeFromBytes dg
//            let orbit = Permutation.powers perm |> Seq.toArray
//            return { sortableSet.gen = sortableSetGen.Orbit perm; 
//                     impl = orbit   |> Array.map(Permutation.toIntSet)
//                                    |> sortableSetImpl.Ints }
//        }


//    let makeStackInts (dg:degree) (data:byte[]) = 
//        result {
//            let! degrees = ByteArray.bytesToDegreeArray data
//            let degTot = Degree.add degrees
//            if degTot <> dg then
//                return! "degree list is incorrect" |> Error
//            else
//                let sortables = CollectionOps.stackSortedBlocks degrees
//                                |> Seq.map(IntSet.create)
//                                |> Seq.toArray
//                return { sortableSet.gen = sortableSetGen.Stack degrees; 
//                         impl = sortables |> sortableSetImpl.Ints }
//        }
        
        
//    let makeStackInts8 (dg:degree) (data:byte[]) = 
//        result {
//            let! degrees = ByteArray.bytesToDegreeArray data
//            let degTot = Degree.add degrees
//            if degTot <> dg then
//                return! "degree list is incorrect" |> Error
//            else
//                let sortables = CollectionOps.stackSortedBlocks degrees
//                                |> Seq.map(IntSet8.create)
//                                |> Seq.toArray
//                return { sortableSet.gen = sortableSetGen.Stack degrees; 
//                            impl = sortables |> sortableSetImpl.Ints8 }
//        }