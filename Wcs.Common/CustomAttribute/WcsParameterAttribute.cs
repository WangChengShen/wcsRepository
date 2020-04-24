using System;
using System.Collections.Generic;
using System.Text;

namespace Wcs.Common
{
	[AttributeUsage(AttributeTargets.Parameter)]
	public class WcsParameterAttribute : Attribute
	{
		public string shortName;
		public WcsParameterAttribute(string shortName)
		{
			this.shortName = shortName;
		}
	}
}
