# Getting Started

For further information on the MonoGame framework, please do checkout their [documentation](http://www.monogame.net/documentation/?page=main).
<br />This guide assumes that you already know how to work with MonoGame's content pipeline.

### General setup

1. Import Bare's shared project into your Visual Studio solution.
2. Link the shared project with your executing MonoGame project.
3. Link a version of [MoonSharp](http://www.moonsharp.org) with your executing project (or define a `NOSCRIPT` preprocessor flag).

### Entrypoint

In order for Bare to run, you need to replace the default `Game1` class with Bare's Entrypoint. The Entrypoint can also be customized to fit your needs by overriding it. *It is highly recommended that you create your own Entrypoint, when using C# to code.*

~~~cs
using BareKit;

public class Main
{
	static Entrypoint entrypoint;

	public static void Main(string args[])
	{	
		entrypoint = new Entrypoint();
		entrypoint.Run();
	}	
}
~~~

### Scripting

*Bare uitilizes [MoonSharp](http://www.moonsharp.org) to implement the Lua scripting language. It is intended that you use Lua for scripting your games. But because Bare's API is written completly in C# and translated to Lua during runtime, you may use C# aswell for more advanced scripting or due to personal preference.*

* *Known caveat: the integrated [Glide](https://bitbucket.org/jacobalbano/glide) tweening engine is not accessible through Lua! ([use this instead](https://github.com/kikito/tween.lua))*

Userdefined Lua scripts by deafult are located in the `Scripts` subdirectory of your project and need to be marked as `Embedded resource` in Visual Studio. *For changing the default directory please have a look into Bare's Entrypoint class.*

To enable Lua scripting create a file in the `Scripting` directory, name it `main.lua` and mark it as `Embedded resource`.  Just add the following lines of code to it and you are done.

~~~lua
function bare.start()
	print("Ready.")
end
~~~

*When executing, this should print out* `Ready.` *in Visual Studio's output window.*

### Callback functions

###### bare.start

~~~lua
function bare.start()
	-- Creates a scene and navigates to it
	local scene = init(bare.scene)
	bare.stage.navigateTo(scene)
end
~~~

*This function gets called once, when Bare is started. This is usually where you load your scenes and setup things specific to your game. You may want to do these things somewhere else, but doing them here ensures that their only done once and therefore saving a lot of system resources.*

###### bare.update

~~~lua
function bare.update()
	-- Increments `number` by `1` per second.
	number = number + 1 * bare.delta
end
~~~

*This function is continoussly called before rendering a frame. Probably this is where all your logic is done.* `bare.delta` *is the so called delta time. It is the amount of time in seconds since the last time this function was called (usually a small value). Due to the event driven nature of the Bare framework (each scene has its own update event), the delta time can be accessed via a variable.*

###### bare.config

~~~lua
function bare.config(e)
	-- Sets the window size
	e.scaling = init(bare.scalingManager, e.graphics, e.window, init(bare.vector3, 950, 16, 9), 1)
	-- Disables rendering the cursor
	e.isMouseVisible = false
end
~~~

*This function is called before the window is created and therefore allows you to make a change to the default size of 720x506 in the way indicated above. Other things like for example mouse visibility are configurable aswell.*

### Standard library

Bare's Lua interpereter contains the complete set of standard functions. Beyond that there are a few important functions for registering and allocating objects from the underlaying C# context.

###### alloc

~~~lua 
-- definition
typedef = alloc(className, assemblyName)
-- example
vector2 = alloc('Microsoft.Xna.Framework.Vector2', 'MonoGame.Framework')
myClass = alloc('MyNamespace.MyClass', _DEFAULT)
~~~

*This function allocates C# classes to the Lua scripting context. When allocating from your executing assembly, you can simply use `_DEFAULT` instead of specifing the assembly name.*

###### dealloc

~~~lua
dealloc(typedef)
~~~
*This function is the counterpart to alloc (use this function carefully).*

###### init

~~~lua
-- definition
instance = init(typedef, arg1, arg2, ...)
-- example
scale = init(vector2, 2)
scale.x = 2.5
print('Current scale: ' .. scale.toString())
~~~

*This function instanciates C# objects inside the Lua scripting context. Methodes and fields of the respective objects can be accessed the same way as in C#, but always need to start lowercase. To initiate garbage collection of an instance simply set its userdata reference to a `nil` value.*

###### enum

~~~lua
-- definituion
reference = enum(typedef)
-- example
background = enum(color).cornflowerBlue
~~~

*This function references C# enums inside the Lua scripting context.*

###### Drawables

~~~lua
bare.container        -- Container drawable.
bare.label            -- Text-label drawable.
bare.rect             -- Rectangular drawable.
bare.scene            -- Scene for rendering drawables.
bare.sprite           -- Sprite drawable.
~~~                           

###### Interfaces

~~~lua
bare.database         -- Interface for storing persistent values.
bare.shader           -- Interface for using shaders.
bare.sound            -- Interface for playing sounds.
~~~

###### Inputs

~~~lua
bare.gamepadInput     -- Interface for handling gamepad input.
bare.keyInput         -- Interface for handling keyboard input.
bare.touchInput       -- Interface for handling touch/mouse input.
~~~

###### Datatypes

~~~lua
bare.color            -- Three/Four dimensional color vector.
bare.rotatedRectangle -- Rotatable, rectangular shape.
bare.timeSpan         -- System.TimeSpan.
bare.vector2          -- Two dimensional vector.
bare.vector3          -- Three dimensional vector.
~~~

###### Enums

~~~lua
bare.blendMode        -- Enums for different color blend modes.
bare.finger           -- Enums for different fingers.
bare.inputState       -- Enums for different input device states.
bare.keys             -- Enums for different keys.
~~~

*For furter information on how each of the modules work, please have a look at it's respective class definition. Each method and field of the module has a short documentation attached to it.*

### Examples

###### Displaying a sprite

*The sprite already has to be imported into MonoGame's content pipline. Also make sure that your sprites and fonts have a scale definition assigned. This is done by adding a `_nx` suffix to the filename. So if you have got a sprite called `banana.png`, the name for the standard size would be `banana_1x.png`. The Scaling sizes have to be of the integer type and a standard size must always be provided.*

##### main.lua
~~~lua 
local scene = require "scene.lua"

function bare.start()
	bare.stage.navigateTo(scene)
end
~~~

##### scene.lua
~~~lua 
local scene = init(bare.scene)
local sprite = nil

scene.entered.add(function(self)
	sprite = init(bare.sprite, self.content, "banana")
	self.addChild(sprite)
end)

return scene
~~~

###### Handling input

##### scene.lua
~~~lua
local scene = init(bare.scene)
local keyEvent = nil

scene.entered.add(function(self)
	keyEvent = init(bare.keyInput, enum(bare.inputState).pressed, enum(bare.keys).k)
	keyEvent.triggered.add(function(sender)
		print(sender .. " triggered the event.")
	end)
	self.input.addChild(keyEvent)
end)

return scene
~~~

###### Playing a sound

*The sound already has to be imported into MonoGame's content pipline. Also make sure that you have `SoundEffect` selected as importer, otherwise the sound will not be playable (and an exeption will be thrown).*

##### scene.lua
~~~lua
local scene = init(bare.scene)
local sound = nil

scene.entered.add(function(self)
	sound = init(bare.sound, self.content, "banana_squash")
	self.sound.addChild(sound.play())
end)

return scene
~~~

### License

*Copyright © 2017 – Samuel Oechsler*

**Modified version of Zlib.** 
*This software is provided 'as-is', without any express or implied warranty. In no event will the authors be held liable for any damages arising from the use of this software. Permission is granted to anyone to use this software for any purpose, including commercial applications, and to alter it and redistribute it freely, subject to the following restrictions: The origin of this software must not be misrepresented; you must not claim that you wrote the original software. If you use this software in a comercial product, an acknowledgment in the product documentation is required. Altered source versions must be plainly marked as such, and must not be misrepresented as being the original software. This notice may not be removed or altered from any source distribution.*
