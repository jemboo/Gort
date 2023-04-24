open System

module Program = let [<EntryPoint>] main _ =

    let yab = WsStandardSorterSets.makeEm ()

    Console.WriteLine("crap")
    Console.Read() |> ignore
    0
