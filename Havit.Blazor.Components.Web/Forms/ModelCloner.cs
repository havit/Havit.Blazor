namespace Havit.Blazor.Components.Web;

internal static class ModelCloner
{
	/// <summary>
	/// Returns a model clone.
	/// Uses strategies (in order):
	/// * <see cref="ICloneable" />
	/// * C# Records
	/// * Object.MemberwiseClone
	/// </summary>
	public static TModel Clone<TModel>(TModel model)
	{
		TModel modelClone;

		if (TryCloneCloneable(model, out modelClone))
		{
			return modelClone;
		}

		if (TryCloneRecord(model, out modelClone))
		{
			return modelClone;
		}

		return CloneMemberwiseClone(model);
	}

	internal static bool TryCloneCloneable<TModel>(TModel model, out TModel modelClone)
	{
		if (model is ICloneable)
		{
			object result = ((ICloneable)model).Clone();
			Contract.Assert(result is TModel, $"{typeof(TModel)}.Clone() must return a type of {typeof(TModel)}.");
			modelClone = (TModel)result;
			return true;
		}

		modelClone = default;
		return false;
	}

	internal static bool TryCloneRecord<TModel>(TModel model, out TModel modelClone)
	{
		System.Reflection.MethodInfo recordCloneMethod = typeof(TModel).GetMethod("<Clone>$");
		if (recordCloneMethod != null)
		{
			modelClone = (TModel)recordCloneMethod.Invoke(model, null);
			return true;
		}

		modelClone = default;
		return false;
	}

	internal static TModel CloneMemberwiseClone<TModel>(TModel model)
	{
		// https://github.com/force-net/DeepCloner also uses a MemberwiseClone method for shallow cloning.
		System.Reflection.MethodInfo memberwiseClone = typeof(TModel).GetMethod("MemberwiseClone", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
		return (TModel)memberwiseClone.Invoke(model, null);
	}

}
