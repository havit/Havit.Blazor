namespace Havit.Blazor.Components.Web.ECharts;

/// <summary>
/// Represents the arguments for the <see cref="HxEChart.OnAxisPointerUpdated" /> event callback.
/// </summary>
public record EChartAxisPointerUpdatedEventArgs
{
	/// <summary>
	/// Series index in incoming <c>Options.Series</c>.
	/// </summary>
	public int SeriesIndex { get; set; }

	/// <summary>
	/// Data index in inside the incoming data array.
	/// </summary>
	public int DataIndexInside { get; set; }

	/// <summary>
	/// Data index in incoming data array.
	/// </summary>
	public int DataIndex { get; set; }

	/// <summary>
	/// List of all axis and their info.
	/// </summary>
	public List<EChartAxisPointerUpdatedAxisInfo> AxesInfo { get; set; } = [];

	public virtual bool Equals(EChartAxisPointerUpdatedEventArgs other)
	{
		if (ReferenceEquals(this, other))
		{
			return true;
		}

		if (other is null)
		{
			return false;
		}

		return (SeriesIndex == other.SeriesIndex)
			&& (DataIndexInside == other.DataIndexInside)
			&& (DataIndex == other.DataIndex)
			&& AxesInfo.SequenceEqual(other.AxesInfo);
	}

	public override int GetHashCode()
	{
		var hash = new HashCode();

		hash.Add(SeriesIndex);
		hash.Add(DataIndexInside);
		hash.Add(DataIndex);
		foreach (var axis in AxesInfo)
		{
			hash.Add(axis);
		}

		return hash.ToHashCode();
	}
}
