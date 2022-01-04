$(function () {
    $("#BtnInitiative").on("click", function () {
        var min_py = 0; var max_py = 0;
        var py = '@profileData.ProjectYear';
        min_py = (+py - 1);
        max_py = (+py);
        StartMonth.SetMinDate(new Date(min_py + '-01-01'));
        StartMonth.SetMaxDate(new Date((max_py + 1) + '-12-31'));
        EndMonth.SetMinDate(new Date(max_py + '-01-01'));
        EndMonth.SetMaxDate(new Date((max_py + 1) + '-12-31'));

        $.post('@Url.Content("~/ActiveInitiative/GrdSubCountryPartial")', { Id: null }, function (data) {
            var obj; GrdSubCountryPopup.ClearItems();
            GrdSubCountryPopup.AddItem("[Please Select]", null);
            $.each(data[0]["SubCountryData"], function (key, value) {
                value = JSON.stringify(value); obj = JSON.parse(value);
                if (obj != null) GrdSubCountryPopup.AddItem(obj.SubCountryName, obj.id);
            });
            GrdSubCountryPopup.SelectIndex(0);
        });
        $.post('@Url.Content("~/ActiveInitiative/GetInfoForPopUp")', { Id: null }, function (data) {
            var obj; GrdInitType.ClearItems(); GrdActionType.ClearItems(); GrdSynImpact.ClearItems(); GrdInitStatus.ClearItems(); TxPortName.ClearItems(); GrdInitCategory.ClearItems(); CboWebinarCat.ClearItems();
            $.each(data[0]["SavingTypeData"], function (key, value) {
                value = JSON.stringify(value); obj = JSON.parse(value);
                if (obj != null) GrdInitType.AddItem(obj.SavingTypeName, obj.id);
            });
            $.each(data[0]["ActionTypeData"], function (key, value) {
                value = JSON.stringify(value); obj = JSON.parse(value);
                if (obj != null) GrdActionType.AddItem(obj.ActionTypeName, obj.id);
            });
            $.each(data[0]["SynImpactData"], function (key, value) {
                value = JSON.stringify(value); obj = JSON.parse(value);
                if (obj != null) GrdSynImpact.AddItem(obj.SynImpactName, obj.id);
            });
            $.each(data[0]["InitStatusData"], function (key, value) {
                value = JSON.stringify(value); obj = JSON.parse(value);
                if (obj != null) GrdInitStatus.AddItem(obj.Status, obj.id);
            });
            $.each(data[0]["PortNameData"], function (key, value) {
                value = JSON.stringify(value); obj = JSON.parse(value);
                if (obj != null) TxPortName.AddItem(obj.PortName, obj.id);
            });
            $.each(data[0]["MCostTypeData"], function (key, value) {
                value = JSON.stringify(value); obj = JSON.parse(value);
                if (obj != null) GrdInitCategory.AddItem(obj.CostTypeName, obj.id);
            });
            $.each(data[0]["MSourceCategory"], function (key, value) {
                value = JSON.stringify(value); obj = JSON.parse(value);
                if (obj != null) CboWebinarCat.AddItem(obj.categoryname, obj.id);
            });
            GrdInitType.SelectIndex(0); GrdActionType.SelectIndex(0); GrdSynImpact.SelectIndex(0); GrdInitStatus.SelectIndex(0); TxPortName.SelectIndex(0); GrdInitCategory.SelectIndex(0); CboWebinarCat.SelectIndex(0);
        });
        CboHoValidity.AddItem("Y"); CboHoValidity.AddItem("N");
        CboWebinarCat.AddItem("");
        CboRPOCValidity.AddItem("KO", "KO"); CboRPOCValidity.AddItem("Under Review", "UR"); CboRPOCValidity.AddItem("OK Level 1 - L1 if FY Target > 200 kUSD (L1= Cost controller)", "L1");
        CboRPOCValidity.AddItem("OK Level 2 - L2 if FY Target > 300 kUSD (L2 =Management RO)", "L2"); CboRPOCValidity.AddItem("OK Level 3 - L3 if FY Target > 500 kUSD (L3 = Coordinateur HO)", "L3");
        $("#FormStatus").val("New");
        $("#btnDuplicate").prop("disabled", true);

        var years_right = '@years_right';
        var project_year = '@profileData.ProjectYear';

        if (years_right.includes(project_year)) {
            $("#btnSave").prop('disabled', false);
        } else {
            $("#btnSave").prop('disabled', true);
        }

        WindowInitiative.Show();
        /*            StartMonth.SetMaxDate(new Date(max_py + '-12-31'));*/
    });
});

function OnInitiativeTypeChanged(s, e) {
    var id = s.GetValue();
    $.post("@Url.Content("~/ActiveInitiative/GetItemFromInitiativeType")", { id: id }, function (data) {
        var obj; CostCategoryID.ClearItems();
        $.each(data[0]["CostTypeData"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj != null) CostCategoryID.AddItem(obj.CostTypeName, obj.id);
        });
        CostCategoryID.SelectIndex(0);
    });
}

function OnCostCategoryChanged(s, e) {
    var id = InitiativeType.GetValue(InitiativeType.GetSelectedIndex()); var id2 = s.GetValue(); var id3 = BrandID.GetValue(BrandID.GetSelectedIndex());
    $.post("@Url.Content("~/ActiveInitiative/GetItemFromCostCategory")", { id: id, id2: id2, id3: id3 }, function (data) {
        var obj; SubCostCategoryID.ClearItems(); ActionTypeID.ClearItems();
        $.each(data[0]["SubCostData"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj != null) SubCostCategoryID.AddItem(obj.SubCostName, obj.id);
        });
        $.each(data[0]["ActionTypeData"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj != null) ActionTypeID.AddItem(obj.ActionTypeName, obj.id);
        });
        SubCostCategoryID.SelectIndex(0); ActionTypeID.SelectIndex(0);
    });
}

