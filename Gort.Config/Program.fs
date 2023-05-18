open System

module Program = 
    let [<EntryPoint>] main _ =


        WsOps.makeEm() |> ignore
        Console.WriteLine("done ...")
        Console.Read() |> ignore
        0