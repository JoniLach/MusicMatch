using Model;
using System;
using System.Web;
using System.Web.UI;

namespace WebApp
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Already logged in — skip the login page
            if (!IsPostBack && Session["LoggedUser"] != null)
            {
                string role = Session["Role"]?.ToString();
                if (role == "Teacher")
                    Response.Redirect("TeacherPage.aspx", false);
                else if (role == "Student")
                    Response.Redirect("StudentPage.aspx", false);
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            lbErrorMessage.Text = "";
            string role = ddlRole.SelectedValue;
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            RegisterAsyncTask(new PageAsyncTask(async () =>
            {
                try
                {
                    if (role == "Teacher")
                    {
                        Teacher teacher = new Teacher { UserName = username, Password = password };
                        Teacher result = await GenericApiClient.PostAsync<Teacher, Teacher>("api/Teacher/Login", teacher);
                        if (result != null)
                        {
                            Session["LoggedUser"] = result;
                            Session["Role"] = "Teacher";
                            Response.Redirect("TeacherPage.aspx", false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                        }
                        else
                            lbErrorMessage.Text = "שם משתמש או סיסמה אינם נכונים";
                    }
                    else
                    {
                        Student student = new Student { UserName = username, Password = password };
                        Student result = await GenericApiClient.PostAsync<Student, Student>("api/Student/Login", student);
                        if (result != null)
                        {
                            Session["LoggedUser"] = result;
                            Session["Role"] = "Student";
                            Response.Redirect("StudentPage.aspx", false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                        }
                        else
                            lbErrorMessage.Text = "שם משתמש או סיסמה אינם נכונים";
                    }
                }
                catch (Exception ex)
                {
                    lbErrorMessage.Text = "שגיאה בחיבור לשרת: " + ex.Message;
                }
            }));
        }
    }
}
