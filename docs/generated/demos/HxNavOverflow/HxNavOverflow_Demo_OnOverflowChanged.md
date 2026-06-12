# HxNavOverflow_Demo_OnOverflowChanged.razor

```razor
<div class="border rounded p-2" style="resize: horizontal; overflow: hidden; min-width: 250px; max-width: 100%;">
	<HxNavOverflow OnOverflowChanged="HandleOverflowChanged">
		<HxNavLink Href="#" Text="Home" />
		<HxNavLink Href="#" Text="Dashboard" />
		<HxNavLink Href="#" Text="Products" />
		<HxNavLink Href="#" Text="Services" />
		<HxNavLink Href="#" Text="Analytics" />
		<HxNavLink Href="#" Text="Reports" />
	</HxNavOverflow>
</div>
<p class="mt-2 mb-0">Visible items: @(_visibleCount?.ToString() ?? "n/a"), overflowing items: @(_overflowCount?.ToString() ?? "n/a")</p>

@code
{
	private int? _visibleCount;
	private int? _overflowCount;

	private void HandleOverflowChanged(NavOverflowChangedEventArgs args)
	{
		_visibleCount = args.VisibleCount;
		_overflowCount = args.OverflowCount;
	}
}

```
