namespace Havit.Blazor.Components.Web.Bootstrap;

[Flags]
public enum MessageBoxButtons
{
#pragma warning disable format
	None				= 0,
	Ok					= 0b_0000_0001,
	Cancel				= 0b_0000_0010,
	OkCancel			= 0b_0000_0011,
	Abort				= 0b_0000_0100,
	Retry				= 0b_0000_1000,
	RetryCancel			= 0b_0000_1010,
	Ignore				= 0b_0001_0000,
	AbortRetryIgnore	= 0b_0001_1100, 
	Yes					= 0b_0010_0000,
	No					= 0b_0100_0000,
	YesNo				= 0b_0110_0000,
	Custom				= 0b_1000_0000,
	CustomCancel		= 0b_1000_0010
#pragma warning restore format
}