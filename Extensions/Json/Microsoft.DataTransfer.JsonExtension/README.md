# JSON Extension

The JSON data transfer extension provides source and sink capabilities for reading from and writing to JSON files. Source and sink both support string, number, and boolean property values, arrays, and hierarchical nested object structures.

## Settings

Source and sink settings both require a `FilePath` parameter, which should specify a path to a JSON file, either as absolute or relative to the application. Sink also supports an optional `Indented` parameter (`false` by default) and an optional `IncludeNullFields` parameter (`false` by default) to control the formatting of the JSON output.

### Source

```json
{
    "FilePath": ""
}
```

### Sink

```json
{
    "FilePath": "",
    "Indented": true
}
```