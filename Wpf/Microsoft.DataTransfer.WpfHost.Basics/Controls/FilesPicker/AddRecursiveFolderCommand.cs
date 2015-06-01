using Microsoft.DataTransfer.Basics.IO;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls.FilesPicker
{
    sealed class AddRecursiveFolderCommand : AddFolderCommandBase
    {
        protected override string GetFolderSearchPattern(string folderPath)
        {
            return PathHelper.Combine(folderPath, @"**\*.*");
        }
    }
}
