namespace global

open System

module RandVars =

    let rndGuidsLcg (gud:Guid) =
        let rndGud = RndGuid.makeLcg gud
        Seq.initInfinite(fun _ -> RndGuid.nextGuid rndGud)

    let rndBitsUint64 (order: order) (rnd: IRando) =
        rnd.NextULong &&& (order |> Order.bitMaskUint64)


    let polarBoxMullerDist meanX stdDevX meanY stdDevY (rnd: IRando) =
        let rec getRands () =
            let u = (2.0 * rnd.NextFloat) - 1.0
            let v = (2.0 * rnd.NextFloat) - 1.0
            let w = u * u + v * v
            if w >= 1.0 then getRands () else u, v, w

        let u, v, w = getRands ()

        let scale = System.Math.Sqrt(-2.0 * System.Math.Log(w) / w)
        let x = scale * u
        let y = scale * v
        (meanX + x * stdDevX, meanY + y * stdDevY)


    let randOneOrZero (pctOnes: float) (rnd: IRando) (len: int) =
        Seq.init len (fun _ -> if (rnd.NextFloat > pctOnes) then 0 else 1)


    let randBits (pctTrue: float) (rnd: IRando) (len: int) =
        Seq.init len (fun _ -> if (rnd.NextFloat > pctTrue) then false else true)


    let randSymbols (symbolCount: symbolSetSize) (rnd: IRando) (len: int) =
        let sc = symbolCount |> SymbolSetSize.value |> int
        Seq.init len (fun _ -> (rnd.NextPositiveInt % sc))


    let drawTwoWithoutRep (symbolCount: symbolSetSize) (rnd: IRando) =
        let sc = symbolCount |> SymbolSetSize.value |> int
        let aBit = rnd.NextPositiveInt % sc
        let mutable bBit = rnd.NextPositiveInt % sc

        while aBit = bBit do
            bBit <- rnd.NextPositiveInt % sc

        if aBit < bBit then aBit, bBit else bBit, aBit


    let fromWeightedDistribution (weightFunction: float -> float) (rnd: IRando) (items: float[]) =
        let bins = items |> Array.map (weightFunction) |> CollectionProps.asCumulative 0.0

        let _nextBin () =
            let nextSamp = rnd.NextFloat * bins.[bins.Length - 1]
            let nextIndex = bins |> Array.findIndexBack (fun av -> av < nextSamp)
            nextIndex

        seq {
            while true do
                items.[_nextBin ()]
        }


    // returns a sequence of draws from initialList without replacement.
    // Does not change initialList
    let fisherYatesShuffle (rnd: IRando) (initialList: array<'a>) =
        let rndmx max = rnd.NextUInt % max
        let availableFlags = Array.init initialList.Length (fun i -> (i, true))

        let nextItem nLeft =
            let nItem = (rndmx nLeft) // Index out of available items

            let index = // Index in original deck
                availableFlags // Go through available array
                |> Seq.filter (fun (ndx, f) -> f) // and pick out only the available tuples
                |> Seq.item (int nItem) // Get the one at our chosen index
                |> fst // and retrieve it's index into the original array

            availableFlags.[index] <- (index, false) // Mark that index as unavailable
            initialList.[index] // and return the original item

        seq { (initialList.Length) .. -1 .. 1 } // Going from the length of the list down to 1
        |> Seq.map (fun i -> nextItem (uint32 i)) // yield the next item


    let randomPermutation (rnd: IRando) (order: order) =
        (fisherYatesShuffle rnd) [| 0 .. (Order.value (order) - 1) |] |> Seq.toArray


    let randomPermutations (rnd: IRando) (order: order) =
        Seq.initInfinite (fun _ -> randomPermutation rnd order)


    let rndPartialTwoCycle (rnd: IRando) (order: order) (cycleCount: int) =
        let rndTupes = randomPermutation rnd order 
                        |> (Seq.chunkBySize 2) |> Seq.toArray
        
        let arrayRet = Array.init (Order.value order) (id)
        for i = 0 to cycleCount - 1 do
            arrayRet.[rndTupes.[i].[0]] <- rndTupes.[i].[1]
            arrayRet.[rndTupes.[i].[1]] <- rndTupes.[i].[0]
        arrayRet


    let rndFullTwoCycle (rnd: IRando) (order: order) =
        rndPartialTwoCycle rnd order (order |> Order.maxSwitchesPerStage)


    // returns a sequence of int[] of length m, made by drawing m
    // items out of [0 .. (n-1)] without replacement, with the m items
    // always being ordered from smallest to largest.
    let rndNchooseM (n: int) (m: int) (rnd: IRando) =
        let _capN (n: int) (cap: int) (srcA: int[]) =
            seq {
                for i in 0 .. (n - 1) do
                    if (srcA.[i] < cap) then
                        yield i
            }
            |> Seq.toList

        randomPermutations rnd (Order.createNr n) |> Seq.map (_capN n m)


    let binomial (rnd: IRando) (freq: float) (numDraws: int) =
            let _draw (randy: IRando) = if randy.NextFloat < freq then 1 else 0
            let mutable i = 0
            let mutable successCount = 0
            while (i < numDraws) do
                successCount <- successCount + _draw rnd
                i <- i + 1
            successCount
