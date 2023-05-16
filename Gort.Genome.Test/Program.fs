open System

module Program = 
    let [<EntryPoint>] main _ =
        //let yab = WsSortableSets.makeEm ()
        //let yab2 = WsSorterSets.makeEm ()
       // let yab = WsSorterSetEval.makeEm ()
        //let yab = WsSorterSetEvalReport.makeEm ()
        let yab = WsSorterSetMutate.makeEm ()
        Console.WriteLine("done")
        Console.Read() |> ignore
        0
