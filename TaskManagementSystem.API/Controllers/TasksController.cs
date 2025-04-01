using AutoMapper;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.API.Contracts;
using TaskManagementSystem.API.Data;
using TaskManagementSystem.API.Exceptions;
using TaskManagementSystem.API.Models.Tasks;

namespace TaskManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITaskRepository _taskRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public TasksController(IMapper mapper, ITaskRepository repository, IPublishEndpoint publishEndpoint)
        {
            _mapper = mapper;
            _taskRepository = repository;
            _publishEndpoint = publishEndpoint;
        }

        // GET: api/Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetTaskDto>>> GetTasks()
        {
            var tasks = await _taskRepository.GetAllAsync();
            var records = _mapper.Map<List<GetTaskDto>>(tasks);
            return Ok(records);
        }

        // GET: api/Task/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetTaskDto>> GetTask(int id)
        {
            var task = await _taskRepository.GetAsync(id);

            if (task == null)
            {
                throw new NotFoundException(nameof(GetTask), id);
            }

            var taskDto = _mapper.Map<GetTaskDto>(task);

            return Ok(taskDto);
        }

        // POST: api/Tasks
        [HttpPost]
        public async Task<ActionResult<TaskEntity>> PostTask(CreateTaskDto createTaskDto)
        {
            var task = _mapper.Map<TaskEntity>(createTaskDto);

            await _taskRepository.AddAsync(task);

            await _publishEndpoint.Publish(new TaskCreatedEvent
            {
                TaskId = task.Id,
                Name = task.Name,
                Description = task.Description,
                Status = task.Status.ToString(),
                AssignedTo = task.AssignedTo,
                CreatedOn = DateTime.UtcNow

            });
            
            return CreatedAtAction("GetTask", new { id = task.Id }, task);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, UpdateTaskDto updateTaskDto)
        {
            if (id != updateTaskDto.Id)
            {
                return BadRequest();
            }

            var task = await _taskRepository.GetAsync(id);

            if (task == null)
            {
                throw new NotFoundException(nameof(GetTask), id);
            }

            _mapper.Map(updateTaskDto, task);

            try
            {
                await _taskRepository.UpdateAsync(task);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            
            await _publishEndpoint.Publish(new TaskUpdatedEvent
            {
                TaskId = task.Id,
                Name = task.Name,
                Description = task.Description,
                Status = task.Status.ToString(),
                AssignedTo = task.AssignedTo,
                UpdatedOn = DateTime.UtcNow
            });

            return NoContent();
        }
        
        private async Task<bool> TaskExists(int id)
        {
            return await _taskRepository.Exists(id);
        }
    }
}
