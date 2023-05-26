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


    //********  SorterSet ****************

    let saveSorterSet
            (cfg:sorterSetRndCfg)
            (sst:sorterSet) 
        =
        WsFile.writeToFile wsFile.SorterSet
            (cfg |> SorterSetRndCfg.getFileName) 
            (sst |> SorterSetDto.toJson)


    let loadSorterSet (cfg:sorterSetRndCfg) =
          result {
            let! txtD = WsFile.readAllText  wsFile.SorterSet
                            (cfg |> SorterSetRndCfg.getFileName)
            return! txtD |> SorterSetDto.fromJson
          }


    let makeSorterSet (cfg) =
        result {
            let sorterSet = SorterSetRndCfg.makeSorterSet cfg
            let! res = sorterSet |> saveSorterSet cfg
            return sorterSet
        }


    let getSorterSetRnd (cfg:sorterSetRndCfg) =
        result {
            let loadRes = loadSorterSet cfg
            match loadRes with
            | Ok ss -> return ss
            | Error _ -> return! (makeSorterSet cfg)
        }



    //********  SorterSetMutate  ****************

    let saveMutatedSorterSet
            (cfg:sorterSetMutatedFromRndCfg)
            (sst:sorterSet) 
        =
        WsFile.writeToFile wsFile.SorterSet
            (cfg |> SorterSetMutatedFromRndCfg.getFileName) 
            (sst |> SorterSetDto.toJson)


    let loadMutatedSorterSet 
            (cfg:sorterSetMutatedFromRndCfg) =
          result {
            let! txtD = 
                WsFile.readAllText  
                    wsFile.SorterSet
                    (cfg |> SorterSetMutatedFromRndCfg.getFileName)
            return! txtD |> SorterSetDto.fromJson
          }


    let makeMutantSorterSet
            (cfg:sorterSetMutatedFromRndCfg) 
        =
        result {
            let! mutantSet = 
                    SorterSetMutatedFromRndCfg.makeMutantSorterSet
                        getSorterSetRnd
                        cfg

            let! resSs = mutantSet |> saveMutatedSorterSet cfg
            return mutantSet
        }
        

    let getMutantSorterSet
            (cfg:sorterSetMutatedFromRndCfg) 
        =
        result {
            let loadRes  = 
                result {
                    let! mut = loadMutatedSorterSet cfg
                    return mut
                }

            match loadRes with
            | Ok mut -> return mut
            | Error _ -> return! (makeMutantSorterSet cfg)
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

        
    

    //********  SorterSetMutate and ParentMap  ****************

    let getMutantSorterSetAndParentMap
            (cfg:sorterSetMutatedFromRndCfg) 
        =
        result {
            let! mutantSet = getMutantSorterSet cfg
            let! parentMap = getParentMap 
                                (cfg |> SorterSetMutatedFromRndCfg.getSorterSetParentMapCfg)

            return mutantSet, parentMap
        }


    //********  SorterSetRnd_EvalAllBitsCfg  ****************

    let saveSorterSetEval
            (cfg:sorterSetRnd_EvalAllBitsCfg)
            (sst:sorterSetEval) 
        =
        let fileName = cfg |> SorterSetRnd_EvalAllBitsCfg.getFileName 
        WsFile.writeToFile 
            wsFile.SorterSetEval 
            fileName 
            (sst |> SorterSetEvalDto.toJson )


    let loadSorterSetRnd_EvalAllBitsCfg
            (cfg:sorterSetRnd_EvalAllBitsCfg) 
          =
          result {
            let! txtD = 
                WsFile.readAllText  
                    wsFile.SorterSetEval
                    (cfg |> SorterSetRnd_EvalAllBitsCfg.getFileName)
            return! txtD |> SorterSetEvalDto.fromJson
          }



    let makeSorterSetRnd_EvalAllBitsCfg
            (cfg:sorterSetRnd_EvalAllBitsCfg) 
        =
        result {
            let! ssEval = 
                    SorterSetRnd_EvalAllBitsCfg.makeSorterSetEval
                        WsCfgs.useParall
                        cfg
                        getSortableSet
                        getSorterSetRnd

            let! resSs = ssEval |> saveSorterSetEval cfg
            return ssEval
        }


    let getSorterSetRnd_EvalAllBits
            (cfg:sorterSetRnd_EvalAllBitsCfg) 
        =
        result {
            let loadRes  = 
                result {
                    let! ssEval = loadSorterSetRnd_EvalAllBitsCfg cfg
                    return ssEval
                }

            match loadRes with
            | Ok ssEval -> return ssEval
            | Error _ -> return! (makeSorterSetRnd_EvalAllBitsCfg cfg)
        }




    //********  ssmfr_EvalAllBitsCfg  ****************

    let save_ssmfr_Eval
            (cfg:ssmfr_EvalAllBitsCfg)
            (sst:sorterSetEval) 
        =
        let fileName = cfg |> Ssmfr_EvalAllBitsCfg.getFileName 
        WsFile.writeToFile 
            wsFile.SorterSetEval 
            fileName 
            (sst |> SorterSetEvalDto.toJson )


    let loadSsmfr_EvalAllBitsCfg
            (cfg:ssmfr_EvalAllBitsCfg) 
          =
          result {
            let! txtD = 
                WsFile.readAllText  
                    wsFile.SorterSetEval
                    (cfg |> Ssmfr_EvalAllBitsCfg.getFileName)
            return! txtD |> SorterSetEvalDto.fromJson
          }



    let makeSsmfr_EvalAllBitsCfg
            (cfg:ssmfr_EvalAllBitsCfg) 
        =
        result {
            let! ssEval = 
                    Ssmfr_EvalAllBitsCfg.makeSorterSetEval
                        WsCfgs.useParall
                        cfg
                        getSortableSet
                        getMutantSorterSet

            let! resSs = ssEval |> save_ssmfr_Eval cfg
            return ssEval
        }


    let getSsmfr_EvalAllBits
            (cfg:ssmfr_EvalAllBitsCfg) 
        =
        result {
            let loadRes  = 
                result {
                    let! ssEval = loadSsmfr_EvalAllBitsCfg cfg
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

    //********  sorterSetRnd_EvalAllBitsMerge_ReportCfg  ****************

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

        //WsCfgs.allSortableSetCfgs ()
        //|> Array.map(getSortableSet)

        //WsCfgs.allSorterSetMutatedFromRndCfgs ()
        //|> Array.map(getMutantSorterSetAndParentMap)
        //WsCfgs.allSorterSetRndCfgs ()
        //|> Array.map(getSorterSetRnd)


        //WsCfgs.allSsmfr_EvalAllBitsCfg ()
        //|> Array.map(getSsmfr_EvalAllBits)

        //WsCfgs.allSsmfr_EvalAllBitsCfg ()
        //|> Array.map(getSsmfr_EvalAllBits)

        //WsCfgs.allSorterSetEvalReportCfgs ()
        //|> Array.map(make_sorterSetRnd_Report)

        //WsCfgs.allssmfrEvalReportCfgs ()
        //|> Array.map(make_ssmr_Report)

        WsCfgs.allssmfrEvalMergeReportCfgs ()
        |> Array.map(make_ssmrMerge_Report)