using Model;
using System;
using System.Web.UI;

namespace WebApp
{
    public partial class StudentPage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedUser"] == null || Session["Role"]?.ToString() != "Student")
            {
                Response.Redirect("Default.aspx", false);
                return;
            }

            if (!IsPostBack)
            {
                LoadStudentInfo();

                RegisterAsyncTask(new PageAsyncTask(LoadData));
            }
        }

        private void LoadStudentInfo()
        {
            Student student = (Student)Session["LoggedUser"];
            lblFirstName.Text = student.FirstName;
            lblFullName.Text = student.FirstName + " " + student.LastName;
            lblUserName.Text = student.UserName;
            lblEmail.Text = student.Email;
            lblRating.Text = student.Rating.ToString("F1");
            lblSessions.Text = student.SessionsCompleted.ToString();
        }

        private async System.Threading.Tasks.Task LoadData()
        {
            Student student = (Student)Session["LoggedUser"];

            try
            {
                LessonList lessons = await GenericApiClient.GetAsync<LessonList>(
                    $"api/Student/GetUpcoming/{student.Id}");

                if (lessons != null)
                {
                    gvLessons.DataSource = lessons;
                    gvLessons.DataBind();
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
