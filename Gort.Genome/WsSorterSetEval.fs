namespace global
open System


module WsSorterSetEval 
        =

    let localFolder = "Eval_Standard_On_Binary"

    let appendLines (fileName:string) (data: string seq) =
        WsCommon.appendLines localFolder fileName data

    let writeToFile (fileName:string) (data: string) =
        WsCommon.writeToFile localFolder fileName data

    let readAllText (fileName:string) =
        WsCommon.readAllText localFolder fileName

    let readAllLines (fileName:string) =
        WsCommon.readAllLines localFolder fileName


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



    let makeSorterSetEval 
            (cfg:sorterSetEvalCfg) 
        =
        let sorterSetEval = 
            cfg |> SorterSetEvalCfg.getSorterSetEval
                        getSortableSet
                        getSorterSet
                        WsCommon.useParall

        let jason = sorterSetEval |> SorterSetEvalDto.toJson
        writeToFile ( cfg |> SorterSetEvalCfg.getFileName ) jason
        |> ignore
        sorterSetEval


    let allCfgs () =
        [| 
           for ordr in WsCommon.orders do
              for genMode in WsCommon.switchGenModes do
                 let sortableSetCfg =
                      SortableSetCfgCertain.getStandardSwitchReducedOneStage 
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
                    (1 |> StageCount.create)
                    sorterEvalMode.DontCheckSuccess
        |]


    let makeEm () =
        allCfgs ()
        |> Array.map(makeSorterSetEval)