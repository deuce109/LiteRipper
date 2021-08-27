namespace LiteRipper.FileSystem

open System.IO

type Drive private (driveInfo: DriveInfo) =
    member _.FullPath = driveInfo.RootDirectory.FullName
    member _.FileSize = driveInfo.TotalSize
    member _.Label = driveInfo.VolumeLabel
    member _.Type = sprintf "%O" driveInfo.DriveType
