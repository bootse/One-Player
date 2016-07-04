using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Player : State
{
    #region Playing State
    class Playing : State
    {
        public Playing(StateMachine state) : base(state)
        {

        }

        public override void AfterUpdate()
        {
            throw new NotImplementedException("Function used for Player parent class");
        }

        public override void EnterState()
        {
            throw new NotImplementedException();
        }

        public override void LeaveState()
        {
            throw new NotImplementedException();
        }

        public override void UpdateState()
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    #region Paused State
    class Paused : State<Player>
    {
        public Paused(StateMachine state) : base(state)
        {

        }

        public override void AfterUpdate()
        {
            throw new NotImplementedException();
        }

        public override void EnterState()
        {
            throw new NotImplementedException();
        }

        public override void LeaveState()
        {
            throw new NotImplementedException();
        }

        public override void UpdateState()
        {
            if(owner.tryToPlay)
            {
                
            }
        }
    }
    #endregion

    #region Stopped State
    class Stopped : State
    {
        public Stopped(StateMachine state) : base(state)
        {

        }

        public override void AfterUpdate()
        {
            throw new NotImplementedException();
        }

        public override void EnterState()
        {
            throw new NotImplementedException();
        }

        public override void LeaveState()
        {
            throw new NotImplementedException();
        }

        public override void UpdateState()
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    #region Skipping State
    class Skipping : State
    {
        public Skipping(StateMachine state) : base(state)
        {
        }

        public override void AfterUpdate()
        {
            throw new NotImplementedException();
        }

        public override void EnterState()
        {
            throw new NotImplementedException();
        }

        public override void LeaveState()
        {
            throw new NotImplementedException();
        }

        public override void UpdateState()
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    public AudioSource audioSource;
    List<object> songPlayList = new List<object>();
    int trackInList = 0;
    bool looping = false;
    AudioClip lastPlayed;
    bool newClip = false;
    float timeRemaining;
    internal bool tryToPlay = false;

    public Player(StateMachine state) : base(state)
    {

    }

    public override void EnterState()
    {
        
    }

    public override void LeaveState()
    {
        
    }

    public override void UpdateState()
    {

    }

    public override void AfterUpdate()
    {
        if (!audioSource.isPlaying)
        {
            if (!newClip)
            {
                audioSource.clip = null;
            }
            if (audioSource.clip == null)
            {
                if (songPlayList.Count > 0)
                {
                    AudioClip clip = songPlayList[trackInList] as AudioClip;
                    if (clip == null)
                    {
                        WWW song = new WWW((string)songPlayList[trackInList]);
                        songPlayList[trackInList] = song.audioClip;
                    }
                    if (clip != null)
                    {
                        audioSource.clip = clip;
                        newClip = true;
                    }
                }
            }
            if (audioSource.clip != null && audioSource.clip.loadState == AudioDataLoadState.Loaded && (newClip || looping))
            {
                audioSource.Play();
                newClip = false;
            }
        }
    }

    public void Play(params object[] args)
    {
        if (songPlayList.Count == trackInList+1 || songPlayList.Count == 0)
        {
            WWW song = new WWW((string)args[0]);
            songPlayList.Add(song.audioClip);
        }
        else
        {
            songPlayList.Add(args[0]);
        }
        if(!audioSource.isPlaying)
        {
            tryToPlay = true;
        }
    }

    public void Skip(params object[] args)
    {
        if(trackInList < songPlayList.Count - 1)
        {
            trackInList++;
            AudioClip song = songPlayList[trackInList] as AudioClip;
            if(song == null)
            {
                string songUrl = (string)songPlayList[trackInList];
                WWW songFile = new WWW(songUrl);
                song = songFile.audioClip;
            }
            audioSource.clip = song;
        }
    }

    public void Previous(params object[] args)
    {
        Debug.Log(audioSource.time);
        if(audioSource.time > 5)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.clip = lastPlayed;
        }
    }
}
