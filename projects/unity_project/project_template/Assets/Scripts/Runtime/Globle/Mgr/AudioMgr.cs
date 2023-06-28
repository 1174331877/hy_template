using LT_CachePool;
using LT_Kernel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace LT_GL
{
    public class AudioMgr : AbsMgr
    {
        private AudioPool m_Pool;
        private GameObject m_AudioSourceAttchGo;

        //音频缓存
        private Dictionary<string, AudioClip> m_AudioClipCache = new Dictionary<string, AudioClip>();

        public override void OnInit(ITuple tuple = null)
        {
            base.OnInit();
            m_AudioSourceAttchGo = new GameObject("AudioSourceAttch", typeof(DontDestroyOnLoad));
            m_Pool = new AudioPool(1);
            m_Pool.GeneratePoolEntryHook += GenerateAudioPoolEntry;
        }

        /// <summary>
        /// 播放音效自动回收
        /// </summary>
        /// <param name="clipKey"></param>
        public void PlayClip(string clipKey)
        {
            var source = Take();
            source.clip = TakeAudioClip(clipKey);
            source.Play();
            LTGL.Ins.TimerMgr.StartTimer(TokenSource.Token, () => { Recycle(source); }, source.clip.length);
        }

        public AudioSource PlayClip(AudioClip clip)
        {
            var source = Take();
            source.clip = clip;
            source.Play();
            LTGL.Ins.TimerMgr.StartTimer(TokenSource.Token, () => { Recycle(source); }, source.clip.length);
            return source;
        }

        /// <summary>
        /// 取出一个音频资源
        /// </summary>
        /// <param name="resKey"></param>
        /// <returns></returns>
        public AudioClip TakeAudioClip(string resKey)
        {
            if (!m_AudioClipCache.TryGetValue(resKey, out var clip))
            {
                clip = LTGL.Ins.ResMgr.LoadAsset<AudioClip>(resKey, TokenSource.Token);
                m_AudioClipCache.Add(resKey, clip);
            }
            return clip;
        }

        private AudioPoolEntry GenerateAudioPoolEntry()
        {
            var audioSource = m_AudioSourceAttchGo.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = false;

            var poolEntry = new AudioPoolEntry(audioSource);
            m_AudioSourceMapPoolEntry.Add(audioSource, poolEntry);
            return poolEntry;
        }

        private Dictionary<AudioSource, AudioPoolEntry> m_AudioSourceMapPoolEntry = new Dictionary<AudioSource, AudioPoolEntry>();

        public AudioSource Take()
        {
            return m_Pool.Take().AudioSource;
        }

        public void Recycle(AudioSource audioSource)
        {
            if (audioSource && m_AudioSourceMapPoolEntry.TryGetValue(audioSource, out var poolEntry))
            {
                m_Pool.Recycle(poolEntry);
            }
        }

        private class AudioPoolEntry : AbsPoolEntry
        {
            public AudioSource AudioSource { get; private set; }

            public AudioPoolEntry(AudioSource audioSource)
            {
                AudioSource = audioSource;
            }

            public override void OnRecycle()
            {
                AudioSource.pitch = 1;
                AudioSource.clip = null;
                AudioSource.volume = 1;
                AudioSource.loop = false;
            }

            public override void OnTake()
            {
            }
        }

        private class AudioPool : AbsPool<AudioPoolEntry>
        {
            public AudioPool(int growthCount) : base(growthCount)
            {
            }
        }
    }
}