namespace global

open System


type sorterId = private SorterId of Guid
type sorterParentId = private SorterParentId of Guid
type sortableCount = private SortableCount of int
type sortableSetId = private SortableSetId of Guid
type sortableSetCount = private SortableSetCount of int
type setOfSortableSetId = private SetOfSortableSetId of Guid
type sorterCount = private SorterCount of int
type sorterSetConcatMapId = private SorterSetConcatMapId of Guid
type sorterSetParentMapId = private SorterSetParentMapId of Guid
type sorterSetId = private SorterSetId of Guid
type stageCount = private StageCount of int
type stageWindowSize = private StageWindowSize of int
type switchCount = private SwitchCount of int
type switchFrequency = private SwitchFrequency of float

module SortableCount =
    let value (SortableCount v) = v
    let create v = SortableCount v

    let repStr v =
        match v with
        | Some r -> sprintf "%d" (value r)
        | None -> ""

    let makeSeq sc = seq { 0 .. (value sc - 1) }


module SorterId =
    let value (SorterId v) = v
    let create v = SorterId v


module SorterParentId =
    let value (SorterParentId v) = v
    let create v = SorterParentId v
    let toSorterId (SorterParentId v) = 
        v |> SorterId.create
    let toSorterParentId (SorterId v) = 
        v |> create


module SorterSetId =
    let value (SorterSetId v) = v
    let create id = SorterSetId id

module SorterSetParentMapId =
    let value (SorterSetParentMapId v) = v
    let create id = SorterSetParentMapId id

module SorterSetConcatMapId =
    let value (SorterSetConcatMapId v) = v
    let create id = SorterSetConcatMapId id

module SorterCount =
    let value (SorterCount v) = v
    let create v = SorterCount v
    let add (a: sorterCount) (b: sorterCount) = create ((value a) + (value b))
    let multiply (a: int) (b: sorterCount) = create (a * (value b))


module SortableSetId =
    let value (SortableSetId v) = v
    let create id = (SortableSetId id)

module SortableSetCount =
    let value (SortableSetCount v) = v
    let create id = (SortableSetCount id)

module SetOfSortableSetId =
    let value (SetOfSortableSetId v) = v
    let create id = (SetOfSortableSetId id)

module SwitchCount =
    let value (SwitchCount v) = v
    let create id = SwitchCount id
    let add (scA:switchCount) (scB:switchCount) =
        (value scA) + (value scB) |> create

    let orderToRecordSwitchCount (ord: order) =
        let d = (Order.value ord)

        let ct =
            match d with
            | 4 -> 5
            | 5 -> 9
            | 6 -> 12
            | 7 -> 16
            | 8 -> 19
            | 9 -> 25
            | 10 -> 29
            | 11 -> 35
            | 12 -> 39
            | 13 -> 45
            | 14 -> 51
            | 15 -> 56
            | 16 -> 60
            | 17 -> 71
            | 18 -> 77
            | 19 -> 85
            | 20 -> 91
            | 21 -> 100
            | 22 -> 107
            | 23 -> 115
            | 24 -> 120
            | 25 -> 132
            | 26 -> 139
            | 27 -> 150
            | 28 -> 155
            | 29 -> 165
            | 30 -> 172
            | 31 -> 180
            | 32 -> 65
            | 64 -> 100
            | _ -> 0

        create ct

    let orderTo900SwitchCount (ord: order) =
        let d = (Order.value ord)

        let ct =
            match d with
            | 6 -> 10
            | 7 -> 100
            | 8
            | 9 -> 160
            | 10
            | 11 -> 300
            | 12
            | 13 -> 400
            | 14
            | 15 -> 500
            | 16
            | 17 -> 800
            | 18
            | 19 -> 1000
            | 20
            | 21 -> 1300
            | 22
            | 23 -> 1600
            | 24
            | 25 -> 1900
            | _ -> 0

        create ct

    let orderTo999SwitchCount (ord: order) =
        let d = (Order.value ord)

        let ct =
            match d with
            | 6
            | 7 -> 600
            | 8
            | 9 -> 700
            | 10
            | 11 -> 800
            | 12
            | 13 -> 1000
            | 14
            | 15 -> 1200
            | 16
            | 17 -> 1600
            | 18
            | 19 -> 2000
            | 20
            | 21 -> 2200
            | 22
            | 23 -> 2600
            | 24
            | 25 -> 3000
            | _ -> 0

        create ct

module SwitchFrequency =
    let value (SwitchFrequency v) = v
    let create vv = SwitchFrequency vv
    let max = create 1.0

module StageCount =
    let value (StageCount v) = v
    let create id = StageCount id
    let add (scA:stageCount) (scB:stageCount) =
        (value scA) + (value scB) |> create

    let toSwitchCount (ord: order) (tCt: stageCount) =
        SwitchCount.create ((Order.value ord) * (value tCt) / 2)

    let orderToRecordStageCount (ord: order) =
        let d = (Order.value ord)

        let ct =
            match d with
            | 4 -> 3
            | 5
            | 6 -> 5
            | 7
            | 8 -> 6
            | 9
            | 10 -> 7
            | 11
            | 12 -> 8
            | 13
            | 14
            | 15
            | 16 -> 9
            | 17 -> 10
            | 18
            | 19
            | 20 -> 11
            | 21
            | 22
            | 23
            | 24 -> 12
            | 25
            | 26 -> 13
            | 27
            | 28
            | 29
            | 30
            | 31 -> 14
            | 32 -> 5
            | 64 -> 10
            | _ -> 0

        create ct

    let orderTo999StageCount (ord: order) =
        let d = (Order.value ord)

        let ct =
            match d with
            | 8
            | 9 -> 140
            | 10
            | 11
            | 12
            | 13
            | 14
            | 15 -> 160
            | 16
            | 17
            | 18
            | 19
            | 20
            | 21 -> 220
            | 22
            | 23
            | 24
            | 25 -> 240
            | 32 -> 600
            | _ -> 0

        create ct

    let orderTo900StageCount (ord: order) =
        let d = (Order.value ord)

        let ct =
            match d with
            | 8
            | 9 -> 35
            | 10
            | 11 -> 50
            | 12
            | 13 -> 60
            | 14
            | 15 -> 65
            | 16
            | 17 -> 95
            | 18
            | 19 -> 110
            | 20
            | 21 -> 120
            | 22
            | 23 -> 130
            | 24
            | 25 -> 140
            | _ -> 0

        create ct

module StageWindowSize =
    let value (StageWindowSize v) = v
    let create v = StageWindowSize v

    let ToSwitchCount (ord: order) (tWz: stageWindowSize) =
        SwitchCount.create ((Order.value ord) * (value tWz) / 2)

    let fromInt v = create v
