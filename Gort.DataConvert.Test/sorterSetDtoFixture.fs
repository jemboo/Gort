namespace Gort.DataConvert.Test
open System

open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type sorterSetDtoFixture() =

    [<TestMethod>]
    member this.toDto() =
      let sorterSetId = Guid.NewGuid() |> SorterSetId.create
      let ordr = 64 |> Order.createNr
      let wPfx = Seq.empty<switch>
      let switchCt = 100 |> SwitchCount.create
      let sorterCt = 10 |> SorterCount.create
      let randy = Rando.create rngType.Lcg (123 |> RandomSeed.create)
      let rndGn () = 
            randy |> Rando.nextRngGen


      let sorterSt = SorterSet.createRandomSwitches 
                        sorterSetId sorterCt ordr wPfx switchCt rndGn

      let cereal = sorterSt |> SorterSetDto.toJson
      let sorterSetBckR = cereal |> SorterSetDto.fromJson
      let sorterSetBck = sorterSetBckR |> Result.ExtractOrThrow

      Assert.IsTrue(CollectionProps.areEqual sorterSt sorterSetBck)
      Assert.IsTrue(true)
