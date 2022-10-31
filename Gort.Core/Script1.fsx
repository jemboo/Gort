type LoggingBuilder() =
    let log p = printfn "expression is %A" p

    member this.Bind(x, f) =
        log x
        f x

    member this.Return(x) =
        x


let logger = new LoggingBuilder()

let loggedWorkflow =
    logger
        {
        let! x = 42
        let! y = 43
        let! z = x + y
        return z
        }


type MaybeBuilder() =

    member this.Bind(x, f) =
        match x with
        | None -> None
        | Some a -> f a

    member this.Return(x) =
        Some x

let maybe = new MaybeBuilder()



let divideBy bottom top =
    if bottom = 0
    then None
    else Some(top/bottom)

let divideByWorkflow init x y z =
    maybe
        {
        let! a = init |> divideBy x
        let! b = a |> divideBy y
        let! c = b |> divideBy z
        return c
        }


type OrElseBuilder() =
    member this.ReturnFrom(x) = x
    member this.Combine (a,b) =
        match a with
        | Some _ -> a  // a succeeds -- use it
        | None -> b    // a fails -- use b instead
    member this.Delay(f) = f()

let orElse = new OrElseBuilder()


let map1 = [ ("1","One"); ("2","Two") ] |> Map.ofList
let map2 = [ ("A","Alice"); ("B","Bob") ] |> Map.ofList
let map3 = [ ("CA","California"); ("NY","New York") ] |> Map.ofList

let multiLookup key = orElse {
    return! map1.TryFind key
    return! map2.TryFind key
    return! map3.TryFind key
    }


multiLookup "A" |> printfn "Result for A is %A"
multiLookup "CA" |> printfn "Result for CA is %A"
multiLookup "X" |> printfn "Result for X is %A"


let strToInt (str:string) =
    let (success, res) = System.Int32.TryParse(str)
    if success then Some res
    else None


let stringAddWorkflow x y z =
    maybe
        {
        let! a = strToInt x
        let! b = strToInt y
        let! c = strToInt z
        return a + b + c
        }


let good = stringAddWorkflow "12" "3" "2"
let bad = stringAddWorkflow "12" "xyz" "2"



let strAdd str i =
    maybe
        {
        let! a = strToInt str
        return a + i
        }

let (>>=) m f =
    match m with
    | Some m -> f m
    | None -> None

let good2 = strToInt "1" >>= strAdd "2" >>= strAdd "3"
let bad2 = strToInt "1" >>= strAdd "xyz" >>= strAdd "3"





open System.Net
open Microsoft.FSharp.Control.WebExtensions

let urlList = [ "GreatSpaceX", "https://www.youtube.com/watch?v=BUvqbgGip-Y"
                "MSDN", "http://msdn.microsoft.com/"
                "Bing", "http://www.bing.com"
              ]

let fetchAsync(name, url:string) =
    async {
        try
            let uri = new System.Uri(url)
            let httpClient = new System.Net.Http.HttpClient()
            let! html = Async.AwaitTask (httpClient.GetStringAsync(uri))
            //let webClient = new WebClient()
            //let! html = webClient.AsyncDownloadString(uri)
            printfn "Read %d characters for %s" html.Length name
        with
            | ex -> printfn "%s" (ex.Message);
    }


//fetchAsync urlList.Head |> Async.RunSynchronously

let runAll() =
    urlList
    |> Seq.map fetchAsync
    |> Seq.map Async.Start
    |> Seq.toArray
    |> ignore



//let runAll() =
//    urlList
//    |> Seq.map fetchAsync
//    |> Async.Parallel
//    |> Async.RunSynchronously
//    |> ignore

runAll()