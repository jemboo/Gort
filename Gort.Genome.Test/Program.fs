open System

module Program = 
    let [<EntryPoint>] main _ =
        //let yab = WsBinarySortableSets.makeEm ()
        //let yab2 = WsSorterSets.makeEm ()
        //let yab = WsSorterSetEval.makeEm ()
        let yab = WsSorterSetEvalReport.makeEm ()
        Console.WriteLine("crap")
        Console.Read() |> ignore
        0
