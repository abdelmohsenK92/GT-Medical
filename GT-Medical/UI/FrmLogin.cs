using GT_Medical.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GT_Medical.UI
{
    public partial class FrmLogin : BaseForm, ITransientService
    {
        public FrmLogin() : base()
        {
            if (DesignMode)
                return;
            InitializeComponent();
            SetTitle("GT-Medical Login");            
            Shown += (s, e) =>
            {
                SetCueBanner(TxtUser, "إسم المستخدم");
                SetCueBanner(TxtPass, "كلمة المرور");
                TxtUser.Select();
            };
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TxtUser.Text))
                ShowTip("من فضلك أدخل إسم المستخدم");
            else if (string.IsNullOrEmpty(TxtPass.Text))
                ShowTip("من فضلك أدخل كلمة المرور");
            else
            {
                if (TxtUser.Text == "admin" && TxtPass.Text == "admin123")
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                    ShowTip("يوجد خطأ باسم المستخدم او كلمة المرور");
            }
        }
    }
}
