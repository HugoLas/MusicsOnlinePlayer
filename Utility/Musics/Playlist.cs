﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Utility.Network.Users;

namespace Utility.Musics
{
    [Serializable]
    public class Playlist : Element
    {
        //public ElementType Type { get; } = ElementType.Album;

        public User Creator { get; set; }
        public override string Name { get; set; }
        public List<Music> musics = new List<Music>();
        public bool Private { get; set; }
        public int Rating { get; set; }
        //public string MID { get; set; }

        public Playlist(User creator, string name, List<Music> Musics, bool IsPrivate)
        {
            Creator = creator;
            Name = name;
            musics = Musics;
            Private = IsPrivate;
            Type = ElementType.Playlist;
            MID = GenerateHash();
        }
        public Playlist(User creator, string name)
        {
            Creator = creator;
            Name = name;
            Type = ElementType.Playlist;
            MID = GenerateHash();
        }

        protected override string KeyToHash() => Name + Private;
    }

    public static class Hash
    {
        public static string SHA256Hash(string value)
        {
            using (var hash = SHA256.Create())
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(value)));
        }
    }

}
