﻿namespace BandLabTestAssignment.Common.Models.Response
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatorId { get; set; }
    }
}