function OnSubCountryChanged(s, e) {
    var id = s.GetValue(); /*var teks = s.GetText();*/
    $.post('@Url.Content("~/ActiveInitiative/GetCountryBySub")', { id: id }, function (data) {
        var obj; CountryID.ClearItems(); BrandID.ClearItems(); RegionID.ClearItems(); SubRegionID.ClearItems(); ClusterID.ClearItems(); RegionalOfficeID.ClearItems(); CostControlID.ClearItems();
        LegalEntityID.ClearItems(); InitiativeType.ClearItems();
        $.each(data[0]["CountryData"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj != null) CountryID.AddItem(obj.CountryName, obj.id);
        });
        $.each(data[0]["BrandData"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj != null) BrandID.AddItem(obj.BrandName, obj.id);
        });
        $.each(data[0]["RegionData"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj != null) RegionID.AddItem(obj.RegionName, obj.id);
        });
        $.each(data[0]["SubRegionData"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj != null) SubRegionID.AddItem(obj.SubRegionName, obj.id);
        });
        $.each(data[0]["ClusterData"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj != null) ClusterID.AddItem(obj.ClusterName, obj.id);
        });
        $.each(data[0]["RegionalOfficeData"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj != null) RegionalOfficeID.AddItem(obj.RegionalOfficeName, obj.id);
        });
        $.each(data[0]["CostControlSiteData"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj != null) CostControlID.AddItem(obj.CostControlSiteName, obj.id);
        });
        $.each(data[0]["LegalEntityData"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj != null) LegalEntityID.AddItem(obj.LegalEntityName, obj.id);
        });
        $.each(data[0]["TypeInitiativeData"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj != null) InitiativeType.AddItem(obj.SavingTypeName, obj.id);
        });
        CountryID.SelectIndex(0); BrandID.SelectIndex(0); RegionID.SelectIndex(0); SubRegionID.SelectIndex(0); ClusterID.SelectIndex(0); RegionalOfficeID.SelectIndex(0); CostControlID.SelectIndex(0);
        LegalEntityID.SelectIndex(0); InitiativeType.SelectIndex(0);
    });
}

function OnBrandChanged(s, e) {
    var id = s.GetValue();
    var countryid = CountryID.GetValue();
    $.post('@Url.Content("~/ActiveInitiative/GetLegalFromBrand")', { brandid: id, countryid: countryid }, function (data) {
        var obj; LegalEntityID.ClearItems();
        $.each(data[0]["LegalEntityData"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj != null) LegalEntityID.AddItem(obj.LegalEntityName, obj.id);
        });
        LegalEntityID.SelectIndex(0);
    });
}

function onContextMenuItemFormGridViewClick(s, e) {

    var key = e.elementIndex;
    var rptId = s.GetRowKey(key);

    switch (e.item.name) {
        case 'Info':
            alert('Edit Detail => s.GetRowKey(key) = ' + rptId + ' || e.elementIndex = ' + key);
            break;
        case 'EditDetail':
            ShowEditWindow(rptId);
            break;
    }
}

function onRowDoubleClick(s, e) {
    var key = e.visibleIndex;
    var rptId = s.GetRowKey(key);

    ShowEditWindow(rptId);
    //WarningDeleteReport("Detail Info For This ARO Counting Number " + rptId, rptId);
}

function PretDut() {
    //GrdSubCountry.GetGridView().SetFocusedRowIndex(0);
    GrdSubCountryPopup.GetGridView().GetItemValues(0, 'SubCountryName', OnGetRowValues);
    //GrdLegalEntity.SetText = '';
    //GrdLegalEntity.GetGridView().GetRowValues(0, 'LegalEntityName', OnGetRowValues);
}

function OnGetRowValues2(value) {
    GrdLegalEntity.SetText = value;
    //GrdLegalEntity.SetValue(value);
}

function OnGetRowValues(value) {
    GrdSubCountryPopup.SetText = value;
}

