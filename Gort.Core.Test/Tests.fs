namespace Gort.Core.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open MathNet.Numerics.LinearAlgebra
open MathNet.Numerics.Distributions

[<TestClass>]
type TestClass() =


    [<TestMethod>]
    member this.product() =
        let m = DenseMatrix.randomStandard<float> 50 50
        Assert.AreEqual(1, 1)

