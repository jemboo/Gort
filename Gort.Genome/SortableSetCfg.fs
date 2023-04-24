namespace global

open System

type sortableSetCfgCertain =
    | All_Bits of order
    | All_Bits_Reduced of order*array<switch>
    | Orbit of permutation


module SortableSetCfgCertain =

    let getOrder (sscc:sortableSetCfgCertain) = 
        match sscc with
        | All_Bits o -> o
        | All_Bits_Reduced (o, _) -> o
        | Orbit p -> p |> Permutation.getOrder


    let getId (sscc:sortableSetCfgCertain) = 
            [|sscc :> obj|] |> GuidUtils.guidFromObjs
            |> SortableSetId.create

    let switchReduceBits
        (ordr:order)
        (sortr:sorter)
        =
        result {
            let refinedSortableSetId = 
                (ordr, (sortr |> Sorter.getSwitches))
                        |> sortableSetCfgCertain.All_Bits_Reduced
                        |> getId
            let! baseSortableSet = 
                SortableSet.makeAllBits
                    (Guid.Empty |> SortableSetId.create)
                    rolloutFormat.RfU64
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

    let getSortableSet (sscc:sortableSetCfgCertain) = 
        match sscc with
        | All_Bits o ->
            SortableSet.makeAllBits
                (sscc |> getId)
                rolloutFormat.RfU64
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
                    (sscc |> getId)
                    None
                    perm


    let getStandardSwitchReducedOneStage (order:order) =
        let sws = TwoCycle.evenMode order 
                    |> Switch.fromTwoCycle
                    |> Seq.toArray
        sortableSetCfgCertain.All_Bits_Reduced (order, sws)