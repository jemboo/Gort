namespace global
open Microsoft.FSharp.Core
open Gort.DataStore.DataModel


module DbLookup = 

    let GetBitPackRById (gortCtxt:IGortContext2) (bitPackRId:int)  = 
        try
            DbQuery.TableById.GetBitPackRById(bitPackRId, gortCtxt) |> Ok
        with
            | ex -> ("error in GetBitPackRById: " + ex.Message ) |> Result.Error


    let GetCauseById (gortCtxt:IGortContext2) (causeId:int) = 
        try
            DbQuery.TableById.GetCauseById(causeId, gortCtxt) |> Ok
        with
            | ex -> ("error in GetCauseById: " + ex.Message ) |> Result.Error


    let GetRandGenRById (gortCtxt:IGortContext2) (rndGenId:int) = 
        try
            DbQuery.TableById.GetRandGenRById(rndGenId, gortCtxt) |> Ok
        with
            | ex -> ("error in GetRandGenRById: " + ex.Message ) |> Result.Error


    let GetSortableSetRById (gortCtxt:IGortContext2) (sortableSetRId:int) = 
        try
            DbQuery.TableById.GetSortableSetRById(sortableSetRId, gortCtxt) |> Ok
        with
            | ex -> ("error in GetSortableSetRById: " + ex.Message ) |> Result.Error


    let GetSorterRById (gortCtxt:IGortContext2) (sorterRId:int) = 
        try
            DbQuery.TableById.GetSortableSetRById(sorterRId, gortCtxt) |> Ok
        with
            | ex -> ("error in GetSorterRById: " + ex.Message ) |> Result.Error 


    let GetSorterSetRById (gortCtxt:IGortContext2) (sorterSetRId:int) = 
        try
            DbQuery.TableById.GetSortableSetRById(sorterSetRId, gortCtxt) |> Ok
        with
            | ex -> ("error in GetSorterSetRById: " + ex.Message ) |> Result.Error 

