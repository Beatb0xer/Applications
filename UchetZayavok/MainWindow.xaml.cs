using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using UchetZayavok;

namespace UchetZayavok
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SqlConnection connection;
        private string connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=EventApplicationsDB;Integrated Security=True";

        public MainWindow()
        {
            InitializeComponent();
            InitializeDatabaseConnection();
            LoadFilterData();
            LoadApplications();
        }

        private void InitializeDatabaseConnection()
        {
            connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения к БД: " + ex.Message);
            }
        }

        private void LoadApplications()
        {
            string query = "SELECT a.ApplicationID, a.FullName, a.ContactInfo, e.EventName, s.StatusName, a.ApplicationDate " +
                          "FROM Applications a " +
                          "JOIN Events e ON a.EventID = e.EventID " +
                          "JOIN ApplicationStatuses s ON a.StatusID = s.StatusID";

            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                ApplicationsDataGrid.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
            }
        }

        private void LoadFilterData()
        {
            try
            {
                // Загрузка статусов для фильтра
                SqlDataAdapter statusAdapter = new SqlDataAdapter("SELECT * FROM ApplicationStatuses", connection);
                DataTable statusTable = new DataTable();
                statusAdapter.Fill(statusTable);
                StatusFilterComboBox.ItemsSource = statusTable.DefaultView;
                StatusFilterComboBox.SelectedIndex = -1;

                // Загрузка мероприятий для фильтра
                SqlDataAdapter eventAdapter = new SqlDataAdapter("SELECT * FROM Events", connection);
                DataTable eventTable = new DataTable();
                eventAdapter.Fill(eventTable);
                EventFilterComboBox.ItemsSource = eventTable.DefaultView;
                EventFilterComboBox.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных для фильтра: " + ex.Message);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Создаем новое окно для добавления заявки
            var addWindow = new AddEditApplicationWindow(null, connection);
            if (addWindow.ShowDialog() == true)
            {
                LoadApplications(); // Обновляем список после добавления
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (ApplicationsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите заявку для редактирования");
                return;
            }

            DataRowView row = (DataRowView)ApplicationsDataGrid.SelectedItem;
            int applicationId = Convert.ToInt32(row["ApplicationID"]);
            
            var editWindow = new AddEditApplicationWindow(applicationId, connection);
            if (editWindow.ShowDialog() == true)
            {
                LoadApplications(); // Обновляем список после редактирования
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ApplicationsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите заявку для удаления");
                return;
            }

            if (MessageBox.Show("Удалить выбранную заявку?", "Подтверждение", 
                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    DataRowView row = (DataRowView)ApplicationsDataGrid.SelectedItem;
                    int applicationId = Convert.ToInt32(row["ApplicationID"]);
                    
                    string query = "DELETE FROM Applications WHERE ApplicationID = @id";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@id", applicationId);
                    cmd.ExecuteNonQuery();
                    
                    LoadApplications(); // Обновляем список после удаления
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении: " + ex.Message);
                }
            }
        }

        private void StatusFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void EventFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ClearFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            StatusFilterComboBox.SelectedIndex = -1;
            EventFilterComboBox.SelectedIndex = -1;
            LoadApplications();
        }

        private void ApplyFilters()
        {
            string baseQuery = "SELECT a.ApplicationID, a.FullName, a.ContactInfo, e.EventName, s.StatusName, a.ApplicationDate " +
                              "FROM Applications a " +
                              "JOIN Events e ON a.EventID = e.EventID " +
                              "JOIN ApplicationStatuses s ON a.StatusID = s.StatusID ";

            string whereClause = "";
            List<SqlParameter> parameters = new List<SqlParameter>();

            if (StatusFilterComboBox.SelectedValue != null)
            {
                whereClause += "s.StatusID = @statusId ";
                parameters.Add(new SqlParameter("@statusId", StatusFilterComboBox.SelectedValue));
            }

            if (EventFilterComboBox.SelectedValue != null)
            {
                whereClause += (string.IsNullOrEmpty(whereClause) ? "" : "AND ") + "e.EventID = @eventId ";
                parameters.Add(new SqlParameter("@eventId", EventFilterComboBox.SelectedValue));
            }

            string query = baseQuery + (string.IsNullOrEmpty(whereClause) ? "" : "WHERE " + whereClause);

            try
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddRange(parameters.ToArray());
                
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                ApplicationsDataGrid.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка фильтрации: " + ex.Message);
            }
        }
    }
}
