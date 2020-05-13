using Havit.Blazor.Components.Web.Bootstrap.Filters;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap.Filters
{
    public partial class HxTextFilter
    {
        //public override IEnumerable<FilterChip> GetChips()
        //{
        //    yield return new FilterChip
        //    {
        //        //Chip = (Label + ": " + Value).ToRenderFragment(),
        //        RemoveCallback = RemoveChip
        //    };
        //}

        protected virtual Task RemoveChip()
        {
            Value = null; // nebo nějaký default?
            // TODO: OnChange?
            return Task.CompletedTask;
        }

        //public override RenderFragment GetFilter()
        //{
        //    return "HxStringFilter - Not implemented".ToRenderFragment();
        //}
    }
}
