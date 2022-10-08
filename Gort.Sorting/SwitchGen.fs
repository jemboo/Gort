namespace global

open System

module SwitchGen =
    // returns a sequence of suffix.length that begins with prefix
    // the prefix.length items at the end of suffix are discarded
    let imposePrefix<'T> (prefix: 'T seq) (suffix: 'T seq) =
        let sa = suffix |> Seq.toArray

        seq {
            yield! prefix
            yield! sa
        }
        |> Seq.take sa.Length


    let oddeven_merge_switches (length: int) =
        let mutable lret = List.Empty
        let t = Math.Ceiling(Math.Log2(length |> float))
        let cxp = Math.Pow(2.0, t - 1.0) |> int
        let mutable p = cxp

        while (p > 0) do
            let mutable q = cxp
            let mutable r = 0
            let mutable d = p

            while (d > 0) do
                seq { 0 .. (length - d - 1) }
                |> Seq.filter (fun v -> (v &&& p) = r)
                |> Seq.iter (fun v -> lret <- { switch.low = v; hi = v + d } :: lret)

                d <- q - p
                q <- q / 2
                r <- p

            p <- p / 2

        lret |> List.rev


    let oddeven_merge_stages (length: int) =
        let mutable lret = List.Empty
        let t = Math.Ceiling(Math.Log2(length |> float))
        let cxp = Math.Pow(2.0, t - 1.0) |> int
        let mutable p = cxp

        while (p > 0) do
            let mutable q = cxp
            let mutable r = 0
            let mutable d = p

            while (d > 0) do
                let mutable lstage = List.Empty

                seq { 0 .. (length - d - 1) }
                |> Seq.filter (fun v -> (v &&& p) = r)
                |> Seq.iter (fun v -> lstage <- { switch.low = v; hi = v + d } :: lstage)

                d <- q - p
                q <- q / 2
                r <- p
                lret <- lstage :: lret

            p <- p / 2

        lret |> List.rev
