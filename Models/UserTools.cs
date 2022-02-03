using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DevExpress.Spreadsheet;

namespace GAIN.Models
{
    public class UploadStatusExportXls
    {
        public UploadStatusExportXls()
        {
            row_status = new List<ExportUserPerRowStatus>();
        }
        public int TotalRow { get; set; }
        public string message { get; set; }
        public string error_message { get; set; }
        public string status { get; set; }
        public string numRowSource { get; set; }
        public string numRowSuccess { get; set; }
        public string successPercentage { get; set; }

        public List<ExportUserPerRowStatus> row_status { get; set; }
    }

    public class ExportUserPerRowStatus
    {
        public ExportUserPerRowStatus()
        {
            error_message = new List<string>();
        }
        public string row { get; set; }
        public string status { get; set; }
        public string success_message { get; set; }

        public List<string> error_message { get; set; }
        public user_list user { get; set; }
    }

    public static class UploadingUtils
    {
        const string RemoveTaskKeyPrefix = "DXRemoveTask_";

        #region RemoveFileWithDelay
        public static void RemoveFileWithDelay(string key, string fullPath, int delay)
        {
            RemoveFileWithDelayInternal(key, fullPath, delay, FileSystemRemoveAction);
        }

        private static void RemoveFileWithDelayInternal(string key, string fullPath, int delay, Action<string, object, CacheItemRemovedReason> fileSystemRemoveAction)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region MyRegion
        static void FileSystemRemoveAction(string key, object value, CacheItemRemovedReason reason)
        {
            string fileFullPath = value.ToString();
            if (File.Exists(fileFullPath))
                File.Delete(fileFullPath);
        }
        #endregion

        #region RemoveFileWithDelayInternal
        static void RemoveFileWithDelayInternal(string fileKey, object fileData, int delay, CacheItemRemovedCallback removeAction)
        {
            string key = RemoveTaskKeyPrefix + fileKey;
            if (HttpRuntime.Cache[key] == null)
            {
                DateTime absoluteExpiration = DateTime.UtcNow.Add(new TimeSpan(0, delay, 0));
                HttpRuntime.Cache.Insert(key, fileData, null, absoluteExpiration,
                    Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, removeAction);
            }
        }
        #endregion
    }

    #region ExportUserListExcelToDb
    public class ExportUserListExcelToDb
    {
        private readonly GainEntities _db;

