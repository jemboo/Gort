namespace global
open System


type switch = {low:int; hi:int}

module Switch = 

    let toString (sw:switch) =
        sprintf "(%d, %d)" sw.low sw.hi

    let maxDegree = 255
    let switchMap = 
        [for hi=0 to maxDegree do 
            for low=0 to hi do 
                yield {switch.low=low; hi=hi}]

    let mapIndexUb = switchMap.Length
    
    let maxMapIndexForDegree (dg:degree)  =
        uint32 ((Degree.value dg)*(Degree.value dg + 1) / 2)
    
    let fromIndexes (dexes:int seq) = 
        dexes |> Seq.map(fun dex -> switchMap.[dex])

    let fromIndexesNonDeg (dexes:int seq) = 
        dexes |> fromIndexes |> Seq.filter(fun sw -> sw.low <> sw.hi)

    let getIndex (switch:switch) =
        (switch.hi * (switch.hi + 1)) / 2 + switch.low
        
    // all switch indexes for degree with lowVal
    let lowOverlapping (dg:degree) 
                       (lowVal:int) =
        seq { for hv = (lowVal + 1) to (Degree.value dg) - 1 do
                    yield (hv * (hv + 1)) / 2 + lowVal  }

    // all switch indexes for degree with hiVal
    let hiOverlapping (dg:degree) 
                      (hiVal:int) =
        seq { for lv = 0 to (hiVal - 1) do
                 yield (hiVal * (hiVal + 1)) / 2 + lv  }

    let zeroSwitches =
        seq { while true do yield {switch.low=0; hi=0} }
    
    // produces switches the two cycles in the permutation
    let extractFromInts (pArray:int[]) =
            seq { for i = 0 to pArray.Length - 1 do
                    let j = pArray.[i]
                    if ((j > i ) && (i = pArray.[j]) ) then
                            yield {switch.low=i; hi=j} }

    let fromPermutation (p:permutation) =
        extractFromInts (Permutation.getArray p)
 
    let fromTwoCycle (tc:twoCycle) =
        extractFromInts (TwoCycle.getArray tc)

    let makeAltEvenOdd (degree:degree) (stageCt:stageCount) =
        result {
            let stages = TwoCycle.makeAltEvenOdd degree 
                                      (Permutation.identity degree)
                        |> Seq.take(StageCount.value stageCt)

            return stages |> Seq.map(fromTwoCycle)
                          |> Seq.concat
        }

    // IRando dependent
    let rndNonDegenSwitchesOfDegree (degree:degree) 
                                    (rnd:IRando) =
        let maxDex = maxMapIndexForDegree degree
        seq { while true do 
                    let p = (int (rnd.NextUInt % maxDex))
                    let sw = switchMap.[p] 
                    if (sw.low <> sw.hi) then
                        yield sw }

    let rndSwitchesOfDegree (degree:degree) 
                            (rnd:IRando) =
        let maxDex = maxMapIndexForDegree degree
        seq { while true do 
                    let p = (int (rnd.NextUInt % maxDex))
                    yield switchMap.[p] }


    let rndSymmetric (degree:degree)
                     (rnd:IRando) =
        let aa (rnd:IRando)  = 
            (TwoCycle.rndSymmetric degree rnd) |> fromTwoCycle
        seq { while true do yield! (aa rnd) }


    let mutateSwitches (order:degree) 
                       (mutationRate:mutationRate) 
                       (rnd:IRando) 
                       (switches:seq<switch>) =
        let mDex = uint32 ((Degree.value order)*(Degree.value order + 1) / 2) 
        let mutateSwitch (switch:switch) =
            match rnd.NextFloat with
            | k when k < (MutationRate.value mutationRate) -> 
                        switchMap.[(int (rnd.NextUInt % mDex))] 
            | _ -> switch
        switches |> Seq.map(fun sw-> mutateSwitch sw)


    let reflect (dg:degree) 
                (sw:switch) =
        { switch.low = sw.hi |> Degree.reflect dg;
          switch.hi = sw.low |> Degree.reflect dg; }



    // filters the switchArray, removing switches that compare 
    // indexes that are not in subset. It then relabels the indexes
    // according to the subset. Ex, if the subset was [2;5;8], then
    // index 2 -> 0; index 5-> 1; index 8 -> 2
    let rebufo (degree:degree)
               (swa:switch array) 
               (subset: int list) =

        let _mapSubset (degree:degree)
                       (subset: int list)  =
            let aRet = Array.create (Degree.value degree) None 
            subset |> List.iteri(fun dex dv -> aRet.[dv] <- Some dex)
            aRet

        let _reduce (redMap:int option [])
                    (sw:switch) =
            let rpL, rpH = (redMap.[sw.low], redMap.[sw.hi])
            match rpL, rpH with
            | Some l, Some h -> Some {switch.low=l; hi=h;}
            | _ , _ -> None

        let redMap = _mapSubset degree subset
        swa |> Array.map(_reduce redMap)
            |> Array.filter(Option.isSome)
            |> Array.map(Option.get)


    // returns a sequence containing all the possible
    // degree reductions of the switch array
    let allMasks (degreeSource:degree)
                 (degreeDest:degree)
                 (swa:switch array) =
        let sd = (Degree.value degreeSource)
        let dd = (Degree.value degreeDest)
        if sd < dd then
            failwith "source degree cannot be smaller than dest"
        CollectionProps.enumNchooseM sd dd
        |> Seq.map(rebufo degreeSource swa)

    // returns a sequence containing random
    // degree reductions of the switch array
    let rndMasks (degreeSource:degree)
                 (degreeDest:degree)
                 (swa:switch array)
                 (rnd:IRando) =
        let sd = (Degree.value degreeSource)
        let dd = (Degree.value degreeDest)
        if sd < dd then
            failwith "source degree cannot be smaller than dest"
        RandGen.rndNchooseM sd dd rnd
        |> Seq.map(rebufo degreeSource swa)