function ShowEditWindow(id) {
    var min_py = 0; var max_py = 0;
    var py = '@profileData.ProjectYear';
    min_py = (+py - 1);
    max_py = (+py);
    StartMonth.SetMinDate(new Date(min_py + '-01-01'));
    StartMonth.SetMaxDate(new Date((max_py + 1) + '-12-31'));
    EndMonth.SetMinDate(new Date(max_py + '-01-01'));
    EndMonth.SetMaxDate(new Date((max_py + 1) + '-12-31'));

    $("#FormStatus").val("Edit");
    $.post('@Url.Content("~/ActiveInitiative/GetInfoForPopUp")', { Id: id }, function (data) {
        var obj; GrdInitType.ClearItems(); GrdActionType.ClearItems(); GrdSynImpact.ClearItems(); GrdInitStatus.ClearItems(); TxPortName.ClearItems(); GrdInitCategory.ClearItems(); GrdSubCost.ClearItems();
        var uType = '@profileData.UserType';
        CboWebinarCat.ClearItems();
        $.each(data[0]["SavingTypeData"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj != null) GrdInitType.AddItem(obj.SavingTypeName, obj.id);
        });
        $.each(data[0]["ActionTypeData"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj != null) GrdActionType.AddItem(obj.ActionTypeName, obj.id);
        });
        $.each(data[0]["SynImpactData"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj != null) GrdSynImpact.AddItem(obj.SynImpactName, obj.id);
        });
        $.each(data[0]["InitStatusData"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj != null) GrdInitStatus.AddItem(obj.Status, obj.id);
        });
        $.each(data[0]["PortNameData"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj != null) TxPortName.AddItem(obj.PortName, obj.id);
        });
        $.each(data[0]["MCostTypeData"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj != null) GrdInitCategory.AddItem(obj.CostTypeName, obj.id);
        });
        $.each(data[0]["MSubCostData"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj != null) GrdSubCost.AddItem(obj.SubCostName, obj.id);
        });
        $.each(data[0]["MSourceCategory"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj != null) CboWebinarCat.AddItem(obj.categoryname, obj.id);
        });
        GrdInitType.SelectIndex(0); GrdActionType.SelectIndex(0); GrdSynImpact.SelectIndex(0); GrdInitStatus.SelectIndex(0); TxPortName.SelectIndex(0); GrdInitCategory.SelectIndex(0); GrdSubCost.SelectIndex(0); CboWebinarCat.SelectIndex(0);
    });

    CboHoValidity.AddItem("Y"); CboHoValidity.AddItem("N");
    CboRPOCValidity.AddItem("KO", "KO"); CboRPOCValidity.AddItem("Under Review", "UR"); CboRPOCValidity.AddItem("OK Level 1 - L1 if FY Target > 200 kUSD (L1= Cost controller)", "L1");
    CboRPOCValidity.AddItem("OK Level 2 - L2 if FY Target > 300 kUSD (L2 =Management RO)", "L2"); CboRPOCValidity.AddItem("OK Level 3 - L3 if FY Target > 500 kUSD (L3 = Coordinateur HO)", "L3");

    $.post('@Url.Content("~/ActiveInitiative/GetInfoById")', { Id: id }, function (data) {
        if (data != '') {
            var obj = JSON.parse(data); var SubCountryID = obj.SubCountryID;
            var uType = '@profileData.UserType'; var projectYear = '@profileData.ProjectYear';
            //console.log("ShowEditWindow->SubCountryID = " + obj.SubCountryID);
            $("#FormID").val(obj.id);
            $("#LblInitiative").text(obj.InitNumber);
            LblRelatedInitiative.SetValue(obj.RelatedInitiative);
            $("#GrdCountry").val(obj.CountryName);
            $("#GrdCountryVal").val(obj.CountryID);
            $("#GrdRegional").val(obj.RegionName);
            $("#GrdRegionalVal").val(obj.RegionID);
            $("#GrdSubRegion").val(obj.SubRegionName);
            $("#GrdSubRegionVal").val(obj.SubRegionID);
            $("#GrdCluster").val(obj.ClusterName);
            $("#GrdClusterVal").val(obj.ClusterID);
            $("#GrdRegionalOffice").val(obj.RegionalOffice_Name);
            $("#GrdRegionalOfficeVal").val(obj.RegionalOfficeID);
            CboConfidential.SetValue(obj.Confidential);
            StartMonth.SetValue(new Date(obj.StartMonth));
            EndMonth.SetValue(new Date(obj.EndMonth));
            CboWebinarCat.SetValue(obj.SourceCategory);
            $("#GrdCostControl").val(obj.CostControlSiteName);
            $("#GrdCostControlVal").val(obj.CostControlID);
            //$("#GrdInitCategory").val(obj.CostTypeName);
            //$("#GrdSubCost").val(obj.SubCostName);
            $("#GrdActionType").val(obj.ActionTypeName);
            $("#GrdSynImpact").val(obj.SynImpactName);
            $("#TxResponsibleName").val(obj.ResponsibleFullName);
            $("#TxLaraCode").val(obj.LaraCode);
            $("#TxDesc").val(obj.Description);
            GrdInitStatus.SetValue(obj.InitStatus);
            TxPortName.SetValue(obj.PortID);
            GrdInitType.SetValue(obj.InitiativeType);
            GrdInitCategory.SetValue(obj.CostCategoryID);
            GrdSubCost.SetValue(obj.SubCostCategoryID);
            GrdActionType.SetValue(obj.ActionTypeID);
            GrdSynImpact.SetValue(obj.SynergyImpactID);
            $("#TxVendorSupp").val(obj.VendorName);
            $("#TxAdditionalInfo").val(obj.AdditionalInfo);
            $("#TxAgency").val(obj.AgencyComment);
            $("#TxRPOCComment").val(obj.RPOCComment);
            $("#TxHOComment").val(obj.HOComment);
            CboHoValidity.SetValue(obj.HOValidity);
            CboRPOCValidity.SetValue(obj.RPOCControl);
            txTarget12.SetValue(obj.TargetTY);
            txTargetFullYear.SetValue(obj.TargetNY);
            txYTDTargetFullYear.SetValue(obj.YTDTarget);
            txYTDSavingFullYear.SetValue(obj.YTDAchieved);

            OnStartMonthChanged();

            if ((new Date(obj.StartMonth).getFullYear()) < projectYear) {
                $(".targetjan").val(formatValue(obj.TargetNexJan)); $(".targetfeb").val(formatValue(obj.TargetNexFeb)); $(".targetmar").val(formatValue(obj.TargetNexMar)); $(".targetapr").val(formatValue(obj.TargetNexApr)); $(".targetmay").val(formatValue(obj.TargetNexMay)); $(".targetjun").val(formatValue(obj.TargetNexJun));
                $(".targetjul").val(formatValue(obj.TargetNexJul)); $(".targetaug").val(formatValue(obj.TargetNexAug)); $(".targetsep").val(formatValue(obj.TargetNexSep)); $(".targetoct").val(formatValue(obj.TargetNexOct)); $(".targetnov").val(formatValue(obj.TargetNexNov)); $(".targetdec").val(formatValue(obj.TargetNexDec));
                $(".savingjan").val(formatValue(obj.AchNexJan)); $(".savingfeb").val(formatValue(obj.AchNexFeb)); $(".savingmar").val(formatValue(obj.AchNexMar)); $(".savingapr").val(formatValue(obj.AchNexApr)); $(".savingmay").val(formatValue(obj.AchNexMay)); $(".savingjun").val(formatValue(obj.AchNexJun));
                $(".savingjul").val(formatValue(obj.AchNexJul)); $(".savingaug").val(formatValue(obj.AchNexAug)); $(".savingsep").val(formatValue(obj.AchNexSep)); $(".savingoct").val(formatValue(obj.AchNexOct)); $(".savingnov").val(formatValue(obj.AchNexNov)); $(".savingdec").val(formatValue(obj.AchNexDec));
            } else {
                $(".targetjan").val(formatValue(obj.TargetJan)); $(".targetfeb").val(formatValue(obj.TargetFeb)); $(".targetmar").val(formatValue(obj.TargetMar)); $(".targetapr").val(formatValue(obj.TargetApr)); $(".targetmay").val(formatValue(obj.TargetMay)); $(".targetjun").val(formatValue(obj.TargetJun));
                $(".targetjul").val(formatValue(obj.TargetJul)); $(".targetaug").val(formatValue(obj.TargetAug)); $(".targetsep").val(formatValue(obj.TargetSep)); $(".targetoct").val(formatValue(obj.TargetOct)); $(".targetnov").val(formatValue(obj.TargetNov)); $(".targetdec").val(formatValue(obj.TargetDec));
                $(".savingjan").val(formatValue(obj.AchJan)); $(".savingfeb").val(formatValue(obj.AchFeb)); $(".savingmar").val(formatValue(obj.AchMar)); $(".savingapr").val(formatValue(obj.AchApr)); $(".savingmay").val(formatValue(obj.AchMay)); $(".savingjun").val(formatValue(obj.AchJun));
                $(".savingjul").val(formatValue(obj.AchJul)); $(".savingaug").val(formatValue(obj.AchAug)); $(".savingsep").val(formatValue(obj.AchSep)); $(".savingoct").val(formatValue(obj.AchOct)); $(".savingnov").val(formatValue(obj.AchNov)); $(".savingdec").val(formatValue(obj.AchDec));

                $(".targetjan2").val(formatValue(obj.TargetNexJan)); $(".targetfeb2").val(formatValue(obj.TargetNexFeb)); $(".targetmar2").val(formatValue(obj.TargetNexMar)); $(".targetapr2").val(formatValue(obj.TargetNexApr)); $(".targetmay2").val(formatValue(obj.TargetNexMay)); $(".targetjun2").val(formatValue(obj.TargetNexJun));
                $(".targetjul2").val(formatValue(obj.TargetNexJul)); $(".targetaug2").val(formatValue(obj.TargetNexAug)); $(".targetsep2").val(formatValue(obj.TargetNexSep)); $(".targetoct2").val(formatValue(obj.TargetNexOct)); $(".targetnov2").val(formatValue(obj.TargetNexNov)); $(".targetdec2").val(formatValue(obj.TargetNexDec));
                $(".savingjan2").val(formatValue(obj.AchNexJan)); $(".savingfeb2").val(formatValue(obj.AchNexFeb)); $(".savingmar2").val(formatValue(obj.AchNexMar)); $(".savingapr2").val(formatValue(obj.AchNexApr)); $(".savingmay2").val(formatValue(obj.AchNexMay)); $(".savingjun2").val(formatValue(obj.AchNexJun));
                $(".savingjul2").val(formatValue(obj.AchNexJul)); $(".savingaug2").val(formatValue(obj.AchNexAug)); $(".savingsep2").val(formatValue(obj.AchNexSep)); $(".savingoct2").val(formatValue(obj.AchNexOct)); $(".savingnov2").val(formatValue(obj.AchNexNov)); $(".savingdec2").val(formatValue(obj.AchNexDec));
            }

            if (obj.InitStatus == 4) {
                $(".txSaving").prop("disabled", true);
            } else if (obj.InitStatus == 1 && uType == 3) {
                $("#btnSave").prop("disabled", true);
            } else if (obj.InitStatus == 2 && uType == 3) //status is final and user is not HO or RO
            {
                $("#btnSave").prop("disabled", true);
            }

            var legalentityidx = obj.LegalEntityID;
            //GrdInitStatus.GetGridView().Refresh();
            //GrdInitType.GetGridView().Refresh();

            $.post('@Url.Content("~/ActiveInitiative/GetCountryBySub")', { Id: SubCountryID, Id2: id }, function (data) {
                var obj2;
                GrdSubCountryPopup.ClearItems(); GrdBrandPopup.ClearItems(); GrdLegalEntityPopup.ClearItems();
                $.each(data[0]["SubCountryData"], function (key, value) {
                    value = JSON.stringify(value); obj2 = JSON.parse(value);
                    if (obj != null) GrdSubCountryPopup.AddItem(obj2.SubCountryName, obj2.id);
                });
                $.each(data[0]["BrandData"], function (key, value) {
                    value = JSON.stringify(value); obj2 = JSON.parse(value);
                    if (obj != null) GrdBrandPopup.AddItem(obj2.BrandName, obj2.id);
                });
                $.each(data[0]["LegalEntityData"], function (key, value) {
                    value = JSON.stringify(value); obj2 = JSON.parse(value);
                    if (obj != null) GrdLegalEntityPopup.AddItem(obj2.LegalEntityName, obj2.id);
                });
                GrdSubCountryPopup.SelectIndex(0); GrdBrandPopup.SelectIndex(0); //GrdLegalEntityPopup.SelectIndex(0);
                GrdLegalEntityPopup.SetValue(legalentityidx);
            });
            getYtdValue();

            var years_right = '@years_right';
            var project_year = '@profileData.ProjectYear';

            if (years_right.includes(project_year)) {
                $("#btnSave").prop('disabled', '');
            } else {
                $("#btnSave").prop('disabled', 'disabled');
            }

            $("#TxAgency, #TxRPOCComment, #TxHOComment").prop("disabled", true);
            if (uType == 1) $("#TxHOComment").prop("disabled", false);
            if (uType == 2) $("#TxRPOCComment").prop("disabled", false);
            if (uType == 3) $("#TxAgency").prop("disabled", false);

            //if (uType == 3) $(".txTarget").prop("disabled", true);

            OnEndMonthChanged();

            var formstatus; var initstatusvalue;
            formstatus = $("#FormStatus").val();
            initstatusvalue = GrdInitStatus.GetValue();
            if (uType == 3 && formstatus == "Edit" && initstatusvalue != "3" && initstatusvalue != "4") {
                $("#chkAuto").prop("disabled", true);
                $(".txTarget").prop("disabled", true);
                StartMonth.clientEnabled = false; EndMonth.clientEnabled = false;
            }

            WindowInitiative.Show();
        }
    });
}

