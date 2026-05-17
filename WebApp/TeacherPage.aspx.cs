using Model;
using System;
using System.Web.UI;

namespace WebApp
{
    public partial class TeacherPage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedUser"] == null || Session["Role"]?.ToString() != "Teacher")
            {
                Response.Redirect("Default.aspx", false);
                return;
            }

            if (!IsPostBack)
            {
                LoadTeacherInfo();

                RegisterAsyncTask(new PageAsyncTask(LoadData));
            }
        }

        private void LoadTeacherInfo()
        {
            Teacher teacher = (Teacher)Session["LoggedUser"];
            lblFirstName.Text = teacher.FirstName;
            lblFullName.Text = teacher.FirstName + " " + teacher.LastName;
            lblUserName.Text = teacher.UserName;
            lblEmail.Text = teacher.Email;
            lblRating.Text = teacher.Rating.ToString("F1");
            lblPrice.Text = teacher.Price.ToString("F0");
        }

        private async System.Threading.Tasks.Task LoadData()
        {
            Teacher teacher = (Teacher)Session["LoggedUser"];

            try
            {
                // טעינת שיעורים קרובים
                LessonList lessons = await GenericApiClient.GetAsync<LessonList>(
                    $"api/Teacher/GetUpcoming/{teacher.Id}");
                if (lessons != null)
                {
                    gvLessons.DataSource = lessons;
                    gvLessons.DataBind();
                }
            }
            catch { }

            try
            {
                // טעינת כל התלמידים
                StudentList students = await GenericApiClient.PostAsync<object, StudentList>(
                    "api/Teacher/GetStudents", null);
                if (students != null)
                {
                    gvStudents.DataSource = students;
                    gvStudents.DataBind();
                }
            }
            catch { }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("Default.aspx");
        }
    }
}
