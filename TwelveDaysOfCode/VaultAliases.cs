using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwelveDaysOfCode
{
	static class VaultAliases
	{
		public static class PropertyDefinitions
		{
			public const string URL = "PD.Url";
			public const string LinkExpiryDate = "PD.LinkExpiryDate";
		}

		public static class Workflows
		{
			public static class ShareLink
			{
				public const string Workflow = "WF.ShareLink";

				public const string NotShared = "WFS.ShareLink.NotShared";
				public const string Shared = "WFS.ShareLink.Shared";
			}
		}
	}
}
