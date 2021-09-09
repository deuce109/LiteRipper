namespace LiteRipper.FileSystem

type File(fullPath: string) =
    interface IFileSystemObject with

        member _.FullPath: string = fullPath
        member _.FileType: string = "File"

        member _.FileSize: int64 = System.IO.FileInfo(fullPath).Length

        member this.Exists() : bool =
            let fullPath = (this :> IFileSystemObject).FullPath
            System.IO.File.Exists fullPath

        member this.Copy(copyPath) : bool =
            let fullPath = (this :> IFileSystemObject).FullPath
            System.IO.File.Copy(fullPath, copyPath)
            System.IO.File.Exists(copyPath)

        member this.ToString() =
            let file = (this :> IFileSystemObject)

            sprintf "File Type: %s\nFull Path: %s\nFile Size: %d\nExists: %b" file.FileType file.FullPath file.FileSize
            <| file.Exists()
