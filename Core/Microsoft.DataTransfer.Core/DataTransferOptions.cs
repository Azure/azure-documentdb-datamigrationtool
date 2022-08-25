namespace Microsoft.DataTransfer.Core;

public class DataTransferOptions
{
    public string? Source { get; set; }
    public string? Sink { get; set; }
    public string? SourceSettingsPath { get; set; }
    public string? SinkSettingsPath { get; set; }
}