export function setupChart(id, dotnetReference, options, autoResize) {
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