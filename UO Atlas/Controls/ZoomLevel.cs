namespace UO_Atlas.Controls
{
    public enum ZoomLevel
    {
        PercentOneSixteenth = 0,
        PercentOneEighth,
        PercentOneQuarter,
        PercentOneHalf,
        PercentOneHundred,
        PercentTwoHundred,
        PercentFourHundred,
        PercentEightHundred,
        PercentSixteenHundred,

        MinimumZoom = PercentOneSixteenth,
        MaximumZoom = PercentSixteenHundred
    }
}