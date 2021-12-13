using System.Collections.Generic;
using System.Runtime.Serialization;

using MFiles.VAF.Configuration;
using MFiles.VAF.Extensions.Email;

using MFilesAPI;

namespace TwelveDaysOfCode
{
	[DataContract]
	public class Day2Config
	{
		[DataMember]
		[JsonConfEditor(
			Label = "Property: Url",
			DefaultValue = VaultAliases.PropertyDefinitions.URL
			)]
		[MFPropertyDef(Datatypes = new MFDataType[] { MFDataType.MFDatatypeMultiLineText })]
		public MFIdentifier PD_Url { get; set; } = VaultAliases.PropertyDefinitions.URL;

		[DataMember]
		[JsonConfEditor(
			Label = "Property: Link Expiry Date",
			DefaultValue = VaultAliases.PropertyDefinitions.LinkExpiryDate
			)]
		[MFPropertyDef(Datatypes = new MFDataType[] { MFDataType.MFDatatypeDate })]
		public MFIdentifier PD_LinkExpiryDate { get; set; } = VaultAliases.PropertyDefinitions.LinkExpiryDate;

		[DataMember]
		[JsonConfEditor(
			Label = "Send Email?"
			)]
		public bool SendEmail { get; set; } = false;

		[DataMember]
		[JsonConfEditor(
			Label = "To",
			Hidden = true,
			ShowWhen = ".parent._children{.key == 'SendEmail' && .value == true}"
			)]
		public string Recipient { get; set; }

		[DataMember]
		[JsonConfEditor(
			Label = "SMTP Configuration",
			Hidden = true,
			ShowWhen = ".parent._children{.key == 'SendEmail' && .value == true}"
			)]
		public VAFSmtpConfiguration SmtpConfiguration { get; set; } = new VAFSmtpConfiguration();


		public IEnumerable<ValidationFinding> Validate(Vault vault)
		{
			if (MFIdentifier.IsNullOrUnresolved(PD_Url))
				yield return new ValidationFinding(
					ValidationFindingType.Error,
					"Day 2",
					"Property: Url is empty or cannot be resolved."
					);

			//if (MFIdentifier.IsNullOrUnresolved(PD_LinkExpiryDate))
			//	yield return new ValidationFinding(
			//		ValidationFindingType.Error,
			//		"Day 2",
			//		"Property: Link Expiry Date is empty or cannot be resolved."
			//		);

			if(SendEmail)
			{
				if (string.IsNullOrWhiteSpace(Recipient))
					yield return new ValidationFinding(
						ValidationFindingType.Error,
						"Day 2",
						"Santa doesn't receive emails without a recipient."
						);

				foreach (var finding in SmtpConfiguration.Validate(vault))
				{
					yield return finding;
				}
			}

		}
	}
}
