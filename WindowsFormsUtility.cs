using System.Data;

namespace CPUWindowsFormsFramework
{
    public class WindowsFormsUtility
    {
        public static void SetListBinding(ComboBox lst, DataTable dtSource, DataTable? dtTarget, string tableName)
        {
            lst.DataBindings.Clear();
            lst.DataSource = dtSource;
            lst.ValueMember = tableName + "Id";
            lst.DisplayMember = lst.Name.Substring(3);
            if (dtTarget != null)
            {
                lst.DataBindings.Add("SelectedValue", dtTarget, lst.ValueMember, false, DataSourceUpdateMode.OnPropertyChanged);
            }
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
        public static void FormatGridForSearchResults(DataGridView grid, string tableName)
        {
            grid.AllowUserToAddRows = false;
            grid.ReadOnly = true;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DoFormatGrid(grid, tableName);
        }

        public static void FormatGridForEdit(DataGridView grid, string tableName)
        {
            grid.EditMode = DataGridViewEditMode.EditOnEnter;
            DoFormatGrid(grid, tableName);
        }

        private static void DoFormatGrid(DataGridView grid, string tableName)
        {
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            grid.RowHeadersWidth = 25;
            foreach(DataGridViewColumn col in grid.Columns)
            {
                if (col.Name.EndsWith("Id"))
                {
                    col.Visible = false;
                }
            }
            string pkName = tableName + "Id";
            if (grid.Columns.Contains(pkName))
            {
                grid.Columns[pkName].Visible = false;
            }
        }

        public static int GetPkIdFromGrid(DataGridView grid, string pkCol, int rowIndex)
        {
            int id = 0;
            if (rowIndex < grid.Rows.Count && grid.Columns.Contains(pkCol) && grid.Rows[rowIndex].Cells[pkCol].Value != DBNull.Value)
            {
                if (grid.Rows[rowIndex].Cells[pkCol].Value is int)
                {
                    id = (int)grid.Rows[rowIndex].Cells[pkCol].Value;
                }
            }
            return id;
        }

        public static int GetPkIdFromComboBox(ComboBox lst)
        {
            int value = 0;
            if(lst.SelectedValue != null && lst.SelectedValue is int)
            {
                value = (int)lst.SelectedValue;
            }

            return value;
        }

        public static void AddComboBoxToGrid(DataGridView grid, DataTable dataSource, string tableName, string displayMember)
        {
            DataGridViewComboBoxColumn c = new();
            c.DataSource = dataSource;
            c.DisplayMember = displayMember;
            c.ValueMember = tableName + "Id";
            c.DataPropertyName = c.ValueMember;
            c.HeaderText = tableName;
            grid.Columns.Insert(0, c);
        }

        public static void AddDeleteButtonToGrid(DataGridView grid, string deleteColName)
        {
            grid.Columns.Add(new DataGridViewButtonColumn() { Text = "X", HeaderText = "Delete", Name = deleteColName, UseColumnTextForButtonValue = true });
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
