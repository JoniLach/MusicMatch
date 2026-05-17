using Model;
using System.Web.Http;
using ViewModel;

namespace WebServices.Controllers
{
    [RoutePrefix("api/Teacher")]
    public class TeacherController : ApiController
    {
        [HttpPost]
        [Route("Login")]
        public IHttpActionResult Login([FromBody] Teacher teacher)
        {
            if (teacher == null)
                return BadRequest("נתוני התחברות חסרים");

            TeacherDB db = new TeacherDB();
            Teacher result = db.Login(teacher.UserName, teacher.Password);
            return Ok(result);
        }

        [HttpPost]
        [Route("GetStudents")]
        public IHttpActionResult GetStudents()
        {
            StudentDB db = new StudentDB();
            return Ok(db.SelectAll());
        }

        [HttpGet]
        [Route("GetUpcoming/{teacherId}")]
        public IHttpActionResult GetUpcoming(int teacherId)
        {
            LessonDB db = new LessonDB();
            return Ok(db.GetUpcomingLessons(teacherId, true));
        }

        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            TeacherDB db = new TeacherDB();
            return Ok(db.SelectAll());
        }
    }
}
