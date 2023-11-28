using System.Threading.Tasks;

namespace CapstoneNHolbrook.Services
{
    public interface ICloudBackupRestore
    {
        Task BackupDatabaseAsync();
        Task RestoreDatabaseAsync();
    }
}
