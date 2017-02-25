using System.Collections.Generic;

namespace BareKit.Audio
{
    public class SoundManager
    {
        List<Sound> sounds;

        public SoundManager()
        {
			sounds = new List<Sound>();
        }

        public void Update()
        {
			foreach (Sound sound in sounds)
				sound.Update();
        }

		public SoundManager AddChild(Sound sound)
        {
			sounds.Add(sound);

			return this;
        }

		public SoundManager RemoveChild(Sound sound)
		{
			sounds.Remove(sound);

			return this;
		}

		public SoundManager Stop()
		{
			foreach (Sound sound in sounds)
				sound.Stop();

			return this;
		}
    }
}
