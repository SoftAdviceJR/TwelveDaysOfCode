using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MFiles.VAF.Common;
using MFiles.VAF.Configuration;
using MFiles.VAF.Extensions;

using MFilesAPI;
using MFilesAPI.Extensions.Email;

namespace TwelveDaysOfCode
{

	public partial class VaultApplication
	{

		/// <summary>
		/// Executed when an object is moved into a workflow state
		/// with alias "WFS.ShareLink.Shared".
		/// </summary>
		/// <param name="env">The vault/object environment.</param>
		[StateAction(VaultAliases.Workflows.ShareLink.Shared)]
		public void CreateSharedLink(StateEnvironment env)
		{
			if (Configuration.Day2Config.Validate(env.Vault).Any(f => f.Type == ValidationFindingType.Error))
				throw new InvalidOperationException("There are configuration errors for 'Day 2'. Please contact your local elf.");

			if (env.ObjVerEx.Info.FilesCount != 1)
				return;

			var file = env.ObjVerEx.Info.Files[1].FileVer;
			file.Version = -1;

			DateTime? expiryDate = DateTime.Now.AddDays(30);


			if (!MFIdentifier.IsNullOrUnresolved(Configuration.Day2Config.PD_LinkExpiryDate) &&
				env.ObjVerEx.HasValue(Configuration.Day2Config.PD_LinkExpiryDate))
			{
				expiryDate = env.ObjVerEx.GetPropertyAsDateTime(Configuration.Day2Config.PD_LinkExpiryDate);
			}

			Timestamp expirationTime = new Timestamp();
			expirationTime.SetValue(expiryDate);

			SharedLinkInfo info = new SharedLinkInfo()
			{
				ObjVer = env.Vault.ObjectOperations.GetLatestObjectVersionAndProperties(env.ObjVer.ObjID, false).ObjVer,
				FileVer = file,
				ExpirationTime = expirationTime,
				LinkType = MFSharedLinkType.MFSharedLinkTypeReadOnly
			};

			info = env.Vault.SharedLinkOperations.CreateSharedLink(info);

			var urlLines = new List<string>();

			if (env.ObjVerEx.HasValue(Configuration.Day2Config.PD_Url))
				urlLines.Add(env.ObjVerEx.GetPropertyText(Configuration.Day2Config.PD_Url));


			string baseAddress = UrlHelper.GetBaseUrlForWebAccess(env.Vault);
			if (string.IsNullOrWhiteSpace(baseAddress))
				baseAddress = "localhost/M-Files"; //Gotta have something to make it work locally

			string newLink = Path.Combine(
				baseAddress,
				"SharedLinks.aspx" +
				"?accesskey=" + info.AccessKey +
				"&VaultGUID=" + env.Vault.GetGUID()
				);

			urlLines.Add(newLink);

			env.ObjVerEx.SaveProperty(Configuration.Day2Config.PD_Url, MFDataType.MFDatatypeMultiLineText, string.Join(Environment.NewLine, urlLines));

			env.ObjVerEx.SetModifiedBy(env.CurrentUserID);

			if (Configuration.Day2Config.SendEmail)
				using (var emailMessage = new EmailMessage(this.Configuration.Day2Config.SmtpConfiguration))
				{
					emailMessage.AddRecipient(AddressType.To, Configuration.Day2Config.Recipient);

					emailMessage.Subject = "\uD83C\uDF85 Merry Christmas \uD83C\uDF84";
					emailMessage.HtmlBody =
						"<html><body>" +
						$"<p>Merry Christmas</p> <br/><br/>" +
						$"<p>Here's a <a href='{newLink}'>link</a></p> <br/><br/>" +
						"</body></html>";
					emailMessage.SetFrom(Configuration.Day2Config.SmtpConfiguration.DefaultSender);

					emailMessage.Send();

				}
		}

	}
}
