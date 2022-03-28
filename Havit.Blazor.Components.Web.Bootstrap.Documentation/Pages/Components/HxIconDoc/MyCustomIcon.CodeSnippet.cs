public class MyCustomIcon : IconBase
{
	public override Type RendererComponentType => typeof(MyCustomIconRenderer);

	public string IconCharacter { get; set; }

	public static MyCustomIcon Poo => new MyCustomIcon { IconCharacter = "💩" };
	public static MyCustomIcon ThumbUp => new MyCustomIcon { IconCharacter = "👍" };
}