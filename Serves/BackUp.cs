using Microsoft.Data.SqlClient;
        using System;
using System.Data.SqlClient;
using System.IO;
using Microsoft.Extensions.Configuration;
namespace CREDAJAX.Serves
{
    public class BackUp
    {




        private readonly string _connectionString;
        private readonly string _backupFolderPath;

        public BackUp(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Mycon");
            _backupFolderPath = "C:\\DatabaseBackups"; 
        }

        public string CreateBackup()
        {
            try
            {
                string databaseName = new SqlConnectionStringBuilder(_connectionString).InitialCatalog;
                string backupFileName = Path.Combine(_backupFolderPath, $"{databaseName}_{DateTime.Now:yyyyMMddHHmmss}.bak");

                if (!Directory.Exists(_backupFolderPath))
                {
                    Directory.CreateDirectory(_backupFolderPath);
                }

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = $"BACKUP DATABASE [{databaseName}] TO DISK = '{backupFileName}'";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                return $"✔️ تم إنشاء النسخة الاحتياطية بنجاح: {backupFileName}";
            }
            catch (Exception ex)
            {
                return $"❌ خطأ أثناء النسخ الاحتياطي: {ex.Message}";
            }
        }
    }

}

