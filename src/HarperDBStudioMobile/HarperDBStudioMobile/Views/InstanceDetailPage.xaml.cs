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
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HarperDBStudioMobile.Views
{
    public partial class InstanceDetailPage : ContentPage
    {
        private string currentRowOperation, currentRecordHashValue = String.Empty;
        private bool isAddNewRow, isDeleteRow, _isOffileMode = false;
        private string hashAttribute = String.Empty;
        private int _currentSelectedSchema, _currentSelectedTable;
        private int _offset, totalRecords = 0;

        RequestOperationsModel requestOperationsModel = new RequestOperationsModel();
        ObservableCollection<string> _schemaList = new ObservableCollection<string>();
        ObservableCollection<string> _schemaTableList = new ObservableCollection<string>();
        Dictionary<string, Dictionary<string, InstanceSchema>> instanceSchemaDict = new Dictionary<string, Dictionary<string, InstanceSchema>>() { };

        List<string> attributes = new List<string>();
        Dictionary<string, string> currentTableData = new Dictionary<string, string>() { };
        List<Dictionary<string, string>> currentTableDataList = new List<Dictionary<string, string>>() { };
        List<Dictionary<string, string>> _currentTableDataList = new List<Dictionary<string, string>>() { };
        List<string> offlineOperationStrings = new List<string>() { };


        RequestSqlActionModel requestSqlActionModel = new RequestSqlActionModel();
        RequestDescribeTableModel requestDescribeTableModel = new RequestDescribeTableModel();
        RequestGetRecordDetailsModel requestGetRecordDetailsModel = new RequestGetRecordDetailsModel();
        RequestUpdateRecordModel requestUpdateRecordModel = new RequestUpdateRecordModel();

        public InstanceDetailPage()
        {
            InitializeComponent();
            addRowButton.IsEnabled = false;
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            this.CheckAndCallCachedOperations(true);
            schemaPicker.ItemsSource = _schemaList;
            tablePicker.ItemsSource = _schemaTableList;
            this.GetSchemaDetails();
            editRecordEditor.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeNone);
            this.previousPageButton.IsEnabled = false;
            this.nextPageButton.IsEnabled = false;
        }

        private void CacheSchemaPickerDetails()
        {
            try
            {
                Preferences.Set(Utils.Utils.STORAGE_KEYS.SCHEMA_DATA.ToString(), JsonConvert.SerializeObject(instanceSchemaDict));
            }
            catch (Exception ex)
            {
                Console.Write("Serialization failed for Schema: " + ex.Message);
            }
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
            //this.totalRecords = this.instanceSchemaDict[this._schemaList[_currentSelectedSchema]][this._schemaTableList[_currentSelectedTable]].record_count;
        }

        private async void GetSchemaDetails()
        {
            this.SwitchLoadingMode(true, "Getting Schema Details...");

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
                    this.CacheSchemaPickerDetails();
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
            //finally
            //{
            //    this.SwitchLoadingMode(false, String.Empty);
            //}

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
            this.SwitchLoadingMode(true, "Generating Columns");
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
            //this.SwitchLoadingMode(false, String.Empty);
        }

        private string GetSelectedJSONValue(string hashValueToSearch)
        {
            foreach (Dictionary<string, string> item in currentTableDataList)
            {
                foreach (var _item in item)
                {
                    if (_item.Value == hashValueToSearch)
                    {
                        return JsonConvert.SerializeObject(item, Formatting.Indented);
                    }
                }
            }
            return null;
        }

        private async void ScrollingGridView_GridRowTapped(object sender, RowTappedEventArgs e)
        {
            string editTableData = String.Empty;
            if (!this._isOffileMode)
            {
                editTableData = await this.GetEditTableData(e.hashValue);
            } else
            {
                editTableData = this.GetSelectedJSONValue(e.hashValue);
            }
            if (String.IsNullOrWhiteSpace(editTableData))
            {
                await DisplayAlert("No Info Found", $"Couldn't find any info with {e.hashValue} Hash Value.", "Ok");
            } else
            {
                this.SwitchEditTableCard(true, editTableData);
                this.currentRecordHashValue = e.hashValue;
                deleteRecordButton.IsVisible = true;
                //updateRecordButton.Text = "Update";
            }
        }

        void tablePicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex == -1)
                return;

            this._currentSelectedTable = selectedIndex;
            this.totalRecords = this.instanceSchemaDict[this._schemaList[_currentSelectedSchema]][this._schemaTableList[_currentSelectedTable]].record_count;
            this.GetTableData();
            
        }

        async void GetTableData(bool switchEditor = false)
        {
            this.SwitchLoadingMode(true, "Getting Table Data...");

            await this.DescribeTable();
            requestSqlActionModel.operation = Utils.Utils.INSTANCE_OPERATIONS.sql.ToString();
            //SELECT * FROM `newSchema`.`newTable`  OFFSET 0 FETCH 20
            requestSqlActionModel.sql = $"SELECT * FROM `{this._schemaList[_currentSelectedSchema]}`.`{this._schemaTableList[_currentSelectedTable]}`  OFFSET {this._offset} FETCH 10";
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
                    _currentTableDataList.Clear();
                    this.PopulateTableData(instanceSchemaTableData.Content.ToString());
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
            //finally {
            //    this.SwitchLoadingMode(false, String.Empty);
            //}
        }

        void PopulateTableData(string data)
        {
            this.SwitchLoadingMode(true, "Populating Table Data...");

            var _jobj = Newtonsoft.Json.Linq.JArray.Parse(data);

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
            int recordCount = this.totalRecords;
            //int recordCount = this.instanceSchemaDict[this._schemaList[_currentSelectedSchema]][this._schemaTableList[_currentSelectedTable]].record_count;
            TotalRecordLabel.Text = $"{this._schemaList[_currentSelectedSchema]} > {this._schemaTableList[_currentSelectedTable]} > {Convert.ToString(recordCount)} records";
            if (Convert.ToInt16(this.currentPageLabel.Text) <= recordCount)
            {
                this.previousPageButton.IsEnabled = false;
                this.nextPageButton.IsEnabled = false;
            }
            if (recordCount == 0)
            {
                this.totalPageLabel.Text = "0";
                this.previousPageButton.IsEnabled = false;
                this.nextPageButton.IsEnabled = false;
                noDataFrame.IsVisible = true;
            }
            else
            {
                noDataFrame.IsVisible = false;
                this.totalPageLabel.Text = Convert.ToInt16(((recordCount - 1) / 10) + 1).ToString();
                if (Convert.ToInt16(this.totalPageLabel.Text) > 1)
                {
                    this.previousPageButton.IsEnabled = true;
                    this.nextPageButton.IsEnabled = true;
                }
                else
                {
                    this.previousPageButton.IsEnabled = false;
                    this.nextPageButton.IsEnabled = false;
                }
            }
            if (Convert.ToInt16(this.currentPageLabel.Text) == 1)
            {
                this.previousPageButton.IsEnabled = false;
            }
            if (Convert.ToInt16(this.currentPageLabel.Text) == Convert.ToInt16(this.totalPageLabel.Text))
            {
                this.nextPageButton.IsEnabled = false;
            }
            this.SwitchLoadingMode(false, String.Empty);
        }

        async Task<string> GetEditTableData(string hashValue)
        {
            this.SwitchLoadingMode(true, "Getting Row Details...");

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
            finally
            {
                this.SwitchLoadingMode(false, String.Empty);
            }
        }

        async Task<bool> DescribeTable()
        {
            this.SwitchLoadingMode(true, "Getting Table Structure...");

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

        void Update_Table_Record(System.Object sender, System.EventArgs e)
        {
            this.SwitchEditTableCard(false, editRecordEditor.Text);
            if (!this.isAddNewRow && !this.isDeleteRow)
            {
                this.currentRowOperation = Utils.Utils.INSTANCE_OPERATIONS.update.ToString();
            }
            this.TableRowEditCalls(this.currentRowOperation);
            this.isAddNewRow = false;
            this.isDeleteRow = false;
        }

        private async void SendCachedOperations()
        {
            int failedOperations = 0;
            foreach (var operationString in offlineOperationStrings)
            {
                try
                {
                    this.TableRowEditCalls("cache", operationString);
                }
                catch (Exception ex)
                {
                    failedOperations++;
                    continue;
                }
            }
            offlineOperationStrings.Clear();
            if (failedOperations > 0)
            {
                await DisplayAlert("Failed Opertion", "Some operation queries failed to be sent", "ok");
            } else
            {
                //After we're done successfully sending all, clear the cache.
                Preferences.Set(Utils.Utils.STORAGE_KEYS.CACHED_OPERATIONS.ToString(), JsonConvert.SerializeObject(offlineOperationStrings));
            }
        }

        async void TableRowEditCalls(string operation, string cachedCleanString = "")
        {
            this.SwitchLoadingMode(true, $"Performing {operation} operation...");

            string cleanString = "{\"operation\":\"" + operation + "\",\"schema\":\"" + this._schemaList[_currentSelectedSchema] + "\",\"table\":\"" + this._schemaTableList[_currentSelectedTable] + "\",\"records\":[" + editRecordEditor.Text.Replace(System.Environment.NewLine, string.Empty).Replace("\t", string.Empty).Replace('”', '"') + "]}";

            if (this.isDeleteRow && !String.IsNullOrWhiteSpace(this.currentRecordHashValue))
            {
                cleanString = "{\"operation\":\"" + operation + "\",\"schema\":\"" + this._schemaList[_currentSelectedSchema] + "\",\"table\":\"" + this._schemaTableList[_currentSelectedTable] + "\",\"hash_values\":[\"" + this.currentRecordHashValue + "\"]}";
            }
            if (!String.IsNullOrWhiteSpace(cachedCleanString))
            {
                cleanString = cachedCleanString;
            }
            if (this._isOffileMode)
            {
                //We'll cache things.
                this.offlineOperationStrings.Add(cleanString);
                try
                {
                    Preferences.Set(Utils.Utils.STORAGE_KEYS.CACHED_OPERATIONS.ToString(), JsonConvert.SerializeObject(offlineOperationStrings));
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    this.SwitchLoadingMode(false, String.Empty);
                }
                var tempObj = JObject.Parse(editRecordEditor.Text.Replace(System.Environment.NewLine, string.Empty).Replace("\t", string.Empty).Replace('”', '"'));

                var firstObject = tempObj.First;
                if (tempObj.ContainsKey("__updatedtime__") == false)
                {
                    firstObject.AddBeforeSelf(new JProperty("__updatedtime__", "Not Yet Added"));
                }
                if (tempObj.ContainsKey("__createdtime__") == false)
                {
                    firstObject.AddAfterSelf(new JProperty("__createdtime__", "Not Yet Added"));
                }
                var Object2 = tempObj.GetValue(hashAttribute, StringComparison.OrdinalIgnoreCase);
                if (Object2 == null)
                {
                    tempObj.Add("hashAttribute", "Not Yet Generated");
                }
                this.PopulateTableData($"[{JsonConvert.SerializeObject(tempObj)}]");

                this.SwitchEditTableCard(false);
            } else
            {
                var editSchemaDetailsClient = RestService.For<IGenericRestClient<string, string>>(LoggedInUserCurrentSelections.INSTANCE_BASE_URL);
                try
                {
                    var editTableSchema = await editSchemaDetailsClient.InstanceCall(LoggedInUserCurrentSelections.current_instance_auth, cleanString);
                    if (editTableSchema != null && editTableSchema.IsSuccessStatusCode && editTableSchema.Content != null)
                    {
                        if (operation == "cache")
                        {
                            if (cleanString.Contains("\"insert\""))
                                operation = "insert";
                            else if (cleanString.Contains("\"delete\""))
                                operation = "delete";
                        }
                        switch (operation)
                        {
                            case "insert":
                                this.totalRecords = this.instanceSchemaDict[this._schemaList[_currentSelectedSchema]][this._schemaTableList[_currentSelectedTable]].record_count + 1;
                                break;
                            case "delete":
                                this.totalRecords = this.instanceSchemaDict[this._schemaList[_currentSelectedSchema]][this._schemaTableList[_currentSelectedTable]].record_count - 1;
                                break;
                            default:
                                break;
                        }
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
                finally
                {
                    this.SwitchLoadingMode(false, String.Empty);
                    this.isDeleteRow = false;
                    this.isAddNewRow = false;
                }
            }
        }

        void cancelEditRecordButton_Clicked(System.Object sender, System.EventArgs e)
        {
            this.SwitchEditTableCard(false);
            //updateRecordButton.Text = "Update";
        }

        void SwitchEditTableCard(bool editFlag, string dataToSet = "")
        {
            editRecordFrame.IsVisible = editFlag;
            TotalRecordLabel.IsVisible = !editFlag;
            gridDataFrame.IsVisible = !editFlag;
            NavigationBar.IsVisible = !editFlag;
            tableMenuStackLayout.IsVisible = !editFlag;
            editRecordEditor.IsReadOnly = !editFlag;
            editRecordEditor.Text = dataToSet;
        }

        void addRowButton_Clicked(System.Object sender, System.EventArgs e)
        {
            string tempJsonString = "{";
            foreach (var attribute in attributes)
            {
                if (attribute == "__updatedtime__" || attribute == "__createdtime__" || attribute == hashAttribute)
                    continue;
                tempJsonString += "\n\t\"" + attribute + "\":null,";
            }
            tempJsonString = tempJsonString.TrimEnd(',');
            tempJsonString += "\n}";
            this.SwitchEditTableCard(true, Regex.Unescape(tempJsonString));
            this.isAddNewRow = true;
            deleteRecordButton.IsVisible = false;
            //updateRecordButton.Text = "Insert";
            this.currentRowOperation = Utils.Utils.INSTANCE_OPERATIONS.insert.ToString();
        }

        async void deleteRecordButton_Clicked(System.Object sender, System.EventArgs e)
        {
            this.isDeleteRow = true;
            this.currentRowOperation = Utils.Utils.INSTANCE_OPERATIONS.delete.ToString();
            bool delete = await DisplayAlert("Delete?", "Are you sure you want to delete this row?", "Ok", "Cancel");
            if (delete)
            {
                this.Update_Table_Record(sender, e);
            }
        }

        void previousPageButton_Clicked(System.Object sender, System.EventArgs e)
        {
            if (this._offset <= 0)
            {
                this.previousPageButton.IsEnabled = false;
            } else
            {
                this._offset -= 10;
                this.GetTableData();
                currentPageLabel.Text = (Convert.ToInt16(currentPageLabel.Text) - 1).ToString();
            }
        }

        void nextPageButton_Clicked(System.Object sender, System.EventArgs e)
        {
            if (Convert.ToInt16(this.totalPageLabel.Text) <= 1)
            {
                this.nextPageButton.IsEnabled = false;
            }
            else
            {
                this._offset += 10;
                this.GetTableData();
                currentPageLabel.Text = (Convert.ToInt16(currentPageLabel.Text) + 1).ToString();
            }
        }

        void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            var current = Connectivity.NetworkAccess;

            //If NO ACCESS, then we have offline mode.
            this._isOffileMode = current == NetworkAccess.None;
            noNetworkLabel.IsVisible = this._isOffileMode;
            this.CheckAndCallCachedOperations();
        }

        void CheckAndCallCachedOperations(bool skipNetworkCheck = false)
        {
            try
            {
                var cachedOperations = Preferences.Get(Utils.Utils.STORAGE_KEYS.CACHED_OPERATIONS.ToString(), null);
                if (cachedOperations != null)
                {
                    offlineOperationStrings = JsonConvert.DeserializeObject<List<string>>(cachedOperations);
                }
            }
            catch (Exception ex)
            {

            }
            //If skipNetworkCheck, just check for offlineOperations and try to push through.
            //Because of failure check in SendCachedOperations(), nothing would be wiped from cache if things fail.
            if ((!this._isOffileMode || skipNetworkCheck) && offlineOperationStrings.Count > 0)
            {
                this.SendCachedOperations();
            }
        }

        void SwitchLoadingMode(bool isLoading, string dataToShow)
        {
            LoadingStackLayout.IsVisible = isLoading;
            addRowButton.IsEnabled = !isLoading;
            gridDataFrame.IsVisible = !isLoading;
            TotalRecordLabel.IsVisible = !isLoading;
            NavigationBar.IsVisible = !isLoading;
            loadingBox.Text = dataToShow;
        }

        void lougoutToolBarItem_Clicked(System.Object sender, System.EventArgs e)
        {
            Utils.Utils.LogoutUser();
        }
    }
}