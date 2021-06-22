using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using HarperDBStudioMobile.Interfaces;
using HarperDBStudioMobile.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Refit;
using Syncfusion.SfDataGrid.XForms;
using Xamarin.Forms;
using Xamarin.Forms.DataGrid;

namespace HarperDBStudioMobile.Views
{
    public partial class InstanceDetailPage : ContentPage
    {
        private bool _isRefreshing;
        private string hashAttribute = String.Empty;
        RequestOperationsModel requestOperationsModel = new RequestOperationsModel();
        ObservableCollection<string> _schemaList = new ObservableCollection<string>();
        ObservableCollection<string> _schemaTableList = new ObservableCollection<string>();
        Dictionary<string, Dictionary<string, InstanceSchema>> instanceSchemaDict = new Dictionary<string, Dictionary<string, InstanceSchema>>() { };
        RequestSqlActionModel requestSqlActionModel = new RequestSqlActionModel();
        RequestDescribeTableModel requestDescribeTableModel = new RequestDescribeTableModel();
        List<string> attributes = new List<string>();
        Dictionary<string, string> currentTableData = new Dictionary<string, string>() { };
        List<Dictionary<string, string>> currentTableDataList = new List<Dictionary<string, string>>() { };
        List<Dictionary<string, string>> _currentTableDataList = new List<Dictionary<string, string>>() { };

        private int _currentSelectedSchema, _currentSelectedTable;
        private int _offset = 0;

        public InstanceDetailPage()
        {
            InitializeComponent();
            schemaPicker.ItemsSource = _schemaList;
            tablePicker.ItemsSource = _schemaTableList;
            this.GetSchemaDetails();
        }

        private void PopulateSchemaPicker()
        {
            foreach (var schema in instanceSchemaDict)
            {
                _schemaList.Add(schema.Key);
            }
            schemaPicker.SelectedIndex = 0;

        }

        private void PopulateTablePicker(string currentSchemaName)
        {
            foreach (var table in instanceSchemaDict[currentSchemaName])
            {
                _schemaTableList.Add(table.Key);
            }
            tablePicker.SelectedIndex = 0;
        }

        private async void GetSchemaDetails()
        {
            requestOperationsModel.operation = Utils.Utils.INSTANCE_OPERATIONS.describe_all.ToString();
            var instanceSchemaClient = RestService.For<IGenericRestClient<string, RequestOperationsModel>>(LoggedInUserCurrentSelections.INSTANCE_BASE_URL);
            try
            {
                var instanceSchema = await instanceSchemaClient.InstanceCall(LoggedInUserCurrentSelections.current_instance_auth, requestOperationsModel);
                if (instanceSchema != null && instanceSchema.IsSuccessStatusCode && instanceSchema.Content != null)
                {
                    var jobj = Newtonsoft.Json.Linq.JObject.Parse(instanceSchema.Content.ToString());
                    foreach (var child in jobj.Children())
                    {
                        Dictionary<string, InstanceSchema> keyValuePairs = new Dictionary<string, InstanceSchema>() { };
                        var _jobj = Newtonsoft.Json.Linq.JObject.Parse(child.First.ToString());
                        foreach (var innerChild in _jobj)
                        {
                            keyValuePairs.Add(innerChild.Key.ToString(), innerChild.Value.ToObject<InstanceSchema>());
                        }
                        instanceSchemaDict.Add(child.Path, keyValuePairs);
                    }
                    this.PopulateSchemaPicker();
                }
                else
                {
                    await DisplayAlert("Error!", "Wrong Login Info?", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error!", ex.Message, "OK");
            }

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        void schemaPicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                this._currentSelectedSchema = selectedIndex;
                this.PopulateTablePicker(_schemaList[selectedIndex]);
            }
        }

        private void GenerateGridColumns()
        {
            foreach (Dictionary<string, string> item in currentTableDataList)
            {
                ScrollingGridView scrollingGridView = new ScrollingGridView(item, this.hashAttribute);
                gridStackLayout.Children.Add(scrollingGridView);
                scrollingGridView.GridRowTapped += ScrollingGridView_GridRowTapped;
            }
        }

        private async void ScrollingGridView_GridRowTapped(object sender, RowTappedEventArgs e)
        {
            //string hashValue = e.hashValue;
            await DisplayAlert("Hash Value", e.hashValue.ToString(), "Ok");
        }

        async void tablePicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex == -1)
                return;

