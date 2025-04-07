﻿using System.Text.Json;
using System.Text.Json.Serialization;
using Havit.Blazor.Components.Web.ECharts;
using Havit.Blazor.Components.Web;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.ECharts;

/// <summary>
/// Component for rendering Apache ECharts.
/// </summary>
public partial class HxEChart : IAsyncDisposable
{
	private static readonly JsonSerializerOptions s_JsonSerializerOptions;

	/// <summary>
	/// Unique identifier for the HTML element representing the chart.
	/// </summary>
	[Parameter] public string ChartId { get; set; } = $"echart-{Guid.NewGuid()}";

	/// <summary>
	/// Options for the chart. See <a href="https://echarts.apache.org/en/option.html">ECharts Option</a> for more details.
	/// </summary>
	[Parameter, EditorRequired] public object Options { get; set; }

	/// <summary>
	/// The height of the chart. Default is <c>400</c> units.
	/// </summary>
	[Parameter] public float Height { get; set; } = 400;

	/// <summary>
	/// The height units. Default is <c>px</c>.
	/// </summary>
	[Parameter] public string HeightUnit { get; set; } = "px";

	/// <summary>
	/// Indicates whether the chart should automatically resize. Default is <c>false</c>.
	/// </summary>
	[Parameter] public bool AutoResize { get; set; } = false;

	/// <summary>
	/// Invoked when the chart is clicked.
	/// </summary>
	[Parameter] public EventCallback<EChartsClickArgs> OnClick { get; set; }

	private readonly IJSRuntime _jsRuntime;
	private IJSObjectReference _jsModule;
	private DotNetObjectReference<HxEChart> _dotNetObjectReference;
	private string _currentOptions;
	private bool _shouldSetupChartOnAfterRender;
	private bool _disposed;

	static HxEChart()
	{
		s_JsonSerializerOptions = new JsonSerializerOptions()
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
			Converters =
			{
				new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
				new JSFuncConverter(),
			}
		};
	}

	public HxEChart(IJSRuntime jsRuntime)
	{
		_dotNetObjectReference = DotNetObjectReference.Create(this);
		_jsRuntime = jsRuntime;
	}

	protected override void OnParametersSet()
	{
		if (Options is not null)
		{
			var newOptions = JsonSerializer.Serialize(Options, s_JsonSerializerOptions);
			if (!string.Equals(_currentOptions, newOptions, StringComparison.Ordinal))
			{
				_shouldSetupChartOnAfterRender = true;
				_currentOptions = newOptions;
			}
		}
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (_shouldSetupChartOnAfterRender)
		{
			await EnsureJsModuleAsync();
			if (_disposed)
			{
				return;
			}

			await _jsModule.InvokeVoidAsync("setupChart", ChartId, _dotNetObjectReference, _currentOptions, AutoResize);
		}

		_shouldSetupChartOnAfterRender = false;
	}

	private async Task EnsureJsModuleAsync()
	{
		_jsModule ??= await _jsRuntime.InvokeAsync<IJSObjectReference>("import", Assets["HxEChart.js"]);
	}

	[JSInvokable("HandleClick")]
	public async Task HandleClick(EChartsClickArgs eventParams)
	{
		await OnClick.InvokeAsync(eventParams);
	}

	async ValueTask IAsyncDisposable.DisposeAsync()
	{
		await DisposeAsyncCore();

		//Dispose(disposing: false);
	}

	protected virtual async ValueTask DisposeAsyncCore()
	{
		_disposed = true;

		if (_jsModule != null)
		{
			try
			{
				await _jsModule.InvokeVoidAsync("dispose", ChartId);
				await _jsModule.DisposeAsync();
			}
			catch (JSDisconnectedException)
			{
				// NOOP
			}
		}

		_dotNetObjectReference?.Dispose();
	}

	/// <summary>
	/// Represents a JavaScript function to be passed to the ECharts options.
	/// </summary>
	/// <param name="RawCode">The raw JavaScript code.</param>
	public record JSFunc(string RawCode);

	private class JSFuncConverter : JsonConverter<JSFunc>
	{
		public override JSFunc Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			throw new NotSupportedException();
		}

		public override void Write(Utf8JsonWriter writer, JSFunc value, JsonSerializerOptions options)
		{
			writer.WriteRawValue(value.RawCode, true);
		}
	}

	private string GetChartHeight()
	{
		return $"{Height}{HeightUnit}";
	}
}
