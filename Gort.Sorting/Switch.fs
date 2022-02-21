namespace global
open System


type Switch = {low:int; hi:int}

module Switch = 

    let toString (sw:Switch) =
        sprintf "(%d, %d)" sw.low sw.hi

    let switchMap = 
        [for hi=0 to 128 do 
            for low=0 to hi do 
                yield {Switch.low=low; Switch.hi=hi}]

    let fromIndexes (dexes:int seq) = 
        dexes |> Seq.map(fun dex -> switchMap.[dex])

    let getIndex (switch:Switch) =
        (switch.hi * (switch.hi + 1)) / 2 + switch.low

    // all the switches of degree that have lowVal as the low switch index
    let lowOverlapping (dg:degree) 
                        (lowVal:int) =
        let dm = (Degree.value dg) - 1
        seq {
                for hv = (lowVal + 1) to dm do
                    yield (hv * (hv + 1)) / 2 + lowVal 
                for blv = 0 to (lowVal - 1) do
                    yield (lowVal * (lowVal + 1)) / 2 + blv 
            }

    // all the switches of degree that have hiVal as the hi switch index
    let hiOverlapping (dg:degree) 
                        (hiVal:int) =
        let dm = (Degree.value dg) - 1
        seq {
                for lv = 0 to (hiVal - 1) do
                        yield (hiVal * (hiVal + 1)) / 2 + lv
                for ohv = (hiVal + 1) to dm do
                        yield (ohv * (ohv + 1)) / 2 + hiVal
            }

    let zeroSwitches =
        seq { while true do yield {Switch.low=0; Switch.hi=0} }
    
    // produces switches from only the two cycle components of the 
    // permutation
    let fromIntArrayAsPerm (pArray:int[]) =
            seq { for i = 0 to pArray.Length - 1 do
                    let j = pArray.[i]
                    if ((j > i ) && (i = pArray.[j]) ) then
                            yield {Switch.low=i; Switch.hi=j} }

    let fromPermutation (p:permutation) =
        fromIntArrayAsPerm (Permutation.getArray p)
 
    let fromTwoCyclePerm (tc:twoCycle) =
        fromIntArrayAsPerm (TwoCycle.getArray tc)

    let switchCountForDegree (dg:degree)  =
        uint32 ((Degree.value dg)*(Degree.value dg + 1) / 2)


    let makeAltEvenOdd (degree:degree) (stageCt:stageCount) =
        result {
            let stages = TwoCycle.makeAltEvenOdd degree 
                                      (Permutation.identity degree)
                        |> Seq.take(StageCount.value stageCt)

            return stages |> Seq.map(fromTwoCyclePerm)
                          |> Seq.concat
        }

    // IRando dependent
    let rndNonDegenSwitchesOfDegree (degree:degree) 
                                    (rnd:IRando) =
        let maxDex = switchCountForDegree degree
        seq { while true do 
                    let p = (int (rnd.NextUInt % maxDex))
                    let sw = switchMap.[p] 
                    if (sw.low <> sw.hi) then
                        yield sw }

    let rndSwitchesOfDegree (degree:degree) 
                            (rnd:IRando) =
        let maxDex = switchCountForDegree degree
        seq { while true do 
                    let p = (int (rnd.NextUInt % maxDex))
                    yield switchMap.[p] }


    let rndSymmetric (degree:degree)
                     (rnd:IRando) =
        let aa (rnd:IRando)  = 
            (TwoCycle.rndSymmetric 
                                degree 
                                rnd )
                    |> fromTwoCyclePerm
        seq { while true do yield! (aa rnd) }


    let mutateSwitches (order:degree) 
                       (mutationRate:mutationRate) 
                       (rnd:IRando) 
                       (switches:seq<Switch>) =
        let mDex = uint32 ((Degree.value order)*(Degree.value order + 1) / 2) 
        let mutateSwitch (switch:Switch) =
            match rnd.NextFloat with
            | k when k < (MutationRate.value mutationRate) -> 
                        switchMap.[(int (rnd.NextUInt % mDex))] 
            | _ -> switch
        switches |> Seq.map(fun sw-> mutateSwitch sw)


    let reflect (dg:degree) 
                (sw:Switch) =
        { Switch.low = sw.hi |> Degree.reflect dg;
          Switch.hi = sw.low |> Degree.reflect dg; }


    //let reduce  (redMap:int option [])
    //            (sw:Switch) =
    //    let rpL, rpH = (redMap.[sw.low], redMap.[sw.hi])
    //    match rpL, rpH with
    //    | Some l, Some h -> Some {Switch.low=l; hi=h;}
    //    | _ , _ -> None


    //let reduceMany (sws:Switch array) 
    //               (redMap:int option []) =
    //    sws |> Array.map(reduce redMap)
    //        |> Array.filter(Option.isSome)
    //        |> Array.map(Option.get)


    //let allMasks (degreeSource:Degree)
    //             (degreeDest:Degree)
    //             (swa:Switch array) =
    //    let sd = (Degree.value degreeSource)
    //    let dd = (Degree.value degreeDest)
    //    if sd < dd then
    //        failwith "source degree cannot be smaller than dest"
    //    Combinatorics.enumNchooseM sd dd
    //    |> Seq.map(Combinatorics.mapSubset degreeSource)
    //    |> Seq.map(reduceMany swa)


    //let rndMasks (degreeSource:degree)
    //             (degreeDest:degree)
    //             (swa:Switch array)
    //             (rnd:IRando) =
    //    let sd = (Degree.value degreeSource)
    //    let dd = (Degree.value degreeDest)
    //    if sd < dd then
    //        failwith "source degree cannot be smaller than dest"

    //    RndGen.rndNchooseM sd dd rnd
    //    |> Seq.map(Combinatorics.mapSubset degreeSource)
    //    |> Seq.map(reduceMany swa)

