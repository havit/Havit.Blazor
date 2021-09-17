using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	// TODO: Move to Havit.Core?
	public readonly struct DateTimeRange
	{
		public DateTime? StartDate { get; init; }
		public DateTime? EndDate { get; init; }

		public override int GetHashCode()
		{
			return HashCode.Combine(StartDate, EndDate);
		}

		public override bool Equals(object obj)
		{
			if (obj is DateTimeRange other)
			{
				return (this.StartDate == other.StartDate) && (this.EndDate == other.EndDate);
			}

			return false;
		}

		public static bool operator ==(DateTimeRange left, DateTimeRange right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(DateTimeRange left, DateTimeRange right)
		{
			return !(left == right);
		}
	}
}
