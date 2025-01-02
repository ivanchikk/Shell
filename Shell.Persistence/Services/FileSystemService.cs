using Microsoft.VisualBasic.FileIO;
using Shell.Application.Common.Interfaces;

namespace Shell.Persistence.Services
{
    public class FileSystemService : IFileSystemService
    {
        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public void CreateFile(string path)
        {
            File.Create(path).Dispose();
        }

        public void DeleteDirectory(string path)
        {
            //Directory.Delete(path);
            FileSystem.DeleteDirectory(path, UIOption.AllDialogs, RecycleOption.SendToRecycleBin);
        }

        public void DeleteFile(string path)
        {
            //File.Delete(path);
            FileSystem.DeleteFile(path, UIOption.AllDialogs, RecycleOption.SendToRecycleBin);
        }

        public void UpdateDirectory(string path, string newPath)
        {
            //Directory.Move(path, newPath);
            FileSystem.MoveDirectory(path, newPath, UIOption.AllDialogs);
        }

        public void UpdateFile(string path, string newPath)
        {
            //File.Move(path, newPath);
            FileSystem.MoveFile(path, newPath, UIOption.AllDialogs);
        }

        public void CopyDirectory(string path, string newPath)
        {
            FileSystem.CopyDirectory(path, newPath, UIOption.AllDialogs);
        }

        public void CopyFile(string path, string newPath)
        {
            //File.Copy(path, newPath, true);
            FileSystem.CopyFile(path, newPath, UIOption.AllDialogs);
        }

        public string GetUniqueNewPath(string path)
        {
            var newPath = path;
            var copyIndex = 0;

            while (Path.Exists(newPath))
            {
                copyIndex++;
                var suffix = copyIndex == 1 ? " - Copy" : $" - Copy ({copyIndex})";
                newPath = Path.Combine(path + suffix);
            }

            return newPath;
        }
    }
}
