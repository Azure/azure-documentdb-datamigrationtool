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
        private readonly string _fileName;

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

            _fileName = fileName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public AzureTablePrimaryKey GetCheckpoint()
        {
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _fileName);
            if (File.Exists(fileName))
            {
                return JsonConvert.DeserializeObject<AzureTablePrimaryKey>(File.ReadAllText(fileName));
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

            using (StreamWriter file = File.CreateText(
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _fileName)))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, checkpoint);
            }
        }
    }
}
