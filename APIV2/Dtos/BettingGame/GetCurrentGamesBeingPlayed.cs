using APIV2.Models;

namespace APIV2.Dtos.BettingGame
{
    public class GetCurrentGamesBeingPlayed
    {
        public int Id { get; set; }

        public int? GameId { get; set; }

        public string? PlannedTime { get; set; }

        public int? WinnerId { get; set; }
        public bool? beingPlayed { get; set; }

        public virtual ICollection<BettingHistoryDto> BettingHistories { get; set; } = new List<BettingHistoryDto>();

        public virtual GameDto? Game { get; set; }
    }
}
