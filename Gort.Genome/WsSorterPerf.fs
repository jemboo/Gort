namespace global
open System


module WsSorterPerf = 

    let standardEvalOnBinaryFolder = 
        "Eval_Standard_On_Binary" |> FileFolder.create





    let runConfig (cfg) =
        ()



    let allCfgs () =
        [| 

        |]


    let makeEm () =
        allCfgs ()
        |> Array.map(runConfig)