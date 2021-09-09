namespace LiteRipper.FileSystem

open LiteRipper.DiscUtilsWrapper
open System.IO

type Folder(fullPath: string) =

    let addToBaseDest (baseSrc: string, fullSrc: string, dest: string) : string = dest + fullSrc.Replace(baseSrc, "")

    let copyFile (src: string, dest: string) =
        if FileInfo(dest).Directory.Exists then
            File.Copy(src, dest)
        else
            Directory.CreateDirectory(FileInfo(dest).Directory.FullName)
            |> ignore

            File.Copy(src, dest)

    let rec copyFolderChildren (baseSrc: string, source: string list, dest: string) : unit =
        match source with
        | head :: tail when Directory.Exists(head) ->
            copyFolderChildren (
                baseSrc,
                List.ofSeq
                <| Directory.EnumerateFileSystemEntries(head),
                dest
            )

            copyFolderChildren (baseSrc, tail, dest)

            copyFolderChildren (baseSrc, tail, dest)
        | head :: tail ->
            copyFile (head, addToBaseDest (baseSrc, head, dest))
            copyFolderChildren (baseSrc, tail, dest)
        | head :: _ -> copyFile (addToBaseDest (baseSrc, head, dest), dest)
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

    member this.CreateIso(isoName: string, volumeLabel: string) : unit =
        let source = (this :> IFileSystemObject).FullPath

        DiscUtilsWrapper.CreateIsoImage(source, isoName, volumeLabel)

    interface IFileSystemObject with

        member _.FullPath: string = fullPath
        member _.FileType: string = "Folder"

        member _.FileSize: int64 =
            getSizeOfFileList (
                List.ofSeq
                <| Directory.EnumerateFileSystemEntries(fullPath)
            )

        member this.Exists() : bool =
            let fullPath = (this :> IFileSystemObject).FullPath
            Directory.Exists fullPath

        member this.Copy(copyPath) : bool =
            let fullPath = (this :> IFileSystemObject).FullPath

            copyFolderChildren (
                fullPath,
                List.ofSeq
                <| Directory.EnumerateFileSystemEntries(fullPath),
                copyPath
            )

            Directory.Exists(copyPath)

        member this.ToString() =
            let folder = (this :> IFileSystemObject)

            sprintf
                "File Type: %s\nFull Path: %s\nFolder Size: %d\nExists: %b"
                folder.FileType
                folder.FullPath
                folder.FileSize
            <| folder.Exists()
