using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public class ValueWrapper<TValue> : INotifyPropertyChanged
	{
		private readonly Func<TValue, string> titleSelector;
		private readonly Func<TValue, string> badgeSelector;
		private readonly Func<TValue, ThemeColor> badgeColorSelector;
		private readonly Func<TValue, string> iconSelector;
		private readonly Func<TValue, IEnumerable<TValue>> childrenSelector;
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
			Func<TValue, string> titleSelector,
			Func<TValue, string> badgeSelector,
			Func<TValue, ThemeColor> badgeColorSelector,
			Func<TValue, string> iconSelector,
			Func<TValue, IEnumerable<TValue>> childrenSelector,
			Action<ValueWrapper<TValue>> onItemSelected
		)
		{
			Value = value;
			Level = level;
			this.iconSelector = iconSelector;
			this.titleSelector = titleSelector;
			this.badgeSelector = badgeSelector;
			this.childrenSelector = childrenSelector;
			this.badgeColorSelector = badgeColorSelector;

			this.onItemSelected = onItemSelected;

			Console.WriteLine($@"ValueWrapper created {titleSelector(Value)}");
		}

		public TValue Value { get; }

		public string Title => title ??= titleSelector(Value);

		public BootstrapIcon Icon => icon ??= (GetBootstrapIconByName(iconSelector?.Invoke(Value)));

		public ThemeColor BadgeColor
		{
			get
			{
				if (badgeColor != null)
				{
					return badgeColor.Value;
				}

				if (badgeColorSelector == null)
				{
					return (badgeColor = ThemeColor.None).Value;
				}

				return (badgeColor = badgeColorSelector(Value)).Value;
			}
		}

		public string Badge => badge ??= badgeSelector?.Invoke(Value);

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
				Console.WriteLine($"{titleSelector(Value)} IsExpanded changed from {isExpanded} to {value}");
				isExpanded = value;
				Console.WriteLine($"{titleSelector(Value)} IsExpanded changed to {isExpanded}");
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

				children = childrenSelector == null
					? Array.Empty<ValueWrapper<TValue>>()
					: childrenSelector(Value)?.Select(value =>
							new ValueWrapper<TValue>(
								value,
								Level + 1,
								titleSelector,
								badgeSelector,
								badgeColorSelector,
								iconSelector,
								childrenSelector,
								onItemSelected))
						.ToArray();

				return children;
			}
		}

		public bool HasChildren => hasChildren ??= (childrenSelector != null && childrenSelector(Value)?.Any() == true);

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