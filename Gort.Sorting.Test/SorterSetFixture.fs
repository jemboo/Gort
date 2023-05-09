namespace Gort.Sorting.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type SorterSetFixture() =

    [<TestMethod>]
    member this.makeSorterSet() = 
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

      let sorterSt2 = SorterSet.load 
                        (sorterSt |> SorterSet.getId)
                        (sorterSt |> SorterSet.getOrder)
                        (sorterSt |> SorterSet.getSorters)
        
      Assert.IsTrue(CollectionProps.areEqual sorterSt sorterSt2)
      Assert.AreEqual(sorterSt, sorterSt2)


    [<TestMethod>]
    member this.createMutationSet() = 
      let sorterSetId = Guid.NewGuid() |> SorterSetId.create
      let ordr = 64 |> Order.createNr
      let wPfx = Seq.empty<switch>
      let switchCt = 100 |> SwitchCount.create
      let baseSorterCt = 3 |> SorterCount.create
      let sorterCt = 20 |> SorterCount.create
      let randy = Rando.create rngType.Lcg (123 |> RandomSeed.create)
      let rndGn () = 
        randy |> Rando.nextRngGen
      let mutationRate = MutationRate.create 0.5

      let sorterStBase = SorterSet.createRandomSwitches 
                                sorterSetId baseSorterCt ordr wPfx switchCt rndGn

      let sorterMutator = SorterUniformMutator.create 
                            None None switchGenMode.Switch mutationRate

      let baseSorters = sorterStBase |> SorterSet.getSorters 
                        |> Seq.take(baseSorterCt |> SorterCount.value)
                        |> Seq.toArray

      let mutants = SorterSet.createMutationSet 
                        baseSorters sorterCt ordr sorterMutator sorterSetId randy

       
      Assert.AreEqual(sorterCt |> SorterCount.value, mutants |> SorterSet.getSorters |> Seq.length)