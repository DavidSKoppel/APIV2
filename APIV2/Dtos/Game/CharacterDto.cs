using APIV2.Models;

namespace APIV2.Dtos.Game
{
    public class CharacterDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        public int? GameId { get; set; }

        public double? Odds { get; set; }
    }
}
