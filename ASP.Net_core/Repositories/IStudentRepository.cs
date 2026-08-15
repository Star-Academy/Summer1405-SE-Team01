using ASP.Net_core.Models;

namespace ASP.Net_core.Repositories
{
    public interface IStudentRepository
    {
        /// <summary>
        /// گرفتن لیست تمام دانشجویان
        /// </summary>
        Task<IEnumerable<Student>> GetAllAsync(string dbType);

        /// <summary>
        /// گرفتن یک دانشجو با StudentNumber
        /// </summary>
        Task<Student?> GetByStudentNumberAsync(string studentNumber, string dbType);

        /// <summary>
        /// ساخت یک دانشجوی جدید
        /// </summary>
        Task<bool> CreateAsync(Student student, string dbType);

        /// <summary>
        /// به‌روزرسانی اطلاعات یک دانشجو
        /// </summary>
        Task<bool> UpdateAsync(string studentNumber, Student student, string dbType);

        /// <summary>
        /// حذف یک دانشجو
        /// </summary>
        Task<bool> DeleteAsync(string studentNumber, string dbType);
    }
}