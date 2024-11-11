namespace Havit.Blazor.Components.Web.Bootstrap;

public interface IHxMessageBoxService
{
	Task<MessageBoxButtons> ShowAsync(MessageBoxRequest request);

	Func<MessageBoxRequest, Task<MessageBoxButtons>> OnShowAsync { get; set; }
}
