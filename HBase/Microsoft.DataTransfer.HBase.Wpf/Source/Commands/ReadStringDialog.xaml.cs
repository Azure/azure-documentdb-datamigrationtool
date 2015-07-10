using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.WpfHost.Basics.Commands;
using System;
using System.Windows;
using System.Windows.Input;

namespace Microsoft.DataTransfer.HBase.Wpf.Source.Commands
{
    partial class ReadStringDialog : Window
    {
        public static readonly DependencyProperty InputStringProperty = DependencyProperty.Register(
            ObjectExtensions.MemberName<ReadStringDialog>(c => c.InputString),
            typeof(string), typeof(ReadStringDialog),
            new FrameworkPropertyMetadata(String.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string InputString
        {
            get { return (string)GetValue(InputStringProperty); }
            set { SetValue(InputStringProperty, value); }
        }

        public ICommand SetOkDialogResult { get; private set; }

        public ReadStringDialog()
        {
            InitializeComponent();
            SetOkDialogResult = new SetOkDialogResultCommand(this);
            LayoutRoot.DataContext = this;
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            MinHeight = ActualHeight;
            MaxHeight = ActualHeight;
        }

        private class SetOkDialogResultCommand : CommandBase
        {
            private Window owner;

            public SetOkDialogResultCommand(Window owner)
            {
                Guard.NotNull("owner", owner);
                this.owner = owner;
            }

            public override bool CanExecute(object parameter)
            {
                return true;
            }

            public override void Execute(object parameter)
            {
                owner.DialogResult = true;
            }
        }
    }
}
