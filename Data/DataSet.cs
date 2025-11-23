using lab1.Models;

namespace lab1.Data
{
    public class DataSet
    {
        public static List<Song> songs = new List<Song>
        {
            new Song { Id = 1, Title = "Wind", Artist = "Akeboshi", DurationSeconds = 221, PlaylistId = 1},
            new Song { Id = 2, Title = "Duvet", Artist = "boa", DurationSeconds = 204, PlaylistId = 1},
            new Song { Id = 3, Title = "Welcome to Jamrock", Artist = "Damian Marley", DurationSeconds = 213, PlaylistId = 2},
            new Song { Id = 4, Title = "Kawakiwoameku", Artist = "minami", DurationSeconds = 252, PlaylistId = 2}
        };

        public static List<Playlist> playlists = new List<Playlist>
        {
            new Playlist { Id = 1, Name = "For good day", Description = "Just chilling", IsPublic = false},
            new Playlist {Id = 2, Name = "Go", Description = "for active day", IsPublic = true}
        };

        public static int MaxSongId = 4;
        public static int MaxPlayListId = 2;
    }
}