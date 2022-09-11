namespace global

module SortingRollout =

    //*********************************************************************
    //********************    Switch Track    *******************************
    //*********************************************************************

    // uses a bool[sorter.switchcount * sortableCount]
    // array to store each switch use
    let private sortAndMakeSwitchTrackForIntRoll
                (sorter:sorter)
                (intRoll:intRoll)
                (sortableCount:sortableCount) =

        let rollArray = intRoll |> IntRoll.getData
        let orderV = sorter.order |> Order.value
        let switchingLg = 
            SortableBySwitchTrackerStandard.create
                        sorter.switchCount
                        sortableCount
            
        let rollingUseArray = switchingLg |> SortableBySwitchTrackerStandard.getData

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

        switchingLg |> sortableBySwitchTracker.ArrayRoll


    // uses a bool[sorter.switchcount * sortableCount]
    // array to store each switch use
    let private sortAndMakeSwitchTrackForUInt8Roll
                (sorter:sorter)
                (uInt8Roll:uInt8Roll)
                (sortableCount:sortableCount) =

        let rollArray = uInt8Roll |> Uint8Roll.getData
        let orderV = sorter.order |> Order.value
        let switchingLg = 
            SortableBySwitchTrackerStandard.create
                        sorter.switchCount
                        sortableCount
            
        let rollingUseArray = switchingLg |> SortableBySwitchTrackerStandard.getData

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

        switchingLg |> sortableBySwitchTracker.ArrayRoll



    // uses a bool[sorter.switchcount * sortableCount]
    // array to store each switch use
    let private sortAndMakeSwitchTrackForUInt16Roll
                (sorter:sorter)
                (uInt8Roll:uInt16Roll)
                (sortableCount:sortableCount) =
        let rollArray = uInt8Roll |> Uint16Roll.getData
        let orderV = sorter.order |> Order.value

        let switchingLg = 
            SortableBySwitchTrackerStandard.create
                        sorter.switchCount
                        sortableCount

        let rollingUseArray = switchingLg |> SortableBySwitchTrackerStandard.getData

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
        
        switchingLg |> sortableBySwitchTracker.ArrayRoll


    // uses a bool[sorter.switchcount * sortableCount]
    // array to store each switch use
    let sortAndMakeSwitchTrackForBs64Roll
                (sorter:sorter)
                (bs64Roll:bs64Roll)
                (sortableCount:sortableCount) =
        let rollArray = bs64Roll |> Bs64Roll.getData
        let orderV = sorter.order |> Order.value
        let switchCts = sorter.switchCount |> SwitchCount.value

        let switchingLg = 
            SortableBySwitchTrackerBitStriped.create
                        sorter.switchCount
                        sortableCount

        let switchUseArray = 
            switchingLg |> SortableBySwitchTrackerBitStriped.getData

        // loop over sortables, then over switches
        let mutable sortableIndex = 0
        let mutable sortableSetOffset = 0
        while (sortableSetOffset < (bs64Roll |> Bs64Roll.getDataArrayLength)) do
            let switchEventOffset = sortableIndex * switchCts
            let mutable localSwitchOffset = 0
            while (localSwitchOffset < switchCts) do
                let switch = sorter.switches.[localSwitchOffset]
                let lv = rollArray.[switch.low + sortableSetOffset]
                let hv = rollArray.[switch.hi + sortableSetOffset]
                rollArray.[switch.hi + sortableSetOffset] <- (lv ||| hv)
                rollArray.[switch.low + sortableSetOffset] <- (lv &&& hv)

                let rv = switchUseArray.[localSwitchOffset]
                switchUseArray.[switchEventOffset + localSwitchOffset] 
                    <- (((~~~hv) &&& lv) ||| rv)
                localSwitchOffset <- localSwitchOffset + 1
            sortableIndex <- sortableIndex + 1
            sortableSetOffset <- sortableSetOffset + orderV
        switchingLg |> sortableBySwitchTracker.BitStriped



    // Uses the sorter to sort a copy of the rollingSorableData
    // returns the transformed copy of the rollingSorableData
    // along with a bool[sorter.switchcount] to store each switch use.
    let applySorterAndMakeSwitchTrack
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

        let switchingTrack =
            match rolloutCopy with
            | B _ ->  failwith "not implemented"

            | U8 _uInt8Roll -> 
                    sortAndMakeSwitchTrackForUInt8Roll
                        sorter
                        _uInt8Roll
                        sortableCount

            | U16 _uInt16Roll -> 
                    sortAndMakeSwitchTrackForUInt16Roll
                        sorter
                        _uInt16Roll
                        sortableCount

            | I32 _intRoll -> 
                    sortAndMakeSwitchTrackForIntRoll
                        sorter
                        _intRoll
                        sortableCount

            | U64 _uInt64Roll -> failwith "not implemented"

            | Bs64 _bs64Roll -> 
                    sortAndMakeSwitchTrackForBs64Roll
                        sorter
                        _bs64Roll
                        sortableCount

        SorterOpResults.make 
            sorterId 
            sortableSetId 
            rolloutCopy 
            (switchingTrack |> sorterOpTracker.NoGrouping )


    
    //*********************************************************************
    //********************    Switch Use Counts    ************************
    //*********************************************************************

    // uses an int[sorter.switchcount] array to accumulate
    // the counts of each switch use
    let private sortAndMakeSwitchUsesForIntRoll
                    (sorter:sorter)
                    (intRoll:intRoll)
                    (sortableCount:sortableCount) =
        
        let switchUses = SwitchUseTrackerStandard.make sorter.switchCount
        let rollArray = intRoll |> IntRoll.getData
        let useCounts = (SwitchUseTrackerStandard.getUseCounters switchUses)
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
        switchUses |> switchUseTracker.Standard


    // uses an int[sorter.switchcount] array to accumulate
    // the counts of each switch use
    let private sortAndMakeSwitchUsesForUInt8Roll
                (sorter:sorter)
                (uInt8Roll:uInt8Roll)
                (sortableCount:sortableCount) =
        
        let switchUses = SwitchUseTrackerStandard.make sorter.switchCount
        let rollArray = uInt8Roll |> Uint8Roll.getData
        let useCounts = (SwitchUseTrackerStandard.getUseCounters switchUses)
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
        switchUses |> switchUseTracker.Standard


    // uses an int[sorter.switchcount] array to accumulate
    // the counts of each switch use
    let private sortAndMakeSwitchUsesForUInt16Roll
                (sorter:sorter)
                (uInt8Roll:uInt16Roll)
                (sortableCount:sortableCount) =
                
        let switchUses = SwitchUseTrackerStandard.make sorter.switchCount
        let rollArray = uInt8Roll |> Uint16Roll.getData
        let useCounts = (SwitchUseTrackerStandard.getUseCounters switchUses)
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
        switchUses |> switchUseTracker.Standard


    let sortAndMakeSwitchUsesForBs64Roll
                (sorter:sorter)
                (bs64Roll:bs64Roll) =
        
        let switchUses = SwitchUseTrackerBitStriped.make sorter.switchCount
        let rollArray = bs64Roll |> Bs64Roll.getData
        let useFlags = switchUses |> SwitchUseTrackerBitStriped.getUseFlags 
        let orderV = sorter.order |> Order.value
        let switchCts = sorter.switchCount |> SwitchCount.value

        let mutable sortableSetOffset = 0
        while (sortableSetOffset < (bs64Roll |> Bs64Roll.getDataArrayLength )) do
            let mutable localSwitchOffset = 0
            while (localSwitchOffset < switchCts) do
                let switch = sorter.switches.[localSwitchOffset]
                let lv = rollArray.[switch.low + sortableSetOffset]
                let hv = rollArray.[switch.hi + sortableSetOffset]
                rollArray.[switch.hi + sortableSetOffset] <- (lv ||| hv)
                rollArray.[switch.low + sortableSetOffset] <- (lv &&& hv)

                let rv = useFlags.[localSwitchOffset]
                useFlags.[localSwitchOffset] <- (((~~~hv) &&& lv) ||| rv)
                localSwitchOffset <- localSwitchOffset + 1
            sortableSetOffset <- sortableSetOffset + orderV
        switchUses |> switchUseTracker.BitStriped

    

    let sortAndMakeSwitchUsesForBs64Rollp
                (sorter:sorter)
                (bs64Roll:bs64Roll) =
        
        let switchUses = SwitchUseTrackerBitStriped.make sorter.switchCount
        let rollArray = bs64Roll |> Bs64Roll.getData
        let useFlags = switchUses |> SwitchUseTrackerBitStriped.getUseFlags 
        let orderV = sorter.order |> Order.value
        let switchCts = sorter.switchCount |> SwitchCount.value
        let maxOffset = bs64Roll 
                        |> Bs64Roll.getDataArrayLength
                        |> (+) (-1)

        let _sortIt (sortableSetOffset:int) =
            let mutable localSwitchOffset = 0
            while (localSwitchOffset < switchCts) do
                let switch = sorter.switches.[localSwitchOffset]
                let lv = rollArray.[switch.low + sortableSetOffset]
                let hv = rollArray.[switch.hi + sortableSetOffset]
                rollArray.[switch.hi + sortableSetOffset] <- (lv ||| hv)
                rollArray.[switch.low + sortableSetOffset] <- (lv &&& hv)

                let rv = useFlags.[localSwitchOffset]
                useFlags.[localSwitchOffset] <- (((~~~hv) &&& lv) ||| rv)
                localSwitchOffset <- localSwitchOffset + 1
          
        let offsets = [|0 .. orderV .. maxOffset|]
        let wak =
            offsets
            |> Array.Parallel.map(_sortIt)

        switchUses |> switchUseTracker.BitStriped

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
        
        let switchUseTracker =
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


        SorterOpResults.make 
            sorterId 
            sortableSetId 
            rolloutCopy 
            (switchUseTracker |> sorterOpTracker.GroupBySwitch )


    let evalSorterWithSortableSet
            (sorterOpTrackMod:sorterOpTrackMode)
            (sorter:sorter) 
            (sortableSet:sortableSet) =
        let sorterId = sorter |> Sorter.makeId

        match sorterOpTrackMod with
        | sorterOpTrackMode.SwitchTrack ->
            applySorterAndMakeSwitchTrack
                sorter
                sorterId
                (sortableSet |> SortableSet.getSortableSetId)
                (sortableSet |> SortableSet.getRollout)
        | sorterOpTrackMode.SwitchUses ->
            applySorterAndMakeSwitchUses
                sorter
                sorterId
                (sortableSet |> SortableSet.getSortableSetId)
                (sortableSet |> SortableSet.getRollout)
