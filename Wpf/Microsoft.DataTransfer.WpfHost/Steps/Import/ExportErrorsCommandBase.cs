using Microsoft.DataTransfer.WpfHost.Basics.Commands;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.WpfHost.Steps.Import
{
    abstract class ExportErrorsCommandBase : CommandBase
    {
        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            var errors = parameter as IReadOnlyCollection<KeyValuePair<string, string>>;
            if (errors == null)
                return;

            PersistErrors(errors);
        }

        protected abstract void PersistErrors(IReadOnlyCollection<KeyValuePair<string, string>> errors);
    }
}
