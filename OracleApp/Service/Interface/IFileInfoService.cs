using OracleApp.Database.Models;

namespace OracleApp.Service.Interface
{
    public interface IFileInfoService
    {
        Task<bool> SaveFileInformation(string filePath, string caseNumber);
        Task<List<FileInformation>> GetAllFileInformation();
    }
}
