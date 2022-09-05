namespace global

module SortingRollout =

    //*********************************************************************
    //********************    Switch Log    *******************************
    //*********************************************************************

    // uses a bool[sorter.switchcount * sortableCount]
    // array to store each switch use
    let private sortAndMakeSwitchLogForIntRoll
                (sorter:sorter)
                (intRoll:intRoll)
                (sortableCount:sortableCount) =

        let rollArray = intRoll |> IntRoll.getData
        let orderV = sorter.order |> Order.value
        let switchingLg = 
            SwitchingLogArrayRoll.create
                        sorter.switchCount
                        sortableCount
            
        let rollingUseArray = switchingLg |> SwitchingLogArrayRoll.getData

        // loop over sortables, then over switches
        let mutable sortableIndex = 0
        while (sortableIndex < (SortableCount.value sortableCount)) do
            let mutable looP = true
            let sortableSetRolloutOffset = sortableIndex * orderV
            let switchEventRolloutOffset = sortableIndex * (SwitchCount.value sorter.switchCount)
            
            let mutable localSwitchOffset = 0
            while ((localSwitchOffset < (sorter.switchCount |> SwitchCount.value)) && looP) do
                let switch = sorter.switches.[localSwitchOffset]
                let lv = rollArray.[switch.low + sortableSetRolloutOffset]
                let hv = rollArray.[switch.hi + sortableSetRolloutOffset]
                if(lv > hv) then
                    rollArray.[switch.hi + sortableSetRolloutOffset] <- lv
                    rollArray.[switch.low + sortableSetRolloutOffset] <- hv
                    rollingUseArray.[localSwitchOffset + switchEventRolloutOffset] <- true
                looP <- ((localSwitchOffset % 20 > 0) ||
                         (not (CollectionProps.isSortedOffset
                                                rollArray
                                                sortableSetRolloutOffset 
                                                orderV)))
                localSwitchOffset <- localSwitchOffset + 1

            sortableIndex <- sortableIndex + 1

        switchingLg |> switchingLog.ArrayRoll


    // uses a bool[sorter.switchcount * sortableCount]
    // array to store each switch use
    let private sortAndMakeSwitchLogForUInt8Roll
                (sorter:sorter)
                (uInt8Roll:uInt8Roll)
                (sortableCount:sortableCount) =

        let rollArray = uInt8Roll |> Uint8Roll.getData
        let orderV = sorter.order |> Order.value
        let switchingLg = 
            SwitchingLogArrayRoll.create
                        sorter.switchCount
                        sortableCount
            
        let rollingUseArray = switchingLg |> SwitchingLogArrayRoll.getData

        // loop over sortables, then over switches
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
                    rollingUseArray.[localSwitchOffset + switchEventRolloutOffset] <- true
                looP <- ((localSwitchOffset % 20 > 0) ||
                         (not (CollectionProps.isSortedOffset
                                                rollArray
                                                sortableSetRolloutOffset 
                                                orderV)))
                localSwitchOffset <- localSwitchOffset + 1

            sortableIndex <- sortableIndex + 1

        switchingLg |> switchingLog.ArrayRoll



    // uses a bool[sorter.switchcount * sortableCount]
    // array to store each switch use
    let private sortAndMakeSwitchLogForUInt16Roll
                (sorter:sorter)
                (uInt8Roll:uInt16Roll)
                (sortableCount:sortableCount) =
        let rollArray = uInt8Roll |> Uint16Roll.getData
        let orderV = sorter.order |> Order.value

        let switchingLg = 
            SwitchingLogArrayRoll.create
                        sorter.switchCount
                        sortableCount

        let rollingUseArray = switchingLg |> SwitchingLogArrayRoll.getData

        // loop over sortables, then over switches
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
                    rollingUseArray.[localSwitchOffset + switchEventRolloutOffset] <- true
                looP <- ((localSwitchOffset % 20 > 0) ||
                         (not (CollectionProps.isSortedOffset
                                                rollArray
                                                sortableSetRolloutOffset 
                                                orderV)))
                localSwitchOffset <- localSwitchOffset + 1

            sortableIndex <- sortableIndex + 1
        
        switchingLg |> switchingLog.ArrayRoll


    // uses a bool[sorter.switchcount * sortableCount]
    // array to store each switch use
    let sortAndMakeSwitchLogForBs64Roll
                (sorter:sorter)
                (bs64Roll:bs64Roll)
                (sortableCount:sortableCount) =
        let rollArray = bs64Roll |> Bs64Roll.getData
        let orderV = sorter.order |> Order.value
        let switchCts = sorter.switchCount |> SwitchCount.value

        let switchingLg = 
            SwitchingLogBitStriped.create
                        sorter.switchCount
                        sortableCount

        let dataUseArray = switchingLg |> SwitchingLogBitStriped.getData

        // loop over sortables, then over switches
        let mutable sortableIndex = 0
        while (sortableIndex < (SortableCount.value sortableCount)) do
            let mutable localSwitchOffset = 0
            let sortableSetRolloutOffset = sortableIndex * orderV
            let switchEventRolloutOffset = sortableIndex * (SwitchCount.value sorter.switchCount)
            while (sortableIndex < (SortableCount.value sortableCount)) do
                let mutable localSwitchOffset = 0
                let sortableSetRolloutOffset = sortableIndex * orderV

                while (localSwitchOffset < switchCts) do
                    let switch = sorter.switches.[localSwitchOffset]
                    let lv = rollArray.[switch.low + sortableSetRolloutOffset]
                    let hv = rollArray.[switch.hi + sortableSetRolloutOffset]
                    let rv = dataUseArray.[localSwitchOffset]
                    rollArray.[switch.hi + sortableSetRolloutOffset] <- (lv ||| hv)
                    rollArray.[switch.low + sortableSetRolloutOffset] <- (lv &&& hv)
                    dataUseArray.[localSwitchOffset] <- (((~~~hv) &&& lv) ||| rv)
                    localSwitchOffset <- localSwitchOffset + 1
                sortableIndex <- sortableIndex + 1
        switchingLg |> switchingLog.BitStriped



    // Uses the sorter to sort a copy of the rollingSorableData
    // returns the transformed copy of the rollingSorableData
    // along with a bool[sorter.switchcount] to store each switch use.
    let applySorterAndMakeSwitchLog
                    (sorter:sorter)
                    (sorterId:sorterId)
                    (sortableSetId:sortableSetId)
                    (rollout:rollout) =

        let sortableCount = 
            rollout 
            |> Rollout.getArrayCount
            |> ArrayCount.value
            |> SortableCount.create

        let rolloutCopy = rollout |> Rollout.copy

        let switchingLog =
            match rolloutCopy with
            | B _ ->  failwith "not implemented"

            | U8 _uInt8Roll -> 
                    sortAndMakeSwitchLogForUInt8Roll
                        sorter
                        _uInt8Roll
                        sortableCount

            | U16 _uInt16Roll -> 
                    sortAndMakeSwitchLogForUInt16Roll
                        sorter
                        _uInt16Roll
                        sortableCount

            | I32 _intRoll -> 
                    sortAndMakeSwitchLogForIntRoll
                        sorter
                        _intRoll
                        sortableCount

            | U64 _uInt64Roll -> failwith "not implemented"

            | Bs64 _bs64Roll -> 
                    sortAndMakeSwitchLogForBs64Roll
                        sorter
                        _bs64Roll
                        sortableCount


        (sorterId, sortableSetId, rolloutCopy, switchingLog) 
            |> sorterResults.NoGrouping


    
    //*********************************************************************
    //********************    Switch Use Counts    ************************
    //*********************************************************************

    // uses an int[sorter.switchcount] array to accumulate
    // the counts of each switch use
    let private sortAndMakeSwitchUsesForIntRoll
                    (sorter:sorter)
                    (intRoll:intRoll)
                    (sortableCount:sortableCount) =
        
        let switchUses = SwitchUseTrackStandard.make sorter.switchCount
        let rollArray = intRoll |> IntRoll.getData
        let useCounts = (SwitchUseTrackStandard.getUseCounts switchUses)
        let orderV = sorter.order |> Order.value

        let mutable sortableIndex = 0
        while (sortableIndex < (SortableCount.value sortableCount)) do

            let mutable looP = true
            let mutable localSwitchOffset = 0
            let sortableSetRolloutOffset = sortableIndex * orderV

            while ((localSwitchOffset < (sorter.switchCount |> SwitchCount.value)) && looP) do
                let switch = sorter.switches.[localSwitchOffset]
                let lv = rollArray.[switch.low + sortableSetRolloutOffset]
                let hv = rollArray.[switch.hi + sortableSetRolloutOffset]
                if(lv > hv) then
                    rollArray.[switch.hi + sortableSetRolloutOffset] <- lv
                    rollArray.[switch.low + sortableSetRolloutOffset] <- hv
                    useCounts.[localSwitchOffset] <- useCounts.[localSwitchOffset] + 1
                looP <- ((localSwitchOffset % 20 > 0) ||
                         (not (CollectionProps.isSortedOffset
                                                rollArray
                                                sortableSetRolloutOffset 
                                                orderV)))
                localSwitchOffset <- localSwitchOffset + 1

            sortableIndex <- sortableIndex + 1
        switchUses |> switchUseTrack.Standard


    // uses an int[sorter.switchcount] array to accumulate
    // the counts of each switch use
    let private sortAndMakeSwitchUsesForUInt8Roll
                (sorter:sorter)
                (uInt8Roll:uInt8Roll)
                (sortableCount:sortableCount) =
        
        let switchUses = SwitchUseTrackStandard.make sorter.switchCount
        let rollArray = uInt8Roll |> Uint8Roll.getData
        let useCounts = (SwitchUseTrackStandard.getUseCounts switchUses)
        let orderV = sorter.order |> Order.value

        let mutable sortableIndex = 0
        while (sortableIndex < (SortableCount.value sortableCount)) do

            let mutable looP = true
            let mutable localSwitchOffset = 0
            let sortableSetRolloutOffset = sortableIndex * orderV

            while ((localSwitchOffset < (sorter.switchCount |> SwitchCount.value)) && looP) do
                let switch = sorter.switches.[localSwitchOffset]
                let lv = rollArray.[switch.low + sortableSetRolloutOffset]
                let hv = rollArray.[switch.hi + sortableSetRolloutOffset]
                if(lv > hv) then
                    rollArray.[switch.hi + sortableSetRolloutOffset] <- lv
                    rollArray.[switch.low + sortableSetRolloutOffset] <- hv
                    useCounts.[localSwitchOffset] <- useCounts.[localSwitchOffset] + 1
                looP <- ((localSwitchOffset % 20 > 0) ||
                         (not (CollectionProps.isSortedOffset
                                                rollArray
                                                sortableSetRolloutOffset 
                                                orderV)))
                localSwitchOffset <- localSwitchOffset + 1

            sortableIndex <- sortableIndex + 1
        switchUses |> switchUseTrack.Standard


    // uses an int[sorter.switchcount] array to accumulate
    // the counts of each switch use
    let private sortAndMakeSwitchUsesForUInt16Roll
                (sorter:sorter)
                (uInt8Roll:uInt16Roll)
                (sortableCount:sortableCount) =
                
        let switchUses = SwitchUseTrackStandard.make sorter.switchCount
        let rollArray = uInt8Roll |> Uint16Roll.getData
        let useCounts = (SwitchUseTrackStandard.getUseCounts switchUses)
        let orderV = sorter.order |> Order.value

        let mutable sortableIndex = 0
        while (sortableIndex < (SortableCount.value sortableCount)) do

            let mutable looP = true
            let mutable localSwitchOffset = 0
            let sortableSetRolloutOffset = sortableIndex * orderV

            while ((localSwitchOffset < (sorter.switchCount |> SwitchCount.value)) && looP) do
                let switch = sorter.switches.[localSwitchOffset]
                let lv = rollArray.[switch.low + sortableSetRolloutOffset]
                let hv = rollArray.[switch.hi + sortableSetRolloutOffset]
                if(lv > hv) then
                    rollArray.[switch.hi + sortableSetRolloutOffset] <- lv
                    rollArray.[switch.low + sortableSetRolloutOffset] <- hv
                    useCounts.[localSwitchOffset] <- useCounts.[localSwitchOffset] + 1
                looP <- ((localSwitchOffset % 20 > 0) ||
                         (not (CollectionProps.isSortedOffset
                                                rollArray
                                                sortableSetRolloutOffset 
                                                orderV)))
                localSwitchOffset <- localSwitchOffset + 1

            sortableIndex <- sortableIndex + 1
        switchUses |> switchUseTrack.Standard


    let sortAndMakeSwitchUsesForBs64Roll
                (sorter:sorter)
                (bs64Roll:bs64Roll)
                (sortableCount:sortableCount) =
        
        let switchUses = SwitchUseTrackBitStriped.make sorter.switchCount
        let rollArray = bs64Roll |> Bs64Roll.getData
        let useFlags = switchUses |> SwitchUseTrackBitStriped.getUseFlags 
        let orderV = sorter.order |> Order.value
        let switchCts = sorter.switchCount |> SwitchCount.value

        let mutable sortableIndex = 0
        while (sortableIndex < (SortableCount.value sortableCount)) do
            let mutable localSwitchOffset = 0
            let sortableSetRolloutOffset = sortableIndex * orderV

            while (localSwitchOffset < switchCts) do
                let switch = sorter.switches.[localSwitchOffset]
                let lv = rollArray.[switch.low + sortableSetRolloutOffset]
                let hv = rollArray.[switch.hi + sortableSetRolloutOffset]
                let rv = useFlags.[localSwitchOffset]
                rollArray.[switch.hi + sortableSetRolloutOffset] <- (lv ||| hv)
                rollArray.[switch.low + sortableSetRolloutOffset] <- (lv &&& hv)
                useFlags.[localSwitchOffset] <- (((~~~hv) &&& lv) ||| rv)
                localSwitchOffset <- localSwitchOffset + 1
            sortableIndex <- sortableIndex + 1
        switchUses |> switchUseTrack.BitStriped

    
    // Uses the sorter to sort a copy of the rollingSorableData
    // returns the transformed copy of the rollingSorableData
    // along with an int[sorter.switchcount] to accumulate the 
    // count of each switch use
    let applySorterAndMakeSwitchUses
                    (sorter:sorter)
                    (sorterId:sorterId)
                    (sortableSetId:sortableSetId)
                    (rollout:rollout) =

        let sortableCount = 
            rollout 
            |> Rollout.getArrayCount
            |> ArrayCount.value
            |> SortableCount.create

        let rolloutCopy = rollout |> Rollout.copy
        
        let switchUses =
            match rolloutCopy with
            | B _ ->  failwith "not implemented"

            | U8 _uInt8Roll -> 
                sortAndMakeSwitchUsesForUInt8Roll
                                    sorter
                                    _uInt8Roll
                                    sortableCount

            | U16 _uInt16Roll ->
                sortAndMakeSwitchUsesForUInt16Roll
                                    sorter
                                    _uInt16Roll
                                    sortableCount

            | I32 _intRoll ->
                sortAndMakeSwitchUsesForIntRoll
                                    sorter
                                    _intRoll
                                    sortableCount

            | U64 _uInt64Roll -> failwith "not implemented"

            | Bs64 _bs644Roll -> 
                    sortAndMakeSwitchUsesForBs64Roll
                                    sorter
                                    _bs644Roll
                                    sortableCount

        (sorterId, sortableSetId, rolloutCopy, switchUses) 
        |> sorterResults.BySwitch





























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
