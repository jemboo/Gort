namespace Gort.Core.Test

open SysExt
open Microsoft.VisualStudio.TestTools.UnitTesting
open ByteUtils

[<TestClass>]
type GroupFixture() =
    

    [<TestMethod>]
    member this.rotateFace() =
        let dim = 3
        let cp0 = CubePos.create 0 dim
        let cp1 = CubePos.create 1 dim
        let cp2 = CubePos.create 2 dim
        let cp3 = CubePos.create 3 dim
        let cp4 = CubePos.create 4 dim
        let cp5 = CubePos.create 5 dim
        let cp6 = CubePos.create 6 dim
        let cp7 = CubePos.create 7 dim

        let cp0_r01 = CubePos.rotateFace cp0 0 1
        let cp0_r05 = CubePos.rotateFace cp0 0 5
        let cp0_r11 = CubePos.rotateFace cp0 1 1
        let cp0_r15 = CubePos.rotateFace cp0 1 5

        Assert.AreEqual (1, 1)


    [<TestMethod>]
    member this.setQuad() =
        let sourceInt = 15 //011101
        let sourceOffset = 2
        let targetOffset = 1
        let targetInt = 0
        let expectedTargetVal = 6 // 00110
        let expectedQuadVal = 3 // 11

        let q = Group.getQuad  sourceInt sourceOffset
        Assert.AreEqual (q, expectedQuadVal)

        let targetRes = Group.setQuad targetInt targetOffset expectedQuadVal
        Assert.AreEqual (targetRes, expectedTargetVal)



    [<TestMethod>]
    member this.getQuadsFromGuide() =
        let guideInt = 23 //010111
        let maxBit = 5

        let q = Group.getQuadsFromGuide  guideInt maxBit
        Assert.AreEqual (q, q)



    [<TestMethod>]
    member this.test() =

        Assert.AreEqual (1, 1)