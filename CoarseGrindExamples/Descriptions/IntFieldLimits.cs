using System;
using Rockabilly.CoarseGrind.Descriptions;
using Rockabilly.Common;

namespace Rockabilly.CoarseGrind.Examples
{
	public class IntFieldLimits : IntLimitsDescription
	{
		// In most cases the field may need to retrieve the limit
		// from an external source, rather than hard-code it.
		public override int UpperLimit
		{
			get
			{
				return 250;
			}
		}

		// In most cases the field may need to retrieve the limit
		// from an external source, rather than hard-code it.
		public override int LowerLimit
		{
			get
			{
				return -250;
			}
		}
	}
}
