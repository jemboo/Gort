open System

module Program = 
    let [<EntryPoint>] main _ =
        WsSorterSetsRnd.makeEm() |> ignore
        Console.WriteLine("done ...")
        Console.Read() |> ignore
        0