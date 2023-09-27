using APIV2.Models;

namespace APIV2.Dtos.BettingGame
{
    public class BettingHistoryDto
    {
        public int Id { get; set; }

        public int? WalletId { get; set; }
        public double? BettingAmount { get; set; }

        public int? BettingGameId { get; set; }
        
        public int? BettingCharacterId { get; set; }

        public bool? Outcome { get; set; }

        public double? BettingResult { get; set; }
    }
}
