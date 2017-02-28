#Getting Started

For further information on how MonoGame works, please checkout their online [documentation](http://www.monogame.net/documentation/?page=main).

To begin, you want to create a custom Entypoint for your game. Bares Entrypoint is what is known as the Game class in MonoGame.

~~~csharp
using BareKit;

public sealed class AppEntrypoint : Entrypoint
{
	public override Initialize()
	{
		base.Initialize();
	}
}
~~~

The Entrypoint holds all systems needed for the Bare framework to work properly. Simply go ahead and run your application. You should end up seeing an empty 
window with dark-grey background.

![empty_window](./Assets/empty_window.tiff)

Although the window seems perfect, the resolution borthers you, right? No problem! Simply change the default ScalingManager via the Scaling field. *It is necessary that this is done before calling the base class Initialize method of your Entrypoint.*

~~~csharp
Scaling = new ScalingManager(graphics, window, size, [scale], [fullscreen]);
~~~

The size vector is of type Vecor3. Because it is used to determine the siez of a window, you might wonder why we are not using a Vector2. This is because we are using the. first argument to determine the width of the window and the second and third for the aspect ratio. 

The correct vector for an 800x600 window therefor would be:

~~~csharp
Vector3 size = new Vector3(800, 4, 3);
~~~

Now that your window is perfectly setup you might wonder how to actually display something. Now to understand how this is done you first must know how Bares display system works (it is super easy to get by the way). Imagine you being the director of a sceneplay. You are given a Stage to perform your play on. You as the director would move the Scenes into place at tell your actors what to do in each of them. Its actually the same in Bare.

~~~csharp
using BareKit.Graphics;

public sealed class MainScene : Scene
{
	public override void Enter(Scene from)
	{
		base.Enter(from);
	}
}
~~~

Now that you have got your Scene class, you can add Drawables like Sprites, Labels and Primitives to display them on screen.

This is what you have to do in order to display a snowball sprite image named "snowball_1x.png" from MonoGames content pipeline:

~~~csharp
using BareKit.Graphics;

public class MainScene : Scene
{
	Sprite snowball;

	protected override void LoadContent()
	{
		base.LoadContent();

		snowball = new Sprite(Content, "snowball");
	}

	public override void Enter(Scene from)
	{
		base.Enter(from);

		AddChild(snowball);
	}
}
~~~

*You also might wonder why you have to add a "\_1x" suffix to our image name in MonoGames content pipeline. This is because Bare chooses your Drawables resolution automaticaly based on your canvas size. A "\_2x" suffix would therefor simply mean that this image it twice as big as the original one. Having at least a "\_1x" file is required.*

But still you will not see any snowball displayed on screen, why? Now this is because you have not added your Scene to the Navigation stack of the Stage yet. So lets do this right away. In your Entrypoints Initialize method call a navigation to your Scenes type.

~~~csharp
public override Initialize()
{
	base.Initialize();
	
	Stage.NavigateTo(typeof(MainScene));
}
~~~

Once you build and run the application agian, you will see your snowball Sprite displayed right in the center of the window.

![window_snowball](./Assets/window_snowball.tiff)



