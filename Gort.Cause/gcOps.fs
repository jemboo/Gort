namespace global
open System
open System.Text
open Gort.Data.Utils
open Gort.Data.DataModel

module gcOps =

    let MakeRndGenRecordAndTable 
                         (rgt:RndGenType) 
                         (seed:int) 
                         (causeId:Guid) 
                         (causePath:string) 
                         (ctxt:IGortContext) =
        try
            let mutable rr = new Gort.Data.DataModel.RandGen()
            rr.CauseId <- causeId;
            rr.CausePath <- causePath;
            rr.RndGenType <- rgt;
            rr.Seed <- seed;
            rr <- IdUtils.AddStructId rr
            ctxt.RandGen.Add rr |> ignore
            ctxt.SaveChanges() |> ignore
            rr |> Ok
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