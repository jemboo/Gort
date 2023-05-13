open System

module Program = 
    let [<EntryPoint>] main _ =
        //let yab = WsSortableSets.makeEm ()
        //let yab2 = WsSorterSets.makeEm ()
       // let yab = WsSorterSetEval.makeEm ()
        let yab = WsSorterSetEvalReport.makeEm ()
        //let yab = WsMutateSorterSets.makeEm ()
        Console.WriteLine("crap")
        Console.Read() |> ignore
        0
