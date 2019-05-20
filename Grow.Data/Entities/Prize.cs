﻿namespace Grow.Data.Entities
{
    public class Prize : BaseEntity
    {
        public string Reward { get; set; }

        public int RewardValue { get; set; }

        public string Description { get; set; }
        
        public PrizeType Type { get; set; }

        public Team Winner { get; set; }

        public Partner GivenBy { get; set; }

        public Contest Contest { get; set; }

        public enum PrizeType
        {
            MainPrize,
            SpecialPrize
        }
    }
}
