namespace global

module SorterSetEval =
    
    let eval
        (sorterEvalMod:sorterEvalMode)
        (sortableSt:sortableSet)
        (sorterSt:sorterSet) 
        (useParallel:useParallel) =
        
        let _splitOutErrors (rs: Result<'a,'b>[]) =
            (
                rs |> Array.filter(Result.isOk)
                   |> Array.map(Result.ExtractOrThrow)
                   ,

                rs |> Array.filter(Result.isError)
                   |> Array.map(Result.ExtractErrorOrThrow)
            )

        if (useParallel |> UseParallel.value) then
            //sorterSt
            //|> SorterSet.getSorters
            //|> Seq.map(fun x -> async { return SorterEval.evalSorterWithSortableSet sorterEvalMod sortableSt x })
            //|> Async.Parallel
            //|> Async.RunSynchronously
            //|> _splitOutErrors

            sorterSt
            |> SorterSet.getSorters
            |> Seq.toArray
            |> Array.Parallel.map(SorterEval.evalSorterWithSortableSet sorterEvalMod sortableSt)
            |> _splitOutErrors



        else
            sorterSt
            |> SorterSet.getSorters
            |> Seq.toArray
            |> Array.map(SorterEval.evalSorterWithSortableSet sorterEvalMod sortableSt)
            |> _splitOutErrors
