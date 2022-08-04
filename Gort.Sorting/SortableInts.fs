namespace global

    type sortableInts = private {values:int[]; symbolSetSize:symbolSetSize}
    module SortableInts =
    
        let Identity (order: order) (symbolCount:symbolSetSize) =
            let ord = (order |> Order.value)
            let sc = symbolCount |> SymbolSetSize.value |> int
            if( ord <> sc) then
                sprintf "order %d and symbolcount %d don't match" ord sc 
                                |> Error
            else
                { sortableInts.values = [|0 .. ord-1|]; 
                  symbolSetSize = symbolCount } |> Ok

        let apply f (p:sortableInts) = f p.values
        let value p = apply id p
        let getOrder (sia:sortableInts) =
            sia.values.Length |> Order.createNr
    
        let make (symbolSetSize:symbolSetSize) (vals:int[]) =
            {sortableInts.values = vals; symbolSetSize=symbolSetSize}

        let makeAllBits (order:order) =
            let symbolSetSize = 2uL |> SymbolSetSize.createNr
            let bitShift = order |> Order.value
            { 0uL .. (1uL <<< bitShift) - 1uL }
                |> Seq.map(ByteUtils.uint64To2ValArray order 0 1)
                |> Seq.map(fun arr ->
                { sortableInts.values = arr; symbolSetSize = symbolSetSize })


        let makeOrbits (maxCount:sortableCount option) 
                       (perm:permutation) =
            let symbolSetSize = perm |> Permutation.getOrder
                                     |> Order.value
                                     |> uint64
                                     |> SymbolSetSize.createNr
            let intOpt = maxCount |> Option.map SortableCount.value
            Permutation.powers intOpt perm
            |> Seq.map(Permutation.getArray)
            |> Seq.map(fun arr ->
                { sortableInts.values = arr; symbolSetSize = symbolSetSize })


        let makeSortedStacks (degStack:order[]) =
            let symbolSetSize = 2uL |> SymbolSetSize.createNr
            let stackedOrder = Order.add degStack
            CollectionOps.stackSortedBlocks degStack 1 0
            |> Seq.map(fun arr ->
                { sortableInts.values = arr; symbolSetSize = symbolSetSize })


        let makeRandomPermutation (randy:IRando) 
                                  (order:order) =
            let symbolSetSize = order |> Order.value |> uint64 |> SymbolSetSize.createNr
            { sortableInts.values = RandVars.randomPermutation randy order; 
              symbolSetSize = symbolSetSize }


        let makeRandomBits (order:order) 
                           (pctOnes:float) (randy:IRando) =
            let symbolSetSize = 2uL |> SymbolSetSize.createNr
            let arrayLength = order |> Order.value
            { sortableInts.values = RandVars.randOneOrZero pctOnes randy arrayLength
                                    |> Seq.toArray;
              symbolSetSize = symbolSetSize }


        let makeRandomSymbols (order:order) 
                              (symbolSetSize:symbolSetSize) (randy:IRando) =
            let symbolSetSize = 2uL |> SymbolSetSize.createNr
            let arrayLength = order |> Order.value
            { sortableInts.values = RandVars.randSymbols symbolSetSize randy arrayLength
                                    |> Seq.toArray;
              symbolSetSize = symbolSetSize }

