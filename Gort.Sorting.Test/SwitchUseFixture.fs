namespace Gort.Sorting.Test
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type SwitchUseFixture () =

    [<TestMethod>]
    member this.fromSortableIntsU8() =
        let expectedSitchCount = 3 |> SwitchCount.create
        let useCounts = [|111; 0; 112; 0; 0; 113|]
        let useFlags = [|111uL; 0uL; 112uL; 0uL; 0uL; 113uL|]

        let switchUseStandard = 
                useCounts 
                |> SwitchUseTrackStandard.apply
                |> switchUseTrack.Standard

        let switchUseBitStriped = 
                useFlags 
                |> SwitchUseTrackBitStriped.apply
                |> switchUseTrack.BitStriped

        let switchUseCountStandard = 
            switchUseStandard 
            |> SwitchUseTrack.getUsedSwitchCount

        let switchUseCountBitStriped = 
            switchUseBitStriped 
            |> SwitchUseTrack.getUsedSwitchCount

        Assert.AreEqual(expectedSitchCount, switchUseCountStandard);
        Assert.AreEqual(expectedSitchCount, switchUseCountBitStriped);
