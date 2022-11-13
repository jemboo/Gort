namespace global

open System
open SysExt


module CollectionOps =

    let takeUpto<'a> (maxCt: int) (source: seq<'a>) =
        source
        |> Seq.mapi (fun dex v -> (dex, v))
        |> Seq.takeWhile (fun tup -> (fst tup) < maxCt)
        |> Seq.map (snd)


    // product map composition: a(b()).
    let inline arrayProduct< ^a when ^a:(static member op_Explicit:^a->int)>
                    (lhs: array<^a>) (rhs: array<^a>) (prod: array<^a>) =
        let dmax = lhs.Length
        //let mutable curdex = zero_of lhs.[0]
        let mutable curdex = 0
        while curdex < dmax do
            prod.[curdex] <- lhs.[rhs.[curdex] |> int]
            curdex <- curdex + 1
        prod

        
    let arrayProductR (lhs: array<int>) (rhs: array<int>) (prod: array<int>) =
        try
            arrayProduct lhs rhs prod |> Ok
        with ex ->
            ("error in compIntArrays: " + ex.Message) |> Result.Error


    let allPowers (a_core: array<int>) =
        seq {
            let mutable _continue = true
            let mutable a_cur = Array.copy a_core
            yield a_cur

            while _continue do
                let a_next = Array.zeroCreate a_cur.Length
                a_cur <- arrayProduct a_core a_cur a_next
                _continue <- not (CollectionProps.isIdentity a_next)
                yield a_cur
        }

    let allPowersCapped (maxCount: int) (a_core: array<int>) =
        seq {
            let mutable _continue = true
            let mutable dex = 0
            let mutable a_cur = Array.copy a_core

            while (_continue && (dex < maxCount)) do
                yield a_cur
                let a_next = Array.zeroCreate a_cur.Length
                a_cur <- arrayProduct a_core a_cur a_next
                _continue <- not (CollectionProps.isIdentity a_next)
                dex <- dex + 1
        }

    let conjIntArraysNr (a_conj: array<int>) (a_core: array<int>) (a_out: array<int>) =
        for i = 0 to a_conj.Length - 1 do
            a_out.[a_conj.[i]] <- a_conj.[a_core.[i]]
        a_out


    // a_conj * a_core * (a_conj ^ -1)
    let conjIntArrays (a_conj: array<int>) (a_core: array<int>) =
        try
            let a_out = Array.zeroCreate a_conj.Length
            conjIntArraysNr a_conj a_core a_out |> Ok
        with ex ->
            ("error in conjIntArrays: " + ex.Message) |> Result.Error


    let filterByPickList (data: 'a[]) (picks: bool[]) =
        try
            let pickCount = picks |> Array.map (fun v -> if v then 1 else 0) |> Array.sum
            let filtAr = Array.zeroCreate pickCount
            let mutable newDex = 0

            for i = 0 to (data.Length - 1) do
                if picks.[i] then
                    filtAr.[newDex] <- data.[i]
                    newDex <- newDex + 1

            filtAr |> Ok
        with ex ->
            ("error in filterByPickList: " + ex.Message) |> Result.Error


    let inline invertArray< ^a when ^a:(static member Zero:^a) and 
                                    ^a:(static member One:^a) and  
                                    ^a:(static member (+):^a* ^a -> ^a) and 
                                    ^a:(static member op_Explicit:^a->int)>
                    (ar: array<^a>) (inv_out: array<^a>) =
        let mutable iv = zero_of ar.[0]
        let incr = one_of ar.[0]          
        for i = 0 to ar.Length - 1 do
            inv_out.[ar.[i] |> int] <- iv
            iv <- iv + incr
        inv_out


    let invertArrayR (a: array<int>) (inv_out: array<int>) =
        try
            invertArray a inv_out |> Ok
        with ex ->
            ("error in inverseMapArray: " + ex.Message) |> Result.Error


    // a_conj * a_core * (a_conj ^ -1)
    let conjIntArraysR (a_conj: array<int>) (a_core: array<int>) =
        result {
            let! a_conj_inv = invertArrayR a_conj (Array.zeroCreate a_core.Length)
            let! rhs = arrayProductR a_core a_conj_inv (Array.zeroCreate a_core.Length)
            return! arrayProductR a_conj rhs (Array.zeroCreate a_core.Length)
        }


    let histogram<'d, 'r when 'r: comparison> (keymaker: 'd -> 'r) (qua: seq<'d>) =
        qua
        |> Seq.fold
            (fun acc fv ->
                let kk = keymaker fv

                if Map.containsKey kk acc then
                    Map.add kk (acc.[kk] + 1) acc
                else
                    Map.add kk 1 acc)
            Map.empty



    //*************************************************************
    //***********    Array Stacking    ****************************
    //*************************************************************

    let stack (lowTohi: 'a[] seq) = lowTohi |> Seq.concat |> Seq.toArray


    let comboStack (subSeqs: 'a[][] seq) =
        let rec _cart LL =
            match LL with
            | [] -> Seq.singleton []
            | L :: Ls ->
                seq {
                    for x in L do
                        for xs in _cart Ls -> x :: xs
                }

        _cart (subSeqs |> Seq.toList) |> Seq.map (stack)


    let stackSortedBlocksOfTwoSymbols (blockSizes: order seq) (hival: 'a) (lowval: 'a) =
        let _allSorted (deg: order) =
            Order.allTwoSymbolOrderedArrays deg hival lowval

        blockSizes |> Seq.map (_allSorted >> Seq.toArray) |> comboStack


    let stackSortedBlocks (blockSizes: order seq) =
        let _allSorted (deg: order) =
            Order.allTwoSymbolOrderedArrays deg true false

        blockSizes |> Seq.map (_allSorted >> Seq.toArray) |> comboStack



    //*************************************************************
    //************    Split Sequence   ****************************
    //*************************************************************

    let chunkByDelimiter<'a> (strm: seq<'a>) (delim: 'a -> bool) =
        let swEnumer = strm.GetEnumerator()
        let mutable rz = ResizeArray()

        seq {
            while swEnumer.MoveNext() do
                rz.Add swEnumer.Current

                if delim swEnumer.Current then
                    yield rz.ToArray()
                    rz <- ResizeArray()
        }


    // returns an array of length chunkSz, which is made by converting vals to a
    // 2d array with chunkSz columns, and then summing over each column.
    let wrapAndSumCols (chunkSz: int) (vals: seq<int>) =
        let addArrays (a: int[]) (b: int[]) =
            Array.init a.Length (fun dex -> a.[dex] + b.[dex])

        vals |> Seq.chunkBySize chunkSz |> Seq.toArray |> Array.reduce addArrays
