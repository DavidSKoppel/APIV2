using APIV2.Dtos.BettingGame;

namespace APIV2.Dtos.Game
{
    public class GameDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public virtual ICollection<CharacterDto> Characters { get; set; } = new List<CharacterDto>();
    }
}
