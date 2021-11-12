using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.UI;
using ProyectoWebForms.DataBase;

namespace ProyectoWebForms
{
    public partial class WebFormCommonUser : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            dlistVaccine.DataBind();
            try
            {
                cmbHour.Items.Clear();
                cmbMinutes.Items.Clear();
                cmbHourFree.Items.Clear();
                SqlDataSource1.SelectParameters["id"].DefaultValue = dlistVaccine.SelectedValue;

                SqlDataSource1.DataSourceMode = SqlDataSourceMode.DataReader;

                SqlDataReader data;

                data = (SqlDataReader)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        for (int i = data.GetInt32(0); i < data.GetInt32(1); i++)
                        {
                            cmbHour.Items.Add(i.ToString());
                            cmbHourFree.Items.Add(i.ToString());
                        }

                        for (int i = 0; i < 60; i = i + data.GetByte(2))
                        {
                            cmbMinutes.Items.Add(i.ToString());
                        }

                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
            //LoadGridView();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            lblDate.Text = "Date: " + DateTime.Now.AddDays(1).ToShortDateString();
        }

        private void LoadGridView()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("appointment_time");
                dt.Columns.Add("full_name");
                dt.Columns.Add("nit");
                foreach (var item in cmbMinutes.Items)
                {
                    string hora = cmbHourFree.Text + ":" + item.ToString();
                    int endHour = int.Parse(cmbHourFree.Text) + 1;
                    string end = endHour + ":" + item.ToString();
                    DataRow row = dt.NewRow();

                    SqlGridDates.SelectParameters["start"].DefaultValue = hora;
                    SqlGridDates.SelectParameters["end"].DefaultValue = end;
                    SqlGridDates.DataSourceMode = SqlDataSourceMode.DataReader;
                    SqlDataReader data;
                    data = (SqlDataReader)SqlGridDates.Select(DataSourceSelectArguments.Empty);
                    int cont = 0;
                    if (data.HasRows)
                    {
                        while (data.Read())
                        {
                            string prueba = data.GetTimeSpan(0).ToString();
                            TimeSpan timeSpan = TimeSpan.Parse(hora);
                            if (TimeSpan.Parse(hora)==data.GetTimeSpan(0))
                            {
                                row["appointment_time"] = hora;
                                row["full_name"] = data.GetString(1);
                                row["nit"] = data.GetString(2);
                                dt.Rows.Add(row);
                                cont++;
                            }
                        }
                    }
                    if (cont==0)
                    {
                        row["appointment_time"] = hora;
                        row["full_name"] = "Libre";
                        row["nit"] = "Libre";

                        dt.Rows.Add(row);
                    } 

                }
                GridView1.DataSource = dt;

                GridView1.DataBind();
                GridView1.UseAccessibleHeader = true;
                GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        protected void bt1_Click(object sender, EventArgs e)
        {
            string hora = cmbHour.Text + ":" + cmbMinutes.Text;
            if (CheckNit(txtNit.Text).Rows.Count >0)
            {
                lblMessage.Text = "La Persona con ese nit ya está registrado";
            }else if (CheckHour(hora).Rows.Count > 0)
            {
                lblMessage.Text = "La hora ya está ocupado";
            }
            else
            {
                List<SqlCommand> commands = new List<SqlCommand>();
                string query = @"INSERT INTO Person(name, last_name, second_last_name, nit, dateUpdate) 
                            VALUES(@name, @lastName, @secondLastName, @nit, CURRENT_TIMESTAMP)";
                string queryC = @"INSERT INTO Email(email, id_person, dateUpdate) VALUES(@email, @id_person, CURRENT_TIMESTAMP)";
                string queryD = @"INSERT INTO Appointment(appointment_date, appointment_time, dateUpdate, id_vaccine, id_person) 
                                VALUES(@date, @hora, CURRENT_TIMESTAMP, " + dlistVaccine.SelectedValue + ", @id_person)";
                int id = int.Parse(DataBaseExec.GetGenerateIDTable("Person"));
                try
                {
                    commands = DataBaseExec.CreateNBasicCommand(3);
                    for (int i = 0; i < commands.Count; i++)
                    {
                        if (i == 0)
                        {
                            commands[i].CommandText = query;
                            commands[i].Parameters.AddWithValue("@name", txtName.Text);
                            commands[i].Parameters.AddWithValue("@lastName", txtLastName.Text);
                            commands[i].Parameters.AddWithValue("@secondLastName", txtSecondLastName.Text);
                            commands[i].Parameters.AddWithValue("@nit", txtNit.Text);
                        }
                        else if (i == 1)
                        {
                            commands[i].CommandText = queryC;
                            commands[i].Parameters.AddWithValue("@id_person", id);
                            commands[i].Parameters.AddWithValue("@email", txtMail.Text);
                        }
                        else
                        {
                            commands[i].CommandText = queryD;
                            commands[i].Parameters.AddWithValue("@id_person", id);
                            commands[i].Parameters.AddWithValue("@hora", TimeSpan.Parse(hora));
                            commands[i].Parameters.AddWithValue("@date", DateTime.Parse(DateTime.Now.AddDays(1).ToShortDateString()));
                        }
                    }
                    DataBaseExec.ExecuteNBasicCommand(commands);
                    lblMessage.Text = "Cita Registrada";
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }

        private DataTable CheckHour(string hora)
        {
            DataTable dt = null;
            string query = @"SELECT id_appointment
                          FROM Appointment
                          WHERE appointment_time = @hour";
            try
            {
                SqlCommand command = DataBaseExec.CreateBasicCommand(query);
                command.Parameters.AddWithValue("@hour", TimeSpan.Parse(hora));
                dt = DataBaseExec.ExecuteDataTableCommand(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        private DataTable CheckNit(string nit)
        {
            string query = @"SELECT P.id_person
                              FROM Appointment A
                              INNER JOIN Person P ON P.id_person = A.id_person
                              WHERE P.nit = @nit";
            DataTable dt = null;
            try
            {
                SqlCommand command = DataBaseExec.CreateBasicCommand(query);
                command.Parameters.AddWithValue("@nit", nit);
                dt = DataBaseExec.ExecuteDataTableCommand(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        protected void btnApplyGrid_Click(object sender, EventArgs e)
        {
            LoadGridView();
        }


    }
}