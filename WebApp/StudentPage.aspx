<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeBehind="StudentPage.aspx.cs" Inherits="WebApp.StudentPage" ResponseEncoding="UTF-8" ContentType="text/html; charset=utf-8" %>

<!DOCTYPE html>
<html lang="he" dir="rtl">
<head runat="server">
    <meta charset="utf-8" />
    <title>איזור התלמיד - MusicMatch</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.rtl.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Heebo:wght@300;400;600&display=swap" rel="stylesheet" />
    <style>
        body { font-family: 'Heebo', sans-serif; background: #0f172a; color: #e2e8f0; margin: 30px; }
        .header-card { background: #1e293b; border-radius: 14px; padding: 24px; margin-bottom: 24px; border: 1px solid #334155; }
        .header-card h2 { color: #60a5fa; font-weight: 600; }
        .info-row { margin-bottom: 8px; color: #94a3b8; }
        .info-row strong { color: #e2e8f0; }
        .section-card { background: #1e293b; border-radius: 14px; padding: 24px; margin-bottom: 24px; border: 1px solid #334155; }
        .section-card h3 { color: #60a5fa; font-weight: 600; border-bottom: 1px solid #334155; padding-bottom: 10px; margin-bottom: 16px; }
        .grid-table { width: 100%; border-collapse: collapse; }
        .grid-table th { background: #60a5fa; color: #0f172a; padding: 10px; text-align: right; font-weight: 600; }
        .grid-table td { padding: 10px; border-bottom: 1px solid #334155; color: #cbd5e1; }
        .grid-table tr:hover td { background: #273548; }
        .empty-msg { color: #64748b; font-style: italic; }
        .btn-logout { background: #ef4444; border: none; color: white; border-radius: 8px; padding: 6px 16px; }
        .btn-logout:hover { background: #dc2626; }
        .stat-box { background: #0f172a; border-radius: 10px; padding: 16px; text-align: center; border: 1px solid #334155; }
        .stat-number { font-size: 2rem; font-weight: 600; color: #60a5fa; }
        .stat-label { color: #64748b; font-size: 0.9rem; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <!-- כותרת + מידע על התלמיד -->
        <div class="header-card">
            <div class="d-flex justify-content-between align-items-start">
                <div>
                    <h2>🎵 שלום, <asp:Label ID="lblFirstName" runat="server"></asp:Label>!</h2>
                    <div class="info-row"><strong>שם מלא:</strong> <asp:Label ID="lblFullName" runat="server"></asp:Label></div>
                    <div class="info-row"><strong>שם משתמש:</strong> <asp:Label ID="lblUserName" runat="server"></asp:Label></div>
                    <div class="info-row"><strong>אימייל:</strong> <asp:Label ID="lblEmail" runat="server"></asp:Label></div>
                </div>
                <asp:Button ID="btnLogout" runat="server" Text="התנתקות" CssClass="btn btn-logout" OnClick="btnLogout_Click" />
            </div>

            <!-- סטטיסטיקות -->
            <div class="row mt-3">
                <div class="col-md-3">
                    <div class="stat-box">
                        <div class="stat-number"><asp:Label ID="lblRating" runat="server"></asp:Label> ⭐</div>
                        <div class="stat-label">דירוג</div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="stat-box">
                        <div class="stat-number"><asp:Label ID="lblSessions" runat="server"></asp:Label></div>
                        <div class="stat-label">שיעורים שהושלמו</div>
                    </div>
                </div>
            </div>
        </div>

        <!-- שיעורים קרובים -->
        <div class="section-card">
            <h3>📅 השיעורים הקרובים שלי</h3>
            <asp:GridView ID="gvLessons" runat="server" CssClass="grid-table"
                AutoGenerateColumns="false"
                EmptyDataText="אין שיעורים קרובים מתוכננים.">
                <EmptyDataRowStyle CssClass="empty-msg" />
                <Columns>
                    <asp:BoundField DataField="LessonDate" HeaderText="תאריך" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="StartTime" HeaderText="שעה" />
                    <asp:BoundField DataField="Duration" HeaderText="משך (דקות)" />
                    <asp:BoundField DataField="TeacherName" HeaderText="מורה" />
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
