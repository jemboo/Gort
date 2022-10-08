namespace global

open System

[<Struct>]
type switch = { low: int; hi: int }

module Switch =

    let toString (sw: switch) = sprintf "(%d, %d)" sw.low sw.hi

    let maxDegree = 255

    let switchMap =
        [ for hi = 0 to maxDegree do
              for low = 0 to hi do
                  yield { switch.low = low; hi = hi } ]

    let mapIndexUb = switchMap.Length

    let maxSwitchIndexForOrder (ord: order) =
        uint32 ((Order.value ord) * (Order.value ord + 1) / 2)

    let fromSwitchIndexes (dexes: int seq) =
        dexes |> Seq.map (fun dex -> switchMap.[dex])

    let removeDegenerateIndexes (dexes: int seq) =
        dexes |> fromSwitchIndexes |> Seq.filter (fun sw -> sw.low <> sw.hi)

    let getIndex (switch: switch) =
        (switch.hi * (switch.hi + 1)) / 2 + switch.low

    // all switch indexes for order with lowVal
    let lowOverlapping (ord: order) (lowVal: int) =
        seq {
            for hv = (lowVal + 1) to (Order.value ord) - 1 do
                yield (hv * (hv + 1)) / 2 + lowVal
        }

    // all switch indexes for order with hiVal
    let hiOverlapping (ord: order) (hiVal: int) =
        seq {
            for lv = 0 to (hiVal - 1) do
                yield (hiVal * (hiVal + 1)) / 2 + lv
        }

    let zeroSwitches =
        seq {
            while true do
                yield { switch.low = 0; hi = 0 }
        }

    // produces switches the two cycles in the permutation
    let extractFromInts (pArray: int[]) =
        seq {
            for i = 0 to pArray.Length - 1 do
                let j = pArray.[i]

                if ((j > i) && (i = pArray.[j])) then
                    yield { switch.low = i; hi = j }
        }

    let fromPermutation (p: permutation) =
        extractFromInts (Permutation.getArray p)

    let fromTwoCycle (tc: twoCycle) = extractFromInts (TwoCycle.getArray tc)

    let makeAltEvenOdd (order: order) (stageCt: stageCount) =
        result {
            let stages =
                TwoCycle.makeAltEvenOdd order (Permutation.identity order)
                |> Seq.take (StageCount.value stageCt)

            return stages |> Seq.map (fromTwoCycle) |> Seq.concat
        }

    // IRando dependent
    let rndNonDegenSwitchesOfDegree (order: order) (rnd: IRando) =
        let maxDex = maxSwitchIndexForOrder order

        seq {
            while true do
                let p = (int (rnd.NextUInt % maxDex))
                let sw = switchMap.[p]

                if (sw.low <> sw.hi) then
                    yield sw
        }

    let rndSwitchesOfDegree (order: order) (rnd: IRando) =
        let maxDex = maxSwitchIndexForOrder order

        seq {
            while true do
                let p = (int (rnd.NextUInt % maxDex))
                yield switchMap.[p]
        }


    let rndSymmetric (order: order) (rnd: IRando) =
        let aa (rnd: IRando) =
            (TwoCycle.rndSymmetric order rnd) |> fromTwoCycle

        seq {
            while true do
                yield! (aa rnd)
        }


    let mutateSwitches (order: order) (mutationRate: switchMutationRate) (rnd: IRando) (switches: seq<switch>) =
        let mDex = uint32 ((Order.value order) * (Order.value order + 1) / 2)

        let mutateSwitch (switch: switch) =
            match rnd.NextFloat with
            | k when k < (SwitchMutationRate.value mutationRate) -> switchMap.[(int (rnd.NextUInt % mDex))]
            | _ -> switch

        switches |> Seq.map (fun sw -> mutateSwitch sw)


    let reflect (ord: order) (sw: switch) =
        { switch.low = sw.hi |> Order.reflect ord
          switch.hi = sw.low |> Order.reflect ord }



    // filters the switchArray, removing switches that compare
    // indexes that are not in subset. It then relabels the indexes
    // according to the subset. Ex, if the subset was [2;5;8], then
    // index 2 -> 0; index 5-> 1; index 8 -> 2
    let rebufo (order: order) (swa: switch array) (subset: int list) =

        let _mapSubset (order: order) (subset: int list) =
            let aRet = Array.create (Order.value order) None
            subset |> List.iteri (fun dex dv -> aRet.[dv] <- Some dex)
            aRet

        let _reduce (redMap: int option[]) (sw: switch) =
            let rpL, rpH = (redMap.[sw.low], redMap.[sw.hi])

            match rpL, rpH with
            | Some l, Some h -> Some { switch.low = l; hi = h }
            | _, _ -> None

        let redMap = _mapSubset order subset

        swa
        |> Array.map (_reduce redMap)
        |> Array.filter (Option.isSome)
        |> Array.map (Option.get)


    // returns a sequence containing all the possible
    // order reductions of the switch array
    let allMasks (orderSource: order) (orderDest: order) (swa: switch array) =
        let sd = (Order.value orderSource)
        let dd = (Order.value orderDest)

        if sd < dd then
            failwith "source order cannot be smaller than dest"

        CollectionProps.enumNchooseM sd dd |> Seq.map (rebufo orderSource swa)

    // returns a sequence containing random
    // order reductions of the switch array
    let rndMasks (orderSource: order) (orderDest: order) (swa: switch array) (rnd: IRando) =
        let sd = (Order.value orderSource)
        let dd = (Order.value orderDest)

        if sd < dd then
            failwith "source order cannot be smaller than dest"

        RandVars.rndNchooseM sd dd rnd |> Seq.map (rebufo orderSource swa)
