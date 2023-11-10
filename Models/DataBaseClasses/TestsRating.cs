namespace Vokimi.Models.DataBaseClasses
{
    public class TestsRating
    {
        public int Id { get; private set; }
        public int UserId { get;private set; }
        public int TestId { get;private set; }
        public short Rating { get; private set; }
        public TestsRating(int id, int userId, int testId, short rating)
        {
            Id = id;
            UserId = userId;
            TestId = testId;
            Rating = rating;
        }
    }
}
