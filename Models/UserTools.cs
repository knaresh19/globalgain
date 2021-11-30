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
                        user_list newUser = new user_list() { USER_MIDDLE_NAME = "-", DEPARTMENT_CODE = "-", PICTURE_PATH = "-" };
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
                                    case "User Name":
                                        if (cell.Value != null)
                                            newUser.username = cell.Value.ToString().Trim();
                                        break;

                                    case "First Name":
                                        if (cell.Value != null)
                                            newUser.USER_FIRST_NAME = cell.Value.ToString().Trim().ToLower();
                                        break;

                                    case "Last Name":
                                        if (cell.Value != null)
                                            newUser.USER_LAST_NAME = cell.Value.ToString().Trim().ToLower();
                                        break;

                                    case "Country Code":
                                        if (cell.Value != null)
                                            newUser.COUNTRY_CODE = cell.Value.ToString();
                                        break;

                                    case "Company Code":
                                        if (cell.Value != null)
                                            newUser.Company_code = cell.Value.ToString();
                                        break;

                                    case "Status":
                                        if (cell.Value != null && !String.IsNullOrWhiteSpace(cell.Value.ToString()))
                                        {
                                            SimpleModel m = listStatus.Find(x => x.def.Trim().ToLower() == cell.Value.ToString().Trim().ToLower());
                                            if (m != null)
                                                newUser.status = m.id;
                                        }
                                        break;

                                    case "Is To Admin":
                                        if (cell.Value != null && !String.IsNullOrWhiteSpace(cell.Value.ToString()))
                                        {
                                            SimpleModel m = listBoolean.Find(x => x.def.Trim().ToLower() == cell.Value.ToString().Trim().ToLower());
                                            if (m != null)
                                                newUser.istoadmin = m.id;
                                        }
                                        break;

                                    case "E-Mail":
                                        if (cell.Value != null)
                                            newUser.email = cell.Value.ToString();
                                        break;

                                    case "Is New":
                                        if (cell.Value != null && !String.IsNullOrWhiteSpace(cell.Value.ToString()))
                                        {
                                            SimpleModel m = listBoolean.Find(x => x.def.Trim().ToLower() == cell.Value.ToString().Trim().ToLower());
                                            if (m != null)
                                                newUser.isNew = m.id;
                                        }
                                        break;

                                    case "Country Right":
                                        if (cell.Value != null && !String.IsNullOrWhiteSpace(cell.Value.ToString()))
                                            newUser.country_right = cell.Value.ToString();
                                        break;

                                    case "Sub Country Right":
                                        if (cell.Value != null && !String.IsNullOrWhiteSpace(cell.Value.ToString()))
                                            newUser.subcountry_right = cell.Value.ToString();
                                        break;

                                    case "Region Right":
                                        if (cell.Value != null)
                                            newUser.region_right = cell.Value.ToString();
                                        break;

                                    case "Sub Region Right":
                                        if (cell.Value != null)
                                            newUser.subregion_right = cell.Value.ToString();
                                        break;

                                    case "Regional Office Right":
                                        if (cell.Value != null)
                                            newUser.RegionalOffice_right = cell.Value.ToString();
                                        break;

                                    case "Cost Control Site Right":
                                        if (cell.Value != null)
                                            newUser.CostControlSite_right = cell.Value.ToString();
                                        break;

                                    case "Brand Right":
                                        if (cell.Value != null)
                                            newUser.Brand_right = cell.Value.ToString();
                                        break;

                                    case "Cost Item Right":
                                        if (cell.Value != null)
                                            newUser.CostItem_right = cell.Value.ToString();
                                        break;

                                    case "Sub Cost Item Right":
                                        if (cell.Value != null)
                                            newUser.SubCostItem_right = cell.Value.ToString();
                                        break;

                                    case "Validity Right":
                                        if (cell.Value != null && !String.IsNullOrWhiteSpace(cell.Value.ToString()))
                                        {
                                            SimpleModel m = listBoolean.Find(x => x.def.Trim().ToLower() == cell.Value.ToString().Trim().ToLower());
                                            if (m != null)
                                                newUser.validity_right = m.id;
                                        }
                                        break;
                                    case "Confidential Right":
                                        if (cell.Value != null && !String.IsNullOrWhiteSpace(cell.Value.ToString()))
                                        {
                                            SimpleModel m = listBoolean.Find(x => x.def.Trim().ToLower() == cell.Value.ToString().Trim().ToLower());
                                            if (m != null)
                                                newUser.confidential_right = m.id;
                                        }
                                        break;
                                    case "User Type":
                                        if (cell.Value != null && !String.IsNullOrWhiteSpace(cell.Value.ToString()))
                                        {
                                            SimpleModel m = listUserType.Find(x => x.def.Trim().ToLower() == cell.Value.ToString().Trim().ToLower());
                                            if (m != null)
                                                newUser.userType = m.id;
                                        }
                                        break;
                                    case "Years Right":
                                        if (cell.Value != null)
                                            newUser.years_right = cell.Value.ToString();
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
                            newUserList.Add(newUser);
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

                    user_list existUser = _db.user_list.Where(d => d.username.Trim().ToLower() == user.username.Trim().ToLower()).FirstOrDefault();
                    if (existUser != null)
                    {
                        itemStatus.error_message.Add(String.Format("User {0} already exist", user.username));
                        itemStatus.status = "EXIST";
                        
                        
                    }
                    else
                    {
                        var result = _db.Entry(user).GetValidationResult();
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
                                _db.user_list.Add(user);
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
                                itemStatus.status = "NOK";
                            }
                        }
                    }
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