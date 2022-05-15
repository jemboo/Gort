namespace global
open System
open System.Text
open Gort.Data.Utils
open Gort.Data.DataModel

module gcOps =

    let SaveChanges (ctxt:IGortContext) =
        try
            ctxt.SaveChanges() |> Ok
        with
            | ex -> ("error in MakeRndGenRecord: " + ex.Message ) |> Error 

    let MakeRndGenRecordAndTable 
                         (rgt:RndGenType) 
                         (seed:int) 
                         (causeId:Guid) 
                         (causePath:string) 
                         (ctxt:IGortContext) 
                         (saveChanges:bool) =
        try
            let mutable rr = new Gort.Data.DataModel.RandGen()
            rr.CauseId <- causeId;
            rr.CausePath <- causePath;
            rr.RndGenType <- rgt;
            rr.Seed <- seed;
            rr <- IdUtils.AddStructId rr
            ctxt.RandGen.Add rr |> ignore
            if saveChanges then
                ctxt.SaveChanges() |> ignore
            rr |> Ok
        with
            | ex -> ("error in MakeRndGenRecord: " + ex.Message ) |> Error 


    let MakeRndGenSetRecordsAndTable 
                         (rg:IRando) 
                         (rgCount:int)
                         (causeId:Guid)
                         (ctxt:IGortContext) =
        let getSeed (tup: string*IRando) =
            (snd tup).Seed |> RandomSeed.value
        try
            result {
                let! rgType = rg.rngType |> miscConv.RngTypeToRandGenType
                let rgs = { 0 .. (rgCount - 1) }
                            |> Seq.map(fun dex -> (dex.ToString(), Rando.nextRngGen rg))
                            |> Seq.toList
                let! res = rgs |> List.map(fun tup -> 
                    MakeRndGenRecordAndTable rgType (getSeed tup) causeId (fst tup) ctxt false)
                               |> Result.sequence
                return! SaveChanges ctxt
            }
        with
            | ex -> ("error in MakeRndGenRecord: " + ex.Message ) |> Result.Error 



    let GetRndGenRecord (rndGenId:Guid) (ctxt:IGortContext) =
        try
            Gort.Data.Utils.Query.GetRndGen(rndGenId, ctxt) |> Ok
        with
            | ex -> ("error in GetRndGenRecord: " + ex.Message ) |> Result.Error


    let MakeRndGenFromRecord (rndGenId:Guid) (ctxt:IGortContext) =
        result {
            let! r = GetRndGenRecord rndGenId ctxt
            let! rngt = r.RndGenType |> miscConv.RndGenTypeToRngType
            let seed = r.Seed |> RandomSeed.create
            return {rngType=rngt; seed=seed}
        }