namespace global

module SortingWithHistory =

    module Ints =

        let sortTHistSwitches (switches: switch list) (sortableInts: sortableIntArray) =
            let mutable i = 0
            let mutable lstRet = [ sortableInts ]
            let mutable newCase = sortableInts

            while (i < switches.Length) do
                newCase <- newCase |> SortableIntArray.copy
                let intArray = newCase |> SortableIntArray.getValues
                let switch = switches.[i]
                let lv = intArray.[switch.low]
                let hv = intArray.[switch.hi]

                if (lv > hv) then
                    intArray.[switch.hi] <- lv
                    intArray.[switch.low] <- hv

                lstRet <- newCase :: lstRet
                i <- i + 1

            lstRet |> List.rev


        let makeWithSorterSegment (sortr: sorter) (mindex: int) (maxdex: int) (sortableInts: sortableIntArray) =
            let sws =
                sortr
                |> Sorter.getSwitches
                |> Array.skip (mindex)
                |> Array.take (maxdex - mindex)
                |> Array.toList

            sortTHistSwitches sws sortableInts

        let makeWithFullSorter (sortr: sorter) (sortableInts: sortableIntArray) =
            let maxDex = sortr |> Sorter.getSwitchCount |> SwitchCount.value
            makeWithSorterSegment sortr 0 maxDex sortableInts


    module Bools =

        let sortTHistSwitches (switches: switch list) (sortableBools: sortableBoolArray) =
            let mutable i = 0
            let mutable lstRet = [ sortableBools ]
            let mutable newCase = sortableBools

            while (i < switches.Length) do
                newCase <- newCase |> SortableBoolArray.copy
                let intArray = newCase |> SortableBoolArray.getValues
                let switch = switches.[i]
                let lv = intArray.[switch.low]
                let hv = intArray.[switch.hi]

                if (lv > hv) then
                    intArray.[switch.hi] <- lv
                    intArray.[switch.low] <- hv

                lstRet <- newCase :: lstRet
                i <- i + 1

            lstRet |> List.rev


        let makeWithSorterSegment (sorter: sorter) (mindex: int) (maxdex: int) (sortableBools: sortableBoolArray) =
            let sws =
                sorter
                |> Sorter.getSwitches
                |> Array.skip (mindex)
                |> Array.take (maxdex - mindex)
                |> Array.toList

            sortTHistSwitches sws sortableBools


        let makeWithFullSorter (sortr: sorter) (sortableBools: sortableBoolArray) =
            let maxDex = sortr |> Sorter.getSwitchCount |> SwitchCount.value
            makeWithSorterSegment sortr 0 maxDex sortableBools
