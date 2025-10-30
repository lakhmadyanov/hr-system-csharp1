using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace HRSimpleApp
{
    public class MainForm : Form
    {
        private List<Department> departments = new List<Department>();
        private List<Employee> employees = new List<Employee>();
        private List<Salary> salaries = new List<Salary>();

        private DataGridView dataGridEmployees;
        private DataGridView dataGridDepartments;
        private DataGridView dataGridSalaries;
        private ComboBox comboDepartments;
        private ComboBox comboEmployees;
        private ComboBox comboFilterDepartment;
        private ComboBox comboFilterEmployee;
        private TextBox textName;
        private TextBox textPosition;
        private TextBox textDepartmentName;
        private TextBox textSalaryAmount;
        private TextBox textSearchEmployee;
        private TextBox textSearchSalary;
        private DateTimePicker dateSalary;
        private DateTimePicker dateFrom;
        private DateTimePicker dateTo;
        private Button btnAddEmployee;
        private Button btnEditEmployee;
        private Button btnDeleteEmployee;
        private Button btnAddDepartment;
        private Button btnDeleteDepartment;
        private Button btnAddSalary;
        private Button btnDeleteSalary;
        private Button btnSearchEmployee;
        private Button btnSearchSalary;
        private Button btnClearFilters;
        private TabControl tabControl;

        public MainForm()
        {
            BuildForm();
            LoadTestData();
            LoadDepartmentsToCombo();
            LoadEmployeesToCombo();
            LoadEmployees();
            LoadDepartmentsToGrid();
            LoadSalaries();
        }

        private void BuildForm()
        {
            this.Text = "Отдел кадров - Управление сотрудниками, отделами и зарплатами";
            this.Size = new Size(900, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            // TabControl для разделения
            tabControl = new TabControl();
            tabControl.Location = new Point(10, 10);
            tabControl.Size = new Size(870, 650);
            this.Controls.Add(tabControl);

            // Вкладки
            TabPage tabEmployees = new TabPage("Сотрудники");
            TabPage tabDepartments = new TabPage("Отделы");
            TabPage tabSalaries = new TabPage("Зарплаты");

            tabControl.Controls.Add(tabEmployees);
            tabControl.Controls.Add(tabDepartments);
            tabControl.Controls.Add(tabSalaries);

            // === ВКЛАДКА СОТРУДНИКОВ ===
            // Поиск
            Label labelSearch = new Label(); labelSearch.Text = "Поиск:"; labelSearch.Location = new Point(10, 10); labelSearch.Size = new Size(50, 20);
            textSearchEmployee = new TextBox(); textSearchEmployee.Location = new Point(70, 10); textSearchEmployee.Size = new Size(200, 20);
            btnSearchEmployee = new Button(); btnSearchEmployee.Text = "Найти"; btnSearchEmployee.Location = new Point(280, 10); btnSearchEmployee.Size = new Size(80, 23);
            btnSearchEmployee.Click += BtnSearchEmployee_Click;

            tabEmployees.Controls.Add(labelSearch);
            tabEmployees.Controls.Add(textSearchEmployee);
            tabEmployees.Controls.Add(btnSearchEmployee);

            // Фильтр по отделам
            Label labelFilter = new Label(); labelFilter.Text = "Фильтр по отделу:"; labelFilter.Location = new Point(370, 10); labelFilter.Size = new Size(100, 20);
            comboFilterDepartment = new ComboBox(); comboFilterDepartment.Location = new Point(480, 10); comboFilterDepartment.Size = new Size(150, 20);
            comboFilterDepartment.SelectedIndexChanged += ComboFilterDepartment_SelectedIndexChanged;

            tabEmployees.Controls.Add(labelFilter);
            tabEmployees.Controls.Add(comboFilterDepartment);

            // Кнопка сброса фильтров
            btnClearFilters = new Button(); btnClearFilters.Text = "Сбросить фильтры"; btnClearFilters.Location = new Point(640, 10); btnClearFilters.Size = new Size(120, 23);
            btnClearFilters.Click += BtnClearFilters_Click;
            tabEmployees.Controls.Add(btnClearFilters);

            // Таблица сотрудников
            dataGridEmployees = new DataGridView();
            dataGridEmployees.Location = new Point(10, 40);
            dataGridEmployees.Size = new Size(850, 200);
            dataGridEmployees.SelectionChanged += DataGridEmployees_SelectionChanged;
            dataGridEmployees.ColumnHeaderMouseClick += DataGridEmployees_ColumnHeaderMouseClick;
            tabEmployees.Controls.Add(dataGridEmployees);

            // Поля для сотрудников
            Label label1 = new Label(); label1.Text = "ФИО:"; label1.Location = new Point(10, 250); label1.Size = new Size(80, 20);
            Label label2 = new Label(); label2.Text = "Должность:"; label2.Location = new Point(10, 280); label2.Size = new Size(80, 20);
            Label label3 = new Label(); label3.Text = "Отдел:"; label3.Location = new Point(10, 310); label3.Size = new Size(80, 20);

            tabEmployees.Controls.Add(label1);
            tabEmployees.Controls.Add(label2);
            tabEmployees.Controls.Add(label3);

            textName = new TextBox(); textName.Location = new Point(100, 250); textName.Size = new Size(200, 20);
            textPosition = new TextBox(); textPosition.Location = new Point(100, 280); textPosition.Size = new Size(200, 20);
            comboDepartments = new ComboBox(); comboDepartments.Location = new Point(100, 310); comboDepartments.Size = new Size(200, 20);

            tabEmployees.Controls.Add(textName);
            tabEmployees.Controls.Add(textPosition);
            tabEmployees.Controls.Add(comboDepartments);

            // Кнопки для сотрудников
            btnAddEmployee = new Button(); btnAddEmployee.Text = "Добавить сотрудника"; btnAddEmployee.Location = new Point(320, 250); btnAddEmployee.Size = new Size(120, 30);
            btnEditEmployee = new Button(); btnEditEmployee.Text = "Изменить сотрудника"; btnEditEmployee.Location = new Point(450, 250); btnEditEmployee.Size = new Size(120, 30);
            btnDeleteEmployee = new Button(); btnDeleteEmployee.Text = "Удалить сотрудника"; btnDeleteEmployee.Location = new Point(320, 290); btnDeleteEmployee.Size = new Size(120, 30);

            btnAddEmployee.Click += BtnAddEmployee_Click;
            btnEditEmployee.Click += BtnEditEmployee_Click;
            btnDeleteEmployee.Click += BtnDeleteEmployee_Click;

            tabEmployees.Controls.Add(btnAddEmployee);
            tabEmployees.Controls.Add(btnEditEmployee);
            tabEmployees.Controls.Add(btnDeleteEmployee);

            // === ВКЛАДКА ОТДЕЛОВ ===
            dataGridDepartments = new DataGridView();
            dataGridDepartments.Location = new Point(10, 10);
            dataGridDepartments.Size = new Size(850, 300);
            dataGridDepartments.ColumnHeaderMouseClick += DataGridDepartments_ColumnHeaderMouseClick;
            tabDepartments.Controls.Add(dataGridDepartments);

            // Поля для отделов
            Label label4 = new Label(); label4.Text = "Название отдела:"; label4.Location = new Point(10, 320); label4.Size = new Size(100, 20);
            tabDepartments.Controls.Add(label4);

            textDepartmentName = new TextBox(); textDepartmentName.Location = new Point(120, 320); textDepartmentName.Size = new Size(200, 20);
            tabDepartments.Controls.Add(textDepartmentName);

            // Кнопки для отделов
            btnAddDepartment = new Button(); btnAddDepartment.Text = "Добавить отдел"; btnAddDepartment.Location = new Point(330, 320); btnAddDepartment.Size = new Size(100, 30);
            btnDeleteDepartment = new Button(); btnDeleteDepartment.Text = "Удалить отдел"; btnDeleteDepartment.Location = new Point(440, 320); btnDeleteDepartment.Size = new Size(100, 30);

            btnAddDepartment.Click += BtnAddDepartment_Click;
            btnDeleteDepartment.Click += BtnDeleteDepartment_Click;

            tabDepartments.Controls.Add(btnAddDepartment);
            tabDepartments.Controls.Add(btnDeleteDepartment);

            // === ВКЛАДКА ЗАРПЛАТ ===
            // Поиск по сотруднику
            Label labelSearchSalary = new Label(); labelSearchSalary.Text = "Сотрудник:"; labelSearchSalary.Location = new Point(10, 10); labelSearchSalary.Size = new Size(70, 20);
            comboFilterEmployee = new ComboBox(); comboFilterEmployee.Location = new Point(90, 10); comboFilterEmployee.Size = new Size(200, 20);
            comboFilterEmployee.SelectedIndexChanged += ComboFilterEmployee_SelectedIndexChanged;

            tabSalaries.Controls.Add(labelSearchSalary);
            tabSalaries.Controls.Add(comboFilterEmployee);

            // Поиск по дате
            Label labelDateFrom = new Label(); labelDateFrom.Text = "С:"; labelDateFrom.Location = new Point(300, 10); labelDateFrom.Size = new Size(20, 20);
            dateFrom = new DateTimePicker(); dateFrom.Location = new Point(320, 10); dateFrom.Size = new Size(100, 20); dateFrom.Value = DateTime.Now.AddMonths(-1);
            Label labelDateTo = new Label(); labelDateTo.Text = "По:"; labelDateTo.Location = new Point(430, 10); labelDateTo.Size = new Size(25, 20);
            dateTo = new DateTimePicker(); dateTo.Location = new Point(460, 10); dateTo.Size = new Size(100, 20); dateTo.Value = DateTime.Now;
            btnSearchSalary = new Button(); btnSearchSalary.Text = "Найти"; btnSearchSalary.Location = new Point(570, 10); btnSearchSalary.Size = new Size(80, 23);
            btnSearchSalary.Click += BtnSearchSalary_Click;

            tabSalaries.Controls.Add(labelDateFrom);
            tabSalaries.Controls.Add(dateFrom);
            tabSalaries.Controls.Add(labelDateTo);
            tabSalaries.Controls.Add(dateTo);
            tabSalaries.Controls.Add(btnSearchSalary);

            // Поиск по сумме
            Label labelSearchAmount = new Label();
            labelSearchAmount.Text = "Поиск по сумме:";
            labelSearchAmount.Location = new Point(10, 40);
            labelSearchAmount.Size = new Size(100, 20);

            textSearchSalary = new TextBox();
            textSearchSalary.Location = new Point(120, 40);
            textSearchSalary.Size = new Size(200, 20);
            // PlaceholderText не поддерживается в .NET Framework, поэтому убираем

            Button btnSearchByAmount = new Button();
            btnSearchByAmount.Text = "Найти по сумме";
            btnSearchByAmount.Location = new Point(330, 40);
            btnSearchByAmount.Size = new Size(100, 23);
            btnSearchByAmount.Click += BtnSearchByAmount_Click;

            tabSalaries.Controls.Add(labelSearchAmount);
            tabSalaries.Controls.Add(textSearchSalary);
            tabSalaries.Controls.Add(btnSearchByAmount);

            // Таблица зарплат
            dataGridSalaries = new DataGridView();
            dataGridSalaries.Location = new Point(10, 70);
            dataGridSalaries.Size = new Size(850, 200);
            dataGridSalaries.ColumnHeaderMouseClick += DataGridSalaries_ColumnHeaderMouseClick;
            tabSalaries.Controls.Add(dataGridSalaries);

            // Поля для зарплат
            Label label5 = new Label(); label5.Text = "Сотрудник:"; label5.Location = new Point(10, 280); label5.Size = new Size(80, 20);
            Label label6 = new Label(); label6.Text = "Сумма:"; label6.Location = new Point(10, 310); label6.Size = new Size(80, 20);
            Label label7 = new Label(); label7.Text = "Дата:"; label7.Location = new Point(10, 340); label7.Size = new Size(80, 20);

            tabSalaries.Controls.Add(label5);
            tabSalaries.Controls.Add(label6);
            tabSalaries.Controls.Add(label7);

            comboEmployees = new ComboBox(); comboEmployees.Location = new Point(100, 280); comboEmployees.Size = new Size(200, 20);
            textSalaryAmount = new TextBox(); textSalaryAmount.Location = new Point(100, 310); textSalaryAmount.Size = new Size(200, 20);
            dateSalary = new DateTimePicker(); dateSalary.Location = new Point(100, 340); dateSalary.Size = new Size(200, 20); dateSalary.Value = DateTime.Now;

            tabSalaries.Controls.Add(comboEmployees);
            tabSalaries.Controls.Add(textSalaryAmount);
            tabSalaries.Controls.Add(dateSalary);

            // Кнопки для зарплат
            btnAddSalary = new Button(); btnAddSalary.Text = "Добавить зарплату"; btnAddSalary.Location = new Point(320, 280); btnAddSalary.Size = new Size(120, 30);
            btnDeleteSalary = new Button(); btnDeleteSalary.Text = "Удалить зарплату"; btnDeleteSalary.Location = new Point(450, 280); btnDeleteSalary.Size = new Size(120, 30);

            btnAddSalary.Click += BtnAddSalary_Click;
            btnDeleteSalary.Click += BtnDeleteSalary_Click;

            tabSalaries.Controls.Add(btnAddSalary);
            tabSalaries.Controls.Add(btnDeleteSalary);
        }

        private void LoadTestData()
        {
            // Отделы
            departments.Add(new Department { Id = 1, Name = "IT отдел" });
            departments.Add(new Department { Id = 2, Name = "Бухгалтерия" });
            departments.Add(new Department { Id = 3, Name = "Отдел продаж" });
            departments.Add(new Department { Id = 4, Name = "Маркетинг" });

            // Сотрудники
            employees.Add(new Employee { Id = 1, FullName = "Иванов Иван", Position = "Программист", DepartmentId = 1 });
            employees.Add(new Employee { Id = 2, FullName = "Петрова Мария", Position = "Бухгалтер", DepartmentId = 2 });
            employees.Add(new Employee { Id = 3, FullName = "Сидоров Алексей", Position = "Менеджер", DepartmentId = 3 });
            employees.Add(new Employee { Id = 4, FullName = "Козлова Анна", Position = "Дизайнер", DepartmentId = 1 });
            employees.Add(new Employee { Id = 5, FullName = "Федоров Дмитрий", Position = "Аналитик", DepartmentId = 3 });

            // Зарплаты
            salaries.Add(new Salary { Id = 1, EmployeeId = 1, Amount = 50000, PaymentDate = new DateTime(2024, 1, 15) });
            salaries.Add(new Salary { Id = 2, EmployeeId = 2, Amount = 45000, PaymentDate = new DateTime(2024, 1, 15) });
            salaries.Add(new Salary { Id = 3, EmployeeId = 1, Amount = 52000, PaymentDate = new DateTime(2024, 2, 15) });
            salaries.Add(new Salary { Id = 4, EmployeeId = 3, Amount = 48000, PaymentDate = new DateTime(2024, 1, 20) });
            salaries.Add(new Salary { Id = 5, EmployeeId = 4, Amount = 47000, PaymentDate = new DateTime(2024, 2, 10) });
            salaries.Add(new Salary { Id = 6, EmployeeId = 5, Amount = 51000, PaymentDate = new DateTime(2024, 2, 12) });
        }

        private void LoadDepartmentsToCombo()
        {
            comboDepartments.Items.Clear();
            comboFilterDepartment.Items.Clear();

            // Добавляем пустой элемент для сброса фильтра
            comboFilterDepartment.Items.Add(new Department { Id = 0, Name = "Все отделы" });

            foreach (var dept in departments)
            {
                comboDepartments.Items.Add(dept);
                comboFilterDepartment.Items.Add(dept);
            }
            comboDepartments.DisplayMember = "Name";
            comboFilterDepartment.DisplayMember = "Name";

            if (comboDepartments.Items.Count > 0)
                comboDepartments.SelectedIndex = 0;
            comboFilterDepartment.SelectedIndex = 0;
        }

        private void LoadEmployeesToCombo()
        {
            comboEmployees.Items.Clear();
            comboFilterEmployee.Items.Clear();

            // Добавляем пустой элемент для сброса фильтра
            comboFilterEmployee.Items.Add(new Employee { Id = 0, FullName = "Все сотрудники", Position = "", DepartmentId = 0 });

            foreach (var emp in employees)
            {
                comboEmployees.Items.Add(emp);
                comboFilterEmployee.Items.Add(emp);
            }
            comboEmployees.DisplayMember = "FullName";
            comboFilterEmployee.DisplayMember = "FullName";

            if (comboEmployees.Items.Count > 0)
                comboEmployees.SelectedIndex = 0;
            comboFilterEmployee.SelectedIndex = 0;
        }

        private void LoadEmployees(List<Employee> employeesToShow = null)
        {
            dataGridEmployees.Rows.Clear();

            if (dataGridEmployees.Columns.Count == 0)
            {
                dataGridEmployees.Columns.Add("Id", "ID");
                dataGridEmployees.Columns.Add("FullName", "ФИО");
                dataGridEmployees.Columns.Add("Position", "Должность");
                dataGridEmployees.Columns.Add("Department", "Отдел");
                dataGridEmployees.Columns["Id"].Visible = false;
            }

            var employeesList = employeesToShow ?? employees;

            foreach (var emp in employeesList)
            {
                string deptName = GetDepartmentName(emp.DepartmentId);
                dataGridEmployees.Rows.Add(emp.Id, emp.FullName, emp.Position, deptName);
            }
        }

        private void LoadDepartmentsToGrid()
        {
            dataGridDepartments.Rows.Clear();

            if (dataGridDepartments.Columns.Count == 0)
            {
                dataGridDepartments.Columns.Add("Id", "ID");
                dataGridDepartments.Columns.Add("Name", "Название отдела");
                dataGridDepartments.Columns.Add("EmployeeCount", "Кол-во сотрудников");
                dataGridDepartments.Columns["Id"].Visible = false;
            }

            foreach (var dept in departments)
            {
                int count = GetEmployeeCountInDepartment(dept.Id);
                dataGridDepartments.Rows.Add(dept.Id, dept.Name, count);
            }
        }

        private void LoadSalaries(List<Salary> salariesToShow = null)
        {
            dataGridSalaries.Rows.Clear();

            if (dataGridSalaries.Columns.Count == 0)
            {
                dataGridSalaries.Columns.Add("Id", "ID");
                dataGridSalaries.Columns.Add("Employee", "Сотрудник");
                dataGridSalaries.Columns.Add("Amount", "Сумма");
                dataGridSalaries.Columns.Add("PaymentDate", "Дата выплаты");
                dataGridSalaries.Columns["Id"].Visible = false;
            }

            var salariesList = salariesToShow ?? salaries;

            foreach (var salary in salariesList)
            {
                string empName = GetEmployeeName(salary.EmployeeId);
                dataGridSalaries.Rows.Add(salary.Id, empName, salary.Amount, salary.PaymentDate.ToShortDateString());
            }
        }

        // === ПОИСК И ФИЛЬТРАЦИЯ ===
        private void BtnSearchEmployee_Click(object sender, EventArgs e)
        {
            string searchText = textSearchEmployee.Text.ToLower();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                LoadEmployees();
                return;
            }

            var filteredEmployees = employees.Where(emp =>
                emp.FullName.ToLower().Contains(searchText) ||
                emp.Position.ToLower().Contains(searchText) ||
                GetDepartmentName(emp.DepartmentId).ToLower().Contains(searchText)
            ).ToList();

            LoadEmployees(filteredEmployees);
        }

        private void ComboFilterDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboFilterDepartment.SelectedItem is Department selectedDept && selectedDept.Id != 0)
            {
                var filteredEmployees = employees.Where(emp => emp.DepartmentId == selectedDept.Id).ToList();
                LoadEmployees(filteredEmployees);
            }
            else
            {
                LoadEmployees();
            }
        }

        private void ComboFilterEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboFilterEmployee.SelectedItem is Employee selectedEmp && selectedEmp.Id != 0)
            {
                var filteredSalaries = salaries.Where(s => s.EmployeeId == selectedEmp.Id).ToList();
                LoadSalaries(filteredSalaries);
            }
            else
            {
                LoadSalaries();
            }
        }

        private void BtnSearchSalary_Click(object sender, EventArgs e)
        {
            var filteredSalaries = salaries.Where(s =>
                s.PaymentDate >= dateFrom.Value.Date &&
                s.PaymentDate <= dateTo.Value.Date
            ).ToList();

            LoadSalaries(filteredSalaries);
        }

        private void BtnSearchByAmount_Click(object sender, EventArgs e)
        {
            if (decimal.TryParse(textSearchSalary.Text, out decimal searchAmount))
            {
                var filteredSalaries = salaries.Where(s => s.Amount == searchAmount).ToList();
                LoadSalaries(filteredSalaries);
            }
            else
            {
                MessageBox.Show("Введите корректную сумму для поиска!");
            }
        }

        private void BtnClearFilters_Click(object sender, EventArgs e)
        {
            textSearchEmployee.Clear();
            comboFilterDepartment.SelectedIndex = 0;
            LoadEmployees();
        }

        // === СОРТИРОВКА ===
        private void DataGridEmployees_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string columnName = dataGridEmployees.Columns[e.ColumnIndex].Name;
            var currentData = GetCurrentEmployeeData();

            switch (columnName)
            {
                case "FullName":
                    currentData = currentData.OrderBy(emp => emp.FullName).ToList();
                    break;
                case "Position":
                    currentData = currentData.OrderBy(emp => emp.Position).ToList();
                    break;
                case "Department":
                    currentData = currentData.OrderBy(emp => GetDepartmentName(emp.DepartmentId)).ToList();
                    break;
            }

            LoadEmployees(currentData);
        }

        private void DataGridDepartments_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string columnName = dataGridDepartments.Columns[e.ColumnIndex].Name;
            List<Department> sortedDepartments;

            switch (columnName)
            {
                case "Name":
                    sortedDepartments = departments.OrderBy(dept => dept.Name).ToList();
                    break;
                case "EmployeeCount":
                    sortedDepartments = departments.OrderByDescending(dept => GetEmployeeCountInDepartment(dept.Id)).ToList();
                    break;
                default:
                    sortedDepartments = departments;
                    break;
            }

            dataGridDepartments.Rows.Clear();
            foreach (var dept in sortedDepartments)
            {
                int count = GetEmployeeCountInDepartment(dept.Id);
                dataGridDepartments.Rows.Add(dept.Id, dept.Name, count);
            }
        }

        private void DataGridSalaries_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string columnName = dataGridSalaries.Columns[e.ColumnIndex].Name;
            var currentData = GetCurrentSalaryData();

            switch (columnName)
            {
                case "Employee":
                    currentData = currentData.OrderBy(s => GetEmployeeName(s.EmployeeId)).ToList();
                    break;
                case "Amount":
                    currentData = currentData.OrderByDescending(s => s.Amount).ToList();
                    break;
                case "PaymentDate":
                    currentData = currentData.OrderByDescending(s => s.PaymentDate).ToList();
                    break;
            }

            LoadSalaries(currentData);
        }

        private List<Employee> GetCurrentEmployeeData()
        {
            var currentData = new List<Employee>();
            foreach (DataGridViewRow row in dataGridEmployees.Rows)
            {
                if (row.Cells["Id"].Value != null)
                {
                    int id = (int)row.Cells["Id"].Value;
                    var emp = employees.FirstOrDefault(e => e.Id == id);
                    if (emp != null) currentData.Add(emp);
                }
            }
            return currentData;
        }

        private List<Salary> GetCurrentSalaryData()
        {
            var currentData = new List<Salary>();
            foreach (DataGridViewRow row in dataGridSalaries.Rows)
            {
                if (row.Cells["Id"].Value != null)
                {
                    int id = (int)row.Cells["Id"].Value;
                    var salary = salaries.FirstOrDefault(s => s.Id == id);
                    if (salary != null) currentData.Add(salary);
                }
            }
            return currentData;
        }

        // === ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ ===
        private string GetDepartmentName(int departmentId)
        {
            foreach (var dept in departments)
            {
                if (dept.Id == departmentId)
                    return dept.Name;
            }
            return "Неизвестно";
        }

        private string GetEmployeeName(int employeeId)
        {
            foreach (var emp in employees)
            {
                if (emp.Id == employeeId)
                    return emp.FullName;
            }
            return "Неизвестно";
        }

        private int GetEmployeeCountInDepartment(int departmentId)
        {
            int count = 0;
            foreach (var emp in employees)
            {
                if (emp.DepartmentId == departmentId)
                    count++;
            }
            return count;
        }

        // === ОБРАБОТЧИКИ СОТРУДНИКОВ ===
        private void BtnAddEmployee_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textName.Text) || string.IsNullOrWhiteSpace(textPosition.Text))
            {
                MessageBox.Show("Заполните ФИО и должность!");
                return;
            }

            int newId = employees.Count > 0 ? employees[employees.Count - 1].Id + 1 : 1;
            Department selectedDept = (Department)comboDepartments.SelectedItem;

            employees.Add(new Employee
            {
                Id = newId,
                FullName = textName.Text,
                Position = textPosition.Text,
                DepartmentId = selectedDept.Id
            });

            LoadEmployees();
            LoadEmployeesToCombo();
            LoadDepartmentsToGrid();
            ClearEmployeeFields();
            MessageBox.Show("Сотрудник добавлен!");
        }

        private void BtnEditEmployee_Click(object sender, EventArgs e)
        {
            if (dataGridEmployees.CurrentRow == null)
            {
                MessageBox.Show("Выберите сотрудника!");
                return;
            }

            int selectedId = (int)dataGridEmployees.CurrentRow.Cells["Id"].Value;
            Employee employee = employees.Find(emp => emp.Id == selectedId);

            if (employee != null)
            {
                employee.FullName = textName.Text;
                employee.Position = textPosition.Text;
                employee.DepartmentId = ((Department)comboDepartments.SelectedItem).Id;

                LoadEmployees();
                LoadEmployeesToCombo();
                LoadDepartmentsToGrid();
                ClearEmployeeFields();
                MessageBox.Show("Данные обновлены!");
            }
        }

        private void BtnDeleteEmployee_Click(object sender, EventArgs e)
        {
            if (dataGridEmployees.CurrentRow == null)
            {
                MessageBox.Show("Выберите сотрудника!");
                return;
            }

            if (MessageBox.Show("Удалить сотрудника?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int selectedId = (int)dataGridEmployees.CurrentRow.Cells["Id"].Value;

                // Удаляем также зарплаты этого сотрудника
                salaries.RemoveAll(s => s.EmployeeId == selectedId);

                employees.RemoveAll(emp => emp.Id == selectedId);
                LoadEmployees();
                LoadEmployeesToCombo();
                LoadDepartmentsToGrid();
                LoadSalaries();
                ClearEmployeeFields();
                MessageBox.Show("Сотрудник удален!");
            }
        }

        private void DataGridEmployees_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridEmployees.CurrentRow != null && dataGridEmployees.CurrentRow.Cells["Id"].Value != null)
            {
                textName.Text = dataGridEmployees.CurrentRow.Cells["FullName"].Value.ToString();
                textPosition.Text = dataGridEmployees.CurrentRow.Cells["Position"].Value.ToString();

                string deptName = dataGridEmployees.CurrentRow.Cells["Department"].Value.ToString();
                foreach (Department dept in comboDepartments.Items)
                {
                    if (dept.Name == deptName)
                    {
                        comboDepartments.SelectedItem = dept;
                        break;
                    }
                }
            }
        }

        // === ОБРАБОТЧИКИ ОТДЕЛОВ ===
        private void BtnAddDepartment_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textDepartmentName.Text))
            {
                MessageBox.Show("Введите название отдела!");
                return;
            }

            int newId = departments.Count > 0 ? departments[departments.Count - 1].Id + 1 : 1;

            departments.Add(new Department
            {
                Id = newId,
                Name = textDepartmentName.Text
            });

            LoadDepartmentsToCombo();
            LoadDepartmentsToGrid();
            textDepartmentName.Clear();
            MessageBox.Show("Отдел добавлен!");
        }

        private void BtnDeleteDepartment_Click(object sender, EventArgs e)
        {
            if (dataGridDepartments.CurrentRow == null)
            {
                MessageBox.Show("Выберите отдел!");
                return;
            }

            int selectedId = (int)dataGridDepartments.CurrentRow.Cells["Id"].Value;

            // Проверяем есть ли сотрудники в этом отделе
            if (GetEmployeeCountInDepartment(selectedId) > 0)
            {
                MessageBox.Show("Нельзя удалить отдел, в котором есть сотрудники!");
                return;
            }

            if (MessageBox.Show("Удалить отдел?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                departments.RemoveAll(dept => dept.Id == selectedId);
                LoadDepartmentsToCombo();
                LoadDepartmentsToGrid();
                MessageBox.Show("Отдел удален!");
            }
        }

        // === ОБРАБОТЧИКИ ЗАРПЛАТ ===
        private void BtnAddSalary_Click(object sender, EventArgs e)
        {
            if (comboEmployees.SelectedItem == null || string.IsNullOrWhiteSpace(textSalaryAmount.Text))
            {
                MessageBox.Show("Выберите сотрудника и введите сумму!");
                return;
            }

            if (!decimal.TryParse(textSalaryAmount.Text, out decimal amount))
            {
                MessageBox.Show("Введите корректную сумму!");
                return;
            }

            int newId = salaries.Count > 0 ? salaries[salaries.Count - 1].Id + 1 : 1;
            Employee selectedEmp = (Employee)comboEmployees.SelectedItem;

            salaries.Add(new Salary
            {
                Id = newId,
                EmployeeId = selectedEmp.Id,
                Amount = amount,
                PaymentDate = dateSalary.Value
            });

            LoadSalaries();
            ClearSalaryFields();
            MessageBox.Show("Зарплата добавлена!");
        }

        private void BtnDeleteSalary_Click(object sender, EventArgs e)
        {
            if (dataGridSalaries.CurrentRow == null)
            {
                MessageBox.Show("Выберите запись о зарплате!");
                return;
            }

            if (MessageBox.Show("Удалить запись о зарплате?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int selectedId = (int)dataGridSalaries.CurrentRow.Cells["Id"].Value;
                salaries.RemoveAll(s => s.Id == selectedId);
                LoadSalaries();
                MessageBox.Show("Запись удалена!");
            }
        }

        private void ClearEmployeeFields()
        {
            textName.Clear();
            textPosition.Clear();
            if (comboDepartments.Items.Count > 0)
                comboDepartments.SelectedIndex = 0;
        }

        private void ClearSalaryFields()
        {
            textSalaryAmount.Clear();
            dateSalary.Value = DateTime.Now;
            if (comboEmployees.Items.Count > 0)
                comboEmployees.SelectedIndex = 0;
        }
    }

    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public int DepartmentId { get; set; }

        public override string ToString()
        {
            return FullName;
        }
    }

    public class Salary
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}