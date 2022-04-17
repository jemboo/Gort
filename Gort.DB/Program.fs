

// For more information see https://aka.ms/fsharp-console-apps
let yab =  Gort.Data.Seed.CauseSeed.GetThat()
let ctxt = new Gort.Data.GortContext()
let wab = SeedData.AddParams ctxt
printfn "Hello from F# %s" wab
