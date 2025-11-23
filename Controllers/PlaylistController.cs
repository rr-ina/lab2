using lab1.Data;
using lab1.Models;
using Microsoft.AspNetCore.Mvc;

namespace lab1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistController : ControllerBase
    {
        private readonly List<Playlist> _playlists;

        public PlaylistController()
        {
            _playlists = DataSet.playlists;
        }

        //getAll
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        [HttpGet(Name = "GetAllPlaylists")]
        public IEnumerable<Playlist> GetAll()
        {
            return _playlists;
        }

        //getAllById
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        [HttpGet("{id:int:min(1)}", Name = "GetPlaylistById")]
        public IActionResult Get(int id)
        {
            var playlist = _playlists.FirstOrDefault(p => p.Id == id);
            return playlist != null ? Ok(playlist) : NotFound();
        }

        //createPlaylist
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        [HttpPost(Name = "AddPlaylist")]
        public IActionResult Post([FromBody] Playlist playlist)
        {
            var validationError = ValidatePlaylist(playlist);
            if (validationError != null)
            {
                return validationError;
            }

            DataSet.MaxPlayListId++;
            playlist.Id = DataSet.MaxPlayListId;

            _playlists.Add(playlist);

            return CreatedAtAction(nameof(Get), new { id = playlist.Id }, playlist);
        }

        //UpdateSongById
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
        [HttpPut("{id:int:min(1)}", Name = "UpdatePlaylist")]
        public IActionResult Put(int id, Playlist playlist)
        {
            var existingPlaylist = _playlists.FirstOrDefault(p => p.Id == id);
            if (existingPlaylist == null)
            {
                return NotFound();
            }

            var validationError = ValidatePlaylist(playlist);
            if (validationError != null)
            {
                return validationError;
            }

            existingPlaylist.Name = playlist.Name;
            existingPlaylist.Description = playlist.Description;
            existingPlaylist.IsPublic = playlist.IsPublic;

            return NoContent();
        }

        //deleteAll
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
        [HttpDelete(Name = "DeleteAllPlaylists")]
        public IActionResult DeleteAll()
        {
            _playlists.Clear();

            DataSet.MaxPlayListId = 0;

            return NoContent();
        }

        //deleteById
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
        [HttpDelete("{id:int:min(1)}", Name = "DeletePlaylist")]
        public IActionResult Delete(int id)
        {
            var playlist = _playlists.FirstOrDefault(p => p.Id == id);
            if (playlist == null)
            {
                return NotFound();
            }

            _playlists.Remove(playlist);
            return NoContent();
        }
        
        private IActionResult? ValidatePlaylist(Playlist playlist)
        {
            if (playlist == null) return BadRequest("Playlist data is null.");

            if (string.IsNullOrEmpty(playlist.Name) || playlist.Name == "string")
                return BadRequest("Name is invalid. Please provide a real name.");

            if (playlist.Description == "string")
                return BadRequest("Description is invalid. Please provide a real description or leave empty.");

            return null;
        }
    }
}