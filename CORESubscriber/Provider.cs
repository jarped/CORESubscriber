﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace CORESubscriber
{
    internal class Provider
    {
        internal static string ConfigFile;

        public static readonly List<object> ProviderDefaults = new List<object> {new XElement("datasets")};

        public static readonly List<object> DatasetDefaults = new List<object>
        {
            new XAttribute("nameSpace", ""),
            new XAttribute("lastindex", 0),
            new XAttribute("wfsClient", "")
        };

        internal static readonly List<string> DatasetFields =
            new List<string> {"datasetId", "name", "version", "applicationSchema"};

        internal static string Password { get; set; }

        internal static string User { get; set; }

        internal static string ApiUrl { get; set; }

        internal static string DatasetId { get; set; }

        public static string ApplicationSchema { get; set; }

        internal static long SubscriberLastIndex { get; set; }

        internal static void Save(IEnumerable<XElement> datasetsList)
        {
            var datasetsDocument = File.Exists(ConfigFile) ? ReadConfigFile() : new XDocument(CreateDefaultProvider());

            AddDatasetsToDocument(datasetsList, datasetsDocument);

            datasetsDocument.Save(new FileStream(ConfigFile, FileMode.OpenOrCreate));
        }

        internal static XDocument ReadConfigFile()
        {
            return XDocument.Parse(File.ReadAllText(ConfigFile));
        }

        private static void AddDatasetsToDocument(IEnumerable<XElement> datasetsList, XContainer datasetsDocument)
        {
            foreach (var xElement in datasetsList)
            {
                if (datasetsDocument.Descendants("datasets").Descendants().Any(d =>
                    DatasetFields.All(f =>
                        d.Attribute(f)?.Value == xElement.Attribute(f)?.Value)
                ))
                    continue;

                datasetsDocument.Descendants("datasets")
                    .First()?.Add(xElement);
            }
        }

        private static XElement CreateDefaultProvider()
        {
            var providerElement = new XElement("provider");

            providerElement.Add(ProviderDefaults);

            return providerElement;
        }

        internal static void SetProviderDefaults()
        {
            ProviderDefaults.Add(new List<object>
            {
                new XAttribute("uri", ApiUrl),
                new XAttribute("user", User),
                new XAttribute("password", Password),
                new XAttribute("applicationSchema", "")
            });
        }
    }
}