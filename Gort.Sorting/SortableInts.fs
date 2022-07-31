namespace global

    type sortableInts = {values:int[]; symbolCount:symbolSetSize}
    module SortableInts =
    
        let Identity (order: order) (symbolCount:symbolSetSize) =
            let ord = (order |> Order.value)
            let sc = (symbolCount |> SymbolSetSize.value)
            if( ord <> sc) then
                sprintf "order %d and symbolcount %d don't match" ord sc 
                                |> Error
            else
                { sortableInts.values = [|0 .. ord-1|]; 
                  symbolCount = symbolCount } |> Ok

        let apply f (p:sortableInts) = f p.values
        let value p = apply id p
        let getOrder (sia:sortableInts) =
            sia.values.Length |> Order.createNr
    
        let makeRandom (randy:IRando) 
                       (order:order) 
                       (symbolCount:symbolSetSize) =

            let ord = (order |> Order.value)
            let sc = (symbolCount |> SymbolSetSize.value)
            match sc with
            | 1 -> { sortableInts.values = RandVars.randOneOrZero 0.5 randy ord
                                                |> Seq.toArray;
                            symbolCount = symbolCount }
            | i when i = ord -> 
                   { sortableInts.values = RandVars.randSymbols symbolCount randy ord
                                                            |> Seq.toArray;
                                   symbolCount = symbolCount }
            | _ -> { sortableInts.values = RandVars.randomPermutation randy order; 
                            symbolCount = symbolCount }
