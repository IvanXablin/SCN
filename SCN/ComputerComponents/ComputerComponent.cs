﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace SCN.ComputerComponents
{
    public abstract class ComputerComponent : INotifyPropertyChanged
    {
        protected SqlConnection _sqlConnection =
            new SqlConnection(ConfigurationManager.ConnectionStrings["SCNDB"].ConnectionString);

        protected string _executedCommand;

        public string SourceUri { get; set; }

        private DataTable _component;
        public DataTable ComponentInfo
        {
            get => _component;
            set
            {
                _component = value;
                OnPropertyChanged(nameof(ComponentInfo));
            }
        }

        public ComputerComponent()
        {
            _sqlConnection.Open();
        }

        public void SetImage(string path)
        {
            SourceUri = Path.GetFullPath(path);
        }

        protected void UpdateInfo(string nameComponent)
        {
            _executedCommand = $"select * from [{nameComponent}]";

            ComponentInfo = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(_executedCommand, _sqlConnection);
            adapter.Fill(ComponentInfo);
        }

        protected void FilterTheInfo(string command)
        {
            _executedCommand = command;
            SqlCommand sqlCommand = new SqlCommand(command, _sqlConnection);

            ComponentInfo.Clear();
            SqlDataAdapter adapter = new SqlDataAdapter(_executedCommand, _sqlConnection);
            adapter.Fill(ComponentInfo);
        }

        protected void AddOrder(string command)
        {
            _executedCommand = command;
            SqlCommand sqlCommand = new SqlCommand(command, _sqlConnection);
            sqlCommand.ExecuteNonQuery();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}