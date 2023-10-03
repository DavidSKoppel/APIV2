using APIV2.Models;

namespace APIV2.Dtos.BettingHistory
{
    public class BettingHistoryForUserDto
    {
        public int Id { get; set; }

        public double? BettingAmount { get; set; }

        public DateTime? CreatedTime { get; set; }

        public bool? Outcome { get; set; }

        public double? BettingResult { get; set; }
        public string BettingCharacter { get; set; }

        public virtual BettingGameForUserDto BettingGame { get; set; }
    }
}
