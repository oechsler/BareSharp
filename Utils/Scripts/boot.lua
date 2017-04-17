-- mscorlib.dll
alloc('System.Type', 'mscorlib')
bare.timeSpan = alloc('System.TimeSpan', 'mscorlib')
if bare.timeSpan == nil then
	alloc('System.Type', 'System.Runtime')
	bare.timeSpan = alloc('System.TimeSpan', 'System.Runtime')
end

-- MonoGame.Framework.dll
alloc('Microsoft.Xna.Framework.Game', 'MonoGame.Framework')
alloc('Microsoft.Xna.Framework.GraphicsDeviceManager', 'MonoGame.Framework')
alloc('Microsoft.Xna.Framework.GameWindow', 'MonoGame.Framework')
bare.vector2 = alloc('Microsoft.Xna.Framework.Vector2', 'MonoGame.Framework')
bare.vector3 = alloc('Microsoft.Xna.Framework.Vector3', 'MonoGame.Framework')
bare.color = alloc('Microsoft.Xna.Framework.Color', 'MonoGame.Framework')
bare.blendMode = alloc('Microsoft.Xna.Framework.Graphics.BlendState', 'MonoGame.Framework')
alloc('Microsoft.Xna.Framework.Graphics.Effect', 'MonoGame.Framework')
alloc('Microsoft.Xna.Framework.Content.ContentManager', 'MonoGame.Framework')
bare.rotatedRectangle = alloc('Microsoft.Xna.Framework.RotatedRectangle', _DEFAULT)

-- BareKit
alloc('BareKit.Storage', _DEFAULT)
bare.database = alloc('BareKit.Database', _DEFAULT)

-- BareKit.Audio
bare.sound = alloc('BareKit.Audio.Sound', _DEFAULT)
alloc('BareKit.Audio.SoundManager', _DEFAULT)

-- BareKit.Graphics
bare.container = alloc('BareKit.Graphics.Container', _DEFAULT)
alloc('BareKit.Graphics.Drawable', _DEFAULT)
bare.label = alloc('BareKit.Graphics.Label', _DEFAULT)
bare.rect = alloc('BareKit.Graphics.Rect', _DEFAULT)
alloc('BareKit.Graphics.ScalingManager', _DEFAULT)
bare.shader = alloc('BareKit.Graphics.Shader', _DEFAULT)
bare.scene = alloc('BareKit.Graphics.Scene', _DEFAULT)
bare.sprite = alloc('BareKit.Graphics.Sprite', _DEFAULT)

-- BareKit.Input
bare.gamepadInput = alloc('BareKit.Input.GamepadInput', _DEFAULT)
bare.buttons = alloc('Microsoft.Xna.Framework.Input.Buttons', 'MonoGame.Framework')
alloc('BareKit.Input.Input', _DEFAULT)
bare.inputState = alloc('BareKit.Input.InputState', _DEFAULT)
alloc('BareKit.Input.InputManager', _DEFAULT)
bare.keyInput = alloc('BareKit.Input.KeyInput', _DEFAULT)
bare.keys = alloc('Microsoft.Xna.Framework.Input.Keys', 'MonoGame.Framework')
bare.touchInput = alloc('BareKit.Input.TouchInput', _DEFAULT)
bare.finger = alloc('BareKit.Input.Finger', _DEFAULT)

-- Load main.lua
require("main")