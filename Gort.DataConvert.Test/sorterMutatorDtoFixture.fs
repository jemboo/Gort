namespace Gort.DataConvert.Test
open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type sorterMutatorDtoFixture() =

    [<TestMethod>]
    member this.toDto() =
      let sorterMutator = 
            SorterUniformMutator.create
                None
                (1 |> SwitchCount.create |> Some)
                switchGenMode.StageSymmetric
                (0.15 |> MutationRate.create)
           |> sorterMutator.Uniform

      let smDto = sorterMutator |> SorterMutatorDto.toJson
      let sorterMutatorBackR = smDto |> SorterMutatorDto.fromJson
      let sorterMutatorBack = sorterMutatorBackR 
                                |> Result.ExtractOrThrow

      Assert.IsTrue(CollectionProps.areEqual
        (sorterMutator |> SorterMutator.getSwitchCountFinal )
        (sorterMutatorBack |> SorterMutator.getSwitchCountFinal )
        
       )