function DeleteReport(id) {
    WarningDeleteReport("Detail Info For This ARO Counting Number " + id, id);
}

function WarningDeleteReport(msg, id) {
    Swal.fire({
        title: 'Detail Info',
        text: msg,
        type: 'info',
        cancelButtonColor: 'btn btn-success'
    }).then((result) => {
        if (result.value) {
            doDeleteReport(id);
        }
    });

    return false;
}

function doDeleteReport(id) {
    $.ajax({
        type: "POST",
        url: urlDeleteReport,
        data: { id: id },
        beforeSend: function () {
            ___lp.Show();
        },
        error: function (jqXHR, textStatus, thrownError) {
            ___lp.Hide();

            if (jqXHR.responseText) {
                if (jqXHR.responseText == "__sessiontimeout") {
                    window.location = _urlLogOff;
                }
            }
            else {
                var errMsg = "Sorry, There is a problem. ";
                if (textStatus.length > 0)
                    errMsg += "(" + textStatus + ").";
                FailMsg(errMsg);
            }
        },
        success: function (response) {
            ___lp.Hide();
            if (response.status == 'success') {
                cmrMainGridView.PerformCallback();
                SuccessMsg("Report successfully deleted.");
            }
            else if (response.status == 'fail') {
                isThereNewRecord = false;
                FailMsg(response.error_message);
            }
            else {
                if (response == "__sessiontimeout")
                    window.location = urlLogOffOrg;
                else
                    FailMsg(response.error_message)
            }
        }
    });
}

