namespace global
open System


module CollectionOps =

    let takeUpto<'a> (maxCt:int) (source:seq<'a>) = 
        source |> Seq.mapi(fun dex v -> (dex, v))
               |> Seq.takeWhile(fun tup -> (fst tup) < maxCt)
               |> Seq.map(snd)


    // product map composition: a(b()).
    let arrayProductInt (lhs:array<int>) 
                        (rhs:array<int>) 
                        (prod:array<int>) =
        for i = 0 to lhs.Length - 1 do
            prod.[i] <- lhs.[rhs.[i]]
        prod


    // product map composition: a(b()).
    let arrayProduct16 (lhs:array<uint16>) 
                       (rhs:array<uint16>) 
                       (prod:array<uint16>) =
           let dmax = lhs.Length |> uint16
           let mutable curdex = 0us
           while curdex < dmax do
               let i = curdex |> int
               prod.[i] <- lhs.[rhs.[i] |> int]
               curdex <- curdex + 1us
           prod


    // product map composition: a(b()).
    let arrayProduct8 (lhs:array<uint8>) 
                      (rhs:array<uint8>) 
                      (prod:array<uint8>) =
           let dmax = lhs.Length |> uint8
           let mutable curdex = 0uy
           while curdex < dmax do
               let i = curdex |> int
               prod.[i] <- lhs.[rhs.[i] |> int]
               curdex <- curdex + 1uy
           prod


    let arrayProductIntR (lhs:array<int>)
                         (rhs:array<int>) 
                         (prod:array<int>) =
        try
            arrayProductInt lhs rhs prod |> Ok
        with
            | ex -> ("error in compIntArrays: " + ex.Message ) 
                    |> Result.Error

    let allPowers (a_core:array<int>) =
        seq {
            let mutable _continue = true
            let mutable a_cur = Array.copy a_core
            yield a_cur 
            while _continue do
                let a_next = Array.zeroCreate a_cur.Length
                a_cur <- arrayProductInt a_core a_cur a_next
                _continue <- not (CollectionProps.isIdentity a_next)
                yield a_cur
        }

    let allPowersCapped (maxCount:int) (a_core:array<int>) =
        seq {
            let mutable _continue = true
            let mutable dex = 0
            let mutable a_cur = Array.copy a_core
            while ( _continue && (dex < maxCount)) do
                yield a_cur 
                let a_next = Array.zeroCreate a_cur.Length
                a_cur <- arrayProductInt a_core a_cur a_next
                _continue <- not (CollectionProps.isIdentity a_next)
                dex <- dex + 1
        }

    let conjIntArraysNr (a_conj:array<int>) 
                        (a_core:array<int>)  
                        (a_out:array<int>) =
        for i = 0 to a_conj.Length - 1 do
            a_out.[a_conj.[i]] <- a_conj.[a_core.[i]]
        a_out

        
    // a_conj * a_core * (a_conj ^ -1)
    let conjIntArrays (a_conj:array<int>) 
                      (a_core:array<int>)  =
        try
            let a_out = Array.zeroCreate a_conj.Length
            conjIntArraysNr a_conj a_core a_out |> Ok
        with
            | ex -> ("error in conjIntArrays: " + ex.Message ) 
                    |> Result.Error

    let filterByPickList (data:'a[]) (picks:bool[]) =
        try
            let pickCount = picks |> Array.map(fun v -> if v then 1 else 0)
                                  |> Array.sum
            let filtAr = Array.zeroCreate pickCount
            let mutable newDex = 0
            for i = 0 to (data.Length - 1) do
                if picks.[i] then 
                    filtAr.[newDex] <- data.[i] 
                    newDex <- newDex + 1
            filtAr |> Ok
        with
            | ex -> ("error in filterByPickList: " + ex.Message ) |> Result.Error


    let invertArrayNr (a:array<int>)
                      (inv_out:array<int>) =
        for i = 0 to a.Length - 1 do
            inv_out.[a.[i]] <- i
        inv_out


    let invertArray (a:array<int>) 
                    (inv_out:array<int>) =   
      try
          invertArrayNr a inv_out |> Ok
      with
        | ex -> ("error in inverseMapArray: " + ex.Message ) 
                |> Result.Error


    // a_conj * a_core * (a_conj ^ -1)
    let conjIntArraysR (a_conj:array<int>)  
                       (a_core:array<int>) =
        result {
            let! a_conj_inv = invertArray a_conj (Array.zeroCreate a_core.Length)
            let! rhs = arrayProductIntR a_core a_conj_inv (Array.zeroCreate a_core.Length)
            return! arrayProductIntR a_conj rhs (Array.zeroCreate a_core.Length)
        }


    let histogram<'d,'r when 'r:comparison> (keymaker:'d->'r) 
                                            (qua:seq<'d>) =
        qua
        |> Seq.fold (fun acc fv ->
                let kk = keymaker fv
                if Map.containsKey kk acc
                then Map.add kk (acc.[kk] + 1) acc
                else Map.add kk 1 acc
            ) Map.empty

    //returns a sequence of bool[], where each bool[] has an ordering
    //between it's members that does not contradict the ordering of the
    //source array.
    let allBooleanVersions (intArray:int[]) =
        None
        //let order = sortableInts |> SortableInts.getOrder |> Order.value
        //let symbolMod = sortableInts.symbolSetSize |> SymbolSetSize.value |> int
        //let values = sortableInts |> SortableInts.getValues
        //seq { 1 .. (symbolMod - 1) }
        //    |> Seq.map(fun thresh ->
        //        Array.init order 
        //                    (fun dex ->
        //        if (values.[dex] >= thresh) then true else false))



//*************************************************************
//***********    Array Stacking    ****************************
//*************************************************************

    let stack (lowTohi: 'a[] seq) =
        lowTohi |> Seq.concat
                |> Seq.toArray


    let comboStack (subSeqs: 'a[][] seq) =
        let rec _cart LL =
            match LL with
            | [] -> Seq.singleton []
            | L::Ls -> seq {for x in L do for xs in _cart Ls -> x::xs}
        _cart (subSeqs |> Seq.toList) |> Seq.map(stack)


    let stackSortedBlocksOfTwoSymbols (blockSizes:order seq) (hival:'a) (lowval:'a) =
        let _allSorted (deg:order) = 
             Order.allTwoSymbolOrderedArrays deg hival lowval
        blockSizes |> Seq.map(_allSorted >> Seq.toArray)
                   |> comboStack


    let stackSortedBlocks (blockSizes:order seq) =
        let _allSorted (deg:order) = 
             Order.allTwoSymbolOrderedArrays deg true false
        blockSizes |> Seq.map(_allSorted >> Seq.toArray)
                   |> comboStack



//*************************************************************
//************    Split Sequence   ****************************
//*************************************************************

    let chunkByDelimiter<'a> (strm:seq<'a>) (delim:'a -> bool) =
        let swEnumer = strm.GetEnumerator();
        let mutable rz = ResizeArray()

        seq {while swEnumer.MoveNext() do
                rz.Add swEnumer.Current
                if delim swEnumer.Current then
                    yield rz.ToArray()
                    rz <- ResizeArray() }


    // returns an array of length chunkSz, which is made by converting vals to a
    // 2d array with chunkSz columns, and then summing over each column. 
    let wrapAndSumCols (chunkSz:int) (vals:seq<int>) =
        let addArrays (a:int[]) (b:int[]) =
            Array.init a.Length (fun dex -> a.[dex] + b.[dex])

        vals |> Seq.chunkBySize chunkSz
             |> Seq.toArray
             |> Array.reduce addArrays