open System

//module Program = let [<EntryPoint>] main _ = 0

[<EntryPoint>]
let main argv =
    printfn "%A" "Hi"
    Console.Read() |> ignore
    0
