namespace global
open System


module WsStandardSorterSets = 

    let standardFolder = "StandardSorterSets"

    let writeData (fileName:string) (data: string seq) =
        TextIO.write "txt" (Some WsCommon.wsRootDir) standardFolder fileName data

    let readData (fileName:string) =
        TextIO.read "txt" (Some WsCommon.wsRootDir) standardFolder fileName


    let sorterCt1 (order:order) = 
        match (order |> Order.value) with
        | 16 -> 2 |> SorterCount.create
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
            (24 |> SwitchCount.create)
            //(SwitchCount.orderTo999SwitchCount order)
            (sorterCt1 order)


    let allCfgs () =
        [| for ordr in [16;18;] do
             for genMode in [switchGenMode.StageSymmetric; 
                             switchGenMode.Switch; 
                             switchGenMode.Stage] do
                 for usePfx in [true;false] do

                    makeRdnDenovoCfg genMode
                                     (ordr |> Order.createNr)
                                     WsCommon.rngGen1
                                     usePfx
        |]


    let saveStandardSorterSet
            (cfg:rndDenovoSorterSetCfg)
            (sst:sorterSet) 
        =
        let fileName = cfg |> RndDenovoSorterSetCfg.getFileName

        let jsns = sst |> SorterSet.getSorters
                       |> Seq.map(SorterDto.toJson)
        writeData       
            fileName
            jsns


    let getSorterSet 
            (cfg:rndDenovoSorterSetCfg) 
        =
        let sorterFileName = 
            sprintf "%s.txt"
                (cfg |> RndDenovoSorterSetCfg.getFileName)
        try
            let allLines = readData sorterFileName |> Result.ExtractOrThrow
            let sorters = allLines |> Array.map(SorterDto.fromJson >> Result.ExtractOrThrow)

            SorterSet.load
                (cfg |> RndDenovoSorterSetCfg.getSorterSetId)
                (cfg |> RndDenovoSorterSetCfg.getOrder)
                sorters
             |> Ok
        with ex ->
            ("error in TextIO.read: " + ex.Message) |> Error


    let runConfig (cfg) =
        let sorterSet = RndDenovoSorterSetCfg.getSorterSet cfg
        let res = sorterSet |> saveStandardSorterSet cfg
        ()


    let makeEm () =
        allCfgs ()
        |> Array.map(runConfig)
