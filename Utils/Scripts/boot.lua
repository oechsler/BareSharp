local _MSCORLIB = "mscorlib"
local _RUNTIME = "System.Runtime"
local _MONOGAME = "MonoGame.Framework"
local _IMPORT = {}

function _IMPORT.core()
	-- Allocate core resources on ".Net" platforms
	alloc("System.IO.Stream", _MSCORLIB)
	alloc("System.Type", _MSCORLIB)

	bare.streamReader = alloc("System.IO.StreamReader", _MSCORLIB)
	bare.streamWriter = alloc("System.IO.StreamWriter", _MSCORLIB)
	bare.timeSpan = alloc("System.TimeSpan", _MSCORLIB)

	-- Allocate core resources on ".Net Core" platforms
	if not bare.timeSpan then
		alloc("System.IO.Stream", _RUNTIME)
		alloc("System.Type", _RUNTIME)

		bare.streamReader = alloc("System.IO.StreamReader", _RUNTIME)
		bare.streamWriter = alloc("System.IO.StreamWriter", _RUNTIME)
		bare.timeSpan = alloc("System.TimeSpan", _RUNTIME)
	end

	-- Allocate core modules from MonoGame.Framework
	alloc("Microsoft.Xna.Framework.Content.ContentManager", _MONOGAME)
	alloc("Microsoft.Xna.Framework.Game", _MONOGAME)
	alloc("Microsoft.Xna.Framework.GameWindow", _MONOGAME)
	alloc("Microsoft.Xna.Framework.Graphics.Effect", _MONOGAME)
	alloc("Microsoft.Xna.Framework.GraphicsDeviceManager", _MONOGAME)

	bare.blendMode = alloc("Microsoft.Xna.Framework.Graphics.BlendState", _MONOGAME)
	bare.color = alloc("Microsoft.Xna.Framework.Color", _MONOGAME)
	bare.rectangle = alloc("Microsoft.Xna.Framework.Rectangle", _MONOGAME)
	bare.rotatedRectangle = alloc("Microsoft.Xna.Framework.RotatedRectangle", _DEFAULT)
	bare.vector2 = alloc("Microsoft.Xna.Framework.Vector2", _MONOGAME)
	bare.vector3 = alloc("Microsoft.Xna.Framework.Vector3", _MONOGAME)
end

function _IMPORT.base()
	-- Allocate the base modules from BareKit
	bare.database = alloc("BareKit.Database", _DEFAULT)
	bare.storage = alloc("BareKit.Storage", _DEFAULT)
end

function _IMPORT.audio()
	-- Allocate the audio modules from BareKit.Audio
	alloc("BareKit.Audio.SoundManager", _DEFAULT)	

	bare.sound = alloc("BareKit.Audio.Sound", _DEFAULT)
end

function _IMPORT.graphics()
	--Allocate the graphics modules from BareKit.Graphics
	alloc("BareKit.Graphics.Drawable", _DEFAULT)

	bare.container = alloc("BareKit.Graphics.Container", _DEFAULT)
	bare.label = alloc("BareKit.Graphics.Label", _DEFAULT)
	bare.rect = alloc("BareKit.Graphics.Rect", _DEFAULT)
	bare.scalingManager = alloc("BareKit.Graphics.ScalingManager", _DEFAULT)
	bare.scene = alloc("BareKit.Graphics.Scene", _DEFAULT)
	bare.shader = alloc("BareKit.Graphics.Shader", _DEFAULT)
	bare.sprite = alloc("BareKit.Graphics.Sprite", _DEFAULT)
end

function _IMPORT.input()
	-- Allocate the input modules from BareKit.Input
	alloc("BareKit.Input.Input", _DEFAULT)
	alloc("BareKit.Input.InputManager", _DEFAULT)

	bare.buttons = alloc("Microsoft.Xna.Framework.Input.Buttons", _MONOGAME)
	bare.finger = alloc("BareKit.Input.Finger", _DEFAULT)
	bare.gamepadInput = alloc("BareKit.Input.GamepadInput", _DEFAULT)
	bare.inputState = alloc("BareKit.Input.InputState", _DEFAULT)
	bare.keyInput = alloc("BareKit.Input.KeyInput", _DEFAULT)
	bare.keys = alloc("Microsoft.Xna.Framework.Input.Keys", _MONOGAME)
	bare.touchInput = alloc("BareKit.Input.TouchInput", _DEFAULT)
end

function _IMPORT.embedded()
	-- Allocate the embedded libraries from BareKit.Utils.Scripts
	class = require(_DEFAULT .. ".Utils.Scripts.30log", true)
	tween = require(_DEFAULT .. ".Utils.Scripts.tween", true)
end

-- Load each module from the import list
for _, module in pairs(_IMPORT) do
	module()
end

-- Require the main module
require("main")