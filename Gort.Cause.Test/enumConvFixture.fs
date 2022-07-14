namespace Gort.Cause.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open Gort.Data.DataModel

[<TestClass>]
type miscConvFixture () =

    let causeId = Guid.Parse("fd100b52-f74e-8930-3cef-bc1f657a82a5");
    let rndGenId = Guid.Parse("58a91a3c-9b8e-4055-0bba-a2ff8a8a977c");
    let texto = new Gort.Data.DataModel.GortContext()

    [<TestMethod>]
    member this.Test1 () =
        Assert.IsTrue(true);


    [<TestMethod>]
    member this.Test () =

        Assert.IsTrue(true);
