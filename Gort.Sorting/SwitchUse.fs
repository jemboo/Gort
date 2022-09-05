namespace global
open SysExt

type switchUseCounts = private { useCounts:int[] }
module SwitchUseCounts =
    let make (switchCount:switchCount) =
        { switchUseCounts.useCounts = 
                Array.zeroCreate (switchCount |> SwitchCount.value)}

    let apply (useCounts:int[]) =
        { switchUseCounts.useCounts = useCounts}

    let getUseCounts (switchUseCounts:switchUseCounts) =
        switchUseCounts.useCounts

    let getSwitchCount (switchUseCounts:switchUseCounts) =
        switchUseCounts.useCounts.Length |> SwitchCount.create


type switchUseTrackStandard = private { useCounts:int[] }
module SwitchUseTrackStandard =

   let getUseCounts (switchUses:switchUseTrackStandard) =
        switchUses.useCounts

   let make (switchCount:switchCount) =
       { switchUseTrackStandard.useCounts = 
              Array.zeroCreate<int> (switchCount |> SwitchCount.value)}

   let apply (useCounts:int[]) =
        { useCounts = useCounts}

   let getUsedSwitchCount (switchUseTrackStandard:switchUseTrackStandard) =
       switchUseTrackStandard.useCounts
        |> Seq.filter((<) 0)
        |> Seq.length
        |> SwitchCount.create


type switchUseTrackBitStriped = private { useFlags:uint64[] }
module SwitchUseTrackBitStriped =

   let make (switchCount:switchCount) =
       { switchUseTrackBitStriped.useFlags = 
              Array.zeroCreate<uint64> (switchCount |> SwitchCount.value)}

   let apply (useFlags:uint64[]) =
        { useFlags = useFlags }

   let getUseFlags (switchUseTrackBitStriped:switchUseTrackBitStriped) =
        switchUseTrackBitStriped.useFlags

   let getUsedSwitchCount (switchUseTrackBitStriped:switchUseTrackBitStriped) =
       switchUseTrackBitStriped.useFlags
            |> Seq.filter((<) 0uL)
            |> Seq.length
            |> SwitchCount.create


type switchOpMode = | Standard | BitStriped
type switchUseTrack =
    | Standard of switchUseTrackStandard
    | BitStriped of switchUseTrackBitStriped

module SwitchUseTrack =

   let init (switchOpMod:switchOpMode) (switchCount:switchCount) =
       match switchOpMod with
       | switchOpMode.Standard ->
            switchCount
            |> SwitchUseTrackStandard.make
            |> switchUseTrack.Standard
       | switchOpMode.BitStriped ->
            SwitchUseTrackBitStriped.make switchCount
            |> switchUseTrack.BitStriped

   let getUseFlags (switchUses:switchUseTrackBitStriped) =
        switchUses.useFlags

   let getSwitchUseCounts (switchUs:switchUseTrack) =
       match switchUs with
       | switchUseTrack.Standard useCounts -> 
            useCounts 
            |> SwitchUseTrackStandard.getUseCounts
            |> SwitchUseCounts.apply
       | switchUseTrack.BitStriped useFlags -> 
            useFlags 
            |> SwitchUseTrackBitStriped.getUseFlags
            |> Array.map(fun u64 -> u64.count |> int)
            |> SwitchUseCounts.apply

   let getUsedSwitchCount (switchUs:switchUseTrack) =
       match switchUs with
       | switchUseTrack.Standard useCounts -> 
            useCounts
            |> SwitchUseTrackStandard.getUsedSwitchCount
       | switchUseTrack.BitStriped useFlags -> 
            useFlags 
            |> SwitchUseTrackBitStriped.getUsedSwitchCount


   let toSwitchOpMode (rollout:rollout) =
        match rollout with
        | B _uBRoll -> switchOpMode.Standard
        | U8 _uInt8Roll -> switchOpMode.Standard
        | U16 _uInt16Roll -> switchOpMode.Standard
        | I32 _intRoll -> switchOpMode.Standard
        | U64 _uInt64Roll -> switchOpMode.Standard
        | Bs64 _bs64Roll -> switchOpMode.BitStriped

