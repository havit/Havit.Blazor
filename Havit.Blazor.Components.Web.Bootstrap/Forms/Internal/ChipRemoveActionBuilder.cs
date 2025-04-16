using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

/// <summary>
/// ChipRemoveActionBuilder is responsible for generating an Action that resets
/// a property referenced by a lambda expression to a specified value (typically default).
/// 
/// Unlike most expression-based tools, this builder defers all expression analysis to the point
/// of action invocation. This enables fast action construction (suitable for per-render use)
/// and evaluates constraints and assumptions only when the action is actually executed.
/// </summary>
public class ChipRemoveActionBuilder
{
	private readonly LambdaExpression _valueExpression;
	private readonly object _resetValue;

	public ChipRemoveActionBuilder(LambdaExpression valueExpression, object resetValue = null)
	{
		ArgumentNullException.ThrowIfNull(valueExpression);
		_valueExpression = valueExpression;
		_resetValue = resetValue;
	}

	public Action<object> Build()
	{
		var valueExpression = _valueExpression;
		var resetValue = _resetValue;

		return (object modelInstance) =>
		{
			ArgumentNullException.ThrowIfNull(modelInstance);

			var accessChain = new List<MemberInfo>();
			Expression current = valueExpression.Body;

			while (current is MemberExpression memberExpr)
			{
				accessChain.Insert(0, memberExpr.Member);
				current = memberExpr.Expression;
			}

			if (accessChain.Count == 0)
			{
				throw new InvalidOperationException("Failed to execute chip removal action. Expression does not contain a valid member chain.\nExpression: " + valueExpression);
			}

			// Find the first member whose DeclaringType is assignable from modelInstance
			int startIndex = -1;
			Type modelType = modelInstance.GetType();
			for (int i = 0; i < accessChain.Count; i++)
			{
				var declaringType = accessChain[i].DeclaringType;
				if (declaringType != null && declaringType.IsAssignableFrom(modelType))
				{
					startIndex = i;
					break;
				}
			}

			if (startIndex == -1)
			{
				throw new InvalidOperationException("Failed to execute chip removal action. No member in the expression matches the model instance type.\nExpression: " + valueExpression);
			}

			object currentObject = modelInstance;
			for (int i = startIndex; i < accessChain.Count - 1; i++)
			{
				var member = accessChain[i];
				currentObject = member switch
				{
					PropertyInfo prop => prop.GetValue(currentObject),
					FieldInfo field => field.GetValue(currentObject),
					_ => throw new NotSupportedException("Failed to execute chip removal action. Unsupported member type in path.\nExpression: " + valueExpression)
				};

				if (currentObject == null)
				{
					throw new NullReferenceException("Failed to execute chip removal action. One of the intermediate objects is null.\nExpression: " + valueExpression);
				}
			}

			if (accessChain[^1] is not PropertyInfo propertyInfo)
			{
				throw new NotSupportedException("Failed to execute chip removal action. Final member in expression must be a property.\nExpression: " + valueExpression);
			}

			propertyInfo.SetValue(currentObject, resetValue);
		};
	}
}
