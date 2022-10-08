namespace global

open SysExt

type switchUseTrackerStandard = private { useCounts: int[] }

module SwitchUseTrackerStandard =

    let getUseCounters (switchUses: switchUseTrackerStandard) = switchUses.useCounts

    let make (switchCount: switchCount) =
        { switchUseTrackerStandard.useCounts = Array.zeroCreate<int> (switchCount |> SwitchCount.value) }

    let apply (useCounts: int[]) = { useCounts = useCounts }

    let getUsedSwitchCount (switchUseTrackStandard: switchUseTrackerStandard) =
        switchUseTrackStandard.useCounts
        |> Seq.filter ((<) 0)
        |> Seq.length
        |> SwitchCount.create


type switchUseTrackerBitStriped = private { useFlags: uint64[] }

module SwitchUseTrackerBitStriped =

    let make (switchCount: switchCount) =
        { switchUseTrackerBitStriped.useFlags = Array.zeroCreate<uint64> (switchCount |> SwitchCount.value) }

    let apply (useFlags: uint64[]) = { useFlags = useFlags }

    let getUseFlags (switchUseTrackBitStriped: switchUseTrackerBitStriped) = switchUseTrackBitStriped.useFlags

    let getSwitchUseCounts (switchUseTrackBitStriped: switchUseTrackerBitStriped) =
        switchUseTrackBitStriped |> getUseFlags |> Array.map (fun l -> l.count |> int)

    let getUsedSwitchCount (switchUseTrackBitStriped: switchUseTrackerBitStriped) =
        switchUseTrackBitStriped.useFlags
        |> Seq.filter ((<) 0uL)
        |> Seq.length
        |> SwitchCount.create



type switchUseTracker =
    | Standard of switchUseTrackerStandard
    | BitStriped of switchUseTrackerBitStriped

module SwitchUseTracker =

    let init (switchOpMod: switchOpMode) (switchCount: switchCount) =
        match switchOpMod with
        | switchOpMode.Standard -> switchCount |> SwitchUseTrackerStandard.make |> switchUseTracker.Standard
        | switchOpMode.BitStriped -> SwitchUseTrackerBitStriped.make switchCount |> switchUseTracker.BitStriped


    let getUseFlags (switchUses: switchUseTrackerBitStriped) = switchUses.useFlags


    let getSwitchUseCounts (switchUs: switchUseTracker) =
        match switchUs with
        | switchUseTracker.Standard switchUseTrackerStandard ->
            switchUseTrackerStandard |> SwitchUseTrackerStandard.getUseCounters
        | switchUseTracker.BitStriped switchUseTrackerBitStriped ->
            switchUseTrackerBitStriped |> SwitchUseTrackerBitStriped.getSwitchUseCounts


    let toSwitchOpMode (rollout: rollout) =
        match rollout with
        | B _uBRoll -> switchOpMode.Standard
        | U8 _uInt8Roll -> switchOpMode.Standard
        | U16 _uInt16Roll -> switchOpMode.Standard
        | I32 _intRoll -> switchOpMode.Standard
        | U64 _uInt64Roll -> switchOpMode.Standard
        | Bs64 _bs64Roll -> switchOpMode.BitStriped
