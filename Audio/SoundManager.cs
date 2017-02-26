using System.Collections.Generic;

namespace BareKit.Audio
{
    public class SoundManager
    {
        List<Sound> sounds;

        /// <summary>
        /// Initializes a new instance of the SoundManager class.
        /// </summary>
        public SoundManager()
        {
			sounds = new List<Sound>();
        }

        /// <summary>
        /// Sends an updatecall to the Sounds.
        /// </summary>
        public void Update()
        {
			foreach (Sound sound in sounds)
				sound.Update();
        }

        /// <summary>
        /// Adds a child Sound to the SoundManager.
        /// </summary>
        /// <param name="child">The child Sound being added.</param>
		public SoundManager AddChild(Sound sound)
        {
			sounds.Add(sound);

			return this;
        }

        /// <summary>
        /// Removes a child Sound to the SoundManager.
        /// </summary>
        /// <param name="child">The child Sound being removed.</param>
		public SoundManager RemoveChild(Sound sound)
		{
			sounds.Remove(sound);

			return this;
		}

        /// <summary>
        /// Stops all playing sounds.
        /// </summary>
		public SoundManager Stop()
		{
			foreach (Sound sound in sounds)
				sound.Stop();

			return this;
		}
    }
}
