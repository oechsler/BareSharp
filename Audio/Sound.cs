using System;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace BareKit.Audio
{
	public class Sound
	{
		ContentManager content;

		SoundEffectInstance sound;
		SoundState state;

		public event EventHandler<EventArgs> Played;
		public event EventHandler<EventArgs> Paused;
		public event EventHandler<EventArgs> Stopped;

		public Sound(ContentManager content, string assetName)
		{
			this.content = content;

			sound = content.Load<SoundEffect>(assetName).CreateInstance();
			state = sound.State;
		}

		public void Update()
		{
			if (sound.State != state)
			{
				switch (sound.State)
				{
					case SoundState.Playing:
						Played?.Invoke(this, EventArgs.Empty);
						break;
					case SoundState.Paused:
						Paused?.Invoke(this, EventArgs.Empty);
						break;
					case SoundState.Stopped:
						Stopped?.Invoke(this, EventArgs.Empty);
						break;
				}

				state = sound.State;
			}
		}

		public Sound Play()
		{
			sound.Play();

			return this;
		}

		public Sound Pause()
		{
			sound.Pause();

			return this;
		}

		public Sound Stop()
		{
			sound.Stop();

			return this;
		}

		public Sound Change(string assetName)
		{
			if (sound.State != SoundState.Stopped)
				Stop();

			sound = content.Load<SoundEffect>(assetName).CreateInstance();

			return this;
		}

		public float Volume
		{
			get { return sound.Volume; }
			set { sound.Volume = value; }
		}

		public float Pitch
		{
			get { return sound.Pitch; }
			set { sound.Pitch = value; }
		}

		public bool Loop
		{
			get { return sound.IsLooped; }
			set { sound.IsLooped = value; }
		}
	}
}
