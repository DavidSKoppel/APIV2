namespace APIV2.Dtos.BettingHistory
{
    public class BettingGameForUserDto
    {
        public DateTime? PlannedTime { get; set; }

        public bool? beingPlayed { get; set; }

        public GameForUserDto Game { get; set; }
    }
}
