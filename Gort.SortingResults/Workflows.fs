open System
open System.IO
open System.Diagnostics

let gortDir = "C:\GortFiles"

let makeGortStream () =
       let fp = sprintf "%s\%s" gortDir (Guid.NewGuid().ToString())
       new StreamWriter(fp, false)

let grtStream = makeGortStream ()

let evo () =
    let sorterSetId = Guid.NewGuid() |> SorterSetId.create
    let useParalll = true |> UseParallel.create
    let ordr = 16 |> Order.createNr
    let switchCt = SwitchCount.orderTo900SwitchCount ordr
    let sorterCt = SorterCount.create 10
    let randy = Rando.create rngType.Lcg (123 |> RandomSeed.create)
    let rndGn () = 
        randy |> Rando.nextRngGen
    let rolloutFormt = rolloutFormat.RfBs64
    let sortableStId = SortableSetId.create (Guid.NewGuid())
    let stageWgt = 0.2 |> StageWeight.create
    let ranker ss = ss |> SorterFitness.fromSpeed stageWgt
    let mutationRate = MutationRate.create 0.01
    let sorterMutator = SorterUniformMutator.create 
                                None None switchGenMode.Stage mutationRate

    let sortableSt =
        SortableSet.makeAllBits sortableStId rolloutFormt ordr |> Result.ExtractOrThrow

    let mutable sorterSt = SorterSet.createRandomSwitches sorterSetId sorterCt ordr [||] switchCt rndGn

    let _repo (gen:int) (sorterSpeedBins:sorterSpeedBin array) = 
        let phenoBins = sorterSpeedBins 
                        |> SorterSpeedBin.toSorterPhenotypeBins
                        |> Seq.toArray

        let mutable dex = 0
        while dex < phenoBins.Length do
            grtStream.WriteLine (phenoBins.[dex] |> SorterPhenotypeBin.report (gen.ToString()))
            dex <- dex + 1


    let _doGen (gen:int) (sst:sorterSet) =
        let sorterEvls =
            SorterSetEval.evalSorters sorterEvalMode.DontCheckSuccess 
                sortableSt (sst |> SorterSet.getSorters) useParalll

        let newSorterSetId = Guid.NewGuid() |> SorterSetId.create

        let sorterSpeedBins = sorterEvls 
                                    |> SorterSpeedBin.fromSorterEvals
                                    |> Seq.toArray
        if true then //gen % 2 = 0 then
            _repo gen sorterSpeedBins

        let ranky = sorterSpeedBins 
                        |> SorterSpeedBin.orderSorterPhenotypesBySliceAndSpeed ranker
                        |> Seq.toArray
        let bestSorterIds = ranky |> Seq.take(9) |> Seq.map(fst) |> Seq.toArray

        let winningSorters = sst 
                                |> SorterSet.getSortersById (9 |> SorterCount.create) bestSorterIds
                                |> Seq.toArray
        SorterSet.createMutationSet 
                    winningSorters sorterCt ordr sorterMutator newSorterSetId randy
    
    let mutable gen = 0
    while gen < 51 do
        sorterSt <- _doGen gen sorterSt
        gen <- gen + 1

    
    grtStream.Flush()
    grtStream.Close()
    1




let res = evo() 


printfn "Hello from F#"
