namespace Gort.DataConvert.Test
open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type sorterDtoFixture() =

    [<TestMethod>]
    member this.toDto() =
      let sorterId = Guid.NewGuid() |> SorterId.create
      let ordr = 64 |> Order.createNr
      let wPfx = Seq.empty<switch>
      let switchCt = 100 |> SwitchCount.create
      let rndGn () = 
        RngGen.lcgFromNow ()
      let sortr = Sorter.randomSwitches ordr wPfx switchCt rndGn sorterId 
      let cereal = sortr |> SorterDto.toJson
      let sortrBack = cereal |> SorterDto.fromJson |> Result.ExtractOrThrow
      Assert.IsTrue(CollectionProps.areEqual sortr sortrBack)