function OnCloseNewInitiativeWindow() {
    $("#LblInitiative").text(@DateTime.Now.Year + "XX####");
    GrdSubCountryPopup.SetValue(null);
    GrdBrandPopup.SetValue(null);
    GrdBrandPopup.ClearItems();
    GrdLegalEntityPopup.SetValue(null);
    GrdLegalEntityPopup.ClearItems();
    //GrdCountry.SetValue(null);
    //GrdRegional.SetValue(null);
    //GrdSubRegion.SetValue(null);
    //GrdCluster.SetValue(null);
    //GrdRegionalOffice.SetValue(null);
    //GrdCostControl.SetValue(null);
    CboConfidential.SetValue(null);
    $(".inputan-popup").val(null);
    $("#GrdCountry").val(null);
    $("#GrdRegional").val(null);
    $("#GrdSubRegion").val(null);
    $("#GrdCluster").val(null);
    $("#GrdRegionalOffice").val(null);
    GrdInitStatus.SetValue(null);
    $("#GrdCostControl").val(null);
    $("#GrdInitCategory").val(null);
    GrdInitCategory.ClearItems();
    $("#GrdSubCost").val(null);
    $("#GrdActionType").val(null);
    $("#GrdSynImpact").val(null);
    $("#TxResponsibleName").val(null);
    $("#TxLaraCode").val(null);
    $("#TxDesc").val(null);
    $("#TxPortName").val(null);
    $("#TxVendorSupp").val(null);
    $("#TxAdditionalInfo").val(null);
    $("#TxAgency").val(null);
    $("#TxRPOCComment").val(null);
    $("#TxHOComment").val(null);
    $("#chkAuto").prop('checked', false);
    $(".txTarget").prop('disabled', false);
    $('.txSaving, .txTarget').val('');
    $('.txSaving,#btnDuplicate,#btnSave,#TxResponsibleName,#TxDesc,#TxLaraCode,#TxPortName,#TxVendorSupp,#TxAdditionalInfo,#TxAgency,#TxRPOCComment,#TxHOComment').prop('disabled', false);
    GrdInitType.SetValue(null);
    GrdInitStatus.ClearItems();
    TxPortName.ClearItems();
    GrdInitType.ClearItems();
    GrdActionType.ClearItems();
    CboHoValidity.ClearItems();
    CboRPOCValidity.ClearItems();
    CboRPOCValidity.ClearItems();
    GrdSubCost.ClearItems();
    txTarget12.SetValue();
    txTargetFullYear.SetValue();
    txYTDTargetFullYear.SetValue();
    txYTDSavingFullYear.SetValue();
    //GrdInitCategory.SetValue(null);
    //GrdSubCost.SetValue(null);
    GrdActionType.SetValue(null);
    //GrdSynImpact.SetValue(null);
    StartMonth.SetValue(null);
    EndMonth.SetValue(null);
    LblRelatedInitiative.SetValue(null);
    CboWebinarCat.ClearItems();
    $("#chkAuto").prop('disabled', false);
    $(".txTarget").prop("disabled", false);
    $("input[name='StartMonth']").prop('disabled', false);
    $("input[name='StartMonth']").prop('readonly', false);
    StartMonth.clientEnabled = true;
    EndMonth.clientEnabled = true;

    //GrdLegalEntity.SetText = '';
    $.post('@Url.Content("~/ActiveInitiative/RemoveSelectedGridLookup")', function (data) {
        console.log(data);
    });
    //GrdBrand.GetGridView().Refresh();
}

