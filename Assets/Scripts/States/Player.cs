using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Player : State<MainController>
{
    #region Playing State
    class Playing : State<Player>
    {
        public override void GlobalProcess()
        {
            throw new NotImplementedException();
        }

        public override void OnEnter()
        {
            throw new NotImplementedException();
        }

        public override void OnExit()
        {
            throw new NotImplementedException();
        }

        public override void Process()
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    #region Paused State
    class Paused : State<Player>
    {
        public override void GlobalProcess()
        {
            throw new NotImplementedException();
        }

        public override void OnEnter()
        {
            throw new NotImplementedException();
        }

        public override void OnExit()
        {
            throw new NotImplementedException();
        }

        public override void Process()
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    #region Stopped State
    class Stopped : State<Player>
    {
        public override void GlobalProcess()
        {
            throw new NotImplementedException();
        }

        public override void OnEnter()
        {
            throw new NotImplementedException();
        }

        public override void OnExit()
        {
            throw new NotImplementedException();
        }

        public override void Process()
        {
            throw new NotImplementedException();
        }

        public void Play(params object[] args)
        {

        }
    }
    #endregion

    #region Skipping State
    class Skipping : State<Player>
    {
        public override void GlobalProcess()
        {
            throw new NotImplementedException();
        }

        public override void OnEnter()
        {
            throw new NotImplementedException();
        }

        public override void OnExit()
        {
            throw new NotImplementedException();
        }

        public override void Process()
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    #region Back State
    class Back : State<Player>
    {
        public override void GlobalProcess()
        {
            throw new NotImplementedException();
        }

        public override void OnEnter()
        {
            throw new NotImplementedException();
        }

        public override void OnExit()
        {
            throw new NotImplementedException();
        }

        public override void Process()
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

    private State<Player>[] playerStates = new State<Player>[]
    {
        new Playing(),
        new Paused(),
        new Stopped(),
        new Skipping(),
        new Back()
    };

    private StateMachine<Player> stateMachine = new StateMachine<Player>();

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

    #region Override Methods
    public override void Configure(MainController owner)
    {
        base.Configure(owner);
        stateMachine.ChangeState(playerStates[2]);
    }

    public override void OnEnter()
    {
        throw new NotImplementedException();
    }

    public override void Process()
    {
        stateMachine.Update();
    }

    public override void OnExit()
    {
        throw new NotImplementedException();
    }

    public override void GlobalProcess()
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
    #endregion
}
