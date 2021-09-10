using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Havit.Blazor.Components.Web
{
	/// <summary>
	/// Extension methods for installation of <see cref="IHxMessageBoxService"/> support.
	/// </summary>
	public static class MessageBoxServiceCollectionExtensions
	{
		/// <summary>
		/// Adds <see cref="IHxMessageBoxService"/> support to be able to display message boxes using HxMessageBoxHost.
		/// </summary>
		public static IServiceCollection AddHxMessageBoxHost(this IServiceCollection services)
		{
			return services.AddScoped<IHxMessageBoxService, HxMessageBoxService>();
		}
	}
}