function OnInit(s, e) {
    GrdSubCountryPopup.SelectIndex(0);
    AdjustSize();
}

function OnEndCallback(s, e) {
    AdjustSize();
    UpdateFigureWidget(s);

}

function UpdateFigureWidget(grid) {
    $('#sp-num-of-init').html(grid.cpNumberOfInitiative);
    $('#sp-total-target').html(grid.cpTotalTarget);
    $('#sp-total-saving-achieved').html(grid.cpTotalSavingAchieved);
}

function AdjustSize() {
    var height = window.innerHeight - 250;//(screen.height - 450);//document.documentElement.clientHeight;
    GrdMainInitiative.SetHeight(height);
}

function BtnExportXls() {
    GrdMainInitiative.ExportTo("Xls");
}

function BtnExportPdf() {
    GrdMainInitiative.ExportTo("Pdf");
}

function OnSubCountryPopupChanged(s, e) {
    var id = s.GetValue();
    $("#GrdCountryVal").val(''); $("#GrdCountry").val(''); $("#GrdRegionalVal").val(''); $("#GrdRegional").val('');
    $("#GrdSubRegionVal").val(''); $("#GrdSubRegion").val(''); $("#GrdClusterVal").val(''); $("#GrdCluster").val('');
    $("#GrdRegionalOfficeVal").val(''); $("#GrdRegionalOffice").val(''); $("#GrdCostControlVal").val(''); $("#GrdCostControl").val('');
    $.post('@Url.Content("~/ActiveInitiative/GetCountryBySub")', { id: id }, function (data) {
        var obj; GrdBrandPopup.ClearItems(); GrdLegalEntityPopup.ClearItems();
        GrdBrandPopup.AddItem("[ Please Select ]", 0);
        $.each(data[0]["BrandData"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj != null) {
                GrdBrandPopup.AddItem(obj.BrandName, obj.id);
            }
        });
        $.each(data[0]["CountryData"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj != null) {
                $("#GrdCountryVal").val(obj.id); $("#GrdCountry").val(obj.CountryName);
            }
        });
        $.each(data[0]["RegionData"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj != null) {
                $("#GrdRegionalVal").val(obj.id); $("#GrdRegional").val(obj.RegionName);
            }
        });
        $.each(data[0]["SubRegionData"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj != null) {
                $("#GrdSubRegionVal").val(obj.id); $("#GrdSubRegion").val(obj.SubRegionName);
            }
        });
        $.each(data[0]["ClusterData"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj != null) {
                $("#GrdClusterVal").val(obj.id); $("#GrdCluster").val(obj.ClusterName);
            }
        });
        $.each(data[0]["RegionalOfficeData"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj != null) {
                $("#GrdRegionalOfficeVal").val(obj.id); $("#GrdRegionalOffice").val(obj.RegionalOfficeName);
            }
        });
        $.each(data[0]["CostControlSiteData"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj != null) {
                $("#GrdCostControlVal").val(obj.id); $("#GrdCostControl").val(obj.CostControlSiteName);
            }
        });
        GrdBrandPopup.SelectIndex(0);
    });
}

function OnBrandPopupChanged(s, e) {
    var id = s.GetValue(); var Country = $("#GrdCountryVal").val(); var SubCountry = GrdSubCountryPopup.GetValue(); var CostControlSite = $("#GrdCostControlVal").val();
    $.post('@Url.Content("~/ActiveInitiative/GetLegalFromBrand")', { BrandID: id, CountryID: Country, SubCountryID: SubCountry, CostControlSiteID: CostControlSite }, function (data) {
        var obj; GrdLegalEntityPopup.ClearItems();
        $.each(data[0]["LegalEntityData"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj != null) GrdLegalEntityPopup.AddItem(obj.LegalEntityName, obj.id);
        });
        GrdLegalEntityPopup.SelectIndex(0);
    });
}

function OnGrdInitTypeChanged(s, e) {
    var id = s.GetValue();
    $.post('@Url.Content("~/ActiveInitiative/GetItemFromInitiativeType")', { id: id }, function (data) {
        var obj; GrdInitCategory.ClearItems();
        $.each(data[0]["CostTypeData"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj != null) GrdInitCategory.AddItem(obj.CostTypeName, obj.id);
        });
        GrdInitCategory.SelectIndex(0);
    });
}

function OnSubCostPopupChanged(s, e) {
    var id = s.GetValue();
    $.post('@Url.Content("~/ActiveInitiative/GetItemFromSubCost")', { id: id }, function (data) {
        var obj; GrdActionType.ClearItems();
        $.each(data[0]["ActionTypeData"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj != null) GrdActionType.AddItem(obj.ActionTypeName, obj.id);
        });
        GrdActionType.SelectIndex(0);
    });
}

function OnGrdInitCategoryPopupChanged(s, e) {
    var id = GrdInitType.GetValue(); var id2 = s.GetValue(); var id3 = GrdBrandPopup.GetValue();
    $.post('@Url.Content("~/ActiveInitiative/GetItemFromCostCategory")', { id: id, id2: id2, id3: id3 }, function (data) {
        var obj; GrdSubCost.ClearItems();
        $.each(data[0]["SubCostData"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj != null) GrdSubCost.AddItem(obj.SubCostName, obj.id);
        });
        GrdSubCost.SelectIndex(0);
    });
}

function OnCboYearChanged(s, e) {
    var id = s.GetText();//s.GetValue();
    $.post("@Url.Content("~/ActiveInitiative/ProjectYear")", { Id: id }, function () {
        window.location.reload();
    });
}

function OnClickEventReview(s, e) {
    var idx = s.GetRowKey(e.visibleIndex);
    $('.titleinitiative').html('');
    $.post("@Url.Content("~/EventReview/SetEventReviewID")", { ID: idx }, function (data) {
        //WindowEventReview.SetContentHtml(data);
        $('.titleinitiative').html('Initiative Number: ' + data);
        GrdEventReview.Refresh();
        WindowEventReview.Show();
    });
}

