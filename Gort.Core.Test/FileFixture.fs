namespace Gort.Core.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open MathNet.Numerics.LinearAlgebra
open MathNet.Numerics.Distributions

[<TestClass>]
type FileFixture() =

    [<TestMethod>]
    let m = DenseMatrix.randomStandard 50 50
    member this.TestMethodPassing() = Assert.IsTrue(true)
