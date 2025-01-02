namespace Shell.Application.Common.Interfaces
{
    public interface IFileSystemService
    {
        public void CreateDirectory(string path);
        public void CreateFile(string path);
        public void DeleteFile(string path);
        public void DeleteDirectory(string path);
        public void UpdateDirectory(string path, string newPath);
        public void UpdateFile(string path, string newPath);
        public void CopyDirectory(string path, string newPath);
        public void CopyFile(string path, string newPath);
        public string GetUniqueNewPath(string path);
    }
}
