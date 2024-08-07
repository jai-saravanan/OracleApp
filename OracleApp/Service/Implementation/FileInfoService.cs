using Microsoft.EntityFrameworkCore;
using OracleApp.Database;
using OracleApp.Database.Models;
using OracleApp.Service.Interface;

namespace OracleApp.Service.Implementation
{
    public class FileInfoService : IFileInfoService
    {
        private FileServerDbContext _dbContext;
        public FileInfoService(FileServerDbContext dbContext)
        {
                _dbContext = dbContext;
        }
        public async Task<bool> SaveFileInformation(string filePath, string caseNumber)
        {
            FileInformation fileInformation = new FileInformation();
            fileInformation.FilePath = filePath;
            fileInformation.CaseNumber = caseNumber;
            _dbContext.FileInfo.Add(fileInformation);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<FileInformation>> GetAllFileInformation()
        {
            return await _dbContext.FileInfo.ToListAsync();
        }

    }
}
