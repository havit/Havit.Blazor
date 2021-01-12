using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Havit.Blazor.Components.Web.Messenger
{
	/// <summary>
	/// Extension methods for installation of HxMessenger support.
	/// </summary>
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// Adds <see cref="IHxMessengerService"/> support to be able to add messages to HxMessenger.
		/// </summary>
		public static IServiceCollection AddHxMessenger(this IServiceCollection services)
		{
			return services.AddScoped<IHxMessengerService, HxMessengerService>();
		}
	}
}
