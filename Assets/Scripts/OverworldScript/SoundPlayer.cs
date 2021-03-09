using UnityEngine.Audio;
using System;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public static SoundPlayer Instance = null;
    public Sound[] Soundtrack;
    public Sound[] Effectstrack;
    public Sound SongPlaying;
    public Sound EffectPlaying;

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
        Play_New_Song("City_Pallet_Town");
    }

    private void Update()
    {
        //testing, if you click left button on mouse, change song
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //Not needed, but tell a song to stop
            FindObjectOfType<SoundPlayer>().StopSong();


            Debug.Log("New Song");
            //play next song
            string song = "Route_1";
            FindObjectOfType<SoundPlayer>().Play_New_Song(song);

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
        if(SongPlaying == null)
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