function OnClickComment(s, e) {
    var id = s.GetRowKey(e.visibleIndex);
    $.post("@Url.Content("~/ActiveInitiative/GetInitiativeComment")", { Id: id }, function (data) {
        var AgencyComment = data.AgencyComment;
        var RPOCComment = data.RPOCComment;
        var HOComment = data.HOComment;
        if ((AgencyComment == null) || (AgencyComment == '')) AgencyComment = 'Not Commented Yet';
        if ((RPOCComment == null) || (RPOCComment == '')) RPOCComment = 'Not Commented Yet';
        if ((HOComment == null) || (HOComment == '')) HOComment = 'Not Commented Yet';

        $(".agencyexcerpt").html(AgencyComment);
        $(".rpocexcerpt").html(RPOCComment);
        $(".hoexcerpt").html(HOComment);
        WindowComment.Show();
    });
}

function OnClickUpload(s, e) {
    var idx = s.GetRowKey(e.visibleIndex);
    $.post("@Url.Content("~/ActiveInitiative/SetIDUploadFile")", { ID: idx }, function (data) {
        var obj;
        var InitNumber = data[0]["InitiativeNumber"];
        $(".UploadInitiativeNumber").html(InitNumber);

        $.each(data[0]["UploadedFileData"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj.UploadedFileName != null) {
                var output = ""; var arrFileName = obj.UploadedFileName.split("|");
                if (arrFileName[0] != '') {
                    for (var x = 0; x < arrFileName.length; x++) {
                        output += "<tr><td width=\"660\"><a href=\"@Url.Content(UploadDirectory)" + arrFileName[x] + "\" target=\"_new\" >" + arrFileName[x] + "</td><td><button type=\"button\" class=\"btn btn-danger btn-xs\" onClick=\"removefile('" + InitNumber + "','" + arrFileName[x] + "',this)\" >Remove</button></td></tr>";
                        x++;
                    }
                    $("#summary-uploaded-files").html(output);
                } else {
                    $("#summary-uploaded-files").html("<tr><td colspan=\"2\"><center>There is no File Uploaded</center></td></tr>");
                }

            } else {
                $("#summary-uploaded-files").html("<tr><td colspan=\"2\"><center>There is no File Uploaded</center></td></tr>");
            }
        });
        WindowUpload.Show();
    });
}

function onUploadControlFileUploadComplete(s, e) {
    if (e.callbackData) {
        var isisekarang = $("#summary-uploaded-files").html();
        var fileData = e.callbackData.split('|');
        var fileName = fileData[0];
        var initiativenumber = fileData[1];

        var classStatus = "label-warning";
        if (status == "success")
            classStatus = "label-success";
        else if (status == "fail")
            classStatus = "label-danger";

        @*var rowCount = $("#summary-uploaded-files tr").length;
        var columnSumm = $("<tr><td><a href=\"@Url.Content(UploadDirectory)" + fileName + "\" target=\"_new\">" + fileName + "</a></td></tr>");
        console.log(columnSumm.html());*@

            if (isisekarang == '<tr><td colspan="2"><center>There is no File Uploaded</center></td></tr>') {
            $("#summary-uploaded-files").html('');
        }
        $("#summary-uploaded-files").append("<tr><td width='660'><a href=\"@Url.Content(UploadDirectory)" + fileName + "\" target=\"_new\">" + fileName + "</a></td><td><button type=\"button\" class=\"btn btn-danger btn-xs\" onClick=\"removefile('" + initiativenumber + "','" + fileName + "',this)\" >Remove</button></td></tr>");
        GrdMainInitiative.Refresh();
    }
}

function removefile(initiativenumber, filename, btndel) {
    Swal.fire({
        title: 'Confirmation',
        text: 'Are you sure to remove this file ?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Remove'
    }).then((result) => {
        if (result.value) {
            $.post("@Url.Content("~/ActiveInitiative/removefile")", { Initiativenumber: initiativenumber, Filename: filename }, function (data) {
                console.log(data);
                $(btndel).closest('tr').remove();

                var tmp = $("#summary-uploaded-files").html();
                if (tmp == "") $("#summary-uploaded-files").html("<tr><td colspan=\"2\"><center>There is no File Uploaded</center></td></tr>");
            });
        }
    });
}

function OnStartMonthChanged() {
    OnEndMonthChanged();

    var awal = StartMonth.GetValue();
    var dt = new Date(awal.getFullYear(), awal.getMonth() + 11, 1);
    EndMonth.SetMaxDate(dt);
}

