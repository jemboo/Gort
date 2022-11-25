namespace global

open System

module SortersFromData =

    let ParseToStages (order: order) (stagesStr: string) =
        let MakeSwitch (s: string) =
            let pcs = s.Split([| ',' |]) |> Seq.map (fun i -> i |> int) |> Seq.toArray
            { switch.low = pcs.[0]; hi = pcs.[1] }

        stagesStr.Split([| '['; ']'; '\n'; '\r'; ' ' |], StringSplitOptions.RemoveEmptyEntries)
        |> Seq.map (fun l ->
            l.Split([| '('; ')' |], StringSplitOptions.RemoveEmptyEntries)
            |> Seq.filter (fun pc -> pc <> ",")
            |> Seq.map (fun pcs -> MakeSwitch pcs)
            |> Seq.toList)
        |> Seq.map (fun sws -> { stage.switches = sws; order = order })


    let ParseToSwitches (stagesStr: string) (order: order) =
        result {
            let! stages = Result.ErrorOnException (ParseToStages order) stagesStr
            return stages |> Seq.map (fun s -> s.switches |> List.toSeq) |> Seq.concat
        }


    let ParseToSorter (sorterD:sorterId) (sorterString: string) (order: order) =
        result {
            let! switchSeq = ParseToSwitches sorterString order
            let switches = switchSeq |> Seq.toArray
            return Sorter.fromSwitches sorterD order switches
        }


module SorterWriter =

    let sb = new System.Text.StringBuilder()
    let myPrint format = Printf.bprintf sb format

    let formatSwitch (s: switch) = myPrint "(%2d,%2d) " s.low s.hi

    let private _formatStage (st: stage) =
        myPrint "["
        st.switches |> List.iter (formatSwitch)
        myPrint "]\n"

    let private _formatSwitches (order: order) (switches: seq<switch>) =
        let stages = Stage.fromSwitches order switches
        stages |> Seq.iter (_formatStage)
        myPrint "\n"

    let private _runWriter writeAction =
        writeAction
        let strRet = sb.ToString()
        sb.Clear() |> ignore
        strRet

    let formatStage (st: stage) = _runWriter (_formatStage st)

    let formatSwitches (order: order) (switches: seq<switch>) =
        _runWriter (_formatSwitches order switches)



type RefSorter =
    | Degree3
    | Degree4
    | Degree4Prefix2
    | Degree5
    | Degree6
    | Degree7
    | Degree8
    | Degree8Prefix3
    | Degree9
    | Degree10
    | Degree10a
    | Degree11
    | Degree12
    | Degree12a
    | Degree13
    | Degree13a
    | Degree14
    | Degree14a
    | Degree15
    | Degree15a
    | Degree16
    | Degree16a
    | Green16
    | End16
    | Green16m
    | End16m
    | Degree17
    | Degree17a
    | Degree17b
    | Degree18
    | Degree19
    | Degree19a
    | Degree20
    | Degree20a
    | Degree21
    | Degree21a
    | Degree22
    | Degree22a
    | Degree22b
    | Degree23
    | Degree23a
    | Degree24
    | Degree24a
    | Degree25
    | Degree25a
    | Degree26
    | Degree26a
    | Degree27
    | Degree27a
    | Degree28
    | Degree28a
    | Degree29
    | Degree29a
    | Degree30
    | Degree30a
    | Degree31
    | Degree31a
    | Degree32
    | Degree32a


