namespace global

// a permutation of the set {0, 1,.. (order-1)}, that is it's own inverse
type twoCycle = private { values: int[] }

module TwoCycle =

    //*************************************************************
    //*******************  ctors  ******************************
    //*************************************************************

    let create (vals: int[]) =
        if CollectionProps.isTwoCycle vals then
            { twoCycle.values = vals } |> Ok
        else
            "not a two cycle" |> Error

    let fromPerm (perm: permutation) = create perm.values

    let toPerm (tc: twoCycle) = { permutation.values = tc.values }

    let create8 (b: uint8[]) = create (b |> Array.map (int))

    let create16 (b: uint16[]) = create (b |> Array.map (int))

    let getArray (tc: twoCycle) = tc.values

    let getDegree (tc: twoCycle) = Order.createNr tc.values.Length

    let identity (order: int) =
        { twoCycle.values = [| 0 .. order - 1 |] }

    let isSorted (tc: twoCycle) =
        CollectionProps.isSorted tc.values

    let makeMonoCycle (order: order) (aDex: int) (bDex: int) =
        { values =
            Array.init (Order.value order) (fun i ->
                if (i = aDex) then bDex
                elif (i = bDex) then aDex
                else i) }

    let makeAllMonoCycles (ord: order) =
        seq {
            for i = 0 to (Order.value (ord) - 1) do
                for j = 0 to i - 1 do
                    yield makeMonoCycle ord i j
        }

    let makeReflection (tc: twoCycle) =
        let _ref pos = Order.reflect (getDegree tc) pos
        { values = Array.init (tc.values.Length) (fun dex -> tc.values.[_ref dex] |> _ref) }



    //*************************************************************
    //*******************  properties  ****************************
    //*************************************************************


    let isATwoCycle (tcp: twoCycle) =
        tcp |> getArray |> CollectionProps.isTwoCycle


    let hasAfixedPoint (tcp: twoCycle) =
        tcp.values |> Seq.mapi (fun dex v -> dex = v) |> Seq.contains (true)


    let isRflSymmetric (tcp: twoCycle) = tcp = (makeReflection tcp)



    //*************************************************************
    //*******************  mutators  ******************************
    //*************************************************************

    let conjugate (tc: twoCycle) (perm: permutation) =
        { twoCycle.values =
            CollectionOps.conjIntArraysNr
                (Permutation.getArray perm)
                (getArray tc)
                (Array.zeroCreate perm.values.Length) }


    let reflect (tcp: twoCycle) =
        let _refV pos =
            Order.reflect (Order.createNr tcp.values.Length) pos

        let _refl =
            Array.init (tcp.values.Length) (fun dex -> tcp.values.[_refV dex] |> _refV)

        { twoCycle.values = _refl }


    let mutateByPair (pair: int * int) (tcp: twoCycle) =
        let tcpA = tcp |> getArray |> Array.copy
        let a, b = pair
        let c = tcpA.[a]
        let d = tcpA.[b]

        if (a = c) && (b = d) then
            tcpA.[a] <- b
            tcpA.[b] <- a
        elif (a = c) then
            tcpA.[a] <- b
            tcpA.[b] <- a
            tcpA.[d] <- d
        elif (b = d) then
            tcpA.[a] <- b
            tcpA.[b] <- a
            tcpA.[c] <- c
        else
            tcpA.[a] <- b
            tcpA.[c] <- d
            tcpA.[b] <- a
            tcpA.[d] <- c

        { twoCycle.values = tcpA }


    let mutateByReflPair (pairs: seq<(int * int)>) (tcp: twoCycle) =
        let ord = tcp.values.Length |> Order.createNr
        //true if _mutato will always turn this into another twoCyclePerm
        let _isMutatoCompatable (mut: twoCycle) =
            (CollectionProps.isTwoCycle mut.values)
            && (isRflSymmetric mut)
            && not (hasAfixedPoint mut)

        let _mutato (pair: int * int) =
            let tca = tcp |> getArray |> Array.copy
            let pA, pB = pair
            let tpA, tpB = tca.[pA], tca.[pB]
            let rA, rB = (pA |> Order.reflect ord), (pB |> Order.reflect ord)
            let rtA, rtB = (tpA |> Order.reflect ord), (tpB |> Order.reflect ord)

            tca.[pA] <- tpB
            tca.[tpB] <- pA

            tca.[pB] <- tpA
            tca.[tpA] <- pB

            tca.[rB] <- rtA
            tca.[rtA] <- rB

            tca.[rA] <- rtB
            tca.[rtB] <- rA

            { twoCycle.values = tca }

        //let muts =
        pairs
        |> Seq.map (fun pr -> _mutato pr)
        |> Seq.filter (_isMutatoCompatable)
        |> Seq.head
    //if muts.Length > 0 then
    //    muts.[0]
    //else
    //    { tcp with values = tcp |> arrayValues |> Array.copy}




    //*************************************************************
    //*******************  generators  ****************************
    //*************************************************************


    // does not error - ignores bad inputs
    let makeFromTupleSeq (ord: order) (tupes: seq<int * int>) =
        let curPa = [| 0 .. (Order.value ord) - 1 |]

        let _validTupe t =
            ((fst t) <> (snd t)) && (Order.within ord (fst t)) && (Order.within ord (snd t))

        let _usableTup t =
            (curPa.[fst (t)] = fst (t)) && (curPa.[snd (t)] = snd (t))

        let _opPa tup =
            if (_validTupe tup) && (_usableTup tup) then
                curPa.[fst (tup)] <- snd (tup)
                curPa.[snd (tup)] <- fst (tup)

        tupes |> Seq.iter (_opPa)
        { twoCycle.values = curPa }



    //*************************************************************
    //***************    IRando dependent   ***********************
    //*************************************************************

    let makeRndMonoCycle (order: order) (rnd: IRando) =
        let sc = order |> Order.value |> uint64 |> SymbolSetSize.createNr
        let tup = RandVars.drawTwoWithoutRep sc rnd
        makeMonoCycle order (fst tup) (snd tup)


    let rndPartialTwoCycle (order: order) (cycleCount: int) (rnd: IRando) =
        { twoCycle.values = RandVars.rndPartialTwoCycle rnd order cycleCount }


    let rndFullTwoCycle (order: order) (rnd: IRando) =
        { values = RandVars.rndFullTwoCycle rnd order }


    let rndSymmetric (ord: order) (rnd: IRando) =
        let deg = (Order.value ord)
        let aRet = Array.init deg (id)

        let chunkRi (rfls: switchRfl) =
            match rfls with
            | Single (i, j, d) ->
                aRet.[i] <- j
                aRet.[j] <- i

            | Unreflectable (i, j, d) ->
                aRet.[i] <- j
                aRet.[j] <- i

            | Pair ((h, i), (j, k), d) ->
                aRet.[i] <- h
                aRet.[h] <- i
                aRet.[j] <- k
                aRet.[k] <- j

            | LeftOver (i, j, d) ->
                aRet.[i] <- j
                aRet.[j] <- i

        SwitchRfl.rndReflectivePairs ord rnd |> Seq.iter (chunkRi)

        { twoCycle.values = aRet }


    let evenMode (order: order) =
        let d = (Order.value order)
        let dm = if (d % 2 > 0) then d - 1 else d

        let yak p =
            if p = dm then p
            else if (p % 2 = 0) then p + 1
            else p - 1

        { twoCycle.values = Array.init d (yak) }


    let oddMode (order: order) =
        let d = (Order.value order)
        let dm = if (d % 2 = 0) then d - 1 else d

        let yak p =
            if p = dm then p
            else if p = 0 then 0
            else if (p % 2 = 0) then p - 1
            else p + 1

        { twoCycle.values = Array.init d (yak) }


    let oddModeFromEvenDegreeWithCap (order: order) =
        let d = (Order.value order)

        let yak p =
            if p = 0 then d - 1
            else if p = d - 1 then 0
            else if (p % 2 = 0) then p - 1
            else p + 1

        { twoCycle.values = Array.init d (yak) }


    let oddModeWithCap (order: order) =
        let d = (Order.value order)

        if (d % 2 = 0) then
            oddModeFromEvenDegreeWithCap order
        else
            oddMode order


    let makeAltEvenOdd (order: order) (conj: permutation) =
        seq {
            while true do
                yield conjugate (evenMode order) conj
                yield conjugate (oddModeWithCap order) conj
        }


    let makeCoConjugateEvenOdd (conj: permutation list) =
        let order = Order.createNr conj.[0].values.Length

        let coes (conj: permutation) =
            result {
                let eve = conjugate (evenMode order) conj
                let odd = conjugate (oddModeWithCap order) conj

                return
                    seq {
                        yield eve
                        yield odd
                    }
            }

        result {
            let! rOf = conj |> List.map (fun c -> coes c) |> Result.sequence
            return rOf |> Seq.concat
        }
