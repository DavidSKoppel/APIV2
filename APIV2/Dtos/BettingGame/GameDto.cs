using APIV2.Models;

namespace APIV2.Dtos.BettingGame
{
    public class GameDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        public string? Desc { get; set; }
        public string? GameImage { get; set; }
        public virtual ICollection<CharacterDto> Characters { get; set; } = new List<CharacterDto>();
    }
}
