namespace Gort.Core.Test

open System
open SysExt
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type SysExtFixture() =

    [<TestMethod>]
    member this.flip64() =
        let v = 5uL
        let fv = v.flip 3
        let ffv = fv.flip 3
        Assert.AreEqual(v, ffv)
        let sv = v.flip 61
        let fsv = sv.unset 61
        Assert.AreEqual(v, fsv)
        let tv = v.set 63
        let ftv = tv.flip 63
        Assert.AreEqual(v, ftv)
        let uv = v.unset 63
        Assert.AreEqual(v, uv)


    [<TestMethod>]
    member this.count64() =
        let v1 = 1uL
        let vv1 = v1.count
        let v2 = 1uL + 4uL
        let vv2 = v2.count
        let v3 = 1uL + 4uL + 16uL
        let vv3 = v3.count
        let v3 = 1uL + 4uL + 16uL + 4096uL
        let vv3 = v3.count
        let v3n = ~~~v3
        let vv3n = v3n.count
        Assert.AreEqual(vv3, 4uL)
        Assert.AreEqual(vv3n, 60uL)


    [<TestMethod>]
    member this.count64b() =
        let bs25 =
            [| 1
               0
               1
               1
               0
               1
               0
               1
               1
               0
               1
               0
               1
               1
               0
               1
               0
               1
               1
               0
               1
               0
               1
               1
               0
               1
               0
               1
               1
               0
               1
               0
               1
               1
               0
               1
               0
               1
               1
               0
               1 |]

        let uint64_25 = 1 |> ByteUtils.thresholdArrayToUint64 bs25
        let onesCt = uint64_25.count
        Assert.AreEqual(onesCt, 25uL)

    [<TestMethod>]
    member this.chunky64b() =
        let bs25 =
            [| 1
               0
               1
               1
               0
               1
               0
               1
               1
               0
               1
               0
               1
               1
               0
               1
               0
               1
               1
               0
               1
               0
               1
               1
               0
               1
               0
               1
               1
               0
               1
               0
               1
               1
               0
               1
               0
               1
               1
               0
               1 |]

        let yy = 196uy
        let len = bs25 |> Seq.chunkBySize (3) |> Seq.toList
        Assert.AreEqual(1, 1)
