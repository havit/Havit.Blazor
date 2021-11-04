using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Havit.Blazor.Components.Web.Bootstrap.TreeViews
{
	public class ValueWrapper<TValue> : INotifyPropertyChanged
	{
		private readonly Func<TValue, string> titleGetter;
		private readonly Func<TValue, string> badgeGetter;
		private readonly Func<TValue, ThemeColor> badgeColorGetter;
		private readonly Func<TValue, string> iconGetter;
		private readonly Func<TValue, IEnumerable<TValue>> childrenGetter;
		private readonly Action<ValueWrapper<TValue>> onItemSelected;

		private string badge;
		private string title;
		private bool? hasChildren;
		private BootstrapIcon icon;
		private ValueWrapper<TValue>[] children;
		private ThemeColor? badgeColor;
		private bool isSelected;
		private bool isExpanded;

		public event PropertyChangedEventHandler PropertyChanged;

		public ValueWrapper(TValue value,
			int level,
			Func<TValue, string> title,
			Func<TValue, string> badge,
			Func<TValue, ThemeColor> badgeColor,
			Func<TValue, string> icon,
			Func<TValue, IEnumerable<TValue>> children,
			Action<ValueWrapper<TValue>> onItemSelected
		)
		{
			Value = value;
			Level = level;
			iconGetter = icon;
			titleGetter = title;
			badgeGetter = badge;
			childrenGetter = children;
			badgeColorGetter = badgeColor;

			this.onItemSelected = onItemSelected;

			Console.WriteLine($@"ValueWrapper created {title(Value)}");
		}

		public TValue Value { get; }

		public string Title => title ??= titleGetter(Value);

		public BootstrapIcon Icon => icon ??= (GetBootstrapIconByName(iconGetter?.Invoke(Value)));

		public ThemeColor BadgeColor
		{
			get
			{
				if (badgeColor != null)
				{
					return badgeColor.Value;
				}

				if (badgeColorGetter == null)
				{
					return (badgeColor = ThemeColor.None).Value;
				}

				return (badgeColor = badgeColorGetter(Value)).Value;
			}
		}

		public string Badge => badge ??= badgeGetter?.Invoke(Value);

		public bool IsSelected
		{
			get => isSelected;
			set
			{
				if (isSelected == value)
				{
					return;
				}

				isSelected = value;
				if (isSelected)
				{
					onItemSelected(this);
				}

				OnPropertyChanged(nameof(IsSelected));
			}
		}

		public bool IsExpanded
		{
			get => isExpanded;
			set
			{
				Console.WriteLine($"{titleGetter(Value)} IsExpanded changed from {isExpanded} to {value}");
				isExpanded = value;
				Console.WriteLine($"{titleGetter(Value)} IsExpanded changed to {isExpanded}");
				OnPropertyChanged(nameof(IsExpanded));
			}
		}

		public int Level { get; set; }

		public ValueWrapper<TValue>[] Children
		{
			get
			{
				if (children != null)
				{
					return children;
				}

				children = childrenGetter == null
					? Array.Empty<ValueWrapper<TValue>>()
					: childrenGetter(Value)?.Select(value =>
							new ValueWrapper<TValue>(
								value,
								Level + 1,
								titleGetter,
								badgeGetter,
								badgeColorGetter,
								iconGetter,
								childrenGetter,
								onItemSelected))
						.ToArray();

				return children;
			}
		}

		public bool HasChildren => hasChildren ??= (childrenGetter != null && childrenGetter(Value)?.Any() == true);

		private BootstrapIcon GetBootstrapIconByName(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}

			var propertyIcon = typeof(BootstrapIcon).GetProperties()
				.FirstOrDefault(p => string.Compare(p.Name, name, StringComparison.OrdinalIgnoreCase) == 0);

			if (propertyIcon == null)
			{
				throw new ArgumentException($"Icon with name {name} doesn't exist.");
			}

			return propertyIcon.GetValue(null, null) as BootstrapIcon;
		}

		protected virtual void OnPropertyChanged(string propertyName = null)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

	}
}