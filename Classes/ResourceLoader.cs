using System.IO;
using System.Reflection;
using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;

namespace BrutalAPI
{
    public static class ResourceLoader
    {
        public static Texture2D LoadTexture(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetManifestResourceNames().First(r => r.Contains(name));
            var resource = assembly.GetManifestResourceStream(resourceName);
            using var memoryStream = new MemoryStream();
            var buffer = new byte[16384];
            int count;
            while ((count = resource!.Read(buffer, 0, buffer.Length)) > 0)
                memoryStream.Write(buffer, 0, count);
            var spriteTexture = new Texture2D(0, 0, TextureFormat.ARGB32, false)
            {
                anisoLevel = 1,
                filterMode = 0
            };

            spriteTexture.LoadImage(memoryStream.ToArray());
            return spriteTexture;
        }

        public static Sprite LoadSprite(string name, int ppu = 1, Vector2? pivot = null)
        {
            if (pivot == null) { pivot = new Vector2(0.5f, 0.5f); }
            var assembly = Assembly.GetExecutingAssembly();

            Sprite sprite;

            try
            {
                var resourceName = assembly.GetManifestResourceNames().First(r => r.Contains(name));
                var resource = assembly.GetManifestResourceStream(resourceName);
                using var memoryStream = new MemoryStream();
                var buffer = new byte[16384];
                int count;
                while ((count = resource!.Read(buffer, 0, buffer.Length)) > 0)
                    memoryStream.Write(buffer, 0, count);
                var spriteTexture = new Texture2D(0, 0, TextureFormat.ARGB32, false)
                {
                    anisoLevel = 1,
                    filterMode = 0
                };

                spriteTexture.LoadImage(memoryStream.ToArray());
                sprite = Sprite.Create(spriteTexture, new Rect(0, 0, spriteTexture.width, spriteTexture.height), (Vector2)pivot, ppu);

            } catch (InvalidOperationException)
            {
                throw new Exception("Missing Texture! Check for typos when using ResourceLoader.LoadSprite() and that all of your textures have their build action as Embedded Resource.");
            }
            
            return sprite;
        }

        public static byte[] ResourceBinary(string name)
        {
            Assembly a = Assembly.GetExecutingAssembly();
            var resourceName = a.GetManifestResourceNames().First(r => r.Contains(name));
            using (Stream resFilestream = a.GetManifestResourceStream(resourceName))
            {
                if (resFilestream == null) return null;
                byte[] ba = new byte[resFilestream.Length];
                resFilestream.Read(ba, 0, ba.Length);
                return ba;
            }
        }

        public static string LoadSound(string fileName)
        {
            if (!BrutalAPI.assemblyDict.ContainsKey(Assembly.GetExecutingAssembly().FullName))
                BrutalAPI.assemblyDict.Add(Assembly.GetExecutingAssembly().FullName, Assembly.GetExecutingAssembly());

            return Assembly.GetExecutingAssembly().FullName + "|" + fileName;
        }


        //ONLY BRUTALAPI
        public static AudioClip LoadSound(string fileName, Assembly assembly)
        {
            var resourceName = assembly.GetManifestResourceNames().First(r => r.Contains(fileName));
            Stream stream = assembly.GetManifestResourceStream(resourceName);

            if (stream == null)
                throw new Exception("Missing sound file! Check for typos when using ResourceLoader.LoadSound() and that all of your sounds have their build action as Embedded Resource.");

            byte[] ba;
            MemoryStream ms = new MemoryStream();
            stream.CopyTo(ms);
            ba = ms.ToArray();

            float[] f = ConvertByteToFloat(ba);

            AudioClip audioClip = AudioClip.Create(fileName, f.Length, 1, 44100, false);
            audioClip.SetData(f, 0);

            return audioClip;
        }

        private static float[] ConvertByteToFloat(byte[] array)
        {
            float[] floatArr = new float[array.Length / 4];
            for (int i = 0; i < floatArr.Length; i++)
            {
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(array, i * 4, 4);
                floatArr[i] = BitConverter.ToSingle(array, i * 4) / 0x80000000;
            }
            return floatArr;
        }
    }
}