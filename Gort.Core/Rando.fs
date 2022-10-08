namespace global

open System

// Rando
type randomSeed = private RandomSeed of int

type rngType =
    | Lcg = 1
    | Net = 2

type rngGen = { rngType: rngType; seed: randomSeed }

type IRando =
    abstract member Count: int
    abstract member Seed: randomSeed
    abstract member NextUInt: uint32
    abstract member NextPositiveInt: int32
    abstract member NextULong: uint64
    abstract member NextFloat: float
    abstract member rngType: rngType

module RandomSeed =
    let value (RandomSeed seed) = seed

    let create (seed: int) =
        (Math.Abs(seed) % 2147483647) |> RandomSeed

    let fromNow () = DateTime.Now.Ticks |> int |> create


module RngGen =
    let createLcg (seed: randomSeed) = { rngType = rngType.Lcg; seed = seed }
    let lcgFromNow () = RandomSeed.fromNow () |> createLcg
    let createNet (seed: randomSeed) = { rngType = rngType.Net; seed = seed }


type RandomNet(seed: randomSeed) =
    let mutable _count = 0
    let rnd = new System.Random(RandomSeed.value seed)

    interface IRando with
        member this.Seed = seed
        member this.Count = _count

        member this.NextUInt =
            _count <- _count + 2
            let vv = (uint32 (rnd.Next()))
            vv + (uint32 (rnd.Next()))

        member this.NextPositiveInt = rnd.Next()

        member this.NextULong =
            let r = this :> IRando
            let vv = (uint64 r.NextUInt)
            (vv <<< 32) + (uint64 r.NextUInt)

        member this.NextFloat =
            _count <- _count + 1
            rnd.NextDouble()

        member this.rngType = rngType.Net


type RandomLcg(seed: randomSeed) =
    let _a = 6364136223846793005UL
    let _c = 1442695040888963407UL
    let mutable _last = (_a * (uint64 (RandomSeed.value seed)) + _c)
    let mutable _count = 0
    member this.Seed = seed
    member this.Count = _count

    member this.NextUInt =
        _count <- _count + 1
        _last <- ((_a * _last) + _c)
        (uint32 (_last >>> 32))

    member this.NextULong =
        let mm = ((_a * _last) + _c)
        _last <- ((_a * mm) + _c)
        _count <- _count + 2
        _last + (mm >>> 32)

    member this.NextFloat =
        (float this.NextUInt) / (float Microsoft.FSharp.Core.uint32.MaxValue)

    interface IRando with
        member this.Seed = this.Seed
        member this.Count = _count
        member this.NextUInt = this.NextUInt
        member this.NextPositiveInt = int (this.NextUInt >>> 1)
        member this.NextULong = this.NextULong
        member this.NextFloat = this.NextFloat
        member this.rngType = rngType.Lcg


module Rando =

    let create rngtype seed =
        match rngtype with
        | rngType.Lcg -> new RandomLcg(seed) :> IRando
        | rngType.Net -> new RandomNet(seed) :> IRando


    let fromRngGen (rg: rngGen) = create rg.rngType rg.seed


    let nextRngGen (randy: IRando) =
        create randy.rngType (RandomSeed.create randy.NextPositiveInt)
