using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPUWindowsFormsFramework
{
    public class WindowsFormsUtility
    {
        public static void SetListBinding(ComboBox lst, DataTable dtSource, DataTable dtTarget, string tableName)
        {
            lst.DataSource = dtSource;
            lst.ValueMember = tableName + "Id";
            lst.DisplayMember = tableName + "Name";
            lst.DataBindings.Add("SelectedValue", dtTarget, lst.ValueMember, false, DataSourceUpdateMode.OnPropertyChanged);
        }
        public static void SetControlBinding(Control ctrl, DataTable dt)
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
                ctrl.DataBindings.Add(propertyName, dt, columnName, true, DataSourceUpdateMode.OnPropertyChanged);
            }
        }
        public static void FormatGridForSearchResults(DataGridView grid)
        {
            grid.AllowUserToAddRows = false;
            grid.ReadOnly = true;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
    }
}
