using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.WpfHost.Basics;
using Microsoft.DataTransfer.WpfHost.ServiceModel;
using Microsoft.DataTransfer.WpfHost.ServiceModel.Steps;
using System;
using System.Windows.Controls;

namespace Microsoft.DataTransfer.WpfHost.Steps
{
    abstract class NavigationStepBase : BindableBase, INavigationStep
    {
        protected static readonly string IsValidMemberName = 
            ObjectExtensions.MemberName<INavigationStep>(s => s.IsValid);

        private bool isValid;
        private bool isAllowed;
        private Lazy<UserControl> presenter;

        protected IDataTransferModel TransferModel { get; private set; }

        public abstract string Title { get; }

        public bool IsValid
        {
            get { return isValid; }
            protected set { SetProperty(ref isValid, value); }
        }

        public bool IsAllowed
        {
            get { return isAllowed; }
            protected set { SetProperty(ref isAllowed, value); }
        }

        public UserControl Presenter
        {
            get { return presenter.Value; }
        }

        public NavigationStepBase(IDataTransferModel transferModel)
        {
            TransferModel = transferModel;
            presenter = new Lazy<UserControl>(CreatePresenter, true);

            IsValid = true;
            IsAllowed = true;
        }

        protected abstract UserControl CreatePresenter();
    }
}
