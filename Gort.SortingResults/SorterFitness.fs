namespace global

type stageWeight = private StageWeight of float
module StageWeight =
    let value (StageWeight v) = v
    let create vl = StageWeight vl


// larger fitness numbers are better
type sorterFitness = private SorterFitness of float
module SorterFitness =
    let value (SorterFitness v) = v
    let create (spbCt: float) =
        spbCt |> SorterFitness

    let fromSpeed (stageWght:stageWeight) (sorterSpd:sorterSpeed) = 
        (stageWght |> StageWeight.value) /
        (sorterSpd |> SorterSpeed.getStageCount |> StageCount.value |> float)
        +
        1.0 /
        (sorterSpd |> SorterSpeed.getSwitchCount |> SwitchCount.value |> float)


type selectionFraction = private SelectionFraction of float
module SelectionFraction =
    let value (SelectionFraction v) = v
    let create vl = SelectionFraction vl


