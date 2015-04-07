using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.DocumentDb.Transformation;
using System;

namespace Microsoft.DataTransfer.DocumentDb.Shared
{
    abstract class DocumentDbAdapterBase<TClient, TConfiguration>
        where TClient : class, IDisposable
        where TConfiguration : class
    {
        private TClient client;

        protected TClient Client { get { return client; } }
        protected IDataItemTransformation Transformation { get; private set; }
        protected TConfiguration Configuration { get; private set; }

        public DocumentDbAdapterBase(TClient client, IDataItemTransformation transformation, TConfiguration configuration)
        {
            Guard.NotNull("client", client);
            Guard.NotNull("transformation", transformation);
            Guard.NotNull("configuration", configuration);

            this.client = client;
            Transformation = transformation;
            Configuration = configuration;
        }

        public virtual void Dispose()
        {
            TrashCan.Throw(ref client);
        }
    }
}
