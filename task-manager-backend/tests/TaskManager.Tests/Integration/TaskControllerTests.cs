using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using TaskManager.Core.Entities;
using TaskManager.WebApi.Dtos;
using Xunit;

namespace TaskManager.Tests.Integration
{
    public class TaskControllerTests : IntegrationTest
    {
        public TaskControllerTests(WebApplicationFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetTask_ReturnsNotFound_WhenTaskDoesNotExist()
        {
            var taskId = Guid.NewGuid();

            var response = await _client.GetAsync($"/api/task/{taskId}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateTask_ReturnsCreatedTask()
        {
            var taskDto = new TaskItemDto(
                "Test Task",
                "This is a test task descriptipon",
                TaskStatusItem.Pending,
                DateTime.UtcNow.AddDays(7),
                Guid.NewGuid()
            );

            // Act: Hacer una solicitud POST al endpoint
            var response = await _client.PostAsJsonAsync("/api/task", taskDto);

            // Assert: Verificar que la respuesta es 201 Created y que la tarea se creó correctamente
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var createdTask = await response.Content.ReadFromJsonAsync<TaskItem>();
            Assert.NotNull(createdTask);
            Assert.Equal(taskDto.Title, createdTask?.Title);
            Assert.Equal(taskDto.Description, createdTask?.Description);
            Assert.Equal(taskDto.StatusItem, createdTask?.StatusItem);
        }

        [Fact]
        public async Task UpdateTask_ReturnsNoContent_WhenTaskExists()
        {
            // Arrange: Crear una tarea para luego actualizarla
            var taskDto = new TaskItemDto(
                "Updated Task",
                "This is an updated task",
                TaskStatusItem.InProgress,
                DateTime.UtcNow.AddDays(7),
                Guid.NewGuid()
            );
            // Crear la tarea
            var createResponse = await _client.PostAsJsonAsync("/api/task", taskDto);
            var createdTask = await createResponse.Content.ReadFromJsonAsync<TaskItem>();

            // Act: Hacer una solicitud PUT para actualizar la tarea
            var updateResponse = await _client.PutAsJsonAsync($"/api/task/{createdTask?.Id}", taskDto);

            // Assert: Verificar que la respuesta es 204 No Content
            Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);
        }

        [Fact]
        public async Task DeleteTask_ReturnsNoContent_WhenTaskExists()
        {
            var taskDto = new TaskItemDto(
                "Task to Delete",
                "This task will be deleted",
                TaskStatusItem.Pending,
                DateTime.UtcNow.AddDays(7),
                Guid.NewGuid()
            );
            var createResponse = await _client.PostAsJsonAsync("/api/task", taskDto);
            var createdTask = await createResponse.Content.ReadFromJsonAsync<TaskItem>();

            // Act: Hacer una solicitud DELETE para eliminar la tarea
            var deleteResponse = await _client.DeleteAsync($"/api/task/{createdTask?.Id}");

            // Assert: Verificar que la respuesta es 204 No Content
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        }
    }
}
