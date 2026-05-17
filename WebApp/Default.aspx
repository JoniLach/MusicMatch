<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeBehind="Default.aspx.cs" Inherits="WebApp.Default" ResponseEncoding="UTF-8" ContentType="text/html; charset=utf-8" %>

<!DOCTYPE html>
<html lang="he" dir="rtl">
<head runat="server">
    <meta charset="utf-8" />
    <title>התחברות - MusicMatch</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.rtl.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Heebo:wght@300;400;600&display=swap" rel="stylesheet" />
    <style>
        body {
            font-family: 'Heebo', sans-serif;
            background: linear-gradient(135deg, #1a1a2e, #16213e);
            min-height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
        }
        .login-card {
            width: 100%;
            max-width: 420px;
            border-radius: 16px;
            box-shadow: 0 20px 40px rgba(0,0,0,0.4);
            background: #1e293b;
            border: 1px solid #334155;
        }
        .login-title {
            font-weight: 600;
            text-align: center;
            color: #4ade80;
            font-size: 1.8rem;
        }
        .login-subtitle {
            text-align: center;
            color: #cbd5e1;
            margin-bottom: 20px;
            line-height: 1.6;
        }
        .form-control, .form-select {
            background: #0f172a;
            border-color: #475569;
            color: #f1f5f9;
        }
        .form-control:focus, .form-select:focus {
            background: #0f172a;
            border-color: #4ade80;
            color: #f1f5f9;
            box-shadow: none;
        }
        .form-control::placeholder { color: #64748b; }
        .form-label { color: #e2e8f0; font-weight: 500; }
        .btn-login {
            border-radius: 10px;
            padding: 10px;
            font-weight: 600;
            background-color: #4ade80;
            border: none;
            color: #0f172a;
            font-size: 1rem;
        }
        .btn-login:hover { background-color: #22c55e; }
        .form-select option { background: #1e293b; color: #f1f5f9; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="card login-card p-4">
            <h2 class="login-title mb-1">🎵 MusicMatch</h2>
            <p class="login-subtitle">
                ברוכים הבאים!<br />
                אנא התחברו כדי לגשת לאיזור האישי שלכם.
            </p>

            <div class="mb-3">
                <label class="form-label">סוג משתמש</label>
                <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-select">
                    <asp:ListItem Value="Teacher">מורה</asp:ListItem>
                    <asp:ListItem Value="Student">תלמיד</asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="mb-3">
                <label class="form-label">שם משתמש</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="הכנס שם משתמש"></asp:TextBox>
            </div>

            <div class="mb-3">
                <label class="form-label">סיסמה</label>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="הכנס סיסמה"></asp:TextBox>
            </div>

            <div class="d-grid mb-2">
                <asp:Button ID="btnLogin" OnClick="btnLogin_Click" runat="server" Text="התחברות" CssClass="btn btn-login" />
            </div>

            <asp:Label ID="lbErrorMessage" runat="server" CssClass="text-danger text-center d-block mt-1"></asp:Label>
        </div>
    </form>
</body>
</html>
