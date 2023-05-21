namespace global

open System

type sortableSetCertainCfg =
    | All_Bits of order
    | All_Bits_Reduced of order*array<switch>
    | Orbit of permutation


module SortableSetCertainCfg =

    let getOrder 
            (sscc:sortableSetCertainCfg) 
        = 
        match sscc with
        | All_Bits o -> o
        | All_Bits_Reduced (o, _) -> o
        | Orbit p -> p |> Permutation.getOrder


    let getSortableSetId 
                (cfg:sortableSetCertainCfg) 
        = 
        [| (cfg.GetType()) :> obj;
            cfg :> obj|] |> GuidUtils.guidFromObjs
        |> SortableSetId.create
    

    let getConfigName 
            (sscc:sortableSetCertainCfg) 
        =
        match sscc with
            | All_Bits o -> 
                sprintf "%s_%d"
                    "All"
                    (sscc |> getOrder |> Order.value)
            | All_Bits_Reduced (o, a) ->
                sprintf "%s_%d_%d"
                    "Reduced"
                    (sscc |> getOrder |> Order.value)
                    (a.Length)
            | Orbit perm ->
                sprintf "%s_%d_%d"
                    "Orbit"
                    (sscc |> getOrder |> Order.value)
                    (perm |> Permutation.powers None |> Seq.length)


    let getFileName
            (sscc:sortableSetCertainCfg) 
        =
        sscc |> getSortableSetId |> SortableSetId.value |> string


    let switchReduceBits
            (ordr:order)
            (sortr:sorter)
        =
        result {
            let refinedSortableSetId = 
                (ordr, (sortr |> Sorter.getSwitches))
                        |> sortableSetCertainCfg.All_Bits_Reduced
                        |> getSortableSetId
            let! baseSortableSet = 
                SortableSet.makeAllBits
                    (Guid.Empty |> SortableSetId.create)
                    rolloutFormat.RfBs64
                    ordr

            let! sorterOpOutput = 
                SortingRollout.makeSorterOpOutput
                    sorterOpTrackMode.SwitchUses
                    baseSortableSet
                    sortr

            let! refined = sorterOpOutput
                            |> SorterOpOutput.getRefinedSortableSet
                                                 refinedSortableSetId
            return refined
        }


    let makeSortableSet 
            (sscc:sortableSetCertainCfg) 
        = 
        match sscc with
        | All_Bits o ->
            SortableSet.makeAllBits
                (sscc |> getSortableSetId)
                rolloutFormat.RfBs64
                o

        | All_Bits_Reduced (o, sa) -> 
            result {
                let sorterId = Guid.Empty |> SorterId.create
                let sorter = Sorter.fromSwitches 
                                sorterId 
                                o
                                sa
                let! refinedSortableSet =
                        switchReduceBits o sorter

                return refinedSortableSet
            }

        | Orbit perm -> 
                SortableSet.makeOrbits
                    (sscc |> getSortableSetId)
                    None
                    perm


    let makeAllBitsReducedOneStage 
            (order:order) 
        =
        let sws = TwoCycle.evenMode order 
                    |> Switch.fromTwoCycle
                    |> Seq.toArray
        sortableSetCertainCfg.All_Bits_Reduced (order, sws)



type sortableSetCfg = 
     | Certain of sortableSetCertainCfg


module SortableSetCfg =

    let getSortableSetId 
            (ssCfg: sortableSetCfg) 
        = 
        match ssCfg with
        | Certain cCfg -> 
            cCfg |> SortableSetCertainCfg.getSortableSetId


    let makeSortableSet
            (ssCfg: sortableSetCfg) 
        = 
        match ssCfg with
        | Certain cCfg -> 
            cCfg |> SortableSetCertainCfg.makeSortableSet


    let getOrder
            (ssCfg: sortableSetCfg) 
        = 
        match ssCfg with
        | Certain cCfg -> 
            cCfg |> SortableSetCertainCfg.getOrder


    let getCfgName
            (ssCfg: sortableSetCfg) 
        =
        match ssCfg with
        | Certain cCfg -> 
            cCfg |> SortableSetCertainCfg.getConfigName


    let getFileName
            (ssCfg: sortableSetCfg) 
        =
        match ssCfg with
        | Certain cCfg -> 
            cCfg |> SortableSetCertainCfg.getFileName