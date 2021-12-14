﻿using Microsoft.DataTransfer.DynamoDb.Source;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.TestsCommon.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DynamoDb.FunctionalTests
{
    [TestClass]
    public abstract class DynamoDbSourceAdapterTestBase : DynamoDbAdapterTestBase
    {
        protected string TableName { get; private set; }
        protected Dictionary<string, object>[] Data { get; private set; }

        protected override void TestInitialize()
        {
            TableName = Guid.NewGuid().ToString("N");
            Data = CreateSampleData();
            DynamoDbHelper.CreateSampleTableAsync(Settings.DynamoDbConnectionString, TableName, Data).Wait();
        }

        protected abstract Dictionary<string, object>[] CreateSampleData();

        protected override void TestCleanup()
        {
            if (!String.IsNullOrEmpty(TableName))
                DynamoDbHelper.DeleteTableAsync(Settings.DynamoDbConnectionString, TableName).Wait();
        }

        protected async Task<List<IDataItem>> ReadData(IDynamoDbSourceAdapterConfiguration configuration)
        {
            using (var adapter = await new DynamoDbSourceAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance, CancellationToken.None))
            {
                return await ReadDataAsync(adapter);
            }
        }
    }
}
