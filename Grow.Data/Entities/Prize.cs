namespace Grow.Data.Entities
{
    public class Prize : BaseContestSubEntity
    {
        public string Reward { get; set; }

        public int RewardValue { get; set; }

        public string Description { get; set; }
        
        public PrizeType Type { get; set; }

        public virtual Team Winner { get; set; }
        public int? WinnerId { get; set; }

        public virtual Partner GivenBy { get; set; }
        public int? GivenById { get; set; }

        public enum PrizeType
        {
            MainPrize,
            SpecialPrize
        }
    }
}
