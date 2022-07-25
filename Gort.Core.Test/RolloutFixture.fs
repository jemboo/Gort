namespace Gort.Core.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type RolloutFixture () =

    [<TestMethod>]
    member this.getUint8s () =
        let byteWdth = (ByteWidth.create 1) |> Result.ExtractOrThrow
        let order = (Order.create 4)  |> Result.ExtractOrThrow
        let rollCt = (ChunkCount.create 7) |> Result.ExtractOrThrow
        let testIndex = 2
        let rollt = Rollout.create byteWdth rollCt order
        let tstData = [|0uy; 1uy; 2uy; 3uy|]
        let outData = Array.zeroCreate<uint8> 4
        rollt |> Rollout.setUint8s testIndex tstData
        rollt |> Rollout.getUint8s testIndex outData
        
        Assert.AreEqual(tstData |> Array.toList, outData |> Array.toList);


    [<TestMethod>]
    member this.getUint16s () =
        let byteWdth =  (ByteWidth.create 2) |> Result.ExtractOrThrow
        let order = (Order.create 4)  |> Result.ExtractOrThrow
        let rollCt = (ChunkCount.create 7) |> Result.ExtractOrThrow
        let testIndex = 2
        let rollt = Rollout.create byteWdth rollCt order
        let tstData = [|0us; 1us; 2us; 3us|]
        let outData = Array.zeroCreate<uint16> 4
        rollt |> Rollout.setUint16s testIndex tstData
        rollt |> Rollout.getUint16s testIndex outData
        
        Assert.AreEqual(tstData |> Array.toList, outData |> Array.toList);


    [<TestMethod>]
    member this.getUint32s () =
        let byteWdth =  (ByteWidth.create 4) |> Result.ExtractOrThrow
        let order = (Order.create 4)  |> Result.ExtractOrThrow
        let rollCt = (ChunkCount.create 7) |> Result.ExtractOrThrow
        let testIndex = 2
        let rollt = Rollout.create byteWdth rollCt order
        let tstData = [|0ul; 1ul; 2ul; 3ul|]
        let outData = Array.zeroCreate<uint32> 4
        rollt |> Rollout.setUint32s testIndex tstData
        rollt |> Rollout.getUint32s testIndex outData
        
        Assert.AreEqual(tstData |> Array.toList, outData |> Array.toList);


    [<TestMethod>]
    member this.getUint64s () =
        let byteWdth =  (ByteWidth.create 8) |> Result.ExtractOrThrow
        let order = (Order.create 4)  |> Result.ExtractOrThrow
        let rollCt = (ChunkCount.create 7) |> Result.ExtractOrThrow
        let testIndex = 2
        let rollt = Rollout.create byteWdth rollCt order
        let tstData = [|0uL; 1uL; 2uL; 3uL|]
        let outData = Array.zeroCreate<uint64> 4
        rollt |> Rollout.setUint64s testIndex tstData
        rollt |> Rollout.getUint64s testIndex outData

        Assert.AreEqual(tstData |> Array.toList, outData |> Array.toList);


    [<TestMethod>]
    member this.TestMethodPassing () =

        Assert.IsTrue(true);