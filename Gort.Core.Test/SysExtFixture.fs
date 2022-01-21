namespace Gort.Core.Test

open System
open SysExt
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type SysExtFixture () =

    [<TestMethod>]
    member this.flip64 () =
        let v = 5uL;
        let fv = v.flip 3
        let ffv = fv.flip 3
        Assert.AreEqual(v, ffv);
        let sv = v.flip 61
        let fsv = sv.unset 61
        Assert.AreEqual(v, fsv);
        let tv = v.set 63
        let ftv = tv.flip 63
        Assert.AreEqual(v, ftv);
        let uv = v.unset 63
        Assert.AreEqual(v, uv);


    [<TestMethod>]
    member this.intAt () =
        let v = 5uL;
        let v0 = v.intAt 0
        let v1 = v.intAt 1
        let v2 = v.intAt 2
        let v3 = v.intAt 3

        Assert.AreEqual(v0, 1);
        Assert.AreEqual(v1, 0);
        Assert.AreEqual(v2, 1);
        Assert.AreEqual(v3, 0);


    //[<TestMethod>]
    //member this.TestMethodPassing () =
        
    //    let v = 5;
    //    let vv = v.count
    //    Assert.IsTrue(true);
