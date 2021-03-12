using UnityEngine.Audio;
using System;
using UnityEngine;

namespace Pokemon
{
    public class SoundPlayer : MonoBehaviour
    {
        // Start is called before the first frame update
        public static SoundPlayer Instance = null;
        public Sound[] Soundtrack;
        public Sound[] Effectstrack;
        public Sound SongPlaying;
        public Sound EffectPlaying;

        private string prevLocation = "Pallet Town";

        bool song_playing;

        public void Awake()
        {

            // If there is not already an instance of SoundManager, set it to this.
            if (Instance == null)
            {
                Instance = this;

            }
            //If an instance already exists, destroy whatever this object is to enforce the singleton.
            else if (Instance != this)
            {
                Destroy(gameObject);
                return;

            }

            //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
            DontDestroyOnLoad(gameObject);


            //allows us to view a list of songs in unity to attach the music to
            foreach (Sound s in Soundtrack)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = 1;
                s.source.pitch = 1;
                s.source.loop = true;
            }
            foreach (Sound e in Effectstrack)
            {
                e.source = gameObject.AddComponent<AudioSource>();
                e.source.clip = e.clip;
                e.source.volume = e.volume;
                e.source.pitch = e.pitch;
                e.source.loop = false;
            }

            //at start of game, play Pallet Town
            Play_New_Song("Pallet Town");
        }

        private void Update()
        {
            if (prevLocation != GameController.location)
            {
                prevLocation = GameController.location;
                FindObjectOfType<SoundPlayer>().StopSong();

                switch (GameController.location)
                {
                    case "Route 1":
                        FindObjectOfType<SoundPlayer>().Play_New_Song("Route 1");
                        break;
                    case "Pallet Town":
                        FindObjectOfType<SoundPlayer>().Play_New_Song("Pallet Town");
                        break;
                    default:
                        FindObjectOfType<SoundPlayer>().Play_New_Song("Pallet Town");
                        break;
                }
            }
            if (GameController.state == GameState.TrainerEncounter)
            {
                FindObjectOfType<SoundPlayer>().StopSong();

                FindObjectOfType<SoundPlayer>().Play_New_Song("Encounter Trainer");
                //FindObjectOfType<SoundPlayer>().T
            }
            if (GameController.startCombatMusic == true)
            {
                FindObjectOfType<SoundPlayer>().StopSong();

                if (GameController.isCatchable == true)
                    FindObjectOfType<SoundPlayer>().Play_New_Song("Battle Wild");
                else
                    FindObjectOfType<SoundPlayer>().Play_New_Song("Battle Trainer");

                GameController.startCombatMusic = false;
            }
            if (GameController.endCombatMusic == true)
            {
                FindObjectOfType<SoundPlayer>().StopSong();
                FindObjectOfType<SoundPlayer>().Play_New_Song(prevLocation);
                GameController.endCombatMusic = false;
            }
            if (Input.GetKeyDown("space"))
            {
                if (song_playing)
                    PauseSong();
                else
                    Play_Current_Song();

            }
        }
        public void Play_New_Song(string name)
        {

            SongPlaying = Array.Find(Soundtrack, Sound => Sound.name == name);
            if (SongPlaying == null)
            {
                Debug.LogWarning("Sound: " + name + " not found");
                return;
            }

            SongPlaying.source.Play();
            song_playing = true;
        }
        public void Play_Current_Song()
        {
            if (SongPlaying == null)
            {
                Debug.LogWarning("Sound: " + name + " not found");
                return;
            }

            song_playing = true;
            SongPlaying.source.Play();


        }
        public void PauseSong()
        {
            if (SongPlaying == null)
                return;

            song_playing = false;
            SongPlaying.source.Pause();
        }
        public void StopSong()
        {
            if (SongPlaying == null)
                return;

            song_playing = false;
            SongPlaying.source.Stop();
        }

        public void PlayEffect(string name)
        {
            EffectPlaying = Array.Find(Effectstrack, Sound => Sound.name == name);
            if (EffectPlaying == null)
            {
                Debug.LogWarning("Effect: " + name + " not found");
                return;
            }

            EffectPlaying.source.Play();
        }
        public void StopEffect()
        {
            if (EffectPlaying == null)
                return;


            EffectPlaying.source.Stop();
        }
    }
}