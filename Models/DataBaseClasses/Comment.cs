﻿namespace Vokimi.Models.DataBaseClasses
{
    public class Comment
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public int TestId { get; private set; }
        public string Text { get; private set; }
        public DateTime LeavingDate { get; private set; }
        public Comment(int id, int userId, int testId, string text, DateTime leavingDate)
        {
            Id = id;
            UserId = userId;
            TestId = testId;
            Text = text;
            LeavingDate = leavingDate;
        }
    }
}
