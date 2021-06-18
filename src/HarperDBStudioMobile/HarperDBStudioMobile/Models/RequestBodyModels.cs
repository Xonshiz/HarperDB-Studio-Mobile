using System;
using System.Collections.Generic;

namespace HarperDBStudioMobile.Models
{
    //{"email":"user@user.com","password":"UserPassword","loggingIn":true}
    public class RequestGetUserModel
    {
        public string email { get; set; }
        public string password { get; set; }
        public bool loggingIn { get; set; }
    }

    //{"org":"somevaluehere","subdomain":"somevaluehere","user_id":"somevaluehere"}
    public class RequestCreateOrganizationModel
    {
        public string org { get; set; }
        public string subdomain { get; set; }
        public string user_id { get; set; }
    }

    //{"customer_id":"123"}
    public class RequestGetCustomerModel
    {
        public string customer_id { get; set; }
    }

    //{"customer_id":"123","stripe_id":"123"}
    public class RequestGetPrepaidSubscriptionModel
    {
        public string customer_id { get; set; }
        public string stripe_id { get; set; }
    }

    //{"customer_id":"123","user_id":"123"}
    public class RequestGetInstancesModel
    {
        public string customer_id { get; set; }
        public string user_id { get; set; }
    }

    //{"customer_id":"123123","user_id":"123"}
    public class RequestGetAlarmsModel
    {
        public string customer_id { get; set; }
        public string user_id { get; set; }
    }

    //{"user_id":"12111111","tc_version":"2020-01-01","customer_id":"123123"}
    public class RequestAddTCAcceptanceModel
    {
        public string user_id { get; set; }
        public string tc_version { get; set; }
        public string customer_id { get; set; }
    }

    //{"user_id":"12111111","customer_id":"123123","instance_name":"newcloudin","is_local":false,"is_ssl":true,"instance_region":"us-east-2",
    //"instance_type":"t3.nano","stripe_plan_id":"price_1GzWzBDzYFZjJeDRhExOUyi5","data_volume_size":1,"stripe_storage_plan_id":"price_1GzpHmDzYFZjJeDRZRp7DOA6"}
    public class RequestAddInstanceModel
    {
        public string customer_id { get; set; }
        public string user_id { get; set; }
        public string instance_name { get; set; }
        public bool is_local { get; set; }
        public bool is_ssl { get; set; }
        public string instance_region { get; set; }
        public string instance_type { get; set; }
        public string stripe_plan_id { get; set; }
        public int data_volume_size { get; set; }
        public string stripe_storage_plan_id { get; set; }
    }

    //Huge Model JSON to be added here.
    public class RequestAddErrorModel
    {
        public string type { get; set; }
        public string status { get; set; }
        public string environment { get; set; }
        public string user { get; set; }
        public string customer_id { get; set; }
        public string url { get; set; }
        public string operation { get; set; }
        public string timestamp { get; set; }
        public ErrorModel error { get; set; }
        public RequestAddInstanceModel request { get; set; }
    }

    //{"operation":"list_roles"}
    public class RequestOperationsModel
    {
        public string operation { get; set; }
    }

    //{"operation":"add_user","role":"super_user","username":"222222","password":"11111","active":true}
    public class RequestOperationsAddUserModel
    {
        public string operation { get; set; }
        public string role { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public bool active { get; set; }
    }

    //{"operation":"drop_user","username":"i-03e4cb2dc9d762524"}
    public class RequestOperationsDropUserModel
    {
        public string operation { get; set; }
        public string username { get; set; }
    }

    //{"compute_stack_id":"2222","customer_id":"123123","fingerprint":"1212312312"}
    public class RequestCreateLicenseModel
    {
        public string compute_stack_id { get; set; }
        public string customer_id { get; set; }
        public string fingerprint { get; set; }
    }

    //{"operation":"set_license","key":"keyvalue","company":"123123"}
    public class RequestSetLicenseModel
    {
        public string operation { get; set; }
        public string key { get; set; }
        public string company { get; set; }
    }

    //{"operation":"create_schema","schema":"newSchema3"}
    public class RequestCreateSchemaModel
    {
        public string operation { get; set; }
        public string schema { get; set; }
    }

    //{"operation":"create_table","schema":"newSchema3","table":"newTable2","hash_attribute":"recId"}
    public class RequestCreateTableModel
    {
        public string operation { get; set; }
        public string schema { get; set; }
        public string table { get; set; }
        public string hash_attribute { get; set; }
    }

    //{"operation":"describe_table","schema":"newSchema3","table":"newTable2"}
    public class RequestDescribeTableModel
    {
        public string operation { get; set; }
        public string schema { get; set; }
        public string table { get; set; }
    }

    //{"operation":"insert","schema":"newSchema3","table":"newTable2","records":[{"name":"my Name","email":"adsasd@gmail.comm"}]}
    public class RequestInsertRecordModel
    {
        public string operation { get; set; }
        public string schema { get; set; }
        public string table { get; set; }
        public List<dynamic> records { get; set; }
    }

    //{"operation":"update","schema":"newSchema","table":"newTable","records":[{"email":"asdasd@gmail.comm","name":"My new Name","recId":"sssssss"}]}
    public class RequestUpdateRecordModel
    {
        public string operation { get; set; }
        public string schema { get; set; }
        public string table { get; set; }
        public List<dynamic> records { get; set; }
    }

    //{"operation":"search_by_hash","schema":"newSchema","table":"newTable","hash_values":["sasdasdas"],"get_attributes":["*"]}
    public class RequestGetRecordDetailsModel
    {
        public string operation { get; set; }
        public string schema { get; set; }
        public string table { get; set; }
        public List<string> hash_values { get; set; }
        public List<string> get_attributes { get; set; }
    }

    //{"operation":"delete","schema":"newSchema","table":"newTable","hash_values":["ssssssss"]}
    public class RequestDeleteRecordModel
    {
        public string operation { get; set; }
        public string schema { get; set; }
        public string table { get; set; }
        public List<string> hash_values { get; set; }
    }

    //{"operation":"sql","sql":"SELECT * FROM `newSchema`.`newTable` ORDER BY `recId` DESC OFFSET 0 FETCH 20"}
    public class RequestSqlActionModel
    {
        public string operation { get; set; }
        public string sql { get; set; }
    }

}
