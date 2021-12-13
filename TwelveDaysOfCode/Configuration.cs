using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using MFiles.VAF.Configuration;
using MFiles.VAF.Configuration.JsonAdaptor;

namespace TwelveDaysOfCode
{
	[DataContract]
	public class Configuration
	{
		[DataMember]
		[JsonConfEditor(
			Label = "Day 2"
			)]
		public Day2Config Day2Config { get; set; } = new Day2Config();


	}
}