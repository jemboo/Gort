namespace global

open System.IO
open System

type fileDir = private FileDir of string
type fileFolder = private FileFolder of string
type filePath = private FilePath of string
type fileName = private FileName of string
type fileExt = private FileExt of string


// folder name (single name)
module FileFolder =
    let value (FileFolder str) = str
    let create str = FileFolder str

// directory name (full path from root to folder)
module FileDir =
    let value (FileDir str) = str
    let create str = FileDir str

    let appendFolder (fn: fileFolder) (fd: fileDir) =
        try
            create (Path.Combine(value fd, fn |> FileFolder.value)) |> Ok
        with ex ->
            ("error in addFolderName: " + ex.Message) |> Result.Error

// file extension (single name)
module FileExt =
    let value (FileExt str) = str
    let create str = FileExt str

// file name only, (no path, no extension)
module FileName =
    let value (FileName str) = str
    let create str = FileName str


// FileDir + (FileFolder (optional)) + FileName + file extension
module FilePath =
    let value (FilePath str) = str
    let create str = FilePath str
    let exists (FilePath str) = System.IO.File.Exists str

    let toFileDir (fp: filePath) =
        Path.GetDirectoryName(value fp) |> FileDir.create

    let toFileName (fp: filePath) =
        Path.GetFileNameWithoutExtension(value fp) |> FileName.create

    let fromParts (fd: fileDir) (fn: fileName) (fe: fileExt) =
        try
            let fne = sprintf "%s%s" (FileName.value fn) (FileExt.value fe)
            create (Path.Combine(FileDir.value fd, fne)) |> Ok
        with ex ->
            ("error in addFolderName: " + ex.Message) |> Result.Error


module FileUtils =
    let makeDirectory (fd: fileDir) =
        try
            Directory.CreateDirectory(FileDir.value fd) |> Ok
        with ex ->
            ("error in makeDirectory: " + ex.Message) |> Result.Error


    let clearDirectory (fd: fileDir) =
        try
            let files = Directory.GetFiles(FileDir.value fd, "*.*")
            files |> Array.map (fun f -> File.Delete(f)) |> ignore
            Directory.Delete(FileDir.value fd) |> Ok
        with ex ->
            ("error in clearDirectory: " + ex.Message) |> Result.Error


    let getFileNamesInDirectory (fd: fileDir) ext =
        try
            Directory.GetFiles((FileDir.value fd), ext)
            |> Array.map (Path.GetFileNameWithoutExtension >> FileName.create)
            |> Ok
        with ex ->
            ("error in getFilesInDirectory: " + ex.Message) |> Result.Error


    let getFileNameWithExtInDirectory (fd: fileDir) ext =
        try
            Directory.GetFiles((FileDir.value fd), ext)
            |> Array.map (Path.GetFileName)
            |> Ok
        with ex ->
            ("error in getFilesInDirectory: " + ex.Message) |> Result.Error


    let getFilePathsInDirectory (fd: fileDir) ext =
        try
            Directory.GetFiles((FileDir.value fd), ext) |> Array.map FilePath.create |> Ok
        with ex ->
            ("error in getFilesInDirectory: " + ex.Message) |> Result.Error


    let readFile (fp: filePath) =
        try
            use sr = new System.IO.StreamReader(FilePath.value fp)
            let res = sr.ReadToEnd()
            sr.Dispose()
            res |> Ok
        with ex ->
            ("error in readFile: " + ex.Message) |> Result.Error


    let readLines<'T> (fp: filePath) =
        try
            System.IO.File.ReadLines(FilePath.value fp) |> Ok
        //System.IO.File.ReadAllLines (FilePath.value fp)
        //                |> Ok
        with ex ->
            ("error in readFile: " + ex.Message) |> Result.Error


    let makeFile (fp: filePath) item =
        try
            System.IO.File.WriteAllText((FilePath.value fp), item)
            //use sw = new StreamWriter(path, append)
            //fprintfn sw "%s" item
            //sw.Dispose()
            true |> Ok
        with ex ->
            ("error in writeFile: " + ex.Message) |> Result.Error


    let makeFileFromLines (fp: filePath) (lines: seq<string>) =
        try
            System.IO.File.WriteAllLines((FilePath.value fp), lines)
            true |> Ok
        with ex ->
            ("error in writeFile: " + ex.Message) |> Result.Error


    let appendToFile (fp: filePath) (lines: seq<string>) =
        try
            System.IO.File.AppendAllLines((FilePath.value fp), lines)
            true |> Ok
        with ex ->
            ("error in writeFile: " + ex.Message) |> Result.Error

    let makeArchiver (dir: fileDir) =
        fun (folder: fileFolder) (file: fileName) (ext: fileExt) (data: seq<string>) ->
            try
                let fne = sprintf "%s.%s" (FileName.value file) (FileExt.value ext)
                let fp = Path.Combine(FileDir.value dir, fne) |> FilePath.create
                let dirInfo = System.IO.Directory.CreateDirectory(FileDir.value dir)
                appendToFile fp data
            with ex ->
                ("error in archiver: " + ex.Message) |> Result.Error


type csvFile = { header:string; records:string[]; directory:fileDir; fileName:string; }

module CsvFile =

    let writeCsvFile (csv:csvFile) =
        try
            Directory.CreateDirectory(FileDir.value(csv.directory)) |> ignore
            let FileDir = sprintf "%s\\%s" (FileDir.value(csv.directory)) csv.fileName
            use sw = new StreamWriter(FileDir, false)
            fprintfn sw "%s" csv.header
            csv.records |> Array.iter(fprintfn sw "%s")
            sw.Dispose()
            true |> Ok
        with
            | ex -> ("error in writeFile: " + ex.Message ) |> Result.Error
