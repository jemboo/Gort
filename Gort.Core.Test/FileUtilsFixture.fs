namespace Gort.Core.Test
open Microsoft.VisualStudio.TestTools.UnitTesting
open Newtonsoft.Json


[<TestClass>]
type FileUtilsFixture() =

    [<TestMethod>]
    member this.writeCsvFile() =
        let csv =
            {
                csvFile.header = "cat"; 
                directory = FileDir.create "c:\\testFileUtils"; 
                fileName = "fileName.txt"; 
                records = [|"a"; "b"|]
            }
        let res = CsvFile.writeCsvFile csv |> Result.ExtractOrThrow
        Assert.IsTrue(res)

    [<TestMethod>]
    member this.makeArchiver() =
        let fDir = FileDir.create "c:\\folderTest\\archiver"
        let folder = FileFolder.create "FileFolder"
        let file = FileName.create "FileName"
        let ext = FileExt.create "txt"
        let testData = seq {"line1"; "line2"}

        let archiver = FileUtils.makeArchiver fDir
        let res = archiver folder file ext testData
        Assert.AreEqual(1, 1)