function OnEndMonthChanged() {
    var targetstartselected = false; var targetstartselected2 = true; var startmon; var projectYear = '@profileData.ProjectYear';
    var endmonthValue = '';
    if (EndMonth.GetValue() == null) {
        endmonthValue = StartMonth.GetValue();
    } else {
        endmonthValue = EndMonth.GetValue();
    }
    var targetty = new Array(); var savingty = new Array();
    if ((new Date(StartMonth.GetValue()).getFullYear()) == projectYear) {
        targetty = Array("targetjan", "targetfeb", "targetmar", "targetapr", "targetmay", "targetjun", "targetjul", "targetaug", "targetsep", "targetoct", "targetnov", "targetdec", "targetjan2", "targetfeb2", "targetmar2", "targetapr2", "targetmay2", "targetjun2", "targetjul2", "targetaug2", "targetsep2", "targetoct2", "targetnov2", "targetdec2");
        savingty = Array("savingjan", "savingfeb", "savingmar", "savingapr", "savingmay", "savingjun", "savingjul", "savingaug", "savingsep", "savingoct", "savingnov", "savingdec", "savingjan2", "savingfeb2", "savingmar2", "savingapr2", "savingmay2", "savingjun2", "savingjul2", "savingaug2", "savingsep2", "savingoct2", "savingnov2", "savingdec2");

        startmon = ((moment(StartMonth.GetValue()).format("M")) - 1);
        var months; var x = 0;
        months = (endmonthValue.getFullYear() - StartMonth.GetValue().getFullYear()) * 12;
        months -= StartMonth.GetValue().getMonth();
        months += endmonthValue.getMonth();
        months++; // ngitung selisih bulan mulai dan bulan akhir initiative

        for (var i = 0; i <= targetty.length; i++) {
            if (i >= startmon && x < months) {
                targetstartselected = true;
            } else {
                targetstartselected = false;
            }

            if (targetstartselected == true) {
                $("." + targetty[i]).prop('disabled', false);
                $("." + savingty[i]).prop('disabled', false);
                x++;
            } else {
                $("." + targetty[i]).val('');
                $("." + savingty[i]).val('');
                $("." + targetty[i]).prop('disabled', true);
                $("." + savingty[i]).prop('disabled', true);
            }
        }
    } else if ((new Date(StartMonth.GetValue()).getFullYear()) < projectYear) {
        targetty = Array("targetjan", "targetfeb", "targetmar", "targetapr", "targetmay", "targetjun", "targetjul", "targetaug", "targetsep", "targetoct", "targetnov", "targetdec", "targetjan2", "targetfeb2", "targetmar2", "targetapr2", "targetmay2", "targetjun2", "targetjul2", "targetaug2", "targetsep2", "targetoct2", "targetnov2", "targetdec2");
        savingty = Array("savingjan", "savingfeb", "savingmar", "savingapr", "savingmay", "savingjun", "savingjul", "savingaug", "savingsep", "savingoct", "savingnov", "savingdec", "savingjan2", "savingfeb2", "savingmar2", "savingapr2", "savingmay2", "savingjun2", "savingjul2", "savingaug2", "savingsep2", "savingoct2", "savingnov2", "savingdec2");

        var endmonth = ((moment(endmonthValue).format("M")) - 1);
        for (var i = 0; i <= targetty.length; i++) {
            if (i <= endmonth) {
                $("." + targetty[i]).prop('disabled', false);
                $("." + savingty[i]).prop('disabled', false);
            } else {
                $("." + targetty[i]).val('');
                $("." + savingty[i]).val('');
                $("." + targetty[i]).prop('disabled', true);
                $("." + savingty[i]).prop('disabled', true);
            }
        }

        $("input[name='StartMonth']").prop('disabled', true);
        $("input[name='StartMonth']").prop('readonly', true);
        StartMonth.clientEnabled = false;
    } else if ((new Date(StartMonth.GetValue()).getFullYear()) > projectYear) {
        targetty = Array("targetjan", "targetfeb", "targetmar", "targetapr", "targetmay", "targetjun", "targetjul", "targetaug", "targetsep", "targetoct", "targetnov", "targetdec", "targetjan2", "targetfeb2", "targetmar2", "targetapr2", "targetmay2", "targetjun2", "targetjul2", "targetaug2", "targetsep2", "targetoct2", "targetnov2", "targetdec2");
        savingty = Array("savingjan", "savingfeb", "savingmar", "savingapr", "savingmay", "savingjun", "savingjul", "savingaug", "savingsep", "savingoct", "savingnov", "savingdec", "savingjan2", "savingfeb2", "savingmar2", "savingapr2", "savingmay2", "savingjun2", "savingjul2", "savingaug2", "savingsep2", "savingoct2", "savingnov2", "savingdec2");

        var startmonth = ((moment(StartMonth.GetValue()).format("M")) - 1);
        var endmonth = ((moment(endmonthValue).format("M")));
        for (var i = 0; i <= targetty.length; i++) {
            if (i > (+startmonth + 11) && i <= (+endmonth + 11)) {
                $("." + targetty[i]).prop('disabled', false);
                $("." + savingty[i]).prop('disabled', false);
            } else {
                $("." + targetty[i]).val('');
                $("." + savingty[i]).val('');
                $("." + targetty[i]).prop('disabled', true);
                $("." + savingty[i]).prop('disabled', true);
            }
        }
    }

    var formstatus; var initstatusvalue; var uType;
    uType = '@profileData.UserType';
    formstatus = $("#FormStatus").val();
    initstatusvalue = GrdInitStatus.GetValue();
    if (uType == 3 && formstatus == "Edit" && initstatusvalue != "4") {
        $("#chkAuto").prop("disabled", true);
        $(".txTarget").prop("disabled", true);
    }
}

function OnInitStatusChanged(s, e) {
    var id = s.GetText();
    var uType = '@profileData.UserType';
    var xFormStatus = $("#FormStatus").val();

    if (uType == 3 && id == "Pending" && xFormStatus != "New") {
        GrdInitStatus.SelectIndex(0);
        Swal.fire(
            'You can not change the status',
            'Agency user can not change to pending',
            'error'
        );
    }
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode != 45 && charCode != 46 && charCode > 31
        && (charCode < 48 || charCode > 57)) {
        Swal.fire(
            'Numeric only',
            'You can not input alphanumeric character here',
            'error'
        );
        return false;
    }
    return true;
}

function OnClickEditInitiative(s, e) {
    var idx = s.GetRowKey(e.visibleIndex);
    ShowEditWindow(idx);
}

function formatValue(n) {
    if (n != null && n != "") {
        n = String(n).replaceAll(',', '');
        return parseFloat(n).toFixed(2).replaceAll(/\d(?=(\d{3})+\.)/g, '$&,');
    } else if (n == 0) {
        return 0;
    } else {
        return "";
    }

}

$(function () {
    setTimeout(function () {
        UpdateFigureWidget(GrdMainInitiative);
    }, 300);
});
