namespace Vokimi.Models.DataBaseClasses
{
    public class TestsTaking
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public int TestId { get; private set; }
        public int ResultPoints { get; private set; }
        public DateOnly TakingDate { get; private set; }
        public TestsTaking(int id, int userId, int testId, int resultPoints, DateOnly takingDate)
        {
            Id = id;
            UserId = userId;
            TestId = testId;
            ResultPoints = resultPoints;
            TakingDate = takingDate;
        }
    }
}
