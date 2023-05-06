namespace global
open System


module WsCommon = 

    let useParall = true |> UseParallel.create

    let wsRootDir = "c:\\GortFiles3"
    let fileExt = "txt"


    let appendLines (localFolder:string) (fileName:string) (data: string seq) =
        TextIO.appendLines "txt" (Some wsRootDir) localFolder fileName data

    let writeToFile (localFolder:string)  (fileName:string) (data: string) =
        TextIO.writeToFile "txt" (Some wsRootDir) localFolder fileName data

    let readAllText (localFolder:string)  (fileName:string) =
        TextIO.readAllText "txt" (Some wsRootDir) localFolder fileName

    let readAllLines (localFolder:string)  (fileName:string) =
        TextIO.readAllLines "txt" (Some wsRootDir) localFolder fileName


    let rngGen1 = RngGen.createLcg (12544 |> RandomSeed.create)
    let rngGen2 = RngGen.createLcg (72574 |> RandomSeed.create)
    let rngGen3 = RngGen.createLcg (82584 |> RandomSeed.create)

    let orders = [|14;16;|] |> Array.map(Order.createNr)

    let switchGenModes =
            [
                switchGenMode.StageSymmetric; 
                switchGenMode.Switch; 
                switchGenMode.Stage
            ]

    let scBase = 2

    let sorterCounts (order:order) = 
        match (order |> Order.value) with
        | 14 -> scBase * 2000 |> SorterCount.create
        | 16 -> scBase * 500  |> SorterCount.create
        | 18 -> scBase * 100  |> SorterCount.create
        | 20 -> scBase * 25   |> SorterCount.create
        | 22 -> scBase * 5    |> SorterCount.create
        | 24 -> scBase        |> SorterCount.create
        | _ -> failwith "not handled"


