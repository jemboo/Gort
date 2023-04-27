namespace global
open System


module WsCommon = 
    let wsRootDir = "c:\\GortFiles"
    let fileExt = "txt"

    let rngGen1 = RngGen.createLcg (12544 |> RandomSeed.create)
    let rngGen2 = RngGen.createLcg (72574 |> RandomSeed.create)
    let rngGen3 = RngGen.createLcg (82584 |> RandomSeed.create)



