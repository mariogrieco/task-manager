using System;
using TaskManager.Core.Entities;

namespace TaskManager.WebApi.Dtos
{
    public class TaskItemDto
    {
        public TaskItemDto(string title, string description, TaskStatusItem statusItem, DateTime dueDate, Guid userId)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            StatusItem = statusItem;
            DueDate = dueDate;
            UserId = userId;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public TaskStatusItem StatusItem { get; set; }
        public DateTime DueDate { get; set; }
        public Guid UserId { get; set; }
    }
}
