using AutoMapper;
using CoffeeManagement.Data.Dtos;
using CoffeeManagement.Data.Dtos.StaffSheet;
using CoffeeManagement.Data.Entities;
using CoffeeManagement.Errors;
using CoffeeManagement.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoffeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class ScheduleController : ControllerBase
    {
        private readonly IGenericRepository<StaffShift> _scheduleRepo;
        private readonly IMapper _mapper;

        public ScheduleController(IGenericRepository<StaffShift> scheduleRepo, IMapper mapper)
        {
            _scheduleRepo = scheduleRepo;
            _mapper = mapper;
        }

        [HttpGet("month")]
        [ProducesResponseType(typeof(ApiResponse<List<ScheduleByDateDto>>), 200)]
        public async Task<IActionResult> GetScheduleByMonth([FromQuery] int year, [FromQuery] int month)
        {
            var startDate = new DateOnly(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var scheduleList = await _scheduleRepo.FindAllAsync(
                predicate: es => es.WorkDate >= startDate && es.WorkDate <= endDate,
                include: q => q.Include(es => es.Shift).Include(es => es.Staff)
            );

            var groupedData = scheduleList
                .GroupBy(s => s.WorkDate)
                .Select(g => new ScheduleByDateDto
                {
                    Date = g.Key,
                    Assignments = _mapper.Map<List<StaffShiftResultDto>>(g.ToList())
                })
                .OrderBy(g => g.Date)
                .ToList();

            return Ok(new ApiResponse<List<ScheduleByDateDto>>(200, data: groupedData));
        }


        [HttpPost("assign-bulk")]
        [Authorize(Roles = "ADMIN,MANAGER")]
        public async Task<IActionResult> AssignShifts([FromBody] List<StaffShiftAssignDto> requests)
        {
            var newAssignments = new List<StaffShift>();
            var conflictErrors = new List<string>();

            var existingDatesAndEmployees = await _scheduleRepo.FindAllAsync(
                es => requests.Select(r => r.WorkDate).Contains(es.WorkDate) &&
                      requests.Select(r => r.StaffId).Contains(es.StaffId)
            );

            var existingSet = existingDatesAndEmployees
                .Select(e => (e.StaffId, e.WorkDate))
                .ToHashSet();

            foreach (var req in requests)
            {
                if (existingSet.Contains((req.StaffId, req.WorkDate)))
                {
                    conflictErrors.Add($"StaffId {req.StaffId} is already assigned a shift on {req.WorkDate}.");
                    continue; // Bỏ qua và kiểm tra ca tiếp theo
                }

                newAssignments.Add(_mapper.Map<StaffShift>(req));
                existingSet.Add((req.StaffId, req.WorkDate));
            }

            if (conflictErrors.Any())
            {
                return BadRequest(new ApiResponse<object>(400, string.Join(" | ", conflictErrors)));
            }

            if (!newAssignments.Any())
            {
                return BadRequest(new ApiResponse<object>(400, "No new valid assignments to add."));
            }

            _scheduleRepo.AddRange(newAssignments);
            await _scheduleRepo.SaveChangesAsync();

            return Ok(new ApiResponse<object>(200, $"Successfully assigned {newAssignments.Count} shifts."));
        }

        /// <summary>
        /// Lấy lịch làm việc của một nhân viên theo StaffId trong khoảng thời gian (tùy chọn), dữ liệu nhóm theo ngày.
        /// </summary>
        [HttpGet("user/{staffId}")]
        [ProducesResponseType(typeof(ApiResponse<List<ScheduleByDateDto>>), 200)]
        public async Task<IActionResult> GetScheduleByUserId(
            string staffId,
            [FromQuery] int? year = null,
            [FromQuery] int? month = null)
        {
            DateOnly? startDate = null;
            DateOnly? endDate = null;
            if (year.HasValue && month.HasValue)
            {
                startDate = new DateOnly(year.Value, month.Value, 1);
                endDate = startDate.Value.AddMonths(1).AddDays(-1);
            }

            var scheduleList = await _scheduleRepo.FindAllAsync(
                predicate: es => es.StaffId == staffId &&
                                 (!startDate.HasValue || es.WorkDate >= startDate.Value) &&
                                 (!endDate.HasValue || es.WorkDate <= endDate.Value),
                include: q => q.Include(es => es.Shift).Include(es => es.Staff)
            );

            var groupedData = scheduleList
                .GroupBy(s => s.WorkDate)
                .Select(g => new ScheduleByDateDto
                {
                    Date = g.Key,
                    Assignments = _mapper.Map<List<StaffShiftResultDto>>(g.ToList())
                })
                .OrderBy(g => g.Date)
                .ToList();

            return Ok(new ApiResponse<List<ScheduleByDateDto>>(200, "success", data: groupedData));
        }


        [HttpPost("assign")]
        [Authorize(Roles = "ADMIN,MANAGER")]
        [ProducesResponseType(typeof(ApiResponse<StaffShiftResultDto>), 200)]
        public async Task<IActionResult> CreateAssignment([FromBody] StaffShiftAssignDto request)
        {
            var newAssignment = _mapper.Map<StaffShift>(request);

            _scheduleRepo.Add(newAssignment);
            await _scheduleRepo.SaveChangesAsync();

            var resultDto = _mapper.Map<StaffShiftResultDto>(newAssignment);
            return Ok(new ApiResponse<StaffShiftResultDto>(200, "success", resultDto));
        }

        [HttpPut("assign/{id}")]
        [Authorize(Roles = "ADMIN,MANAGER")]
        [ProducesResponseType(typeof(ApiResponse<StaffShiftResultDto>), 200)]
        public async Task<IActionResult> UpdateAssignment(Guid id, [FromBody] StaffShiftAssignDto request)
        {
            var assignment = await _scheduleRepo.GetByIdAsync(id);
            if (assignment == null)
            {
                return NotFound(new ApiResponse<object>(404, "Phân công không tồn tại."));
            }


            _mapper.Map(request, assignment);

            _scheduleRepo.Update(assignment);
            await _scheduleRepo.SaveChangesAsync();

            var resultDto = _mapper.Map<StaffShiftResultDto>(assignment);
            return Ok(new ApiResponse<StaffShiftResultDto>(200, "success", resultDto));
        }


        /// <summary>
        /// Xóa một phân công ca.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN,MANAGER")]
        public async Task<IActionResult> RemoveAssignment(Guid id)
        {
            var assignment = await _scheduleRepo.GetByIdAsync(id);
            if (assignment == null) return NotFound(new ApiResponse<object>(404));

            _scheduleRepo.Remove(assignment);
            await _scheduleRepo.SaveChangesAsync();
            return Ok(new ApiResponse<object>(200, "Assignment removed successfully."));
        }
    }
}