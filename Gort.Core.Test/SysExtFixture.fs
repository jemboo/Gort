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



    //[<TestMethod>]
    //member this.TestMethodPassing () =
        
    //    let v = 5;
    //    let vv = v.count
    //    Assert.IsTrue(true);
