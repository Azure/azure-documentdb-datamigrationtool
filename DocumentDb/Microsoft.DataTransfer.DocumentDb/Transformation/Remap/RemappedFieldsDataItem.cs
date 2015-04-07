using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Basics.Collections;
using Microsoft.DataTransfer.Extensibility;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.DataTransfer.DocumentDb.Transformation.Remap
{
    sealed class RemappedFieldsDataItem : DataItemWrapper
    {
        private IReadOnlyMap<string, string> fieldsMapping;
        private HashSet<string> inactiveMappings;

        public RemappedFieldsDataItem(IDataItem dataItem, IReadOnlyMap<string, string> fieldsMapping)
            : base(dataItem)
        {
            Guard.NotNull("fieldsMapping", fieldsMapping);
            this.fieldsMapping = fieldsMapping;
            inactiveMappings = GetNonExistingMappingFields(fieldsMapping);
        }

        private HashSet<string> GetNonExistingMappingFields(IReadOnlyMap<string, string> fieldsMapping)
        {
            var nonExistingFields = new HashSet<string>();

            string mappedFieldName;
            foreach (var fieldName in base.GetFieldNames())
            {
                if (!fieldsMapping.TryGetKey(fieldName, out mappedFieldName))
                    // If nothing maps into current field - ignore
                    continue;

                // There is a mapping that results into already existing field, preserve source field name
                nonExistingFields.Add(mappedFieldName);
            }

            // All preserved source field names should be ignored (should not belong to data item), otherwise we have a conflict
            foreach (var fieldName in base.GetFieldNames())
                if (nonExistingFields.Contains(fieldName) &&
                    // this is not a check, just get target field name for the exception message
                    fieldsMapping.TryGetValue(fieldName, out mappedFieldName))
                    throw Errors.DataItemAlreadyContainsField(mappedFieldName);

            return nonExistingFields;
        }

        public override IEnumerable<string> GetFieldNames()
        {
            string newFieldName;
            foreach (var fieldName in base.GetFieldNames())
                yield return fieldsMapping.TryGetValue(fieldName, out newFieldName) ? newFieldName : fieldName;
        }

        public override object GetValue(string fieldName)
        {
            // Note: Only top-level fields remapping is supported
            string originalFieldName;

            return base.GetValue(
                fieldsMapping.TryGetKey(fieldName, out originalFieldName) && !inactiveMappings.Contains(originalFieldName)
                    ? originalFieldName : fieldName);
        }
    }
}
