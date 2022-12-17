using System;
using UnityEngine;
using FMODUnity;
using MonoMod.RuntimeDetour;
using System.Reflection;
using System.Collections;

namespace BrutalAPI
{
    public class SoundManager : MonoBehaviour
    {
        public AudioSourcePool pool;
        public void Awake()
        {
            DontDestroyOnLoad(gameObject);

            IDetour PatchOneShot = new Hook(
                   typeof(RuntimeManager).GetMethod("PlayOneShot", new Type[2] {typeof(string), typeof(Vector3)}),
                   typeof(SoundManager).GetMethod("PatchOneShot", (BindingFlags)(-1)));

            IDetour PatchOneShotAttatched = new Hook(
                    typeof(RuntimeManager).GetMethod("PlayOneShotAttached", new Type[2] { typeof(string), typeof(GameObject) }),
                    typeof(SoundManager).GetMethod("PatchOneShotAttached", (BindingFlags)(-1)));

            pool = new AudioSourcePool();
            Debug.Log("Sound Manager Ready");
        }

        //ONE SHOT
        public static void PatchOneShot(Action<string, Vector3> orig, string path, Vector3 position)
        {
            Assembly a;

            //Sound not modded
            if (path.Split('|').Length < 2)
            {
                orig(path, position);
                return;
            }

            string assemblyName = path.Split('|')[0];
            string fileName = path.Split('|')[1];

            //Couldn't find assembly
            if (!BrutalAPI.assemblyDict.TryGetValue(assemblyName, out a))
            {
                orig(path, position);
                return;
            }           

            //Could find assembly
            AudioClip audioClip = ResourceLoader.LoadSound(fileName, a);
            AudioSource audioSource = BrutalAPI.soundManager.pool.Get();

            audioSource.PlayOneShot(audioClip);
            BrutalAPI.soundManager.StartCoroutine(BrutalAPI.soundManager.WaitForSound(audioSource));
        }

        //ONE SHOT ATTATCHED
        public static void PatchOneShotAttached(Action<string, GameObject> orig, string path, GameObject gameObject)
        {
            Assembly a;
            string assemblyName = path.Split('|')[0];

            //Couldn't find assembly
            if (!BrutalAPI.assemblyDict.TryGetValue(assemblyName, out a))
            {
                orig(path, gameObject);
                return;
            }

            //Could find assembly
            AudioClip audioClip = ResourceLoader.LoadSound(path, a);
            AudioSource audioSource = BrutalAPI.soundManager.pool.Get();
            audioSource.PlayOneShot(audioClip);
            BrutalAPI.soundManager.StartCoroutine(BrutalAPI.soundManager.WaitForSound(audioSource));
        }

        public IEnumerator WaitForSound(AudioSource audioSource)
        {
            yield return new WaitUntil(() => audioSource.isPlaying == false);
            BrutalAPI.soundManager.pool.Put(audioSource);
        }
    }

    public class AudioSourcePool
    {
        private const int MAX_SIZE = 25;
        private readonly Queue objectPool;
        private int counter = 0;
        public AudioSourcePool()
        {
            objectPool = new Queue();
        }

        public AudioSource Get()
        {
            if (objectPool.Count > 0)
            {
                counter--;
                return (AudioSource)objectPool.Dequeue();
            }

            counter++;
            var go = new GameObject("ModdedAudioSource");
            GameObject.DontDestroyOnLoad(go);
            return go.AddComponent<AudioSource>();
        }

        public void Put(AudioSource item)
        {
            if (counter < MAX_SIZE)
            {
                counter++;
                objectPool.Enqueue(item);
            }
            else
            {
                Debug.LogError("Pool is full!");
            }
        }
    }
}