        public ExportUserListExcelToDb(GainEntities db)
        {
            _db = db;
        }
        public UploadStatusExportXls UserExcelDataToDatabase(string sourceExcelFilePath, string UserName)
        {
            StatusOptionModel StatusOptionModel = new StatusOptionModel();
            List<SimpleModel> listStatus = StatusOptionModel.StatusOption();

            BoolOptionModel BolOptionModel = new BoolOptionModel();
            List<SimpleModel> listBoolean = BolOptionModel.BoolOption();

            UserTypeOptionModel UserTypeOptionModel = new UserTypeOptionModel();
            List<SimpleModel> listUserType = UserTypeOptionModel.UserTypeOption();

            UploadStatusExportXls model = new UploadStatusExportXls();
            List<user_list> newUserList = new List<user_list>();
            List<user_list> savedUserList = new List<user_list>();
            //List<ExportUserPerRowStatus> statusReports = new List<ExportUserPerRowStatus>();
            //List<payment> model = new List<payment>();
            int successImportedRow = 0;
            int numSourceRow = 0;
            try
            {
                using (FileStream stream = new FileStream(sourceExcelFilePath, FileMode.Open))
                {
                    Workbook workbook = new Workbook();
                    workbook.LoadDocument(stream);
                    Worksheet worksheet = workbook.Worksheets[0];
                    RowCollection rows = worksheet.Rows;
                    CellRange dataRange = worksheet.GetUsedRange();

                    numSourceRow = dataRange.RowCount;
                    int numCols = dataRange.ColumnCount;
                    model.numRowSource = String.Format("{0:N0}", numSourceRow - 1);

                    DateTime curDateTime = DateTime.UtcNow; ;
                    List<string> headersFile = new List<string>();
                    for (int row = 0; row < numSourceRow; row++)
                    {
                        user_list user = new user_list() { USER_MIDDLE_NAME = "-", DEPARTMENT_CODE = "-", PICTURE_PATH = "-" };
                        for (int j = 0; j < numCols; j++)
                        {
                            Cell cell = worksheet.Cells[row, j];

                            cell.Value.Type.ToString();
                            if (row == 0)
                                headersFile.Add(cell.Value.ToString());
                            else
                            {
                                switch (headersFile[j])
                                {
                                    case "#":
                                        if (cell.Value.IsEmpty)
                                            user.user_id = 0;
                                        else
                                        {
                                            int uid = 0;
                                            if (int.TryParse(cell.Value.ToString().Trim(), out uid)){
                                                user.user_id = uid;
                                            }
                                        }
                                        break;

                                    case "User Name":
                                        if (!cell.Value.IsEmpty)
                                            user.username = cell.Value.ToString().Trim();
                                        break;

                                    case "First Name":
                                        if (!cell.Value.IsEmpty)
                                            user.USER_FIRST_NAME = cell.Value.ToString().Trim().ToLower();
                                        break;

                                    case "Last Name":
                                        if (!cell.Value.IsEmpty)
                                            user.USER_LAST_NAME = cell.Value.ToString().Trim().ToLower();
                                        break;

                                    case "Country Code":
                                        if (!cell.Value.IsEmpty)
                                            user.COUNTRY_CODE = cell.Value.ToString();
                                        break;

                                    case "Company Code":
                                        if (!cell.Value.IsEmpty)
                                            user.Company_code = cell.Value.ToString();
                                        break;

                                    case "Status":
                                        if (!cell.Value.IsEmpty && !String.IsNullOrWhiteSpace(cell.Value.ToString()))
                                        {
                                            SimpleModel m = listStatus.Find(x => x.def.Trim().ToLower() == cell.Value.ToString().Trim().ToLower());
                                            if (m != null)
                                                user.status = m.id;
                                        }
                                        break;

                                    case "Is To Admin":
                                        if (!cell.Value.IsEmpty && !String.IsNullOrWhiteSpace(cell.Value.ToString()))
                                        {
                                            SimpleModel m = listBoolean.Find(x => x.def.Trim().ToLower() == cell.Value.ToString().Trim().ToLower());
                                            if (m != null)
                                                user.istoadmin = m.id;
                                        }
                                        break;

                                    case "E-Mail":
                                        if (!cell.Value.IsEmpty)
                                            user.email = cell.Value.ToString();
                                        break;

                                    case "Is New":
                                        if (!cell.Value.IsEmpty && !String.IsNullOrWhiteSpace(cell.Value.ToString()))
                                        {
                                            SimpleModel m = listBoolean.Find(x => x.def.Trim().ToLower() == cell.Value.ToString().Trim().ToLower());
                                            if (m != null)
                                                user.isNew = m.id;
                                        }
                                        break;

                                    case "Country Right":
                                        if (!cell.Value.IsEmpty && !String.IsNullOrWhiteSpace(cell.Value.ToString()))
                                            user.country_right = cell.Value.ToString();
                                        break;

                                    case "Sub Country Right":
                                        if (!cell.Value.IsEmpty && !String.IsNullOrWhiteSpace(cell.Value.ToString()))
                                            user.subcountry_right = cell.Value.ToString();
                                        break;

                                    case "Region Right":
                                        if (!cell.Value.IsEmpty)
                                            user.region_right = cell.Value.ToString();
                                        break;

                                    case "Sub Region Right":
                                        if (!cell.Value.IsEmpty)
                                            user.subregion_right = cell.Value.ToString();
                                        break;

                                    case "Regional Office Right":
                                        if (!cell.Value.IsEmpty)
                                            user.RegionalOffice_right = cell.Value.ToString();
                                        break;

                                    case "Cost Control Site Right":
                                        if (!cell.Value.IsEmpty)
                                            user.CostControlSite_right = cell.Value.ToString();
                                        break;

                                    case "Brand Right":
                                        if (!cell.Value.IsEmpty)
                                            user.Brand_right = cell.Value.ToString();
                                        break;

                                    case "Cost Item Right":
                                        if (!cell.Value.IsEmpty)
                                            user.CostItem_right = cell.Value.ToString();
                                        break;

                                    case "Sub Cost Item Right":
                                        if (!cell.Value.IsEmpty)
                                            user.SubCostItem_right = cell.Value.ToString();
                                        break;

                                    case "Validity Right":
                                        if (!cell.Value.IsEmpty && !String.IsNullOrWhiteSpace(cell.Value.ToString()))
                                        {
                                            SimpleModel m = listBoolean.Find(x => x.def.Trim().ToLower() == cell.Value.ToString().Trim().ToLower());
                                            if (m != null)
                                                user.validity_right = m.id;
                                        }
                                        break;
                                    case "Confidential Right":
                                        if (!cell.Value.IsEmpty && !String.IsNullOrWhiteSpace(cell.Value.ToString()))
                                        {
                                            SimpleModel m = listBoolean.Find(x => x.def.Trim().ToLower() == cell.Value.ToString().Trim().ToLower());
                                            if (m != null)
                                                user.confidential_right = m.id;
                                        }
                                        break;
                                    case "User Type":
                                        if (!cell.Value.IsEmpty && !String.IsNullOrWhiteSpace(cell.Value.ToString()))
                                        {
                                            SimpleModel m = listUserType.Find(x => x.def.Trim().ToLower() == cell.Value.ToString().Trim().ToLower());
                                            if (m != null)
                                                user.userType = m.id;
                                        }
                                        break;
                                    case "Years Right":
                                        if (!cell.Value.IsEmpty)
                                            user.years_right = cell.Value.ToString();
                                        break;
                                }
                            }
                        } // end of j (column)
                        if (row == 0)
                        {
                            if (!isHeaderValid(headersFile))
                            {
                                model.numRowSource = "0";
                                model.error_message = "Invalid Format Uploaded File.";
                                break;
                            }

                        }
                        else
                        {
                            newUserList.Add(user);
                        }
                    }// end of i (row)
                } // end of using FileStream
            }

            catch (Exception ex)
            {
                model.error_message = GetInnermostExceptionMessage(ex);
            }

            if (newUserList.Count > 0)
            {
                int row = 1;
                foreach (user_list user in newUserList)
                {
                    ExportUserPerRowStatus itemStatus = new ExportUserPerRowStatus { row = row.ToString(), user = user };

                    DbEntityValidationResult result = _db.Entry(user).GetValidationResult();
                    if (!result.IsValid)
                    {
                        itemStatus.status = "NOK";
                        foreach (DbValidationError error in result.ValidationErrors)
                        {
                            itemStatus.error_message.Add(error.ErrorMessage);
                        }
                    }
                    else
                    {
                        int affectedRow = 0;
                        try
                        {
                            if (user.user_id == 0)
                            {
                                if(_db.user_list.Any(d => d.username.Trim().ToLower() == user.username.Trim().ToLower()))
                                {
                                    itemStatus.error_message.Add(String.Format("Usename {0} already exist.", user.username));
                                    itemStatus.status = "EXIST";
                                }
                                else
                                {
                                    _db.user_list.Add(user);
                                }
                            }
                            else
                            {
                                user_list existUser = _db.user_list.Find(user.user_id);
                                if (existUser != null)
                                {
                                    existUser.username = user.username;
                                    existUser.email = user.email;
                                    existUser.USER_FIRST_NAME = user.USER_FIRST_NAME;
                                    existUser.USER_LAST_NAME = user.USER_LAST_NAME;
                                    existUser.COUNTRY_CODE = user.COUNTRY_CODE;
                                    existUser.Company_code = user.Company_code;
                                    existUser.status = user.status;
                                    existUser.istoadmin = user.istoadmin;
                                    existUser.isNew = user.isNew;
                                    existUser.country_right = user.country_right;
                                    existUser.subcountry_right = user.subcountry_right;
                                    existUser.region_right = user.region_right;
                                    existUser.subregion_right = user.subregion_right;
                                    existUser.RegionalOffice_right = user.RegionalOffice_right;
                                    existUser.CostControlSite_right = user.CostControlSite_right;
                                    existUser.Brand_right = user.Brand_right;
                                    existUser.CostItem_right = user.CostItem_right;
                                    existUser.SubCostItem_right = user.SubCostItem_right;
                                    existUser.validity_right = user.validity_right;
                                    existUser.confidential_right = user.confidential_right;
                                    existUser.userType = user.userType;
                                    existUser.years_right = user.years_right;
                                }
                            }
                            affectedRow = _db.SaveChanges();
                        }
                        catch (DbEntityValidationException ex)
                        {
                            // Retrieve the error messages as a list of strings.
                            foreach (DbEntityValidationResult error in ex.EntityValidationErrors)
                            {
                                foreach (var inErr in error.ValidationErrors)
                                {
                                    itemStatus.error_message.Add(inErr.ErrorMessage);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            string err = GetInnermostExceptionMessage(ex);
                            itemStatus.status = "NOK";
                            itemStatus.error_message.Add(err);
                        }

                        if (affectedRow > 0)
                        {
                            itemStatus.status = "OK";
                        }
                        else
                        {
                            if(user.user_id > 0)
                            {
                                itemStatus.status = "NOUPDATE";
                                itemStatus.error_message.Add(String.Format("No data have changed.", user.username));

                            }
                            else
                                itemStatus.status = "NOK";
                        }
                    }
                    //user_list existUser = _db.user_list.Where(d => d.username.Trim().ToLower() == user.username.Trim().ToLower()).FirstOrDefault();
                    //if (existUser != null)
                    //{
                    //    itemStatus.error_message.Add(String.Format("User {0} already exist", user.username));
                    //    itemStatus.status = "EXIST";
                    //}
                    //else
                    //{

                    //}
                    model.row_status.Add(itemStatus);
                    row++;
                }
            }

            return model;
        }

        int? ConvertDefToId(string val)
        {
            int idVal;
            if(int.TryParse(val, out idVal))
            {
                return idVal;
            }
            return null;
        }

        bool isHeaderValid(List<string> headers)
        {
            if (headers.Contains("User Name") && headers.Contains("First Name") && headers.Contains("Last Name") && headers.Contains("Country Code") && headers.Contains("Company Code"))
                return true;
            return false;
        }

        private string GetInnermostExceptionMessage(Exception exception)
        {
            if (exception.InnerException != null)
                return GetInnermostExceptionMessage(exception.InnerException);

            return exception.Message;
        }
    }
    #endregion
    

}