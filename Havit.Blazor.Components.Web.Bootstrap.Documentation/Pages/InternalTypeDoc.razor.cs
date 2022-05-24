using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Pages
{
	public partial class InternalTypeDoc
	{
		private static readonly Dictionary<string, Type> delegateTypes = new()
		{
			{ "AutosuggestDataProviderDelegate", typeof(AutosuggestDataProviderDelegate<>) },
			{ "GridDataProviderDelegate", typeof(GridDataProviderDelegate<>) },
			{ "InputTagsDataProviderDelegate", typeof(InputTagsDataProviderDelegate) },
			{ "CalendarDateCustomizationProviderDelegate", typeof(CalendarDateCustomizationProviderDelegate) }
		};

		[Parameter] public string TypeText { get; set; }
		[Parameter] public Type Type { get; set; }

		private bool isDelegate;

		protected override void OnParametersSet()
		{
			try
			{
				var typeInfo = GetType(TypeText);
				Type = typeInfo.type;
				isDelegate = typeInfo.isDelegate;
			}
			catch { }
			InvokeAsync(StateHasChanged);
		}

		public static bool DetermineIfTypeIsInternal(string typeText)
		{
			try
			{
				Type type = GetType(typeText).type;
				if (type is null)
				{
					return false;
				}
				return true;
			}
			catch
			{
				return false;
			}
		}

		private static (Type type, bool isDelegate) GetType(string typeName)
		{
			Type result;

			// Formatting typeName.
			int? openingBracePosition = GetOpeningBracePosition(typeName);
			if (openingBracePosition.HasValue)
			{
				typeName = typeName.Remove(openingBracePosition.Value, typeName.Length - openingBracePosition.Value);
			}

			// Handling delegate types, all other types are found by the Type.GetType() method.
			delegateTypes.TryGetValue(typeName, out result);
			if (result is not null)
			{
				return (result, true);
			}

			try
			{
				result = Type.GetType($"Havit.Blazor.Components.Web.Bootstrap.{typeName}, Havit.Blazor.Components.Web.Bootstrap");
				if (result is not null)
				{
					return (result, false);
				}
			}
			catch { }

			try
			{
				result = Type.GetType($"Havit.Blazor.Components.Web.{typeName}, Havit.Blazor.Components.Web");
				if (result is not null)
				{
					return (result, false);
				}
			}
			catch { }

			return (null, false);
		}

		private static int? GetOpeningBracePosition(string text)
		{
			for (int i = 0; i < text.Length; i++)
			{
				if (text[i] == '<')
				{
					return i;
				}
			}

			return null;
		}
	}
}
