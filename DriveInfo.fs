namespace LiteRipper

open System.IO
open System

module Utils =

    let round (x: float, d: int) =
        let rounded = Math.Round(x, d)

        if rounded < x then
            x + Math.Pow(float 10, float -d)
        else
            rounded

    let generateFileSize (filesize: float) : string =
        let rec loop size iteration =
            match size with
            | check when check < float 1024 -> check, iteration
            | _ -> loop (size / float 1024) (iteration + 1)

        let decimalSize, suffixCounter = loop filesize 0

        let suffix =
            match suffixCounter with
            | 0 -> "B"
            | 1 -> "KB"
            | 2 -> "MB"
            | 3 -> "GB"
            | 4 -> "TB"
            | _ -> ""

        let rounded = round (decimalSize, 2)

        sprintf "%g %s" rounded suffix

    let rec selectDrives (drives: DriveInfo list, driveType: DriveType) : DriveInfo list =
        match drives with
        | head :: tail when head.DriveType = driveType -> head :: selectDrives (tail, driveType)
        | _ :: tail -> selectDrives (tail, driveType)
        | head :: tail when
            head.DriveType.Equals(driveType)
            && tail.Length = 0
            ->
            [ head ]
        | [] -> []

    let rec printDrives (drives: DriveInfo list) =
        if drives.Length <> 0 then
            printfn "Label: %s \nDrive Type: %O \nDrive Size: %s \n" drives.Head.VolumeLabel drives.Head.DriveType
            <| generateFileSize (float drives.Head.TotalSize)


            printDrives drives.Tail

    let getDrivesByType (driveType: string) : DriveInfo list =
        let driveTypeObj: DriveType =
            Enum.Parse(typedefof<DriveType>, driveType, true) :?> DriveType

        let drives = List.ofArray (DriveInfo.GetDrives())
        selectDrives (drives, driveTypeObj)
