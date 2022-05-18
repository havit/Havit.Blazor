namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// <see href="https://getbootstrap.com/docs/5.1/components/dropdowns/">Bootstrap 5 Dropdown</see> component.
	/// </summary>
	public partial class HxDropdown : IDropdownContainer
	{
		[Parameter] public DropdownDirection Direction { get; set; }

		/// <summary>
		/// Set <c>true</c> to create a <see href="https://getbootstrap.com/docs/5.1/components/dropdowns/#split-button">split dropdown</see> (using a <c>btn-group</c>).
		/// </summary>
		[Parameter] public bool Split { get; set; }

		/// <summary>
		/// By default, the dropdown menu is closed when clicking inside or outside the dropdown menu (<see cref="DropdownAutoClose.True"/>).
		/// You can use the AutoClose parameter to change this behavior of the dropdown.
		/// <see href="https://getbootstrap.com/docs/5.1/components/dropdowns/#auto-close-behavior">https://getbootstrap.com/docs/5.1/components/dropdowns/#auto-close-behavior</see>.
		/// </summary>
		[Parameter] public DropdownAutoClose AutoClose { get; set; } = DropdownAutoClose.True;

		/// <summary>
		/// Any additional CSS class to apply.
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		/// <summary>
		/// Additional attributes to be splatted onto an underlying <c>div</c> element.
		/// </summary>
		[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

		[Parameter] public RenderFragment ChildContent { get; set; }

		[CascadingParameter] protected HxNavbar NavbarContainer { get; set; }

		bool IDropdownContainer.IsOpen { get; set; }

		protected string GetDropdownDirectionCssClass()
		{
			return this.Direction switch
			{
				DropdownDirection.Down => "dropdown",
				DropdownDirection.Up => "dropup",
				DropdownDirection.Start => "dropstart",
				DropdownDirection.End => "dropend",
				_ => throw new InvalidOperationException($"Unknown {nameof(DropdownDirection)} value {Direction}.")
			};
		}

		protected string GetCssClass()
		{
			/*
				.btn-group
				The basic.dropdown class brings just position:relative requirement(+ directions within other variations).
				.btn-group is needed for Split buttons AND brings default in-row positioning to behave like regular buttons.
				It is used in almost all Bootstrap samples. Might be replaced with more appropriate class if needed.
				.btn-group cannot be used in Navbar as it breaks responsiveness.
			*/
			return CssClassHelper.Combine(
				GetDropdownDirectionCssClass(),
				((this.NavbarContainer is null) || this.Split) ? "btn-group" : null,
				this.CssClass);
		}
	}
}
