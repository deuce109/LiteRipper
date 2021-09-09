namespace LiteRipper.FileSystem

open System.IO
open LiteRipper.DiscUtilsWrapper

type Drive(driveInfo: DriveInfo) =

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

    member _.Type = sprintf "%O" driveInfo.DriveType
    member _.Label = driveInfo.VolumeLabel

    member this.CreateIso(isoName: string, volumeLabel: string) : unit =
        let source = (this :> IFileSystemObject).FullPath


        DiscUtilsWrapper.CreateIsoImage(source, isoName, volumeLabel)




    interface IFileSystemObject with


        member _.FullPath = driveInfo.RootDirectory.FullName

        member _.FileSize =
            driveInfo.TotalSize - driveInfo.AvailableFreeSpace

        member _.FileType = "Drive"

        member this.Copy(copyPath: string) : bool =
            let fullPath = (this :> IFileSystemObject).FullPath

            copyFolderChildren (
                fullPath,
                List.ofSeq
                <| Directory.EnumerateFileSystemEntries(fullPath),
                copyPath
            )

            Directory.Exists(copyPath)

        member this.Exists() : bool =

            Directory.Exists((this :> IFileSystemObject).FullPath)

        member this.ToString() : string =
            let drive = (this :> IFileSystemObject)

            sprintf
                "File Type: %s\nFull Path: %s\nFolder Size: %d\nExists: %b"
                drive.FileType
                drive.FullPath
                drive.FileSize
            <| drive.Exists()
