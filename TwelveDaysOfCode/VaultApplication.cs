using System;
using System.Diagnostics;

using MFiles.VAF;
using MFiles.VAF.AppTasks;
using MFiles.VAF.Common;
using MFiles.VAF.Configuration;
using MFiles.VAF.Configuration.AdminConfigurations;
using MFiles.VAF.Extensions;

using MFilesAPI;

namespace TwelveDaysOfCode
{
	/// <summary>
	/// The entry point for this Vault Application Framework application.
	/// </summary>
	/// <remarks>Examples and further information available on the developer portal: http://developer.m-files.com/. </remarks>
	public partial class VaultApplication
		: ConfigurableVaultApplicationBase<Configuration>
	{
		public override string GetName(IConfigurationRequestContext context)
		{
			return "Twelve Days Of Code";
		}
	}
}