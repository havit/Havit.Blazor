﻿using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.ECharts;

public partial class HxEChart : IAsyncDisposable
{
	private static readonly JsonSerializerOptions s_JsonSerializerOptions;

	[Parameter] public string ChartId { get; set; } = $"echart-{Guid.NewGuid()}";

	[Parameter] public object Options { get; set; }
	[Parameter] public string Height { get; set; } = "400px";
	[Parameter] public bool AutoResize { get; set; } = false;

	[Parameter] public EventCallback<EChartsClickArgs> OnClick { get; set; }

	[Inject] protected IJSRuntime JsRuntime { get; set; }

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

	public HxEChart()
	{
		_dotNetObjectReference = DotNetObjectReference.Create(this);
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
		_jsModule ??= await JsRuntime.InvokeAsync<IJSObjectReference>("import", Assets["HxEChart.js"]);
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

	public record JSFunc(string RawCode);

	public class JSFuncConverter : JsonConverter<JSFunc>
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
}