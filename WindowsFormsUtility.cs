using System.Data;
using System.DirectoryServices.ActiveDirectory;

namespace CPUWindowsFormsFramework
{
    public class WindowsFormsUtility
    {
        public static void SetListBinding(ComboBox lst, DataTable dtSource, DataTable dtTarget, string tableName)
        {
            lst.DataBindings.Clear();
            lst.DataSource = dtSource;
            lst.ValueMember = tableName + "Id";
            lst.DisplayMember = tableName + "Name";
            lst.DataBindings.Add("SelectedValue", dtTarget, lst.ValueMember, false, DataSourceUpdateMode.OnPropertyChanged);
        }
        public static void SetControlBinding(Control ctrl, BindingSource bindSource)
        {
            string propertyName = "";
            string controlName = ctrl.Name.ToLower();
            string controlType = controlName.Substring(0, 3);
            string columnName = controlName.Substring(3);
            switch (controlType)
            {
                case "lbl":
                case "txt":
                    propertyName = "Text";
                    break;

                case "dtp":
                    propertyName = "Value";
                    break;
            }
            if (!string.IsNullOrEmpty(propertyName) && !string.IsNullOrEmpty(columnName))
            {
                ctrl.DataBindings.Clear();
                ctrl.DataBindings.Add(propertyName, bindSource, columnName, true, DataSourceUpdateMode.OnPropertyChanged);
            }
        }
        public static void FormatGridForSearchResults(DataGridView grid)
        {
            grid.AllowUserToAddRows = false;
            grid.ReadOnly = true;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        public static bool FormIsOpenAlredy(Type frmtype, int pkValue = 0)
        {
            foreach (Form frm in Application.OpenForms)
            {
                int frmPkValue = 0;
                if (frm.Tag != null && frm.Tag is int)
                {
                    frmPkValue = (int)frm.Tag;
                }

                if (frm.GetType() == frmtype && pkValue == frmPkValue)
                {
                    frm.Activate();
                    return true;
                }
            }
            return false;
        }

        public static void SetUpNav(ToolStrip ts)
        {
            ts.Items.Clear();
            foreach (Form f in Application.OpenForms)
            {
                if (!f.IsMdiContainer)
                {
                    ToolStripButton btn = new() { Text = f.Text, Tag = f };
                    btn.Click += Btn_Click;
                    ts.Items.Add(btn);
                    ts.Items.Add(new ToolStripSeparator());
                }
            }
        }

        private static void Btn_Click(object? sender, EventArgs e)
        {
            if (sender is ToolStripButton)
            {
                ToolStripButton btn = (ToolStripButton)sender;
                if (btn.Tag != null && btn.Tag is Form)
                {
                    ((Form)btn.Tag).Activate();
                }
            }
        }
    }
}
