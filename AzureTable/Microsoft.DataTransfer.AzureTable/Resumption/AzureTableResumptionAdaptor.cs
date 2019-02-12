using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Microsoft.DataTransfer.AzureTable.Resumption
{
    /// <summary>
    /// 
    /// </summary>
    public class AzureTableResumptionAdaptor : IDataTransferResumptionAdapter<AzureTablePrimaryKey>
    {
        private readonly string _fileFullPath;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public AzureTableResumptionAdaptor(string fileName)
        {
            Guard.NotEmpty(nameof(fileName), fileName);

            if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                throw new ArgumentException("File name contains invalid characters.");
            }

            var localAppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var folderName = Path.Combine(localAppDataFolder, "dt");
            Directory.CreateDirectory(folderName);

            _fileFullPath = Path.Combine(folderName, fileName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public AzureTablePrimaryKey GetCheckpoint()
        {
            if (File.Exists(_fileFullPath))
            {
                return JsonConvert.DeserializeObject<AzureTablePrimaryKey>(File.ReadAllText(_fileFullPath));
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="checkpoint"></param>
        public void SaveCheckpoint(AzureTablePrimaryKey checkpoint)
        {
            Guard.NotNull(nameof(checkpoint), checkpoint);

            using (StreamWriter file = File.CreateText(_fileFullPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, checkpoint);
            }
        }
    }
}
