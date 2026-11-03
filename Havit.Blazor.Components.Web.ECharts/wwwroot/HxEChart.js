const AXIS_POINTER_CALL_TIMEOUT_MS = 1000;

export function setupChart(id, dotnetReference, options, autoResize, subscribeOnAxisPointerUpdate) {
	if (options === null || options === undefined) {
		// defensive: a null/undefined payload would crash echarts.setOption() and take the whole render down
		return;
	}

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
			chart.__axisPointerUpdate = {
				pendingParams: null,
				pendingKey: null,
				sentKey: null,
				frame: 0,
				callId: 0,
				inFlight: false,
				disposed: false,
			};
			chart.on('updateAxisPointer', (params) => {
				handleUpdateAxisPointer(dotnetReference, chart, params);
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

	if (element.echart.__axisPointerUpdate) {
		// the consumer may reset its state together with the new options - send the next update even when the axis pointer has not moved
		// (ECharts does not fire updateAxisPointer after setOption, the next one comes with the next pointer move)
		element.echart.__axisPointerUpdate.sentKey = null;
	}

	element.echart.setOption(optionsObject, { notMerge: true });
}

function handleUpdateAxisPointer(dotnetReference, chart, params) {
	// ECharts fires the event on every pointer move, even when the axis pointer stays at the same position.
	// Only a change is sent to .NET, at most once per animation frame and with at most one call in flight (the latest params win),
	// so the update arrives while the pointer is still moving (a debounce would postpone it until the pointer stops)
	// and .NET is not flooded with calls.
	const state = chart.__axisPointerUpdate;
	const key = JSON.stringify(params);
	if (!params.axesInfo?.length && (key !== state.pendingKey)) {
		// the pointer has just left the axes (or the chart) - the consumer may reset its state (e.g. in its own mouseleave handler),
		// the next position has to be sent even when it equals the one sent before leaving (the leave itself may be superseded before it is sent)
		state.sentKey = null;
	}
	state.pendingParams = params;
	state.pendingKey = key;
	scheduleAxisPointerUpdate(dotnetReference, state);
}

function scheduleAxisPointerUpdate(dotnetReference, state) {
	if (state.frame || state.inFlight || state.disposed) {
		return;
	}

	state.frame = requestAnimationFrame(() => {
		state.frame = 0;
		sendAxisPointerUpdate(dotnetReference, state);
	});
}

async function sendAxisPointerUpdate(dotnetReference, state) {
	if (state.pendingKey === state.sentKey) {
		return;
	}

	const callId = ++state.callId;
	state.sentKey = state.pendingKey;
	state.inFlight = true;
	// Blazor Server never completes a call lost together with the connection (e.g. during a reconnect),
	// waiting for it would block all the following updates - stop waiting after the timeout
	const timeout = setTimeout(() => {
		completeAxisPointerUpdateCall(dotnetReference, state, callId);
	}, AXIS_POINTER_CALL_TIMEOUT_MS);
	try {
		await dotnetReference.invokeMethodAsync('HandleAxisPointerUpdate', state.pendingParams);
	} finally {
		clearTimeout(timeout);
		completeAxisPointerUpdateCall(dotnetReference, state, callId);
	}
}

function completeAxisPointerUpdateCall(dotnetReference, state, callId) {
	if (!state.inFlight || (state.callId !== callId)) {
		// a call given up after the timeout has finished late - a newer call may already be in flight
		return;
	}

	state.inFlight = false;
	// the axis pointer moved while the call was in flight - send the latest position in the next frame
	if (state.pendingKey !== state.sentKey) {
		scheduleAxisPointerUpdate(dotnetReference, state);
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
	if (chart.__axisPointerUpdate) {
		// a call still in flight must not schedule another one when it finishes
		chart.__axisPointerUpdate.disposed = true;
		cancelAnimationFrame(chart.__axisPointerUpdate.frame);
	}
	chart.dispose();
	element.echart = null;
}