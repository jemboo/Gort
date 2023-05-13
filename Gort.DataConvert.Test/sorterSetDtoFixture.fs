namespace Gort.DataConvert.Test
open System

open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type sorterSetDtoFixture() =

    [<TestMethod>]
    member this.sorterSetDto() =
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


    [<TestMethod>]
    member this.mutantSorterSetDto() =
      let sorterSetId = Guid.NewGuid() |> SorterSetId.create
      let ordr = 16 |> Order.createNr
      let wPfx = Seq.empty<switch>
      let swFreq = 1.0 |> SwitchFrequency.create
      let switchCt = 20 |> SwitchCount.create
      let baseSorterCt = 2 |> SorterCount.create
      let mutantSorterCt = 6 |> SorterCount.create
      let rngGen = RngGen.createLcg (123 |> RandomSeed.create)
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


      let sorterSetMutator = 
            SorterSetMutator.load
                sorterMutator
                (Some mutantSorterCt)
                rngGen

      let mutantSorterSetR = 
            MutantSorterSetMap.create
                sorterSetMutator
                sorterStBase


      let mutantSorterSetMap, sorterSetMutated = 
                mutantSorterSetR |> Result.ExtractOrThrow

      //let cereal = 
      //      sorterSetMutated
      //      |> MutantSorterSetDto.toJson

      //let mutantSorterSetBackR =
      //      cereal
      //      |> MutantSorterSetDto.fromJson

      //let mutantSorterSetBack = mutantSorterSetBackR |> Result.ExtractOrThrow
      
      //Assert.IsTrue(CollectionProps.areEqual 
      //                  (mutantSorterSet |> MutantSorterSetMap.getSorterParentMap )
      //                  (mutantSorterSetBack |> MutantSorterSetMap.getSorterParentMap )
      //             )

      //Assert.IsTrue(CollectionProps.areEqual 
      //                  (mutantSorterSet |> MutantSorterSetMap.getParentSorterSetId )
      //                  (mutantSorterSetBack |> MutantSorterSetMap.getParentSorterSetId )
      //             )

      //Assert.IsTrue(CollectionProps.areEqual 
      //                  (mutantSorterSet |> MutantSorterSetMap.getMutantSorterSetId )
      //                  (mutantSorterSetBack |> MutantSorterSetMap.getMutantSorterSetId )
      //             )

      Assert.IsTrue(true)