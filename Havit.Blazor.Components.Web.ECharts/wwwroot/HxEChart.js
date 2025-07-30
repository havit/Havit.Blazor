const AXIS_POINTER_DEBOUNCE_MS = 50;

export function setupChart(id, dotnetReference, options, autoResize, subscribeOnAxisPointerUpdate) {
	let optionsObject = eval('(' + options + ')');

	const element = document.getElementById(id);
	if (!element) {
		return;
	}
	if (!element.echart) {
		const chart = echarts.init(element);
		element.echartsDotnetReference = dotnetReference;

		if (autoResize) {
			element.resizeObserver = new ResizeObserver(() => { resizeChart(id); });
			element.resizeObserver.observe(element);
		}

		if (subscribeOnAxisPointerUpdate) {
			chart.on('updateAxisPointer', (params) => {
				onAxisPointerUpdate(dotnetReference, chart, params);
			});
		}

		chart.getZr().on('click', function (event) {
			event.event.stopPropagation();
			event.event.preventDefault();

			if (!event.target) {
				// blank click event
				handleClick(dotnetReference);
			}
		});

		chart.on('click', (params) => {
			// echarts event
			handleClick(dotnetReference, params);
		});
		element.echart = chart;
	}

	element.echart.setOption(optionsObject);
}

function onAxisPointerUpdate(dotnetReference, chart, params) {
	const dataIndex = params.dataIndex;

	if (dataIndex !== undefined) {
		const seriesList = chart.getOption().series ?? [];

		const seriesEvents = seriesList.map((series, index) => {
			if (series?.data?.[dataIndex] === undefined) {
				return null;
			}

			let rawValue = series.data[dataIndex];
			let value = Array.isArray(rawValue) ? rawValue[1] : rawValue;

			return {
				seriesIndex: index,
				axisDimension: params?.axesInfo?.[0]?.axisDim,
				valueIndex: params?.axesInfo?.[0]?.value,
				dataIndex: dataIndex,
				value: value
			};
		}).filter(x => x !== null); // remove any nulls for missing data

		if (chart.__axisPointerTimeout) {
			clearTimeout(chart.__axisPointerTimeout);
		}

		chart.__axisPointerTimeout = setTimeout(() => {
			dotnetReference.invokeMethodAsync('HandleAxisPointerUpdate', seriesEvents);
		}, AXIS_POINTER_DEBOUNCE_MS);
	}
}

function handleClick(dotnetReference, params) {
	// we do not want to pass all the properties of the event
	dotnetReference.invokeMethodAsync('HandleClick', {
		componentType: params?.componentType,
		seriesType: params?.seriesType,
		seriesName: params?.seriesName,
		seriesIndex: params?.seriesIndex,
		name: params?.name,
		dataIndex: params?.dataIndex,
		value: params?.value,
		targetType: params?.targetType,
	});
}

function resizeChart(id) {
	const element = document.getElementById(id);
	if (!element || !element.echart) {
		return;
	}

	const chart = element.echart;
	chart.resize();
}

export function dispose(id) {
	const element = document.getElementById(id);
	if (!element || !element.echart) {
		return;
	}

	const chart = element.echart;

	if (element.resizeObserver) {
		element.resizeObserver.unobserve(element);
		element.resizeObserver.disconnect();
		element.resizeObserver = null;
	}
	chart.dispose();
	element.echart = null;
}