namespace Gort.DataConvert.Test
open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open DomainTables

[<TestClass>]
type sorterDtoFixture() =


    [<TestMethod>]
    member this.toDto() =
      let sorterId = Guid.NewGuid() |> SorterId.create
      let ordr = 16 |> Order.createNr
      let wPfx = Seq.empty<switch>
      let switchCt = 10 |> SwitchCount.create
      let rndGn = RngGen.lcgFromNow () |> Rando.fromRngGen
      let sortr = Sorter.randomSwitches sorterId ordr wPfx switchCt rndGn
      let cereal = sortr |> SorterDto.toJson
      let sortrBack = cereal |> SorterDto.fromJson |> Result.ExtractOrThrow
      Assert.IsTrue(CollectionProps.areEqual sortr sortrBack)
