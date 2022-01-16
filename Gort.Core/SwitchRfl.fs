namespace global
open System


type switchRfl =
     | Single of int*int*degree
     | Unreflectable of int*int*degree
     | Pair of (int*int)*(int*int)*degree
     | LeftOver of int*int*degree


module SwitchRfl =

    let isReflSymmetric (degree:degree) (pair:int*int) =
        (pair |> fst |> Degree.reflect degree) = (snd pair)


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


    let isAFullSet (degree:degree) 
                   (rflses:switchRfl seq) = 
        let dexes = rflses |> Seq.map(getIndexes)
                           |> Seq.concat
                           |> Seq.sort
                           |> Seq.toList
        [1 .. ((Degree.value degree) - 1)] = dexes


    //makes reflective pairs to fill up degree slots.
    let rndReflectivePairs (degree:degree)
                           (rnd:IRando) =
            let _rndmx max = 
                (int rnd.NextPositiveInt) % max
            let _reflectD (dex:int) =
               dex |> Degree.reflect degree

            let _flagedArray = 
                Array.init (Degree.value degree)
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
                   (nItemA, nItemB, degree) |> switchRfl.Single
                // if one of the nodes is on the center line, then make a (non-reflective) 
                // pair out of them
                else if (nItemA = (_reflectD nItemA)) || 
                        (nItemB = (_reflectD nItemB)) then
                    (nItemA, nItemB, degree) |> switchRfl.Unreflectable
                else
                    let res = _getReflection nItemA nItemB
                    match res with
                    | Some (reflA, reflB) ->
                              ((nItemA, nItemB), (reflA, reflB), degree) 
                                    |> switchRfl.Pair
                    // if a reflective pair cannot be made from these two, then
                    // make them into a (non-reflective) pair
                    | None -> (nItemA, nItemB, degree) |> switchRfl.LeftOver

            seq { while _canContinue() do yield _nextItems() }

    // the reflectivePairs function above generates only good 
    // reflective pairs for even degree
    let goodRndReflectivePairs (degree:degree)
                               (rnd:IRando) =
         seq { 
            while true do
                 let rfpa = rndReflectivePairs degree rnd          
                            |> Seq.toArray
                 if (rfpa |> Array.forall (isGood)) then
                    yield rfpa
              }




