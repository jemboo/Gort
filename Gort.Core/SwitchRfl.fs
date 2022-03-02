namespace global
open System


type switchRfl =
     | Single of int*int*order
     | Unreflectable of int*int*order
     | Pair of (int*int)*(int*int)*order
     | LeftOver of int*int*order


module SwitchRfl =

    let isReflSymmetric (order:order) (pair:int*int) =
        (pair |> fst |> Order.reflect order) = (snd pair)


    let getIndexes (rfls:switchRfl) =
        match rfls with
        | Single (i, j, d)         ->  seq { i; j; }
        | Unreflectable (i, j, d)  ->  seq { i; j; }
        | Pair ((h, i), (j, k), d) ->  seq { h; i; j; k; }
        | LeftOver (i, j, d)       ->  seq { i; j; }


    let isGood (rfls:switchRfl) =
        match rfls with
        | Single _         ->  true
        | Unreflectable _  ->  false
        | Pair _           ->  true
        | LeftOver _       ->  false


    let isAFullSet (order:order) 
                   (rflses:switchRfl seq) = 
        let dexes = rflses |> Seq.map(getIndexes)
                           |> Seq.concat
                           |> Seq.sort
                           |> Seq.toList
        [1 .. ((Order.value order) - 1)] = dexes


    //makes reflective pairs to fill up order slots.
    let rndReflectivePairs (order:order)
                           (rnd:IRando) =
            let _rndmx max = 
                (int rnd.NextPositiveInt) % max
            let _reflectD (dex:int) =
               dex |> Order.reflect order

            let _flagedArray = 
                Array.init (Order.value order)
                           (fun i -> (i, true))

            let _availableFlags() =
                _flagedArray                         
                |> Seq.filter (fun (ndx,f) -> f)

            let _canContinue() =
                _availableFlags() |> Seq.length > 1

            let _nextItem() =
                let nItem = _rndmx (_availableFlags() |> Seq.length)     
                let index =                          
                    _availableFlags()
                    |> Seq.item (int nItem)             
                    |> fst   
                _flagedArray.[index] <- (index, false) 
                index

            let _getReflection (a:int) (b:int) =
                let aR = _reflectD a
                let bR = _reflectD b
                if (snd _flagedArray.[aR]) && (snd _flagedArray.[bR]) then
                    _flagedArray.[aR] <- (aR, false) 
                    _flagedArray.[bR] <- (bR, false) 
                    Some (bR, aR)
                else    
                    None

            let _nextItems() =
                let nItemA = _nextItem()
                let nItemB = _nextItem()
                if nItemA = (_reflectD nItemB) then
                   (nItemA, nItemB, order) |> switchRfl.Single
                // if one of the nodes is on the center line, then make a (non-reflective) 
                // pair out of them
                else if (nItemA = (_reflectD nItemA)) || 
                        (nItemB = (_reflectD nItemB)) then
                    (nItemA, nItemB, order) |> switchRfl.Unreflectable
                else
                    let res = _getReflection nItemA nItemB
                    match res with
                    | Some (reflA, reflB) ->
                              ((nItemA, nItemB), (reflA, reflB), order) 
                                    |> switchRfl.Pair
                    // if a reflective pair cannot be made from these two, then
                    // make them into a (non-reflective) pair
                    | None -> (nItemA, nItemB, order) |> switchRfl.LeftOver

            seq { while _canContinue() do yield _nextItems() }

    // the reflectivePairs function above generates only good 
    // reflective pairs for even order
    let goodRndReflectivePairs (order:order)
                               (rnd:IRando) =
         seq { 
            while true do
                 let rfpa = rndReflectivePairs order rnd          
                            |> Seq.toArray
                 if (rfpa |> Array.forall (isGood)) then
                    yield rfpa
              }




