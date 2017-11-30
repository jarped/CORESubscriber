﻿using System.Linq;
using CORESubscriber.Xml;

namespace CORESubscriber.SoapAction
{
    public class GetChangelog
    {
        public static string Run()
        {
            const string action = "GetChangelog";

            var getChangelog = SoapRequest.GetSoapContentByAction(action);

            getChangelog.Descendants(XmlNamespaces.Geosynchronization + XmlAttributes.ChangelogId.LocalName).First()
                    .Value =
                Dataset.OrderedChangelogId.ToString();

            var responseContent = SoapRequest.Send(action, getChangelog);

            var returnValue = responseContent
                .Descendants(XmlNamespaces.Geosynchronization + XmlElements.DownloadUri.LocalName).First().Value;

            return returnValue;
        }
    }
}