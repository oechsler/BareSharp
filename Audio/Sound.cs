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

        /// <summary>
        /// Occours once the Sounds playback starts.
        /// </summary>
		public event EventHandler<EventArgs> Played;
        /// <summary>
        /// Occours once the Sounds playback pauses.
        /// </summary>
		public event EventHandler<EventArgs> Paused;
        /// <summary>
        /// Occours once the Sounds playback stops.
        /// </summary>
		public event EventHandler<EventArgs> Stopped;

        /// <summary>
        /// Initializes a new instance of the Sound class.
        /// </summary>
        /// <param name="content">The content pipeline which the asset will be loaded from.</param>
		/// <param name="assetName">The within the content pipeline assigned name.</param>
		public Sound(ContentManager content, string assetName)
		{
			this.content = content;

			sound = content.Load<SoundEffect>(assetName).CreateInstance();
			state = sound.State;
		}

        /// <summary>
        /// Updates the state event system of the Sound.
        /// </summary>
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

        /// <summary>
        /// Starts the playback of the Sound.
        /// </summary>
		public Sound Play()
		{
			sound.Play();

			return this;
		}

        /// <summary>
        /// Pauses the playback of the Sound.
        /// </summary>
		public Sound Pause()
		{
			sound.Pause();

			return this;
		}

        /// <summary>
        /// Stops the playback of the Sound.
        /// </summary>
		public Sound Stop()
		{
			sound.Stop();

			return this;
		}

        /// <summary>
        /// Canges the attached sound asset.
        /// </summary>
        /// <param name="assetName">The within the content pipeline assigned name.</param>
		public Sound Change(string assetName)
		{
			if (sound.State != SoundState.Stopped)
				Stop();

			sound = content.Load<SoundEffect>(assetName).CreateInstance();

			return this;
		}

        /// <summary>
        /// Gets or sets the Sounds volume value.
        /// </summary>
		public float Volume
		{
			get { return sound.Volume; }
			set { sound.Volume = value; }
		}

        /// <summary>
        /// Gets or sets the Sounds pitch value.
        /// </summary>
        public float Pitch
		{
			get { return sound.Pitch; }
			set { sound.Pitch = value; }
		}

        /// <summary>
        /// Gets the value indicating whether the Sound is looped.
        /// </summary>
		public bool IsLooped
		{
			get { return sound.IsLooped; }
			set { sound.IsLooped = value; }
		}
	}
}
