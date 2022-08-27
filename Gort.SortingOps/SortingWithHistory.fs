namespace global

module SortingWithHistory =

    module Ints =

        let sortTHistSwitches (switches:switch list)
                              (sortableInts:sortableInts) =
            let mutable i = 0
            let mutable lstRet = [sortableInts]
            let mutable newCase = sortableInts

            while (i < switches.Length) do
                newCase <- newCase |> SortableInts.copy
                let intArray = newCase |> SortableInts.getValues
                let switch = switches.[i]
                let lv = intArray.[switch.low]
                let hv = intArray.[switch.hi]
                if(lv > hv) then
                    intArray.[switch.hi] <- lv
                    intArray.[switch.low] <- hv
                lstRet <- newCase::lstRet
                i <- i+1
            lstRet |> List.rev


        let makeWithSorterSegment (sorter:sorter) 
                                  (mindex:int) 
                                  (maxdex:int) 
                                  (sortableInts:sortableInts) =
            let sws = sorter.switches |> Array.skip(mindex)
                                      |> Array.take(maxdex - mindex)
                                      |> Array.toList
            sortTHistSwitches sws sortableInts


        let makeWithFullSorter (sorter:sorter) (sortableInts:sortableInts) =
            let sl = SwitchCount.value sorter.switchCount
            makeWithSorterSegment sorter 0 sl sortableInts



    module Bools =

        let sortTHistSwitches (switches:switch list)
                              (sortableBools:sortableBools) =
            let mutable i = 0
            let mutable lstRet = [sortableBools]
            let mutable newCase = sortableBools

            while (i < switches.Length) do
                newCase <- newCase |> SortableBools.copy
                let intArray = newCase |> SortableBools.getValues
                let switch = switches.[i]
                let lv = intArray.[switch.low]
                let hv = intArray.[switch.hi]
                if(lv > hv) then
                    intArray.[switch.hi] <- lv
                    intArray.[switch.low] <- hv
                lstRet <- newCase::lstRet
                i <- i + 1
            lstRet |> List.rev


        let makeWithSorterSegment (sorter:sorter) 
                                (mindex:int) 
                                (maxdex:int) 
                                (sortableBools:sortableBools) =
            let sws = sorter.switches |> Array.skip(mindex)
                                      |> Array.take(maxdex - mindex)
                                      |> Array.toList
            sortTHistSwitches sws sortableBools


        let makeWithFullSorter (sorter:sorter) (sortableBools:sortableBools) =
            let sl = SwitchCount.value sorter.switchCount
            makeWithSorterSegment sorter 0 sl sortableBools
