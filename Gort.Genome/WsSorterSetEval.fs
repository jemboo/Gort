namespace global
open System


module WsSorterSetEval 
        =
    let useParall = true |> UseParallel.create

    let standardEvalOnBinaryFolder = "Eval_Standard_On_Binary"

    let writeData (fileName:string) (data: string) =
        TextIO.writeToFile "txt" (Some WsCommon.wsRootDir) standardEvalOnBinaryFolder fileName data

    let readAllText (fileName:string) =
        TextIO.readAllText "txt" (Some WsCommon.wsRootDir) standardEvalOnBinaryFolder fileName

    let readAllLines (fileName:string) =
        TextIO.readAllLines "txt" (Some WsCommon.wsRootDir) standardEvalOnBinaryFolder fileName


    let getSortableSet cfg 
        =
        WsBinarySortableSets.getSortableSet cfg
        |> Result.ExtractOrThrow


    let getSorterSet cfg
        =
        WsSorterSets.getSorterSet cfg
        |> Result.ExtractOrThrow


    let getSorterSetEval 
            (cfg:sorterSetEvalCfg) 
        =
        let sorterSetEvalFileName = 
                (cfg |> SorterSetEvalCfg.getFileName)
        try
            result {
               let! txtD = readAllText sorterSetEvalFileName
               return! txtD |> SorterSetEvalDto.fromJson
            }
            with ex ->
                ("error in WsSorterSetEval.getSorterSetEval: " + ex.Message) |> Error



    let runConfig 
            (cfg:sorterSetEvalCfg) 
        =
        let sorterSetEval = 
                (cfg |> SorterSetEvalCfg.getSorterSetEval
                        getSortableSet
                        getSorterSet)
                     useParall

        let jason = sorterSetEval |> SorterSetEvalDto.toJson
        writeData (cfg |> SorterSetEvalCfg.getFileName ) jason


    let orders = [|16; 18|] |> Array.map(Order.createNr)
    let genModes = [switchGenMode.StageSymmetric; 
                    switchGenMode.Switch; 
                    switchGenMode.Stage]

    let allCfgs () =
        [| 
           for ordr in orders do
              for genMode in genModes do
                 let sortableSetCfg =
                      sortableSetCfgCertain.All_Bits 
                            ordr
                      |> sortableSetCfg.Certain
                 let sorterSetCfg = 
                        WsSorterSets.makeRdnDenovoCfg
                            genMode
                            ordr
                            WsCommon.rngGen1
                            true
                      |> sorterSetCfg.RndDenovo

                 SorterSetEvalCfg.create
                    sortableSetCfg
                    sorterSetCfg
                    sorterEvalMode.DontCheckSuccess
        |]


    let makeEm () =
        allCfgs ()
        |> Array.map(runConfig)