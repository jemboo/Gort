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

      let baseSorters = 
            sorterStBase 
                |> SorterSet.getSorters 
                |> Seq.take(baseSorterCt |> SorterCount.value)
                |> Seq.toArray

      let mutantSorterSetR = 
            MutantSorterSet.create
                sorterMutator
                randy
                mutantSorterCt
                baseSorters

      let mutantSorterSet = mutantSorterSetR |> Result.ExtractOrThrow

      let cereal = 
            mutantSorterSet
            |> MutantSorterSetDto.toJson

      let mutantSorterSetBackR =
            cereal
            |> MutantSorterSetDto.fromJson

      let mutantSorterSetBack = mutantSorterSetBackR |> Result.ExtractOrThrow

      
      Assert.IsTrue(CollectionProps.areEqual 
                        (mutantSorterSet |> MutantSorterSet.getParentMap )
                        (mutantSorterSetBack |> MutantSorterSet.getParentMap )
                   )

      Assert.IsTrue(CollectionProps.areEqual 
                        (mutantSorterSet |> MutantSorterSet.getSorterSet )
                        (mutantSorterSetBack |> MutantSorterSet.getSorterSet )
                   )



