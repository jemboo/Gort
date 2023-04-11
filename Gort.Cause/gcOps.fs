namespace global

open System
open Gort.Data.Utils
open Gort.Data.DataModel

module gcOps =

    let MakeRandGenRecordAndTable
        (randGenType: RandGenType)
        (seed: int)
        (causeId: Guid)
        (causePath: string)
        (ctxt: IGortContext)
        =
        try
            let mutable rr = new RandGen()
            rr.CauseId <- causeId
            rr.CausePath <- causePath
            rr.RandGenType <- randGenType
            rr.Seed <- seed
            rr <- IdUtils.AddStructId rr
            ctxt.RandGen.Add rr |> ignore
            rr |> Ok
        with ex ->
            ("error in MakeRndGenRecord: " + ex.Message) |> Error


    let MakeRandGenSetRecordsAndTable 
            (randy: IRando) 
            (rgCount: int) 
            (causeId: Guid) 
            (ctxt: IGortContext) 
            =
        let getSeed (tup: string * IRando) = (snd tup).Seed |> RandomSeed.value

        try
            result {
                let! randGenType = randy.rngType |> miscConv.RngTypeToRandGenType

                let rgs =
                    { 0 .. (rgCount - 1) }
                    |> Seq.map (fun dex -> (dex.ToString(), Rando.nextRando randy))
                    |> Seq.toList

                let! res =
                    rgs
                    |> List.map (fun tup -> MakeRandGenRecordAndTable randGenType (getSeed tup) causeId (fst tup) ctxt)
                    |> Result.sequence

                return res.Length
            }
        with ex ->
            ("error in MakeRndGenRecord: " + ex.Message) |> Result.Error


    let MakeRandGenFromRecord (rndGenId: Guid) (ctxt: IGortContext) =
        try
            DomainQuery.GetRandGen(rndGenId, ctxt) |> Ok
        with ex ->
            ("error in GetRandGenRecord: " + ex.Message) |> Result.Error


    let MakeRngGenFromRecord (randGenId: Guid) (ctxt: IGortContext) =
        result {
            let! r = MakeRandGenFromRecord randGenId ctxt
            let! rngt = r.RandGenType |> miscConv.RandGenTypeToRngType
            let seed = r.Seed |> RandomSeed.create
            return RngGen.create rngt seed
        }