            this._currentSelectedTable = selectedIndex;

            await this.DescribeTable();

            requestSqlActionModel.operation = Utils.Utils.INSTANCE_OPERATIONS.sql.ToString();
            //SELECT * FROM `newSchema`.`newTable`  OFFSET 0 FETCH 20
            requestSqlActionModel.sql = $"SELECT * FROM `{this._schemaList[_currentSelectedSchema]}`.`{this._schemaTableList[_currentSelectedTable]}`  OFFSET {this._offset} FETCH 20";
            var instanceSchemaClient = RestService.For<IGenericRestClient<string, RequestSqlActionModel>>(LoggedInUserCurrentSelections.INSTANCE_BASE_URL);
            try
            {
                var instanceSchemaTableData = await instanceSchemaClient.InstanceCall(LoggedInUserCurrentSelections.current_instance_auth, requestSqlActionModel);
                if (instanceSchemaTableData != null && instanceSchemaTableData.IsSuccessStatusCode && instanceSchemaTableData.Content != null)
                {
                    attributes.Clear();
                    foreach (var attribute in instanceSchemaDict[this._schemaList[_currentSelectedSchema]][this._schemaTableList[_currentSelectedTable]].attributes)
                    {
                        attributes.Add(attribute.attribute.ToString());
                    }

                    var _jobj = Newtonsoft.Json.Linq.JArray.Parse(instanceSchemaTableData.Content.ToString());

                    foreach (var item in _jobj)
                    {
                        var parsedString = Newtonsoft.Json.Linq.JObject.Parse(item.ToString());
                        Dictionary<string, string> _currentTableData = new Dictionary<string, string>() { };
                        foreach (var dataRow in parsedString)
                        {
                            _currentTableData.Add(dataRow.Key, dataRow.Value.ToString());

                        }
                        _currentTableDataList.Add(_currentTableData);
                    }

                    currentTableDataList.Clear();
                    currentTableDataList = _currentTableDataList;
                    this.GenerateGridColumns();
                    //this.PopulateDataGrid();
                }
                else
                {
                    await DisplayAlert("Error!", "Wrong Login Info?", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error!", ex.Message, "OK");
            }
        }

        async Task<bool> DescribeTable()
        {
            requestDescribeTableModel.operation = Utils.Utils.INSTANCE_OPERATIONS.describe_table.ToString();
            requestDescribeTableModel.schema = this._schemaList[_currentSelectedSchema];
            requestDescribeTableModel.table = this._schemaTableList[_currentSelectedTable];
            var instanceSchemaClient = RestService.For<IGenericRestClient<string, RequestDescribeTableModel>>(LoggedInUserCurrentSelections.INSTANCE_BASE_URL);
            try
            {
                var instanceSchema = await instanceSchemaClient.InstanceCall(LoggedInUserCurrentSelections.current_instance_auth, requestDescribeTableModel);
                if (instanceSchema != null && instanceSchema.IsSuccessStatusCode && instanceSchema.Content != null)
                {
                    var match = Regex.Match(instanceSchema.Content.ToString(), "\"hash_attribute\":\"(.*?)\"", RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        this.hashAttribute = match.Groups[1].Value.ToString();
                        return true;
                    } else
                    {
                        return false;
                    }
                    //var jobj = Newtonsoft.Json.Linq.JObject.Parse(instanceSchema.Content.ToString());
                    //foreach (JContainer child in jobj.Children())
                    //{
                    //    string FirstChild = child.First.ToString();
                    //    if (child.First.ToString().StartsWith("["))
                    //    {
                    //        continue;
                    //    }
                    //    Dictionary<string, InstanceSchema> keyValuePairs = new Dictionary<string, InstanceSchema>() { };
                    //    var _jobj = Newtonsoft.Json.Linq.JObject.Parse(child.First.ToString());
                    //    foreach (var innerChild in _jobj)
                    //    {
                    //        keyValuePairs.Add(innerChild.Key.ToString(), innerChild.Value.ToObject<InstanceSchema>());
                    //    }
                    //    //instanceSchemaDict.Add(child.Path, keyValuePairs);
                    //}
                    //this.PopulateSchemaPicker();
                }
                else
                {
                    await DisplayAlert("Error!", "Wrong Login Info?", "OK");
                    return false;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error!", ex.Message, "OK");
                return false;
            }
        }
    }
}