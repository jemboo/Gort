namespace global

    type sortableIntArray = 
        private { 
                    values:int[]; 
                    order:order;
                    symbolSetSize:symbolSetSize }


    type sortableBoolArray =
        private { 
                  values:bool[]; 
                  order:order    }


    module SortableIntArray =
    
        let Identity (order: order) (symbolCount:symbolSetSize) =
            let ordV = (order |> Order.value)
            let sc = symbolCount |> SymbolSetSize.value |> int
            if( ordV <> sc) then
                sprintf "order %d and symbolcount %d don't match" ordV sc 
                                |> Error
            else
                { sortableIntArray.values = [|0 .. ordV-1|]; 
                  order = order;
                  symbolSetSize = symbolCount } |> Ok

        let apply f (p:sortableIntArray) = f p.values
        let value p = apply id p
        let getValues (sia:sortableIntArray) = sia.values
        let getOrder (sia:sortableIntArray) = sia.order
        let getSymbolSetSize (sia:sortableIntArray) = sia.symbolSetSize
    

        let make (order:order) 
                 (symbolSetSize:symbolSetSize) 
                 (vals:int[]) =
            { sortableIntArray.values = vals;
              order = order;
              symbolSetSize = symbolSetSize}


        let copy (toCopy:sortableIntArray) =
            { toCopy with values = toCopy.values |> Array.copy }


        let isSorted (sortableInts:sortableIntArray) =
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
                { sortableIntArray.values = arr;
                  order = order;
                  symbolSetSize = symbolSetSize })


        let makeRandomPermutation (order:order) 
                                  (randy:IRando) =
            let symbolSetSize = order |> Order.value |> uint64 |> SymbolSetSize.createNr
            { sortableIntArray.values = RandVars.randomPermutation randy order;
              order = order;
              symbolSetSize = symbolSetSize }


        let makeRandomSymbol (order:order) 
                             (symbolSetSize:symbolSetSize) 
                             (randy:IRando) =
            let arrayLength = order |> Order.value
            let intArray =
                 RandVars.randSymbols 
                      symbolSetSize randy arrayLength
                 |> Seq.toArray

            { sortableIntArray.values = intArray;
              order = order;
              symbolSetSize = symbolSetSize }

        
        let makeRandomSymbols (order:order) 
                              (symbolSetSize:symbolSetSize) 
                              (rnd:IRando) =
            seq { while true do 
                    yield makeRandomSymbol order symbolSetSize rnd }



    module SortableBoolArray =

        let apply f (p:sortableBoolArray) = f p.values
        let getValues p = apply id p
        let getOrder (sia:sortableBoolArray) = sia.order
    
        let make (order:order) 
                 (vals:bool[]) =
            { sortableBoolArray.values = vals;
              order = order; }

        
        let copy (toCopy:sortableBoolArray) =
            { toCopy with values = toCopy.values |> Array.copy }


        let isSorted (sortableBools:sortableBoolArray) =
            sortableBools |> getValues 
                          |> CollectionProps.isSorted_idiom 


        let makeAllBits (order:order) =
            let symbolSetSize = 2uL |> SymbolSetSize.createNr
            let bitShift = order |> Order.value
            { 0uL .. (1uL <<< bitShift) - 1uL }
                |> Seq.map(ByteUtils.uint64ToBoolArray order)
                |> Seq.map(fun arr ->
                { sortableBoolArray.values = arr;
                  order = order;})


        let makeAllForOrder (order:order) =
            let bitShift = order |> Order.value
            { 0uL .. (1uL <<< bitShift) - 1uL }
                |> Seq.map(ByteUtils.uint64ToBoolArray order)
                |> Seq.map(fun arr ->
                    { sortableBoolArray.values = arr; order = order;})


        let makeSortedStacks (orderStack:order[]) =
            let stackedOrder = Order.add orderStack
            CollectionOps.stackSortedBlocks orderStack
            |> Seq.map(fun arr ->
                { sortableBoolArray.values = arr; order = stackedOrder;})


        let makeRandomBits (order:order)
                           (pctTrue:float)
                           (randy:IRando) =
            let arrayLength = order |> Order.value
            Seq.initInfinite (fun _ ->
                { sortableBoolArray.values = RandVars.randBits pctTrue randy arrayLength
                                          |> Seq.toArray;
                  order = order; } 
              )


        let allBooleanVersions (sortableInts:sortableIntArray) =
            let order = sortableInts |> SortableIntArray.getOrder |> Order.value
            let values = sortableInts |> SortableIntArray.getValues
            let threshHolds = 
                    values |> Set.ofArray |> Set.toArray 
                    |> Array.sort |> Array.skip 1
            threshHolds
                |> Seq.map(fun thresh ->
                    Array.init order (fun dex ->
                        if (values.[dex] >= thresh) then true else false))



        let expandToSortableBits (sortableIntsSeq:seq<sortableIntArray>) =
            let order = sortableIntsSeq |> Seq.head |> SortableIntArray.getOrder
            sortableIntsSeq |> Seq.map(allBooleanVersions)
                            |> Seq.concat
                            |> Seq.distinct
                            |> Seq.map(make order)

                            