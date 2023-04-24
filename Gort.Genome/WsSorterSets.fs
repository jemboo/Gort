namespace global
open System


module WsStandardSorterSets = 

    let sorterSetDir = "c:\\GortFiles" |> FileDir.create
    let standardFolder = "StandardSorterSets" |> FileFolder.create
    let fileExt = "txt" |> FileExt.create
    let archiver = FileUtils.makeArchiver sorterSetDir


    let rngGen1 = RngGen.createLcg (12544 |> RandomSeed.create)
    let rngGen2 = RngGen.createLcg (72574 |> RandomSeed.create)
    let rngGen3 = RngGen.createLcg (82584 |> RandomSeed.create)

    let sorterCt1 (order:order) = 
        match (order |> Order.value) with
        | 16 -> 50 |> SorterCount.create
        | 18 -> 50 |> SorterCount.create
        | 20 -> 50 |> SorterCount.create
        | 22 -> 50 |> SorterCount.create
        | 24 -> 50 |> SorterCount.create
        | _ -> failwith "not handled"


    let makeRdnDenovoCfg 
            (switchGenMode:switchGenMode)
            (order:order)
            (rngGen:rngGen)
            (usePfx:bool)
            =
        let pfx = 
            match usePfx with
            | true ->
                TwoCycle.evenMode order
                |> Switch.fromTwoCycle
                |> Seq.toArray
            | false ->
                [||]
                    
        RndDenovoSorterSetCfg.create 
            order rngGen switchGenMode pfx
            (SwitchCount.orderTo999SwitchCount order)
            (sorterCt1 order)


    let getRdnDenovoId
            (switchGenMode:switchGenMode)
            (order:order)
            (rngGen:rngGen)
            (usePfx:bool)
            =
        makeRdnDenovoCfg 
            switchGenMode
            order
            rngGen
            usePfx
        |> RndDenovoSorterSetCfg.makeSorterSetId


    let fileNameFromSorterSet (sst:sorterSet) =
        sst |> SorterSet.getId |> SorterSetId.value 
            |> string |> FileName.create


    let fileNameFromCfg
            (switchGenMode:switchGenMode)
            (order:order)
            (rngGen:rngGen)
            (usePfx:bool)
            =
        getRdnDenovoId 
                switchGenMode
                order
                rngGen
                usePfx
        |> string |> FileName.create


    let allCfgs () =
        [| for ordr in [16;18;20;22;24] do
             for genMode in [switchGenMode.StageSymmetric; 
                             switchGenMode.Switch; 
                             switchGenMode.Stage] do
                 for usePfx in [true;false] do

                    makeRdnDenovoCfg genMode
                                     (ordr |> Order.createNr)
                                     rngGen1
                                     usePfx
        |]


    let saveStandardSorterSet (sst:sorterSet) =
        let fileName = sst |> fileNameFromSorterSet
        let jsns = sst |> SorterSet.getSorters
                       |> Seq.map(SorterDto.toJson)
        archiver standardFolder fileName fileExt jsns


    let runConfig (cfg) =
        let sorterSet = RndDenovoSorterSetCfg.makeSorterSet cfg
        let res = sorterSet |> saveStandardSorterSet 
        ()


    let makeEm () =
        allCfgs ()
        |> Array.map(runConfig)
