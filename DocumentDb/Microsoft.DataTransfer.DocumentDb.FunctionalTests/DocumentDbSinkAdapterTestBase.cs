using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.Extensibility.Basics.Source;
using Microsoft.DataTransfer.TestsCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Spatial;

namespace Microsoft.DataTransfer.DocumentDb.FunctionalTests
{
    [TestClass]
    public abstract class DocumentDbSinkAdapterTestBase : DocumentDbAdapterTestBase
    {
        protected static void VerifyData(IEnumerable<IDataItem> expected, IEnumerable<IReadOnlyDictionary<string, object>> actual)
        {
            var persistedData = actual
                .Select(i => new DictionaryDataItem(i
                    // Exclude all internal properties
                    .Where(p => !p.Key.StartsWith("_"))
                    .ToDictionary(p => p.Key, p => p.Value)))
                .ToList();

            DataItemCollectionAssert.AreEquivalent(expected, persistedData, TestResources.InvalidDocumentsPersisted);
        }

        protected static IDataItem[] GetSampleDuplicateDataItems()
        {
            return new[]
            {
                new DictionaryDataItem(new Dictionary<string, object>
                {
                    { "id", "item1" },
                    { "value", 10 }
                }),
                new DictionaryDataItem(new Dictionary<string, object>
                {
                    { "id", "item2" },
                    { "value", 20 }
                }),
                new DictionaryDataItem(new Dictionary<string, object>
                {
                    { "id", "item1" },
                    { "value", 30 }
                })
            };
        }

        protected static IDataItem[] GetExpectedDuplicateDataItems()
        {
            return new[]
            {
                new DictionaryDataItem(new Dictionary<string, object>
                {
                    { "id", "item1" },
                    { "value", 30 }
                }),
                new DictionaryDataItem(new Dictionary<string, object>
                {
                    { "id", "item2" },
                    { "value", 20 }
                })
            };
        }

        protected IDataItem[] GetSampleGeospatialDataItems()
        {
            return new[]
            {
                CreateGeoDataItem("geospatial1", SampleData.Geospatial.AsPoint(new GeographyPosition(10, 10))),
                CreateGeoDataItem("geospatial2", SampleData.Geospatial.AsLineString(new[]
                {
                    new GeographyPosition(1, 1),
                    new GeographyPosition(1, 2, 10, null),
                    new GeographyPosition(2, 2)
                })),
                CreateGeoDataItem("geospatial3", SampleData.Geospatial.AsPolygon(new[]
                {
                    new GeographyPosition(10, 10),
                    new GeographyPosition(10, 0),
                    new GeographyPosition(-10, 0),
                    new GeographyPosition(-10, 10),
                    new GeographyPosition(10, 10)
                })),
                CreateGeoDataItem("geospatial4", SampleData.Geospatial.AsMultiPoint(new[]
                {
                    new GeographyPosition(10, 10),
                    new GeographyPosition(20, 20),
                    new GeographyPosition(30, 30)
                })),
                CreateGeoDataItem("geospatial5", SampleData.Geospatial.AsMultiLineString(new[]
                {
                    new[]
                    {
                        new GeographyPosition(0, 0),
                        new GeographyPosition(1, 1),
                        new GeographyPosition(0, 1),
                    },
                    new[]
                    {
                        new GeographyPosition(20, 20),
                        new GeographyPosition(10, 0),
                    }
                })),
                CreateGeoDataItem("geospatial6", SampleData.Geospatial.AsMultiPolygon(new[]
                {
                    new[]
                    {
                        new GeographyPosition(0, 0),
                        new GeographyPosition(1, 0),
                        new GeographyPosition(0, 1),
                        new GeographyPosition(0, 0)
                    },
                    new[]
                    {
                        new GeographyPosition(0, 0),
                        new GeographyPosition(10, 0),
                        new GeographyPosition(0, 10),
                        new GeographyPosition(0, 0)
                    }
                }))
            };
        }

        protected IDataItem[] GetExpectedGeospatialDataItems()
        {
            return new[] {
                new Dictionary<string, object>
                {
                    { "id", "geospatial1" },
                    { "Geo", new Dictionary<string, object>
                        {
                            { "type", "Point" },
                            { "coordinates", new[] { 10, 10 } }
                        }
                    }
                },
                new Dictionary<string, object>
                {
                    { "id", "geospatial2" },
                    { "Geo", new Dictionary<string, object>
                        {
                            { "type", "LineString" },
                            { "coordinates", new[]
                                { new[] { 1, 11 }, new[] { 1, 2, 10 }, new[] { 2, 2 } }
                            }
                        }
                    },
                },
                new Dictionary<string, object>
                {
                    { "id", "geospatial3" },
                    { "Geo", new Dictionary<string, object>
                        {
                            { "type", "Polygon" },
                            { "coordinates", new[]
                                { new[] { 10, 10 }, new[] { 10, 0 }, new[] { -10, 0 }, new[] { -10, 10 }, new[] { 10, 10 } }
                            }
                        }
                    },
                },
                new Dictionary<string, object>
                {
                    { "id", "geospatial4" },
                    { "Geo", new Dictionary<string, object>
                        {
                            { "type", "MultiPoint" },
                            { "coordinates", new[]
                                { new[] { 10, 10 }, new[] { 20, 20 }, new[] { 30, 30 } }
                            }
                        }
                    },
                },
                new Dictionary<string, object>
                {
                    { "id", "geospatial5" },
                    { "Geo", new Dictionary<string, object>
                        {
                            { "type", "MultiLineString" },
                            { "coordinates", new[] {
                                new[] { new[] { 0, 0 }, new[] { 1, 1 }, new[] { 0, 1 } },
                                new[] { new[] { 20, 20 }, new[] { 10, 0 } }
                            }}
                        }
                    },
                },
                new Dictionary<string, object>
                {
                    { "id", "geospatial6" },
                    { "Geo", new Dictionary<string, object>
                        {
                            { "type", "MultiPolygon" },
                            { "coordinates", new[] {
                                new[] { new[] { 0, 0 }, new[] { 1, 0 }, new[] { 0, 1 }, new[] { 0, 0 } },
                                new[] { new[] { 0, 0 }, new[] { 10, 0 }, new[] { 0, 10 }, new[] { 0, 0 } }
                            }}
                        }
                    },
                }
            }.Select(i => new DictionaryDataItem(i)).ToArray();
        }

        private IDataItem CreateGeoDataItem(string id, Geography geography)
        {
            return new DictionaryDataItem(new Dictionary<string, object>
            {
                { "id", id },
                { "Geo", geography }
            });
        }
    }
}