module RefSorter =

    let getStringAndDegree (refSorter: RefSorter) =
        let d (v: int) =
            let qua = (Order.create v) |> Result.toOption
            qua.Value

        match refSorter with
        | Degree3 -> (SorterData.Degree3Str, d 3) |> Ok
        | Degree4 -> (SorterData.Degree4Str, d 4) |> Ok
        | Degree4Prefix2 -> (SorterData.Degree4Prefix2Str, d 4) |> Ok
        | Degree5 -> (SorterData.Degree5Str, d 5) |> Ok
        | Degree6 -> (SorterData.Degree6Str, d 6) |> Ok
        | Degree7 -> (SorterData.Degree7Str, d 7) |> Ok
        | Degree8 -> (SorterData.Degree8Str, d 8) |> Ok
        | Degree9 -> (SorterData.Degree9Str, d 9) |> Ok
        | Degree8Prefix3 -> (SorterData.Degree8Prefix3Str, d 8) |> Ok
        | Degree10 -> (SorterData.Degree10Str, d 10) |> Ok
        | Degree11 -> (SorterData.Degree11Str, d 11) |> Ok
        | Degree12 -> (SorterData.Degree12Str, d 12) |> Ok
        | Degree13 -> (SorterData.Degree13Str, d 13) |> Ok
        | Degree14 -> (SorterData.Degree14Str, d 14) |> Ok
        | Degree15 -> (SorterData.Degree15Str, d 15) |> Ok
        | Degree16 -> (SorterData.Degree16Str, d 16) |> Ok
        | Green16 -> (SorterData.Degree16_Green, d 16) |> Ok
        | End16 -> (SorterData.Degree16_END, d 16) |> Ok
        | Green16m -> (SorterData.Degree16_GreenM, d 16) |> Ok
        | End16m -> (SorterData.Degree16_ENDM, d 16) |> Ok
        | Degree17 -> (SorterData.Degree17Str, d 17) |> Ok
        | Degree18 -> (SorterData.Degree18Str, d 18) |> Ok
        | Degree20 -> (SorterData.Degree20Str, d 20) |> Ok
        | Degree22 -> (SorterData.Degree22Str, d 22) |> Ok
        | Degree23 -> (SorterData.Degree23Str, d 23) |> Ok
        | Degree24 -> (SorterData.Degree24aStr, d 24) |> Ok
        | Degree25 -> (SorterData.Degree25Str, d 25) |> Ok
        | Degree26 -> (SorterData.Degree26Str, d 26) |> Ok
        | Degree28 -> (SorterData.Degree28Str, d 28) |> Ok
        | Degree32 -> (SorterData.Degree32Str, d 32) |> Ok
        | _ -> "no match found in GetStringAndDegree" |> Error


    let createRefSorter (sorterD:sorterId) (refSorter: RefSorter) =
        result {
            let! (sorterString, order) = (getStringAndDegree refSorter)
            return! SortersFromData.ParseToSorter sorterD sorterString order
        }


    let private _goodRefSorterForDegree (order: order) =
        let d = (Order.value order)

        match d with
        | 3 -> RefSorter.Degree3 |> Ok
        | 4 -> RefSorter.Degree4 |> Ok
        | 5 -> RefSorter.Degree5 |> Ok
        | 6 -> RefSorter.Degree6 |> Ok
        | 7 -> RefSorter.Degree7 |> Ok
        | 8 -> RefSorter.Degree8 |> Ok
        | 9 -> RefSorter.Degree9 |> Ok
        | 10 -> RefSorter.Degree10 |> Ok
        | 11 -> RefSorter.Degree11 |> Ok
        | 12 -> RefSorter.Degree12 |> Ok
        | 13 -> RefSorter.Degree13 |> Ok
        | 14 -> RefSorter.Degree14 |> Ok
        | 15 -> RefSorter.Degree15 |> Ok
        | 16 -> RefSorter.Degree16 |> Ok
        | 17 -> RefSorter.Degree17 |> Ok
        | 18 -> RefSorter.Degree18 |> Ok
        | 19 -> RefSorter.Degree19 |> Ok
        | 20 -> RefSorter.Degree20 |> Ok
        | 21 -> RefSorter.Degree21 |> Ok
        | 22 -> RefSorter.Degree22 |> Ok
        | 23 -> RefSorter.Degree23 |> Ok
        | 24 -> RefSorter.Degree24 |> Ok
        | 25 -> RefSorter.Degree25 |> Ok
        | 26 -> RefSorter.Degree26 |> Ok
        | 27 -> RefSorter.Degree27 |> Ok
        | 28 -> RefSorter.Degree28 |> Ok
        | 29 -> RefSorter.Degree29 |> Ok
        | 30 -> RefSorter.Degree30 |> Ok
        | 31 -> RefSorter.Degree31 |> Ok
        | 32 -> RefSorter.Degree32 |> Ok
        | _ -> "no match found in RefSorterForDegree" |> Error


    let goodRefSorterForOrder (sorterD:sorterId) (order: order) =
        result {
            let! refSorter = _goodRefSorterForDegree order
            return! createRefSorter sorterD refSorter
        }
