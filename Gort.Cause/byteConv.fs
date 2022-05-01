namespace global
open System
open System.Text

module byteConv =

    let intFromBytes (bVals:byte[]) =
        try
            BitConverter.ToInt32(bVals, 0) |> Ok
        with
            | ex -> ("error in getInt: " + ex.Message ) |> Result.Error 


    let intArrayFromBytes (bVals:byte[]) =
        try
            let iB = Array.zeroCreate<Int32> (bVals.Length / 4)
            Buffer.BlockCopy(bVals, 0, iB, 0, bVals.Length)
            iB  |> Ok
        with
            | ex -> ("error in getIntArray: " + ex.Message ) |> Result.Error 


    let doubleFromBytes (bVals:byte[]) =
        try
            BitConverter.ToDouble(bVals, 0) |> Ok
        with
            | ex -> ("error in getDouble: " + ex.Message ) |> Result.Error 


    let doubleArrayFromBytes (bVals:byte[]) =
        try
            let iB = Array.zeroCreate<float> (bVals.Length / 8)
            Buffer.BlockCopy(bVals, 0, iB, 0, bVals.Length)
            iB  |> Ok
        with
            | ex -> ("error in getIntArray: " + ex.Message ) |> Result.Error 


    let stringFromBytes (bVals:byte[]) =
        try
            Encoding.Default.GetString(bVals) |> Ok
        with
            | ex -> ("error in getDouble: " + ex.Message ) |> Result.Error 


    let stringArrayFromBytes (bVals:byte[]) =
        try
            let csv = Encoding.Default.GetString(bVals)
            csv.Split("\n".ToCharArray()) |> Ok
        with
            | ex -> ("error in getDouble: " + ex.Message ) |> Result.Error 


    let guidFromBytes (bVals:byte[]) =
        try
            Guid(bVals) |> Ok
        with
            | ex -> ("error in getGuid: " + ex.Message ) |> Result.Error 


    let guidArrayFromBytes (bVals:byte[]) =
        try
            let gA = Array.zeroCreate<Guid> (bVals.Length / 16)
            for i in 0 .. (gA.Length - 1) do
                let gSl = bVals.AsSpan().Slice(i * 16, 16)
                gA[i] <- new Guid(gSl)
            gA |> Ok
        with
            | ex -> ("error in guidArrayFromBytes: " + ex.Message ) |> Result.Error 


    let bytesFromInt (intV:int) =
        try
            BitConverter.GetBytes(intV) |> Ok
        with
            | ex -> ("error in bytesFromInt: " + ex.Message ) |> Result.Error 


    let bytesFromIntArray (intA:int[]) =
        try
            let iB = Array.zeroCreate<byte> (intA.Length * 4)
            Buffer.BlockCopy(intA, 0, iB, 0, intA.Length)
            iB  |> Ok
        with
            | ex -> ("error in bytesFromIntArray: " + ex.Message ) |> Result.Error 


    let bytesFromDouble (fltVal:float) =
        try
            BitConverter.GetBytes(fltVal) |> Ok
        with
            | ex -> ("error in bytesFromDouble: " + ex.Message ) |> Result.Error 


    let bytesFromDoubleAray (dblA:float[]) =
        try
            let dB = Array.zeroCreate<byte> (dblA.Length * 8)
            Buffer.BlockCopy(dblA, 0, dB, 0, dB.Length)
            dB  |> Ok
        with
            | ex -> ("error in bytesFromDoubleAray: " + ex.Message ) |> Result.Error 


    let bytesFromString (str:string) =
        try
            Encoding.ASCII.GetBytes(str) |> Ok
        with
            | ex -> ("error in bytesFromString: " + ex.Message ) |> Result.Error 


    let bytesFromStringArray (strA:string[]) =
        try
            let fs = String.Join("\n", strA);
            bytesFromString fs |> Ok
        with
            | ex -> ("error in bytesFromStringArray: " + ex.Message ) |> Result.Error 


    let bytesFromGuid (guVal:Guid) =
        try
            guVal.ToByteArray() |> Ok
        with
            | ex -> ("error in bytesFromGuid: " + ex.Message ) |> Result.Error 


    let bytesFromGuidArray (guids:Guid[]) =
        try
            let gA = Array.zeroCreate<byte> (guids.Length * 16)
            for i in 0 .. (guids.Length - 1) do
                let gSl = guids[i].ToByteArray();
                Buffer.BlockCopy(gSl, 0, gA, i * 16, gSl.Length)
            gA |> Ok
        with
            | ex -> ("error in bytesFromGuidArray: " + ex.Message ) |> Result.Error 