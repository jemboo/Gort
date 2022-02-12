namespace global
open System

type sortableFormat = 
    | u8 = 0
    | u16 = 1
    | b64 = 2

type sortableSetImpl2 = 
    | Bp64 of uint64[]
    | Ints of intSet[]
    | Ints8 of intSet8[]


type sortableSetGen =
    | AllBits of degree
    | Explicit of degree
    | Orbit of permutation
    | Stack of List<degree>


type sortableSet2 = {gen:sortableSetGen; impl:sortableSetImpl2}


type sortableSet = {id:sortableSetId; rollout:rollout}
    
module SortableSetGen =

    let makeAllBits (dg:int) =
        result {
            let! dg = Degree.create dg
            return sortableSetGen.AllBits dg
        }

    let makeOrbit (perm:int[]) =
        result {
            let! perm = Permutation.create perm
            return sortableSetGen.Orbit perm |> Ok
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
        | Orbit perm -> Permutation.getDegree perm
        | Stack dgs -> Degree.add dgs



module SortableSet =

    let getSortableFormat (ss:sortableSet) =
        match (Rollout.getRollWidth ss.rollout |> ByteWidth.value) with
        | x when (x = 1)  ->
            sortableFormat.u8
        | x when (x = 2)  -> 
            sortableFormat.u16
        | x when (x = 4)  ->
            "invalid rollWidth for sortableSet" |> failwith
        | x when (x = 8)  -> 
            sortableFormat.b64
        | _ ->  "invalid rollWidth" |> failwith


    let makeAllBits (dg:degree) (format:sortableFormat) = 
        match format with
        | sortableFormat.u8 -> 
            result {
                let! sortables = IntSet8.allForAsSeq dg
                                |> Seq.map(IntSet8.getValues)
                                |> Seq.concat
                                |> Seq.toArray
                                |> ByteArray.convertUint8sToBytes
                let! rollWdth = (ByteWidth.create 1)
                let! rollLen = (RollLength.create (Degree.value dg))
                let! rollCt = (RollCount.create (1 <<< (Degree.value dg)))
                let! rollout = sortables |> Rollout.init rollWdth rollLen rollCt
                let ssId = SortableSetId.create (GuidUtils.guidFromBytes sortables)
                return {sortableSet.id = ssId; rollout = rollout }
            }
        | sortableFormat.u16 -> 
            result {
                let! sortables = IntSet16.allForAsSeq dg
                                 |> Seq.map(IntSet16.getValues)
                                 |> Seq.concat
                                 |> Seq.toArray
                                 |> ByteArray.convertUint16sToBytes
                let! rollWdth = (ByteWidth.create 2)
                let! rollLen = (RollLength.create (Degree.value dg))
                let! rollCt = (RollCount.create (1 <<< (Degree.value dg)))
                let! rollout = sortables |> Rollout.init rollWdth rollLen rollCt
                let ssId = SortableSetId.create (GuidUtils.guidFromBytes sortables)
                return {sortableSet.id = ssId; rollout = rollout }
            }
        | sortableFormat.b64 -> 
            result {
                let sortables = IntSet8.allForAsSeq dg
                                |> Seq.map(IntSet8.getValues)
                                |> Seq.concat
                                |> Seq.map(byte)
                                |> Seq.toArray
                let! rollWdth = (ByteWidth.create 1)
                let! rollLen = (RollLength.create (Degree.value dg))
                let! rollCt = (RollCount.create (1 <<< (Degree.value dg)))
                let! rollout = sortables |> Rollout.init rollWdth rollLen rollCt
                let ssId = SortableSetId.create (GuidUtils.guidFromBytes sortables)
                return {sortableSet.id = ssId; rollout = rollout }
            }
        | _ -> "invalid format in makeAllBits" |> Error



    //let makeExplicitBp64 (dg:degree) (data:byte[]) = 
    //    result {
    //        let! perms = Permutation.makeArrayFromBytes dg data
    //        return { sortableSet2.gen = sortableSetGen.Explicit dg; 
    //                 impl = perms |> Array.map(Permutation.toIntSet)
    //                              |> sortableSetImpl2.Ints }
    //    }


    //let makeOrbiInts (dg:degree) (data:byte[]) = 
    //    result {
    //        let arrayLen = (Degree.value dg) * (dg |> Degree.bytesNeededFor)
    //        let! perm = data.[0 .. (arrayLen - 1)] |> Permutation.makeFromBytes dg
    //        let orbit = Permutation.powers perm |> Seq.toArray
    //        return { sortableSet2.gen = sortableSetGen.Orbit perm; 
    //                 impl = orbit   |> Array.map(Permutation.toIntSet)
    //                                |> sortableSetImpl2.Ints }
    //    }


    let makeStackInts (dg:degree) (bw:byteWidth) (data:byte[]) = 
        result {
            let! degrees = ByteArray.bytesToDegreeArray data
            let degTot = Degree.add degrees
            if degTot <> dg then
                return! "degree list is incorrect" |> Error
            else
                let! sortables = 
                    match (ByteWidth.value bw) with
                    | 1 -> CollectionOps.stackSortedBlocks degrees 1uy 0uy
                            |> Seq.concat
                            |> Seq.toArray
                            |> ByteArray.convertUint8sToBytes
                    | 2 -> CollectionOps.stackSortedBlocks degrees 1us 0us
                            |> Seq.concat
                            |> Seq.toArray
                            |> ByteArray.convertUint16sToBytes
                    | 8 -> CollectionOps.stackSortedBlocks degrees 1uy 0uy
                            |> Seq.concat
                            |> Seq.toArray
                            |> ByteArray.convertUint8sToBytes
                    | _ -> "incorrect byteWidth" |> Error
                let! rollLen = RollLength.create (Degree.value dg)
                let! rollCt = RollCount.create (sortables.Length / (ByteWidth.value bw))
                let! rollout = sortables |> Rollout.init bw rollLen rollCt
                let ssId = SortableSetId.create (GuidUtils.guidFromBytes sortables)
                return {sortableSet.id = ssId; rollout = rollout }
        }
        
        
    let makeStackInts8 (dg:degree) (data:byte[]) = 
        result {
            let! degrees = ByteArray.bytesToDegreeArray data
            let  degTot = Degree.add degrees
            if degTot <> dg then
                return! "degree list is incorrect" |> Error
            else
                let! sortables = CollectionOps.stackSortedBlocks degrees 1uy 0uy
                                    |> Seq.concat
                                    |> Seq.toArray
                                    |> ByteArray.convertUint8sToBytes
                let! rollWdth = (ByteWidth.create 1)
                let! rollLen = (RollLength.create (Degree.value dg))
                let! rollCt = (RollCount.create sortables.Length )
                let! rollout = sortables |> Rollout.init rollWdth rollLen rollCt
                let ssId = SortableSetId.create (GuidUtils.guidFromBytes sortables)
                return {sortableSet.id = ssId; rollout = rollout }
        }