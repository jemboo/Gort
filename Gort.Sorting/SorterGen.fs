namespace global
open System

type sorter = 
    { 
        order:order; 
        switches:array<switch>; 
        switchCount:switchCount
    }

module SorterGen =

    let makeNone = None