using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace AsteriskMod.ModdingHelperTools
{
    internal class FakeUnitaleUtil
    {
        public void PlaySound(string basis, string sound, float volume = 0.65f)
        {
            sound = SimInstance.FakeFileLoader.getRelativePathWithoutExtension(sound).Replace('\\', '/');
            for (int i = 1; i > 0; i++)
            {
                object audio = NewMusicManager.audiolist[basis + i];
                if (audio != null)
                {
                    if (audio.ToString().ToLower() != "null")
                    {
                        if (!NewMusicManager.isStopped(basis + i))
                            continue;
                    }
                    else
                    {
                        NewMusicManager.audiolist.Remove(basis + i);
                        NewMusicManager.CreateChannel(basis + i);
                    }
                }
                else
                    NewMusicManager.CreateChannel(basis + i);
                NewMusicManager.PlaySound(basis + i, sound);
                NewMusicManager.SetVolume(basis + i, volume);
                break;
            }
        }

        public void PlaySound(string basis, AudioClip sound, float volume = 0.65f) { PlaySound(basis, sound.name, volume); }
    }
}
