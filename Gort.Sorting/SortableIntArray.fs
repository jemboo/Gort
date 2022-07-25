namespace global

    type sortableIntArray = {values:int[]}
    module SortableIntArray =
    
        let Identity (order: order) = 
            let arrLen = order |> Order.value
            { sortableIntArray.values = [|0 .. arrLen-1|] }
        let apply f (p:sortableIntArray) = f p.values
        let value p = apply id p
        let getOrder (sia:sortableIntArray) =
            sia.values.Length |> Order.createNr
    

