using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace UchetZayavok
{
    public partial class AddEditApplicationWindow : Window
    {
        private readonly int? _applicationId;
        private readonly SqlConnection _connection;

        public AddEditApplicationWindow(int? applicationId, SqlConnection connection)
        {
            InitializeComponent();
            _applicationId = applicationId;
            _connection = connection;
            LoadData();
            
            if (_applicationId.HasValue)
                LoadApplicationData();
        }

        private void LoadData()
        {
            // Загрузка списка мероприятий
            var eventsAdapter = new SqlDataAdapter("SELECT * FROM Events", _connection);
            var eventsTable = new DataTable();
            eventsAdapter.Fill(eventsTable);
            EventsComboBox.ItemsSource = eventsTable.DefaultView;

            // Загрузка списка статусов
            var statusesAdapter = new SqlDataAdapter("SELECT * FROM ApplicationStatuses", _connection);
            var statusesTable = new DataTable();
            statusesAdapter.Fill(statusesTable);
            StatusesComboBox.ItemsSource = statusesTable.DefaultView;
        }

        private void LoadApplicationData()
        {
            string query = "SELECT * FROM Applications WHERE ApplicationID = @id";
            SqlCommand cmd = new SqlCommand(query, _connection);
            cmd.Parameters.AddWithValue("@id", _applicationId);
            
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                FullNameTextBox.Text = reader["FullName"].ToString();
                ContactInfoTextBox.Text = reader["ContactInfo"].ToString();
                
                foreach (DataRowView item in EventsComboBox.Items)
                {
                    if ((int)item["EventID"] == (int)reader["EventID"])
                        EventsComboBox.SelectedItem = item;
                }
                
                foreach (DataRowView item in StatusesComboBox.Items)
                {
                    if ((int)item["StatusID"] == (int)reader["StatusID"])
                        StatusesComboBox.SelectedItem = item;
                }
            }
            reader.Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FullNameTextBox.Text) || 
                string.IsNullOrWhiteSpace(ContactInfoTextBox.Text) ||
                EventsComboBox.SelectedItem == null ||
                StatusesComboBox.SelectedItem == null)
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            try
            {
                DataRowView selectedEvent = (DataRowView)EventsComboBox.SelectedItem;
                DataRowView selectedStatus = (DataRowView)StatusesComboBox.SelectedItem;
                
                string query;
                SqlCommand cmd;
                
                if (_applicationId.HasValue)
                {
                    query = "UPDATE Applications SET FullName = @name, ContactInfo = @contact, " +
                            "EventID = @eventId, StatusID = @statusId WHERE ApplicationID = @id";
                    cmd = new SqlCommand(query, _connection);
                    cmd.Parameters.AddWithValue("@id", _applicationId);
                }
                else
                {
                    query = "INSERT INTO Applications (FullName, ContactInfo, EventID, StatusID) " +
                            "VALUES (@name, @contact, @eventId, @statusId)";
                    cmd = new SqlCommand(query, _connection);
                }
                
                cmd.Parameters.AddWithValue("@name", FullNameTextBox.Text);
                cmd.Parameters.AddWithValue("@contact", ContactInfoTextBox.Text);
                cmd.Parameters.AddWithValue("@eventId", selectedEvent["EventID"]);
                cmd.Parameters.AddWithValue("@statusId", selectedStatus["StatusID"]);
                
                cmd.ExecuteNonQuery();
                
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.Message);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
