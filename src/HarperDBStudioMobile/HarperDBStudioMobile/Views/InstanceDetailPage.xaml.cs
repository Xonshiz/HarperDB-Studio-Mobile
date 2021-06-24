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
        RequestGetRecordDetailsModel requestGetRecordDetailsModel = new RequestGetRecordDetailsModel();
        RequestUpdateRecordModel requestUpdateRecordModel = new RequestUpdateRecordModel();
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
            _schemaList.Clear();
            foreach (var schema in instanceSchemaDict)
            {
                _schemaList.Add(schema.Key);
            }
            schemaPicker.SelectedIndex = 0;

        }

        private void PopulateTablePicker(string currentSchemaName)
        {
            _schemaTableList.Clear();
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
            gridStackLayout.Children.Clear();

            Dictionary<string, string> tempDict = attributes.ToDictionary(x => x, x => x);
            ScrollingGridView _scrollingGridView = new ScrollingGridView(tempDict, this.hashAttribute, true);
            gridStackLayout.Children.Add(_scrollingGridView);
            _scrollingGridView.GridRowTapped += ScrollingGridView_GridRowTapped;

            foreach (Dictionary<string, string> item in currentTableDataList)
            {
                ScrollingGridView scrollingGridView = new ScrollingGridView(item, this.hashAttribute, false);
                gridStackLayout.Children.Add(scrollingGridView);
                scrollingGridView.GridRowTapped += ScrollingGridView_GridRowTapped;
            }
        }

        private async void ScrollingGridView_GridRowTapped(object sender, RowTappedEventArgs e)
        {
            string editTableData = await this.GetEditTableData(e.hashValue);
            if (String.IsNullOrWhiteSpace(editTableData))
            {
                await DisplayAlert("No Info Found", $"Couldn't find any info with {e.hashValue} Hash Value.", "Ok");
            } else
            {
                this.SwitchEditTableCard(true, editTableData);
                editRecordFrame.IsVisible = true;
                gridDataFrame.IsVisible = false;
                editRecordEditor.IsReadOnly = false;
                editRecordEditor.Text = editTableData;
            }
            //await DisplayAlert("Hash Value", e.hashValue.ToString(), "Ok");
        }

        void tablePicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex == -1)
                return;

            this._currentSelectedTable = selectedIndex;

            this.GetTableData();
            
        }

        async void GetTableData(bool switchEditor = false)
        {
            await this.DescribeTable();
            requestSqlActionModel.operation = Utils.Utils.INSTANCE_OPERATIONS.sql.ToString();
            //SELECT * FROM `newSchema`.`newTable`  OFFSET 0 FETCH 20
            requestSqlActionModel.sql = $"SELECT * FROM `{this._schemaList[_currentSelectedSchema]}`.`{this._schemaTableList[_currentSelectedTable]}`  OFFSET {this._offset} FETCH 20";
            var instanceSchemaClient = RestService.For<IGenericRestClient<string, RequestSqlActionModel>>(LoggedInUserCurrentSelections.INSTANCE_BASE_URL);
            try
            {
                _currentTableDataList.Clear();
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

                    currentTableDataList = _currentTableDataList;
                    this.GenerateGridColumns();
                    if (switchEditor)
                    {
                        //Make Editor HIDDEN and READONLY.
                        this.SwitchEditTableCard(false);
                    }
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

        async Task<string> GetEditTableData(string hashValue)
        {
            requestGetRecordDetailsModel.operation = Utils.Utils.INSTANCE_OPERATIONS.search_by_hash.ToString();
            requestGetRecordDetailsModel.schema = this._schemaList[_currentSelectedSchema];
            requestGetRecordDetailsModel.table = this._schemaTableList[_currentSelectedTable];
            requestGetRecordDetailsModel.get_attributes = new List<string>() { "*" };
            requestGetRecordDetailsModel.hash_values = new List<string>() { hashValue };
            var editSchemaDetailsClient = RestService.For<IGenericRestClient<string, RequestGetRecordDetailsModel>>(LoggedInUserCurrentSelections.INSTANCE_BASE_URL);
            try
            {
                var editTableSchema = await editSchemaDetailsClient.InstanceCall(LoggedInUserCurrentSelections.current_instance_auth, requestGetRecordDetailsModel);
                if (editTableSchema != null && editTableSchema.IsSuccessStatusCode && editTableSchema.Content != null)
                {
                    var _jobj = Newtonsoft.Json.Linq.JArray.Parse(editTableSchema.Content.ToString());
                    return _jobj.First.ToString().ToString();
                }
                else
                {
                    return String.Empty;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error!", ex.Message, "OK");
                return String.Empty;
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

        async void Update_Table_Record(System.Object sender, System.EventArgs e)
        {
            string cleanString = "{\"operation\":\"update\",\"schema\":\"" + this._schemaList[_currentSelectedSchema] + "\",\"table\":\"" + this._schemaTableList[_currentSelectedTable] + "\",\"records\":[" + editRecordEditor.Text.Replace(System.Environment.NewLine, string.Empty) + "]}";


            var editSchemaDetailsClient = RestService.For<IGenericRestClient<string, string>>(LoggedInUserCurrentSelections.INSTANCE_BASE_URL);
            try
            {
                var editTableSchema = await editSchemaDetailsClient.InstanceCall(LoggedInUserCurrentSelections.current_instance_auth, cleanString);
                if (editTableSchema != null && editTableSchema.IsSuccessStatusCode && editTableSchema.Content != null)
                {
                    //Console.Write(editTableSchema.Content);
                    this.GetTableData(true);
                }
                else
                {
                    Console.Write(editTableSchema.Content);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error!", ex.Message, "OK");
            }
        }

        void cancelEditRecordButton_Clicked(System.Object sender, System.EventArgs e)
        {
            this.SwitchEditTableCard(false);
            //editRecordFrame.IsVisible = false;
            //gridDataFrame.IsVisible = true;
            //editRecordEditor.IsReadOnly = true;
            //editRecordEditor.Text = String.Empty;
        }

        void SwitchEditTableCard(bool editFlag, string dataToSet = "")
        {
            editRecordFrame.IsVisible = editFlag;
            gridDataFrame.IsVisible = !editFlag;
            editRecordEditor.IsReadOnly = !editFlag;
            editRecordEditor.Text = dataToSet;
        }
    }
}