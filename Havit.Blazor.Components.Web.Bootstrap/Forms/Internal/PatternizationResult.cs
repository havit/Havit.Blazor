namespace Havit.Blazor.Components.Web.Bootstrap.Forms.Internal
{
	// source: https://www.codeproject.com/Articles/1135909/String-parsing-with-custom-patterns

	internal class PatternizationResult
	{
		#region Constructor

		public PatternizationResult(Exception exception)
			: this(string.Empty, new Dictionary<string, object>(), exception)
		{
		}

		public PatternizationResult(string pattern, Dictionary<string, object> result)
			: this(pattern, result, null)
		{
		}

		public PatternizationResult(string pattern, Dictionary<string, object> result, Exception exception)
		{
			Pattern = pattern;
			Result = result;
			Exception = exception;
		}

		#endregion

		#region Properties

		public string Pattern
		{
			get;
			private set;
		}

		public Dictionary<string, object> Result
		{
			get;
			private set;
		}

		public Exception Exception
		{
			get;
			private set;
		}

		#endregion

		#region Public methods

		public bool MarkerHasValue(string marker)
		{
			return Result.ContainsKey(marker);
		}

		public TValue GetMarkerValue<TValue>(string marker)
		{
			return (TValue)Result[marker];
		}

		#endregion
	}
}
