using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
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
        RequestOperationsModel requestOperationsModel = new RequestOperationsModel();
        ObservableCollection<string> _schemaList = new ObservableCollection<string>();
        ObservableCollection<string> _schemaTableList = new ObservableCollection<string>();
        Dictionary<string, Dictionary<string, InstanceSchema>> instanceSchemaDict = new Dictionary<string, Dictionary<string, InstanceSchema>>() { };
        RequestSqlActionModel requestSqlActionModel = new RequestSqlActionModel();
        List<string> attributes = new List<string>();
        Dictionary<string, string> currentTableData = new Dictionary<string, string>() { };
        List<Dictionary<string, string>> currentTableDataList = new List<Dictionary<string, string>>() { };
        List<Dictionary<string, string>> _currentTableDataList = new List<Dictionary<string, string>>() { };


        public const string JsonData = "[{\"OrderID\":1,\"EmployeeID\":100,\"FirstName\":'Gina',\"LastName\":'Gable'}," +
                                       "{\"OrderID\":2,\"EmployeeID\":200,\"FirstName\":'Danielle',\"LastName\":'Rooney'}," +
                                      "{\"OrderID\":3,\"EmployeeID\":300,\"FirstName\":'Frank',\"LastName\":'Gable'},]";
        public ObservableCollection<DynamicModel> DynamicCollection { get; set; }
        public List<Dictionary<string, string>> DynamicJsonCollection { get; set; }

        private int _currentSelectedSchema, _currentSelectedTable;
        private int _offset = 0;

        private void GridSetup()
        {
            dataGrid.ColumnSizer = ColumnSizer.Star;
            dataGrid.ItemsSource = DynamicCollection;
        }

        public InstanceDetailPage()
        {
            InitializeComponent();
            this.GridSetup();
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

        //private void PopulateDataGrid()
        //{
        //    ColumnCollection colCollection = new ColumnCollection();
        //    foreach (var columnName in currentTableDataList[0])
        //    {
        //        colCollection.Add(new DataGridColumn()
        //        {
        //            FormattedTitle = new FormattedString()
        //            {
        //                Spans =
        //                {
        //                    new Span() { Text = columnName.Key, FontSize = 13, TextColor = Color.Black, FontAttributes = FontAttributes.Bold }
        //                }
        //            },
        //            PropertyName = columnName.Key,
        //            Width = GridLength.Star
        //        });
        //    }
        //}

        private void GenerateGridColumns()
        {

            dataGrid.Columns.Clear();
            foreach (string attribute in attributes)
            {
                dataGrid.Columns.Add(new GridTextColumn()
                {
                    HeaderText = attribute,
                    MappingName = $"Values[{attribute}]",
                    LineBreakMode = LineBreakMode.WordWrap,
                    TextAlignment = TextAlignment.Center,
                    HeaderTextAlignment = TextAlignment.Center,
                });
            }
            //dataGrid.ItemsSource = DynamicCollection;
            if (dataGrid.ItemsSource == null)
            {
                dataGrid.ItemsSource = DynamicCollection;
            }

            //dataGrid.Columns.Clear();
            //dataGrid.Columns.Add(new GridTextColumn()
            //{
            //    HeaderText = "Order ID",
            //    MappingName = "Values[OrderID]",
            //    LineBreakMode = LineBreakMode.WordWrap,
            //    TextAlignment = TextAlignment.Center,
            //    HeaderTextAlignment = TextAlignment.Center,
            //});

            //dataGrid.Columns.Add(new GridTextColumn()
            //{
            //    HeaderText = "Customer ID",
            //    MappingName = "Values[EmployeeID]",
            //    LineBreakMode = LineBreakMode.WordWrap,
            //    TextAlignment = TextAlignment.Center,
            //    HeaderTextAlignment = TextAlignment.Center,
            //});
            //dataGrid.ItemsSource = DynamicCollection;

            //DynamicJsonCollection = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(JsonData);
            //var data = new ObservableCollection<DynamicModel>();
            //foreach (var item in DynamicJsonCollection)
            //{
            //    var obj = new DynamicModel() { Values = item };
            //    data.Add(obj);
            //}
            //DynamicCollection = data;
        }

        private ObservableCollection<DynamicModel> PopulateData()
        {
            var data = new ObservableCollection<DynamicModel>();
            foreach (var item in DynamicJsonCollection)
            {
                var obj = new DynamicModel() { Values = item };
                data.Add(obj);
            }
            return data;
        }

        async void tablePicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex == -1)
                return;

            this._currentSelectedTable = selectedIndex;

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
                    this.GenerateGridColumns();
                    DynamicJsonCollection = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(instanceSchemaTableData.Content.ToString());
                    DynamicCollection = PopulateData();

                    //var _jobj = Newtonsoft.Json.Linq.JArray.Parse(instanceSchemaTableData.Content.ToString());

                    //foreach (var item in _jobj)
                    //{
                    //    var parsedString = Newtonsoft.Json.Linq.JObject.Parse(item.ToString());
                    //    Dictionary<string, string> _currentTableData = new Dictionary<string, string>() { };
                    //    foreach (var dataRow in parsedString)
                    //    {
                    //        _currentTableData.Add(dataRow.Key, dataRow.Value.ToString());

                    //    }
                    //    _currentTableDataList.Add(_currentTableData);
                    //}

                    //currentTableDataList.Clear();
                    //currentTableDataList = _currentTableDataList;
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
    }
}