namespace global
open System
//type sorterEvalsSelector = private { selector: (sorterEval array -> sorterEval array) }

//module SorterEvalsSelector = 
//    let getSelector (sorterEvalsSelectr:sorterEvalsSelector) = 
//        sorterEvalsSelectr.selector

//    let make (selectr: (sorterEval array -> sorterEval array)) = 
//        { selector = selectr }

type sorterSetEvalId = private SorterSetEvalId of Guid

module SorterSetEvalId =
    let value (SorterSetEvalId v) = v

    let create (id: Guid) =
        id |> SorterSetEvalId


type sorterSetEval =
    private
        {
            sorterSetEvalId:sorterSetEvalId
            sorterSetId:sorterSetId
            sortableSetId:sortableSetId
            sorterEvals:sorterEval[]
        }

module SorterSetEval 
       =
    let getSorterSetEvalId (ssEvl:sorterSetEval) =
            ssEvl.sorterSetEvalId

    let getSorterSetlId (ssEvl:sorterSetEval) =
            ssEvl.sorterSetId

    let getSortableSetId (ssEvl:sorterSetEval) =
            ssEvl.sortableSetId

    let getSorterEvals (ssEvl:sorterSetEval) =
            ssEvl.sorterEvals
            
    let evalSorters
            (sorterEvalMode: sorterEvalMode)
            (sortableSt: sortableSet)
            (sorters: sorter seq)
            (useParallel: useParallel) 
        : (sorterEval array) =

        let _splitOutErrors (rs: Result<'a, 'b>[]) =
            (rs |> Array.filter (Result.isOk) |> Array.map (Result.ExtractOrThrow),

             rs |> Array.filter (Result.isError) |> Array.map (Result.ExtractErrorOrThrow))

        if (useParallel |> UseParallel.value) then
            //sorters
            //|> Seq.map(fun x -> async { return SorterEval.evalSorterWithSortableSet sorterEvalMode sortableSt x })
            //|> Async.Parallel
            //|> Async.RunSynchronously
            //|> _splitOutErrors
            sorters
            |> Seq.toArray
            |> Array.Parallel.map (SorterEval.evalSorterWithSortableSet sorterEvalMode sortableSt)

        else
            sorters
            |> Seq.toArray
            |> Array.map (SorterEval.evalSorterWithSortableSet sorterEvalMode sortableSt)




    let create
            (sorterSetEvalId: sorterSetEvalId)
            (sorterSetId: sorterSetId)
            (sortableStId: sortableSetId)
            (sorterEvals: sorterEval[])
        =
          {
            sorterSetEvalId = sorterSetEvalId
            sorterSetId = sorterSetId
            sortableSetId = sortableStId
            sorterEvals = sorterEvals
          }


    let make
            (sorterSetEvalId: sorterSetEvalId)
            (sorterEvalMode: sorterEvalMode)
            (sorterSet: sorterSet)
            (sortableSt: sortableSet)
            (useParallel: useParallel) 
        =
        try
          let sorters = sorterSet |> SorterSet.getSorters
          let sorterEvals = evalSorters sorterEvalMode sortableSt sorters useParallel
          create
                sorterSetEvalId
                (sorterSet |> SorterSet.getId)
                (sortableSt |> SortableSet.getSortableSetId)
                sorterEvals
          |> Ok
        with ex ->
            ("error in SorterSetEval.make: " + ex.Message) 
            |> Error


    let getSorterIdsForUpgrade 
            (sorterEvalMod: sorterEvalMode)
            (sorterEvals : sorterEval array)
       : (sorterId array) =
        sorterEvals 
        |> Array.filter(SorterEval.shouldRetest sorterEvalMod)
        |> Array.map(SorterEval.getSorterId)



    //let twoPassEvaluation
    //    (sorterEvalMode1: sorterEvalMode)
    //    (sortableSt1: sortableSet)
    //    (sorterEvalsSelectr:sorterEvalsSelector)
    //    (sorterEvalMode2: sorterEvalMode)
    //    (sortableSt2: sortableSet)
    //    (sorters: sorter seq)
    //    (useParallel: useParallel) 
    //    : (sorterEval array) =
    //    let sorterMap = sorters 
    //                    |> Seq.map(fun s -> ( s |> Sorter.getSorterId, s))
    //                    |> Map.ofSeq

    //    let round1Evals = evalSorters sorterEvalMode1 sortableSt1 (sorterMap |> Map.values) useParallel
    //    let round1SelectedIds = round1Evals 
    //                            |> (sorterEvalsSelectr |> SorterEvalsSelector.getSelector)
    //                            |> getSorterIdsForUpgrade sorterEvalMode2

    //    let sortersRound2 = round1SelectedIds |> Array.map(fun srtrId -> sorterMap |> Map.find srtrId)
    //    let round2Evals = evalSorters sorterEvalMode2 sortableSt2 sortersRound2 useParallel
    //    round2Evals