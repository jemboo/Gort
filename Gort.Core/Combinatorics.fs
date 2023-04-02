namespace global

open SysExt
open Microsoft.FSharp.Core


module Combinatorics =
    // all the bool arrays of length m with n members true
    let enumerateMwithN (m:int) (n:int) =
        let maxVal = ( 1 <<< m ) - 1
        seq { for i = 0 to maxVal do
                if i.count_dense = n then
                    yield i.toBoolArray m }