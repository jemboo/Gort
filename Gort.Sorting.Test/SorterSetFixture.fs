namespace Gort.Sorting.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type SorterSetFixture() =

    [<TestMethod>]
    member this.trndSeqst() = 
      let sorterSetId = Guid.NewGuid() |> SorterSetId.create
      let ordr = 64 |> Order.createNr
      let wPfx = Seq.empty<switch>
      let switchCt = 100 |> SwitchCount.create
      let sorterCt = 10 |> SorterCount.create
      let rndGn = RngGen.lcgFromNow ()

      let sorterSt = SorterSet.createRandomSwitches 
                        sorterSetId sorterCt ordr wPfx switchCt rndGn

      let sorterSt2 = SorterSet.load 
                        (sorterSt |> SorterSet.getId)
                        (sorterSt |> SorterSet.getOrder)
                        (sorterSt |> SorterSet.getSorters)
        
      Assert.IsTrue(CollectionProps.areEqual sorterSt sorterSt2)
      Assert.AreEqual(sorterSt, sorterSt2)
