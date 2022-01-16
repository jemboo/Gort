namespace global
open System

// Sorter
type sorterId = private SorterId of Guid
type sortableCount = private SortableCount of int
type sortableSetId = private SortableSetId of Guid
type sorterCount = private SorterCount of int
type sorterSetId = private SorterSetId of Guid
type stageCount = private StageCount of int
type stageWindowSize = private StageWindowSize of int
type switchCount = private SwitchCount of int
type switchUses = {weights:int[]}


module SortableCount =
    let value (SortableCount v) = v
    let create v = SortableCount v
    let repStr v = match v with
                          | Some r -> sprintf "%d" (value r)
                          | None -> ""

module SorterId =
    let value (SorterId v) = v
    let create v = SorterId v

module SorterSetId =
    let value (SorterSetId v) = v
    let create id = SorterSetId id

module SorterCount =
    let value (SorterCount v) = v
    let create v = SorterCount v
    let add (a:sorterCount) (b:sorterCount) =
        create ((value a) + (value b))

module SortableSetId =
    let value (SortableSetId v) = v
    let create id = (SortableSetId id)
    let fromGuid (id:Guid) = create id


module SwitchCount =
    let value (SwitchCount v) = v
    let create id = SwitchCount id

    let degreeToRecordSwitchCount (dg:degree) =
        let d = (Degree.value dg)
        let ct = match d with
                    | 4 -> 5    | 5 -> 9    | 6 -> 12
                    | 7 -> 16   | 8 -> 19   | 9 -> 25
                    | 10 -> 29  | 11 -> 35  | 12 -> 39
                    | 13 -> 45  | 14 -> 51  | 15 -> 56
                    | 16 -> 60  | 17 -> 71  | 18 -> 77
                    | 19 -> 85  | 20 -> 91  | 21 -> 100
                    | 22 -> 107 | 23 -> 115 | 24 -> 120
                    | 25 -> 132 | 26 -> 139 | 27 -> 150
                    | 28 -> 155 | 29 -> 165 | 30 -> 172
                    | 31 -> 180 | 32 -> 65 | 64 -> 100
                    | _ -> 0
        create ct


    let degreeTo900SwitchCount (dg:degree) =
        let d = (Degree.value dg)
        let ct = match d with
                    | 6  | 7 -> 100     | 8  | 9 -> 160
                    | 10 | 11 -> 300    | 12 | 13 -> 400
                    | 14 | 15 -> 500    | 16 | 17 -> 800
                    | 18 | 19 -> 1000   | 20 | 21 -> 1300
                    | 22 | 23 -> 1600   | 24 | 25 -> 1900
                    | _ -> 0
        create ct


    let degreeTo999SwitchCount (dg:degree) =
        let d = (Degree.value dg)
        let ct = match d with
                    | 6  | 7 -> 600    | 8  | 9 -> 700
                    | 10 | 11 -> 800   | 12 | 13 -> 1000
                    | 14 | 15 -> 1200  | 16 | 17 -> 1600
                    | 18 | 19 -> 2000  | 20 | 21 -> 2200
                    | 22 | 23 -> 2600  | 24 | 25 -> 3000
                    | _ -> 0
        create ct


module StageCount =
    let value (StageCount v) = v
    let create id = StageCount id
    let toSwitchCount (dg:degree) (tCt:stageCount) =
        SwitchCount.create ((Degree.value dg) * (value tCt) / 2)


    let degreeToRecordStageCount (dg:degree) =
        let d = (Degree.value dg)
        let ct = match d with
                    | 4 ->  3
                    | 5 | 6 ->  5
                    | 7 | 8 ->  6
                    | 9 | 10 -> 7
                    | 11 | 12 -> 8
                    | 13 | 14 | 15 | 16 -> 9
                    | 17 -> 10
                    | 18 | 19 | 20 -> 11
                    | 21 | 22 | 23 | 24 -> 12
                    | 25 | 26 -> 13
                    | 27 | 28 | 29 | 30 | 31 -> 14
                    | 32 -> 5
                    | 64 -> 10
                    | _ -> 0
        create ct

    let degreeTo999StageCount (dg:degree) =
        let d = (Degree.value dg)
        let ct = match d with
                    | 8 | 9 -> 140
                    | 10 | 11 | 12 | 13 | 14 | 15 -> 160
                    | 16 | 17 | 18 | 19 | 20 | 21 -> 220
                    | 22 | 23 | 24 | 25 -> 240
                    | 32 -> 600
                    | _ -> 0
        create ct

    let degreeTo900StageCount (dg:degree) =
        let d = (Degree.value dg)
        let ct = match d with
                    | 8 | 9 -> 35
                    | 10 | 11 -> 50
                    | 12 | 13 -> 60
                    | 14 | 15 -> 65
                    | 16 | 17 -> 95
                    | 18 | 19 -> 110
                    | 20 | 21 -> 120
                    | 22 | 23 -> 130
                    | 24 | 25 -> 140
                    | _ -> 0
        create ct



module StageWindowSize =
    let value (StageWindowSize v) = v
    let create v = StageWindowSize v
    let ToSwitchCount (dg:degree) (tWz:stageWindowSize) =
        SwitchCount.create ((Degree.value dg) * (value tWz) / 2)
    let fromInt v = create v
