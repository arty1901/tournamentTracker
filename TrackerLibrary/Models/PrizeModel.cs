using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary
{
    public class PrizeModel
    {
        public int Id { get; set; }
        public int PlaceNumber { get; set; }
        public string  PlaceName { get; set; }
        public decimal PrizeAmount { get; set; }
        public double PrizePrecentage { get; set; }

        public PrizeModel()
        {

        }

        public PrizeModel(string placeName, string placeNumber, string prizeAmount, string prizePercentage)
        {
            this.PlaceName = PlaceName;

            int placeNumberValue = 0;
            int.TryParse(placeNumber, out placeNumberValue);
            this.PlaceNumber = placeNumberValue;

            decimal prizeAmountValue = 0;
            decimal.TryParse(prizeAmount, out prizeAmountValue);
            this.PrizeAmount = prizeAmountValue;

            double prizePersentageValue = 0;
            double.TryParse(prizePercentage, out prizePersentageValue);
            this.PrizePrecentage = prizePersentageValue;
        }
    }
}
