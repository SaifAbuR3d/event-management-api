namespace EventManagement.Domain;

public class Constants
{
    public class Image
    {
        public const int MaxImageUrlLength = 2048;
        public const int MaxAlternativeTextLength = 100;
    }

    public class Review
    {
        public const int MinRating = 1;
        public const int MaxRating = 5;
        public const int MinReviewTitleLength = 2;
        public const int MaxReviewTitleLength = 50;
        public const int MinReviewDescriptionLength = 2;
        public const int MaxReviewDescriptionLength = 1000;
    }

    public class Location
    {
        public const double MinLatitude = -90;
        public const double MaxLatitude = 90;
        public const double MinLongitude = -180;
        public const double MaxLongitude = 180;
    }
}
