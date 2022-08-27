namespace global
open System
//open SortingEval

module SortingArrayRoll =
    // uses a (sorter.switchcount * sortableCount ) length 
    // array to store each switch use, thus no SAG (Switch 
    // Action Grouping)
    let private switchRangeWithNoSAGintRoll
                (sorter:sorter)
                (intRoll:intRoll) 
                (rollingUseCounts:bool[])
                (sortableCount:sortableCount) =

        let rollArray = intRoll |> IntRoll.getData
        let orderV = sorter.order |> Order.value

        let mutable sortableIndex = 0
        while (sortableIndex < (SortableCount.value sortableCount)) do

            let mutable looP = true
            let mutable localSwitchOffset = 0
            let sortableSetRolloutOffset = sortableIndex * orderV
            let switchEventRolloutOffset = sortableIndex * (SwitchCount.value sorter.switchCount)

            while ((localSwitchOffset < (sorter.switchCount |> SwitchCount.value)) && looP) do
                let switch = sorter.switches.[localSwitchOffset]
                let lv = rollArray.[switch.low + sortableSetRolloutOffset]
                let hv = rollArray.[switch.hi + sortableSetRolloutOffset]
                if(lv > hv) then
                    rollArray.[switch.hi + sortableSetRolloutOffset] <- lv
                    rollArray.[switch.low + sortableSetRolloutOffset] <- hv
                    rollingUseCounts.[localSwitchOffset + switchEventRolloutOffset] <- true
                looP <- ((localSwitchOffset % 20 > 0) ||
                         (not (CollectionProps.isSortedOffset
                                                rollArray
                                                sortableSetRolloutOffset 
                                                orderV)))
                localSwitchOffset <- localSwitchOffset + 1

            sortableIndex <- sortableIndex + 1



    // uses a (sorter.switchcount * sortableCount ) length 
    // array to store each switch use, thus no SAG (Switch 
    // Action Grouping)
    let private switchRangeWithNoSAGuInt8Roll
                (sorter:sorter)
                (uInt8Roll:uInt8Roll) 
                (rollingUseCounts:bool[])
                (sortableCount:sortableCount) =

        let rollArray = uInt8Roll |> Uint8Roll.getData
        let orderV = sorter.order |> Order.value

        let mutable sortableIndex = 0
        while (sortableIndex < (SortableCount.value sortableCount)) do

            let mutable looP = true
            let mutable localSwitchOffset = 0
            let sortableSetRolloutOffset = sortableIndex * orderV
            let switchEventRolloutOffset = sortableIndex * (SwitchCount.value sorter.switchCount)

            while ((localSwitchOffset < (sorter.switchCount |> SwitchCount.value)) && looP) do
                let switch = sorter.switches.[localSwitchOffset]
                let lv = rollArray.[switch.low + sortableSetRolloutOffset]
                let hv = rollArray.[switch.hi + sortableSetRolloutOffset]
                if(lv > hv) then
                    rollArray.[switch.hi + sortableSetRolloutOffset] <- lv
                    rollArray.[switch.low + sortableSetRolloutOffset] <- hv
                    rollingUseCounts.[localSwitchOffset + switchEventRolloutOffset] <- true
                looP <- ((localSwitchOffset % 20 > 0) ||
                         (not (CollectionProps.isSortedOffset
                                                rollArray
                                                sortableSetRolloutOffset 
                                                orderV)))
                localSwitchOffset <- localSwitchOffset + 1

        sortableIndex <- sortableIndex + 1



    // uses a (sorter.switchcount * sortableCount ) length 
    // array to store each switch use, thus no SAG (Switch 
    // Action Grouping)
    let private switchRangeWithNoSAGuInt16Roll
                (sorter:sorter)
                (uInt8Roll:uInt16Roll) 
                (rollingUseCounts:bool[])
                (sortableCount:sortableCount) =
        let rollArray = uInt8Roll |> Uint16Roll.getData
        let orderV = sorter.order |> Order.value

        let mutable sortableIndex = 0
        while (sortableIndex < (SortableCount.value sortableCount)) do

            let mutable looP = true
            let mutable localSwitchOffset = 0
            let sortableSetRolloutOffset = sortableIndex * orderV
            let switchEventRolloutOffset = sortableIndex * (SwitchCount.value sorter.switchCount)

            while ((localSwitchOffset < (sorter.switchCount |> SwitchCount.value)) && looP) do
                let switch = sorter.switches.[localSwitchOffset]
                let lv = rollArray.[switch.low + sortableSetRolloutOffset]
                let hv = rollArray.[switch.hi + sortableSetRolloutOffset]
                if(lv > hv) then
                    rollArray.[switch.hi + sortableSetRolloutOffset] <- lv
                    rollArray.[switch.low + sortableSetRolloutOffset] <- hv
                    rollingUseCounts.[localSwitchOffset + switchEventRolloutOffset] <- true
                looP <- ((localSwitchOffset % 20 > 0) ||
                         (not (CollectionProps.isSortedOffset
                                                rollArray
                                                sortableSetRolloutOffset 
                                                orderV)))
                localSwitchOffset <- localSwitchOffset + 1

        sortableIndex <- sortableIndex + 1



    // creates a (sorter.switchcount * sortableCount ) length 
    // array to store each switch use, thus no SAG (Switch 
    // Action Grouping)
    // Uses the sorter to sort a copy of the rollingSorableData
    // returns the transformed copy of the rollingSorableData
    // along with
    let sorterWithNoSAG
                    (sorter:sorter)
                    (sortableSetId:sortableSetId)
                    (symbolSetSize:symbolSetSize)
                    (rollout:rollout) =

        let sortableCountV = rollout 
                            |> Rollout.getArrayLength
                            |> ArrayLength.value

        //if (switchCount <> sorter.switchCount) then 
        //        failwith (sprintf "useRollLength %d is not correct" ( switchCount |> SwitchCount.value ))

        let sortableCount = rollout 
                                |> Rollout.getArrayCount
                                |> ArrayCount.value
                                |> SortableCount.create

        let rollingUseCountArrayLength = (sorter.switchCount |> SwitchCount.value) *
                                         (sortableCount |> SortableCount.value)

        let rollingUseCounts = Array.zeroCreate<bool> rollingUseCountArrayLength
        let rollingSortableDataCopy = rollout |> Rollout.copy
        let sortableSetSorted = rollingSortableDataCopy 
                                    |> SortableSet.makeArrayRoll sortableSetId symbolSetSize
        match rollingSortableDataCopy with
        | U8 _uInt8Roll -> switchRangeWithNoSAGuInt8Roll
                                sorter
                                _uInt8Roll
                                rollingUseCounts
                                sortableCount

        | U16 _uInt16Roll -> switchRangeWithNoSAGuInt16Roll
                                sorter
                                _uInt16Roll
                                rollingUseCounts
                                sortableCount

        | I32 _intRoll -> switchRangeWithNoSAGintRoll
                                sorter
                                _intRoll
                                rollingUseCounts
                                sortableCount

        | U64 _uInt64Roll -> failwith "not implemented"


        let switchEventRollout = SwitchingLogArrayRoll.create
                                                sorter.switchCount
                                                sortableCount
                                                rollingUseCounts
                                    |> switchingLog.ArrayRoll

        (sortableSetSorted, switchEventRollout) |> sortingResults.NoGrouping


    //// uses a sorter.switchcount length array to store accumulated
    //// switch uses
    //let private switchRangeMakeSwitchUses 
    //                (sorter:sorter) 
    //                (mindex:int) 
    //                (maxdex:int) 
    //                (switchUses:switchUses) 
    //                (sortableSetRollout:intSetsRollout) 
    //                (sortableIndex:int) =
    //    let useWeights = (SwitchUses.getWeights switchUses)
    //    let sortableSetRolloutOffset = sortableIndex * (Degree.value sorter.degree)
    //    let mutable looP = true
    //    let mutable localSwitchOffset = mindex
    //    while ((localSwitchOffset < maxdex) && looP) do
    //        let switch = sorter.switches.[localSwitchOffset]
    //        let lv = sortableSetRollout.baseArray.[switch.low + sortableSetRolloutOffset]
    //        let hv = sortableSetRollout.baseArray.[switch.hi + sortableSetRolloutOffset]
    //        if(lv > hv) then
    //            sortableSetRollout.baseArray.[switch.hi + sortableSetRolloutOffset] <- lv
    //            sortableSetRollout.baseArray.[switch.low + sortableSetRolloutOffset] <- hv
    //            useWeights.[localSwitchOffset] <- useWeights.[localSwitchOffset] + 1
    //            looP <- ((localSwitchOffset % 20 > 0) ||
    //                     (not (Combinatorics.isSortedOffsetI 
    //                                            sortableSetRollout.baseArray 
    //                                            sortableSetRolloutOffset 
    //                                            (Degree.value(sorter.degree)))))
    //        localSwitchOffset <- localSwitchOffset+1




    //// uses a sorter.switchcount length array to store accumulated
    //// switch uses
    //let sorterMakeSwitchUses 
    //                (sorter:sorter) 
    //                (ssRollout:intSetsRollout) 
    //                (switchusePlan:Sorting.switchUsePlan) =
    //    let switchCount = (SwitchCount.value sorter.switchCount)
    //    let firstSwitchDex, lastSwitchDex, switchUses = 
    //        match switchusePlan with
    //        | Sorting.switchUsePlan.All -> 
    //            (0, switchCount, (SwitchUses.createEmpty sorter.switchCount))
    //        | Sorting.switchUsePlan.Range (min, max) -> 
    //            (min, max, (SwitchUses.createEmpty sorter.switchCount))
    //        | Sorting.switchUsePlan.Indexes (min, max, swu) -> 
    //            let cpyWgts = swu.weights |> Array.copy
    //            (min, max, SwitchUses.init cpyWgts)

    //    let sortableSetRolloutCopy = (IntSetsRollout.copy ssRollout)
    //    let mutable sortableIndex=0
    //    while (sortableIndex < (SortableCount.value ssRollout.sortableCount)) do
    //            switchRangeMakeSwitchUses 
    //                sorter 
    //                firstSwitchDex 
    //                lastSwitchDex 
    //                switchUses 
    //                sortableSetRolloutCopy 
    //                sortableIndex
    //            sortableIndex <- sortableIndex + 1
    //    switchEventRecords.BySwitch {
    //        groupBySwitch.switchUses = switchUses; 
    //        groupBySwitch.sortableRollout = sortableSetRollout.Int 
    //                                            sortableSetRolloutCopy
    //}

    //// uses a sorter.switchcount length array to store accumulated
    //// switch uses
    //let private switchRangeMakeSwitchUsesSlice
    //                (sorter:sorter) 
    //                (mindex:int) 
    //                (maxdex:int) 
    //                (switchUses:switchUses) 
    //                (sortableArray:int[]) =
    //    let useWeights = (SwitchUses.getWeights switchUses)
    //    let mutable looP = true
    //    let mutable localSwitchOffset = mindex
    //    while ((localSwitchOffset < maxdex) && looP) do
    //        let switch = sorter.switches.[localSwitchOffset]
    //        let lv = sortableArray.[switch.low]
    //        let hv = sortableArray.[switch.hi]
    //        if(lv > hv) then
    //            sortableArray.[switch.hi] <- lv
    //            sortableArray.[switch.low] <- hv
    //            useWeights.[localSwitchOffset] <- useWeights.[localSwitchOffset] + 1
    //            looP <- ((localSwitchOffset % 20 > 0) ||
    //                     (not (Combinatorics.isSortedI sortableArray )))
    //        localSwitchOffset <- localSwitchOffset+1



    //// uses a sorter.switchcount length array to store accumulated
    //// switch uses
    //let sorterMakeSwitchUsesSlice
    //                (sorter:sorter) 
    //                (ssRollout:intSetsRollout) 
    //                (switchusePlan:Sorting.switchUsePlan) =
    //    let switchCount = (SwitchCount.value sorter.switchCount)
    //    let firstSwitchDex, lastSwitchDex, switchUses = 
    //        match switchusePlan with
    //        | Sorting.switchUsePlan.All -> 
    //            (0, switchCount, (SwitchUses.createEmpty sorter.switchCount))
    //        | Sorting.switchUsePlan.Range (min, max) -> 
    //            (min, max, (SwitchUses.createEmpty sorter.switchCount))
    //        | Sorting.switchUsePlan.Indexes (min, max, swu) -> 
    //            let cpyWgts = swu.weights |> Array.copy
    //            (min, max, SwitchUses.init cpyWgts)

    //    let intsRolloutCopy = Array.zeroCreate ssRollout.baseArray.Length
    //    let sortableArray = Array.zeroCreate (Degree.value ssRollout.degree)
    //    let mutable sortableIndex=0
    //    while (sortableIndex < (SortableCount.value ssRollout.sortableCount)) do
    //            let bDexMin = sortableIndex * (Degree.value sorter.degree)
    //            let bDexMax = bDexMin + (Degree.value sorter.degree) - 1
    //            ByteUtils.mapIntArrays bDexMin ssRollout.baseArray 0 sortableArray (Degree.value sorter.degree)
    //            switchRangeMakeSwitchUsesSlice 
    //                sorter 
    //                firstSwitchDex 
    //                lastSwitchDex 
    //                switchUses 
    //                sortableArray 
    //            sortableIndex <- sortableIndex + 1
    //            ByteUtils.mapIntArrays 0 sortableArray bDexMin intsRolloutCopy (Degree.value sorter.degree)
    //    switchEventRecords.BySwitch {
    //        groupBySwitch.switchUses = switchUses; 
    //        groupBySwitch.sortableRollout = sortableSetRollout.Int 
    //                                            (IntSetsRollout.createNr ssRollout.degree intsRolloutCopy)
    //    }
        

    //let evalSorterOnIntSetsRollout
    //                (sorter:sorter)
    //                (intSetsRollout:intSetsRollout)
    //                (switchusePlan:Sorting.switchUsePlan) 
    //                (switchEventAgg:Sorting.eventGrouping) =
    //    match switchEventAgg with
    //    | Sorting.eventGrouping.NoGrouping -> 
    //            sorterWithNoSAG 
    //                sorter intSetsRollout switchusePlan
    //    | Sorting.eventGrouping.BySwitch -> 
    //            sorterMakeSwitchUses 
    //                sorter intSetsRollout switchusePlan


    //let evalSorterOnBinary (sorter:sorter)
    //               (intSetsRollout:intSetsRollout)
    //               (switchusePlan:Sorting.switchUsePlan) 
    //               (switchEventAgg:Sorting.eventGrouping) =
    //    evalSorterOnIntSetsRollout
    //        sorter intSetsRollout switchusePlan switchEventAgg


    //let evalSorterOnInteger (sorter:sorter)
    //                        (intSetsRollout:intSetsRollout)
    //                        (switchusePlan:Sorting.switchUsePlan) 
    //                        (switchEventAgg:Sorting.eventGrouping) =
    //    evalSorterOnIntSetsRollout
    //        sorter intSetsRollout switchusePlan switchEventAgg



    //module SorterSet =

    //    let eval<'T> 
    //            (sorterSet:sorterSet)
    //            (intSetsRollout:intSetsRollout)
    //            (switchusePlan:Sorting.switchUsePlan) 
    //            (switchEventAgg:Sorting.eventGrouping) 
    //            (_parallel:UseParallel) 
    //            (proc:sortingResult -> Result<'T, string>) =

    //        let rewrap ssr tup = 
    //            let sorterId, sorter = tup
    //            let swEvRecs = evalSorterOnIntSetsRollout 
    //                                sorter ssr switchusePlan switchEventAgg
    //            let resSoSS = {
    //                sortingResult.sorter = sorter;
    //                sortingResult.switchEventRecords = swEvRecs;
    //                sortingResult.sorterId = sorterId;
    //            }
    //            proc resSoSS

    //        result  {
    //            return!
    //                match UseParallel.value(_parallel) with
    //                | true  -> sorterSet.sorters |> Map.toArray 
    //                                             |> Array.Parallel.map(rewrap intSetsRollout)
    //                                             |> Array.toList
    //                                             |> Result.sequence
    //                | false -> sorterSet.sorters |> Map.toList 
    //                                             |> List.map(rewrap intSetsRollout)
    //                                             |> Result.sequence
    //        }
