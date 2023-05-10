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
      let ordr = 16 |> Order.createNr
      let wPfx = Seq.empty<switch>
      let swFreq = 1.0 |> SwitchFrequency.create
      let switchCt = 20 |> SwitchCount.create
      let baseSorterCt = 2 |> SorterCount.create
      let mutantSorterCt = 6 |> SorterCount.create
      let randy = Rando.create rngType.Lcg (123 |> RandomSeed.create)
      let rndGn () = 
        randy |> Rando.nextRngGen
      let mutationRate = MutationRate.create 0.15

      let sorterStBase = SorterSet.createRandomStages 
                                sorterSetId baseSorterCt swFreq ordr wPfx switchCt rndGn

      let sorterMutator = 
        SorterUniformMutator.create 
             None None switchGenMode.Stage mutationRate
        |> sorterMutator.Uniform

      let baseSorters = sorterStBase |> SorterSet.getSorters 
                        |> Seq.take(baseSorterCt |> SorterCount.value)
                        |> Seq.toArray

      let sorterSetOfMutants =
        SorterSet.createMutationSet 
                    baseSorters
                    mutantSorterCt
                    ordr
                    sorterMutator
                    sorterSetId
                    randy
 
      Assert.AreEqual(
        mutantSorterCt |> SorterCount.value, 
        sorterSetOfMutants |> SorterSet.getSorters |> Seq.length)


      let mutantSorterSetR = 
            MutantSorterSet.create
                sorterMutator
                randy
                mutantSorterCt
                baseSorters

      let mutantSorterSet = mutantSorterSetR |> Result.ExtractOrThrow

      Assert.AreEqual(1,1)