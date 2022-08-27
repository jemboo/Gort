namespace global

    type sortableInts = 
        private { 
                    values:int[]; 
                    order:order;
                    symbolSetSize:symbolSetSize }


    type sortableBools =
        private { 
                    values:bool[]; 
                    order:order    }


    module SortableInts =
    
        let Identity (order: order) (symbolCount:symbolSetSize) =
            let ordV = (order |> Order.value)
            let sc = symbolCount |> SymbolSetSize.value |> int
            if( ordV <> sc) then
                sprintf "order %d and symbolcount %d don't match" ordV sc 
                                |> Error
            else
                { sortableInts.values = [|0 .. ordV-1|]; 
                  order = order;
                  symbolSetSize = symbolCount } |> Ok

        let apply f (p:sortableInts) = f p.values
        let value p = apply id p
        let getValues (sia:sortableInts) = sia.values
        let getOrder (sia:sortableInts) = sia.order
        let getSymbolSetSize (sia:sortableInts) = sia.symbolSetSize
    
        let make (order:order) 
                 (symbolSetSize:symbolSetSize) 
                 (vals:int[]) =
            { sortableInts.values = vals;
              order = order;
              symbolSetSize = symbolSetSize}


        let copy (toCopy:sortableInts) =
            { toCopy with values = toCopy.values |> Array.copy }


        let isSorted (sortableInts:sortableInts) =
            sortableInts |> getValues 
                         |> CollectionProps.isSorted_idiom 

        let makeOrbits (maxCount:sortableCount option) 
                       (perm:permutation) =
            let order = perm |> Permutation.getOrder
            let symbolSetSize = order |> Order.value
                                      |> uint64
                                      |> SymbolSetSize.createNr
            let intOpt = maxCount |> Option.map SortableCount.value
            Permutation.powers intOpt perm
            |> Seq.map(Permutation.getArray)
            |> Seq.map(fun arr ->
                { sortableInts.values = arr;
                  order = order;
                  symbolSetSize = symbolSetSize })


        let makeRandomPermutation (order:order) 
                                  (randy:IRando) =
            let symbolSetSize = order |> Order.value |> uint64 |> SymbolSetSize.createNr
            { sortableInts.values = RandVars.randomPermutation randy order;
              order = order;
              symbolSetSize = symbolSetSize }


        let makeRandomSymbols (order:order) 
                              (symbolSetSize:symbolSetSize) 
                              (randy:IRando) =
            let arrayLength = order |> Order.value
            { sortableInts.values = RandVars.randSymbols 
                                        symbolSetSize randy arrayLength
                                    |> Seq.toArray;
              order = order;
              symbolSetSize = symbolSetSize }

        
        let makeRandomSymbolsSeq (order:order) 
                                 (symbolSetSize:symbolSetSize) 
                                 (rnd:IRando) =
            seq { while true do 
                    yield makeRandomSymbols order symbolSetSize rnd }



    module SortableBools =

        let apply f (p:sortableBools) = f p.values
        let getValues p = apply id p
        let getOrder (sia:sortableBools) = sia.order
    
        let make (order:order) 
                 (vals:bool[]) =
            { sortableBools.values = vals;
              order = order; }

        
        let copy (toCopy:sortableBools) =
            { toCopy with values = toCopy.values |> Array.copy }


        let isSorted (sortableBools:sortableBools) =
            sortableBools |> getValues 
                          |> CollectionProps.isSorted_idiom 


        let makeAllBits (order:order) =
            let symbolSetSize = 2uL |> SymbolSetSize.createNr
            let bitShift = order |> Order.value
            { 0uL .. (1uL <<< bitShift) - 1uL }
                |> Seq.map(ByteUtils.uint64ToBoolArray order)
                |> Seq.map(fun arr ->
                { sortableBools.values = arr;
                  order = order;})


        let makeAllForOrder (order:order) =
            let bitShift = order |> Order.value
            { 0uL .. (1uL <<< bitShift) - 1uL }
                |> Seq.map(ByteUtils.uint64ToBoolArray order)
                |> Seq.map(fun arr ->
                    { sortableBools.values = arr; order = order;})


        let makeSortedStacks (orderStack:order[]) =
            let stackedOrder = Order.add orderStack
            CollectionOps.stackSortedBlocks orderStack
            |> Seq.map(fun arr ->
                { sortableBools.values = arr; order = stackedOrder;})


        let makeRandomBits (order:order)
                           (pctTrue:float)
                           (randy:IRando) =
            let arrayLength = order |> Order.value
            { sortableBools.values = RandVars.randBits 
                                        pctTrue randy arrayLength
                                    |> Seq.toArray;
              order = order; }


        let allBitVersions (sortableInts:sortableInts) =
            let order = sortableInts |> SortableInts.getOrder |> Order.value
            let symbolMod = sortableInts.symbolSetSize |> SymbolSetSize.value |> int
            let values = sortableInts |> SortableInts.getValues
            seq { 0 .. symbolMod }
                |> Seq.map(fun thresh ->
                    Array.init order 
                                (fun dex-> if (values.[dex] >= thresh) then true else false))


        let expandToSortableBits (sortableIntsSeq:seq<sortableInts>) =
            let order = sortableIntsSeq |> Seq.head |> SortableInts.getOrder
            sortableIntsSeq |> Seq.map(allBitVersions)
                            |> Seq.concat
                            |> Seq.distinct
                            |> Seq.map(make order)

                            