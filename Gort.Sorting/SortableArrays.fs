namespace global

    type sortableInts = 
        private { 
                    values:int[]; 
                    order:order;
                    symbolSetSize:symbolSetSize }


    type sortableBits =
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


        let makeAllBits (order:order) =
            let symbolSetSize = 2uL |> SymbolSetSize.createNr
            let bitShift = order |> Order.value
            { 0uL .. (1uL <<< bitShift) - 1uL }
                |> Seq.map(ByteUtils.uint64To2ValArray order 0 1)
                |> Seq.map(fun arr ->
                { sortableInts.values = arr;
                  order = order;
                  symbolSetSize = symbolSetSize })


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


        let makeSortedStacks (orderStack:order[]) =
            let symbolSetSize = 2uL |> SymbolSetSize.createNr
            let stackedOrder = Order.add orderStack
            CollectionOps.stackSortedBlocksOfTwoSymbols orderStack 1 0
            |> Seq.map(fun arr ->
                { sortableInts.values = arr; 
                  order = stackedOrder;
                  symbolSetSize = symbolSetSize })


        let makeRandomPermutation (order:order) 
                                  (randy:IRando) =
            let symbolSetSize = order |> Order.value |> uint64 |> SymbolSetSize.createNr
            { sortableInts.values = RandVars.randomPermutation randy order;
              order = order;
              symbolSetSize = symbolSetSize }


        let makeRandomBits (order:order) 
                           (pctOnes:float)
                           (randy:IRando) =
            let symbolSetSize = 2uL |> SymbolSetSize.createNr
            let arrayLength = order |> Order.value
            { sortableInts.values = RandVars.randOneOrZero 
                                        pctOnes randy arrayLength
                                    |> Seq.toArray;
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


        let allBitVersionsO (sortableInts:sortableInts) =
            let order = sortableInts |> getOrder |> Order.value
            let symbolMod = sortableInts.symbolSetSize |> SymbolSetSize.value |> int
            let values = sortableInts |> getValues
            seq { 0 .. symbolMod }
                |> Seq.map(fun thresh -> Array.init order 
                                          (fun dex-> if (values.[dex] >= thresh) then 1 else 0))


        let expandToSortableBitsO (sortableIntsSeq:seq<sortableInts>) =
            let order = sortableIntsSeq |> Seq.head |> getOrder
            let symbolSetSize = sortableIntsSeq |> Seq.head |> getSymbolSetSize
            sortableIntsSeq |> Seq.map(allBitVersionsO)
                            |> Seq.concat
                            |> Seq.distinct
                            |> Seq.map(make order symbolSetSize)

                            
    module SortableBits =

        let apply f (p:sortableBits) = f p.values
        let getValues p = apply id p
        let getOrder (sia:sortableBits) = sia.order
    
        let make (order:order) 
                 (vals:bool[]) =
            { sortableBits.values = vals;
              order = order; }


        let makeAllForOrder (order:order) =
            let bitShift = order |> Order.value
            { 0uL .. (1uL <<< bitShift) - 1uL }
                |> Seq.map(ByteUtils.uint64ToBoolArray order)
                |> Seq.map(fun arr ->
                    { sortableBits.values = arr; order = order;})


        let makeSortedStacks (orderStack:order[]) =
            let stackedOrder = Order.add orderStack
            CollectionOps.stackSortedBlocks orderStack
            |> Seq.map(fun arr ->
                { sortableBits.values = arr; order = stackedOrder;})


        let makeRandomBits (order:order)
                           (pctOnes:float)
                           (randy:IRando) =
            let arrayLength = order |> Order.value
            { sortableBits.values = RandVars.randBits 
                                        pctOnes randy arrayLength
                                    |> Seq.toArray;
              order = order; }

