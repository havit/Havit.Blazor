﻿using Havit.Blazor.Components.Web.Infrastructure;
using Microsoft.AspNetCore.Components.Routing;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// <see href="https://getbootstrap.com/docs/5.3/components/navs-tabs/">Bootstrap nav-link</see> component.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxNavLink#HxNavLink">https://havit.blazor.eu/components/HxNavLink#HxNavLink</see>
/// </summary>
public partial class HxNavLink : ICascadeEnabledComponent
{
	[CascadingParameter] protected FormState FormState { get; set; }
	FormState ICascadeEnabledComponent.FormState { get => FormState; set => FormState = value; }

	/// <summary>
	/// Navigation target.
	/// </summary>
	[Parameter] public string Href { get; set; }

	/// <summary>
	/// Text of the item.
	/// </summary>
	[Parameter] public string Text { get; set; }

	/// <summary>
	/// URL matching behavior for the underlying <see cref="NavLink"/>.
	/// Default is <see cref="NavLinkMatch.Prefix"/>.
	/// You can set the value to <c>null</c> to disable the matching.
	/// </summary>
	[Parameter] public NavLinkMatch? Match { get; set; } = NavLinkMatch.Prefix;

	/// <summary>
	/// Raised when the item is clicked.
	/// </summary>
	[Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
	/// <summary>
	/// Triggers the <see cref="OnClick"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnClickAsync(MouseEventArgs args) => OnClick.InvokeAsync(args);

	/// <summary>
	/// Stops event propagation when the item is clicked. Default is <c>null</c>, which means <c>true</c> when <see cref="OnClick"/> is set.
	/// </summary>
	[Parameter] public bool? OnClickStopPropagation { get; set; }

	/// <summary>
	/// Prevents the default action for the onclick event. Default is <c>null</c>, which means <c>true</c> when <see cref="OnClick"/> is set.
	/// </summary>
	[Parameter] public bool? OnClickPreventDefault { get; set; }

	/// <summary>
	/// Additional CSS class.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	/// <inheritdoc cref="ICascadeEnabledComponent.Enabled" />
	[Parameter] public bool? Enabled { get; set; }

	/// <summary>
	/// Content.
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }

	/// <summary>
	/// Additional attributes to be splatted onto an underlying <see cref="NavLink"/> component.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }
}
