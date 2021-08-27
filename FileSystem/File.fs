namespace LiteRipper.FileSystem

type File(fullPath: string) =
    interface IFileSystemObject with

        member this.FullPath: string = fullPath
        member this.FileType: string = "File"

        member this.FileSize: int64 = System.IO.FileInfo(fullPath).Length

        member this.Exists() : bool =
            let fullPath = (this :> IFileSystemObject).FullPath
            System.IO.File.Exists fullPath

        member this.Copy(copyPath) : bool =
            let fullPath = (this :> IFileSystemObject).FullPath
            System.IO.File.Copy(fullPath, copyPath)
            System.IO.File.Exists(copyPath)

        member this.ToString() =
            let file = (this :> IFileSystemObject)

            sprintf "File Type: %s\nFull Path: %s\nExists: %b" file.FileType file.FullPath
            <| file.Exists()
