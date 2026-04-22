# HxMarkdown_Demo_CodeBlocks.razor

```razor
<HxMarkdown Content="@_markdown" />

@code {
	private string _markdown = """
		```csharp
		public class HelloWorld
		{
		    public static void Main()
		    {
		        Console.WriteLine("Hello, World!");
		    }
		}
		```

		Inline code: `var x = 42;`
		""";
}

```
