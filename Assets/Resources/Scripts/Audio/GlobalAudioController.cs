using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace SubwaySpraypainter {

	// Handles global audio events like music, stings, and VO.
	public class GlobalAudioController : SingletonMonobehavior<GlobalAudioController> {

		// Reference to the master mixer.
		[SerializeField]
		private AudioMixer mixer;
		public static AudioMixer Mixer {
			get {
				return Instance.mixer;
			}
		}

		// List of global audio channels.
		[SerializeField]
		private AudioMixerGroup[] channels;

		// Dictionary of audio sources hashed against their audio mixer group.
		private Dictionary<string, AudioSource> sources = new Dictionary<string, AudioSource>();

		// Self-initialize this component.
		protected override void Awake() {
			base.Awake();

			// Create an audio channel for each ID.
			foreach(AudioMixerGroup channel in channels) {
				AudioSource source = gameObject.AddComponent<AudioSource>();
				source.outputAudioMixerGroup = channel;
				sources.Add(channel.name, source);
			}
		}

		// Play a sound in a channel.
		public static void Play(string channel, AudioClip sound) {
			Instance.sources[channel].PlayOneShot(sound);
		}

		// Transition to the specified snapshot name.
		public static void TransitionState(string snapshotName) {
			Mixer.FindSnapshot(snapshotName).TransitionTo(0.01f);
		}
	}
}