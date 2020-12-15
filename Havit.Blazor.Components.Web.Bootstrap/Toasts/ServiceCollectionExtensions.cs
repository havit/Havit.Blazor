using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Havit.Blazor.Components.Web.Bootstrap
{
    public static class ServiceCollectionExtensions
    {
		/// <summary>
		/// Adds <see cref="IMessenger"/> support to be able to add messages to <see cref="HxMessenger"/>.
		/// </summary>
		public static IServiceCollection AddHxMessenger(this IServiceCollection services)
		{
			return services.AddScoped<IMessenger, Messenger>();
		}
	}
}
