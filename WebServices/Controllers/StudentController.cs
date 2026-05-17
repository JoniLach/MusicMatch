using Model;
using System.Web.Http;
using ViewModel;

namespace WebServices.Controllers
{
    [RoutePrefix("api/Student")]
    public class StudentController : ApiController
    {
        [HttpPost]
        [Route("Login")]
        public IHttpActionResult Login([FromBody] Student student)
        {
            if (student == null)
                return BadRequest("נתוני התחברות חסרים");

            StudentDB db = new StudentDB();
            Student result = db.Login(student.UserName, student.Password);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetUpcoming/{studentId}")]
        public IHttpActionResult GetUpcoming(int studentId)
        {
            LessonDB db = new LessonDB();
            return Ok(db.GetUpcomingLessons(studentId, false));
        }

        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            StudentDB db = new StudentDB();
            return Ok(db.SelectAll());
        }
    }
}
