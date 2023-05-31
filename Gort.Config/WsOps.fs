namespace global
open System


module WsOps = 

    //********  SortableSet ****************

    let saveSortableSet 
            (cfg:sortableSetCfg)
            (sst:sortableSet) 
        =
        let fileName = cfg |> SortableSetCfg.getFileName 
        WsFile.writeToFile wsFile.SortableSet fileName (sst |> SortableSetDto.toJson )


    let loadSortableSet (cfg:sortableSetCfg) =
          result {
            let! txtD = WsFile.readAllText  wsFile.SortableSet
                            (cfg |> SortableSetCfg.getFileName)
            return! txtD |> SortableSetDto.fromJson
          }


    let makeSortableSet (cfg:sortableSetCfg) =
        result {
            let! sortableSet = SortableSetCfg.makeSortableSet cfg
            let res = sortableSet 
                        |> saveSortableSet cfg
                        |> Result.map(ignore)
            return sortableSet
        }


    let getSortableSet (cfg:sortableSetCfg) =
        result {
            let loadRes = loadSortableSet cfg
            match loadRes with
            | Ok ss -> return ss
            | Error _ -> return! (makeSortableSet cfg)
        }


    //********  ParentMap  ****************
    
    let saveSorterSetParentMap
            (cfg:sorterSetParentMapCfg)
            (sst:sorterSetParentMap) 
        =
        WsFile.writeToFile wsFile.SorterSetMap
            (cfg |> SorterSetParentMapCfg.getFileName) 
            (sst |> SorterSetParentMapDto.toJson)



    let loadSorterSetParentMap
            (cfg:sorterSetParentMapCfg) 
          =
          result {
            let! txtD = 
                WsFile.readAllText  
                    wsFile.SorterSetMap
                    (cfg |> SorterSetParentMapCfg.getFileName)
            return! txtD |> SorterSetParentMapDto.fromJson
          }


    let makeSorterSetParentMap
            (cfg:sorterSetParentMapCfg) 
        =
        result {
            let parentMap = 
                    SorterSetParentMapCfg.makeParentMap
                        cfg

            let! resSs = parentMap |> saveSorterSetParentMap cfg
            return parentMap
        }


    let getParentMap
            (cfg:sorterSetParentMapCfg) 
        =
        result {
            let loadRes  = 
                result {
                    let! mut = loadSorterSetParentMap cfg
                    return mut
                }

            match loadRes with
            | Ok mut -> return mut
            | Error _ -> return! (makeSorterSetParentMap cfg)
        }


    //********  SorterSet ****************

    let saveSorterSet
            (fileName:string)
            (sst:sorterSet) 
        =
        WsFile.writeToFile wsFile.SorterSet
            fileName
            (sst |> SorterSetDto.toJson)


    let loadSorterSet (fileName:string) =
          result {
            let! txtD = 
                WsFile.readAllText  
                    wsFile.SorterSet
                    fileName
            return! txtD |> SorterSetDto.fromJson
          }


    let getSorterSet
            (cfg:sorterSetCfg)
        =
        SorterSetCfg.getSorterSet
                saveSorterSet
                loadSorterSet
                getParentMap
                cfg


    //********  SorterSet_Eval  ****************


    let saveSorterSetEval
            (fileName:string)
            (sst:sorterSetEval) 
        =
        WsFile.writeToFile 
            wsFile.SorterSetEval 
            fileName 
            (sst |> SorterSetEvalDto.toJson )


    let loadSorterSet_Eval (fileName:string) 
        =
        result {
            let! txtD = 
                WsFile.readAllText  
                    wsFile.SorterSetEval
                    fileName
            return! txtD |> SorterSetEvalDto.fromJson
        }


    let getSorterSetEval
            (up:useParallel)
            (cfg:sorterSet_EvalCfg) 
        =
        SorterSet_EvalCfg.getSorterSetEval
            getSortableSet
            getSorterSet
            saveSorterSetEval
            loadSorterSet_Eval
            up
            cfg


    //********  SorterSetRnd_EvalAllBitsCfg  ****************

    let makeSorterSetRnd_EvalAllBitsCfg
            (cfg:sorterSetRnd_EvalAllBitsCfg) 
        =
        result {
            let fileName = cfg |> SorterSetRnd_EvalAllBitsCfg.getFileName 
            let! ssEval = 
                    SorterSetRnd_EvalAllBitsCfg.makeSorterSetEval
                        WsCfgs.useParall
                        cfg
                        getSortableSet
                        getSorterSet

            let! resSs = ssEval |> saveSorterSetEval fileName
            return ssEval
        }


    let getSorterSetRnd_EvalAllBits
            (cfg:sorterSetRnd_EvalAllBitsCfg) 
        =
        result {
            let loadRes  = 
                result {
                    let fileName = cfg |> SorterSetRnd_EvalAllBitsCfg.getFileName
                    let! ssEval = fileName |> loadSorterSet_Eval
                    return ssEval
                }

            match loadRes with
            | Ok ssEval -> return ssEval
            | Error _ -> return! (makeSorterSetRnd_EvalAllBitsCfg cfg)
        }




    //********  ssmfr_EvalAllBitsCfg  ****************

    let makeSsmfr_EvalAllBitsCfg
            (cfg:ssmfr_EvalAllBitsCfg) 
        =
        result {
            let fileName = cfg |> Ssmfr_EvalAllBitsCfg.getFileName 
            let! ssEval = 
                    Ssmfr_EvalAllBitsCfg.makeSorterSetEval
                        WsCfgs.useParall
                        cfg
                        getSortableSet
                        getSorterSet

            let! resSs = ssEval |> saveSorterSetEval fileName
            return ssEval
        }


    let getSsmfr_EvalAllBits
            (cfg:ssmfr_EvalAllBitsCfg) 
        =
        result {
            let loadRes  = 
                result {
                    let fileName = cfg |> Ssmfr_EvalAllBitsCfg.getFileName
                    let! ssEval = fileName |> loadSorterSet_Eval
                    return ssEval
                }

            match loadRes with
            | Ok ssEval -> return ssEval
            | Error _ -> return! (makeSsmfr_EvalAllBitsCfg cfg)
        }



    //********  sorterSetRnd_EvalAllBits_ReportCfg  ****************

    let make_sorterSetRnd_Report 
            (cfg:sorterSetRnd_EvalAllBits_ReportCfg) 
        =
        let reportFileName = cfg |> SorterSetRnd_EvalAllBits_ReportCfg.getReportFileName

        WsFile.writeLinesIfNew
            wsFile.SorterEvalReport
            reportFileName 
            [SorterSetRnd_EvalAllBits_ReportCfg.getReportHeader()]
        |> Result.ExtractOrThrow |> ignore


        let repLines_set =
            SorterSetRnd_EvalAllBits_ReportCfg.makeSorterSetEvalReport
                    cfg
                    getSorterSetRnd_EvalAllBits

        repLines_set
            |> Seq.iter(
                fun res ->
                    match res with
                    | Ok repLines ->
                            WsFile.appendLines
                                    wsFile.SorterEvalReport
                                    reportFileName 
                                    repLines
                            |> Result.ExtractOrThrow |> ignore

                    | Error m -> Console.WriteLine(m)
                )


    //********  sorterSetRnd_EvalAllBits_ReportCfg  ****************

    let make_ssmr_Report 
            (cfg:ssmfr_EvalAllBits_ReportCfg) 
        =
        let reportFileName = cfg |> Ssmfr_EvalAllBits_ReportCfg.getReportFileName

        WsFile.writeLinesIfNew
            wsFile.SorterEvalReport
            reportFileName 
            [Ssmfr_EvalAllBits_ReportCfg.getReportHeader()]
        |> Result.ExtractOrThrow |> ignore


        let repLines_set =
            Ssmfr_EvalAllBits_ReportCfg.makeSorterSetEvalReport
                    cfg
                    getSsmfr_EvalAllBits

        repLines_set
            |> Seq.iter(
                fun res ->
                    match res with
                    | Ok repLines ->
                            WsFile.appendLines
                                    wsFile.SorterEvalReport
                                    reportFileName 
                                    repLines
                            |> Result.ExtractOrThrow |> ignore

                    | Error m -> Console.WriteLine(m)
                )

    //********  ssmfr_EvalAllBitsMerge_ReportCfg  ****************

    let make_ssmrMerge_Report 
            (cfg:ssmfr_EvalAllBitsMerge_ReportCfg) 
        =
        let reportFileName = cfg |> Ssmfr_EvalAllBitsMerge_ReportCfg.getReportFileName

        WsFile.writeLinesIfNew
            wsFile.SorterEvalMergeReport
            reportFileName 
            [Ssmfr_EvalAllBitsMerge_ReportCfg.getReportHeader()]
        |> Result.ExtractOrThrow |> ignore


        let repLines_set =
            Ssmfr_EvalAllBitsMerge_ReportCfg.makeSorterSetEvalReport
                    cfg
                    getSorterSetRnd_EvalAllBits
                    getSsmfr_EvalAllBits
                    getParentMap
 
        repLines_set
            |> Seq.iter(
                fun res ->
                    match res with
                    | Ok repLines ->
                            WsFile.appendLines
                                    wsFile.SorterEvalMergeReport
                                    reportFileName 
                                    repLines
                            |> Result.ExtractOrThrow |> ignore

                    | Error m -> Console.WriteLine(m)
                )




    let makeEm () =
        let res =
            //WsCfgs.allSortableSetCfgs ()
            //|> Array.map(getSortableSet)

            //WsCfgs.allSorterSetMutatedFromRndCfgs ()
            //|> Array.map(sorterSetCfg.RndMutated)
            //|> Array.map(getSorterSet)

            //WsCfgs.allSorterSetRndCfgs ()
            //|> Array.map(sorterSetCfg.Rnd)
            //|> Array.map(getSorterSet)

            
            //WsCfgs.allSorterSetRnd_EvalAllBitsCfg ()
            //|> Array.map(sorterSet_EvalCfg.Rnd)
            //|> Array.map(getSorterSetEval ( true |> UseParallel.create))

            WsCfgs.allSsmfr_EvalAllBitsCfg ()
            |> Array.map(sorterSet_EvalCfg.RndMutated)
            |> Array.map(getSorterSetEval ( true |> UseParallel.create))


            //WsCfgs.allSsmfr_EvalAllBitsCfg ()
            //|> Array.map(getSsmfr_EvalAllBits)


            //WsCfgs.allSorterSetEvalReportCfgs ()
            //|> Array.map(make_sorterSetRnd_Report)

            //WsCfgs.allssmfrEvalReportCfgs ()
            //|> Array.map(make_ssmr_Report)

            //WsCfgs.allssmfrEvalMergeReportCfgs ()
            //|> Array.map(make_ssmrMerge_Report)

        res