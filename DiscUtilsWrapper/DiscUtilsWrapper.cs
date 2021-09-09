using System;
using System.IO;
using System.Text;


using DiscUtils.Iso9660;

namespace LiteRipper.DiscUtilsWrapper
{
    public static class DiscUtilsWrapper
    {
        public static void CreateIsoImage(string source, string targetIso, string volumeName)
        {
            try
            {
                CDBuilder iso = new CDBuilder
                {
                    UseJoliet = true,
                    VolumeIdentifier = volumeName
                };
                string[] files = Directory.GetFiles(source, "*", SearchOption.AllDirectories);


                foreach (string file in files)
                {
                    FileInfo fi = new FileInfo(file);
                    if (fi.Directory.FullName == Path.GetFullPath(source))
                    {
                        iso.AddFile($"{fi.Name}", fi.FullName);
                        continue;
                    }
                    string dirStruct = fi.Directory.FullName.Replace(Path.GetFullPath(source), string.Empty).Trim('\\').Trim('/');
                    char pathSeperator = dirStruct.Split("//").Length == 0 ? '\\' : '/';

                    string[] pathParts = dirStruct.Split(pathSeperator);
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < pathParts.Length; i++)
                    {
                        if (i != 0)
                            sb.Append("\\");
                        sb.Append(pathParts[i]);

                        iso.AddDirectory(sb.ToString());
                    }
                    iso.AddFile($"{sb.ToString()}\\{fi.Name}", fi.FullName);
                }

                iso.Build(targetIso);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
