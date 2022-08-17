namespace Gort.DataConvert.Test
open Microsoft.VisualStudio.TestTools.UnitTesting
open DomainTables

[<TestClass>]
type sorterSetDtoFixture () =

    [<TestMethod>]
    member this.sb() =
        let quaMain = [|1;2;3|]
        let quaAppend = [|4;5;6|]
        let jam = quaAppend |> Array.append quaMain
        Assert.IsTrue(true)


