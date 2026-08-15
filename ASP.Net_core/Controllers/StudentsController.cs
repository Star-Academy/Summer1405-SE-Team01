using Microsoft.AspNetCore.Mvc;
using ASP.Net_core.Models;
using ASP.Net_core.Repositories;

namespace ASP.Net_core.Controllers
{
    /// <summary>
    /// کنترلر مدیریت دانشجویان
    /// پشتیبانی از هر دو دیتابیس PostgreSQL و SQL Server
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository _repository;

        /// <summary>
        /// Dependency Injection: ASP.NET Core خودش Repository را به کنترلر تزریق می‌کند
        /// </summary>
        public StudentsController(IStudentRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// GET: api/students?db=postgres یا api/students?db=sqlserver
        /// دریافت لیست تمام دانشجویان
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Student>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery] string db)
        {
            try
            {
                var students = await _repository.GetAllAsync(db);
                return Ok(students);
            }
            catch (ArgumentException ex)
            {
                // خطای پارامتر db نامعتبر
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                // خطاهای پیش‌بینی نشده مثل قطع بودن دیتابیس
                return StatusCode(500, new { error = "خطای داخلی سرور", detail = ex.Message });
            }
        }

        /// <summary>
        /// GET: api/students/{studentNumber}?db=postgres
        /// دریافت یک دانشجو با StudentNumber
        /// </summary>
        [HttpGet("{studentNumber}")]
        [ProducesResponseType(typeof(Student), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByStudentNumber(string studentNumber, [FromQuery] string db)
        {
            try
            {
                var student = await _repository.GetByStudentNumberAsync(studentNumber, db);
                
                if (student == null)
                    return NotFound(new { message = $"دانشجو با شماره دانشجویی '{studentNumber}' یافت نشد." });

                return Ok(student);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "خطای داخلی سرور", detail = ex.Message });
            }
        }

        /// <summary>
        /// POST: api/students?db=postgres
        /// ساخت دانشجوی جدید
        /// Body باید JSON باشد
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Student), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] Student student, [FromQuery] string db)
        {
            // اعتبارسنجی اولیه ورودی
            if (student == null || string.IsNullOrWhiteSpace(student.StudentNumber))
                return BadRequest(new { error = "اطلاعات دانشجوی نامعتبر است." });

            try
            {
                var success = await _repository.CreateAsync(student, db);
                
                if (!success)
                    return BadRequest(new { error = "عملیات Insert با شکست مواجه شد." });

                // CreatedAtAction باعث می‌شود کد 201 Created برگردد
                // و لینک GET دانشجو جدید در هدر Location قرار می‌گیرد
                return CreatedAtAction(
                    nameof(GetByStudentNumber),
                    new { studentNumber = student.StudentNumber, db },
                    student);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "خطا در ثبت دانشجو", detail = ex.Message });
            }
        }

        /// <summary>
        /// PUT: api/students/{studentNumber}?db=sqlserver
        /// به‌روزرسانی اطلاعات یک دانشجو
        /// </summary>
        [HttpPut("{studentNumber}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(string studentNumber, [FromBody] Student student, [FromQuery] string db)
        {
            if (student == null)
                return BadRequest(new { error = "داده‌های ارسالی نامعتبر است." });

            try
            {
                var success = await _repository.UpdateAsync(studentNumber, student, db);
                
                if (!success)
                    return NotFound(new { message = $"دانشجو با شماره دانشجویی '{studentNumber}' یافت نشد یا به‌روزرسانی با شکست مواجه شد." });

                // کد 204 NoContent یعنی عملیات موفق بود اما بدنه‌ای برای برگشت نیست
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "خطا در به‌روزرسانی", detail = ex.Message });
            }
        }

        /// <summary>
        /// DELETE: api/students/{studentNumber}?db=postgres
        /// حذف یک دانشجو
        /// </summary>
        [HttpDelete("{studentNumber}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(string studentNumber, [FromQuery] string db)
        {
            try
            {
                var success = await _repository.DeleteAsync(studentNumber, db);
                
                if (!success)
                    return NotFound(new { message = $"دانشجو با شماره دانشجویی '{studentNumber}' یافت نشد." });

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "خطا در حذف دانشجو", detail = ex.Message });
            }
        }
    }
}