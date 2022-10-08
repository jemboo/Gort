namespace global

open System
open Gort.Data.Instance
open Gort.Data.Instance.CauseBuilder
open Gort.Data.Instance.SeedParams
open Gort.Data.Instance.CauseBuilder.SortableSet
open Gort.Data.Instance.CauseBuilder.Utils

module RunUtils =

    let RunCbRandGen (ctxt: Gort.Data.DataModel.IGortContext) (wsName: string) =
        let seedParams = new SeedParamsA()

        result {
            let! resSp = Utils.LoadStatics ctxt
            let! resSt = Utils.LoadSeedParams seedParams ctxt
            let mutable curCauseDex = 1
            let descr = sprintf "RandGen_%d" curCauseDex

            let cbRnd =
                new CbRand(wsName, curCauseDex, descr, seedParams.RngSeed, seedParams.RngType)

            let! resCbRnd = Utils.LoadCauseBuilder cbRnd ctxt
            let! dexB = CauseOps.RunNextCause ctxt wsName
            return dexB
        }


    let RunCbRandGenSet (ctxt: Gort.Data.DataModel.IGortContext) (wsName: string) =
        let seedParams = new SeedParamsA()

        result {
            let! resSp = Utils.LoadStatics ctxt
            let! resSt = Utils.LoadSeedParams seedParams ctxt
            let mutable curCauseDex = 1
            let descr = sprintf "RndGen_%d" curCauseDex

            let cbRnd =
                new CbRand(wsName, curCauseDex, descr, seedParams.RngSeed, seedParams.RngType)

            let! resCbRnd = Utils.LoadCauseBuilder cbRnd ctxt
            let! dexB1 = CauseOps.RunNextCause ctxt wsName
            let! pRamRngId = Utils.GetParamRngId ctxt cbRnd
            curCauseDex <- curCauseDex + 1
            let descr2 = sprintf "RandGenSet_%d" curCauseDex

            let cbRndSet =
                new CbRandSet(wsName, curCauseDex, descr2, pRamRngId, seedParams.RngCount)

            let! resCbRnd = Utils.LoadCauseBuilder cbRndSet ctxt
            let! dexB2 = CauseOps.RunNextCause ctxt wsName
            return pRamRngId.ParamId
        }
