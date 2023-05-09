namespace Gort.Sorting.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type SorterMutatorFixture() =

    [<TestMethod>]
    member this.makeMutants() = 
      let pfxSorterId = Guid.NewGuid() |> SorterId.create
      let sfxSorterId = Guid.NewGuid() |> SorterId.create
      let crossSorterId = Guid.NewGuid() |> SorterId.create
      let pfxSwitches = [|1;2;3;4;5;6;7;8;9;10;11;12|] 
                                  |> Switch.fromSwitchIndexes
                                  |> Seq.toList
      let sfxSwitches = [|20;21;22;23;24;25;26;27;28;29;30|] 
                                  |> Switch.fromSwitchIndexes
                                  |> Seq.toList
      let ordr = 16 |> Order.createNr

      let sorterPfx = Sorter.fromSwitches pfxSorterId ordr pfxSwitches
      let sorterSfx = Sorter.fromSwitches sfxSorterId ordr sfxSwitches
      
      let pfxLen = 4 |> SwitchCount.create
      let finalLen = 11 |> SwitchCount.create
      let randy = Rando.create rngType.Lcg (123 |> RandomSeed.create)

      let crossedSrtr = Sorter.crossOver pfxLen finalLen sorterPfx sorterSfx crossSorterId randy
      let crossedSeq = crossedSrtr 
                        |> Sorter.getSwitches 
                        |> Switch.toIndexes
                        |> Seq.toList
      let expectedSeq = [1;2;3;4;24;25;26;27;28;29;30]


      Assert.AreEqual(1, 1)
