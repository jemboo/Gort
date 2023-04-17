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


    let drawTwoWithoutRep 
        (symbolCount: symbolSetSize) 
        (rnd: IRando) 
        =
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



    // returns a sequence of draws from initialList without replacement.
    // Does not change initialList
    let fisherYatesReflShuffle (rnd: IRando) (initialList: array<int>) =
        let len = initialList.Length
        let order = len |> Order.createNr

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
            let retVal = initialList.[index] // and return the original item
            let reflVal = Order.reflect order retVal
            availableFlags.[reflVal] <- (reflVal, false) // Mark that index as unavailable
            retVal 

        let firstHalf = 
            seq { (len) .. -2 .. 1 } // Going from the length of the list down to 1
            |> Seq.map (fun i -> nextItem (uint32 i)) // yield the next item
            |> Seq.toArray

        let aRet = Array.zeroCreate initialList.Length
        for dex = 0 to (initialList.Length / 2 - 1) do
            aRet.[dex] <- firstHalf.[dex]
            aRet.[len - 1 - dex] <- Order.reflect order (firstHalf.[dex])
        aRet




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


type rndChoice = private RndChoice of float
module RndChoice =

    let value (RndChoice v) = v

    let create (value: float) =
        value |> RndChoice

    let choose (randy:IRando) 
               (rndChoic:rndChoice) 
               (a:'a) 
               (b:'a) =
        if (value rndChoic) > randy.NextFloat then a else b


    let delta (rndChoic:rndChoice) (amt:float) =
        let inc = (value rndChoic) + amt
        if (inc > 1.0) then 1.0 else
            if (inc < 0.0) then 0.0 else
                inc


    let mutate (randy:IRando) 
               (rndChoic:rndChoice) 
               (amt:float) =
        if (0.5) > randy.NextFloat then (delta rndChoic amt) else 
            (delta rndChoic ( -1.0 * amt))


    let randomSeq (randy:IRando) =
        Seq.initInfinite (fun _ -> create randy.NextFloat)


   
type rndChooser<'a> = private { rndChoic: rndChoice; choiceA:'a;  choiceB:'a}
    
module RndChooser =

    let choice (rndChoosr:rndChooser<'a>) = 
        rndChoosr.rndChoic

    let choiceA (rndChoosr:rndChooser<'a>) = 
        rndChoosr.choiceA

    let choiceB (rndChoosr:rndChooser<'a>) = 
        rndChoosr.choiceA

    let chooseSeq (randy:IRando) 
                  (rndChoosers:rndChooser<'a> seq) =
        rndChoosers |> Seq.map(fun rc -> RndChoice.choose randy rc.rndChoic rc.choiceA rc.choiceB)