namespace global
open System


module WsBinarySortableSets = 

    let sorterSetDir = "c:\\GortFiles" |> FileDir.create
    let standardFolder = "BinarySortableSets" |> FileFolder.create
    let fileExt = "txt" |> FileExt.create
    let archiver = FileUtils.makeArchiver sorterSetDir
       // let res = archiver folder file ext testData

    let orders = [|16;18;20;22;24|] |> Array.map(Order.createNr)

    let allCfgs () =
        [| 
          for ordr in orders do
            SortableSetCfgCertain.getStandardSwitchReducedOneStage ordr

          for ordr in orders do
            sortableSetCfgCertain.All_Bits ordr
        |]


    let getSortableSetId
            (ordr:order)
            (usePfx:bool)
            =
        if usePfx then
            SortableSetCfgCertain.getStandardSwitchReducedOneStage ordr
            |> SortableSetCfgCertain.getId
        else
            sortableSetCfgCertain.All_Bits ordr
            |> SortableSetCfgCertain.getId


    let fileNameFromSortableSet (sst:sortableSet) =
        sst |> SortableSet.getSortableSetId |> SortableSetId.value 
            |> string |> FileName.create


    let saveSortableSet (sst:sortableSet) =
        let fileName = sst |> fileNameFromSortableSet
        let jsns = []
        //let jsns = sst |> SortableSet.getRollout
        //               |> Seq.map(RolloutDto)
        archiver standardFolder fileName fileExt jsns


    let runConfig (cfg) =
        let sortableSet = SortableSetCfgCertain.getSortableSet cfg
                            |> Result.ExtractOrThrow
        let res = sortableSet |> saveSortableSet 
        ()



    let makeEm () =
        allCfgs ()
        |> Array.map(runConfig)
