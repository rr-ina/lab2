using lab1.Data;
using lab1.Models;
using Microsoft.AspNetCore.Mvc;

namespace lab1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongController : ControllerBase
    {
        private readonly List<Song> _songs;
        private readonly List<Playlist> _playlists;

        public SongController()
        {
            _songs = DataSet.songs;
            _playlists = DataSet.playlists;
        }

        //getAll
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        [HttpGet(Name = "GetAllSongs")]
        public IEnumerable<Song> GetAll()
        {
            return _songs;
        }

        //getAllById
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        [HttpGet("{id:int:min(1)}", Name = "GetSongById")]
        public IActionResult Get(int id)
        {
            var song = _songs.FirstOrDefault(s => s.Id == id);
            return song != null ? Ok(song) : NotFound();
        }

        //createSong
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        [HttpPost(Name = "AddSong")]
        public IActionResult Post([FromBody] Song song)
        {
            var validationError = ValidateSong(song);
            if (validationError != null)
            {
                return validationError;
            }

            DataSet.MaxSongId++;
            song.Id = DataSet.MaxSongId;

            _songs.Add(song);

            return CreatedAtAction(nameof(Get), new { id = song.Id }, song);
        }

        //UpdateSongById
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
        [HttpPut("{id:int:min(1)}", Name = "UpdateSong")]
        public IActionResult Put(int id, Song song)
        {
            var existingSong = _songs.FirstOrDefault(s => s.Id == id);
            if (existingSong == null)
            {
                return NotFound();
            }

            var validationError = ValidateSong(song);
            if (validationError != null)
            {
                return validationError;
            }

            existingSong.Title = song.Title;
            existingSong.Artist = song.Artist;
            existingSong.DurationSeconds = song.DurationSeconds;
            existingSong.PlaylistId = song.PlaylistId;

            return NoContent();
        }

        //deleteAll
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
        [HttpDelete(Name = "DeleteAllSongs")]
        public IActionResult DeleteAll()
        {
            _songs.Clear();
            
            DataSet.MaxSongId = 0; 

            return NoContent();
        }

        //deleteById
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
        [HttpDelete("{id:int:min(1)}", Name = "DeleteSong")]
        public IActionResult Delete(int id)
        {
            var song = _songs.FirstOrDefault(s => s.Id == id);
            if (song == null)
            {
                return NotFound();
            }

            _songs.Remove(song);
            return NoContent();
        }

        private IActionResult? ValidateSong(Song song)
        {
            if (song == null) return BadRequest("Song data is null.");

            if (string.IsNullOrEmpty(song.Title) || song.Title == "string")
                return BadRequest("Title is invalid.");

            if (string.IsNullOrEmpty(song.Artist) || song.Artist == "string")
                return BadRequest("Artist is invalid.");

            if (song.DurationSeconds <= 0)
                return BadRequest("Duration must be greater than 0.");

            if (!_playlists.Any(p => p.Id == song.PlaylistId))
                return NotFound($"Playlist with ID {song.PlaylistId} not found.");

            return null;
        }
    }
}