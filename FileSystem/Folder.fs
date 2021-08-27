namespace LiteRipper.FileSystem

open System.IO

type Folder(fullPath: string) =
    let rec copyFolderChildren (source: string list, dest: string) : unit =
        match source with
        | head :: tail when Directory.Exists(head) ->
            copyFolderChildren (
                List.ofSeq
                <| Directory.EnumerateFileSystemEntries(head),
                dest
            )

            copyFolderChildren (tail, dest)
        | head :: tail ->
            File.Copy(head, dest)
            copyFolderChildren (tail, dest)
        | head :: _ -> File.Copy(head, dest)
        | [] -> ()

    let rec getSizeOfFileList (files: string list) : int64 =
        match files with
        | head :: tail when Directory.Exists(head) ->
            getSizeOfFileList (
                List.ofSeq
                <| Directory.EnumerateFileSystemEntries(head)
            )
            + getSizeOfFileList (tail)
        | head :: tail when File.Exists(head) -> FileInfo(head).Length + getSizeOfFileList (tail)
        | [] -> int64 0
        | _ :: _ -> int64 0

    interface IFileSystemObject with

        member this.FullPath: string = fullPath
        member this.FileType: string = "File"

        member this.FileSize: int64 =
            getSizeOfFileList (
                List.ofSeq
                <| Directory.EnumerateFileSystemEntries(fullPath)
            )

        member this.Exists() : bool =
            let fullPath = (this :> IFileSystemObject).FullPath
            Directory.Exists fullPath

        member this.Copy(copyPath) : bool =
            let fullPath = (this :> IFileSystemObject).FullPath

            Directory.Exists(copyPath)

        member this.ToString() =
            let file = (this :> IFileSystemObject)

            sprintf "File Type: %s\nFull Path: %s\nExists: %b" file.FileType file.FullPath
            <| file.Exists()
