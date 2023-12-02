using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewspaperSellerModels
{
    public class DayTypeDistribution
    {
        public Enums.DayType DayType { get; set; }
        public decimal Probability { get; set; }
        public decimal CummProbability { get; set; }
        public int MinRange { get; set; }
        public int MaxRange { get; set; }
        // calculate cumulative range for day type 
        public void day_prob(Enums.DayType day, decimal probability, decimal cumulative)
        {
            if (probability != 0)
            {
                this.DayType = day;
                this.Probability = probability;
                this.CummProbability = probability + cumulative;
                this.MinRange = (int)(cumulative * 100) + 1;//start the range
                this.MaxRange = (int)(CummProbability * 100);//end the range
            }

        }


    }
}
