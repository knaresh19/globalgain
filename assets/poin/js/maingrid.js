////@{
////    var profileData = this.Session["DefaultGAINSess"] as GAIN.Models.LoginSession;
////    var user_type = profileData.UserType;
////    var years_right = profileData.years_right;
////    var projectYear = profileData.ProjectYear;
////    var projectMonth = profileData.ProjectMonth;
////}

function URLContent(url) {
    return UrlContent + url;
}

$(function () {
    $("#BtnInitiative").on("click", function () {

        $('#isProcurement').val('0');

        var isProcurement = 0;
        clear_Procurement_BackCalcs();
        clear_Procurement_fields();

        var min_py = 0; var max_py = 0;
        var py = projectYear;
        min_py = (+py - 1);
        max_py = (+py);

        // debugger;
        StartMonth.SetMinDate(new Date(min_py + '-01-01'));
        StartMonth.SetMaxDate(new Date(max_py + '-12-31'));
        EndMonth.SetMinDate(new Date(max_py + '-01-01'));
        EndMonth.SetMaxDate(new Date((max_py + 1) + '-12-31'));

        if (projectYear > 2022) { StartMonth.SetMinDate(new Date((max_py) + '-01-01')); }


        $.post(URLContent('ActiveInitiative/GrdSubCountryPartial'), { Id: null }, function (data) {
            var obj; GrdSubCountryPopup.ClearItems();
            GrdSubCountryPopup.AddItem("[Please Select]", null);
            $.each(data[0]["SubCountryData"], function (key, value) {
                value = JSON.stringify(value); obj = JSON.parse(value);
                if (obj != null) GrdSubCountryPopup.AddItem(obj.SubCountryName, obj.id);
            });
            GrdSubCountryPopup.SelectIndex(0);
        });
        $.post(URLContent('ActiveInitiative/GetInfoForPopUp'), { Id: null }, function (data) {
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

            if (isProcurement == 1) {
                GrdActionType.clientEnabled = false;
                GrdActionType.SelectIndex(1);
            }
            else { GrdActionType.SelectIndex(0); }

            GrdInitType.SelectIndex(0); GrdSynImpact.SelectIndex(0); GrdInitStatus.SelectIndex(0); TxPortName.SelectIndex(0); GrdInitCategory.SelectIndex(0); CboWebinarCat.SelectIndex(0);
        });
        CboHoValidity.AddItem("Y"); CboHoValidity.AddItem("N");
        CboWebinarCat.AddItem("");
        CboRPOCValidity.AddItem("KO", "KO"); CboRPOCValidity.AddItem("Under Review", "UR"); CboRPOCValidity.AddItem("OK Level 1 - L1 if FY Target > 200 kUSD (L1= Cost controller)", "L1");
        CboRPOCValidity.AddItem("OK Level 2 - L2 if FY Target > 300 kUSD (L2 =Management RO)", "L2"); CboRPOCValidity.AddItem("OK Level 3 - L3 if FY Target > 500 kUSD (L3 = Coordinateur HO)", "L3");
        $("#FormStatus").val("New");
        $("#btnDuplicate").prop("disabled", true);

        //var years_right = years_right;
        var project_year = projectYear;

        if (years_right.includes(project_year)) {
            $("#btnSave").prop('disabled', false);
        } else {
            $("#btnSave").prop('disabled', true);
        }
        //Change the labels 
        $('#ljan').html('Jan-' + py);
        $('#lfeb').html('Feb-' + py);
        $('#lmar').html('Mar-' + py);
        $('#lapr').html('Apr-' + py);
        $('#lmay').html('May-' + py);
        $('#ljun').html('Jun-' + py);
        $('#ljul').html('Jul-' + py);
        $('#laug').html('Aug-' + py);
        $('#lsep').html('Sep-' + py);
        $('#loct').html('Oct-' + py);
        $('#lnov').html('Nov-' + py);
        $('#ldec').html('Dec-' + py);
        var npy = parseInt(py) + 1;
        $('#lnexjan').html('Jan-' + npy);
        $('#lnexfeb').html('Feb-' + npy);
        $('#lnexmar').html('Mar-' + npy);
        $('#lnexapr').html('Apr-' + npy);
        $('#lnexmay').html('May-' + npy);
        $('#lnexjun').html('Jun-' + npy);
        $('#lnexjul').html('Jul-' + npy);
        $('#lnexaug').html('Aug-' + npy);
        $('#lnexsep').html('Sep-' + npy);
        $('#lnexoct').html('Oct-' + npy);
        $('#lnexnov').html('Nov-' + npy);
        $('#lnexdec').html('Dec-' + npy);

        $('#_divProcurement').prop('style', 'display:none');
        $('#_divOptimization').prop('style', 'display:block');

        if (role_code == 'ADM' && istoadmin == 0) {
            $('#TxHOComment').prop('disabled', false);
        }
        else if (role_code == 'CRT' && istoadmin == 0) {
            $('#TxRPOCComment').prop('disabled', false);
        }
        else if (role_code == 'RO' && istoadmin == 0) {
            $('#TxAgency').prop('disabled', false);
        }
        else if (istoadmin == 1) {
            $('#TxAgency,#TxRPOCComment,#TxHOComment').prop('disabled', false);
        }

        WindowInitiative.Show();
        /*            StartMonth.SetMaxDate(new Date(max_py + '-12-31'));*/
    });
    $('#BtnProcurement').on("click", function () {

        var isProcurement = 1;
        $('#isProcurement').val('1');
        clear_Procurement_BackCalcs();
        clear_Procurement_fields();
        var min_py = 0; var max_py = 0;
        var py = projectYear;
        if (py < 2023) {
            py = 2023;
            min_py = (+py);
            max_py = (+py+1);
        } else {
            min_py = (+py);
            max_py = (+py + 1);
        }
        //debugger;
        StartMonth.SetMinDate(new Date(min_py + '-01-01'));
        StartMonth.SetMaxDate(new Date(max_py + '-12-31'));
        EndMonth.SetMinDate(new Date(min_py + '-01-01'));
        EndMonth.SetMaxDate(new Date((max_py) + '-12-31'));

        $.post(URLContent('ActiveInitiative/GrdSubCountryPartial'), { Id: null }, function (data) {
            var obj; GrdSubCountryPopup.ClearItems();
            GrdSubCountryPopup.AddItem("[Please Select]", null);
            $.each(data[0]["SubCountryData"], function (key, value) {
                value = JSON.stringify(value); obj = JSON.parse(value);
                if (obj != null) GrdSubCountryPopup.AddItem(obj.SubCountryName, obj.id);
            });
            GrdSubCountryPopup.SelectIndex(0);
        });
        $.post(URLContent('ActiveInitiative/GetInfoForPopUp'), { Id: null }, function (data) {
            var obj; GrdInitType.ClearItems(); GrdActionType.ClearItems(); GrdSynImpact.ClearItems(); GrdInitStatus.ClearItems(); TxPortName.ClearItems(); GrdInitCategory.ClearItems(); CboWebinarCat.ClearItems();
           
            $.each(data[0]["SavingTypeData"], function (key, value) {
                value = JSON.stringify(value); obj = JSON.parse(value);
                if (obj != null) {

                    if (obj.SavingTypeName == "Positive Cost Impact" || obj.SavingTypeName == "Negative Cost Impact") {
                        GrdInitType.AddItem(obj.SavingTypeName, obj.id);
                    }
                };
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

            if (isProcurement == 1) {

                GrdInitType.clientEnabled = false;
                GrdInitType.SetText("Positive Cost Impact");

                GrdActionType.clientEnabled = false;
                GrdActionType.SelectIndex(2);
            }
            else
            {
                GrdInitType.SelectIndex(0);
                GrdActionType.SelectIndex(0);
            }

            /*GrdInitType.SelectIndex(0);*/ GrdSynImpact.SelectIndex(0); GrdInitStatus.SelectIndex(0); TxPortName.SelectIndex(0); GrdInitCategory.SelectIndex(0); CboWebinarCat.SelectIndex(0);
        });
        CboHoValidity.AddItem("Y"); CboHoValidity.AddItem("N");
        CboWebinarCat.AddItem("");
        CboRPOCValidity.AddItem("KO", "KO"); CboRPOCValidity.AddItem("Under Review", "UR"); CboRPOCValidity.AddItem("OK Level 1 - L1 if FY Target > 200 kUSD (L1= Cost controller)", "L1");
        CboRPOCValidity.AddItem("OK Level 2 - L2 if FY Target > 300 kUSD (L2 =Management RO)", "L2"); CboRPOCValidity.AddItem("OK Level 3 - L3 if FY Target > 500 kUSD (L3 = Coordinateur HO)", "L3");
        $("#FormStatus").val("New");
        $("#btnDuplicate").prop("disabled", true);

        //var years_right = years_right;
        var project_year = projectYear;

        if (years_right.includes(project_year)) {
            $("#btnSave").prop('disabled', false);
        } else {
            $("#btnSave").prop('disabled', true);
        }
        //Change the labels 
        $('#ljan').html('Jan-' + py);
        $('#lfeb').html('Feb-' + py);
        $('#lmar').html('Mar-' + py);
        $('#lapr').html('Apr-' + py);
        $('#lmay').html('May-' + py);
        $('#ljun').html('Jun-' + py);
        $('#ljul').html('Jul-' + py);
        $('#laug').html('Aug-' + py);
        $('#lsep').html('Sep-' + py);
        $('#loct').html('Oct-' + py);
        $('#lnov').html('Nov-' + py);
        $('#ldec').html('Dec-' + py);
        var npy = parseInt(py) + 1;
        $('#lnexjan').html('Jan-' + npy);
        $('#lnexfeb').html('Feb-' + npy);
        $('#lnexmar').html('Mar-' + npy);
        $('#lnexapr').html('Apr-' + npy);
        $('#lnexmay').html('May-' + npy);
        $('#lnexjun').html('Jun-' + npy);
        $('#lnexjul').html('Jul-' + npy);
        $('#lnexaug').html('Aug-' + npy);
        $('#lnexsep').html('Sep-' + npy);
        $('#lnexoct').html('Oct-' + npy);
        $('#lnexnov').html('Nov-' + npy);
        $('#lnexdec').html('Dec-' + npy);

        $('#_divProcurement').prop('style', 'display:block');
        $('#_divOptimization').prop('style', 'display:none');

        
        if (role_code == 'ADM' && istoadmin == 0) {
            $('#TxHOComment').prop('disabled', false);
        }
        else if (role_code == 'CRT' && istoadmin == 0) {
            $('#TxRPOCComment').prop('disabled', false);
        }
        else if (role_code == 'RO' && istoadmin == 0) {
            $('#TxAgency').prop('disabled', false);
        }
        else if (istoadmin == 1)
        {
            $('#TxAgency,#TxRPOCComment,#TxHOComment').prop('disabled', false);
        }

        WindowInitiative.Show();
        /*            StartMonth.SetMaxDate(new Date(max_py + '-12-31'));*/
    });

    $(".txSaving, .txTarget").on("change", function () {
        $(this).val(formatValue($(this).val()));
    });

    $(".targetjan, .targetfeb, .targetmar, .targetapr, .targetmay, .targetjun, .targetjul, .targetaug, .targetsep, .targetoct, .targetnov, .targetdec").on("change", function () {
        hitungtahunini();
    });

    $(".targetjan2, .targetfeb2, .targetmar2, .targetapr2, .targetmay2, .targetjun2, .targetjul2, .targetaug2, .targetsep2, .targetoct2, .targetnov2, .targetdec2").on("change", function () {
        hitungtahunini();
    });

    $("#btnSave").on('click', function () {
        //var projectYear = '@profileData.ProjectYear';
        /*if ((new Date(StartMonth.GetValue()).getFullYear()) < projectYear) {
            Swal.fire({
                title: 'Initiative of Previous Year',
                text: 'This initiative is a previous year initiative. Total sum of month target for current year and next year will not match the initial target 12 months.',
                icon: 'warning',
                showCancelButton: false
            }).then((result) => {
                SaveInitiative();
            });
        }*/
        //else {
        SaveInitiative();
        //}
    });

    $("#btnEditx").on('click', function () {
        var n = $(this).text();
        if (n === "Cancel") {
            $(this).text("Edit");
            $("#chkAuto").prop('disabled', true);
            CheckUncheck();
        } else if (n === "Edit") {
            $(this).text("Cancel");
            $("#chkAuto").prop('disabled', false);
            CheckUncheck();
        }
    });

    $("#btnClose").on('click', function () {
        $('.txTarget').prop('disabled', false);
        $('.txTarget').prop('readonly', false);
        WindowInitiative.Hide();
    });

    $("#btnDuplicate").on("click", function () {
        var subCountry = GrdSubCountryPopup.GetText();
        var subCountryID = GrdSubCountryPopup.GetValue();
        var brand = GrdBrandPopup.GetText();
        var legal = GrdLegalEntityPopup.GetText();
        $("#FormStatus").val("New");
        $("#LblInitiative").text("DUP-0001");
        $("#btnDuplicate").prop("disabled", true);
        $.post(URLContent('ActiveInitiative/GrdSubCountryPartial'), { Id: null }, function (data) {
            var obj; GrdSubCountryPopup.ClearItems();
            $.each(data[0]["SubCountryData"], function (key, value) {
                value = JSON.stringify(value); obj = JSON.parse(value);
                GrdSubCountryPopup.AddItem(obj.SubCountryName, obj.id);
            });

            GrdSubCountryPopup.SetText(subCountry);

            $.post(URLContent('ActiveInitiative/GetCountryBySub'), { id: subCountryID }, function (data) {
                var obj2; GrdBrandPopup.ClearItems(); GrdLegalEntityPopup.ClearItems();
                $.each(data[0]["BrandData"], function (key, value) {
                    value = JSON.stringify(value); obj2 = JSON.parse(value);
                    if (obj2 != null) GrdBrandPopup.AddItem(obj2.BrandName, obj2.id);
                });
                // debugger;
                GrdBrandPopup.SetText(brand);
                var brandid = GrdBrandPopup.GetValue(); var countryid = $("#GrdCountryVal").val(); var subcountryid = GrdSubCountryPopup.GetValue(); var costcontrolsiteid = $("#GrdCostControlVal").val();
                $.post(URLContent('ActiveInitiative/GetLegalFromBrand'), { brandid: brandid, countryid: countryid, subcountryid: subcountryid, costcontrolsiteid: costcontrolsiteid }, function (data) {
                    var obj3;
                    $.each(data[0]["LegalEntityData"], function (key, value) {
                        value = JSON.stringify(value); obj3 = JSON.parse(value);
                        if (obj3 != null) GrdLegalEntityPopup.AddItem(obj3.LegalEntityName, obj3.id);
                    });
                    GrdLegalEntityPopup.SetText(legal);
                });
            });
            /*GrdSubCountryPopup.SelectIndex(0);*/
            //GrdBrandPopup.ClearItems();
            //GrdLegalEntityPopup.ClearItems();
            //$('#GrdCountry').val(''); $('#GrdCountryVal').val('');
            //$('#GrdRegional').val(''); $('#GrdRegionalVal').val('');
            //$('#GrdSubRegion').val(''); $('#GrdSubRegionVal').val('');
            //$('#GrdCluster').val(''); $('#GrdClusterVal').val('');
            //$('#GrdRegionalOffice').val(''); $('#GrdRegionalOfficeVal').val('');
            //$('#GrdCostControl').val(''); $('#GrdCostControlVal').val('');

            //var years_right = '@years_right';
            var project_year = projectYear;

            if (years_right.includes(project_year)) {
                $("#btnSave").prop('disabled', false);
            } else {
                $("#btnSave").prop('disabled', true);
            }

            StartMonth.SetValue(); EndMonth.SetValue();
            var min_py = 0; var max_py = 0;
            var py = projectYear;
            min_py = (+py - 1);
            max_py = (+py);
            StartMonth.SetMinDate(new Date(min_py + '-01-01'));
            StartMonth.SetMaxDate(new Date((max_py) + '-12-31'));
            EndMonth.SetMinDate(new Date(max_py + '-01-01'));
            EndMonth.SetMaxDate(new Date((max_py + 1) + '-12-31'));
            $(".txTarget").prop("disabled", false); $(".txSaving").prop("disabled", false); $(".txTarget").val(''); $(".txSaving").val('');
            txTarget12.SetValue(""); txTargetFullYear.SetValue(""); txYTDTargetFullYear.SetValue(""); txYTDSavingFullYear.SetValue("");

            StartMonth.clientEnabled = true;
            EndMonth.clientEnabled = true;
            $('#chkAuto').prop('disabled', false);
        });
        Swal.fire(
            'Duplicated Successfully',
            'Initiative has been duplicated successfully. You can save it now.',
            'success'
        );
    });

    setTimeout(function () {
        UpdateFigureWidget(GrdMainInitiative);
    }, 300);

});


$(function () {
    $("#BtnProcurementInitiative").on("click", function () {
        var min_py = 0; var max_py = 0;
        var py = projectYear;
        if (py < 2023) {
            py = 2023;
            min_py = (+py);
            max_py = (+py);
        } else {
            min_py = (+py);
            max_py = (+py);
        }

        // debugger;
        StartMonth.SetMinDate(new Date(min_py + '-01-01'));
        StartMonth.SetMaxDate(new Date(max_py + '-12-31'));
        EndMonth.SetMinDate(new Date(max_py + '-01-01'));
        EndMonth.SetMaxDate(new Date((max_py + 1) + '-12-31'));

        if (projectYear > 2022) { StartMonth.SetMinDate(new Date((max_py) + '-01-01')); }


        $.post(URLContent('ActiveInitiative/GrdSubCountryPartial'), { Id: null }, function (data) {
            var obj; GrdSubCountryPopup.ClearItems();
            GrdSubCountryPopup.AddItem("[Please Select]", null);
            $.each(data[0]["SubCountryData"], function (key, value) {
                value = JSON.stringify(value); obj = JSON.parse(value);
                if (obj != null) GrdSubCountryPopup.AddItem(obj.SubCountryName, obj.id);
            });
            GrdSubCountryPopup.SelectIndex(0);
        });
        $.post(URLContent('ActiveInitiative/GetInfoForPopUp'), { Id: null }, function (data) {
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
            if (projectYear > 2022) {
                //  GrdActionType.clientEnabled = false;
                GrdActionType.SelectIndex(1);
            }
            else { GrdActionType.SelectIndex(0); }

            GrdInitType.SelectIndex(0); GrdSynImpact.SelectIndex(0); GrdInitStatus.SelectIndex(0); TxPortName.SelectIndex(0); GrdInitCategory.SelectIndex(0); CboWebinarCat.SelectIndex(0);
        });
        CboHoValidity.AddItem("Y"); CboHoValidity.AddItem("N");
        CboWebinarCat.AddItem("");
        CboRPOCValidity.AddItem("KO", "KO"); CboRPOCValidity.AddItem("Under Review", "UR"); CboRPOCValidity.AddItem("OK Level 1 - L1 if FY Target > 200 kUSD (L1= Cost controller)", "L1");
        CboRPOCValidity.AddItem("OK Level 2 - L2 if FY Target > 300 kUSD (L2 =Management RO)", "L2"); CboRPOCValidity.AddItem("OK Level 3 - L3 if FY Target > 500 kUSD (L3 = Coordinateur HO)", "L3");
        $("#FormStatus").val("New");
        $("#btnDuplicate").prop("disabled", true);

        //var years_right = years_right;
        var project_year = projectYear;

        if (years_right.includes(project_year)) {
            $("#btnSave").prop('disabled', false);
        } else {
            $("#btnSave").prop('disabled', true);
        }
        //Change the labels 
        $('#ljan').html('Jan-' + py);
        $('#lfeb').html('Feb-' + py);
        $('#lmar').html('Mar-' + py);
        $('#lapr').html('Apr-' + py);
        $('#lmay').html('May-' + py);
        $('#ljun').html('Jun-' + py);
        $('#ljul').html('Jul-' + py);
        $('#laug').html('Aug-' + py);
        $('#lsep').html('Sep-' + py);
        $('#loct').html('Oct-' + py);
        $('#lnov').html('Nov-' + py);
        $('#ldec').html('Dec-' + py);
        var npy = parseInt(py) + 1;
        $('#lnexjan').html('Jan-' + npy);
        $('#lnexfeb').html('Feb-' + npy);
        $('#lnexmar').html('Mar-' + npy);
        $('#lnexapr').html('Apr-' + npy);
        $('#lnexmay').html('May-' + npy);
        $('#lnexjun').html('Jun-' + npy);
        $('#lnexjul').html('Jul-' + npy);
        $('#lnexaug').html('Aug-' + npy);
        $('#lnexsep').html('Sep-' + npy);
        $('#lnexoct').html('Oct-' + npy);
        $('#lnexnov').html('Nov-' + npy);
        $('#lnexdec').html('Dec-' + npy);
        WindowInitiative.Show();
        /*            StartMonth.SetMaxDate(new Date(max_py + '-12-31'));*/
    });

    $(".txSaving, .txTarget").on("change", function () {
        $(this).val(formatValue($(this).val()));
    });

    $(".targetjan, .targetfeb, .targetmar, .targetapr, .targetmay, .targetjun, .targetjul, .targetaug, .targetsep, .targetoct, .targetnov, .targetdec").on("change", function () {
        hitungtahunini();
    });

    $(".targetjan2, .targetfeb2, .targetmar2, .targetapr2, .targetmay2, .targetjun2, .targetjul2, .targetaug2, .targetsep2, .targetoct2, .targetnov2, .targetdec2").on("change", function () {
        hitungtahunini();
    });

    $("#btnSave").on('click', function () {
        //var projectYear = '@profileData.ProjectYear';
        /*if ((new Date(StartMonth.GetValue()).getFullYear()) < projectYear) {
            Swal.fire({
                title: 'Initiative of Previous Year',
                text: 'This initiative is a previous year initiative. Total sum of month target for current year and next year will not match the initial target 12 months.',
                icon: 'warning',
                showCancelButton: false
            }).then((result) => {
                SaveInitiative();
            });
        }*/
        //else {
        SaveInitiative();
        //}
    });

    $("#btnEditx").on('click', function () {
        var n = $(this).text();
        if (n === "Cancel") {
            $(this).text("Edit");
            $("#chkAuto").prop('disabled', true);
            CheckUncheck();
        } else if (n === "Edit") {
            $(this).text("Cancel");
            $("#chkAuto").prop('disabled', false);
            CheckUncheck();
        }
    });

    $("#btnClose").on('click', function () {
        $('.txTarget').prop('disabled', false);
        $('.txTarget').prop('readonly', false);
        WindowInitiative.Hide();
    });

    $("#btnDuplicate").on("click", function () {
        var subCountry = GrdSubCountryPopup.GetText();
        var subCountryID = GrdSubCountryPopup.GetValue();
        var brand = GrdBrandPopup.GetText();
        var legal = GrdLegalEntityPopup.GetText();
        $("#FormStatus").val("New");
        $("#LblInitiative").text("DUP-0001");
        $("#btnDuplicate").prop("disabled", true);
        $.post(URLContent('ActiveInitiative/GrdSubCountryPartial'), { Id: null }, function (data) {
            var obj; GrdSubCountryPopup.ClearItems();
            $.each(data[0]["SubCountryData"], function (key, value) {
                value = JSON.stringify(value); obj = JSON.parse(value);
                GrdSubCountryPopup.AddItem(obj.SubCountryName, obj.id);
            });

            GrdSubCountryPopup.SetText(subCountry);

            $.post(URLContent('ActiveInitiative/GetCountryBySub'), { id: subCountryID }, function (data) {
                var obj2; GrdBrandPopup.ClearItems(); GrdLegalEntityPopup.ClearItems();
                $.each(data[0]["BrandData"], function (key, value) {
                    value = JSON.stringify(value); obj2 = JSON.parse(value);
                    if (obj2 != null) GrdBrandPopup.AddItem(obj2.BrandName, obj2.id);
                });
                // debugger;
                GrdBrandPopup.SetText(brand);
                var brandid = GrdBrandPopup.GetValue(); var countryid = $("#GrdCountryVal").val(); var subcountryid = GrdSubCountryPopup.GetValue(); var costcontrolsiteid = $("#GrdCostControlVal").val();
                $.post(URLContent('ActiveInitiative/GetLegalFromBrand'), { brandid: brandid, countryid: countryid, subcountryid: subcountryid, costcontrolsiteid: costcontrolsiteid }, function (data) {
                    var obj3;
                    $.each(data[0]["LegalEntityData"], function (key, value) {
                        value = JSON.stringify(value); obj3 = JSON.parse(value);
                        if (obj3 != null) GrdLegalEntityPopup.AddItem(obj3.LegalEntityName, obj3.id);
                    });
                    GrdLegalEntityPopup.SetText(legal);
                });
            });
            /*GrdSubCountryPopup.SelectIndex(0);*/
            //GrdBrandPopup.ClearItems();
            //GrdLegalEntityPopup.ClearItems();
            //$('#GrdCountry').val(''); $('#GrdCountryVal').val('');
            //$('#GrdRegional').val(''); $('#GrdRegionalVal').val('');
            //$('#GrdSubRegion').val(''); $('#GrdSubRegionVal').val('');
            //$('#GrdCluster').val(''); $('#GrdClusterVal').val('');
            //$('#GrdRegionalOffice').val(''); $('#GrdRegionalOfficeVal').val('');
            //$('#GrdCostControl').val(''); $('#GrdCostControlVal').val('');

            //var years_right = '@years_right';
            var project_year = projectYear;

            if (years_right.includes(project_year)) {
                $("#btnSave").prop('disabled', false);
            } else {
                $("#btnSave").prop('disabled', true);
            }

            StartMonth.SetValue(); EndMonth.SetValue();
            var min_py = 0; var max_py = 0;
            var py = projectYear;
            min_py = (+py - 1);
            max_py = (+py);
            StartMonth.SetMinDate(new Date(min_py + '-01-01'));
            StartMonth.SetMaxDate(new Date((max_py) + '-12-31'));
            EndMonth.SetMinDate(new Date(max_py + '-01-01'));
            EndMonth.SetMaxDate(new Date((max_py + 1) + '-12-31'));
            $(".txTarget").prop("disabled", false); $(".txSaving").prop("disabled", false); $(".txTarget").val(''); $(".txSaving").val('');
            txTarget12.SetValue(""); txTargetFullYear.SetValue(""); txYTDTargetFullYear.SetValue(""); txYTDSavingFullYear.SetValue("");

            StartMonth.clientEnabled = true;
            EndMonth.clientEnabled = true;
            $('#chkAuto').prop('disabled', false);
        });
        Swal.fire(
            'Duplicated Successfully',
            'Initiative has been duplicated successfully. You can save it now.',
            'success'
        );
    });

    setTimeout(function () {
        UpdateFigureWidget(GrdMainInitiative);
    }, 300);

});

function clear_Procurement_BackCalcs() {
    $('#Actual_CPU_Nmin1_Jan').val('0');
    $('#Actual_CPU_Nmin1_Feb').val('0');
    $('#Actual_CPU_Nmin1_Mar').val('0');
    $('#Actual_CPU_Nmin1_Apr').val('0');
    $('#Actual_CPU_Nmin1_May').val('0');
    $('#Actual_CPU_Nmin1_Jun').val('0');
    $('#Actual_CPU_Nmin1_Jul').val('0');
    $('#Actual_CPU_Nmin1_Aug').val('0');
    $('#Actual_CPU_Nmin1_Sep').val('0');
    $('#Actual_CPU_Nmin1_Oct').val('0');
    $('#Actual_CPU_Nmin1_Nov').val('0');
    $('#Actual_CPU_Nmin1_Dec').val('0');
    $('#Actual_CPU_Nmin1_Total').val('0');
    bind_Actual_CPU_Nmin1();

    $('#Target_CPU_N_Jan').val('0');
    $('#Target_CPU_N_Feb').val('0');
    $('#Target_CPU_N_Mar').val('0');
    $('#Target_CPU_N_Apr').val('0');
    $('#Target_CPU_N_May').val('0');
    $('#Target_CPU_N_Jun').val('0');
    $('#Target_CPU_N_Jul').val('0');
    $('#Target_CPU_N_Aug').val('0');
    $('#Target_CPU_N_Sep').val('0');
    $('#Target_CPU_N_Oct').val('0');
    $('#Target_CPU_N_Nov').val('0');
    $('#Target_CPU_N_Dec').val('0');
    $('#Target_CPU_N_Total').val('0');
    bind_Target_CPU_N_Total();

    $('#A_Price_effect_Jan').val('0');
    $('#A_Price_effect_Feb').val('0');
    $('#A_Price_effect_Mar').val('0');
    $('#A_Price_effect_Apr').val('0');
    $('#A_Price_effect_May').val('0');
    $('#A_Price_effect_Jun').val('0');
    $('#A_Price_effect_Jul').val('0');
    $('#A_Price_effect_Aug').val('0');
    $('#A_Price_effect_Sep').val('0');
    $('#A_Price_effect_Oct').val('0');
    $('#A_Price_effect_Nov').val('0');
    $('#A_Price_effect_Dec').val('0');
    $('#A_Price_effect_Total').val('0');
    bind_A_Price_effect();

    $('#A_Volume_Effect_Jan').val('0');
    $('#A_Volume_Effect_Feb').val('0');
    $('#A_Volume_Effect_Mar').val('0');
    $('#A_Volume_Effect_Apr').val('0');
    $('#A_Volume_Effect_May').val('0');
    $('#A_Volume_Effect_Jun').val('0');
    $('#A_Volume_Effect_Jul').val('0');
    $('#A_Volume_Effect_Aug').val('0');
    $('#A_Volume_Effect_Sep').val('0');
    $('#A_Volume_Effect_Oct').val('0');
    $('#A_Volume_Effect_Nov').val('0');
    $('#A_Volume_Effect_Dec').val('0');
    $('#A_Volume_Effect_Total').val('0');
    bind_A_Volume_Effect();

    $('#Achievement_Jan').val('0');
    $('#Achievement_Feb').val('0');
    $('#Achievement_Mar').val('0');
    $('#Achievement_Apr').val('0');
    $('#Achievement_May').val('0');
    $('#Achievement_Jun').val('0');
    $('#Achievement_Jul').val('0');
    $('#Achievement_Aug').val('0');
    $('#Achievement_Sep').val('0');
    $('#Achievement_Oct').val('0');
    $('#Achievement_Nov').val('0');
    $('#Achievement_Dec').val('0');
    $('#Achievement_Total').val('0');
    bind_Achievement();

    $('#ST_Price_effect_Jan').val('0');
    $('#ST_Price_effect_Feb').val('0');
    $('#ST_Price_effect_Mar').val('0');
    $('#ST_Price_effect_Apr').val('0');
    $('#ST_Price_effect_May').val('0');
    $('#ST_Price_effect_Jun').val('0');
    $('#ST_Price_effect_Jul').val('0');
    $('#ST_Price_effect_Aug').val('0');
    $('#ST_Price_effect_Sep').val('0');
    $('#ST_Price_effect_Oct').val('0');
    $('#ST_Price_effect_Nov').val('0');
    $('#ST_Price_effect_Dec').val('0');
    $('#ST_Price_effect_Total').val('0');
    bind_ST_Price_effect();

    $('#ST_Volume_Effect_Jan').val('0');
    $('#ST_Volume_Effect_Feb').val('0');
    $('#ST_Volume_Effect_Mar').val('0');
    $('#ST_Volume_Effect_Apr').val('0');
    $('#ST_Volume_Effect_May').val('0');
    $('#ST_Volume_Effect_Jun').val('0');
    $('#ST_Volume_Effect_Jul').val('0');
    $('#ST_Volume_Effect_Aug').val('0');
    $('#ST_Volume_Effect_Sep').val('0');
    $('#ST_Volume_Effect_Oct').val('0');
    $('#ST_Volume_Effect_Nov').val('0');
    $('#ST_Volume_Effect_Dec').val('0');
    $('#ST_Volume_Effect_Total').val('0');
    bind_ST_Volume_Effect();

    $('#FY_Secured_Target_Jan').val('0');
    $('#FY_Secured_Target_Feb').val('0');
    $('#FY_Secured_Target_Mar').val('0');
    $('#FY_Secured_Target_Apr').val('0');
    $('#FY_Secured_Target_May').val('0');
    $('#FY_Secured_Target_Jun').val('0');
    $('#FY_Secured_Target_Jul').val('0');
    $('#FY_Secured_Target_Aug').val('0');
    $('#FY_Secured_Target_Sep').val('0');
    $('#FY_Secured_Target_Oct').val('0');
    $('#FY_Secured_Target_Nov').val('0');
    $('#FY_Secured_Target_Dec').val('0');
    $('#FY_Secured_Target_Total').val('0');
    bind_FY_Secured_Target();

    $('#CPI_Effect_Jan').val('0');
    $('#CPI_Effect_Feb').val('0');
    $('#CPI_Effect_Mar').val('0');
    $('#CPI_Effect_Apr').val('0');
    $('#CPI_Effect_May').val('0');
    $('#CPI_Effect_Jun').val('0');
    $('#CPI_Effect_Jul').val('0');
    $('#CPI_Effect_Aug').val('0');
    $('#CPI_Effect_Sep').val('0');
    $('#CPI_Effect_Oct').val('0');
    $('#CPI_Effect_Nov').val('0');
    $('#CPI_Effect_Dec').val('0');
    $('#CPI_Effect_Total').val('0');

    $('#CPI_Jan').val('0');
    $('#CPI_Feb').val('0');
    $('#CPI_Mar').val('0');
    $('#CPI_Apr').val('0');
    $('#CPI_May').val('0');
    $('#CPI_Jun').val('0');
    $('#CPI_Jul').val('0');
    $('#CPI_Aug').val('0');
    $('#CPI_Sep').val('0');
    $('#CPI_Oct').val('0');
    $('#CPI_Nov').val('0');
    $('#CPI_Dec').val('0');

    bind_CPI_Fields();


}

function clear_Procurement_fields() {
    CboUnitOfVolume.SetValue('NONE');
    txt_Input_Actuals_Volumes_Nmin1.SetValue(0);
    txt_Input_Target_Volumes.SetValue(0);
    txt_Total_Actual_volume_N.SetValue(0);
    txt_Spend_Nmin1.SetValue(0);
    txt_Spend_N.SetValue(0);
    txt_CPI.SetValue(0);
    txt_janActual_volume_N.SetValue(0);
    txt_febActual_volume_N.SetValue(0);
    txt_marActual_volume_N.SetValue(0);
    txt_aprActual_volume_N.SetValue(0);
    txt_mayActual_volume_N.SetValue(0);
    txt_junActual_volume_N.SetValue(0);
    txt_julActual_volume_N.SetValue(0);
    txt_augActual_volume_N.SetValue(0);
    txt_sepActual_volume_N.SetValue(0);
    txt_octActual_volume_N.SetValue(0);
    txt_novActual_volume_N.SetValue(0);
    txt_decActual_volume_N.SetValue(0);
    txt_N_FY_Sec_PRICE_EF.SetValue(0);
    txt_N_FY_Sec_VOLUME_EF.SetValue(0);
    txt_N_YTD_Sec_PRICE_EF.SetValue(0);
    txt_N_YTD_Sec_VOLUME_EF.SetValue(0);
    txt_YTD_Achieved_PRICE_EF.SetValue(0);
    txt_YTD_Achieved_VOLUME_EF.SetValue(0);
    txt_YTD_Cost_Avoid_Vs_CPI.SetValue(0);
    txt_FY_Cost_Avoid_Vs_CPI.SetValue(0);
    txt_N_FY_Secured.SetValue(0);
    txt_N_YTD_Secured.SetValue(0);

    txt_YTD_achieved.SetValue(0);
    //$('#txt_YTD_achieved').val('');

    $('#Actual_volume_Nmin1_Jan').val('0');
    $('#Actual_volume_Nmin1_Feb').val('0');
    $('#Actual_volume_Nmin1_Mar').val('0');
    $('#Actual_volume_Nmin1_Apr').val('0');
    $('#Actual_volume_Nmin1_May').val('0');
    $('#Actual_volume_Nmin1_Jun').val('0');
    $('#Actual_volume_Nmin1_Jul').val('0');
    $('#Actual_volume_Nmin1_Aug').val('0');
    $('#Actual_volume_Nmin1_Sep').val('0');
    $('#Actual_volume_Nmin1_Oct').val('0');
    $('#Actual_volume_Nmin1_Nov').val('0');
    $('#Actual_volume_Nmin1_Dec').val('0');
    bind_Actual_volume_Nmin1();

    $('#Target_Volumes_Jan').val('0');
    $('#Target_Volumes_Feb').val('0');
    $('#Target_Volumes_Mar').val('0');
    $('#Target_Volumes_Apr').val('0');
    $('#Target_Volumes_May').val('0');
    $('#Target_Volumes_Jun').val('0');
    $('#Target_Volumes_Jul').val('0');
    $('#Target_Volumes_Aug').val('0');
    $('#Target_Volumes_Sep').val('0');
    $('#Target_Volumes_Oct').val('0');
    $('#Target_Volumes_Nov').val('0');
    $('#Target_Volumes_Dec').val('0');
    bind_Target_Volumes();


}

function isAll_Procurement_Mandatory_fields_entered(_isProcurement) {
    var isFlag = false;

    if (_isProcurement == 1) {
        //ENH153-2 procurment properties get field value in variable
        var xUnit_of_volumes = CboUnitOfVolume.GetValue();
        var xInput_Actuals_Volumes_Nmin1 = txt_Input_Actuals_Volumes_Nmin1.GetValue();
        var xInput_Target_Volumes = txt_Input_Target_Volumes.GetValue();
        var xTotal_Actual_volume_N = txt_Total_Actual_volume_N.GetValue();
        var xSpend_Nmin1 = txt_Spend_Nmin1.GetValue();
        var xSpend_N = txt_Spend_N.GetValue();
        var xCPI = txt_CPI.GetValue();
        var xjanActual_volume_N = txt_janActual_volume_N.GetValue();
        var xfebActual_volume_N = txt_febActual_volume_N.GetValue();
        var xmarActual_volume_N = txt_marActual_volume_N.GetValue();
        var xaprActual_volume_N = txt_aprActual_volume_N.GetValue();
        var xmayActual_volume_N = txt_mayActual_volume_N.GetValue();
        var xjunActual_volume_N = txt_junActual_volume_N.GetValue();
        var xjulActual_volume_N = txt_julActual_volume_N.GetValue();
        var xaugActual_volume_N = txt_augActual_volume_N.GetValue();
        var xsepActual_volume_N = txt_sepActual_volume_N.GetValue();
        var xoctActual_volume_N = txt_octActual_volume_N.GetValue();
        var xnovActual_volume_N = txt_novActual_volume_N.GetValue();
        var xdecActual_volume_N = txt_decActual_volume_N.GetValue();
        var xN_FY_Sec_PRICE_EF = txt_N_FY_Sec_PRICE_EF.GetValue();
        var xN_FY_Sec_VOLUME_EF = txt_N_FY_Sec_VOLUME_EF.GetValue();
        var xN_YTD_Sec_PRICE_EF = txt_N_YTD_Sec_PRICE_EF.GetValue();
        var xN_YTD_Sec_VOLUME_EF = txt_N_YTD_Sec_VOLUME_EF.GetValue();
        var xYTD_Achieved_PRICE_EF = txt_YTD_Achieved_PRICE_EF.GetValue();
        var xYTD_Achieved_VOLUME_EF = txt_YTD_Achieved_VOLUME_EF.GetValue();
        var xYTD_Cost_Avoid_Vs_CPI = txt_YTD_Cost_Avoid_Vs_CPI.GetValue();
        var xFY_Cost_Avoid_Vs_CPI = txt_FY_Cost_Avoid_Vs_CPI.GetValue();
        //ENH153-2 procurment properties get field value in variable

        if ((xUnit_of_volumes != null && xUnit_of_volumes != '') && (xInput_Actuals_Volumes_Nmin1 != null && xInput_Actuals_Volumes_Nmin1 != '') &&
            (xInput_Target_Volumes != null && xInput_Target_Volumes != '') && (xTotal_Actual_volume_N != null && xTotal_Actual_volume_N != '') &&
            (xSpend_Nmin1 != null && xSpend_Nmin1 != '') && (xSpend_N != null && xSpend_N != '') &&
            /*(xCPI != null && xCPI != '') &&*/ (xjanActual_volume_N != null && xjanActual_volume_N != '') &&
            (xfebActual_volume_N != null && xfebActual_volume_N != '') && (xmarActual_volume_N != null && xmarActual_volume_N != '') &&
            (xaprActual_volume_N != null && xaprActual_volume_N != '') && (xmayActual_volume_N != null && xmayActual_volume_N != '') &&
            (xjunActual_volume_N != null && xjunActual_volume_N != '') && (xjulActual_volume_N != null && xjulActual_volume_N != '') &&
            (xaugActual_volume_N != null && xaugActual_volume_N != '') && (xsepActual_volume_N != null && xsepActual_volume_N != '') &&
            (xoctActual_volume_N != null && xoctActual_volume_N != '') && (xnovActual_volume_N != null && xnovActual_volume_N != '') &&
            (xdecActual_volume_N != null && xdecActual_volume_N != '') && (xN_FY_Sec_PRICE_EF != null && xN_FY_Sec_PRICE_EF != '') &&
            (xN_FY_Sec_VOLUME_EF != null && xN_FY_Sec_VOLUME_EF != '') && (xN_YTD_Sec_PRICE_EF != null && xN_YTD_Sec_PRICE_EF != '') &&
            (xN_YTD_Sec_VOLUME_EF != null && xN_YTD_Sec_VOLUME_EF != '') && (xYTD_Achieved_PRICE_EF != null && xYTD_Achieved_PRICE_EF != '') &&
            (xYTD_Achieved_VOLUME_EF != null && xYTD_Achieved_VOLUME_EF != '') && (xYTD_Cost_Avoid_Vs_CPI != null && xYTD_Cost_Avoid_Vs_CPI != '') &&
            (xFY_Cost_Avoid_Vs_CPI != null && xFY_Cost_Avoid_Vs_CPI != '')) {
            isFlag = true;
        }
        else {
            isFlag = false;
        }

    }
    else {
        isFlag = true;
    }
    return isFlag;
}

function OnInitiativeTypeChanged(s, e) {
    var id = s.GetValue();
    $.post(URLContent('ActiveInitiative/GetItemFromInitiativeType'), { id: id }, function (data) {
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
    $.post(URLContent('ActiveInitiative/GetItemFromCostCategory'), { id: id, id2: id2, id3: id3 }, function (data) {
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
    // debugger;
    var id = s.GetValue(); /*var teks = s.GetText();*/
    $.post(URLContent('ActiveInitiative/GetCountryBySub'), { id: id }, function (data) {
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
        $.each(data[0]["LegalEntityData"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj != null) LegalEntityID.AddItem(obj.LegalEntityName, obj.id);
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
    $.post(URLContent('ActiveInitiative/GetLegalFromBrand'), { brandid: id, countryid: countryid }, function (data) {
        var obj; LegalEntityID.ClearItems();
        $.each(data[0]["LegalEntityData"],
            function (key, value) {
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
    //GrdLegalEntity.SetText = value;
    //GrdLegalEntity.SetValue(value);
}

function OnGetRowValues(value) {
    GrdSubCountryPopup.SetText = value;
}

function ShowEditWindow(id) {

    clear_Procurement_BackCalcs();
    clear_Procurement_fields();

    var min_py = 0; var max_py = 0;
    var py = projectYear;
    min_py = (+py - 1);
    max_py = (+py);
    StartMonth.SetMinDate(new Date(min_py + '-01-01'));
    StartMonth.SetMaxDate(new Date((max_py) + '-12-31'));
    EndMonth.SetMinDate(new Date(max_py + '-01-01'));
    EndMonth.SetMaxDate(new Date((max_py + 1) + '-12-31'));
    if (projectYear > 2022) { StartMonth.SetMinDate(new Date((max_py) + '-12-31')); }

    var GrdInit_Type = 0, GrdAction_Type = 0, GrdSyn_Impact = 0, GrdInit_Status = 0, TxPort_Name = 0;
    var GrdInit_Category = 0, CboWebinar_Cat = 0, Grd_SubCost = 0;


    $("#FormStatus").val("Edit");
    // debugger;




    CboHoValidity.AddItem("Y"); CboHoValidity.AddItem("N");
    CboRPOCValidity.AddItem("KO", "KO"); CboRPOCValidity.AddItem("Under Review", "UR"); CboRPOCValidity.AddItem("OK Level 1 - L1 if FY Target > 200 kUSD (L1= Cost controller)", "L1");
    CboRPOCValidity.AddItem("OK Level 2 - L2 if FY Target > 300 kUSD (L2 =Management RO)", "L2"); CboRPOCValidity.AddItem("OK Level 3 - L3 if FY Target > 500 kUSD (L3 = Coordinateur HO)", "L3");
    // debugger;
    $.post(URLContent('ActiveInitiative/GetInfoById'), { Id: id }, function (data) {
        if (data != '') {
            var obj = JSON.parse(data); var SubCountryID = obj.SubCountryID;

            var isProcurement = obj.isProcurement;
            if (isProcurement == null) { isProcurement = 0; }
            $('#isProcurement').val(isProcurement)

            GrdInit_Type = obj.InitiativeType;
            GrdAction_Type = obj.ActionTypeID;
            GrdSyn_Impact = obj.SynergyImpactID;
            GrdInit_Status = obj.InitStatus;
            TxPort_Name = obj.PortID;
            GrdInit_Category = obj.CostCategoryID;
            CboWebinar_Cat = obj.SourceCategory;
            Grd_SubCost = obj.SubCostCategoryID;

            var brandId = obj.BrandID;
            var legalentityidx = obj.LegalEntityID;



            $.post(URLContent('ActiveInitiative/GetInfoForPopUp'), { Id: id }, function (DDdata) {
                var obj1; GrdInitType.ClearItems(); GrdActionType.ClearItems(); GrdSynImpact.ClearItems(); GrdInitStatus.ClearItems(); TxPortName.ClearItems(); GrdInitCategory.ClearItems(); GrdSubCost.ClearItems();
                var uType = user_type;
                CboWebinarCat.ClearItems();
                if (isProcurement == 1) {
                    //$.each(DDdata[0]["SavingTypeData"], function (key, dd_value) {
                    //    dd_value = JSON.stringify(dd_value); obj1 = JSON.parse(dd_value);
                    //    if (obj1 != null) GrdInitType.AddItem(obj1.SavingTypeName, obj1.id);
                    //});
                    $.each(DDdata[0]["SavingTypeData"], function (key, dd_value) {
                        dd_value = JSON.stringify(dd_value); obj1 = JSON.parse(dd_value);
                        if (obj1 != null)
                        {
                            if (obj1.SavingTypeName == "Positive Cost Impact" || obj1.SavingTypeName == "Negative Cost Impact") {
                                GrdInitType.AddItem(obj1.SavingTypeName, obj1.id);
                            }
                            
                        }
                    });
                }
                else {
                    $.each(DDdata[0]["SavingTypeData"], function (key, dd_value) {
                        dd_value = JSON.stringify(dd_value); obj1 = JSON.parse(dd_value);
                        if (obj1 != null) GrdInitType.AddItem(obj1.SavingTypeName, obj1.id);
                    });
                }
                $.each(DDdata[0]["ActionTypeData"], function (key, dd_value) {
                    dd_value = JSON.stringify(dd_value); obj1 = JSON.parse(dd_value);
                    if (obj1 != null) GrdActionType.AddItem(obj1.ActionTypeName, obj1.id);
                    console.log(obj1.id);
                });
                $.each(DDdata[0]["SynImpactData"], function (key, dd_value) {
                    dd_value = JSON.stringify(dd_value); obj1 = JSON.parse(dd_value);
                    if (obj1 != null) GrdSynImpact.AddItem(obj1.SynImpactName, obj1.id);
                });
                $.each(DDdata[0]["InitStatusData"], function (key, dd_value) {
                    dd_value = JSON.stringify(dd_value); obj1 = JSON.parse(dd_value);
                    if (obj1 != null) GrdInitStatus.AddItem(obj1.Status, obj1.id);
                });
                $.each(DDdata[0]["PortNameData"], function (key, dd_value) {
                    dd_value = JSON.stringify(dd_value); obj1 = JSON.parse(dd_value);
                    if (obj1 != null) TxPortName.AddItem(obj1.PortName, obj1.id);
                });
                $.each(DDdata[0]["MCostTypeData"], function (key, dd_value) {
                    dd_value = JSON.stringify(dd_value); obj1 = JSON.parse(dd_value);
                    if (obj1 != null) GrdInitCategory.AddItem(obj1.CostTypeName, obj1.id);
                });
                $.each(DDdata[0]["MSubCostData"], function (key, dd_value) {
                    dd_value = JSON.stringify(dd_value); obj1 = JSON.parse(dd_value);
                    if (obj1 != null) GrdSubCost.AddItem(obj1.SubCostName, obj1.id);
                });
                $.each(DDdata[0]["MSourceCategory"], function (key, dd_value) {
                    dd_value = JSON.stringify(dd_value); obj1 = JSON.parse(dd_value);
                    if (obj1 != null) CboWebinarCat.AddItem(obj1.categoryname, obj1.id);
                });


                $.post(URLContent('ActiveInitiative/GetCountryBySub'), { Id: SubCountryID, Id2: id }, function (D_data) {
                    var obj2;
                    ////// debugger;;
                    GrdSubCountryPopup.ClearItems(); GrdBrandPopup.ClearItems(); GrdLegalEntityPopup.ClearItems();
                    $.each(D_data[0]["SubCountryData"], function (key, d_value) {
                        d_value = JSON.stringify(d_value); obj2 = JSON.parse(d_value);
                        if (obj2 != null) GrdSubCountryPopup.AddItem(obj2.SubCountryName, obj2.id);
                    });
                    $.each(D_data[0]["BrandData"], function (key, d_value) {
                        d_value = JSON.stringify(d_value); obj2 = JSON.parse(d_value);
                        if (obj2 != null) GrdBrandPopup.AddItem(obj2.BrandName, obj2.id);
                    });
                    ////// debugger;;
                    $.each(D_data[0]["LegalEntityData"], function (key, d_value) {
                        d_value = JSON.stringify(d_value); obj2 = JSON.parse(d_value);
                        if (obj2 != null) GrdLegalEntityPopup.AddItem(obj2.LegalEntityName, obj2.id);
                    });

                    GrdSubCountryPopup.SelectIndex(SubCountryID); GrdBrandPopup.SelectIndex(brandId); GrdLegalEntityPopup.SelectIndex(legalentityidx);
                    GrdSubCountryPopup.SetValue(obj.SubCountryID);
                    GrdBrandPopup.SetValue(obj.BrandID);
                    GrdLegalEntityPopup.SetValue(obj.LegalEntityID);
                });


                if (GrdInit_Type != null) {
                    if (isProcurement == 1) {
                        GrdInitType.clientEnabled = false;
                    }
                    GrdInitType.SelectIndex(GrdInit_Type);
                }
                else { GrdInitType.SelectIndex(0); }

                if (GrdAction_Type != null)
                    GrdActionType.SelectIndex(GrdAction_Type);
                else
                    GrdActionType.SelectIndex(0);

                if (GrdSyn_Impact != null)
                    GrdSynImpact.SelectIndex(GrdSyn_Impact);
                else
                    GrdSynImpact.SelectIndex(0);

                if (GrdInit_Status != null)
                    GrdInitStatus.SelectIndex(GrdInit_Status);
                else
                    GrdInitStatus.SelectIndex(0);

                if (TxPort_Name != null)
                    TxPortName.SelectIndex(TxPort_Name);
                else
                    TxPortName.SelectIndex(0);

                if (GrdInit_Category != null)
                    GrdInitCategory.SelectIndex(GrdInit_Category);
                else
                    GrdInitCategory.SelectIndex(0);

                if (CboWebinar_Cat != null)
                    CboWebinarCat.SelectIndex(CboWebinar_Cat);
                else
                    CboWebinarCat.SelectIndex(0);

                if (Grd_SubCost != null)
                    GrdSubCost.SelectIndex(Grd_SubCost);
                else
                    GrdSubCost.SelectIndex(0);

                // GrdSubCost.SelectIndex(0);



                TxPortName.SetValue(obj.PortID);
                GrdInitType.SetValue(obj.InitiativeType);
                GrdActionType.SetValue(obj.ActionTypeID);
                GrdSynImpact.SetValue(obj.SynergyImpactID);
                GrdInitStatus.SetValue(obj.InitStatus);
                GrdInitCategory.SetValue(obj.CostCategoryID);
                CboWebinarCat.SetValue(obj.SourceCategory);
                GrdSubCost.SetValue(obj.SubCostCategoryID);




                // GrdInitType.SelectIndex(0); GrdActionType.SelectIndex(0); GrdSynImpact.SelectIndex(0); GrdInitStatus.SelectIndex(0);
                //TxPortName.SelectIndex(0); GrdInitCategory.SelectIndex(0); GrdSubCost.SelectIndex(0); CboWebinarCat.SelectIndex(0);
            });


            var uType = user_type; /*var projectYear = projectYear;*/
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
            $("#GrdBrandPopup").val(obj.brandname);
            $("#GrdLegalEntityPopup").val(obj.LegalEntityName);
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
            GrdBrandPopup.SetValue(obj.BrandID);
            GrdLegalEntityPopup.SetValue(obj.LegalEntityID);


            //ENH153-2 Procurement fields 
            if (isProcurement == 1) {

                CboUnitOfVolume.SetText(obj.Unit_of_volumes);
                txt_Input_Actuals_Volumes_Nmin1.SetValue(obj.Input_Actuals_Volumes_Nmin1);
                txt_Input_Target_Volumes.SetValue(obj.Input_Target_Volumes);
                txt_Total_Actual_volume_N.SetValue(obj.Total_Actual_volume_N);
                txt_Spend_Nmin1.SetValue(obj.Spend_Nmin1);
                txt_Spend_N.SetValue(obj.Spend_N);
                txt_CPI.SetValue(obj.CPI);
                txt_janActual_volume_N.SetValue(obj.janActual_volume_N);
                txt_febActual_volume_N.SetValue(obj.febActual_volume_N);
                txt_marActual_volume_N.SetValue(obj.marActual_volume_N);
                txt_aprActual_volume_N.SetValue(obj.aprActual_volume_N);
                txt_mayActual_volume_N.SetValue(obj.mayActual_volume_N);
                txt_junActual_volume_N.SetValue(obj.junActual_volume_N);
                txt_julActual_volume_N.SetValue(obj.julActual_volume_N);
                txt_augActual_volume_N.SetValue(obj.augActual_volume_N);
                txt_sepActual_volume_N.SetValue(obj.sepActual_volume_N);
                txt_octActual_volume_N.SetValue(obj.octActual_volume_N);
                txt_novActual_volume_N.SetValue(obj.novActual_volume_N);
                txt_decActual_volume_N.SetValue(obj.decActual_volume_N);

                txt_N_FY_Sec_PRICE_EF.SetValue(obj.N_FY_Sec_PRICE_EF);
                txt_N_FY_Sec_VOLUME_EF.SetValue(obj.N_FY_Sec_VOLUME_EF);
                txt_N_FY_Secured.SetValue(parseFloat(obj.N_FY_Sec_PRICE_EF) + parseFloat(obj.N_FY_Sec_VOLUME_EF));

                txt_N_YTD_Sec_PRICE_EF.SetValue(obj.N_YTD_Sec_PRICE_EF);
                txt_N_YTD_Sec_VOLUME_EF.SetValue(obj.N_YTD_Sec_VOLUME_EF);
                txt_N_YTD_Secured.SetValue(parseFloat(obj.N_YTD_Sec_PRICE_EF) + parseFloat(obj.N_YTD_Sec_VOLUME_EF));

                txt_YTD_Achieved_PRICE_EF.SetValue(obj.YTD_Achieved_PRICE_EF);
                txt_YTD_Achieved_VOLUME_EF.SetValue(obj.YTD_Achieved_VOLUME_EF);

                //$('#txt_YTD_achieved').val(parseFloat(obj.YTD_Achieved_PRICE_EF) + parseFloat(obj.YTD_Achieved_VOLUME_EF));
                txt_YTD_achieved.SetValue(parseFloat(obj.YTD_Achieved_PRICE_EF) + parseFloat(obj.YTD_Achieved_VOLUME_EF));

                txt_YTD_Cost_Avoid_Vs_CPI.SetValue(obj.YTD_Cost_Avoid_Vs_CPI);
                txt_FY_Cost_Avoid_Vs_CPI.SetValue(obj.FY_Cost_Avoid_Vs_CPI);
            }

            //ENH153-2 Procurement fields 
            // debugger;
            OnStartMonthChanged();
            let startyear = new Date(obj.StartMonth).getFullYear()
            let nextyear = startyear + 1;

            //Change the labels 
            if (isProcurement != 1) {
                $('#ljan').html('Jan-' + startyear);
                $('#lfeb').html('Feb-' + startyear);
                $('#lmar').html('Mar-' + startyear);
                $('#lapr').html('Apr-' + startyear);
                $('#lmay').html('May-' + startyear);
                $('#ljun').html('Jun-' + startyear);
                $('#ljul').html('Jul-' + startyear);
                $('#laug').html('Aug-' + startyear);
                $('#lsep').html('Sep-' + startyear);
                $('#loct').html('Oct-' + startyear);
                $('#lnov').html('Nov-' + startyear);
                $('#ldec').html('Dec-' + startyear);

                $('#lnexjan').html('Jan-' + nextyear);
                $('#lnexfeb').html('Feb-' + nextyear);
                $('#lnexmar').html('Mar-' + nextyear);
                $('#lnexapr').html('Apr-' + nextyear);
                $('#lnexmay').html('May-' + nextyear);
                $('#lnexjun').html('Jun-' + nextyear);
                $('#lnexjul').html('Jul-' + nextyear);
                $('#lnexaug').html('Aug-' + nextyear);
                $('#lnexsep').html('Sep-' + nextyear);
                $('#lnexoct').html('Oct-' + nextyear);
                $('#lnexnov').html('Nov-' + nextyear);
                $('#lnexdec').html('Dec-' + nextyear);

                //End change the labels 
                //commented the below if else to show the full initiative figures  
                /* if ((new Date(obj.StartMonth).getFullYear()) < projectYear) {
                     $(".targetjan").val(formatValue(obj.TargetNexJan)); $(".targetfeb").val(formatValue(obj.TargetNexFeb)); $(".targetmar").val(formatValue(obj.TargetNexMar)); $(".targetapr").val(formatValue(obj.TargetNexApr)); $(".targetmay").val(formatValue(obj.TargetNexMay)); $(".targetjun").val(formatValue(obj.TargetNexJun));
                     $(".targetjul").val(formatValue(obj.TargetNexJul)); $(".targetaug").val(formatValue(obj.TargetNexAug)); $(".targetsep").val(formatValue(obj.TargetNexSep)); $(".targetoct").val(formatValue(obj.TargetNexOct)); $(".targetnov").val(formatValue(obj.TargetNexNov)); $(".targetdec").val(formatValue(obj.TargetNexDec));
                     $(".savingjan").val(formatValue(obj.AchNexJan)); $(".savingfeb").val(formatValue(obj.AchNexFeb)); $(".savingmar").val(formatValue(obj.AchNexMar)); $(".savingapr").val(formatValue(obj.AchNexApr)); $(".savingmay").val(formatValue(obj.AchNexMay)); $(".savingjun").val(formatValue(obj.AchNexJun));
                     $(".savingjul").val(formatValue(obj.AchNexJul)); $(".savingaug").val(formatValue(obj.AchNexAug)); $(".savingsep").val(formatValue(obj.AchNexSep)); $(".savingoct").val(formatValue(obj.AchNexOct)); $(".savingnov").val(formatValue(obj.AchNexNov)); $(".savingdec").val(formatValue(obj.AchNexDec));
                 } else {*/
                $(".targetjan").val(formatValue(obj.TargetJan)); $(".targetfeb").val(formatValue(obj.TargetFeb)); $(".targetmar").val(formatValue(obj.TargetMar)); $(".targetapr").val(formatValue(obj.TargetApr)); $(".targetmay").val(formatValue(obj.TargetMay)); $(".targetjun").val(formatValue(obj.TargetJun));
                $(".targetjul").val(formatValue(obj.TargetJul)); $(".targetaug").val(formatValue(obj.TargetAug)); $(".targetsep").val(formatValue(obj.TargetSep)); $(".targetoct").val(formatValue(obj.TargetOct)); $(".targetnov").val(formatValue(obj.TargetNov)); $(".targetdec").val(formatValue(obj.TargetDec));
                $(".savingjan").val(formatValue(obj.AchJan)); $(".savingfeb").val(formatValue(obj.AchFeb)); $(".savingmar").val(formatValue(obj.AchMar)); $(".savingapr").val(formatValue(obj.AchApr)); $(".savingmay").val(formatValue(obj.AchMay)); $(".savingjun").val(formatValue(obj.AchJun));
                $(".savingjul").val(formatValue(obj.AchJul)); $(".savingaug").val(formatValue(obj.AchAug)); $(".savingsep").val(formatValue(obj.AchSep)); $(".savingoct").val(formatValue(obj.AchOct)); $(".savingnov").val(formatValue(obj.AchNov)); $(".savingdec").val(formatValue(obj.AchDec));

                $(".targetjan2").val(formatValue(obj.TargetNexJan)); $(".targetfeb2").val(formatValue(obj.TargetNexFeb)); $(".targetmar2").val(formatValue(obj.TargetNexMar)); $(".targetapr2").val(formatValue(obj.TargetNexApr)); $(".targetmay2").val(formatValue(obj.TargetNexMay)); $(".targetjun2").val(formatValue(obj.TargetNexJun));
                $(".targetjul2").val(formatValue(obj.TargetNexJul)); $(".targetaug2").val(formatValue(obj.TargetNexAug)); $(".targetsep2").val(formatValue(obj.TargetNexSep)); $(".targetoct2").val(formatValue(obj.TargetNexOct)); $(".targetnov2").val(formatValue(obj.TargetNexNov)); $(".targetdec2").val(formatValue(obj.TargetNexDec));
                $(".savingjan2").val(formatValue(obj.AchNexJan)); $(".savingfeb2").val(formatValue(obj.AchNexFeb)); $(".savingmar2").val(formatValue(obj.AchNexMar)); $(".savingapr2").val(formatValue(obj.AchNexApr)); $(".savingmay2").val(formatValue(obj.AchNexMay)); $(".savingjun2").val(formatValue(obj.AchNexJun));
                $(".savingjul2").val(formatValue(obj.AchNexJul)); $(".savingaug2").val(formatValue(obj.AchNexAug)); $(".savingsep2").val(formatValue(obj.AchNexSep)); $(".savingoct2").val(formatValue(obj.AchNexOct)); $(".savingnov2").val(formatValue(obj.AchNexNov)); $(".savingdec2").val(formatValue(obj.AchNexDec));
                // }

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
            var brandId = obj.BrandID;

            getYtdValue();
            hitungtahunini();
            //var years_right = '@years_right';
            var project_year = projectYear;

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
            if (uType == 3 && formstatus == "Edit" && initstatusvalue != "4") {
                $("#chkAuto").prop("disabled", true);
                $(".txTarget").prop("disabled", true); //prevent Agency User to edit the target except pending initiative
                StartMonth.clientEnabled = false; EndMonth.clientEnabled = false; //prevent Agency User from selecting different Start / End dates (Except for Pending initiative)
                txTarget12.clientEnabled = false;
            }
            if (uType == 3 && formstatus == "Edit" && (initstatusvalue == "4" || initstatusvalue == "15")) {


                var targetjan = $(".targetjan").val().replaceAll(",", ""); var targetfeb = $(".targetfeb").val().replaceAll(",", ""); var targetmar = $(".targetmar").val().replaceAll(",", ""); var targetapr = $(".targetapr").val().replaceAll(",", ""); var targetmay = $(".targetmay").val().replaceAll(",", "");
                var targetjun = $(".targetjun").val().replaceAll(",", ""); var targetjul = $(".targetjul").val().replaceAll(",", ""); var targetaug = $(".targetaug").val().replaceAll(",", ""); var targetsep = $(".targetsep").val().replaceAll(",", ""); var targetoct = $(".targetoct").val().replaceAll(",", "");
                var targetnov = $(".targetnov").val().replaceAll(",", ""); var targetdec = $(".targetdec").val().replaceAll(",", "");


                if (targetjan.length > 1)
                    $(".targetjan").prop("disabled", false);
                if (targetfeb.length > 1)
                    $(".targetfeb").prop("disabled", false);
                if (targetmar.length > 1)
                    $(".targetmar").prop("disabled", false);
                if (targetapr.length > 1)
                    $(".targetapr").prop("disabled", false);
                if (targetmay.length > 1)
                    $(".targetmay").prop("disabled", false);
                if (targetjun.length > 1)
                    $(".targetjun").prop("disabled", false);
                if (targetjul.length > 1)
                    $(".targetjul").prop("disabled", false);
                if (targetaug.length > 1)
                    $(".targetaug").prop("disabled", false);
                if (targetsep.length > 1)
                    $(".targetsep").prop("disabled", false);
                if (targetoct.length > 1)
                    $(".targetoct").prop("disabled", false);
                if (targetnov.length > 1)
                    $(".targetnov").prop("disabled", false);
                if (targetdec.length > 1)
                    $(".targetdec").prop("disabled", false);
            }

            //ENH153-2 back end calculation
            if (isProcurement == 1) {

                $.post(URLContent('ActiveInitiative/get_Monthly_CPI'), { id: obj.CountryID }, function (CPI_data) {
                    if (CPI_data != null) {
                        setCPI_on_Country_Selection(CPI_data);
                    }

                    $.post(URLContent('ActiveInitiative/getProcuementCalcs'), { id: id }, function (procurement_data) {
                        if (procurement_data != null) {

                            $('#Actual_CPU_Nmin1_Jan').val(procurement_data.jan_Actual_CPU_Nmin1);
                            $('#Actual_CPU_Nmin1_Feb').val(procurement_data.feb_Actual_CPU_Nmin1);
                            $('#Actual_CPU_Nmin1_Mar').val(procurement_data.march_Actual_CPU_Nmin1);
                            $('#Actual_CPU_Nmin1_Apr').val(procurement_data.apr_Actual_CPU_Nmin1);
                            $('#Actual_CPU_Nmin1_May').val(procurement_data.may_Actual_CPU_Nmin1);
                            $('#Actual_CPU_Nmin1_Jun').val(procurement_data.jun_Actual_CPU_Nmin1);
                            $('#Actual_CPU_Nmin1_Jul').val(procurement_data.jul_Actual_CPU_Nmin1);
                            $('#Actual_CPU_Nmin1_Aug').val(procurement_data.aug_Actual_CPU_Nmin1);
                            $('#Actual_CPU_Nmin1_Sep').val(procurement_data.sep_Actual_CPU_Nmin1);
                            $('#Actual_CPU_Nmin1_Oct').val(procurement_data.oct_Actual_CPU_Nmin1);
                            $('#Actual_CPU_Nmin1_Nov').val(procurement_data.nov_Actual_CPU_Nmin1);
                            $('#Actual_CPU_Nmin1_Dec').val(procurement_data.dec_Actual_CPU_Nmin1);
                            $('#Target_CPU_N_Jan').val(procurement_data.jan_Target_CPU_N);
                            $('#Target_CPU_N_Feb').val(procurement_data.feb_Target_CPU_N);
                            $('#Target_CPU_N_Mar').val(procurement_data.march_Target_CPU_N);
                            $('#Target_CPU_N_Apr').val(procurement_data.apr_Target_CPU_N);
                            $('#Target_CPU_N_May').val(procurement_data.may_Target_CPU_N);
                            $('#Target_CPU_N_Jun').val(procurement_data.jun_Target_CPU_N);
                            $('#Target_CPU_N_Jul').val(procurement_data.jul_Target_CPU_N);
                            $('#Target_CPU_N_Aug').val(procurement_data.aug_Target_CPU_N);
                            $('#Target_CPU_N_Sep').val(procurement_data.sep_Target_CPU_N);
                            $('#Target_CPU_N_Oct').val(procurement_data.oct_Target_CPU_N);
                            $('#Target_CPU_N_Nov').val(procurement_data.nov_Target_CPU_N);
                            $('#Target_CPU_N_Dec').val(procurement_data.dec_Target_CPU_N);
                            $('#A_Price_effect_Jan').val(procurement_data.jan_A_Price_effect);
                            $('#A_Price_effect_Feb').val(procurement_data.feb_A_Price_effect);
                            $('#A_Price_effect_Mar').val(procurement_data.march_A_Price_effect);
                            $('#A_Price_effect_Apr').val(procurement_data.apr_A_Price_effect);
                            $('#A_Price_effect_May').val(procurement_data.may_A_Price_effect);
                            $('#A_Price_effect_Jun').val(procurement_data.jun_A_Price_effect);
                            $('#A_Price_effect_Jul').val(procurement_data.jul_A_Price_effect);
                            $('#A_Price_effect_Aug').val(procurement_data.aug_A_Price_effect);
                            $('#A_Price_effect_Sep').val(procurement_data.sep_A_Price_effect);
                            $('#A_Price_effect_Oct').val(procurement_data.oct_A_Price_effect);
                            $('#A_Price_effect_Nov').val(procurement_data.nov_A_Price_effect);
                            $('#A_Price_effect_Dec').val(procurement_data.dec_A_Price_effect);
                            $('#A_Volume_Effect_Jan').val(procurement_data.jan_A_Volume_Effect);
                            $('#A_Volume_Effect_Feb').val(procurement_data.feb_A_Volume_Effect);
                            $('#A_Volume_Effect_Mar').val(procurement_data.march_A_Volume_Effect);
                            $('#A_Volume_Effect_Apr').val(procurement_data.apr_A_Volume_Effect);
                            $('#A_Volume_Effect_May').val(procurement_data.may_A_Volume_Effect);
                            $('#A_Volume_Effect_Jun').val(procurement_data.jun_A_Volume_Effect);
                            $('#A_Volume_Effect_Jul').val(procurement_data.jul_A_Volume_Effect);
                            $('#A_Volume_Effect_Aug').val(procurement_data.aug_A_Volume_Effect);
                            $('#A_Volume_Effect_Sep').val(procurement_data.sep_A_Volume_Effect);
                            $('#A_Volume_Effect_Oct').val(procurement_data.oct_A_Volume_Effect);
                            $('#A_Volume_Effect_Nov').val(procurement_data.nov_A_Volume_Effect);
                            $('#A_Volume_Effect_Dec').val(procurement_data.dec_A_Volume_Effect);
                            $('#Achievement_Jan').val(procurement_data.jan_Achievement);
                            $('#Achievement_Feb').val(procurement_data.feb_Achievement);
                            $('#Achievement_Mar').val(procurement_data.march_Achievement);
                            $('#Achievement_Apr').val(procurement_data.apr_Achievement);
                            $('#Achievement_May').val(procurement_data.may_Achievement);
                            $('#Achievement_Jun').val(procurement_data.jun_Achievement);
                            $('#Achievement_Jul').val(procurement_data.jul_Achievement);
                            $('#Achievement_Aug').val(procurement_data.aug_Achievement);
                            $('#Achievement_Sep').val(procurement_data.sep_Achievement);
                            $('#Achievement_Oct').val(procurement_data.oct_Achievement);
                            $('#Achievement_Nov').val(procurement_data.nov_Achievement);
                            $('#Achievement_Dec').val(procurement_data.dec_Achievement);
                            $('#ST_Price_effect_Jan').val(procurement_data.jan_ST_Price_effect);
                            $('#ST_Price_effect_Feb').val(procurement_data.feb_ST_Price_effect);
                            $('#ST_Price_effect_Mar').val(procurement_data.march_ST_Price_effect);
                            $('#ST_Price_effect_Apr').val(procurement_data.apr_ST_Price_effect);
                            $('#ST_Price_effect_May').val(procurement_data.may_ST_Price_effect);
                            $('#ST_Price_effect_Jun').val(procurement_data.jun_ST_Price_effect);
                            $('#ST_Price_effect_Jul').val(procurement_data.jul_ST_Price_effect);
                            $('#ST_Price_effect_Aug').val(procurement_data.aug_ST_Price_effect);
                            $('#ST_Price_effect_Sep').val(procurement_data.sep_ST_Price_effect);
                            $('#ST_Price_effect_Oct').val(procurement_data.oct_ST_Price_effect);
                            $('#ST_Price_effect_Nov').val(procurement_data.nov_ST_Price_effect);
                            $('#ST_Price_effect_Dec').val(procurement_data.dec_ST_Price_effect);
                            $('#ST_Volume_Effect_Jan').val(procurement_data.jan_ST_Volume_Effect);
                            $('#ST_Volume_Effect_Feb').val(procurement_data.feb_ST_Volume_Effect);
                            $('#ST_Volume_Effect_Mar').val(procurement_data.march_ST_Volume_Effect);
                            $('#ST_Volume_Effect_Apr').val(procurement_data.apr_ST_Volume_Effect);
                            $('#ST_Volume_Effect_May').val(procurement_data.may_ST_Volume_Effect);
                            $('#ST_Volume_Effect_Jun').val(procurement_data.jun_ST_Volume_Effect);
                            $('#ST_Volume_Effect_Jul').val(procurement_data.jul_ST_Volume_Effect);
                            $('#ST_Volume_Effect_Aug').val(procurement_data.aug_ST_Volume_Effect);
                            $('#ST_Volume_Effect_Sep').val(procurement_data.sep_ST_Volume_Effect);
                            $('#ST_Volume_Effect_Oct').val(procurement_data.oct_ST_Volume_Effect);
                            $('#ST_Volume_Effect_Nov').val(procurement_data.nov_ST_Volume_Effect);
                            $('#ST_Volume_Effect_Dec').val(procurement_data.dec_ST_Volume_Effect);
                            $('#FY_Secured_Target_Jan').val(procurement_data.jan_FY_Secured_Target);
                            $('#FY_Secured_Target_Feb').val(procurement_data.feb_FY_Secured_Target);
                            $('#FY_Secured_Target_Mar').val(procurement_data.march_FY_Secured_Target);
                            $('#FY_Secured_Target_Apr').val(procurement_data.apr_FY_Secured_Target);
                            $('#FY_Secured_Target_May').val(procurement_data.may_FY_Secured_Target);
                            $('#FY_Secured_Target_Jun').val(procurement_data.jun_FY_Secured_Target);
                            $('#FY_Secured_Target_Jul').val(procurement_data.jul_FY_Secured_Target);
                            $('#FY_Secured_Target_Aug').val(procurement_data.aug_FY_Secured_Target);
                            $('#FY_Secured_Target_Sep').val(procurement_data.sep_FY_Secured_Target);
                            $('#FY_Secured_Target_Oct').val(procurement_data.oct_FY_Secured_Target);
                            $('#FY_Secured_Target_Nov').val(procurement_data.nov_FY_Secured_Target);
                            $('#FY_Secured_Target_Dec').val(procurement_data.dec_FY_Secured_Target);
                            $('#CPI_Effect_Jan').val(procurement_data.jan_CPI_Effect);
                            $('#CPI_Effect_Feb').val(procurement_data.feb_CPI_Effect);
                            $('#CPI_Effect_Mar').val(procurement_data.march_CPI_Effect);
                            $('#CPI_Effect_Apr').val(procurement_data.apr_CPI_Effect);
                            $('#CPI_Effect_May').val(procurement_data.may_CPI_Effect);
                            $('#CPI_Effect_Jun').val(procurement_data.jun_CPI_Effect);
                            $('#CPI_Effect_Jul').val(procurement_data.jul_CPI_Effect);
                            $('#CPI_Effect_Aug').val(procurement_data.aug_CPI_Effect);
                            $('#CPI_Effect_Sep').val(procurement_data.sep_CPI_Effect);
                            $('#CPI_Effect_Oct').val(procurement_data.oct_CPI_Effect);
                            $('#CPI_Effect_Nov').val(procurement_data.nov_CPI_Effect);
                            $('#CPI_Effect_Dec').val(procurement_data.dec_CPI_Effect);

                            //$('#CPI_Jan').val(procurement_data.jan_CPI);
                            //$('#CPI_Feb').val(procurement_data.feb_CPI);
                            //$('#CPI_Mar').val(procurement_data.mar_CPI);
                            //$('#CPI_Apr').val(procurement_data.apr_CPI);
                            //$('#CPI_May').val(procurement_data.may_CPI);
                            //$('#CPI_Jun').val(procurement_data.jun_CPI);
                            //$('#CPI_Jul').val(procurement_data.jul_CPI);
                            //$('#CPI_Aug').val(procurement_data.aug_CPI);
                            //$('#CPI_Sep').val(procurement_data.sep_CPI);
                            //$('#CPI_Oct').val(procurement_data.oct_CPI);
                            //$('#CPI_Nov').val(procurement_data.nov_CPI);
                            //$('#CPI_Dec').val(procurement_data.dec_CPI);

                        }

                        if (isProcurement == 1) {

                            calculate_Procurement_Field();

                            $('#_divOptimization').prop('style', 'display:none');
                            $('#_divProcurement').prop('style', 'display:block');
                        }
                        else {

                            clear_Procurement_fields();
                            clear_Procurement_BackCalcs();
                            $('#_divOptimization').prop('style', 'display:block');
                            $('#_divProcurement').prop('style', 'display:none');

                            if (role_code == 'ADM' && istoadmin == 0) {
                                $('#TxHOComment').prop('disabled', false);
                            }
                            else if (role_code == 'CRT' && istoadmin == 0) {
                                $('#TxRPOCComment').prop('disabled', false);
                            }
                            else if (role_code == 'RO' && istoadmin == 0) {
                                $('#TxAgency').prop('disabled', false);
                            }
                            else if (istoadmin == 1) {
                                $('#TxAgency,#TxRPOCComment,#TxHOComment').prop('disabled', false);
                            }
                        }
                    });
                });
            }
            else {

                clear_Procurement_fields();
                clear_Procurement_BackCalcs();
                $('#_divOptimization').prop('style', 'display:block');
                $('#_divProcurement').prop('style', 'display:none');

                if (role_code == 'ADM' && istoadmin == 0) {
                    $('#TxHOComment').prop('disabled', false);
                }
                else if (role_code == 'CRT' && istoadmin == 0) {
                    $('#TxRPOCComment').prop('disabled', false);
                }
                else if (role_code == 'RO' && istoadmin == 0) {
                    $('#TxAgency').prop('disabled', false);
                }
                else if (istoadmin == 1) {
                    $('#TxAgency,#TxRPOCComment,#TxHOComment').prop('disabled', false);
                }
            }
            //ENH153-2 


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
    $("#LblInitiative").text(SystemYearNow + "XX####");
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
    $('.txSaving,#btnDuplicate,#btnSave,#TxResponsibleName,#TxDesc,#TxLaraCode,#TxPortName,#TxVendorSupp,#TxAdditionalInfo,#TxAgency,#TxRPOCComment,#TxHOComment').prop('disabled', true);
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
    txTarget12.clientEnabled = true;

    //GrdLegalEntity.SetText = '';
    $.post(URLContent('ActiveInitiative/RemoveSelectedGridLookup'), function (data) {
        console.log(data);
    });
    //GrdBrand.GetGridView().Refresh();
}

function OnInit(s, e) {
    if (projectYear > 2022) {
        BtnProcurement.hidden = false;
        BtnInitiative.innerText = "Create Operational Optimization";
    }
    else {
        BtnProcurement.hidden = true;
        BtnInitiative.innerText = "Create New";
    }
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

function OnActionTypePopupChanged(s, e) {
    //debugger
    var selectedItem = s.prevInputValue;
    //if (selectedItem == "Optimization / Efficiencys") {
    //    if ($('#_divOptimization') != null) {
    //        $('#_divOptimization').prop('style', 'display:block')
    //    }
    //    if ($('#_divProcurement') != null) {
    //        $('#_divProcurement').prop('style', 'display:none')
    //    }
    //}
    //else {
    //    if ($('#_divOptimization') != null) {
    //        $('#_divOptimization').prop('style', 'display:none')
    //    }
    //    if ($('#_divProcurement') != null) {
    //        $('#_divProcurement').prop('style', 'display:block')
    //    }
    //}
}

function OnSubCountryPopupChanged(s, e) {

    var isProcurement = $('#isProcurement').val();
    var id = s.GetValue();
    $("#GrdCountryVal").val(''); $("#GrdCountry").val(''); $("#GrdRegionalVal").val(''); $("#GrdRegional").val('');
    $("#GrdSubRegionVal").val(''); $("#GrdSubRegion").val(''); $("#GrdClusterVal").val(''); $("#GrdCluster").val('');
    $("#GrdRegionalOfficeVal").val(''); $("#GrdRegionalOffice").val(''); $("#GrdCostControlVal").val(''); $("#GrdCostControl").val('');
    $.post(URLContent('ActiveInitiative/GetCountryBySub'), { id: id }, function (data) {
        var obj; GrdBrandPopup.ClearItems(); GrdLegalEntityPopup.ClearItems();
        GrdBrandPopup.AddItem("[ Please Select ]", 0);
        $.each(data[0]["BrandData"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj != null) {
                GrdBrandPopup.AddItem(obj.BrandName, obj.id);
            }
        });
        // debugger;
        GrdLegalEntityPopup.AddItem("[ Please Select ]", 0);
        $.each(data[0]["LegalEntityData"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj != null) {
                GrdLegalEntityPopup.AddItem(obj.LegalEntityName, obj.id);
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

        if (isProcurement == 1) {
            setCPI_on_Country_Selection(data[0]["mcpi"]);
            calculate_Procurement_Field();
        }

        GrdBrandPopup.SelectIndex(0);
    });
}

function setCPI_on_Country_Selection(_obj) {
    const months = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec")
    if (_obj != null) {
        var i = 0;
        while (i < 12) {
            var xCPI = _obj[i]["CPI"];
            if (xCPI != null) {
                xCPI = xCPI * 100;
            }
            else {
                xCPI = 0;
            }
            $("#CPI_" + months[i]).val(parseFloat(xCPI));
            i++;
        }
    }

    bind_Monthly_CPI();
}

function OnBrandPopupChanged(s, e) {
    var id = s.GetValue(); var Country = $("#GrdCountryVal").val(); var SubCountry = GrdSubCountryPopup.GetValue(); var CostControlSite = $("#GrdCostControlVal").val();
    $.post(URLContent('ActiveInitiative/GetLegalFromBrand'), { BrandID: id, CountryID: Country, SubCountryID: SubCountry, CostControlSiteID: CostControlSite }, function (data) {
        var obj; GrdLegalEntityPopup.ClearItems();
        $.each(data[0]["LegalEntityData"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj != null) GrdLegalEntityPopup.AddItem(obj.LegalEntityName, obj.id);
        });
        $.each(data[0]["CostControlSiteData"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj != null) {
                $("#GrdCostControlVal").val(obj.id); $("#GrdCostControl").val(obj.CostControlSiteName);
            }
        });
        GrdLegalEntityPopup.SelectIndex(0);

    });
}

function OnGrdInitTypeChanged(s, e) {
    var id = s.GetValue();
    $.post(URLContent('ActiveInitiative/GetItemFromInitiativeType'), { id: id }, function (data) {
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
    var xisProcurement = $('#isProcurement').val();


    if (projectYear <= 2022) {
        //    //  $("$GrdActionType").prop('disabled', false);

        //    if (xisProcurement == 0)
        //        GrdActionType.SelectIndex(43);
        //    else
        //        GrdActionType.SelectIndex(51);
        //}
        //else {
        $.post(URLContent('ActiveInitiative/GetItemFromSubCost'), { id: id }, function (data) {
            var obj; GrdActionType.ClearItems();
            $.each(data[0]["ActionTypeData"], function (key, value) {
                value = JSON.stringify(value); obj = JSON.parse(value);
                if (obj != null) GrdActionType.AddItem(obj.ActionTypeName, obj.id);
            });
            GrdActionType.SelectIndex(0);
        });
        // GrdActionType.SelectIndex(0);
    };
}

function OnGrdInitCategoryPopupChanged(s, e) {
    // debugger;
    var id = GrdInitType.GetValue(); var id2 = s.GetValue(); var id3 = GrdBrandPopup.GetValue();
    $.post(URLContent('ActiveInitiative/GetItemFromCostCategory'), { id: id, id2: id2, id3: id3 }, function (data) {
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
    $.post(URLContent('ActiveInitiative/ProjectYear'), { Id: id }, function () {
        window.location.reload();
    });
}

function OnCboMonthChanged(s, e) {
    var id = s.GetValue();//s.GetValue();
    $.post(URLContent('ActiveInitiative/ProjectMonth'), { Id: id }, function () {
        window.location.reload();
    });
}

function OnClickEventReview(s, e) {
    var idx = s.GetRowKey(e.visibleIndex);
    $('.titleinitiative').html('');
    $.post(URLContent('EventReview/SetEventReviewID'), { ID: idx }, function (data) {
        //WindowEventReview.SetContentHtml(data);
        $('.titleinitiative').html('Initiative Number: ' + data);
        GrdEventReview.Refresh();
        WindowEventReview.Show();
    });
}

function OnClickComment(s, e) {
    var id = s.GetRowKey(e.visibleIndex);
    $.post(URLContent('ActiveInitiative/GetInitiativeComment'), { Id: id }, function (data) {
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
    $.post(URLContent('ActiveInitiative/SetIDUploadFile'), { ID: idx }, function (data) {
        var obj;
        var InitNumber = data[0]["InitiativeNumber"];
        $(".UploadInitiativeNumber").html(InitNumber);

        $.each(data[0]["UploadedFileData"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj.UploadedFileName != null) {
                var output = ""; var arrFileName = obj.UploadedFileName.split("|");
                if (arrFileName[0] != '') {
                    for (var x = 0; x < arrFileName.length; x++) {
                        if (arrFileName[x] != '')
                            output += "<tr><td width=\"660\"><a href=\"" + UploadDirectory + arrFileName[x] + "\" target=\"_new\" >" + arrFileName[x] + "</td><td><button type=\"button\" class=\"btn btn-danger btn-xs\" onClick=\"removefile('" + InitNumber + "','" + arrFileName[x] + "',this)\" >Remove</button></td></tr>";

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

        //@*var rowCount = $("#summary-uploaded-files tr").length;
        //var columnSumm = $("<tr><td><a href=\"@Url.Content(UploadDirectory)" + fileName + "\" target=\"_new\">" + fileName + "</a></td></tr>");
        //console.log(columnSumm.html());*@

        if (isisekarang == '<tr><td colspan="2"><center>There is no File Uploaded</center></td></tr>') {
            $("#summary-uploaded-files").html('');
        }

        if (fileName != '')
            $("#summary-uploaded-files").append("<tr><td width='660'><a href=\"" + UploadDirectory + fileName + "\" target=\"_new\">" + fileName + "</a></td><td><button type=\"button\" class=\"btn btn-danger btn-xs\" onClick=\"removefile('" + initiativenumber + "','" + fileName + "',this)\" >Remove</button></td></tr>");

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
            $.post(URLContent('ActiveInitiative/removefile'), { Initiativenumber: initiativenumber, Filename: filename }, function (data) {
                console.log(data);
                $(btndel).closest('tr').remove();

                var tmp = $("#summary-uploaded-files").html();
                if (tmp == "") $("#summary-uploaded-files").html("<tr><td colspan=\"2\"><center>There is no File Uploaded</center></td></tr>");
                GrdMainInitiative.Refresh();
            });
        }
    });
}

function OnStartMonthChanged() {
    OnEndMonthChanged();

    var awal = StartMonth.GetValue();
    var xisProcurement = $('#isProcurement').val();
    //if (xisProcurement == 1) {

    //    var _dt = new Date(awal.getFullYear(), awal.getMonth(), 1);
    //    EndMonth.SetMinDate(_dt);

    //    var dt = new Date(awal.getFullYear(), 11, 1);
    //    EndMonth.SetMaxDate(dt);
    //}
    //else {
    //    var dt = new Date(awal.getFullYear(), awal.getMonth() + 11, 1);
    //    EndMonth.SetMaxDate(dt);
    //}

    //old code 
    var dt = new Date(awal.getFullYear(), awal.getMonth() + 11, 1);
    EndMonth.SetMaxDate(dt);

    if (xisProcurement == 1) {
        //ENH153-2 calculations caller
        calculate_Actual_CPU_Nmin1();
        calculate_Target_CPU_N();
        calculate_A_Price_effect();
        calculate_A_Volume_Effect();

        calculate_ST_Price_effect();
        calculate_ST_Volume_Effect();

        calculate_CPI_Effect();
        bind_CPI_Fields();
        //ENH153-2 calculations caller
    }
}

function OnEndMonthChanged() {
    var targetstartselected = false; var targetstartselected2 = true; var startmon; /*var projectYear = '@profileData.ProjectYear';*/
    var xisProcurement = $('#isProcurement').val();

    var endmonthValue = '';
    if (EndMonth.GetValue() == null) {
        endmonthValue = StartMonth.GetValue();
    } else {
        endmonthValue = EndMonth.GetValue();
    }

    var targetty = new Array(); var savingty = new Array();
    //remove the IF becasue start month and project year doesnt matter 
    /* if ((new Date(StartMonth.GetValue()).getFullYear()) == projectYear) {*/
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
    // if project year > start year then disable the startyear targets and achievements

    let startyear = StartMonth.GetValue().getFullYear();
    if (projectYear != startyear) {
        for (var i = 0; i < 12; i++) {

            $("." + targetty[i]).prop('disabled', true);
            $("." + savingty[i]).prop('disabled', true);
        }
    }
    //}
    //else if ((new Date(StartMonth.GetValue()).getFullYear()) < projectYear) {
    //    targetty = Array("targetjan", "targetfeb", "targetmar", "targetapr", "targetmay", "targetjun", "targetjul", "targetaug", "targetsep", "targetoct", "targetnov", "targetdec", "targetjan2", "targetfeb2", "targetmar2", "targetapr2", "targetmay2", "targetjun2", "targetjul2", "targetaug2", "targetsep2", "targetoct2", "targetnov2", "targetdec2");
    //    savingty = Array("savingjan", "savingfeb", "savingmar", "savingapr", "savingmay", "savingjun", "savingjul", "savingaug", "savingsep", "savingoct", "savingnov", "savingdec", "savingjan2", "savingfeb2", "savingmar2", "savingapr2", "savingmay2", "savingjun2", "savingjul2", "savingaug2", "savingsep2", "savingoct2", "savingnov2", "savingdec2");

    //    var endmonth = ((moment(endmonthValue).format("M")) - 1);
    //    for (var i = 0; i <= targetty.length; i++) {
    //        if (i <= endmonth) {
    //            $("." + targetty[i]).prop('disabled', false);
    //            $("." + savingty[i]).prop('disabled', false);
    //        } else {
    //            $("." + targetty[i]).val('');
    //            $("." + savingty[i]).val('');
    //            $("." + targetty[i]).prop('disabled', true);
    //            $("." + savingty[i]).prop('disabled', true);
    //        }
    //    }

    //    $("input[name='StartMonth']").prop('disabled', true);
    //    $("input[name='StartMonth']").prop('readonly', true);
    //    StartMonth.clientEnabled = false;
    //}
    /* else if ((new Date(StartMonth.GetValue()).getFullYear()) > projectYear)*/
    //{
    //    targetty = Array("targetjan", "targetfeb", "targetmar", "targetapr", "targetmay", "targetjun", "targetjul", "targetaug", "targetsep", "targetoct", "targetnov", "targetdec", "targetjan2", "targetfeb2", "targetmar2", "targetapr2", "targetmay2", "targetjun2", "targetjul2", "targetaug2", "targetsep2", "targetoct2", "targetnov2", "targetdec2");
    //    savingty = Array("savingjan", "savingfeb", "savingmar", "savingapr", "savingmay", "savingjun", "savingjul", "savingaug", "savingsep", "savingoct", "savingnov", "savingdec", "savingjan2", "savingfeb2", "savingmar2", "savingapr2", "savingmay2", "savingjun2", "savingjul2", "savingaug2", "savingsep2", "savingoct2", "savingnov2", "savingdec2");

    //    var startmonth = ((moment(StartMonth.GetValue()).format("M")) - 1);
    //    var endmonth = ((moment(endmonthValue).format("M")));
    //    for (var i = 0; i <= targetty.length; i++) {
    //        if (i > (+startmonth + 11) && i <= (+endmonth + 11)) {
    //            $("." + targetty[i]).prop('disabled', false);
    //            $("." + savingty[i]).prop('disabled', false);
    //        } else {
    //            $("." + targetty[i]).val('');
    //            $("." + savingty[i]).val('');
    //            $("." + targetty[i]).prop('disabled', true);
    //            $("." + savingty[i]).prop('disabled', true);
    //        }
    //    }
    //}

    if (xisProcurement == 1) {
        //ENH153-2 calculations caller
        calculate_Actual_CPU_Nmin1();
        calculate_Target_CPU_N();
        calculate_A_Price_effect();
        calculate_A_Volume_Effect();

        calculate_ST_Price_effect();
        calculate_ST_Volume_Effect();

        calculate_CPI_Effect();
        bind_CPI_Fields();
        //ENH153-2 calculations caller
    }
}

function OnInitStatusChanged(s, e) {
    var id = s.GetText();
    var uType = user_type;
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
    if (n === 0) {
        return 0;
    }
    else if (n == null || n == "") {
        return "";

    } else {
        n = String(n).replaceAll(',', '');
        return parseFloat(n).toFixed(2).replaceAll(/\d(?=(\d{3})+\.)/g, '$&,');
    }

}

function calculateSum() {
    var sum = 0; var nama;
    $(".txTarget").each(function () {
        nama = this.name;
        if (nama.indexOf("2") < 1) {
            if (this.value.length != 0) {
                sum += parseFloat(this.value.replaceAll(",", ""));
            }
        }
    });
    return sum.toFixed(2);
}

function calculateAllTarget() {
    var sum = 0; var nama;
    $(".txTarget").each(function () {
        nama = this.name;
        if (this.value.length != 0) {
            sum += parseFloat(this.value.replaceAll(",", ""));
        }
    });
    return sum.toFixed(2);
}

function SaveInitiative() {
    Swal.fire({
        title: 'Do you want to save the changes?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Save'
    }).then((result) => {
        if (result.isConfirmed) {
            var xisProcurement = $('#isProcurement').val();
            var xFormStatus = $("#FormStatus").val();
            var xFormID = $("#FormID").val();
            var xGrdSubCountry = GrdSubCountryPopup.GetValue();
            var xGrdBrand = GrdBrandPopup.GetValue();
            var xGrdLegalEntity = GrdLegalEntityPopup.GetValue();
            var xGrdCountry = $("#GrdCountryVal").val();
            var xGrdRegional = $("#GrdRegionalVal").val();
            var xGrdSubRegion = $("#GrdSubRegionVal").val();
            var xGrdCluster = $("#GrdClusterVal").val();
            var xGrdRegionalOffice = $("#GrdRegionalOfficeVal").val();
            var xGrdCostControl = $("#GrdCostControlVal").val();
            var xCboConfidential = CboConfidential.GetValue();
            var xTxResponsibleName = $("#TxResponsibleName").val();
            var xTxDesc = $("#TxDesc").val();
            var xGrdInitStatus = GrdInitStatus.GetValue();
            var xTxLaraCode = $("#TxLaraCode").val();
            var xTxPortName = (TxPortName.GetValue() == 0 ? 1 : TxPortName.GetValue());
            var xTxVendorSupp = $("#TxVendorSupp").val();
            var xTxAdditionalInfo = $("#TxAdditionalInfo").val();
            var xGrdInitType = GrdInitType.GetValue();
            var xGrdInitCategory = GrdInitCategory.GetValue();
            var xGrdSubCost = GrdSubCost.GetValue();
            var xGrdActionType = GrdActionType.GetValue();
            var xGrdSynImpact = GrdSynImpact.GetValue();
            var xStartMonth = StartMonth.GetValue();
            if (xStartMonth != null) xStartMonth = xStartMonth.format("Y-m-d");
            var xEndMonth = EndMonth.GetValue();
            if (xEndMonth != null) xEndMonth = xEndMonth.format("Y-m-d");
            var xTxAgency = $("#TxAgency").val();
            var xTxRPOCComment = $("#TxRPOCComment").val();
            var xTxHOComment = $("#TxHOComment").val();
            var xCboHoValidity = CboHoValidity.GetValue();
            var xCboRPOCValidity = CboRPOCValidity.GetValue();
            var xTxTarget12 = txTarget12.GetValue();
            var xTxTargetFullYear = txTargetFullYear.GetValue();
            var xTxYTDTargetFullYear = txYTDTargetFullYear.GetValue();
            var xTxYTDSavingFullYear = txYTDSavingFullYear.GetValue();
            var xProjectYear = CboYear.GetText();
            var xRelatedInitiative = LblRelatedInitiative.GetValue();
            var xCboWebinarCat = CboWebinarCat.GetValue();

            //ENH153-2 procurment properties get field value in variable
            var xUnit_of_volumes = CboUnitOfVolume.GetValue();
            var xInput_Actuals_Volumes_Nmin1 = txt_Input_Actuals_Volumes_Nmin1.GetValue();
            var xInput_Target_Volumes = txt_Input_Target_Volumes.GetValue();
            var xTotal_Actual_volume_N = txt_Total_Actual_volume_N.GetValue();
            var xSpend_Nmin1 = txt_Spend_Nmin1.GetValue();
            var xSpend_N = txt_Spend_N.GetValue();
            var xCPI = txt_CPI.GetValue();
            var xjanActual_volume_N = txt_janActual_volume_N.GetValue();
            var xfebActual_volume_N = txt_febActual_volume_N.GetValue();
            var xmarActual_volume_N = txt_marActual_volume_N.GetValue();
            var xaprActual_volume_N = txt_aprActual_volume_N.GetValue();
            var xmayActual_volume_N = txt_mayActual_volume_N.GetValue();
            var xjunActual_volume_N = txt_junActual_volume_N.GetValue();
            var xjulActual_volume_N = txt_julActual_volume_N.GetValue();
            var xaugActual_volume_N = txt_augActual_volume_N.GetValue();
            var xsepActual_volume_N = txt_sepActual_volume_N.GetValue();
            var xoctActual_volume_N = txt_octActual_volume_N.GetValue();
            var xnovActual_volume_N = txt_novActual_volume_N.GetValue();
            var xdecActual_volume_N = txt_decActual_volume_N.GetValue();
            var xN_FY_Sec_PRICE_EF = txt_N_FY_Sec_PRICE_EF.GetValue();
            var xN_FY_Sec_VOLUME_EF = txt_N_FY_Sec_VOLUME_EF.GetValue();
            var xN_YTD_Sec_PRICE_EF = txt_N_YTD_Sec_PRICE_EF.GetValue();
            var xN_YTD_Sec_VOLUME_EF = txt_N_YTD_Sec_VOLUME_EF.GetValue();
            var xYTD_Achieved_PRICE_EF = txt_YTD_Achieved_PRICE_EF.GetValue();
            var xYTD_Achieved_VOLUME_EF = txt_YTD_Achieved_VOLUME_EF.GetValue();
            var xYTD_Cost_Avoid_Vs_CPI = txt_YTD_Cost_Avoid_Vs_CPI.GetValue();
            var xFY_Cost_Avoid_Vs_CPI = txt_FY_Cost_Avoid_Vs_CPI.GetValue();
            //ENH153-2 procurment properties get field value in variable

            //ENH153-2 procurment back end calculation get field value in variable

            var xjan_Actual_CPU_Nmin1 = $('#Actual_CPU_Nmin1_Jan').val();
            var xfeb_Actual_CPU_Nmin1 = $('#Actual_CPU_Nmin1_Feb').val();
            var xmarch_Actual_CPU_Nmin1 = $('#Actual_CPU_Nmin1_Mar').val();
            var xapr_Actual_CPU_Nmin1 = $('#Actual_CPU_Nmin1_Apr').val();
            var xmay_Actual_CPU_Nmin1 = $('#Actual_CPU_Nmin1_May').val();
            var xjun_Actual_CPU_Nmin1 = $('#Actual_CPU_Nmin1_Jun').val();
            var xjul_Actual_CPU_Nmin1 = $('#Actual_CPU_Nmin1_Jul').val();
            var xaug_Actual_CPU_Nmin1 = $('#Actual_CPU_Nmin1_Aug').val();
            var xsep_Actual_CPU_Nmin1 = $('#Actual_CPU_Nmin1_Sep').val();
            var xoct_Actual_CPU_Nmin1 = $('#Actual_CPU_Nmin1_Oct').val();
            var xnov_Actual_CPU_Nmin1 = $('#Actual_CPU_Nmin1_Nov').val();
            var xdec_Actual_CPU_Nmin1 = $('#Actual_CPU_Nmin1_Dec').val();
            var xjan_Target_CPU_N = $('#Target_CPU_N_Jan').val();
            var xfeb_Target_CPU_N = $('#Target_CPU_N_Feb').val();
            var xmarch_Target_CPU_N = $('#Target_CPU_N_Mar').val();
            var xapr_Target_CPU_N = $('#Target_CPU_N_Apr').val();
            var xmay_Target_CPU_N = $('#Target_CPU_N_May').val();
            var xjun_Target_CPU_N = $('#Target_CPU_N_Jun').val();
            var xjul_Target_CPU_N = $('#Target_CPU_N_Jul').val();
            var xaug_Target_CPU_N = $('#Target_CPU_N_Aug').val();
            var xsep_Target_CPU_N = $('#Target_CPU_N_Sep').val();
            var xoct_Target_CPU_N = $('#Target_CPU_N_Oct').val();
            var xnov_Target_CPU_N = $('#Target_CPU_N_Nov').val();
            var xdec_Target_CPU_N = $('#Target_CPU_N_Dec').val();
            var xjan_A_Price_effect = $('#A_Price_effect_Jan').val();
            var xfeb_A_Price_effect = $('#A_Price_effect_Feb').val();
            var xmarch_A_Price_effect = $('#A_Price_effect_Mar').val();
            var xapr_A_Price_effect = $('#A_Price_effect_Apr').val();
            var xmay_A_Price_effect = $('#A_Price_effect_May').val();
            var xjun_A_Price_effect = $('#A_Price_effect_Jun').val();
            var xjul_A_Price_effect = $('#A_Price_effect_Jul').val();
            var xaug_A_Price_effect = $('#A_Price_effect_Aug').val();
            var xsep_A_Price_effect = $('#A_Price_effect_Sep').val();
            var xoct_A_Price_effect = $('#A_Price_effect_Oct').val();
            var xnov_A_Price_effect = $('#A_Price_effect_Nov').val();
            var xdec_A_Price_effect = $('#A_Price_effect_Dec').val();
            var xjan_A_Volume_Effect = $('#A_Volume_Effect_Jan').val();
            var xfeb_A_Volume_Effect = $('#A_Volume_Effect_Feb').val();
            var xmarch_A_Volume_Effect = $('#A_Volume_Effect_Mar').val();
            var xapr_A_Volume_Effect = $('#A_Volume_Effect_Apr').val();
            var xmay_A_Volume_Effect = $('#A_Volume_Effect_May').val();
            var xjun_A_Volume_Effect = $('#A_Volume_Effect_Jun').val();
            var xjul_A_Volume_Effect = $('#A_Volume_Effect_Jul').val();
            var xaug_A_Volume_Effect = $('#A_Volume_Effect_Aug').val();
            var xsep_A_Volume_Effect = $('#A_Volume_Effect_Sep').val();
            var xoct_A_Volume_Effect = $('#A_Volume_Effect_Oct').val();
            var xnov_A_Volume_Effect = $('#A_Volume_Effect_Nov').val();
            var xdec_A_Volume_Effect = $('#A_Volume_Effect_Dec').val();
            var xjan_Achievement = $('#Achievement_Jan').val();
            var xfeb_Achievement = $('#Achievement_Feb').val();
            var xmarch_Achievement = $('#Achievement_Mar').val();
            var xapr_Achievement = $('#Achievement_Apr').val();
            var xmay_Achievement = $('#Achievement_May').val();
            var xjun_Achievement = $('#Achievement_Jun').val();
            var xjul_Achievement = $('#Achievement_Jul').val();
            var xaug_Achievement = $('#Achievement_Aug').val();
            var xsep_Achievement = $('#Achievement_Sep').val();
            var xoct_Achievement = $('#Achievement_Oct').val();
            var xnov_Achievement = $('#Achievement_Nov').val();
            var xdec_Achievement = $('#Achievement_Dec').val();
            var xjan_ST_Price_effect = $('#ST_Price_effect_Jan').val();
            var xfeb_ST_Price_effect = $('#ST_Price_effect_Feb').val();
            var xmarch_ST_Price_effect = $('#ST_Price_effect_Mar').val();
            var xapr_ST_Price_effect = $('#ST_Price_effect_Apr').val();
            var xmay_ST_Price_effect = $('#ST_Price_effect_May').val();
            var xjun_ST_Price_effect = $('#ST_Price_effect_Jun').val();
            var xjul_ST_Price_effect = $('#ST_Price_effect_Jul').val();
            var xaug_ST_Price_effect = $('#ST_Price_effect_Aug').val();
            var xsep_ST_Price_effect = $('#ST_Price_effect_Sep').val();
            var xoct_ST_Price_effect = $('#ST_Price_effect_Oct').val();
            var xnov_ST_Price_effect = $('#ST_Price_effect_Nov').val();
            var xdec_ST_Price_effect = $('#ST_Price_effect_Dec').val();
            var xjan_ST_Volume_Effect = $('#ST_Volume_Effect_Jan').val();
            var xfeb_ST_Volume_Effect = $('#ST_Volume_Effect_Feb').val();
            var xmarch_ST_Volume_Effect = $('#ST_Volume_Effect_Mar').val();
            var xapr_ST_Volume_Effect = $('#ST_Volume_Effect_Apr').val();
            var xmay_ST_Volume_Effect = $('#ST_Volume_Effect_May').val();
            var xjun_ST_Volume_Effect = $('#ST_Volume_Effect_Jun').val();
            var xjul_ST_Volume_Effect = $('#ST_Volume_Effect_Jul').val();
            var xaug_ST_Volume_Effect = $('#ST_Volume_Effect_Aug').val();
            var xsep_ST_Volume_Effect = $('#ST_Volume_Effect_Sep').val();
            var xoct_ST_Volume_Effect = $('#ST_Volume_Effect_Oct').val();
            var xnov_ST_Volume_Effect = $('#ST_Volume_Effect_Nov').val();
            var xdec_ST_Volume_Effect = $('#ST_Volume_Effect_Dec').val();
            var xjan_FY_Secured_Target = $('#FY_Secured_Target_Jan').val();
            var xfeb_FY_Secured_Target = $('#FY_Secured_Target_Feb').val();
            var xmarch_FY_Secured_Target = $('#FY_Secured_Target_Mar').val();
            var xapr_FY_Secured_Target = $('#FY_Secured_Target_Apr').val();
            var xmay_FY_Secured_Target = $('#FY_Secured_Target_May').val();
            var xjun_FY_Secured_Target = $('#FY_Secured_Target_Jun').val();
            var xjul_FY_Secured_Target = $('#FY_Secured_Target_Jul').val();
            var xaug_FY_Secured_Target = $('#FY_Secured_Target_Aug').val();
            var xsep_FY_Secured_Target = $('#FY_Secured_Target_Sep').val();
            var xoct_FY_Secured_Target = $('#FY_Secured_Target_Oct').val();
            var xnov_FY_Secured_Target = $('#FY_Secured_Target_Nov').val();
            var xdec_FY_Secured_Target = $('#FY_Secured_Target_Dec').val();
            var xjan_CPI_Effect = $('#CPI_Effect_Jan').val();
            var xfeb_CPI_Effect = $('#CPI_Effect_Feb').val();
            var xmarch_CPI_Effect = $('#CPI_Effect_Mar').val();
            var xapr_CPI_Effect = $('#CPI_Effect_Apr').val();
            var xmay_CPI_Effect = $('#CPI_Effect_May').val();
            var xjun_CPI_Effect = $('#CPI_Effect_Jun').val();
            var xjul_CPI_Effect = $('#CPI_Effect_Jul').val();
            var xaug_CPI_Effect = $('#CPI_Effect_Aug').val();
            var xsep_CPI_Effect = $('#CPI_Effect_Sep').val();
            var xoct_CPI_Effect = $('#CPI_Effect_Oct').val();
            var xnov_CPI_Effect = $('#CPI_Effect_Nov').val();
            var xdec_CPI_Effect = $('#CPI_Effect_Dec').val();

            var xjan_CPI = $('#CPI_Jan').val();
            var xfeb_CPI = $('#CPI_Feb').val();
            var xmar_CPI = $('#CPI_Mar').val();
            var xapr_CPI = $('#CPI_Apr').val();
            var xmay_CPI = $('#CPI_May').val();
            var xjun_CPI = $('#CPI_Jun').val();
            var xjul_CPI = $('#CPI_Jul').val();
            var xaug_CPI = $('#CPI_Aug').val();
            var xsep_CPI = $('#CPI_Sep').val();
            var xoct_CPI = $('#CPI_Oct').val();
            var xnov_CPI = $('#CPI_Nov').val();
            var xdec_CPI = $('#CPI_Dec').val();

            //ENH153-2 procurment back end calculation get field value in variable

            //var projectYear = '@profileData.ProjectYear';
            //Comment the below clause to save everything to DB 
            //if ((new Date(StartMonth.GetValue()).getFullYear()) < projectYear) {
            //    // gak usah disimpen year yang sebelumnya
            //    var targetjan2 = $(".targetjan").val().replaceAll(",", ""); var targetfeb2 = $(".targetfeb").val().replaceAll(",", ""); var targetmar2 = $(".targetmar").val().replaceAll(",", ""); var targetapr2 = $(".targetapr").val().replaceAll(",", ""); var targetmay2 = $(".targetmay").val().replaceAll(",", "");
            //    var targetjun2 = $(".targetjun").val().replaceAll(",", ""); var targetjul2 = $(".targetjul").val().replaceAll(",", ""); var targetaug2 = $(".targetaug").val().replaceAll(",", ""); var targetsep2 = $(".targetsep").val().replaceAll(",", ""); var targetoct2 = $(".targetoct").val().replaceAll(",", "");
            //    var targetnov2 = $(".targetnov").val().replaceAll(",", ""); var targetdec2 = $(".targetdec").val().replaceAll(",", "");

            //    var savingjan2 = $(".savingjan").val().replaceAll(",", ""); var savingfeb2 = $(".savingfeb").val().replaceAll(",", ""); var savingmar2 = $(".savingmar").val().replaceAll(",", ""); var savingapr2 = $(".savingapr").val().replaceAll(",", ""); var savingmay2 = $(".savingmay").val().replaceAll(",", "");
            //    var savingjun2 = $(".savingjun").val().replaceAll(",", ""); var savingjul2 = $(".savingjul").val().replaceAll(",", ""); var savingaug2 = $(".savingaug").val().replaceAll(",", ""); var savingsep2 = $(".savingsep").val().replaceAll(",", ""); var savingoct2 = $(".savingoct").val().replaceAll(",", "");
            //    var savingnov2 = $(".savingnov").val().replaceAll(",", ""); var savingdec2 = $(".savingdec").val().replaceAll(",", "");
            //} else {
            var targetjan = $(".targetjan").val().replaceAll(",", ""); var targetfeb = $(".targetfeb").val().replaceAll(",", ""); var targetmar = $(".targetmar").val().replaceAll(",", ""); var targetapr = $(".targetapr").val().replaceAll(",", ""); var targetmay = $(".targetmay").val().replaceAll(",", "");
            var targetjun = $(".targetjun").val().replaceAll(",", ""); var targetjul = $(".targetjul").val().replaceAll(",", ""); var targetaug = $(".targetaug").val().replaceAll(",", ""); var targetsep = $(".targetsep").val().replaceAll(",", ""); var targetoct = $(".targetoct").val().replaceAll(",", "");
            var targetnov = $(".targetnov").val().replaceAll(",", ""); var targetdec = $(".targetdec").val().replaceAll(",", "");
            var targetjan2 = $(".targetjan2").val().replaceAll(",", ""); var targetfeb2 = $(".targetfeb2").val().replaceAll(",", ""); var targetmar2 = $(".targetmar2").val().replaceAll(",", ""); var targetapr2 = $(".targetapr2").val().replaceAll(",", ""); var targetmay2 = $(".targetmay2").val().replaceAll(",", "");
            var targetjun2 = $(".targetjun2").val().replaceAll(",", ""); var targetjul2 = $(".targetjul2").val().replaceAll(",", ""); var targetaug2 = $(".targetaug2").val().replaceAll(",", ""); var targetsep2 = $(".targetsep2").val().replaceAll(",", ""); var targetoct2 = $(".targetoct2").val().replaceAll(",", "");
            var targetnov2 = $(".targetnov2").val().replaceAll(",", ""); var targetdec2 = $(".targetdec2").val().replaceAll(",", "");

            var savingjan = $(".savingjan").val().replaceAll(",", ""); var savingfeb = $(".savingfeb").val().replaceAll(",", ""); var savingmar = $(".savingmar").val().replaceAll(",", ""); var savingapr = $(".savingapr").val().replaceAll(",", ""); var savingmay = $(".savingmay").val().replaceAll(",", "");
            var savingjun = $(".savingjun").val().replaceAll(",", ""); var savingjul = $(".savingjul").val().replaceAll(",", ""); var savingaug = $(".savingaug").val().replaceAll(",", ""); var savingsep = $(".savingsep").val().replaceAll(",", ""); var savingoct = $(".savingoct").val().replaceAll(",", "");
            var savingnov = $(".savingnov").val().replaceAll(",", ""); var savingdec = $(".savingdec").val().replaceAll(",", "");
            var savingjan2 = $(".savingjan2").val().replaceAll(",", ""); var savingfeb2 = $(".savingfeb2").val().replaceAll(",", ""); var savingmar2 = $(".savingmar2").val().replaceAll(",", ""); var savingapr2 = $(".savingapr2").val().replaceAll(",", ""); var savingmay2 = $(".savingmay2").val().replaceAll(",", "");
            var savingjun2 = $(".savingjun2").val().replaceAll(",", ""); var savingjul2 = $(".savingjul2").val().replaceAll(",", ""); var savingaug2 = $(".savingaug2").val().replaceAll(",", ""); var savingsep2 = $(".savingsep2").val().replaceAll(",", ""); var savingoct2 = $(".savingoct2").val().replaceAll(",", "");
            var savingnov2 = $(".savingnov2").val().replaceAll(",", ""); var savingdec2 = $(".savingdec2").val().replaceAll(",", "");
            //}

            //alert(xGrdSubCountry + " " + xGrdBrand + " " + xGrdLegalEntity + " " + xGrdCountry + " " + xGrdRegional + " " + xGrdSubRegion + " " + xGrdCluster + " " + xGrdRegionalOffice + " " + xGrdCostControl + " " + xCboConfidential + " " + xGrdInitStatus + " " + xGrdInitType + " " + xGrdInitCategory + " " + xGrdSubCost + " " + xGrdActionType + " " + xGrdSynImpact + " " + xStartMonth + " " + xEndMonth);

            if (
                xGrdSubCountry != null && xGrdBrand != null && xGrdLegalEntity != null && xGrdCountry != null && xGrdRegional != null &&
                xGrdSubRegion != null && xGrdCluster != null && xGrdRegionalOffice != null && xGrdCostControl != null && xCboConfidential != null &&
                xGrdInitStatus > 0 && xGrdInitType > 0 && xGrdInitCategory > 0 && xGrdSubCost > 0 && xGrdActionType > 0 &&
                xGrdSynImpact > 0 && xStartMonth != null && xEndMonth != null
                &&
                isAll_Procurement_Mandatory_fields_entered(xisProcurement)
            ) {
                var sum = 0; var x2 = 0;
                $(".txTarget").each(function () {
                    if (isNaN(parseFloat($(this).val())))
                        sum += 0;
                    else
                        sum += parseFloat($(this).val().replaceAll(",", ""));
                });

                // if ((new Date(StartMonth.GetValue()).getFullYear()) == projectYear) {
                var a = (parseFloat(txTargetFullYear.GetValue()));
                var b = (parseFloat(txTarget12.GetValue()));
                //code change by Sudhish for -ve /+ve

                var originalfullyeartarget = a;
                var originaltwelevetarget = b;
                var sumofmonthlytarget = sum;
                //Need to ABS first then floor
                //a = Math.abs(Math.floor(a));
                //b = Math.abs(Math.floor(b));
                //sum = Math.abs(Math.floor(sum));

                a = Math.floor(Math.abs(a));
                b = Math.floor(Math.abs(b));
                sum = Math.floor(Math.abs(sum));
                //if ((a >= b) || (sum != (b))) {
                //    Swal.fire(
                //        'Target 12 Months Less Than Total Target Field',
                //        'Target This Year Cannot Greater Than Target 12 Months and Target 12 Months Cannot Less Than Total Monthly Target Field',
                //        'error'
                //    );
                //    return;
                //}
                //var sign =Math.sign(a)
                //do the comparison only if signs are equal 

                /* if (Math.sign(originaltwelevetarget) == Math.sign(originalfullyeartarget)) {*/
                if (Math.sign(originaltwelevetarget) == Math.sign(sumofmonthlytarget)) {
                    //if (a > b) {
                    //    Swal.fire(
                    //        'Inconsistent Targets',
                    //        'All Applicable Target of This Year : <strong>' + originalfullyeartarget +'</strong> cannot be Greater Than Target 12 Months : <strong>'+originaltwelevetarget+'</strong>',
                    //        'error'
                    //    );
                    //    return;
                    //}

                    if (!((sum) == (b + 1) || (sum) == b || (sum + 1) == b)) { // tolerance $1
                        Swal.fire(
                            'Inconsistent Target',
                            'The amount of All Applicable Target (current SUM of input is <strong>' + sumofmonthlytarget + '</strong>) and Target 12 Months (current input as <strong> ' + originaltwelevetarget + '</strong>) need to be aligned',
                            'error'
                        );
                        return;
                    }
                }
                else {
                    // debugger;
                    Swal.fire(
                        'Inconsistent Target',
                        'Sum of monthly target and 12 months target,both  should be positive or negative',
                        'error'
                    );
                    return;
                }
                //}

                //else {
                //    // debugger;
                //    Swal.fire(
                //        'Inconsistent Target',
                //        'Target 12 Months and Target Current Year both should be positive or negative value',
                //        'error'
                //    );
                //    return;
                //}

                // }
                //I think the below is not required since all the values are shown so the alert control should be simple 
                //else if ((new Date(StartMonth.GetValue()).getFullYear()) < projectYear) {
                //    // debugger;
                //    var x1 = StartMonth.GetValue().getMonth();
                //    var months;
                //    months = (EndMonth.GetValue().getFullYear() - StartMonth.GetValue().getFullYear()) * 12;
                //    months -= StartMonth.GetValue().getMonth();
                //    months += EndMonth.GetValue().getMonth();
                //    months++; // ngitung selisih bulan mulai dan bulan akhir initiative
                //    lastmonth = EndMonth.GetValue().getMonth();
                //    lastmonth++;
                //    var a = (parseFloat(txTargetFullYear.GetValue()));
                //    var b = (parseFloat(txTarget12.GetValue()));
                //    //Added by Sudhish for positive /-ve 
                //    var originalfullyeartarget = a;
                //    var originaltwelevetarget = b;
                //    var sumofmonthlytarget = sum;
                //    x1 = Math.abs(x1);
                //    x2 = Math.abs(((b / months)) * (12 - x1));//x2 = ((12 - x1) * Math.abs(Math.floor(targetjan2)));
                //    //x2 = Math.abs(Math.floor(x2));

                //    //a = Math.abs(Math.floor(a));
                //    //b = Math.abs(Math.floor(b));
                //    //sum = Math.abs(Math.floor(sum));

                //    //Floor after abs for -ve comparison 
                //    x2 = Math.floor(Math.abs(x2));

                //    a = Math.floor(Math.abs(a));
                //    b = Math.floor(Math.abs(b));
                //    sum = Math.floor(Math.abs(sum));
                //    //alert('a = ' + a + ' || b = ' + b + ' || sum = ' + sum + ' || x2 = ' + x2 + " || months = " + months + " || (12 - x1) = " + (12 - x1));
                //    //if ((a >= b) || ((x2 + sum) > (b + 5))) { // tolerance $5
                //    if (Math.sign(originaltwelevetarget) == Math.sign(sumofmonthlytarget)) {
                //        if (Math.sign(originaltwelevetarget) == Math.sign(sumofmonthlytarget)) {
                //            //if (a > b) {
                //            //    Swal.fire(
                //            //        'Inconsistent Target',
                //            //         'All Applicable Target of This Year : <strong>' + originalfullyeartarget +'</strong> cannot be Greater Than Target 12 Months : <strong>'+originaltwelevetarget+'</strong>',
                //            //        'error'
                //            //    );
                //            //    return;
                //            //}

                //            if (!((x2 + sum) == (b + 1) || (x2 + sum) == b ||(x2+sum+1) == b)) { // tolerance $1
                //                ////Swal.fire(
                //                ////    'Inconsistent Target',
                //                ////    'The amount of All Applicable Target (current SUM of input is <strong>' + (sum) + '</strong>) and current year target <strong>'+Math.floor((b/months)*lastmonth)+
                //                ////    '</strong> (prorated target 12 month for current year('+b+'/'+months+'*'+lastmonth+')) need to be aligned',
                //                ////    'error'
                //                ////);
                //                //return;
                //            }
                //        }
                //        else {
                //            Swal.fire(
                //                'Inconsistent Target',
                //                'Sum of monthly target and 12 months target,both  should be positive or negative',
                //                'error'
                //            );
                //            return;
                //        }
                //    }
                //    else {
                //        Swal.fire(
                //            'Inconsistent Target',
                //            'Target 12 Months and Target Current Year both should be positive or negative value',
                //            'error'
                //        );
                //        return;
                //    }
                //}
                var sumtarget = 0;
                if ((new Date(StartMonth.GetValue()).getFullYear()) == projectYear) {
                    var sum = 0;
                    sumtarget = calculateSum();
                    sumtarget = Math.abs(sumtarget);
                } else if ((new Date(StartMonth.GetValue()).getFullYear()) < projectYear) {
                    sumtarget = Math.abs(sum);
                    //sumtarget = (((12 - (StartMonth.GetValue().getMonth())) * targetjan2) + sum);
                }
                // debugger;
                //if (Number(xTxTargetFullYear).toFixed(0) == Number(sumtarget).toFixed(0)) {
                // if (Math.floor(Math.abs(Number(xTxTargetFullYear))) == Math.floor(Number(sumtarget))) {
                $.post(URLContent('ActiveInitiative/SaveNew'), {
                    FormID: xFormID,
                    FormStatus: xFormStatus,
                    GrdSubCountry: xGrdSubCountry,
                    GrdBrand: xGrdBrand,
                    GrdLegalEntity: xGrdLegalEntity,
                    GrdCountry: xGrdCountry,
                    GrdRegional: xGrdRegional,
                    GrdSubRegion: xGrdSubRegion,
                    GrdCluster: xGrdCluster,
                    GrdRegionalOffice: xGrdRegionalOffice,
                    GrdCostControl: xGrdCostControl,
                    CboConfidential: xCboConfidential,
                    TxResponsibleName: xTxResponsibleName,
                    TxDesc: xTxDesc,
                    GrdInitStatus: xGrdInitStatus,
                    TxLaraCode: xTxLaraCode,
                    TxPortName: xTxPortName,
                    TxVendorSupp: xTxVendorSupp,
                    TxAdditionalInfo: xTxAdditionalInfo,
                    GrdInitType: xGrdInitType,
                    GrdInitCategory: xGrdInitCategory,
                    GrdSubCost: xGrdSubCost,
                    GrdActionType: xGrdActionType,
                    GrdSynImpact: xGrdSynImpact,
                    StartMonth: xStartMonth,
                    EndMonth: xEndMonth,
                    TxAgency: xTxAgency,
                    TxRPOCComment: xTxRPOCComment,
                    TxHOComment: xTxHOComment,
                    CboHoValidity: xCboHoValidity,
                    CboRPOCValidity: xCboRPOCValidity,
                    TxTarget12: xTxTarget12,
                    TxTargetFullYear: xTxTargetFullYear,
                    TxYTDTargetFullYear: xTxYTDTargetFullYear,
                    TxYTDSavingFullYear: xTxYTDSavingFullYear,
                    ProjectYear: xProjectYear,
                    RelatedInitiative: xRelatedInitiative,
                    SourceCategory: xCboWebinarCat,
                    targetjan: targetjan, targetfeb: targetfeb, targetmar: targetmar, targetapr: targetapr, targetmay: targetmay, targetjun: targetjun, targetjul: targetjul, targetaug: targetaug, targetsep: targetsep, targetoct: targetoct, targetnov: targetnov, targetdec: targetdec,
                    targetjan2: targetjan2, targetfeb2: targetfeb2, targetmar2: targetmar2, targetapr2: targetapr2, targetmay2: targetmay2, targetjun2: targetjun2, targetjul2: targetjul2, targetaug2: targetaug2, targetsep2: targetsep2, targetoct2: targetoct2, targetnov2: targetnov2, targetdec2: targetdec2,
                    savingjan: savingjan, savingfeb: savingfeb, savingmar: savingmar, savingapr: savingapr, savingmay: savingmay, savingjun: savingjun, savingjul: savingjul, savingaug: savingaug, savingsep: savingsep, savingoct: savingoct, savingnov: savingnov, savingdec: savingdec,
                    savingjan2: savingjan2, savingfeb2: savingfeb2, savingmar2: savingmar2, savingapr2: savingapr2, savingmay2: savingmay2, savingjun2: savingjun2, savingjul2: savingjul2, savingaug2: savingaug2, savingsep2: savingsep2, savingoct2: savingoct2, savingnov2: savingnov2, savingdec2: savingdec2,
                    Unit_of_volumes: xUnit_of_volumes,
                    Input_Actuals_Volumes_Nmin1: xInput_Actuals_Volumes_Nmin1,
                    Input_Target_Volumes: xInput_Target_Volumes,
                    Total_Actual_volume_N: xTotal_Actual_volume_N,
                    Spend_Nmin1: xSpend_Nmin1,
                    Spend_N: xSpend_N,
                    CPI: xCPI,
                    janActual_volume_N: xjanActual_volume_N,
                    febActual_volume_N: xfebActual_volume_N,
                    marActual_volume_N: xmarActual_volume_N,
                    aprActual_volume_N: xaprActual_volume_N,
                    mayActual_volume_N: xmayActual_volume_N,
                    junActual_volume_N: xjunActual_volume_N,
                    julActual_volume_N: xjulActual_volume_N,
                    augActual_volume_N: xaugActual_volume_N,
                    sepActual_volume_N: xsepActual_volume_N,
                    octActual_volume_N: xoctActual_volume_N,
                    novActual_volume_N: xnovActual_volume_N,
                    decActual_volume_N: xdecActual_volume_N,
                    N_FY_Sec_PRICE_EF: xN_FY_Sec_PRICE_EF,
                    N_FY_Sec_VOLUME_EF: xN_FY_Sec_VOLUME_EF,
                    N_YTD_Sec_PRICE_EF: xN_YTD_Sec_PRICE_EF,
                    N_YTD_Sec_VOLUME_EF: xN_YTD_Sec_VOLUME_EF,
                    YTD_Achieved_PRICE_EF: xYTD_Achieved_PRICE_EF,
                    YTD_Achieved_VOLUME_EF: xYTD_Achieved_VOLUME_EF,
                    YTD_Cost_Avoid_Vs_CPI: xYTD_Cost_Avoid_Vs_CPI,
                    FY_Cost_Avoid_Vs_CPI: xFY_Cost_Avoid_Vs_CPI,
                    isProcurement: xisProcurement,
                    _t_initiative_calcs: {

                        jan_Actual_CPU_Nmin1: xjan_Actual_CPU_Nmin1,
                        feb_Actual_CPU_Nmin1: xfeb_Actual_CPU_Nmin1,
                        march_Actual_CPU_Nmin1: xmarch_Actual_CPU_Nmin1,
                        apr_Actual_CPU_Nmin1: xapr_Actual_CPU_Nmin1,
                        may_Actual_CPU_Nmin1: xmay_Actual_CPU_Nmin1,
                        jun_Actual_CPU_Nmin1: xjun_Actual_CPU_Nmin1,
                        jul_Actual_CPU_Nmin1: xjul_Actual_CPU_Nmin1,
                        aug_Actual_CPU_Nmin1: xaug_Actual_CPU_Nmin1,
                        sep_Actual_CPU_Nmin1: xsep_Actual_CPU_Nmin1,
                        oct_Actual_CPU_Nmin1: xoct_Actual_CPU_Nmin1,
                        nov_Actual_CPU_Nmin1: xnov_Actual_CPU_Nmin1,
                        dec_Actual_CPU_Nmin1: xdec_Actual_CPU_Nmin1,
                        jan_Target_CPU_N: xjan_Target_CPU_N,
                        feb_Target_CPU_N: xfeb_Target_CPU_N,
                        march_Target_CPU_N: xmarch_Target_CPU_N,
                        apr_Target_CPU_N: xapr_Target_CPU_N,
                        may_Target_CPU_N: xmay_Target_CPU_N,
                        jun_Target_CPU_N: xjun_Target_CPU_N,
                        jul_Target_CPU_N: xjul_Target_CPU_N,
                        aug_Target_CPU_N: xaug_Target_CPU_N,
                        sep_Target_CPU_N: xsep_Target_CPU_N,
                        oct_Target_CPU_N: xoct_Target_CPU_N,
                        nov_Target_CPU_N: xnov_Target_CPU_N,
                        dec_Target_CPU_N: xdec_Target_CPU_N,
                        jan_A_Price_effect: xjan_A_Price_effect,
                        feb_A_Price_effect: xfeb_A_Price_effect,
                        march_A_Price_effect: xmarch_A_Price_effect,
                        apr_A_Price_effect: xapr_A_Price_effect,
                        may_A_Price_effect: xmay_A_Price_effect,
                        jun_A_Price_effect: xjun_A_Price_effect,
                        jul_A_Price_effect: xjul_A_Price_effect,
                        aug_A_Price_effect: xaug_A_Price_effect,
                        sep_A_Price_effect: xsep_A_Price_effect,
                        oct_A_Price_effect: xoct_A_Price_effect,
                        nov_A_Price_effect: xnov_A_Price_effect,
                        dec_A_Price_effect: xdec_A_Price_effect,
                        jan_A_Volume_Effect: xjan_A_Volume_Effect,
                        feb_A_Volume_Effect: xfeb_A_Volume_Effect,
                        march_A_Volume_Effect: xmarch_A_Volume_Effect,
                        apr_A_Volume_Effect: xapr_A_Volume_Effect,
                        may_A_Volume_Effect: xmay_A_Volume_Effect,
                        jun_A_Volume_Effect: xjun_A_Volume_Effect,
                        jul_A_Volume_Effect: xjul_A_Volume_Effect,
                        aug_A_Volume_Effect: xaug_A_Volume_Effect,
                        sep_A_Volume_Effect: xsep_A_Volume_Effect,
                        oct_A_Volume_Effect: xoct_A_Volume_Effect,
                        nov_A_Volume_Effect: xnov_A_Volume_Effect,
                        dec_A_Volume_Effect: xdec_A_Volume_Effect,
                        jan_Achievement: xjan_Achievement,
                        feb_Achievement: xfeb_Achievement,
                        march_Achievement: xmarch_Achievement,
                        apr_Achievement: xapr_Achievement,
                        may_Achievement: xmay_Achievement,
                        jun_Achievement: xjun_Achievement,
                        jul_Achievement: xjul_Achievement,
                        aug_Achievement: xaug_Achievement,
                        sep_Achievement: xsep_Achievement,
                        oct_Achievement: xoct_Achievement,
                        nov_Achievement: xnov_Achievement,
                        dec_Achievement: xdec_Achievement,
                        jan_ST_Price_effect: xjan_ST_Price_effect,
                        feb_ST_Price_effect: xfeb_ST_Price_effect,
                        march_ST_Price_effect: xmarch_ST_Price_effect,
                        apr_ST_Price_effect: xapr_ST_Price_effect,
                        may_ST_Price_effect: xmay_ST_Price_effect,
                        jun_ST_Price_effect: xjun_ST_Price_effect,
                        jul_ST_Price_effect: xjul_ST_Price_effect,
                        aug_ST_Price_effect: xaug_ST_Price_effect,
                        sep_ST_Price_effect: xsep_ST_Price_effect,
                        oct_ST_Price_effect: xoct_ST_Price_effect,
                        nov_ST_Price_effect: xnov_ST_Price_effect,
                        dec_ST_Price_effect: xdec_ST_Price_effect,
                        jan_ST_Volume_Effect: xjan_ST_Volume_Effect,
                        feb_ST_Volume_Effect: xfeb_ST_Volume_Effect,
                        march_ST_Volume_Effect: xmarch_ST_Volume_Effect,
                        apr_ST_Volume_Effect: xapr_ST_Volume_Effect,
                        may_ST_Volume_Effect: xmay_ST_Volume_Effect,
                        jun_ST_Volume_Effect: xjun_ST_Volume_Effect,
                        jul_ST_Volume_Effect: xjul_ST_Volume_Effect,
                        aug_ST_Volume_Effect: xaug_ST_Volume_Effect,
                        sep_ST_Volume_Effect: xsep_ST_Volume_Effect,
                        oct_ST_Volume_Effect: xoct_ST_Volume_Effect,
                        nov_ST_Volume_Effect: xnov_ST_Volume_Effect,
                        dec_ST_Volume_Effect: xdec_ST_Volume_Effect,
                        jan_FY_Secured_Target: xjan_FY_Secured_Target,
                        feb_FY_Secured_Target: xfeb_FY_Secured_Target,
                        march_FY_Secured_Target: xmarch_FY_Secured_Target,
                        apr_FY_Secured_Target: xapr_FY_Secured_Target,
                        may_FY_Secured_Target: xmay_FY_Secured_Target,
                        jun_FY_Secured_Target: xjun_FY_Secured_Target,
                        jul_FY_Secured_Target: xjul_FY_Secured_Target,
                        aug_FY_Secured_Target: xaug_FY_Secured_Target,
                        sep_FY_Secured_Target: xsep_FY_Secured_Target,
                        oct_FY_Secured_Target: xoct_FY_Secured_Target,
                        nov_FY_Secured_Target: xnov_FY_Secured_Target,
                        dec_FY_Secured_Target: xdec_FY_Secured_Target,
                        jan_CPI_Effect: xjan_CPI_Effect,
                        feb_CPI_Effect: xfeb_CPI_Effect,
                        march_CPI_Effect: xmarch_CPI_Effect,
                        apr_CPI_Effect: xapr_CPI_Effect,
                        may_CPI_Effect: xmay_CPI_Effect,
                        jun_CPI_Effect: xjun_CPI_Effect,
                        jul_CPI_Effect: xjul_CPI_Effect,
                        aug_CPI_Effect: xaug_CPI_Effect,
                        sep_CPI_Effect: xsep_CPI_Effect,
                        oct_CPI_Effect: xoct_CPI_Effect,
                        nov_CPI_Effect: xnov_CPI_Effect,
                        dec_CPI_Effect: xdec_CPI_Effect,
                        jan_CPI: xjan_CPI,
                        feb_CPI: xfeb_CPI,
                        mar_CPI: xmar_CPI,
                        apr_CPI: xapr_CPI,
                        may_CPI: xmay_CPI,
                        jun_CPI: xjun_CPI,
                        jul_CPI: xjul_CPI,
                        aug_CPI: xaug_CPI,
                        sep_CPI: xsep_CPI,
                        oct_CPI: xoct_CPI,
                        nov_CPI: xnov_CPI,
                        dec_CPI: xdec_CPI

                    }
                }, function (data) {
                    // debugger;
                    if (data.substring(0, 5) == "saved") {
                        if (xFormStatus == "New") $("#initsukses").html("New Initiative Number: " + data.substring(6, data.length)); else $("#initsukses").html("Initiative successfully saved!");
                        $("#btnEdit").click(); $("#btnClose").click();
                        WindowInitiative.Hide();
                        WindowOkSaved.Show();
                    }
                });
                // } else {
                //Swal.fire(
                //    'Value is not match',
                //    'Target year (USD) is not match with all applicable monthly target',
                //    'error'
                //);
                // }
            }
            else {
                Swal.fire(
                    'Mandatory field cannot blank',
                    'You must enter all mandatory field which marked as <font color="red">red asterisk</font>',
                    'error'
                );
            }
        }
    });
}

function CheckUncheckCalculate() {
    // debugger;
    var targetstartselected = false; var targetstartselected2 = true; var startmon;/* var projectYear = '@profileData.ProjectYear';*/ var uType = user_type;
    //Count how many targets are enabled and just divide by total
    if (($('#chkAuto').is(':checked')) && txTargetFullYear.GetValue() != "") {
        if ((new Date(StartMonth.GetValue()).getFullYear()) == projectYear) {
            const targetty = new Array("targetjan", "targetfeb", "targetmar", "targetapr", "targetmay", "targetjun", "targetjul", "targetaug", "targetsep", "targetoct", "targetnov", "targetdec", "targetjan2", "targetfeb2", "targetmar2", "targetapr2", "targetmay2", "targetjun2", "targetjul2", "targetaug2", "targetsep2", "targetoct2", "targetnov2", "targetdec2");
            startmon = ((moment(StartMonth.GetValue()).format("M")) - 1);
            var months; var x = 0;
            months = (EndMonth.GetValue().getFullYear() - StartMonth.GetValue().getFullYear()) * 12;
            months -= StartMonth.GetValue().getMonth();
            months += EndMonth.GetValue().getMonth();
            months++; // ngitung selisih bulan mulai dan bulan akhir initiative

            var txTarget12Value; var nilai; var nilai2; var arrnilai = []; var startmon2 = startmon;
            txTarget12Value = Number(txTarget12.GetValue());
            nilai = Number(txTarget12.GetValue() / months).toFixed(2);

            for (var a = 0; a <= months; a++) {
                arrnilai[a] = nilai;
            }
            if ((nilai * months) < Number(txTarget12.GetValue())) {
                var sisa = Number(txTarget12.GetValue()) - (nilai * months);
                arrnilai[months - 1] = (Number(arrnilai[months - 1].replaceAll(",", "")) + Number(sisa)).toFixed(2);
            }

            for (var i = 0; i < targetty.length; i++) {
                if (i >= startmon && x < months) {
                    targetstartselected = true;
                } else {
                    targetstartselected = false;
                }

                if (targetstartselected == true) {
                    $("." + targetty[i]).val(formatValue(arrnilai[x]));
                    $("." + targetty[i]).prop('disabled', false);
                    x++;
                } else {
                    $("." + targetty[i]).val("");
                    $("." + targetty[i]).prop('disabled', true);
                }
            }
        } else if ((new Date(StartMonth.GetValue()).getFullYear()) < projectYear) {
            const targetty = Array("targetjan", "targetfeb", "targetmar", "targetapr", "targetmay", "targetjun", "targetjul", "targetaug", "targetsep", "targetoct", "targetnov", "targetdec", "targetjan2", "targetfeb2", "targetmar2", "targetapr2", "targetmay2", "targetjun2", "targetjul2", "targetaug2", "targetsep2", "targetoct2", "targetnov2", "targetdec2");
            const savingty = Array("savingjan", "savingfeb", "savingmar", "savingapr", "savingmay", "savingjun", "savingjul", "savingaug", "savingsep", "savingoct", "savingnov", "savingdec", "savingjan2", "savingfeb2", "savingmar2", "savingapr2", "savingmay2", "savingjun2", "savingjul2", "savingaug2", "savingsep2", "savingoct2", "savingnov2", "savingdec2");
            //find the sum of previous year 
            // debugger;

            let previoussum = 0;
            for (var i = 0; i <= 11; i++) {

                //previoussum = previoussum + Number($("." + targetty[i]).val());
                // $("." + targetty[o] + tex).val().replaceAll(",", "");
                let currentvalue = $("." + targetty[i]).val().replaceAll(",", "");
                if (currentvalue != "") {
                    if (targetty[i].length > 0) {
                        previoussum = previoussum + parseFloat(currentvalue);
                    }
                }
            }
            previoussum = previoussum.toFixed(2);
            let balance = txTarget12.GetValue() - previoussum;
            var endmonth = ((moment(EndMonth.GetValue()).format("M")));
            var eachmonth = (balance / endmonth).toFixed(2);
            for (var i = 0; i <= endmonth - 1; i++) {
                $("." + targetty[i + 12]).val(formatValue(parseFloat(eachmonth)));
            }
            ////Commented for simplicity 
            //months = (EndMonth.GetValue().getFullYear() - StartMonth.GetValue().getFullYear()) * 12;
            //months -= StartMonth.GetValue().getMonth();
            //months += EndMonth.GetValue().getMonth();
            //months++; // ngitung selisih bulan mulai dan bulan akhir initiative

            ////var months = ((moment(EndMonth.GetValue()).format("M")));
            //var nilai = Number(txTarget12.GetValue() / months).toFixed(2);
            ////var endmonth = ((moment(EndMonth.GetValue()).format("M")));

            //if ((nilai * months) < Number(txTarget12.GetValue())) {
            //    var sisa = Number(txTarget12.GetValue()) - ((+nilai) * (+months));
            //    sisa = (Number(sisa)).toFixed(2);
            //} else {
            //    sisa = 0;
            //}

            //for (var i = 0; i <= targetty.length; i++) {
            //    if (i < endmonth) {
            //        if (i == (endmonth - 1)) {
            //            $("." + targetty[i]).val(formatValue((parseFloat(nilai) + parseFloat(sisa))));
            //        } else {
            //            $("." + targetty[i]).val(formatValue(nilai));
            //        }

            //        $("." + targetty[i]).prop('disabled', false);
            //        $("." + savingty[i]).prop('disabled', false);
            //    } else {
            //        $("." + targetty[i]).val('');
            //        $("." + savingty[i]).val('');
            //        $("." + targetty[i]).prop('disabled', true);
            //        $("." + savingty[i]).prop('disabled', true);
            //    }
            //    // debugger;
            //    if ($("." + targetty[i]).prop('disabled'))
            //        {
            //        console.log("Hi");
            //    }
            //}

            ////End commented for simplicity

            $("input[name='StartMonth']").prop('disabled', true);
            $("input[name='StartMonth']").prop('readonly', true);
            StartMonth.clientEnabled = false;
        } else if ((new Date(StartMonth.GetValue()).getFullYear()) > projectYear) {
            const targetty = new Array("targetjan", "targetfeb", "targetmar", "targetapr", "targetmay", "targetjun", "targetjul", "targetaug", "targetsep", "targetoct", "targetnov", "targetdec", "targetjan2", "targetfeb2", "targetmar2", "targetapr2", "targetmay2", "targetjun2", "targetjul2", "targetaug2", "targetsep2", "targetoct2", "targetnov2", "targetdec2");
            startmon = ((moment(StartMonth.GetValue()).format("M")) - 1);
            var months; var x = 0;
            months = (EndMonth.GetValue().getFullYear() - StartMonth.GetValue().getFullYear()) * 12;
            months -= StartMonth.GetValue().getMonth();
            months += EndMonth.GetValue().getMonth();
            months++; // ngitung selisih bulan mulai dan bulan akhir initiative

            var txTarget12Value; var nilai; var nilai2; var arrnilai = []; var startmon2 = startmon;
            nilai = Number(txTarget12.GetValue() / months).toFixed(2);

            for (var a = 0; a <= months; a++) {
                arrnilai[a] = nilai;
            }
            if ((nilai * months) < Number(txTarget12.GetValue())) {
                var sisa = Number(txTarget12.GetValue()) - (nilai * months);
                arrnilai[months - 1] = (Number(arrnilai[months - 1].replaceAll(",", "")) + Number(sisa)).toFixed(2);
            }

            for (var i = 0; i < targetty.length; i++) {
                if (i >= (+startmon + 12) && x < months) {
                    targetstartselected = true;
                } else {
                    targetstartselected = false;
                }

                if (targetstartselected == true) {
                    $("." + targetty[i]).val(formatValue(arrnilai[x]));
                    $("." + targetty[i]).prop('disabled', false);
                    x++;
                } else {
                    $("." + targetty[i]).val("");
                    $("." + targetty[i]).prop('disabled', true);
                }
            }
        }
    } else {
        const targetty = Array("targetjan", "targetfeb", "targetmar", "targetapr", "targetmay", "targetjun", "targetjul", "targetaug", "targetsep", "targetoct", "targetnov", "targetdec", "targetjan2", "targetfeb2", "targetmar2", "targetapr2", "targetmay2", "targetjun2", "targetjul2", "targetaug2", "targetsep2", "targetoct2", "targetnov2", "targetdec2");
        for (var i = 0; i < targetty.length; i++) {
            if ($("." + targetty[i]).prop('disabled')) {

            }
            else {
                $("." + targetty[i]).val("");
            }
        }
        txTargetFullYear.SetValue("");
    }
    getYtdValue();
    hitungtahunini();
}

function hitungtahunini() {
    var startyear = new Date(StartMonth.GetValue()).getFullYear();
    var d = new Date();
    let currentyear = d.getFullYear();
    let tex = "";

    if (projectYear == startyear) {

        m = 12
    }
    else {
        tex = "2";
    }
    if (startyear === currentyear) {

        tex = "";
    }
    else {

        if (projectYear == startyear) {

            tex = "";
        }
        else {
            tex = "2";
        }
        //tex = "2";
    }
    const targetty = new Array("targetjan" + tex, "targetfeb" + tex, "targetmar" + tex, "targetapr" + tex, "targetmay" + tex, "targetjun" + tex, "targetjul" + tex, "targetaug" + tex, "targetsep" + tex, "targetoct" + tex, "targetnov" + tex, "targetdec" + tex);
    var d = new Date();
    var m = d.getMonth(); var nilai = 0; var hitung = 0; var saving = 0; var hitungsaving = 0;
    for (var o = 0; o < targetty.length; o++) {
        nilai = $("." + targetty[o]).val().replaceAll(",", "");
        if (nilai != "") {
            nilai = parseFloat(nilai).toFixed(2); hitung = parseFloat(hitung).toFixed(2);
            hitung = (parseFloat(nilai) + parseFloat(hitung));
        }
    }
    txTargetFullYear.SetValue(hitung);

    //var sum = 0;
    //$(".txTarget").each(function () {
    //    sum += +$(this).val();
    //});

    //if ((parseFloat(txTargetFullYear.GetValue()) >= parseFloat(txTarget12.GetValue())) || (sum != parseFloat(txTarget12.GetValue()))) {
    //    Swal.fire(
    //        'Target 12 Months Less Than Total Target Field',
    //        'Target This Year Cannot Greater Than Target 12 Months or Target 12 Months Less Than Total Target Field',
    //        'error'
    //    );
    //    for (var o = 0; o < targetty.length; o++) {
    //        $("." + targetty[o]).val("");
    //    }
    //}
}

function getYtdValue() {
    const targetty = new Array("targetjan", "targetfeb", "targetmar", "targetapr", "targetmay", "targetjun", "targetjul", "targetaug", "targetsep", "targetoct", "targetnov", "targetdec")//, "targetjan2", "targetfeb2", "targetmar2", "targetapr2", "targetmay2", "targetjun2", "targetjul2", "targetaug2", "targetsep2", "targetoct2", "targetnov2", "targetdec2");
    const savingty = new Array("savingjan", "savingfeb", "savingmar", "savingapr", "savingmay", "savingjun", "savingjul", "savingaug", "savingsep", "savingoct", "savingnov", "savingdec")//, "savingjan2", "savingfeb2", "savingmar2", "savingapr2", "savingmay2", "savingjun2", "savingjul2", "savingaug2", "savingsep2", "savingoct2", "savingnov2", "savingdec2");
    //var d = new Date();
    var d = new Date();
    //var m = d.getMonth();
    //var m = projectMonth - 1;
    var m = projectMonth;
    // debugger;
    //var startmon = ((moment(StartMonth.GetValue()).format("M")));
    var endmon = ((moment(StartMonth.GetValue()).format("M")));
    var startyear = new Date(StartMonth.GetValue()).getFullYear()
    let offset = -1;
    let currentyear = d.getFullYear();
    var tex = "";
    //if ((new Date(StartMonth.GetValue()).getFullYear()) == projectYear) {
    //    m = startmon;
    //}

    if (currentyear == startyear) {
        // m = startmon;
        offset = 0;
    }
    else {

        if (projectYear == startyear) {

            //m = 12
            //m = projectMonth - 1;
            m = projectMonth;
        }
        else {
            tex = "2";
        }
        //m = startmon + 12;
        offset = 0;
        //comment this 
        // m = endmon;

    }
    /* if (offset > 0) {*/
    var nilai = 0; var hitung = 0; var saving = 0; var hitungsaving = 0;
    for (var o = offset; o < targetty.length; o++) {
        if (o < m) {
            nilai = $("." + targetty[o] + tex).val().replaceAll(",", "");
            if (nilai != "") {
                nilai = parseFloat(nilai); hitung = parseFloat(hitung);
                hitung = (nilai + hitung);
            }

            saving = $("." + savingty[o] + tex).val().replaceAll(",", "");
            if (saving != "") {
                saving = parseFloat(saving); hitungsaving = parseFloat(hitungsaving);
                hitungsaving = (saving + hitungsaving);
            }
        }
    }
    txYTDTargetFullYear.SetValue(hitung);
    txYTDSavingFullYear.SetValue(hitungsaving);


}







//ENH153-2 calculations


function calculate_Procurement_Field() {
    Input_Actuals_Volumes_Nmin1_keyUp();
    Input_Target_Volumes_keyUp();
    txt_Spend_Nmin1_KeyUp();
    calculate_txt_Total_Actual_volume_N();
}

function Input_Actuals_Volumes_Nmin1_keyUp() {
    var xInput_Actuals_Volumes_Nmin1 = txt_Input_Actuals_Volumes_Nmin1.GetValue();
    if (xInput_Actuals_Volumes_Nmin1 != null) {
        if (xInput_Actuals_Volumes_Nmin1 != '0' && xInput_Actuals_Volumes_Nmin1 != '0.') {
            var int_xInput_Actuals_Volumes_Nmin1 = parseInt(xInput_Actuals_Volumes_Nmin1);
            var Actual_volume_Nmin1 = (int_xInput_Actuals_Volumes_Nmin1 / 12);

            Actual_volume_Nmin1 = Actual_volume_Nmin1.toFixed(_toFixed);

            $('#Actual_volume_Nmin1_Jan').val(Actual_volume_Nmin1);
            $('#Actual_volume_Nmin1_Feb').val(Actual_volume_Nmin1);
            $('#Actual_volume_Nmin1_Mar').val(Actual_volume_Nmin1);
            $('#Actual_volume_Nmin1_Apr').val(Actual_volume_Nmin1);
            $('#Actual_volume_Nmin1_May').val(Actual_volume_Nmin1);
            $('#Actual_volume_Nmin1_Jun').val(Actual_volume_Nmin1);
            $('#Actual_volume_Nmin1_Jul').val(Actual_volume_Nmin1);
            $('#Actual_volume_Nmin1_Aug').val(Actual_volume_Nmin1);
            $('#Actual_volume_Nmin1_Sep').val(Actual_volume_Nmin1);
            $('#Actual_volume_Nmin1_Oct').val(Actual_volume_Nmin1);
            $('#Actual_volume_Nmin1_Nov').val(Actual_volume_Nmin1);
            $('#Actual_volume_Nmin1_Dec').val(Actual_volume_Nmin1);

            //ENH153-2 calculations caller
            calculate_Actual_CPU_Nmin1();
            calculate_Target_CPU_N();
            calculate_A_Price_effect();
            calculate_A_Volume_Effect();

            calculate_ST_Price_effect();
            calculate_ST_Volume_Effect();
            calculate_CPI_Effect();
            bind_CPI_Fields();
            //ENH153-2 calculations caller
        }
    }

    bind_Actual_volume_Nmin1();
}

function bind_Actual_volume_Nmin1() {
    var _Actual_volume_Nmin1_Jan = $('#Actual_volume_Nmin1_Jan') != null ? $('#Actual_volume_Nmin1_Jan').val() : 0;
    var _Actual_volume_Nmin1_Feb = $('#Actual_volume_Nmin1_Feb') != null ? $('#Actual_volume_Nmin1_Feb').val() : 0;
    var _Actual_volume_Nmin1_Mar = $('#Actual_volume_Nmin1_Mar') != null ? $('#Actual_volume_Nmin1_Mar').val() : 0;
    var _Actual_volume_Nmin1_Apr = $('#Actual_volume_Nmin1_Apr') != null ? $('#Actual_volume_Nmin1_Apr').val() : 0;
    var _Actual_volume_Nmin1_May = $('#Actual_volume_Nmin1_May') != null ? $('#Actual_volume_Nmin1_May').val() : 0;
    var _Actual_volume_Nmin1_Jun = $('#Actual_volume_Nmin1_Jun') != null ? $('#Actual_volume_Nmin1_Jun').val() : 0;
    var _Actual_volume_Nmin1_Jul = $('#Actual_volume_Nmin1_Jul') != null ? $('#Actual_volume_Nmin1_Jul').val() : 0;
    var _Actual_volume_Nmin1_Aug = $('#Actual_volume_Nmin1_Aug') != null ? $('#Actual_volume_Nmin1_Aug').val() : 0;
    var _Actual_volume_Nmin1_Sep = $('#Actual_volume_Nmin1_Sep') != null ? $('#Actual_volume_Nmin1_Sep').val() : 0;
    var _Actual_volume_Nmin1_Oct = $('#Actual_volume_Nmin1_Oct') != null ? $('#Actual_volume_Nmin1_Oct').val() : 0;
    var _Actual_volume_Nmin1_Nov = $('#Actual_volume_Nmin1_Nov') != null ? $('#Actual_volume_Nmin1_Nov').val() : 0;
    var _Actual_volume_Nmin1_Dec = $('#Actual_volume_Nmin1_Dec') != null ? $('#Actual_volume_Nmin1_Dec').val() : 0;

    $('#Actual_volume_Nmin1_Jan_').val(Math.round(parseFloat(_Actual_volume_Nmin1_Jan)).toLocaleString("en-US"));
    $('#Actual_volume_Nmin1_Feb_').val(Math.round(parseFloat(_Actual_volume_Nmin1_Feb)).toLocaleString("en-US"));
    $('#Actual_volume_Nmin1_Mar_').val(Math.round(parseFloat(_Actual_volume_Nmin1_Mar)).toLocaleString("en-US"));
    $('#Actual_volume_Nmin1_Apr_').val(Math.round(parseFloat(_Actual_volume_Nmin1_Apr)).toLocaleString("en-US"));
    $('#Actual_volume_Nmin1_May_').val(Math.round(parseFloat(_Actual_volume_Nmin1_May)).toLocaleString("en-US"));
    $('#Actual_volume_Nmin1_Jun_').val(Math.round(parseFloat(_Actual_volume_Nmin1_Jun)).toLocaleString("en-US"));
    $('#Actual_volume_Nmin1_Jul_').val(Math.round(parseFloat(_Actual_volume_Nmin1_Jul)).toLocaleString("en-US"));
    $('#Actual_volume_Nmin1_Aug_').val(Math.round(parseFloat(_Actual_volume_Nmin1_Aug)).toLocaleString("en-US"));
    $('#Actual_volume_Nmin1_Sep_').val(Math.round(parseFloat(_Actual_volume_Nmin1_Sep)).toLocaleString("en-US"));
    $('#Actual_volume_Nmin1_Oct_').val(Math.round(parseFloat(_Actual_volume_Nmin1_Oct)).toLocaleString("en-US"));
    $('#Actual_volume_Nmin1_Nov_').val(Math.round(parseFloat(_Actual_volume_Nmin1_Nov)).toLocaleString("en-US"));
    $('#Actual_volume_Nmin1_Dec_').val(Math.round(parseFloat(_Actual_volume_Nmin1_Dec)).toLocaleString("en-US"));
}


function Input_Target_Volumes_keyUp() {
    var xInput_Target_Volumes = txt_Input_Target_Volumes.GetValue();
    if (xInput_Target_Volumes != null) {
        if (xInput_Target_Volumes != '0' && xInput_Target_Volumes != '0.') {
            var int_xInput_Target_Volumes = parseInt(xInput_Target_Volumes);
            var Target_Volumes = (int_xInput_Target_Volumes / 12);

            Target_Volumes = parseFloat(Target_Volumes).toFixed(_toFixed);

            $('#Target_Volumes_Jan').val(Target_Volumes);
            $('#Target_Volumes_Feb').val(Target_Volumes);
            $('#Target_Volumes_Mar').val(Target_Volumes);
            $('#Target_Volumes_Apr').val(Target_Volumes);
            $('#Target_Volumes_May').val(Target_Volumes);
            $('#Target_Volumes_Jun').val(Target_Volumes);
            $('#Target_Volumes_Jul').val(Target_Volumes);
            $('#Target_Volumes_Aug').val(Target_Volumes);
            $('#Target_Volumes_Sep').val(Target_Volumes);
            $('#Target_Volumes_Oct').val(Target_Volumes);
            $('#Target_Volumes_Nov').val(Target_Volumes);
            $('#Target_Volumes_Dec').val(Target_Volumes);

            //ENH153-2 calculations caller
            calculate_Actual_CPU_Nmin1();
            calculate_Target_CPU_N();
            calculate_A_Price_effect();
            calculate_A_Volume_Effect();

            calculate_ST_Price_effect();
            calculate_ST_Volume_Effect();

            calculate_CPI_Effect();
            bind_CPI_Fields();
            //ENH153-2 calculations caller
        }
    }

    bind_Target_Volumes();
}

function bind_Target_Volumes() {
    var _Target_Volumes_Jan = $('#Target_Volumes_Jan') != null ? $('#Target_Volumes_Jan').val() : 0;
    var _Target_Volumes_Feb = $('#Target_Volumes_Feb') != null ? $('#Target_Volumes_Feb').val() : 0;
    var _Target_Volumes_Mar = $('#Target_Volumes_Mar') != null ? $('#Target_Volumes_Mar').val() : 0;
    var _Target_Volumes_Apr = $('#Target_Volumes_Apr') != null ? $('#Target_Volumes_Apr').val() : 0;
    var _Target_Volumes_May = $('#Target_Volumes_May') != null ? $('#Target_Volumes_May').val() : 0;
    var _Target_Volumes_Jun = $('#Target_Volumes_Jun') != null ? $('#Target_Volumes_Jun').val() : 0;
    var _Target_Volumes_Jul = $('#Target_Volumes_Jul') != null ? $('#Target_Volumes_Jul').val() : 0;
    var _Target_Volumes_Aug = $('#Target_Volumes_Aug') != null ? $('#Target_Volumes_Aug').val() : 0;
    var _Target_Volumes_Sep = $('#Target_Volumes_Sep') != null ? $('#Target_Volumes_Sep').val() : 0;
    var _Target_Volumes_Oct = $('#Target_Volumes_Oct') != null ? $('#Target_Volumes_Oct').val() : 0;
    var _Target_Volumes_Nov = $('#Target_Volumes_Nov') != null ? $('#Target_Volumes_Nov').val() : 0;
    var _Target_Volumes_Dec = $('#Target_Volumes_Dec') != null ? $('#Target_Volumes_Dec').val() : 0;

    $('#Target_Volumes_Jan_').val(Math.round(parseFloat(_Target_Volumes_Jan)).toLocaleString("en-US"));
    $('#Target_Volumes_Feb_').val(Math.round(parseFloat(_Target_Volumes_Feb)).toLocaleString("en-US"));
    $('#Target_Volumes_Mar_').val(Math.round(parseFloat(_Target_Volumes_Mar)).toLocaleString("en-US"));
    $('#Target_Volumes_Apr_').val(Math.round(parseFloat(_Target_Volumes_Apr)).toLocaleString("en-US"));
    $('#Target_Volumes_May_').val(Math.round(parseFloat(_Target_Volumes_May)).toLocaleString("en-US"));
    $('#Target_Volumes_Jun_').val(Math.round(parseFloat(_Target_Volumes_Jun)).toLocaleString("en-US"));
    $('#Target_Volumes_Jul_').val(Math.round(parseFloat(_Target_Volumes_Jul)).toLocaleString("en-US"));
    $('#Target_Volumes_Aug_').val(Math.round(parseFloat(_Target_Volumes_Aug)).toLocaleString("en-US"));
    $('#Target_Volumes_Sep_').val(Math.round(parseFloat(_Target_Volumes_Sep)).toLocaleString("en-US"));
    $('#Target_Volumes_Oct_').val(Math.round(parseFloat(_Target_Volumes_Oct)).toLocaleString("en-US"));
    $('#Target_Volumes_Nov_').val(Math.round(parseFloat(_Target_Volumes_Nov)).toLocaleString("en-US"));
    $('#Target_Volumes_Dec_').val(Math.round(parseFloat(_Target_Volumes_Dec)).toLocaleString("en-US"));
}

function txt_Spend_Nmin1_KeyUp() {
    var xSpend_Nmin1 = txt_Spend_Nmin1.GetValue();

    if (xSpend_Nmin1 != null) {

        if (xSpend_Nmin1 != '0' && xSpend_Nmin1 != '0.') {
            var float_xSpend_Nmin1 = parseFloat(xSpend_Nmin1);

            //ENH153-2 calculations caller
            calculate_Actual_CPU_Nmin1();
            calculate_Target_CPU_N();
            calculate_A_Price_effect();
            calculate_A_Volume_Effect();

            calculate_ST_Price_effect();
            calculate_ST_Volume_Effect();

            calculate_CPI_Effect();
            bind_CPI_Fields();
            //ENH153-2 calculations caller
        }
    }
}

function Actual_volume_N_keyUp() {
    calculate_Procurement_Field();
}

function calculate_txt_Total_Actual_volume_N() {
    if ((txt_janActual_volume_N != null &&
        txt_febActual_volume_N != null &&
        txt_marActual_volume_N != null &&
        txt_aprActual_volume_N != null &&
        txt_mayActual_volume_N != null &&
        txt_junActual_volume_N != null &&
        txt_julActual_volume_N != null &&
        txt_augActual_volume_N != null &&
        txt_sepActual_volume_N != null &&
        txt_octActual_volume_N != null &&
        txt_novActual_volume_N != null &&
        txt_decActual_volume_N != null)) {

        var xtxt_Total_Actual_volume_N = (parseFloat(txt_janActual_volume_N.GetValue()) +
            parseFloat(txt_febActual_volume_N.GetValue()) +
            parseFloat(txt_marActual_volume_N.GetValue()) +
            parseFloat(txt_aprActual_volume_N.GetValue()) +
            parseFloat(txt_mayActual_volume_N.GetValue()) +
            parseFloat(txt_junActual_volume_N.GetValue()) +
            parseFloat(txt_julActual_volume_N.GetValue()) +
            parseFloat(txt_augActual_volume_N.GetValue()) +
            parseFloat(txt_sepActual_volume_N.GetValue()) +
            parseFloat(txt_octActual_volume_N.GetValue()) +
            parseFloat(txt_novActual_volume_N.GetValue()) +
            parseFloat(txt_decActual_volume_N.GetValue()));

        txt_Total_Actual_volume_N.SetValue(parseFloat(xtxt_Total_Actual_volume_N).toFixed(_toFixed));
    }
}
//POP UP Calcs

function calculate_Actual_CPU_Nmin1() {
    var xInput_Actuals_Volumes_Nmin1 = txt_Input_Actuals_Volumes_Nmin1.GetValue();
    var xSpend_Nmin1 = txt_Spend_Nmin1.GetValue();

    if (xInput_Actuals_Volumes_Nmin1 != null && xSpend_Nmin1 != null) {

        if ((xInput_Actuals_Volumes_Nmin1 != '0' && xInput_Actuals_Volumes_Nmin1 != '0.') && (xSpend_Nmin1 != '0' && xSpend_Nmin1 != '0.')) {
            var float_xInput_Actuals_Volumes_Nmin1 = parseFloat(xInput_Actuals_Volumes_Nmin1);
            var float_xSpend_Nmin1 = parseFloat(xSpend_Nmin1);

            var Actual_CPU_Nmin1 = float_xSpend_Nmin1 / float_xInput_Actuals_Volumes_Nmin1;

            Actual_CPU_Nmin1 = parseFloat(Actual_CPU_Nmin1).toFixed(_toFixed);

            $('#Actual_CPU_Nmin1_Jan').val(Actual_CPU_Nmin1);
            $('#Actual_CPU_Nmin1_Feb').val(Actual_CPU_Nmin1);
            $('#Actual_CPU_Nmin1_Mar').val(Actual_CPU_Nmin1);
            $('#Actual_CPU_Nmin1_Apr').val(Actual_CPU_Nmin1);
            $('#Actual_CPU_Nmin1_May').val(Actual_CPU_Nmin1);
            $('#Actual_CPU_Nmin1_Jun').val(Actual_CPU_Nmin1);
            $('#Actual_CPU_Nmin1_Jul').val(Actual_CPU_Nmin1);
            $('#Actual_CPU_Nmin1_Aug').val(Actual_CPU_Nmin1);
            $('#Actual_CPU_Nmin1_Sep').val(Actual_CPU_Nmin1);
            $('#Actual_CPU_Nmin1_Oct').val(Actual_CPU_Nmin1);
            $('#Actual_CPU_Nmin1_Nov').val(Actual_CPU_Nmin1);
            $('#Actual_CPU_Nmin1_Dec').val(Actual_CPU_Nmin1);

        }

    }

    calculate_Actual_CPU_Nmin1_Total();
}

function calculate_Actual_CPU_Nmin1_Total() {
    if (($('#Actual_CPU_Nmin1_Jan') != null &&
        $('#Actual_CPU_Nmin1_Feb') != null &&
        $('#Actual_CPU_Nmin1_Mar') != null &&
        $('#Actual_CPU_Nmin1_Apr') != null &&
        $('#Actual_CPU_Nmin1_May') != null &&
        $('#Actual_CPU_Nmin1_Jun') != null &&
        $('#Actual_CPU_Nmin1_Jul') != null &&
        $('#Actual_CPU_Nmin1_Aug') != null &&
        $('#Actual_CPU_Nmin1_Sep') != null &&
        $('#Actual_CPU_Nmin1_Oct') != null &&
        $('#Actual_CPU_Nmin1_Nov') != null &&
        $('#Actual_CPU_Nmin1_Dec') != null)) {

        var xActual_CPU_Nmin1_Total = (parseFloat($('#Actual_CPU_Nmin1_Jan').val()) +
            parseFloat($('#Actual_CPU_Nmin1_Feb').val()) +
            parseFloat($('#Actual_CPU_Nmin1_Mar').val()) +
            parseFloat($('#Actual_CPU_Nmin1_Apr').val()) +
            parseFloat($('#Actual_CPU_Nmin1_May').val()) +
            parseFloat($('#Actual_CPU_Nmin1_Jun').val()) +
            parseFloat($('#Actual_CPU_Nmin1_Jul').val()) +
            parseFloat($('#Actual_CPU_Nmin1_Aug').val()) +
            parseFloat($('#Actual_CPU_Nmin1_Sep').val()) +
            parseFloat($('#Actual_CPU_Nmin1_Oct').val()) +
            parseFloat($('#Actual_CPU_Nmin1_Nov').val()) +
            parseFloat($('#Actual_CPU_Nmin1_Dec').val()));

        $('#Actual_CPU_Nmin1_Total').val(parseFloat(xActual_CPU_Nmin1_Total / 12).toFixed(_toFixed));
    }

    bind_Actual_CPU_Nmin1();
}

function bind_Actual_CPU_Nmin1() {
    var _Actual_CPU_Nmin1_Jan = $('#Actual_CPU_Nmin1_Jan') != null ? $('#Actual_CPU_Nmin1_Jan').val() : 0;
    var _Actual_CPU_Nmin1_Feb = $('#Actual_CPU_Nmin1_Feb') != null ? $('#Actual_CPU_Nmin1_Feb').val() : 0;
    var _Actual_CPU_Nmin1_Mar = $('#Actual_CPU_Nmin1_Mar') != null ? $('#Actual_CPU_Nmin1_Mar').val() : 0;
    var _Actual_CPU_Nmin1_Apr = $('#Actual_CPU_Nmin1_Apr') != null ? $('#Actual_CPU_Nmin1_Apr').val() : 0;
    var _Actual_CPU_Nmin1_May = $('#Actual_CPU_Nmin1_May') != null ? $('#Actual_CPU_Nmin1_May').val() : 0;
    var _Actual_CPU_Nmin1_Jun = $('#Actual_CPU_Nmin1_Jun') != null ? $('#Actual_CPU_Nmin1_Jun').val() : 0;
    var _Actual_CPU_Nmin1_Jul = $('#Actual_CPU_Nmin1_Jul') != null ? $('#Actual_CPU_Nmin1_Jul').val() : 0;
    var _Actual_CPU_Nmin1_Aug = $('#Actual_CPU_Nmin1_Aug') != null ? $('#Actual_CPU_Nmin1_Aug').val() : 0;
    var _Actual_CPU_Nmin1_Sep = $('#Actual_CPU_Nmin1_Sep') != null ? $('#Actual_CPU_Nmin1_Sep').val() : 0;
    var _Actual_CPU_Nmin1_Oct = $('#Actual_CPU_Nmin1_Oct') != null ? $('#Actual_CPU_Nmin1_Oct').val() : 0;
    var _Actual_CPU_Nmin1_Nov = $('#Actual_CPU_Nmin1_Nov') != null ? $('#Actual_CPU_Nmin1_Nov').val() : 0;
    var _Actual_CPU_Nmin1_Dec = $('#Actual_CPU_Nmin1_Dec') != null ? $('#Actual_CPU_Nmin1_Dec').val() : 0;
    var _Actual_CPU_Nmin1_Total = $('#Actual_CPU_Nmin1_Total') != null ? $('#Actual_CPU_Nmin1_Total').val() : 0;

    $('#Actual_CPU_Nmin1_Jan_').val(Math.round(parseFloat(_Actual_CPU_Nmin1_Jan)).toLocaleString("en-US"));
    $('#Actual_CPU_Nmin1_Feb_').val(Math.round(parseFloat(_Actual_CPU_Nmin1_Feb)).toLocaleString("en-US"));
    $('#Actual_CPU_Nmin1_Mar_').val(Math.round(parseFloat(_Actual_CPU_Nmin1_Mar)).toLocaleString("en-US"));
    $('#Actual_CPU_Nmin1_Apr_').val(Math.round(parseFloat(_Actual_CPU_Nmin1_Apr)).toLocaleString("en-US"));
    $('#Actual_CPU_Nmin1_May_').val(Math.round(parseFloat(_Actual_CPU_Nmin1_May)).toLocaleString("en-US"));
    $('#Actual_CPU_Nmin1_Jun_').val(Math.round(parseFloat(_Actual_CPU_Nmin1_Jun)).toLocaleString("en-US"));
    $('#Actual_CPU_Nmin1_Jul_').val(Math.round(parseFloat(_Actual_CPU_Nmin1_Jul)).toLocaleString("en-US"));
    $('#Actual_CPU_Nmin1_Aug_').val(Math.round(parseFloat(_Actual_CPU_Nmin1_Aug)).toLocaleString("en-US"));
    $('#Actual_CPU_Nmin1_Sep_').val(Math.round(parseFloat(_Actual_CPU_Nmin1_Sep)).toLocaleString("en-US"));
    $('#Actual_CPU_Nmin1_Oct_').val(Math.round(parseFloat(_Actual_CPU_Nmin1_Oct)).toLocaleString("en-US"));
    $('#Actual_CPU_Nmin1_Nov_').val(Math.round(parseFloat(_Actual_CPU_Nmin1_Nov)).toLocaleString("en-US"));
    $('#Actual_CPU_Nmin1_Dec_').val(Math.round(parseFloat(_Actual_CPU_Nmin1_Dec)).toLocaleString("en-US"));
    $('#Actual_CPU_Nmin1_Total_').val(Math.round(parseFloat(_Actual_CPU_Nmin1_Total)).toLocaleString("en-US"));

}



function isAll_InputGiven_for_Target_CPU_N() {
    var is_flag = false;

    var xSpend_N = parseFloat(txt_Spend_N.GetValue());
    var _year = new Date(StartMonth.GetValue()).getFullYear();


    if (xSpend_N != null) {
        if (xSpend_N != '0.' && xSpend_N != '0' && xSpend_N != '') {
            is_flag = true;
        }
        else {
            is_flag = false;
        }
    }
    else {
        is_flag = false;
    }
    if (is_flag) {
        if (_year < 2023) {
            is_flag = false;
        }
        else {
            is_flag = true;
        }
    }

    return is_flag;
}

function calculate_Target_CPU_N() {

    //if(1st jan of current year<Start month , -- if true -- ActualCPU N-1 jan  , 
    //-- else -- (('$ SPEND N' - ('Actual CPU N - 1' * (MONTH(Start month) - 1) * Target  volume N)) /(13-MONTH(Start month)))/Target  volume N)

    var xSpend_N = parseFloat(txt_Spend_N.GetValue());
    var _year = new Date(StartMonth.GetValue()).getFullYear();

    //selected start Month - Month
    var selected_StartMonth = new Date(StartMonth.GetValue()).getMonth() + 1;

    //selected start Month - Year
    var Start_Month_year = parseInt(new Date(StartMonth.GetValue()).getMonth() + 1 + String(_year));

    var x13_minus_StartMonth = 13 - parseInt(selected_StartMonth);

    if (isAll_InputGiven_for_Target_CPU_N()) {


        //Target_CPU_N_Jan
        var thisMonth_year = parseInt(1 + String(projectYear));
        if (thisMonth_year < Start_Month_year) {
            $('#Target_CPU_N_Jan').val($('#Actual_CPU_Nmin1_Jan').val());
        }
        else {

            var xActual_CPU_Nmin1_Jan = parseFloat($('#Actual_CPU_Nmin1_Jan').val());
            var xTarget_Volumes_Jan = parseFloat($('#Target_Volumes_Jan').val());

            //1st formula '$ SPEND N - 1' - ('Actual CPU N - 1' * (MONTH(Start month) - 1) * Target  volume N
            var formula1 = parseFloat(xSpend_N - (xActual_CPU_Nmin1_Jan * (selected_StartMonth - 1) * xTarget_Volumes_Jan));

            //2nd formula (13-MONTH($L$9)) i.e. (13-MONTH(Start month))
            var formula2 = parseFloat(x13_minus_StartMonth);

            //3rd formula 3 = Formula 1 / Formula 2
            var formula3 = 0;
            if (x13_minus_StartMonth > 0) {
                formula3 = formula1 / formula2;
            }

            //Formula 4 = Formula 3 / Target  volume N
            var formula4 = 0;
            if (xTarget_Volumes_Jan > 0) {
                formula4 = formula3 / xTarget_Volumes_Jan;
            }

            $('#Target_CPU_N_Jan').val(formula4.toFixed(_toFixed));
        }


        formula1 = 0; formula2 = 0; formula3 = 0; formula4 = 0;
        //Target_CPU_N_Feb
        var thisMonth_year = parseInt(2 + String(projectYear));
        if (thisMonth_year < Start_Month_year) {
            $('#Target_CPU_N_Feb').val($('#Actual_CPU_Nmin1_Feb').val());
        }
        else {

            var xActual_CPU_Nmin1_Feb = parseFloat($('#Actual_CPU_Nmin1_Feb').val());
            var xTarget_Volumes_Feb = parseFloat($('#Target_Volumes_Feb').val());

            //1st formula '$ SPEND N - 1' - ('Actual CPU N - 1' * (MONTH(Start month) - 1) * Target  volume N
            var formula1 = parseFloat(xSpend_N - (xActual_CPU_Nmin1_Feb * (selected_StartMonth - 1) * xTarget_Volumes_Feb));

            //2nd formula (13-MONTH($L$9)) i.e. (13-MONTH(Start month))
            var formula2 = parseFloat(x13_minus_StartMonth);

            //3rd formula 3 = Formula 1 / Formula 2
            var formula3 = 0;
            if (x13_minus_StartMonth > 0) {
                formula3 = formula1 / formula2;
            }

            //Formula 4 = Formula 3 / Target  volume N
            var formula4 = 0;
            if (xTarget_Volumes_Feb > 0) {
                formula4 = formula3 / xTarget_Volumes_Feb;
            }

            $('#Target_CPU_N_Feb').val(formula4);
        }

        formula1 = 0; formula2 = 0; formula3 = 0; formula4 = 0;
        //Target_CPU_N_Mar
        var thisMonth_year = parseInt(3 + String(projectYear));
        if (thisMonth_year < Start_Month_year) {
            $('#Target_CPU_N_Mar').val($('#Actual_CPU_Nmin1_Mar').val());
        }
        else {

            var xActual_CPU_Nmin1_Mar = parseFloat($('#Actual_CPU_Nmin1_Mar').val());
            var xTarget_Volumes_Mar = parseFloat($('#Target_Volumes_Mar').val());

            //1st formula '$ SPEND N - 1' - ('Actual CPU N - 1' * (MONTH(Start month) - 1) * Target  volume N
            var formula1 = parseFloat(xSpend_N - (xActual_CPU_Nmin1_Mar * (selected_StartMonth - 1) * xTarget_Volumes_Mar));

            //2nd formula (13-MONTH($L$9)) i.e. (13-MONTH(Start month))
            var formula2 = parseFloat(x13_minus_StartMonth);

            //3rd formula 3 = Formula 1 / Formula 2
            var formula3 = 0;
            if (x13_minus_StartMonth > 0) {
                formula3 = formula1 / formula2;
            }

            //Formula 4 = Formula 3 / Target  volume N
            var formula4 = 0;
            if (xTarget_Volumes_Mar > 0) {
                formula4 = formula3 / xTarget_Volumes_Mar;
            }

            $('#Target_CPU_N_Mar').val(formula4);
        }

        formula1 = 0; formula2 = 0; formula3 = 0; formula4 = 0;
        //Target_CPU_N_Apr
        var thisMonth_year = parseInt(4 + String(projectYear));
        if (thisMonth_year < Start_Month_year) {
            $('#Target_CPU_N_Apr').val($('#Actual_CPU_Nmin1_Apr').val());
        }
        else {

            var xActual_CPU_Nmin1_Apr = parseFloat($('#Actual_CPU_Nmin1_Apr').val());
            var xTarget_Volumes_Apr = parseFloat($('#Target_Volumes_Apr').val());

            //1st formula '$ SPEND N - 1' - ('Actual CPU N - 1' * (MONTH(Start month) - 1) * Target  volume N
            var formula1 = parseFloat(xSpend_N - (xActual_CPU_Nmin1_Apr * (selected_StartMonth - 1) * xTarget_Volumes_Apr));

            //2nd formula (13-MONTH($L$9)) i.e. (13-MONTH(Start month))
            var formula2 = parseFloat(x13_minus_StartMonth);

            //3rd formula 3 = Formula 1 / Formula 2
            var formula3 = 0;
            if (x13_minus_StartMonth > 0) {
                formula3 = formula1 / formula2;
            }

            //Formula 4 = Formula 3 / Target  volume N
            var formula4 = 0;
            if (xTarget_Volumes_Apr > 0) {
                formula4 = formula3 / xTarget_Volumes_Apr;
            }

            $('#Target_CPU_N_Apr').val(formula4);
        }

        formula1 = 0; formula2 = 0; formula3 = 0; formula4 = 0;
        //Target_CPU_N_May
        var thisMonth_year = parseInt(5 + String(projectYear));
        if (thisMonth_year < Start_Month_year) {
            $('#Target_CPU_N_May').val($('#Actual_CPU_Nmin1_May').val());
        }
        else {

            var xActual_CPU_Nmin1_May = parseFloat($('#Actual_CPU_Nmin1_May').val());
            var xTarget_Volumes_May = parseFloat($('#Target_Volumes_May').val());

            //1st formula '$ SPEND N - 1' - ('Actual CPU N - 1' * (MONTH(Start month) - 1) * Target  volume N
            var formula1 = parseFloat(xSpend_N - (xActual_CPU_Nmin1_May * (selected_StartMonth - 1) * xTarget_Volumes_May));

            //2nd formula (13-MONTH($L$9)) i.e. (13-MONTH(Start month))
            var formula2 = parseFloat(x13_minus_StartMonth);

            //3rd formula 3 = Formula 1 / Formula 2
            var formula3 = 0;
            if (x13_minus_StartMonth > 0) {
                formula3 = formula1 / formula2;
            }

            //Formula 4 = Formula 3 / Target  volume N
            var formula4 = 0;
            if (xTarget_Volumes_May > 0) {
                formula4 = formula3 / xTarget_Volumes_May;
            }

            $('#Target_CPU_N_May').val(formula4);
        }

        formula1 = 0; formula2 = 0; formula3 = 0; formula4 = 0;
        //Target_CPU_N_Jun
        var thisMonth_year = parseInt(6 + String(projectYear));
        if (thisMonth_year < Start_Month_year) {
            $('#Target_CPU_N_Jun').val($('#Actual_CPU_Nmin1_Jun').val());
        }
        else {

            var xActual_CPU_Nmin1_Jun = parseFloat($('#Actual_CPU_Nmin1_Jun').val());
            var xTarget_Volumes_Jun = parseFloat($('#Target_Volumes_Jun').val());

            //1st formula '$ SPEND N - 1' - ('Actual CPU N - 1' * (MONTH(Start month) - 1) * Target  volume N
            var formula1 = parseFloat(xSpend_N - (xActual_CPU_Nmin1_Jun * (selected_StartMonth - 1) * xTarget_Volumes_Jun));

            //2nd formula (13-MONTH($L$9)) i.e. (13-MONTH(Start month))
            var formula2 = parseFloat(x13_minus_StartMonth);

            //3rd formula 3 = Formula 1 / Formula 2
            var formula3 = 0;
            if (x13_minus_StartMonth > 0) {
                formula3 = formula1 / formula2;
            }

            //Formula 4 = Formula 3 / Target  volume N
            var formula4 = 0;
            if (xTarget_Volumes_Jun > 0) {
                formula4 = formula3 / xTarget_Volumes_Jun;
            }

            $('#Target_CPU_N_Jun').val(formula4);
        }

        formula1 = 0; formula2 = 0; formula3 = 0; formula4 = 0;
        //Target_CPU_N_Jul
        var thisMonth_year = parseInt(7 + String(projectYear));
        if (thisMonth_year < Start_Month_year) {
            $('#Target_CPU_N_Jul').val($('#Actual_CPU_Nmin1_Jul').val());
        }
        else {

            var xActual_CPU_Nmin1_Jul = parseFloat($('#Actual_CPU_Nmin1_Jul').val());
            var xTarget_Volumes_Jul = parseFloat($('#Target_Volumes_Jul').val());

            //1st formula '$ SPEND N - 1' - ('Actual CPU N - 1' * (MONTH(Start month) - 1) * Target  volume N
            var formula1 = parseFloat(xSpend_N - (xActual_CPU_Nmin1_Jul * (selected_StartMonth - 1) * xTarget_Volumes_Jul));

            //2nd formula (13-MONTH($L$9)) i.e. (13-MONTH(Start month))
            var formula2 = parseFloat(x13_minus_StartMonth);

            //3rd formula 3 = Formula 1 / Formula 2
            var formula3 = 0;
            if (x13_minus_StartMonth > 0) {
                formula3 = formula1 / formula2;
            }

            //Formula 4 = Formula 3 / Target  volume N
            var formula4 = 0;
            if (xTarget_Volumes_Jul > 0) {
                formula4 = formula3 / xTarget_Volumes_Jul;
            }

            $('#Target_CPU_N_Jul').val(formula4);
        }

        formula1 = 0; formula2 = 0; formula3 = 0; formula4 = 0;
        //Target_CPU_N_Aug
        var thisMonth_year = parseInt(8 + String(projectYear));
        if (thisMonth_year < Start_Month_year) {
            $('#Target_CPU_N_Aug').val($('#Actual_CPU_Nmin1_Aug').val());
        }
        else {

            var xActual_CPU_Nmin1_Aug = parseFloat($('#Actual_CPU_Nmin1_Aug').val());
            var xTarget_Volumes_Aug = parseFloat($('#Target_Volumes_Aug').val());

            //1st formula '$ SPEND N - 1' - ('Actual CPU N - 1' * (MONTH(Start month) - 1) * Target  volume N
            var formula1 = parseFloat(xSpend_N - (xActual_CPU_Nmin1_Aug * (selected_StartMonth - 1) * xTarget_Volumes_Aug));

            //2nd formula (13-MONTH($L$9)) i.e. (13-MONTH(Start month))
            var formula2 = parseFloat(x13_minus_StartMonth);

            //3rd formula 3 = Formula 1 / Formula 2
            var formula3 = 0;
            if (x13_minus_StartMonth > 0) {
                formula3 = formula1 / formula2;
            }

            //Formula 4 = Formula 3 / Target  volume N
            var formula4 = 0;
            if (xTarget_Volumes_Aug > 0) {
                formula4 = formula3 / xTarget_Volumes_Aug;
            }

            $('#Target_CPU_N_Aug').val(formula4);
        }

        formula1 = 0; formula2 = 0; formula3 = 0; formula4 = 0;
        //Target_CPU_N_Sep
        var thisMonth_year = parseInt(9 + String(projectYear));
        if (thisMonth_year < Start_Month_year) {
            $('#Target_CPU_N_Sep').val($('#Actual_CPU_Nmin1_Sep').val());
        }
        else {

            var xActual_CPU_Nmin1_Sep = parseFloat($('#Actual_CPU_Nmin1_Sep').val());
            var xTarget_Volumes_Sep = parseFloat($('#Target_Volumes_Sep').val());

            //1st formula '$ SPEND N - 1' - ('Actual CPU N - 1' * (MONTH(Start month) - 1) * Target  volume N
            var formula1 = parseFloat(xSpend_N - (xActual_CPU_Nmin1_Sep * (selected_StartMonth - 1) * xTarget_Volumes_Sep));

            //2nd formula (13-MONTH($L$9)) i.e. (13-MONTH(Start month))
            var formula2 = parseFloat(x13_minus_StartMonth);

            //3rd formula 3 = Formula 1 / Formula 2
            var formula3 = 0;
            if (x13_minus_StartMonth > 0) {
                formula3 = formula1 / formula2;
            }

            //Formula 4 = Formula 3 / Target  volume N
            var formula4 = 0;
            if (xTarget_Volumes_Sep > 0) {
                formula4 = formula3 / xTarget_Volumes_Sep;
            }

            $('#Target_CPU_N_Sep').val(formula4);
        }

        formula1 = 0; formula2 = 0; formula3 = 0; formula4 = 0;
        //Target_CPU_N_Oct
        var thisMonth_year = parseInt(10 + String(projectYear));
        if (thisMonth_year < Start_Month_year) {
            $('#Target_CPU_N_Oct').val($('#Actual_CPU_Nmin1_Oct').val());
        }
        else {

            var xActual_CPU_Nmin1_Oct = parseFloat($('#Actual_CPU_Nmin1_Oct').val());
            var xTarget_Volumes_Oct = parseFloat($('#Target_Volumes_Oct').val());

            //1st formula '$ SPEND N - 1' - ('Actual CPU N - 1' * (MONTH(Start month) - 1) * Target  volume N
            var formula1 = parseFloat(xSpend_N - (xActual_CPU_Nmin1_Oct * (selected_StartMonth - 1) * xTarget_Volumes_Oct));

            //2nd formula (13-MONTH($L$9)) i.e. (13-MONTH(Start month))
            var formula2 = parseFloat(x13_minus_StartMonth);

            //3rd formula 3 = Formula 1 / Formula 2
            var formula3 = 0;
            if (x13_minus_StartMonth > 0) {
                formula3 = formula1 / formula2;
            }

            //Formula 4 = Formula 3 / Target  volume N
            var formula4 = 0;
            if (xTarget_Volumes_Oct > 0) {
                formula4 = formula3 / xTarget_Volumes_Oct;
            }

            $('#Target_CPU_N_Oct').val(formula4);
        }

        formula1 = 0; formula2 = 0; formula3 = 0; formula4 = 0;
        //Target_CPU_N_Nov
        var thisMonth_year = parseInt(11 + String(projectYear));
        if (thisMonth_year < Start_Month_year) {
            $('#Target_CPU_N_Nov').val($('#Actual_CPU_Nmin1_Nov').val());
        }
        else {

            var xActual_CPU_Nmin1_Nov = parseFloat($('#Actual_CPU_Nmin1_Nov').val());
            var xTarget_Volumes_Nov = parseFloat($('#Target_Volumes_Nov').val());

            //1st formula '$ SPEND N - 1' - ('Actual CPU N - 1' * (MONTH(Start month) - 1) * Target  volume N
            var formula1 = parseFloat(xSpend_N - (xActual_CPU_Nmin1_Nov * (selected_StartMonth - 1) * xTarget_Volumes_Nov));

            //2nd formula (13-MONTH($L$9)) i.e. (13-MONTH(Start month))
            var formula2 = parseFloat(x13_minus_StartMonth);

            //3rd formula 3 = Formula 1 / Formula 2
            var formula3 = 0;
            if (x13_minus_StartMonth > 0) {
                formula3 = formula1 / formula2;
            }

            //Formula 4 = Formula 3 / Target  volume N
            var formula4 = 0;
            if (xTarget_Volumes_Nov > 0) {
                formula4 = formula3 / xTarget_Volumes_Nov;
            }

            $('#Target_CPU_N_Nov').val(formula4);
        }

        formula1 = 0; formula2 = 0; formula3 = 0; formula4 = 0;
        //Target_CPU_N_Dec
        var thisMonth_year = parseInt(12 + String(projectYear));
        if (thisMonth_year < Start_Month_year) {
            $('#Target_CPU_N_Dec').val($('#Actual_CPU_Nmin1_Dec').val());
        }
        else {

            var xActual_CPU_Nmin1_Dec = parseFloat($('#Actual_CPU_Nmin1_Dec').val());
            var xTarget_Volumes_Dec = parseFloat($('#Target_Volumes_Dec').val());

            //1st formula '$ SPEND N - 1' - ('Actual CPU N - 1' * (MONTH(Start month) - 1) * Target  volume N
            var formula1 = parseFloat(xSpend_N - (xActual_CPU_Nmin1_Dec * (selected_StartMonth - 1) * xTarget_Volumes_Dec));

            //2nd formula (13-MONTH($L$9)) i.e. (13-MONTH(Start month))
            var formula2 = parseFloat(x13_minus_StartMonth);

            //3rd formula 3 = Formula 1 / Formula 2
            var formula3 = 0;
            if (x13_minus_StartMonth > 0) {
                formula3 = formula1 / formula2;
            }

            //Formula 4 = Formula 3 / Target  volume N
            var formula4 = 0;
            if (xTarget_Volumes_Dec > 0) {
                formula4 = formula3 / xTarget_Volumes_Dec;
            }

            $('#Target_CPU_N_Dec').val(formula4);
        }
    }

    calculate_Target_CPU_N_Total();
}

function calculate_Target_CPU_N_Total() {

    if (($('#Target_CPU_N_Jan') != null &&
        $('#Target_CPU_N_Feb') != null &&
        $('#Target_CPU_N_Mar') != null &&
        $('#Target_CPU_N_Apr') != null &&
        $('#Target_CPU_N_May') != null &&
        $('#Target_CPU_N_Jun') != null &&
        $('#Target_CPU_N_Jul') != null &&
        $('#Target_CPU_N_Aug') != null &&
        $('#Target_CPU_N_Sep') != null &&
        $('#Target_CPU_N_Oct') != null &&
        $('#Target_CPU_N_Nov') != null &&
        $('#Target_CPU_N_Dec') != null)) {
        var xTarget_CPU_N_Total = (parseFloat($('#Target_CPU_N_Jan').val()) +
            parseFloat($('#Target_CPU_N_Feb').val()) +
            parseFloat($('#Target_CPU_N_Mar').val()) +
            parseFloat($('#Target_CPU_N_Apr').val()) +
            parseFloat($('#Target_CPU_N_May').val()) +
            parseFloat($('#Target_CPU_N_Jun').val()) +
            parseFloat($('#Target_CPU_N_Jul').val()) +
            parseFloat($('#Target_CPU_N_Aug').val()) +
            parseFloat($('#Target_CPU_N_Sep').val()) +
            parseFloat($('#Target_CPU_N_Oct').val()) +
            parseFloat($('#Target_CPU_N_Nov').val()) +
            parseFloat($('#Target_CPU_N_Dec').val()));

        $('#Target_CPU_N_Total').val(parseFloat((xTarget_CPU_N_Total) / 12).toFixed(_toFixed));

    }

    bind_Target_CPU_N_Total();
}

function bind_Target_CPU_N_Total() {
    var _Target_CPU_N_Jan = $('#Target_CPU_N_Jan') != null ? $('#Target_CPU_N_Jan').val() : 0;
    var _Target_CPU_N_Feb = $('#Target_CPU_N_Feb') != null ? $('#Target_CPU_N_Feb').val() : 0;
    var _Target_CPU_N_Mar = $('#Target_CPU_N_Mar') != null ? $('#Target_CPU_N_Mar').val() : 0;
    var _Target_CPU_N_Apr = $('#Target_CPU_N_Apr') != null ? $('#Target_CPU_N_Apr').val() : 0;
    var _Target_CPU_N_May = $('#Target_CPU_N_May') != null ? $('#Target_CPU_N_May').val() : 0;
    var _Target_CPU_N_Jun = $('#Target_CPU_N_Jun') != null ? $('#Target_CPU_N_Jun').val() : 0;
    var _Target_CPU_N_Jul = $('#Target_CPU_N_Jul') != null ? $('#Target_CPU_N_Jul').val() : 0;
    var _Target_CPU_N_Aug = $('#Target_CPU_N_Aug') != null ? $('#Target_CPU_N_Aug').val() : 0;
    var _Target_CPU_N_Sep = $('#Target_CPU_N_Sep') != null ? $('#Target_CPU_N_Sep').val() : 0;
    var _Target_CPU_N_Oct = $('#Target_CPU_N_Oct') != null ? $('#Target_CPU_N_Oct').val() : 0;
    var _Target_CPU_N_Nov = $('#Target_CPU_N_Nov') != null ? $('#Target_CPU_N_Nov').val() : 0;
    var _Target_CPU_N_Dec = $('#Target_CPU_N_Dec') != null ? $('#Target_CPU_N_Dec').val() : 0;
    var _Target_CPU_N_Total = $('#Target_CPU_N_Total') != null ? $('#Target_CPU_N_Total').val() : 0;

    $('#Target_CPU_N_Jan_').val(Math.round(parseFloat(_Target_CPU_N_Jan)).toLocaleString("en-US"));
    $('#Target_CPU_N_Feb_').val(Math.round(parseFloat(_Target_CPU_N_Feb)).toLocaleString("en-US"));
    $('#Target_CPU_N_Mar_').val(Math.round(parseFloat(_Target_CPU_N_Mar)).toLocaleString("en-US"));
    $('#Target_CPU_N_Apr_').val(Math.round(parseFloat(_Target_CPU_N_Apr)).toLocaleString("en-US"));
    $('#Target_CPU_N_May_').val(Math.round(parseFloat(_Target_CPU_N_May)).toLocaleString("en-US"));
    $('#Target_CPU_N_Jun_').val(Math.round(parseFloat(_Target_CPU_N_Jun)).toLocaleString("en-US"));
    $('#Target_CPU_N_Jul_').val(Math.round(parseFloat(_Target_CPU_N_Jul)).toLocaleString("en-US"));
    $('#Target_CPU_N_Aug_').val(Math.round(parseFloat(_Target_CPU_N_Aug)).toLocaleString("en-US"));
    $('#Target_CPU_N_Sep_').val(Math.round(parseFloat(_Target_CPU_N_Sep)).toLocaleString("en-US"));
    $('#Target_CPU_N_Oct_').val(Math.round(parseFloat(_Target_CPU_N_Oct)).toLocaleString("en-US"));
    $('#Target_CPU_N_Nov_').val(Math.round(parseFloat(_Target_CPU_N_Nov)).toLocaleString("en-US"));
    $('#Target_CPU_N_Dec_').val(Math.round(parseFloat(_Target_CPU_N_Dec)).toLocaleString("en-US"));
    $('#Target_CPU_N_Total_').val(Math.round(parseFloat(_Target_CPU_N_Total)).toLocaleString("en-US"));
}


//Acheivement START

function isAll_InputGiven_for_A_Price_effect() {
    var isFlag = false;

    var _SYear = new Date(StartMonth.GetValue()).getFullYear();
    var _EYear = new Date(EndMonth.GetValue()).getFullYear();

    if (_SYear < 2023) {
        isFlag = false;
    }
    else {
        isFlag = true;
    }

    if (isFlag) {
        if (_EYear < 2023) {
            isFlag = false;
        }
        else {
            isFlag = true;
        }
    }

    return isFlag
}

function calculate_A_Price_effect() {
    //=+IF(OR(C15<$L$9,C15>$L$10),0,(((C31-C30)*C18)))
    //if(1st jan of current year<Start month OR 1st jan of current year > End Month, -- if true -- 0 ,
    // -- else -- (((Target CPU N -Actual CPU N-1)* Actual volume N ))
    if (isAll_InputGiven_for_A_Price_effect()) {

        var _SYear = new Date(StartMonth.GetValue()).getFullYear();
        //selected start Month - Month
        var selected_StartMonth = new Date(StartMonth.GetValue()).getMonth() + 1;
        //selected start Month & Year
        //var Start_Month_year = parseInt(new Date(StartMonth.GetValue()).getMonth() + 1 + String(_SYear));
        //var Start_Month_year = parseFloat(String(_SYear) +"."+ String(new Date(StartMonth.GetValue()).getMonth() + 1));
        var Start_Month_year = new Date(_SYear,new Date(StartMonth.GetValue()).getMonth(),1);


        var _EYear = new Date(EndMonth.GetValue()).getFullYear();
        //selected End Month - Month
        var selected_EndMonth = new Date(EndMonth.GetValue()).getMonth() + 1;
        //selected Start Month & Year
        //var End_Month_year = parseFloat(String(_EYear) + "." + String(new Date(EndMonth.GetValue()).getMonth() + 1));
        var End_Month_year = new Date(_EYear, new Date(EndMonth.GetValue()).getMonth(), 1);

        //A_Price_effect_Jan
        var monthIndex = 1;
        //var thisMonth_year = parseInt(monthIndex + String(projectYear));
        //var thisMonth_year = parseFloat(String(projectYear) + "." + String(monthIndex));
        var thisMonth_year = new Date(projectYear, monthIndex-1, 1);

        if (thisMonth_year < Start_Month_year || thisMonth_year > End_Month_year) {
            $('#A_Price_effect_Jan').val(0);
        }
        else {
            //(((Target CPU N - Actual CPU N - 1)* Actual volume N ))
            var xjan_Target_CPU_N = parseFloat($('#Target_CPU_N_Jan').val());
            var xjan_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Jan').val());
            var xjanActual_volume_N = parseFloat(txt_janActual_volume_N.GetValue());

            var formula1 = (((xjan_Target_CPU_N - xjan_Actual_CPU_Nmin1) * xjanActual_volume_N));

            $('#A_Price_effect_Jan').val(formula1);
        }

        //A_Price_effect_Feb
        var monthIndex = 2;
        //var thisMonth_year = parseInt(monthIndex + String(projectYear));
        //var thisMonth_year = parseFloat(String(projectYear) + "." + String(monthIndex));
        var thisMonth_year = new Date(projectYear, monthIndex - 1, 1);

        if (thisMonth_year < Start_Month_year || thisMonth_year > End_Month_year) {
            $('#A_Price_effect_Feb').val(0);
        }
        else {
            //(((Target CPU N - Actual CPU N - 1)* Actual volume N ))
            var xfeb_Target_CPU_N = parseFloat($('#Target_CPU_N_Feb').val());
            var xfeb_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Feb').val());
            var xfebActual_volume_N = parseFloat(txt_febActual_volume_N.GetValue());

            var formula2 = (((xfeb_Target_CPU_N - xfeb_Actual_CPU_Nmin1) * xfebActual_volume_N));

            $('#A_Price_effect_Feb').val(formula2);
        }
        //A_Price_effect_Mar
        var monthIndex = 3;
        //var thisMonth_year = parseInt(monthIndex + String(projectYear));
        //var thisMonth_year = parseFloat(String(projectYear) + "." + String(monthIndex));
        var thisMonth_year = new Date(projectYear, monthIndex - 1, 1);

        if (thisMonth_year < Start_Month_year || thisMonth_year > End_Month_year) {
            $('#A_Price_effect_Mar').val(0);
        }
        else {
            //(((Target CPU N - Actual CPU N - 1)* Actual volume N ))
            var xmarch_Target_CPU_N = parseFloat($('#Target_CPU_N_Mar').val());
            var xmarch_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Mar').val());
            var xmarActual_volume_N = parseFloat(txt_marActual_volume_N.GetValue());

            var formula3 = (((xmarch_Target_CPU_N - xmarch_Actual_CPU_Nmin1) * xmarActual_volume_N));

            $('#A_Price_effect_Mar').val(formula3);
        }
        //A_Price_effect_Apr
        var monthIndex = 4;
        //var thisMonth_year = parseInt(monthIndex + String(projectYear));
        //var thisMonth_year = parseFloat(String(projectYear) + "." + String(monthIndex));
        var thisMonth_year = new Date(projectYear, monthIndex - 1, 1);

        if (thisMonth_year < Start_Month_year || thisMonth_year > End_Month_year) {
            $('#A_Price_effect_Apr').val(0);
        }
        else {
            //(((Target CPU N - Actual CPU N - 1)* Actual volume N ))
            var xapr_Target_CPU_N = parseFloat($('#Target_CPU_N_Apr').val());
            var xapr_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Apr').val());
            var xaprActual_volume_N = parseFloat(txt_aprActual_volume_N.GetValue());

            var formula4 = (((xapr_Target_CPU_N - xapr_Actual_CPU_Nmin1) * xaprActual_volume_N));

            $('#A_Price_effect_Apr').val(formula4);
        }
        //A_Price_effect_May
        var monthIndex = 5;
        //var thisMonth_year = parseInt(monthIndex + String(projectYear));
        //var thisMonth_year = parseFloat(String(projectYear) + "." + String(monthIndex));
        var thisMonth_year = new Date(projectYear, monthIndex - 1, 1);

        if (thisMonth_year < Start_Month_year || thisMonth_year > End_Month_year) {
            $('#A_Price_effect_May').val(0);
        }
        else {
            //(((Target CPU N - Actual CPU N - 1)* Actual volume N ))
            var xmay_Target_CPU_N = parseFloat($('#Target_CPU_N_May').val());
            var xmay_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_May').val());
            var xmayActual_volume_N = parseFloat(txt_mayActual_volume_N.GetValue());

            var formula5 = (((xmay_Target_CPU_N - xmay_Actual_CPU_Nmin1) * xmayActual_volume_N));

            $('#A_Price_effect_May').val(formula5);
        }
        //A_Price_effect_Jun
        var monthIndex = 6;
        //var thisMonth_year = parseInt(monthIndex + String(projectYear));
        //var thisMonth_year = parseFloat(String(projectYear) + "." + String(monthIndex));
        var thisMonth_year = new Date(projectYear, monthIndex - 1, 1);

        if (thisMonth_year < Start_Month_year || thisMonth_year > End_Month_year) {
            $('#A_Price_effect_Jun').val(0);
        }
        else {
            //(((Target CPU N - Actual CPU N - 1)* Actual volume N ))
            var xjun_Target_CPU_N = parseFloat($('#Target_CPU_N_Jun').val());
            var xjun_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Jun').val());
            var xjunActual_volume_N = parseFloat(txt_junActual_volume_N.GetValue());

            var formula6 = (((xjun_Target_CPU_N - xjun_Actual_CPU_Nmin1) * xjunActual_volume_N));

            $('#A_Price_effect_Jun').val(formula6);
        }
        //A_Price_effect_Jul
        var monthIndex = 7;
        //var thisMonth_year = parseInt(monthIndex + String(projectYear));
        //var thisMonth_year = parseFloat(String(projectYear) + "." + String(monthIndex));
        var thisMonth_year = new Date(projectYear, monthIndex - 1, 1);

        if (thisMonth_year < Start_Month_year || thisMonth_year > End_Month_year) {
            $('#A_Price_effect_Jul').val(0);
        }
        else {
            //(((Target CPU N - Actual CPU N - 1)* Actual volume N ))
            var xjul_Target_CPU_N = parseFloat($('#Target_CPU_N_Jul').val());
            var xjul_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Jul').val());
            var xjulActual_volume_N = parseFloat(txt_julActual_volume_N.GetValue());

            var formula7 = (((xjul_Target_CPU_N - xjul_Actual_CPU_Nmin1) * xjulActual_volume_N));

            $('#A_Price_effect_Jul').val(formula7);
        }
        //A_Price_effect_Aug
        var monthIndex = 8;
        //var thisMonth_year = parseInt(monthIndex + String(projectYear));
        //var thisMonth_year = parseInt(String(projectYear) + "." + String(monthIndex));
        var thisMonth_year = new Date(projectYear, monthIndex - 1, 1);

        if (thisMonth_year < Start_Month_year || thisMonth_year > End_Month_year) {
            $('#A_Price_effect_Aug').val(0);
        }
        else {
            //(((Target CPU N - Actual CPU N - 1)* Actual volume N ))
            var xaug_Target_CPU_N = parseFloat($('#Target_CPU_N_Aug').val());
            var xaug_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Aug').val());
            var xaugActual_volume_N = parseFloat(txt_augActual_volume_N.GetValue());

            var formula8 = (((xaug_Target_CPU_N - xaug_Actual_CPU_Nmin1) * xaugActual_volume_N));

            $('#A_Price_effect_Aug').val(formula8);
        }
        //A_Price_effect_Sep
        var monthIndex = 9;
        //var thisMonth_year = parseInt(monthIndex + String(projectYear));
        //var thisMonth_year = parseFloat(String(projectYear) + "." + String(monthIndex));
        var thisMonth_year = new Date(projectYear, monthIndex - 1, 1);

        if (thisMonth_year < Start_Month_year || thisMonth_year > End_Month_year) {
            $('#A_Price_effect_Sep').val(0);
        }
        else {
            //(((Target CPU N - Actual CPU N - 1)* Actual volume N ))
            var xsep_Target_CPU_N = parseFloat($('#Target_CPU_N_Sep').val());
            var xsep_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Sep').val());
            var xsepActual_volume_N = parseFloat(txt_sepActual_volume_N.GetValue());

            var formula9 = (((xsep_Target_CPU_N - xsep_Actual_CPU_Nmin1) * xsepActual_volume_N));

            $('#A_Price_effect_Sep').val(formula9);
        }
        //A_Price_effect_Oct
        var monthIndex = 10;
        //var thisMonth_year = parseInt(monthIndex + String(projectYear));
        //var thisMonth_year = parseFloat(String(projectYear) + "." + String(monthIndex));
        var thisMonth_year = new Date(projectYear, monthIndex - 1, 1);

        if (thisMonth_year < Start_Month_year || thisMonth_year > End_Month_year) {
            $('#A_Price_effect_Oct').val(0);
        }
        else {
            //(((Target CPU N - Actual CPU N - 1)* Actual volume N ))
            var xoct_Target_CPU_N = parseFloat($('#Target_CPU_N_Oct').val());
            var xoct_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Oct').val());
            var xoctActual_volume_N = parseFloat(txt_octActual_volume_N.GetValue());

            var formula10 = (((xoct_Target_CPU_N - xoct_Actual_CPU_Nmin1) * xoctActual_volume_N));

            $('#A_Price_effect_Oct').val(formula10);
        }
        //A_Price_effect_Nov
        var monthIndex = 11;
        //var thisMonth_year = parseInt(monthIndex + String(projectYear));
        //var thisMonth_year = parseFloat(String(projectYear) + "." + String(monthIndex));
        var thisMonth_year = new Date(projectYear, monthIndex - 1, 1);

        if (thisMonth_year < Start_Month_year || thisMonth_year > End_Month_year) {
            $('#A_Price_effect_Nov').val(0);
        }
        else {
            //(((Target CPU N - Actual CPU N - 1)* Actual volume N ))
            var xnov_Target_CPU_N = parseFloat($('#Target_CPU_N_Nov').val());
            var xnov_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Nov').val());
            var xnovActual_volume_N = parseFloat(txt_novActual_volume_N.GetValue());

            var formula11 = (((xnov_Target_CPU_N - xnov_Actual_CPU_Nmin1) * xnovActual_volume_N));

            $('#A_Price_effect_Nov').val(formula11);
        }
        //A_Price_effect_Dec
        var monthIndex = 12;
        //var thisMonth_year = parseInt(monthIndex + String(projectYear));
        //var thisMonth_year = parseFloat(String(projectYear) + "." + String(monthIndex));
        var thisMonth_year = new Date(projectYear, monthIndex - 1, 1);
        if (thisMonth_year < Start_Month_year || thisMonth_year > End_Month_year) {
            $('#A_Price_effect_Dec').val(0);
        }
        else {
            //(((Target CPU N - Actual CPU N - 1)* Actual volume N ))
            var xdec_Target_CPU_N = parseFloat($('#Target_CPU_N_Dec').val());
            var xdec_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Dec').val());
            var xdecActual_volume_N = parseFloat(txt_decActual_volume_N.GetValue());

            var formula12 = (((xdec_Target_CPU_N - xdec_Actual_CPU_Nmin1) * xdecActual_volume_N));

            $('#A_Price_effect_Dec').val(formula12);
        }

    }

    calculate_A_Price_effect_Total();
}

function calculate_A_Price_effect_Total() {
    if (($('#A_Price_effect_Jan') != null &&
        $('#A_Price_effect_Feb') != null &&
        $('#A_Price_effect_Mar') != null &&
        $('#A_Price_effect_Apr') != null &&
        $('#A_Price_effect_May') != null &&
        $('#A_Price_effect_Jun') != null &&
        $('#A_Price_effect_Jul') != null &&
        $('#A_Price_effect_Aug') != null &&
        $('#A_Price_effect_Sep') != null &&
        $('#A_Price_effect_Oct') != null &&
        $('#A_Price_effect_Nov') != null &&
        $('#A_Price_effect_Dec') != null)) {
        var xA_Price_effect_Total = (parseFloat($('#A_Price_effect_Jan').val()) +
            parseFloat($('#A_Price_effect_Feb').val()) +
            parseFloat($('#A_Price_effect_Mar').val()) +
            parseFloat($('#A_Price_effect_Apr').val()) +
            parseFloat($('#A_Price_effect_May').val()) +
            parseFloat($('#A_Price_effect_Jun').val()) +
            parseFloat($('#A_Price_effect_Jul').val()) +
            parseFloat($('#A_Price_effect_Aug').val()) +
            parseFloat($('#A_Price_effect_Sep').val()) +
            parseFloat($('#A_Price_effect_Oct').val()) +
            parseFloat($('#A_Price_effect_Nov').val()) +
            parseFloat($('#A_Price_effect_Dec').val()));

        $('#A_Price_effect_Total').val(parseFloat(xA_Price_effect_Total).toFixed(_toFixed));

        calculate_Achievement();
        calculate_txt_YTD_Achieved_PRICE_EF();
    }

    bind_A_Price_effect();
}

function bind_A_Price_effect() {
    var _A_Price_effect_Jan = $('#A_Price_effect_Jan') != null ? $('#A_Price_effect_Jan').val() : 0;
    var _A_Price_effect_Feb = $('#A_Price_effect_Feb') != null ? $('#A_Price_effect_Feb').val() : 0;
    var _A_Price_effect_Mar = $('#A_Price_effect_Mar') != null ? $('#A_Price_effect_Mar').val() : 0;
    var _A_Price_effect_Apr = $('#A_Price_effect_Apr') != null ? $('#A_Price_effect_Apr').val() : 0;
    var _A_Price_effect_May = $('#A_Price_effect_May') != null ? $('#A_Price_effect_May').val() : 0;
    var _A_Price_effect_Jun = $('#A_Price_effect_Jun') != null ? $('#A_Price_effect_Jun').val() : 0;
    var _A_Price_effect_Jul = $('#A_Price_effect_Jul') != null ? $('#A_Price_effect_Jul').val() : 0;
    var _A_Price_effect_Aug = $('#A_Price_effect_Aug') != null ? $('#A_Price_effect_Aug').val() : 0;
    var _A_Price_effect_Sep = $('#A_Price_effect_Sep') != null ? $('#A_Price_effect_Sep').val() : 0;
    var _A_Price_effect_Oct = $('#A_Price_effect_Oct') != null ? $('#A_Price_effect_Oct').val() : 0;
    var _A_Price_effect_Nov = $('#A_Price_effect_Nov') != null ? $('#A_Price_effect_Nov').val() : 0;
    var _A_Price_effect_Dec = $('#A_Price_effect_Dec') != null ? $('#A_Price_effect_Dec').val() : 0;
    var _A_Price_effect_Total = $('#A_Price_effect_Total') != null ? $('#A_Price_effect_Total').val() : 0;

    $('#A_Price_effect_Jan_').val(Math.round(parseFloat(_A_Price_effect_Jan)).toLocaleString("en-US"));
    $('#A_Price_effect_Feb_').val(Math.round(parseFloat(_A_Price_effect_Feb)).toLocaleString("en-US"));
    $('#A_Price_effect_Mar_').val(Math.round(parseFloat(_A_Price_effect_Mar)).toLocaleString("en-US"));
    $('#A_Price_effect_Apr_').val(Math.round(parseFloat(_A_Price_effect_Apr)).toLocaleString("en-US"));
    $('#A_Price_effect_May_').val(Math.round(parseFloat(_A_Price_effect_May)).toLocaleString("en-US"));
    $('#A_Price_effect_Jun_').val(Math.round(parseFloat(_A_Price_effect_Jun)).toLocaleString("en-US"));
    $('#A_Price_effect_Jul_').val(Math.round(parseFloat(_A_Price_effect_Jul)).toLocaleString("en-US"));
    $('#A_Price_effect_Aug_').val(Math.round(parseFloat(_A_Price_effect_Aug)).toLocaleString("en-US"));
    $('#A_Price_effect_Sep_').val(Math.round(parseFloat(_A_Price_effect_Sep)).toLocaleString("en-US"));
    $('#A_Price_effect_Oct_').val(Math.round(parseFloat(_A_Price_effect_Oct)).toLocaleString("en-US"));
    $('#A_Price_effect_Nov_').val(Math.round(parseFloat(_A_Price_effect_Nov)).toLocaleString("en-US"));
    $('#A_Price_effect_Dec_').val(Math.round(parseFloat(_A_Price_effect_Dec)).toLocaleString("en-US"));
    $('#A_Price_effect_Total_').val(Math.round(parseFloat(_A_Price_effect_Total)).toLocaleString("en-US"));
}




function calculate_A_Volume_Effect() {
    //A_Volume_Effect_Jan
    var xjanActual_volume_N = txt_janActual_volume_N.GetValue();
    var xActual_volume_Nmin1_Jan = $('#Actual_volume_Nmin1_Jan').val();
    var xjan_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Jan').val());

    if (xjanActual_volume_N != null && xActual_volume_Nmin1_Jan != null && xjan_Actual_CPU_Nmin1 != null) {
        var formula1 = (xjanActual_volume_N - xActual_volume_Nmin1_Jan) * xjan_Actual_CPU_Nmin1
        $('#A_Volume_Effect_Jan').val(formula1);
    }

    //A_Volume_Effect_Feb
    var xfebActual_volume_N = txt_febActual_volume_N.GetValue();
    var xActual_volume_Nmin1_Feb = $('#Actual_volume_Nmin1_Feb').val();
    var xfeb_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Feb').val());

    if (xfebActual_volume_N != null && xActual_volume_Nmin1_Feb != null && xfeb_Actual_CPU_Nmin1 != null) {
        var formula2 = (xfebActual_volume_N - xActual_volume_Nmin1_Feb) * xfeb_Actual_CPU_Nmin1
        $('#A_Volume_Effect_Feb').val(formula2);
    }

    //A_Volume_Effect_Mar
    var xmarActual_volume_N = txt_marActual_volume_N.GetValue();
    var xActual_volume_Nmin1_Mar = $('#Actual_volume_Nmin1_Mar').val();
    var xmarch_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Mar').val());

    if (xmarActual_volume_N != null && xActual_volume_Nmin1_Mar != null && xmarch_Actual_CPU_Nmin1 != null) {
        var formula3 = (xmarActual_volume_N - xActual_volume_Nmin1_Mar) * xmarch_Actual_CPU_Nmin1
        $('#A_Volume_Effect_Mar').val(formula3);
    }

    //A_Volume_Effect_Apr
    var xaprActual_volume_N = txt_aprActual_volume_N.GetValue();
    var xActual_volume_Nmin1_Apr = $('#Actual_volume_Nmin1_Apr').val();
    var xapr_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Apr').val());

    if (xaprActual_volume_N != null && xActual_volume_Nmin1_Apr != null && xapr_Actual_CPU_Nmin1 != null) {
        var formula4 = (xaprActual_volume_N - xActual_volume_Nmin1_Apr) * xapr_Actual_CPU_Nmin1
        $('#A_Volume_Effect_Apr').val(formula4);
    }
    //A_Volume_Effect_May
    var xmayActual_volume_N = txt_mayActual_volume_N.GetValue();
    var xActual_volume_Nmin1_May = $('#Actual_volume_Nmin1_May').val();
    var xmay_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_May').val());

    if (xmayActual_volume_N != null && xActual_volume_Nmin1_May != null && xmay_Actual_CPU_Nmin1 != null) {
        var formula5 = (xmayActual_volume_N - xActual_volume_Nmin1_May) * xmay_Actual_CPU_Nmin1
        $('#A_Volume_Effect_May').val(formula5);
    }
    //A_Volume_Effect_Jun
    var xjunActual_volume_N = txt_junActual_volume_N.GetValue();
    var xActual_volume_Nmin1_Jun = $('#Actual_volume_Nmin1_Jun').val();
    var xjun_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Jun').val());

    if (xjunActual_volume_N != null && xActual_volume_Nmin1_Jun != null && xjun_Actual_CPU_Nmin1 != null) {
        var formula6 = (xjunActual_volume_N - xActual_volume_Nmin1_Jun) * xjun_Actual_CPU_Nmin1
        $('#A_Volume_Effect_Jun').val(formula6);
    }
    //A_Volume_Effect_Jul
    var xjulActual_volume_N = txt_julActual_volume_N.GetValue();
    var xActual_volume_Nmin1_Jul = $('#Actual_volume_Nmin1_Jul').val();
    var xjul_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Jul').val());

    if (xjulActual_volume_N != null && xActual_volume_Nmin1_Jul != null && xjul_Actual_CPU_Nmin1 != null) {
        var formula7 = (xjulActual_volume_N - xActual_volume_Nmin1_Jul) * xjul_Actual_CPU_Nmin1
        $('#A_Volume_Effect_Jul').val(formula7);
    }
    //A_Volume_Effect_Aug
    var xaugActual_volume_N = txt_augActual_volume_N.GetValue();
    var xActual_volume_Nmin1_Aug = $('#Actual_volume_Nmin1_Aug').val();
    var xaug_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Aug').val());

    if (xaugActual_volume_N != null && xActual_volume_Nmin1_Aug != null && xaug_Actual_CPU_Nmin1 != null) {
        var formula8 = (xaugActual_volume_N - xActual_volume_Nmin1_Aug) * xaug_Actual_CPU_Nmin1
        $('#A_Volume_Effect_Aug').val(formula8);
    }
    //A_Volume_Effect_Sep
    var xsepActual_volume_N = txt_sepActual_volume_N.GetValue();
    var xActual_volume_Nmin1_Sep = $('#Actual_volume_Nmin1_Sep').val();
    var xsep_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Sep').val());

    if (xsepActual_volume_N != null && xActual_volume_Nmin1_Sep != null && xsep_Actual_CPU_Nmin1 != null) {
        var formula9 = (xsepActual_volume_N - xActual_volume_Nmin1_Sep) * xsep_Actual_CPU_Nmin1
        $('#A_Volume_Effect_Sep').val(formula9);
    }
    //A_Volume_Effect_Oct
    var xoctActual_volume_N = txt_octActual_volume_N.GetValue();
    var xActual_volume_Nmin1_Oct = $('#Actual_volume_Nmin1_Oct').val();
    var xoct_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Oct').val());

    if (xoctActual_volume_N != null && xActual_volume_Nmin1_Oct != null && xoct_Actual_CPU_Nmin1 != null) {
        var formula10 = (xoctActual_volume_N - xActual_volume_Nmin1_Oct) * xoct_Actual_CPU_Nmin1
        $('#A_Volume_Effect_Oct').val(formula10);
    }
    //A_Volume_Effect_Nov
    var xnovActual_volume_N = txt_novActual_volume_N.GetValue();
    var xActual_volume_Nmin1_Nov = $('#Actual_volume_Nmin1_Nov').val();
    var xnov_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Nov').val());

    if (xnovActual_volume_N != null && xActual_volume_Nmin1_Nov != null && xnov_Actual_CPU_Nmin1 != null) {
        var formula11 = (xnovActual_volume_N - xActual_volume_Nmin1_Nov) * xnov_Actual_CPU_Nmin1
        $('#A_Volume_Effect_Nov').val(formula11);
    }
    //A_Volume_Effect_Dec
    var xdecActual_volume_N = txt_decActual_volume_N.GetValue();
    var xActual_volume_Nmin1_Dec = $('#Actual_volume_Nmin1_Dec').val();
    var xdec_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Dec').val());

    if (xdecActual_volume_N != null && xActual_volume_Nmin1_Dec != null && xdec_Actual_CPU_Nmin1 != null) {
        var formula12 = (xdecActual_volume_N - xActual_volume_Nmin1_Dec) * xdec_Actual_CPU_Nmin1
        $('#A_Volume_Effect_Dec').val(formula12);
    }


    calculate_A_Volume_Effect_Total();
}

function calculate_A_Volume_Effect_Total() {
    if (($('#A_Volume_Effect_Jan') != null &&
        $('#A_Volume_Effect_Feb') != null &&
        $('#A_Volume_Effect_Mar') != null &&
        $('#A_Volume_Effect_Apr') != null &&
        $('#A_Volume_Effect_May') != null &&
        $('#A_Volume_Effect_Jun') != null &&
        $('#A_Volume_Effect_Jul') != null &&
        $('#A_Volume_Effect_Aug') != null &&
        $('#A_Volume_Effect_Sep') != null &&
        $('#A_Volume_Effect_Oct') != null &&
        $('#A_Volume_Effect_Nov') != null &&
        $('#A_Volume_Effect_Dec') != null)) {
        var xA_Volume_Effect_Total = (parseFloat($('#A_Volume_Effect_Jan').val()) +
            parseFloat($('#A_Volume_Effect_Feb').val()) +
            parseFloat($('#A_Volume_Effect_Mar').val()) +
            parseFloat($('#A_Volume_Effect_Apr').val()) +
            parseFloat($('#A_Volume_Effect_May').val()) +
            parseFloat($('#A_Volume_Effect_Jun').val()) +
            parseFloat($('#A_Volume_Effect_Jul').val()) +
            parseFloat($('#A_Volume_Effect_Aug').val()) +
            parseFloat($('#A_Volume_Effect_Sep').val()) +
            parseFloat($('#A_Volume_Effect_Oct').val()) +
            parseFloat($('#A_Volume_Effect_Nov').val()) +
            parseFloat($('#A_Volume_Effect_Dec').val()));

        $('#A_Volume_Effect_Total').val(parseFloat(xA_Volume_Effect_Total).toFixed(_toFixed));

        calculate_Achievement();
        calculated_txt_YTD_Achieved_VOLUME_EF();
    }

    bind_A_Volume_Effect();
}

function bind_A_Volume_Effect() {
    var _A_Volume_Effect_Jan = $('#A_Volume_Effect_Jan') != null ? $('#A_Volume_Effect_Jan').val() : 0;
    var _A_Volume_Effect_Feb = $('#A_Volume_Effect_Feb') != null ? $('#A_Volume_Effect_Feb').val() : 0;
    var _A_Volume_Effect_Mar = $('#A_Volume_Effect_Mar') != null ? $('#A_Volume_Effect_Mar').val() : 0;
    var _A_Volume_Effect_Apr = $('#A_Volume_Effect_Apr') != null ? $('#A_Volume_Effect_Apr').val() : 0;
    var _A_Volume_Effect_May = $('#A_Volume_Effect_May') != null ? $('#A_Volume_Effect_May').val() : 0;
    var _A_Volume_Effect_Jun = $('#A_Volume_Effect_Jun') != null ? $('#A_Volume_Effect_Jun').val() : 0;
    var _A_Volume_Effect_Jul = $('#A_Volume_Effect_Jul') != null ? $('#A_Volume_Effect_Jul').val() : 0;
    var _A_Volume_Effect_Aug = $('#A_Volume_Effect_Aug') != null ? $('#A_Volume_Effect_Aug').val() : 0;
    var _A_Volume_Effect_Sep = $('#A_Volume_Effect_Sep') != null ? $('#A_Volume_Effect_Sep').val() : 0;
    var _A_Volume_Effect_Oct = $('#A_Volume_Effect_Oct') != null ? $('#A_Volume_Effect_Oct').val() : 0;
    var _A_Volume_Effect_Nov = $('#A_Volume_Effect_Nov') != null ? $('#A_Volume_Effect_Nov').val() : 0;
    var _A_Volume_Effect_Dec = $('#A_Volume_Effect_Dec') != null ? $('#A_Volume_Effect_Dec').val() : 0;
    var _A_Volume_Effect_Total = $('#A_Volume_Effect_Total') != null ? $('#A_Volume_Effect_Total').val() : 0;

    $('#A_Volume_Effect_Jan_').val(Math.round(parseFloat(_A_Volume_Effect_Jan)).toLocaleString("en-US"));
    $('#A_Volume_Effect_Feb_').val(Math.round(parseFloat(_A_Volume_Effect_Feb)).toLocaleString("en-US"));
    $('#A_Volume_Effect_Mar_').val(Math.round(parseFloat(_A_Volume_Effect_Mar)).toLocaleString("en-US"));
    $('#A_Volume_Effect_Apr_').val(Math.round(parseFloat(_A_Volume_Effect_Apr)).toLocaleString("en-US"));
    $('#A_Volume_Effect_May_').val(Math.round(parseFloat(_A_Volume_Effect_May)).toLocaleString("en-US"));
    $('#A_Volume_Effect_Jun_').val(Math.round(parseFloat(_A_Volume_Effect_Jun)).toLocaleString("en-US"));
    $('#A_Volume_Effect_Jul_').val(Math.round(parseFloat(_A_Volume_Effect_Jul)).toLocaleString("en-US"));
    $('#A_Volume_Effect_Aug_').val(Math.round(parseFloat(_A_Volume_Effect_Aug)).toLocaleString("en-US"));
    $('#A_Volume_Effect_Sep_').val(Math.round(parseFloat(_A_Volume_Effect_Sep)).toLocaleString("en-US"));
    $('#A_Volume_Effect_Oct_').val(Math.round(parseFloat(_A_Volume_Effect_Oct)).toLocaleString("en-US"));
    $('#A_Volume_Effect_Nov_').val(Math.round(parseFloat(_A_Volume_Effect_Nov)).toLocaleString("en-US"));
    $('#A_Volume_Effect_Dec_').val(Math.round(parseFloat(_A_Volume_Effect_Dec)).toLocaleString("en-US"));
    $('#A_Volume_Effect_Total_').val(Math.round(parseFloat(_A_Volume_Effect_Total)).toLocaleString("en-US"));

}



function calculate_Achievement() {

    //Achievement_Jan
    xA_Price_effect_Jan = $('#A_Price_effect_Jan').val();
    xA_Volume_Effect_Jan = $('#A_Volume_Effect_Jan').val();
    if (xA_Price_effect_Jan != null && xA_Volume_Effect_Jan != null) {
        $('#Achievement_Jan').val((parseFloat(xA_Price_effect_Jan) + parseFloat(xA_Volume_Effect_Jan)).toFixed(_toFixed));
    }
    //Achievement_Feb
    xA_Price_effect_Feb = $('#A_Price_effect_Feb').val();
    xA_Volume_Effect_Feb = $('#A_Volume_Effect_Feb').val();
    if (xA_Price_effect_Feb != null && xA_Volume_Effect_Feb != null) {
        $('#Achievement_Feb').val((parseFloat(xA_Price_effect_Feb) + parseFloat(xA_Volume_Effect_Feb)).toFixed(_toFixed));
    }
    //Achievement_Mar
    xA_Price_effect_Mar = $('#A_Price_effect_Mar').val();
    xA_Volume_Effect_Mar = $('#A_Volume_Effect_Mar').val();
    if (xA_Price_effect_Mar != null && xA_Volume_Effect_Mar != null) {
        $('#Achievement_Mar').val((parseFloat(xA_Price_effect_Mar) + parseFloat(xA_Volume_Effect_Mar)).toFixed(_toFixed));
    }
    //Achievement_Apr
    xA_Price_effect_Apr = $('#A_Price_effect_Apr').val();
    xA_Volume_Effect_Apr = $('#A_Volume_Effect_Apr').val();
    if (xA_Price_effect_Apr != null && xA_Volume_Effect_Apr != null) {
        $('#Achievement_Apr').val((parseFloat(xA_Price_effect_Apr) + parseFloat(xA_Volume_Effect_Apr)).toFixed(_toFixed));
    }
    //Achievement_May
    xA_Price_effect_May = $('#A_Price_effect_May').val();
    xA_Volume_Effect_May = $('#A_Volume_Effect_May').val();
    if (xA_Price_effect_May != null && xA_Volume_Effect_May != null) {
        $('#Achievement_May').val((parseFloat(xA_Price_effect_May) + parseFloat(xA_Volume_Effect_May)).toFixed(_toFixed));
    }
    //Achievement_Jun
    xA_Price_effect_Jun = $('#A_Price_effect_Jun').val();
    xA_Volume_Effect_Jun = $('#A_Volume_Effect_Jun').val();
    if (xA_Price_effect_Jun != null && xA_Volume_Effect_Jun != null) {
        $('#Achievement_Jun').val((parseFloat(xA_Price_effect_Jun) + parseFloat(xA_Volume_Effect_Jun)).toFixed(_toFixed));
    }
    //Achievement_Jul
    xA_Price_effect_Jul = $('#A_Price_effect_Jul').val();
    xA_Volume_Effect_Jul = $('#A_Volume_Effect_Jul').val();
    if (xA_Price_effect_Jul != null && xA_Volume_Effect_Jul != null) {
        $('#Achievement_Jul').val((parseFloat(xA_Price_effect_Jul) + parseFloat(xA_Volume_Effect_Jul)).toFixed(_toFixed));
    }
    //Achievement_Aug
    xA_Price_effect_Aug = $('#A_Price_effect_Aug').val();
    xA_Volume_Effect_Aug = $('#A_Volume_Effect_Aug').val();
    if (xA_Price_effect_Aug != null && xA_Volume_Effect_Aug != null) {
        $('#Achievement_Aug').val((parseFloat(xA_Price_effect_Aug) + parseFloat(xA_Volume_Effect_Aug)).toFixed(_toFixed));
    }
    //Achievement_Sep
    xA_Price_effect_Sep = $('#A_Price_effect_Sep').val();
    xA_Volume_Effect_Sep = $('#A_Volume_Effect_Sep').val();
    if (xA_Price_effect_Sep != null && xA_Volume_Effect_Sep != null) {
        $('#Achievement_Sep').val((parseFloat(xA_Price_effect_Sep) + parseFloat(xA_Volume_Effect_Sep)).toFixed(_toFixed));
    }
    //Achievement_Oct
    xA_Price_effect_Oct = $('#A_Price_effect_Oct').val();
    xA_Volume_Effect_Oct = $('#A_Volume_Effect_Oct').val();
    if (xA_Price_effect_Oct != null && xA_Volume_Effect_Oct != null) {
        $('#Achievement_Oct').val((parseFloat(xA_Price_effect_Oct) + parseFloat(xA_Volume_Effect_Oct)).toFixed(_toFixed));
    }
    //Achievement_Nov
    xA_Price_effect_Nov = $('#A_Price_effect_Nov').val();
    xA_Volume_Effect_Nov = $('#A_Volume_Effect_Nov').val();
    if (xA_Price_effect_Nov != null && xA_Volume_Effect_Nov != null) {
        $('#Achievement_Nov').val((parseFloat(xA_Price_effect_Nov) + parseFloat(xA_Volume_Effect_Nov)).toFixed(_toFixed));
    }
    //Achievement_Dec
    xA_Price_effect_Dec = $('#A_Price_effect_Dec').val();
    xA_Volume_Effect_Dec = $('#A_Volume_Effect_Dec').val();
    if (xA_Price_effect_Dec != null && xA_Volume_Effect_Dec != null) {
        $('#Achievement_Dec').val((parseFloat(xA_Price_effect_Dec) + parseFloat(xA_Volume_Effect_Dec)).toFixed(_toFixed));
    }

    calculate_Achievement_Total();
}

function calculate_Achievement_Total() {

    if (($('#Achievement_Jan') != null &&
        $('#Achievement_Feb') != null &&
        $('#Achievement_Mar') != null &&
        $('#Achievement_Apr') != null &&
        $('#Achievement_May') != null &&
        $('#Achievement_Jun') != null &&
        $('#Achievement_Jul') != null &&
        $('#Achievement_Aug') != null &&
        $('#Achievement_Sep') != null &&
        $('#Achievement_Oct') != null &&
        $('#Achievement_Nov') != null &&
        $('#Achievement_Dec') != null)) {
        var xAchievement_Total = (parseFloat($('#Achievement_Jan').val()) +
            parseFloat($('#Achievement_Feb').val()) +
            parseFloat($('#Achievement_Mar').val()) +
            parseFloat($('#Achievement_Apr').val()) +
            parseFloat($('#Achievement_May').val()) +
            parseFloat($('#Achievement_Jun').val()) +
            parseFloat($('#Achievement_Jul').val()) +
            parseFloat($('#Achievement_Aug').val()) +
            parseFloat($('#Achievement_Sep').val()) +
            parseFloat($('#Achievement_Oct').val()) +
            parseFloat($('#Achievement_Nov').val()) +
            parseFloat($('#Achievement_Dec').val()));

        $('#Achievement_Total').val(parseFloat(xAchievement_Total).toFixed(_toFixed));
    }

    bind_Achievement();
}

function bind_Achievement() {

    var _Achievement_Jan = $('#Achievement_Jan') != null ? $('#Achievement_Jan').val() : 0;
    var _Achievement_Feb = $('#Achievement_Feb') != null ? $('#Achievement_Feb').val() : 0;
    var _Achievement_Mar = $('#Achievement_Mar') != null ? $('#Achievement_Mar').val() : 0;
    var _Achievement_Apr = $('#Achievement_Apr') != null ? $('#Achievement_Apr').val() : 0;
    var _Achievement_May = $('#Achievement_May') != null ? $('#Achievement_May').val() : 0;
    var _Achievement_Jun = $('#Achievement_Jun') != null ? $('#Achievement_Jun').val() : 0;
    var _Achievement_Jul = $('#Achievement_Jul') != null ? $('#Achievement_Jul').val() : 0;
    var _Achievement_Aug = $('#Achievement_Aug') != null ? $('#Achievement_Aug').val() : 0;
    var _Achievement_Sep = $('#Achievement_Sep') != null ? $('#Achievement_Sep').val() : 0;
    var _Achievement_Oct = $('#Achievement_Oct') != null ? $('#Achievement_Oct').val() : 0;
    var _Achievement_Nov = $('#Achievement_Nov') != null ? $('#Achievement_Nov').val() : 0;
    var _Achievement_Dec = $('#Achievement_Dec') != null ? $('#Achievement_Dec').val() : 0;
    var _Achievement_Total = $('#Achievement_Total') != null ? $('#Achievement_Total').val() : 0;

    $('#Achievement_Jan_').val(Math.round(parseFloat(_Achievement_Jan)).toLocaleString("en-US"));
    $('#Achievement_Feb_').val(Math.round(parseFloat(_Achievement_Feb)).toLocaleString("en-US"));
    $('#Achievement_Mar_').val(Math.round(parseFloat(_Achievement_Mar)).toLocaleString("en-US"));
    $('#Achievement_Apr_').val(Math.round(parseFloat(_Achievement_Apr)).toLocaleString("en-US"));
    $('#Achievement_May_').val(Math.round(parseFloat(_Achievement_May)).toLocaleString("en-US"));
    $('#Achievement_Jun_').val(Math.round(parseFloat(_Achievement_Jun)).toLocaleString("en-US"));
    $('#Achievement_Jul_').val(Math.round(parseFloat(_Achievement_Jul)).toLocaleString("en-US"));
    $('#Achievement_Aug_').val(Math.round(parseFloat(_Achievement_Aug)).toLocaleString("en-US"));
    $('#Achievement_Sep_').val(Math.round(parseFloat(_Achievement_Sep)).toLocaleString("en-US"));
    $('#Achievement_Oct_').val(Math.round(parseFloat(_Achievement_Oct)).toLocaleString("en-US"));
    $('#Achievement_Nov_').val(Math.round(parseFloat(_Achievement_Nov)).toLocaleString("en-US"));
    $('#Achievement_Dec_').val(Math.round(parseFloat(_Achievement_Dec)).toLocaleString("en-US"));
    $('#Achievement_Total_').val(Math.round(parseFloat(_Achievement_Total)).toLocaleString("en-US"));

}

//Acheivement END


//Secured Target START
function calculate_ST_Price_effect() {
    //=+IF( OR(C15<$L$9,C15>$L$10 ),0, +(C31-C30)*C17 )
    if (isAll_InputGiven_for_A_Price_effect()) {

        var _SYear = new Date(StartMonth.GetValue()).getFullYear();
        //selected start Month - Month
        var selected_StartMonth = new Date(StartMonth.GetValue()).getMonth() + 1;
        //selected start Month & Year
        //var Start_Month_year = parseInt(new Date(StartMonth.GetValue()).getMonth() + 1 + String(_SYear));
        //var Start_Month_year = parseInt(String(_SYear) + String(new Date(StartMonth.GetValue()).getMonth() + 1));
        var Start_Month_year = new Date(_SYear, new Date(StartMonth.GetValue()).getMonth(), 1);

        var _EYear = new Date(EndMonth.GetValue()).getFullYear();
        //selected End Month - Month
        var selected_EndMonth = new Date(EndMonth.GetValue()).getMonth() + 1;
        //selected Start Month & Year
        //var End_Month_year = parseInt(String(_EYear) + String(new Date(EndMonth.GetValue()).getMonth() + 1));
        var End_Month_year = new Date(_EYear, new Date(EndMonth.GetValue()).getMonth(), 1);

        //ST_Price_effect_Jan
        var monthIndex = 1;
        //var thisMonth_year = parseInt(monthIndex + String(projectYear));
        //var thisMonth_year = parseInt(String(projectYear) + monthIndex);
        var thisMonth_year = new Date(projectYear, monthIndex - 1, 1);

        if (thisMonth_year < Start_Month_year || thisMonth_year > End_Month_year) {
            $('#ST_Price_effect_Jan').val(0);
        }
        else {
            //(((Target CPU N - Actual CPU N - 1)* Actual volume N ))
            var xjan_Target_CPU_N = parseFloat($('#Target_CPU_N_Jan').val());
            var xjan_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Jan').val());
            var xTarget_Volumes_Jan = $('#Target_Volumes_Jan').val();

            var formula1 = (((xjan_Target_CPU_N - xjan_Actual_CPU_Nmin1) * xTarget_Volumes_Jan));

            $('#ST_Price_effect_Jan').val(formula1);
        }

        //ST_Price_effect_Feb
        var monthIndex = 2;
        //var thisMonth_year = parseInt(monthIndex + String(projectYear));
        //var thisMonth_year = parseInt(String(projectYear) + monthIndex);
        var thisMonth_year = new Date(projectYear, monthIndex - 1, 1);

        if (thisMonth_year < Start_Month_year || thisMonth_year > End_Month_year) {
            $('#ST_Price_effect_Feb').val(0);
        }
        else {
            //(((Target CPU N - Actual CPU N - 1)* Actual volume N ))
            var xfeb_Target_CPU_N = parseFloat($('#Target_CPU_N_Feb').val());
            var xfeb_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Feb').val());
            var xTarget_Volumes_Feb = $('#Target_Volumes_Feb').val();

            var formula2 = (((xfeb_Target_CPU_N - xfeb_Actual_CPU_Nmin1) * xTarget_Volumes_Feb));

            $('#ST_Price_effect_Feb').val(formula2);
        }

        //ST_Price_effect_Mar
        var monthIndex = 3;
        //var thisMonth_year = parseInt(monthIndex + String(projectYear));
        //var thisMonth_year = parseInt(String(projectYear) + monthIndex);
        var thisMonth_year = new Date(projectYear, monthIndex - 1, 1);

        if (thisMonth_year < Start_Month_year || thisMonth_year > End_Month_year) {
            $('#ST_Price_effect_Mar').val(0);
        }
        else {
            //(((Target CPU N - Actual CPU N - 1)* Actual volume N ))
            var xmarch_Target_CPU_N = parseFloat($('#Target_CPU_N_Mar').val());
            var xmarch_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Mar').val());
            var xTarget_Volumes_Mar = $('#Target_Volumes_Mar').val();

            var formula3 = (((xmarch_Target_CPU_N - xmarch_Actual_CPU_Nmin1) * xTarget_Volumes_Mar));

            $('#ST_Price_effect_Mar').val(formula3);
        }

        //ST_Price_effect_Apr
        var monthIndex = 4;
        //var thisMonth_year = parseInt(monthIndex + String(projectYear));
        //var thisMonth_year = parseInt(String(projectYear) + monthIndex);
        var thisMonth_year = new Date(projectYear, monthIndex - 1, 1);

        if (thisMonth_year < Start_Month_year || thisMonth_year > End_Month_year) {
            $('#ST_Price_effect_Apr').val(0);
        }
        else {
            //(((Target CPU N - Actual CPU N - 1)* Actual volume N ))
            var xapr_Target_CPU_N = parseFloat($('#Target_CPU_N_Apr').val());
            var xapr_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Apr').val());
            var xTarget_Volumes_Apr = $('#Target_Volumes_Apr').val();

            var formula4 = (((xapr_Target_CPU_N - xapr_Actual_CPU_Nmin1) * xTarget_Volumes_Apr));

            $('#ST_Price_effect_Apr').val(formula4);
        }

        //ST_Price_effect_May
        var monthIndex = 5;
        //var thisMonth_year = parseInt(monthIndex + String(projectYear));
        //var thisMonth_year = parseInt(String(projectYear) + monthIndex);
        var thisMonth_year = new Date(projectYear, monthIndex - 1, 1);

        if (thisMonth_year < Start_Month_year || thisMonth_year > End_Month_year) {
            $('#ST_Price_effect_May').val(0);
        }
        else {
            //(((Target CPU N - Actual CPU N - 1)* Actual volume N ))
            var xmay_Target_CPU_N = parseFloat($('#Target_CPU_N_May').val());
            var xmay_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_May').val());
            var xTarget_Volumes_May = $('#Target_Volumes_May').val();

            var formula5 = (((xmay_Target_CPU_N - xmay_Actual_CPU_Nmin1) * xTarget_Volumes_May));

            $('#ST_Price_effect_May').val(formula5);
        }

        //ST_Price_effect_Jun
        var monthIndex = 6;
        //var thisMonth_year = parseInt(monthIndex + String(projectYear));
        //var thisMonth_year = parseInt(String(projectYear) + monthIndex);
        var thisMonth_year = new Date(projectYear, monthIndex - 1, 1);

        if (thisMonth_year < Start_Month_year || thisMonth_year > End_Month_year) {
            $('#ST_Price_effect_Jun').val(0);
        }
        else {
            //(((Target CPU N - Actual CPU N - 1)* Actual volume N ))
            var xjun_Target_CPU_N = parseFloat($('#Target_CPU_N_Jun').val());
            var xjun_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Jun').val());
            var xTarget_Volumes_Jun = $('#Target_Volumes_Jun').val();

            var formula6 = (((xjun_Target_CPU_N - xjun_Actual_CPU_Nmin1) * xTarget_Volumes_Jun));

            $('#ST_Price_effect_Jun').val(formula6);
        }

        //ST_Price_effect_Jul
        var monthIndex = 7;
        //var thisMonth_year = parseInt(monthIndex + String(projectYear));
        //var thisMonth_year = parseInt(String(projectYear) + monthIndex);
        var thisMonth_year = new Date(projectYear, monthIndex - 1, 1);

        if (thisMonth_year < Start_Month_year || thisMonth_year > End_Month_year) {
            $('#ST_Price_effect_Jul').val(0);
        }
        else {
            //(((Target CPU N - Actual CPU N - 1)* Actual volume N ))
            var xjul_Target_CPU_N = parseFloat($('#Target_CPU_N_Jul').val());
            var xjul_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Jul').val());
            var xTarget_Volumes_Jul = $('#Target_Volumes_Jul').val();

            var formula7 = (((xjul_Target_CPU_N - xjul_Actual_CPU_Nmin1) * xTarget_Volumes_Jul));

            $('#ST_Price_effect_Jul').val(formula7);
        }

        //ST_Price_effect_Aug
        var monthIndex = 8;
        //var thisMonth_year = parseInt(monthIndex + String(projectYear));
        //var thisMonth_year = parseInt(String(projectYear) + monthIndex);
        var thisMonth_year = new Date(projectYear, monthIndex - 1, 1);

        if (thisMonth_year < Start_Month_year || thisMonth_year > End_Month_year) {
            $('#ST_Price_effect_Aug').val(0);
        }
        else {
            //(((Target CPU N - Actual CPU N - 1)* Actual volume N ))
            var xaug_Target_CPU_N = parseFloat($('#Target_CPU_N_Aug').val());
            var xaug_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Aug').val());
            var xTarget_Volumes_Aug = $('#Target_Volumes_Aug').val();

            var formula8 = (((xaug_Target_CPU_N - xaug_Actual_CPU_Nmin1) * xTarget_Volumes_Aug));

            $('#ST_Price_effect_Aug').val(formula8);
        }

        //ST_Price_effect_Sep
        var monthIndex = 9;
        //var thisMonth_year = parseInt(monthIndex + String(projectYear));
        //var thisMonth_year = parseInt(String(projectYear) + monthIndex);
        var thisMonth_year = new Date(projectYear, monthIndex - 1, 1);

        if (thisMonth_year < Start_Month_year || thisMonth_year > End_Month_year) {
            $('#ST_Price_effect_Sep').val(0);
        }
        else {
            //(((Target CPU N - Actual CPU N - 1)* Actual volume N ))
            var xsep_Target_CPU_N = parseFloat($('#Target_CPU_N_Sep').val());
            var xsep_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Sep').val());
            var xTarget_Volumes_Sep = $('#Target_Volumes_Sep').val();

            var formula9 = (((xsep_Target_CPU_N - xsep_Actual_CPU_Nmin1) * xTarget_Volumes_Sep));

            $('#ST_Price_effect_Sep').val(formula9);
        }

        //ST_Price_effect_Oct
        var monthIndex = 10;
        //var thisMonth_year = parseInt(monthIndex + String(projectYear));
        //var thisMonth_year = parseInt(String(projectYear) + monthIndex);
        var thisMonth_year = new Date(projectYear, monthIndex - 1, 1);

        if (thisMonth_year < Start_Month_year || thisMonth_year > End_Month_year) {
            $('#ST_Price_effect_Oct').val(0);
        }
        else {
            //(((Target CPU N - Actual CPU N - 1)* Actual volume N ))
            var xoct_Target_CPU_N = parseFloat($('#Target_CPU_N_Oct').val());
            var xoct_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Oct').val());
            var xTarget_Volumes_Oct = $('#Target_Volumes_Oct').val();

            var formula10 = (((xoct_Target_CPU_N - xoct_Actual_CPU_Nmin1) * xTarget_Volumes_Oct));

            $('#ST_Price_effect_Oct').val(formula10);
        }

        //ST_Price_effect_Nov
        var monthIndex = 11;
        //var thisMonth_year = parseInt(monthIndex + String(projectYear));
        //var thisMonth_year = parseInt(String(projectYear) + monthIndex);
        var thisMonth_year = new Date(projectYear, monthIndex - 1, 1);

        if (thisMonth_year < Start_Month_year || thisMonth_year > End_Month_year) {
            $('#ST_Price_effect_Nov').val(0);
        }
        else {
            //(((Target CPU N - Actual CPU N - 1)* Actual volume N ))
            var xnov_Target_CPU_N = parseFloat($('#Target_CPU_N_Nov').val());
            var xnov_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Nov').val());
            var xTarget_Volumes_Nov = $('#Target_Volumes_Nov').val();

            var formula11 = (((xnov_Target_CPU_N - xnov_Actual_CPU_Nmin1) * xTarget_Volumes_Nov));

            $('#ST_Price_effect_Nov').val(formula11);
        }

        //ST_Price_effect_Dec
        var monthIndex = 12;
        //var thisMonth_year = parseInt(monthIndex + String(projectYear));
        //var thisMonth_year = parseInt(String(projectYear) + monthIndex);
        var thisMonth_year = new Date(projectYear, monthIndex - 1, 1);

        if (thisMonth_year < Start_Month_year || thisMonth_year > End_Month_year) {
            $('#ST_Price_effect_Dec').val(0);
        }
        else {
            //(((Target CPU N - Actual CPU N - 1)* Actual volume N ))
            var xdec_Target_CPU_N = parseFloat($('#Target_CPU_N_Dec').val());
            var xdec_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Dec').val());
            var xTarget_Volumes_Dec = $('#Target_Volumes_Dec').val();

            var formula12 = (((xdec_Target_CPU_N - xdec_Actual_CPU_Nmin1) * xTarget_Volumes_Dec));

            $('#ST_Price_effect_Dec').val(formula12);
        }

    }

    calculate_ST_Price_effect_Total();
}

function calculate_ST_Price_effect_Total() {
    if (($('#ST_Price_effect_Jan') != null &&
        $('#ST_Price_effect_Feb') != null &&
        $('#ST_Price_effect_Mar') != null &&
        $('#ST_Price_effect_Apr') != null &&
        $('#ST_Price_effect_May') != null &&
        $('#ST_Price_effect_Jun') != null &&
        $('#ST_Price_effect_Jul') != null &&
        $('#ST_Price_effect_Aug') != null &&
        $('#ST_Price_effect_Sep') != null &&
        $('#ST_Price_effect_Oct') != null &&
        $('#ST_Price_effect_Nov') != null &&
        $('#ST_Price_effect_Dec') != null)) {
        var xST_Price_effect_Total = (parseFloat($('#ST_Price_effect_Jan').val()) +
            parseFloat($('#ST_Price_effect_Feb').val()) +
            parseFloat($('#ST_Price_effect_Mar').val()) +
            parseFloat($('#ST_Price_effect_Apr').val()) +
            parseFloat($('#ST_Price_effect_May').val()) +
            parseFloat($('#ST_Price_effect_Jun').val()) +
            parseFloat($('#ST_Price_effect_Jul').val()) +
            parseFloat($('#ST_Price_effect_Aug').val()) +
            parseFloat($('#ST_Price_effect_Sep').val()) +
            parseFloat($('#ST_Price_effect_Oct').val()) +
            parseFloat($('#ST_Price_effect_Nov').val()) +
            parseFloat($('#ST_Price_effect_Dec').val()));

        $('#ST_Price_effect_Total').val(parseFloat(xST_Price_effect_Total).toFixed(_toFixed));

        calculate_txt_N_YTD_Sec_PRICE_EF();
        calculate_FY_Secured_Target();
        calculate_txt_N_FY_Sec_PRICE_EF();
        bind_ST_Price_effect();
    }
}

function bind_ST_Price_effect() {

    var _ST_Price_effect_Jan = $('#ST_Price_effect_Jan') != null ? $('#ST_Price_effect_Jan').val() : 0;
    var _ST_Price_effect_Feb = $('#ST_Price_effect_Feb') != null ? $('#ST_Price_effect_Feb').val() : 0;
    var _ST_Price_effect_Mar = $('#ST_Price_effect_Mar') != null ? $('#ST_Price_effect_Mar').val() : 0;
    var _ST_Price_effect_Apr = $('#ST_Price_effect_Apr') != null ? $('#ST_Price_effect_Apr').val() : 0;
    var _ST_Price_effect_May = $('#ST_Price_effect_May') != null ? $('#ST_Price_effect_May').val() : 0;
    var _ST_Price_effect_Jun = $('#ST_Price_effect_Jun') != null ? $('#ST_Price_effect_Jun').val() : 0;
    var _ST_Price_effect_Jul = $('#ST_Price_effect_Jul') != null ? $('#ST_Price_effect_Jul').val() : 0;
    var _ST_Price_effect_Aug = $('#ST_Price_effect_Aug') != null ? $('#ST_Price_effect_Aug').val() : 0;
    var _ST_Price_effect_Sep = $('#ST_Price_effect_Sep') != null ? $('#ST_Price_effect_Sep').val() : 0;
    var _ST_Price_effect_Oct = $('#ST_Price_effect_Oct') != null ? $('#ST_Price_effect_Oct').val() : 0;
    var _ST_Price_effect_Nov = $('#ST_Price_effect_Nov') != null ? $('#ST_Price_effect_Nov').val() : 0;
    var _ST_Price_effect_Dec = $('#ST_Price_effect_Dec') != null ? $('#ST_Price_effect_Dec').val() : 0;
    var _ST_Price_effect_Total = $('#ST_Price_effect_Total') != null ? $('#ST_Price_effect_Total').val() : 0;

    $('#ST_Price_effect_Jan_').val(Math.round(parseFloat(_ST_Price_effect_Jan)).toLocaleString("en-US"));
    $('#ST_Price_effect_Feb_').val(Math.round(parseFloat(_ST_Price_effect_Feb)).toLocaleString("en-US"));
    $('#ST_Price_effect_Mar_').val(Math.round(parseFloat(_ST_Price_effect_Mar)).toLocaleString("en-US"));
    $('#ST_Price_effect_Apr_').val(Math.round(parseFloat(_ST_Price_effect_Apr)).toLocaleString("en-US"));
    $('#ST_Price_effect_May_').val(Math.round(parseFloat(_ST_Price_effect_May)).toLocaleString("en-US"));
    $('#ST_Price_effect_Jun_').val(Math.round(parseFloat(_ST_Price_effect_Jun)).toLocaleString("en-US"));
    $('#ST_Price_effect_Jul_').val(Math.round(parseFloat(_ST_Price_effect_Jul)).toLocaleString("en-US"));
    $('#ST_Price_effect_Aug_').val(Math.round(parseFloat(_ST_Price_effect_Aug)).toLocaleString("en-US"));
    $('#ST_Price_effect_Sep_').val(Math.round(parseFloat(_ST_Price_effect_Sep)).toLocaleString("en-US"));
    $('#ST_Price_effect_Oct_').val(Math.round(parseFloat(_ST_Price_effect_Oct)).toLocaleString("en-US"));
    $('#ST_Price_effect_Nov_').val(Math.round(parseFloat(_ST_Price_effect_Nov)).toLocaleString("en-US"));
    $('#ST_Price_effect_Dec_').val(Math.round(parseFloat(_ST_Price_effect_Dec)).toLocaleString("en-US"));
    $('#ST_Price_effect_Total_').val(Math.round(parseFloat(_ST_Price_effect_Total)).toLocaleString("en-US"));
}


function calculate_ST_Volume_Effect() {
    //ST_Volume_Effect_Jan
    var xTarget_Volumes_Jan = $('#Target_Volumes_Jan').val();
    var xActual_volume_Nmin1_Jan = $('#Actual_volume_Nmin1_Jan').val();
    var xjan_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Jan').val());

    if (xTarget_Volumes_Jan != null && xActual_volume_Nmin1_Jan != null && xjan_Actual_CPU_Nmin1 != null) {
        var formula1 = (xTarget_Volumes_Jan - xActual_volume_Nmin1_Jan) * xjan_Actual_CPU_Nmin1
        $('#ST_Volume_Effect_Jan').val(formula1);
    }
    //ST_Volume_Effect_Feb
    var xTarget_Volumes_Feb = $('#Target_Volumes_Feb').val();
    var xActual_volume_Nmin1_Feb = $('#Actual_volume_Nmin1_Feb').val();
    var xfeb_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Feb').val());

    if (xTarget_Volumes_Feb != null && xActual_volume_Nmin1_Feb != null && xfeb_Actual_CPU_Nmin1 != null) {
        var formula2 = (xTarget_Volumes_Feb - xActual_volume_Nmin1_Feb) * xfeb_Actual_CPU_Nmin1
        $('#ST_Volume_Effect_Feb').val(formula2);
    }
    //ST_Volume_Effect_Mar
    var xTarget_Volumes_Mar = $('#Target_Volumes_Mar').val();
    var xActual_volume_Nmin1_Mar = $('#Actual_volume_Nmin1_Mar').val();
    var xmarch_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Mar').val());

    if (xTarget_Volumes_Mar != null && xActual_volume_Nmin1_Mar != null && xmarch_Actual_CPU_Nmin1 != null) {
        var formula3 = (xTarget_Volumes_Mar - xActual_volume_Nmin1_Mar) * xmarch_Actual_CPU_Nmin1
        $('#ST_Volume_Effect_Mar').val(formula3);
    }
    //ST_Volume_Effect_Apr
    var xTarget_Volumes_Apr = $('#Target_Volumes_Apr').val();
    var xActual_volume_Nmin1_Apr = $('#Actual_volume_Nmin1_Apr').val();
    var xapr_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Apr').val());

    if (xTarget_Volumes_Apr != null && xActual_volume_Nmin1_Apr != null && xapr_Actual_CPU_Nmin1 != null) {
        var formula4 = (xTarget_Volumes_Apr - xActual_volume_Nmin1_Apr) * xapr_Actual_CPU_Nmin1
        $('#ST_Volume_Effect_Apr').val(formula4);
    }
    //ST_Volume_Effect_May
    var xTarget_Volumes_May = $('#Target_Volumes_May').val();
    var xActual_volume_Nmin1_May = $('#Actual_volume_Nmin1_May').val();
    var xmay_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_May').val());

    if (xTarget_Volumes_May != null && xActual_volume_Nmin1_May != null && xmay_Actual_CPU_Nmin1 != null) {
        var formula5 = (xTarget_Volumes_May - xActual_volume_Nmin1_May) * xmay_Actual_CPU_Nmin1
        $('#ST_Volume_Effect_May').val(formula5);
    }
    //ST_Volume_Effect_Jun
    var xTarget_Volumes_Jun = $('#Target_Volumes_Jun').val();
    var xActual_volume_Nmin1_Jun = $('#Actual_volume_Nmin1_Jun').val();
    var xjun_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Jun').val());

    if (xTarget_Volumes_Jun != null && xActual_volume_Nmin1_Jun != null && xjun_Actual_CPU_Nmin1 != null) {
        var formula6 = (xTarget_Volumes_Jun - xActual_volume_Nmin1_Jun) * xjun_Actual_CPU_Nmin1
        $('#ST_Volume_Effect_Jun').val(formula6);
    }
    //ST_Volume_Effect_Jul
    var xTarget_Volumes_Jul = $('#Target_Volumes_Jul').val();
    var xActual_volume_Nmin1_Jul = $('#Actual_volume_Nmin1_Jul').val();
    var xjul_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Jul').val());

    if (xTarget_Volumes_Jul != null && xActual_volume_Nmin1_Jul != null && xjul_Actual_CPU_Nmin1 != null) {
        var formula7 = (xTarget_Volumes_Jul - xActual_volume_Nmin1_Jul) * xjul_Actual_CPU_Nmin1
        $('#ST_Volume_Effect_Jul').val(formula7);
    }
    //ST_Volume_Effect_Aug
    var xTarget_Volumes_Aug = $('#Target_Volumes_Aug').val();
    var xActual_volume_Nmin1_Aug = $('#Actual_volume_Nmin1_Aug').val();
    var xaug_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Aug').val());

    if (xTarget_Volumes_Aug != null && xActual_volume_Nmin1_Aug != null && xaug_Actual_CPU_Nmin1 != null) {
        var formula8 = (xTarget_Volumes_Aug - xActual_volume_Nmin1_Aug) * xaug_Actual_CPU_Nmin1
        $('#ST_Volume_Effect_Aug').val(formula8);
    }
    //ST_Volume_Effect_Sep
    var xTarget_Volumes_Sep = $('#Target_Volumes_Sep').val();
    var xActual_volume_Nmin1_Sep = $('#Actual_volume_Nmin1_Sep').val();
    var xsep_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Sep').val());

    if (xTarget_Volumes_Sep != null && xActual_volume_Nmin1_Sep != null && xsep_Actual_CPU_Nmin1 != null) {
        var formula9 = (xTarget_Volumes_Sep - xActual_volume_Nmin1_Sep) * xsep_Actual_CPU_Nmin1
        $('#ST_Volume_Effect_Sep').val(formula9);
    }
    //ST_Volume_Effect_Oct
    var xTarget_Volumes_Oct = $('#Target_Volumes_Oct').val();
    var xActual_volume_Nmin1_Oct = $('#Actual_volume_Nmin1_Oct').val();
    var xoct_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Oct').val());

    if (xTarget_Volumes_Oct != null && xActual_volume_Nmin1_Oct != null && xoct_Actual_CPU_Nmin1 != null) {
        var formula10 = (xTarget_Volumes_Oct - xActual_volume_Nmin1_Oct) * xoct_Actual_CPU_Nmin1
        $('#ST_Volume_Effect_Oct').val(formula10);
    }
    //ST_Volume_Effect_Nov
    var xTarget_Volumes_Nov = $('#Target_Volumes_Nov').val();
    var xActual_volume_Nmin1_Nov = $('#Actual_volume_Nmin1_Nov').val();
    var xnov_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Nov').val());

    if (xTarget_Volumes_Nov != null && xActual_volume_Nmin1_Nov != null && xnov_Actual_CPU_Nmin1 != null) {
        var formula11 = (xTarget_Volumes_Nov - xActual_volume_Nmin1_Nov) * xnov_Actual_CPU_Nmin1
        $('#ST_Volume_Effect_Nov').val(formula11);
    }
    //ST_Volume_Effect_Dec
    var xTarget_Volumes_Dec = $('#Target_Volumes_Dec').val();
    var xActual_volume_Nmin1_Dec = $('#Actual_volume_Nmin1_Dec').val();
    var xdec_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Dec').val());

    if (xTarget_Volumes_Dec != null && xActual_volume_Nmin1_Dec != null && xdec_Actual_CPU_Nmin1 != null) {
        var formula12 = (xTarget_Volumes_Dec - xActual_volume_Nmin1_Dec) * xdec_Actual_CPU_Nmin1
        $('#ST_Volume_Effect_Dec').val(formula12);
    }

    calculate_ST_Volume_Effect_Total();
}

function calculate_ST_Volume_Effect_Total() {
    if (($('#ST_Volume_Effect_Jan') != null &&
        $('#ST_Volume_Effect_Feb') != null &&
        $('#ST_Volume_Effect_Mar') != null &&
        $('#ST_Volume_Effect_Apr') != null &&
        $('#ST_Volume_Effect_May') != null &&
        $('#ST_Volume_Effect_Jun') != null &&
        $('#ST_Volume_Effect_Jul') != null &&
        $('#ST_Volume_Effect_Aug') != null &&
        $('#ST_Volume_Effect_Sep') != null &&
        $('#ST_Volume_Effect_Oct') != null &&
        $('#ST_Volume_Effect_Nov') != null &&
        $('#ST_Volume_Effect_Dec') != null)) {
        var xST_Volume_Effect_Total = (parseFloat($('#ST_Volume_Effect_Jan').val()) +
            parseFloat($('#ST_Volume_Effect_Feb').val()) +
            parseFloat($('#ST_Volume_Effect_Mar').val()) +
            parseFloat($('#ST_Volume_Effect_Apr').val()) +
            parseFloat($('#ST_Volume_Effect_May').val()) +
            parseFloat($('#ST_Volume_Effect_Jun').val()) +
            parseFloat($('#ST_Volume_Effect_Jul').val()) +
            parseFloat($('#ST_Volume_Effect_Aug').val()) +
            parseFloat($('#ST_Volume_Effect_Sep').val()) +
            parseFloat($('#ST_Volume_Effect_Oct').val()) +
            parseFloat($('#ST_Volume_Effect_Nov').val()) +
            parseFloat($('#ST_Volume_Effect_Dec').val()));

        $('#ST_Volume_Effect_Total').val(parseFloat(xST_Volume_Effect_Total).toFixed(_toFixed));

        calculate_FY_Secured_Target();
        calculate_txt_N_YTD_Sec_VOLUME_EF();
        calculate_txt_N_FY_Sec_VOLUME_EF();
    }

    bind_ST_Volume_Effect();
}

function bind_ST_Volume_Effect() {
    var _ST_Volume_Effect_Jan = $('#ST_Volume_Effect_Jan') != null ? $('#ST_Volume_Effect_Jan').val() : 0;
    var _ST_Volume_Effect_Feb = $('#ST_Volume_Effect_Feb') != null ? $('#ST_Volume_Effect_Feb').val() : 0;
    var _ST_Volume_Effect_Mar = $('#ST_Volume_Effect_Mar') != null ? $('#ST_Volume_Effect_Mar').val() : 0;
    var _ST_Volume_Effect_Apr = $('#ST_Volume_Effect_Apr') != null ? $('#ST_Volume_Effect_Apr').val() : 0;
    var _ST_Volume_Effect_May = $('#ST_Volume_Effect_May') != null ? $('#ST_Volume_Effect_May').val() : 0;
    var _ST_Volume_Effect_Jun = $('#ST_Volume_Effect_Jun') != null ? $('#ST_Volume_Effect_Jun').val() : 0;
    var _ST_Volume_Effect_Jul = $('#ST_Volume_Effect_Jul') != null ? $('#ST_Volume_Effect_Jul').val() : 0;
    var _ST_Volume_Effect_Aug = $('#ST_Volume_Effect_Aug') != null ? $('#ST_Volume_Effect_Aug').val() : 0;
    var _ST_Volume_Effect_Sep = $('#ST_Volume_Effect_Sep') != null ? $('#ST_Volume_Effect_Sep').val() : 0;
    var _ST_Volume_Effect_Oct = $('#ST_Volume_Effect_Oct') != null ? $('#ST_Volume_Effect_Oct').val() : 0;
    var _ST_Volume_Effect_Nov = $('#ST_Volume_Effect_Nov') != null ? $('#ST_Volume_Effect_Nov').val() : 0;
    var _ST_Volume_Effect_Dec = $('#ST_Volume_Effect_Dec') != null ? $('#ST_Volume_Effect_Dec').val() : 0;
    var _ST_Volume_Effect_Total = $('#ST_Volume_Effect_Total') != null ? $('#ST_Volume_Effect_Total').val() : 0;

    $('#ST_Volume_Effect_Jan_').val(Math.round(parseFloat(_ST_Volume_Effect_Jan)).toLocaleString("en-US"));
    $('#ST_Volume_Effect_Feb_').val(Math.round(parseFloat(_ST_Volume_Effect_Feb)).toLocaleString("en-US"));
    $('#ST_Volume_Effect_Mar_').val(Math.round(parseFloat(_ST_Volume_Effect_Mar)).toLocaleString("en-US"));
    $('#ST_Volume_Effect_Apr_').val(Math.round(parseFloat(_ST_Volume_Effect_Apr)).toLocaleString("en-US"));
    $('#ST_Volume_Effect_May_').val(Math.round(parseFloat(_ST_Volume_Effect_May)).toLocaleString("en-US"));
    $('#ST_Volume_Effect_Jun_').val(Math.round(parseFloat(_ST_Volume_Effect_Jun)).toLocaleString("en-US"));
    $('#ST_Volume_Effect_Jul_').val(Math.round(parseFloat(_ST_Volume_Effect_Jul)).toLocaleString("en-US"));
    $('#ST_Volume_Effect_Aug_').val(Math.round(parseFloat(_ST_Volume_Effect_Aug)).toLocaleString("en-US"));
    $('#ST_Volume_Effect_Sep_').val(Math.round(parseFloat(_ST_Volume_Effect_Sep)).toLocaleString("en-US"));
    $('#ST_Volume_Effect_Oct_').val(Math.round(parseFloat(_ST_Volume_Effect_Oct)).toLocaleString("en-US"));
    $('#ST_Volume_Effect_Nov_').val(Math.round(parseFloat(_ST_Volume_Effect_Nov)).toLocaleString("en-US"));
    $('#ST_Volume_Effect_Dec_').val(Math.round(parseFloat(_ST_Volume_Effect_Dec)).toLocaleString("en-US"));
    $('#ST_Volume_Effect_Total_').val(Math.round(parseFloat(_ST_Volume_Effect_Total)).toLocaleString("en-US"));
}


function calculate_FY_Secured_Target() {

    //FY_Secured_Target_Jan
    xST_Price_effect_Jan = $('#ST_Price_effect_Jan').val();
    xST_Volume_Effect_Jan = $('#ST_Volume_Effect_Jan').val();
    if (xST_Price_effect_Jan != null && xST_Volume_Effect_Jan != null) {
        $('#FY_Secured_Target_Jan').val((parseFloat(xST_Price_effect_Jan) + parseFloat(xST_Volume_Effect_Jan)).toFixed(_toFixed));
    }
    //FY_Secured_Target_Feb
    xST_Price_effect_Feb = $('#ST_Price_effect_Feb').val();
    xST_Volume_Effect_Feb = $('#ST_Volume_Effect_Feb').val();
    if (xST_Price_effect_Feb != null && xST_Volume_Effect_Feb != null) {
        $('#FY_Secured_Target_Feb').val((parseFloat(xST_Price_effect_Feb) + parseFloat(xST_Volume_Effect_Feb)).toFixed(_toFixed));
    }
    //FY_Secured_Target_Mar
    xST_Price_effect_Mar = $('#ST_Price_effect_Mar').val();
    xST_Volume_Effect_Mar = $('#ST_Volume_Effect_Mar').val();
    if (xST_Price_effect_Mar != null && xST_Volume_Effect_Mar != null) {
        $('#FY_Secured_Target_Mar').val((parseFloat(xST_Price_effect_Mar) + parseFloat(xST_Volume_Effect_Mar)).toFixed(_toFixed));
    }
    //FY_Secured_Target_Apr
    xST_Price_effect_Apr = $('#ST_Price_effect_Apr').val();
    xST_Volume_Effect_Apr = $('#ST_Volume_Effect_Apr').val();
    if (xST_Price_effect_Apr != null && xST_Volume_Effect_Apr != null) {
        $('#FY_Secured_Target_Apr').val((parseFloat(xST_Price_effect_Apr) + parseFloat(xST_Volume_Effect_Apr)).toFixed(_toFixed));
    }
    //FY_Secured_Target_May
    xST_Price_effect_May = $('#ST_Price_effect_May').val();
    xST_Volume_Effect_May = $('#ST_Volume_Effect_May').val();
    if (xST_Price_effect_May != null && xST_Volume_Effect_May != null) {
        $('#FY_Secured_Target_May').val((parseFloat(xST_Price_effect_May) + parseFloat(xST_Volume_Effect_May)).toFixed(_toFixed));
    }
    //FY_Secured_Target_Jun
    xST_Price_effect_Jun = $('#ST_Price_effect_Jun').val();
    xST_Volume_Effect_Jun = $('#ST_Volume_Effect_Jun').val();
    if (xST_Price_effect_Jun != null && xST_Volume_Effect_Jun != null) {
        $('#FY_Secured_Target_Jun').val((parseFloat(xST_Price_effect_Jun) + parseFloat(xST_Volume_Effect_Jun)).toFixed(_toFixed));
    }
    //FY_Secured_Target_Jul
    xST_Price_effect_Jul = $('#ST_Price_effect_Jul').val();
    xST_Volume_Effect_Jul = $('#ST_Volume_Effect_Jul').val();
    if (xST_Price_effect_Jul != null && xST_Volume_Effect_Jul != null) {
        $('#FY_Secured_Target_Jul').val((parseFloat(xST_Price_effect_Jul) + parseFloat(xST_Volume_Effect_Jul)).toFixed(_toFixed));
    }
    //FY_Secured_Target_Aug
    xST_Price_effect_Aug = $('#ST_Price_effect_Aug').val();
    xST_Volume_Effect_Aug = $('#ST_Volume_Effect_Aug').val();
    if (xST_Price_effect_Aug != null && xST_Volume_Effect_Aug != null) {
        $('#FY_Secured_Target_Aug').val((parseFloat(xST_Price_effect_Aug) + parseFloat(xST_Volume_Effect_Aug)).toFixed(_toFixed));
    }
    //FY_Secured_Target_Sep
    xST_Price_effect_Sep = $('#ST_Price_effect_Sep').val();
    xST_Volume_Effect_Sep = $('#ST_Volume_Effect_Sep').val();
    if (xST_Price_effect_Sep != null && xST_Volume_Effect_Sep != null) {
        $('#FY_Secured_Target_Sep').val((parseFloat(xST_Price_effect_Sep) + parseFloat(xST_Volume_Effect_Sep)).toFixed(_toFixed));
    }
    //FY_Secured_Target_Oct
    xST_Price_effect_Oct = $('#ST_Price_effect_Oct').val();
    xST_Volume_Effect_Oct = $('#ST_Volume_Effect_Oct').val();
    if (xST_Price_effect_Oct != null && xST_Volume_Effect_Oct != null) {
        $('#FY_Secured_Target_Oct').val((parseFloat(xST_Price_effect_Oct) + parseFloat(xST_Volume_Effect_Oct)).toFixed(_toFixed));
    }
    //FY_Secured_Target_Nov
    xST_Price_effect_Nov = $('#ST_Price_effect_Nov').val();
    xST_Volume_Effect_Nov = $('#ST_Volume_Effect_Nov').val();
    if (xST_Price_effect_Nov != null && xST_Volume_Effect_Nov != null) {
        $('#FY_Secured_Target_Nov').val((parseFloat(xST_Price_effect_Nov) + parseFloat(xST_Volume_Effect_Nov)).toFixed(_toFixed));
    }
    //FY_Secured_Target_Dec
    xST_Price_effect_Dec = $('#ST_Price_effect_Dec').val();
    xST_Volume_Effect_Dec = $('#ST_Volume_Effect_Dec').val();
    if (xST_Price_effect_Dec != null && xST_Volume_Effect_Dec != null) {
        $('#FY_Secured_Target_Dec').val((parseFloat(xST_Price_effect_Dec) + parseFloat(xST_Volume_Effect_Dec)).toFixed(_toFixed));
    }

    calculate_FY_Secured_Target_Total();
}

function calculate_FY_Secured_Target_Total() {

    if (($('#FY_Secured_Target_Jan') != null &&
        $('#FY_Secured_Target_Feb') != null &&
        $('#FY_Secured_Target_Mar') != null &&
        $('#FY_Secured_Target_Apr') != null &&
        $('#FY_Secured_Target_May') != null &&
        $('#FY_Secured_Target_Jun') != null &&
        $('#FY_Secured_Target_Jul') != null &&
        $('#FY_Secured_Target_Aug') != null &&
        $('#FY_Secured_Target_Sep') != null &&
        $('#FY_Secured_Target_Oct') != null &&
        $('#FY_Secured_Target_Nov') != null &&
        $('#FY_Secured_Target_Dec') != null)) {
        var xFY_Secured_Target_Total = (parseFloat($('#FY_Secured_Target_Jan').val()) +
            parseFloat($('#FY_Secured_Target_Feb').val()) +
            parseFloat($('#FY_Secured_Target_Mar').val()) +
            parseFloat($('#FY_Secured_Target_Apr').val()) +
            parseFloat($('#FY_Secured_Target_May').val()) +
            parseFloat($('#FY_Secured_Target_Jun').val()) +
            parseFloat($('#FY_Secured_Target_Jul').val()) +
            parseFloat($('#FY_Secured_Target_Aug').val()) +
            parseFloat($('#FY_Secured_Target_Sep').val()) +
            parseFloat($('#FY_Secured_Target_Oct').val()) +
            parseFloat($('#FY_Secured_Target_Nov').val()) +
            parseFloat($('#FY_Secured_Target_Dec').val()));

        $('#FY_Secured_Target_Total').val(parseFloat(xFY_Secured_Target_Total).toFixed(_toFixed));
        bind_FY_Secured_Target();
    }
}

function bind_FY_Secured_Target() {

    var _FY_Secured_Target_Jan = $('#FY_Secured_Target_Jan') != null ? $('#FY_Secured_Target_Jan').val() : 0;
    var _FY_Secured_Target_Feb = $('#FY_Secured_Target_Feb') != null ? $('#FY_Secured_Target_Feb').val() : 0;
    var _FY_Secured_Target_Mar = $('#FY_Secured_Target_Mar') != null ? $('#FY_Secured_Target_Mar').val() : 0;
    var _FY_Secured_Target_Apr = $('#FY_Secured_Target_Apr') != null ? $('#FY_Secured_Target_Apr').val() : 0;
    var _FY_Secured_Target_May = $('#FY_Secured_Target_May') != null ? $('#FY_Secured_Target_May').val() : 0;
    var _FY_Secured_Target_Jun = $('#FY_Secured_Target_Jun') != null ? $('#FY_Secured_Target_Jun').val() : 0;
    var _FY_Secured_Target_Jul = $('#FY_Secured_Target_Jul') != null ? $('#FY_Secured_Target_Jul').val() : 0;
    var _FY_Secured_Target_Aug = $('#FY_Secured_Target_Aug') != null ? $('#FY_Secured_Target_Aug').val() : 0;
    var _FY_Secured_Target_Sep = $('#FY_Secured_Target_Sep') != null ? $('#FY_Secured_Target_Sep').val() : 0;
    var _FY_Secured_Target_Oct = $('#FY_Secured_Target_Oct') != null ? $('#FY_Secured_Target_Oct').val() : 0;
    var _FY_Secured_Target_Nov = $('#FY_Secured_Target_Nov') != null ? $('#FY_Secured_Target_Nov').val() : 0;
    var _FY_Secured_Target_Dec = $('#FY_Secured_Target_Dec') != null ? $('#FY_Secured_Target_Dec').val() : 0;
    var _FY_Secured_Target_Total = $('#FY_Secured_Target_Total') != null ? $('#FY_Secured_Target_Total').val() : 0;

    $('#FY_Secured_Target_Jan_').val(Math.round(parseFloat(_FY_Secured_Target_Jan)).toLocaleString("en-US"));
    $('#FY_Secured_Target_Feb_').val(Math.round(parseFloat(_FY_Secured_Target_Feb)).toLocaleString("en-US"));
    $('#FY_Secured_Target_Mar_').val(Math.round(parseFloat(_FY_Secured_Target_Mar)).toLocaleString("en-US"));
    $('#FY_Secured_Target_Apr_').val(Math.round(parseFloat(_FY_Secured_Target_Apr)).toLocaleString("en-US"));
    $('#FY_Secured_Target_May_').val(Math.round(parseFloat(_FY_Secured_Target_May)).toLocaleString("en-US"));
    $('#FY_Secured_Target_Jun_').val(Math.round(parseFloat(_FY_Secured_Target_Jun)).toLocaleString("en-US"));
    $('#FY_Secured_Target_Jul_').val(Math.round(parseFloat(_FY_Secured_Target_Jul)).toLocaleString("en-US"));
    $('#FY_Secured_Target_Aug_').val(Math.round(parseFloat(_FY_Secured_Target_Aug)).toLocaleString("en-US"));
    $('#FY_Secured_Target_Sep_').val(Math.round(parseFloat(_FY_Secured_Target_Sep)).toLocaleString("en-US"));
    $('#FY_Secured_Target_Oct_').val(Math.round(parseFloat(_FY_Secured_Target_Oct)).toLocaleString("en-US"));
    $('#FY_Secured_Target_Nov_').val(Math.round(parseFloat(_FY_Secured_Target_Nov)).toLocaleString("en-US"));
    $('#FY_Secured_Target_Dec_').val(Math.round(parseFloat(_FY_Secured_Target_Dec)).toLocaleString("en-US"));
    $('#FY_Secured_Target_Total_').val(Math.round(parseFloat(_FY_Secured_Target_Total)).toLocaleString("en-US"));
}

//Secured Target END

function isAll_InputGiven_for_CPI_Effect() {

    var isFlag = false;
    //var xCPI = txt_CPI.GetValue();
    //if (xCPI != null) {
    //    if (xCPI == '' || xCPI == '0' || xCPI == '0.') {
    //        isFlag = false;
    //    }
    //    else {
            isFlag = true;
    //    }

    //}
    //else {
    //    isFlag = false;
    //}

    return isFlag;
}

function calculate_CPI_Effect() {
    if (isAll_InputGiven_for_CPI_Effect()) {
        //var xCPI = parseFloat(txt_CPI.GetValue());
        var xCPI_Jan = $('#CPI_Jan') != null ? $('#CPI_Jan').val() : 0;
        var xCPI_Feb = $('#CPI_Feb') != null ? $('#CPI_Feb').val() : 0;
        var xCPI_Mar = $('#CPI_Mar') != null ? $('#CPI_Mar').val() : 0;
        var xCPI_Apr = $('#CPI_Apr') != null ? $('#CPI_Apr').val() : 0;
        var xCPI_May = $('#CPI_May') != null ? $('#CPI_May').val() : 0;
        var xCPI_Jun = $('#CPI_Jun') != null ? $('#CPI_Jun').val() : 0;
        var xCPI_Jul = $('#CPI_Jul') != null ? $('#CPI_Jul').val() : 0;
        var xCPI_Aug = $('#CPI_Aug') != null ? $('#CPI_Aug').val() : 0;
        var xCPI_Sep = $('#CPI_Sep') != null ? $('#CPI_Sep').val() : 0;
        var xCPI_Oct = $('#CPI_Oct') != null ? $('#CPI_Oct').val() : 0;
        var xCPI_Nov = $('#CPI_Nov') != null ? $('#CPI_Nov').val() : 0;
        var xCPI_Dec = $('#CPI_Dec') != null ? $('#CPI_Dec').val() : 0;

        //CPI_Effect_Jan
        var xjan_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Jan').val());
        var xjan_Target_CPU_N = parseFloat($('#Target_CPU_N_Jan').val());
        var xjanActual_volume_N = parseFloat(txt_janActual_volume_N.GetValue());

        var formula1_1 = ((xjan_Target_CPU_N - xjan_Actual_CPU_Nmin1) / xjan_Actual_CPU_Nmin1);
        if (formula1_1 < xCPI_Jan) {

            var formula2_1 = ((xjan_Target_CPU_N - (1 + (xCPI_Jan / 100)) * xjan_Actual_CPU_Nmin1)) * xjanActual_volume_N;
            $('#CPI_Effect_Jan').val(parseFloat(formula2_1).toFixed(_toFixed));
        }
        else {
            $('#CPI_Effect_Jan').val(0);
        }
        //CPI_Effect_Feb
        var xfeb_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Feb').val());
        var xfeb_Target_CPU_N = parseFloat($('#Target_CPU_N_Feb').val());
        var xfebActual_volume_N = parseFloat(txt_febActual_volume_N.GetValue());

        var formula1_2 = ((xfeb_Target_CPU_N - xfeb_Actual_CPU_Nmin1) / xfeb_Actual_CPU_Nmin1);
        if (formula1_2 < xCPI_Feb) {

            var formula2_2 = ((xfeb_Target_CPU_N - (1 + (xCPI_Feb / 100)) * xfeb_Actual_CPU_Nmin1)) * xfebActual_volume_N;
            $('#CPI_Effect_Feb').val(parseFloat(formula2_2).toFixed(_toFixed));
        }
        else {
            $('#CPI_Effect_Feb').val(0);
        }
        //CPI_Effect_Mar
        var xmarch_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Mar').val());
        var xmarch_Target_CPU_N = parseFloat($('#Target_CPU_N_Mar').val());
        var xmarActual_volume_N = parseFloat(txt_marActual_volume_N.GetValue());

        var formula1_3 = ((xmarch_Target_CPU_N - xmarch_Actual_CPU_Nmin1) / xmarch_Actual_CPU_Nmin1);
        if (formula1_3 < xCPI_Mar) {

            var formula2_3 = ((xmarch_Target_CPU_N - (1 + (xCPI_Mar / 100)) * xmarch_Actual_CPU_Nmin1)) * xmarActual_volume_N;
            $('#CPI_Effect_Mar').val(parseFloat(formula2_3).toFixed(_toFixed));
        }
        else {
            $('#CPI_Effect_Mar').val(0);
        }
        //CPI_Effect_Apr
        var xapr_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Apr').val());
        var xapr_Target_CPU_N = parseFloat($('#Target_CPU_N_Apr').val());
        var xaprActual_volume_N = parseFloat(txt_aprActual_volume_N.GetValue());

        var formula1_4 = ((xapr_Target_CPU_N - xapr_Actual_CPU_Nmin1) / xapr_Actual_CPU_Nmin1);
        if (formula1_4 < xCPI_Apr) {

            var formula2_4 = ((xapr_Target_CPU_N - (1 + (xCPI_Apr / 100)) * xapr_Actual_CPU_Nmin1)) * xaprActual_volume_N;
            $('#CPI_Effect_Apr').val(parseFloat(formula2_4).toFixed(_toFixed));
        }
        else {
            $('#CPI_Effect_Apr').val(0);
        }
        //CPI_Effect_May
        var xmay_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_May').val());
        var xmay_Target_CPU_N = parseFloat($('#Target_CPU_N_May').val());
        var xmayActual_volume_N = parseFloat(txt_mayActual_volume_N.GetValue());

        var formula1_5 = ((xmay_Target_CPU_N - xmay_Actual_CPU_Nmin1) / xmay_Actual_CPU_Nmin1);
        if (formula1_5 < xCPI_May) {

            var formula2_5 = ((xmay_Target_CPU_N - (1 + (xCPI_May / 100)) * xmay_Actual_CPU_Nmin1)) * xmayActual_volume_N;
            $('#CPI_Effect_May').val(parseFloat(formula2_5).toFixed(_toFixed));
        }
        else {
            $('#CPI_Effect_May').val(0);
        }
        //CPI_Effect_Jun
        var xjun_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Jun').val());
        var xjun_Target_CPU_N = parseFloat($('#Target_CPU_N_Jun').val());
        var xjunActual_volume_N = parseFloat(txt_junActual_volume_N.GetValue());

        var formula1_6 = ((xjun_Target_CPU_N - xjun_Actual_CPU_Nmin1) / xjun_Actual_CPU_Nmin1);
        if (formula1_6 < xCPI_Jun) {

            var formula2_6 = ((xjun_Target_CPU_N - (1 + (xCPI_Jun / 100)) * xjun_Actual_CPU_Nmin1)) * xjunActual_volume_N;
            $('#CPI_Effect_Jun').val(parseFloat(formula2_6).toFixed(_toFixed));
        }
        else {
            $('#CPI_Effect_Jun').val(0);
        }
        //CPI_Effect_Jul
        var xjul_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Jul').val());
        var xjul_Target_CPU_N = parseFloat($('#Target_CPU_N_Jul').val());
        var xjulActual_volume_N = parseFloat(txt_julActual_volume_N.GetValue());

        var formula1_7 = ((xjul_Target_CPU_N - xjul_Actual_CPU_Nmin1) / xjul_Actual_CPU_Nmin1);
        if (formula1_7 < xCPI_Jul) {

            var formula2_7 = ((xjul_Target_CPU_N - (1 + (xCPI_Jul / 100)) * xjul_Actual_CPU_Nmin1)) * xjulActual_volume_N;
            $('#CPI_Effect_Jul').val(parseFloat(formula2_7).toFixed(_toFixed));
        }
        else {
            $('#CPI_Effect_Jul').val(0);
        }
        //CPI_Effect_Aug
        var xaug_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Aug').val());
        var xaug_Target_CPU_N = parseFloat($('#Target_CPU_N_Aug').val());
        var xaugActual_volume_N = parseFloat(txt_augActual_volume_N.GetValue());

        var formula1_8 = ((xaug_Target_CPU_N - xaug_Actual_CPU_Nmin1) / xaug_Actual_CPU_Nmin1);
        if (formula1_8 < xCPI_Aug) {

            var formula2_8 = ((xaug_Target_CPU_N - (1 + (xCPI_Aug / 100)) * xaug_Actual_CPU_Nmin1)) * xaugActual_volume_N;
            $('#CPI_Effect_Aug').val(parseFloat(formula2_8).toFixed(_toFixed));
        }
        else {
            $('#CPI_Effect_Aug').val(0);
        }
        //CPI_Effect_Sep
        var xsep_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Sep').val());
        var xsep_Target_CPU_N = parseFloat($('#Target_CPU_N_Sep').val());
        var xsepActual_volume_N = parseFloat(txt_sepActual_volume_N.GetValue());

        var formula1_9 = ((xsep_Target_CPU_N - xsep_Actual_CPU_Nmin1) / xsep_Actual_CPU_Nmin1);
        if (formula1_9 < xCPI_Sep) {

            var formula2_9 = ((xsep_Target_CPU_N - (1 + (xCPI_Sep / 100)) * xsep_Actual_CPU_Nmin1)) * xsepActual_volume_N;
            $('#CPI_Effect_Sep').val(parseFloat(formula2_9).toFixed(_toFixed));
        }
        else {
            $('#CPI_Effect_Sep').val(0);
        }
        //CPI_Effect_Oct
        var xoct_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Oct').val());
        var xoct_Target_CPU_N = parseFloat($('#Target_CPU_N_Oct').val());
        var xoctActual_volume_N = parseFloat(txt_octActual_volume_N.GetValue());

        var formula1_10 = ((xoct_Target_CPU_N - xoct_Actual_CPU_Nmin1) / xoct_Actual_CPU_Nmin1);
        if (formula1_10 < xCPI_Oct) {

            var formula2_10 = ((xoct_Target_CPU_N - (1 + (xCPI_Oct / 100)) * xoct_Actual_CPU_Nmin1)) * xoctActual_volume_N;
            $('#CPI_Effect_Oct').val(parseFloat(formula2_10).toFixed(_toFixed));
        }
        else {
            $('#CPI_Effect_Oct').val(0);
        }
        //CPI_Effect_Nov
        var xnov_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Nov').val());
        var xnov_Target_CPU_N = parseFloat($('#Target_CPU_N_Nov').val());
        var xnovActual_volume_N = parseFloat(txt_novActual_volume_N.GetValue());

        var formula1_11 = ((xnov_Target_CPU_N - xnov_Actual_CPU_Nmin1) / xnov_Actual_CPU_Nmin1);
        if (formula1_11 < xCPI_Nov) {

            var formula2_11 = ((xnov_Target_CPU_N - (1 + (xCPI_Nov / 100)) * xnov_Actual_CPU_Nmin1)) * xnovActual_volume_N;
            $('#CPI_Effect_Nov').val(parseFloat(formula2_11).toFixed(_toFixed));
        }
        else {
            $('#CPI_Effect_Nov').val(0);
        }
        //CPI_Effect_Dec
        var xdec_Actual_CPU_Nmin1 = parseFloat($('#Actual_CPU_Nmin1_Dec').val());
        var xdec_Target_CPU_N = parseFloat($('#Target_CPU_N_Dec').val());
        var xdecActual_volume_N = parseFloat(txt_decActual_volume_N.GetValue());

        var formula1_12 = ((xdec_Target_CPU_N - xdec_Actual_CPU_Nmin1) / xdec_Actual_CPU_Nmin1);
        if (formula1_12 < xCPI_Dec) {

            var formula2_12 = ((xdec_Target_CPU_N - (1 + (xCPI_Dec / 100)) * xdec_Actual_CPU_Nmin1)) * xdecActual_volume_N;
            $('#CPI_Effect_Dec').val(parseFloat(formula2_12).toFixed(_toFixed));
        }
        else {
            $('#CPI_Effect_Dec').val(0);
        }

    }

    calculate_CPI_Effect_Total();
}

function calculate_CPI_Effect_Total() {
    if (($('#CPI_Effect_Jan') != null &&
        $('#CPI_Effect_Feb') != null &&
        $('#CPI_Effect_Mar') != null &&
        $('#CPI_Effect_Apr') != null &&
        $('#CPI_Effect_May') != null &&
        $('#CPI_Effect_Jun') != null &&
        $('#CPI_Effect_Jul') != null &&
        $('#CPI_Effect_Aug') != null &&
        $('#CPI_Effect_Sep') != null &&
        $('#CPI_Effect_Oct') != null &&
        $('#CPI_Effect_Nov') != null &&
        $('#CPI_Effect_Dec') != null)) {
        var xCPI_Effect_Total = (parseFloat($('#CPI_Effect_Jan').val()) +
            parseFloat($('#CPI_Effect_Feb').val()) +
            parseFloat($('#CPI_Effect_Mar').val()) +
            parseFloat($('#CPI_Effect_Apr').val()) +
            parseFloat($('#CPI_Effect_May').val()) +
            parseFloat($('#CPI_Effect_Jun').val()) +
            parseFloat($('#CPI_Effect_Jul').val()) +
            parseFloat($('#CPI_Effect_Aug').val()) +
            parseFloat($('#CPI_Effect_Sep').val()) +
            parseFloat($('#CPI_Effect_Oct').val()) +
            parseFloat($('#CPI_Effect_Nov').val()) +
            parseFloat($('#CPI_Effect_Dec').val()));

        $('#CPI_Effect_Total').val(parseFloat(xCPI_Effect_Total).toFixed(_toFixed));
        calculate_txt_YTD_Cost_Avoid_Vs_CPI();
    }
}

function bind_CPI_Fields() {

    var _CPI_Effect_Jan = $('#CPI_Effect_Jan') != null ? $('#CPI_Effect_Jan').val() : 0;
    var _CPI_Effect_Feb = $('#CPI_Effect_Feb') != null ? $('#CPI_Effect_Feb').val() : 0;
    var _CPI_Effect_Mar = $('#CPI_Effect_Mar') != null ? $('#CPI_Effect_Mar').val() : 0;
    var _CPI_Effect_Apr = $('#CPI_Effect_Apr') != null ? $('#CPI_Effect_Apr').val() : 0;
    var _CPI_Effect_May = $('#CPI_Effect_May') != null ? $('#CPI_Effect_May').val() : 0;
    var _CPI_Effect_Jun = $('#CPI_Effect_Jun') != null ? $('#CPI_Effect_Jun').val() : 0;
    var _CPI_Effect_Jul = $('#CPI_Effect_Jul') != null ? $('#CPI_Effect_Jul').val() : 0;
    var _CPI_Effect_Aug = $('#CPI_Effect_Aug') != null ? $('#CPI_Effect_Aug').val() : 0;
    var _CPI_Effect_Sep = $('#CPI_Effect_Sep') != null ? $('#CPI_Effect_Sep').val() : 0;
    var _CPI_Effect_Oct = $('#CPI_Effect_Oct') != null ? $('#CPI_Effect_Oct').val() : 0;
    var _CPI_Effect_Nov = $('#CPI_Effect_Nov') != null ? $('#CPI_Effect_Nov').val() : 0;
    var _CPI_Effect_Dec = $('#CPI_Effect_Dec') != null ? $('#CPI_Effect_Dec').val() : 0;
    var _CPI_Effect_Total = $('#CPI_Effect_Total') != null ? $('#CPI_Effect_Total').val() : 0;

    $('#CPI_Effect_Jan_').val(Math.round(parseFloat(_CPI_Effect_Jan)).toLocaleString("en-US"));
    $('#CPI_Effect_Feb_').val(Math.round(parseFloat(_CPI_Effect_Feb)).toLocaleString("en-US"));
    $('#CPI_Effect_Mar_').val(Math.round(parseFloat(_CPI_Effect_Mar)).toLocaleString("en-US"));
    $('#CPI_Effect_Apr_').val(Math.round(parseFloat(_CPI_Effect_Apr)).toLocaleString("en-US"));
    $('#CPI_Effect_May_').val(Math.round(parseFloat(_CPI_Effect_May)).toLocaleString("en-US"));
    $('#CPI_Effect_Jun_').val(Math.round(parseFloat(_CPI_Effect_Jun)).toLocaleString("en-US"));
    $('#CPI_Effect_Jul_').val(Math.round(parseFloat(_CPI_Effect_Jul)).toLocaleString("en-US"));
    $('#CPI_Effect_Aug_').val(Math.round(parseFloat(_CPI_Effect_Aug)).toLocaleString("en-US"));
    $('#CPI_Effect_Sep_').val(Math.round(parseFloat(_CPI_Effect_Sep)).toLocaleString("en-US"));
    $('#CPI_Effect_Oct_').val(Math.round(parseFloat(_CPI_Effect_Oct)).toLocaleString("en-US"));
    $('#CPI_Effect_Nov_').val(Math.round(parseFloat(_CPI_Effect_Nov)).toLocaleString("en-US"));
    $('#CPI_Effect_Dec_').val(Math.round(parseFloat(_CPI_Effect_Dec)).toLocaleString("en-US"));
    $('#CPI_Effect_Total_').val(Math.round(parseFloat(_CPI_Effect_Total)).toLocaleString("en-US"));

    bind_Monthly_CPI();
}

function bind_Monthly_CPI() {

    var _CPI_Jan = $('#CPI_Jan') != null ? $('#CPI_Jan').val() : 0;
    var _CPI_Feb = $('#CPI_Feb') != null ? $('#CPI_Feb').val() : 0;
    var _CPI_Mar = $('#CPI_Mar') != null ? $('#CPI_Mar').val() : 0;
    var _CPI_Apr = $('#CPI_Apr') != null ? $('#CPI_Apr').val() : 0;
    var _CPI_May = $('#CPI_May') != null ? $('#CPI_May').val() : 0;
    var _CPI_Jun = $('#CPI_Jun') != null ? $('#CPI_Jun').val() : 0;
    var _CPI_Jul = $('#CPI_Jul') != null ? $('#CPI_Jul').val() : 0;
    var _CPI_Aug = $('#CPI_Aug') != null ? $('#CPI_Aug').val() : 0;
    var _CPI_Sep = $('#CPI_Sep') != null ? $('#CPI_Sep').val() : 0;
    var _CPI_Oct = $('#CPI_Oct') != null ? $('#CPI_Oct').val() : 0;
    var _CPI_Nov = $('#CPI_Nov') != null ? $('#CPI_Nov').val() : 0;
    var _CPI_Dec = $('#CPI_Dec') != null ? $('#CPI_Dec').val() : 0;

    $('#CPI_Jan_').val(Math.round(parseFloat(_CPI_Jan)).toLocaleString("en-US"));
    $('#CPI_Feb_').val(Math.round(parseFloat(_CPI_Feb)).toLocaleString("en-US"));
    $('#CPI_Mar_').val(Math.round(parseFloat(_CPI_Mar)).toLocaleString("en-US"));
    $('#CPI_Apr_').val(Math.round(parseFloat(_CPI_Apr)).toLocaleString("en-US"));
    $('#CPI_May_').val(Math.round(parseFloat(_CPI_May)).toLocaleString("en-US"));
    $('#CPI_Jun_').val(Math.round(parseFloat(_CPI_Jun)).toLocaleString("en-US"));
    $('#CPI_Jul_').val(Math.round(parseFloat(_CPI_Jul)).toLocaleString("en-US"));
    $('#CPI_Aug_').val(Math.round(parseFloat(_CPI_Aug)).toLocaleString("en-US"));
    $('#CPI_Sep_').val(Math.round(parseFloat(_CPI_Sep)).toLocaleString("en-US"));
    $('#CPI_Oct_').val(Math.round(parseFloat(_CPI_Oct)).toLocaleString("en-US"));
    $('#CPI_Nov_').val(Math.round(parseFloat(_CPI_Nov)).toLocaleString("en-US"));
    $('#CPI_Dec_').val(Math.round(parseFloat(_CPI_Dec)).toLocaleString("en-US"));
}
//POP UP Calcs



//N FY Secured START
function calculate_txt_N_FY_Sec_PRICE_EF() {
    var xST_Price_effect_Total = $('#ST_Price_effect_Total').val();
    if (xST_Price_effect_Total != null) {
        txt_N_FY_Sec_PRICE_EF.SetValue(parseFloat(xST_Price_effect_Total).toFixed(_toFixed));
    }

    calculate_txt_N_FY_Secured();
}

function calculate_txt_N_FY_Sec_VOLUME_EF() {
    var xST_Volume_Effect_Total = $('#ST_Volume_Effect_Total').val();
    if (xST_Volume_Effect_Total != null) {
        txt_N_FY_Sec_VOLUME_EF.SetValue(parseFloat(xST_Volume_Effect_Total).toFixed(_toFixed));
    }

    calculate_txt_N_FY_Secured();
}

function calculate_txt_N_FY_Secured() {
    var xtxt_N_FY_Sec_PRICE_EF = txt_N_FY_Sec_PRICE_EF.GetValue();
    var xtxt_N_FY_Sec_VOLUME_EF = txt_N_FY_Sec_VOLUME_EF.GetValue();

    if (xtxt_N_FY_Sec_PRICE_EF != null && xtxt_N_FY_Sec_VOLUME_EF != null) {
        var xtxt_N_FY_Secured = parseFloat(xtxt_N_FY_Sec_PRICE_EF) + parseFloat(xtxt_N_FY_Sec_VOLUME_EF);

        txt_N_FY_Secured.SetValue(parseFloat(xtxt_N_FY_Secured).toFixed(_toFixed));
    }
}
//N FY Secured END

//Secured Target YTD Calcs START
function calculate_txt_N_YTD_Sec_PRICE_EF() {
    const ST_PriceEffect = new Array("ST_Price_effect_Jan", "ST_Price_effect_Feb", "ST_Price_effect_Mar", "ST_Price_effect_Apr", "ST_Price_effect_May", "ST_Price_effect_Jun", "ST_Price_effect_Jul", "ST_Price_effect_Aug", "ST_Price_effect_Sep", "ST_Price_effect_Oct", "ST_Price_effect_Nov", "ST_Price_effect_Dec");
    var till_Month = projectMonth - 1;
    var start_month = 0;

    if (start_month != till_Month) {
        var xN_YTD_Sec_PRICE_EF = 0
        while (start_month <= till_Month) {
            xN_YTD_Sec_PRICE_EF = (parseFloat(xN_YTD_Sec_PRICE_EF) + parseFloat($("#" + ST_PriceEffect[start_month]).val()));
            start_month += 1;
        }

        txt_N_YTD_Sec_PRICE_EF.SetValue(parseFloat(xN_YTD_Sec_PRICE_EF).toFixed(_toFixed));
    }
    else {
        txt_N_YTD_Sec_PRICE_EF.SetValue(parseFloat($("#" + ST_PriceEffect[start_month]).val()).toFixed(_toFixed));
    }

    calculate_txt_N_YTD_Secured();
}

function calculate_txt_N_YTD_Sec_VOLUME_EF() {
    const ST_VolumeEffect = new Array("ST_Volume_Effect_Jan", "ST_Volume_Effect_Feb", "ST_Volume_Effect_Mar", "ST_Volume_Effect_Apr", "ST_Volume_Effect_May", "ST_Volume_Effect_Jun", "ST_Volume_Effect_Jul", "ST_Volume_Effect_Aug", "ST_Volume_Effect_Sep", "ST_Volume_Effect_Oct", "ST_Volume_Effect_Nov", "ST_Volume_Effect_Dec");
    var till_Month = projectMonth - 1;
    var start_month = 0;


    if (start_month != till_Month) {
        var xN_YTD_Sec_VOLUME_EF = 0
        while (start_month <= till_Month) {
            xN_YTD_Sec_VOLUME_EF = (parseFloat(xN_YTD_Sec_VOLUME_EF) + parseFloat($("#" + ST_VolumeEffect[start_month]).val()));
            start_month += 1;
        }

        txt_N_YTD_Sec_VOLUME_EF.SetValue(parseFloat(xN_YTD_Sec_VOLUME_EF).toFixed(_toFixed));
    }
    else {
        txt_N_YTD_Sec_VOLUME_EF.SetValue(parseFloat($("#" + ST_VolumeEffect[start_month]).val()).toFixed(_toFixed));
    }

    calculate_txt_N_YTD_Secured();
}

function calculate_txt_N_YTD_Secured() {
    var xtxt_N_YTD_Sec_PRICE_EF = txt_N_YTD_Sec_PRICE_EF.GetValue();
    var xtxt_N_YTD_Sec_VOLUME_EF = txt_N_YTD_Sec_VOLUME_EF.GetValue();

    if (xtxt_N_YTD_Sec_PRICE_EF != null && xtxt_N_YTD_Sec_VOLUME_EF != null) {
        var xtxt_N_YTD_Secured = parseFloat(xtxt_N_YTD_Sec_PRICE_EF) + parseFloat(xtxt_N_YTD_Sec_VOLUME_EF);
        txt_N_YTD_Secured.SetValue(xtxt_N_YTD_Secured.toFixed(_toFixed));
    }
}
//Secured Target YTD Calcs END

//Acheivement YTD Calcs STARTS
function calculate_txt_YTD_Achieved_PRICE_EF() {

    const A_Price_effect = new Array("A_Price_effect_Jan", "A_Price_effect_Feb", "A_Price_effect_Mar", "A_Price_effect_Apr", "A_Price_effect_May", "A_Price_effect_Jun", "A_Price_effect_Jul", "A_Price_effect_Aug", "A_Price_effect_Sep", "A_Price_effect_Oct", "A_Price_effect_Nov", "A_Price_effect_Dec");
    var till_Month = projectMonth - 1;
    var start_month = 0;

    if (start_month != till_Month) {
        var xYTD_Achieved_PRICE_EF = 0
        while (start_month <= till_Month) {
            xYTD_Achieved_PRICE_EF = (parseFloat(xYTD_Achieved_PRICE_EF) + parseFloat($("#" + A_Price_effect[start_month]).val()));
            start_month += 1;
        }

        txt_YTD_Achieved_PRICE_EF.SetValue(parseFloat(xYTD_Achieved_PRICE_EF).toFixed(_toFixed));
    }
    else {
        txt_YTD_Achieved_PRICE_EF.SetValue(parseFloat($("#" + A_Price_effect[start_month]).val()).toFixed(_toFixed));
    }

    calculate_txt_YTD_achieved();
}

function calculated_txt_YTD_Achieved_VOLUME_EF() {

    const A_VOLUME_EF = new Array("A_Volume_Effect_Jan", "A_Volume_Effect_Feb", "A_Volume_Effect_Mar", "A_Volume_Effect_Apr", "A_Volume_Effect_May", "A_Volume_Effect_Jun", "A_Volume_Effect_Jul", "A_Volume_Effect_Aug", "A_Volume_Effect_Sep", "A_Volume_Effect_Oct", "A_Volume_Effect_Nov", "A_Volume_Effect_Dec");
    var till_Month = projectMonth - 1;
    var start_month = 0;

    if (start_month != till_Month) {
        var xYTD_Achieved_VOLUME_EF = 0
        while (start_month <= till_Month) {
            xYTD_Achieved_VOLUME_EF = (parseFloat(xYTD_Achieved_VOLUME_EF) + parseFloat($("#" + A_VOLUME_EF[start_month]).val()));
            start_month += 1;
        }
        txt_YTD_Achieved_VOLUME_EF.SetValue(parseFloat(xYTD_Achieved_VOLUME_EF).toFixed(_toFixed));
    }
    else {
        txt_YTD_Achieved_VOLUME_EF.SetValue(parseFloat($("#" + A_VOLUME_EF[start_month]).val()).toFixed(_toFixed));
    }

    calculate_txt_YTD_achieved();
}

function calculate_txt_YTD_achieved() {
    var xtxt_YTD_Achieved_PRICE_EF = txt_YTD_Achieved_PRICE_EF.GetValue();
    var xtxt_YTD_Achieved_VOLUME_EF = txt_YTD_Achieved_VOLUME_EF.GetValue();

    if (xtxt_YTD_Achieved_PRICE_EF != null && xtxt_YTD_Achieved_VOLUME_EF != null) {

        var xtxt_YTD_achieved = parseFloat(xtxt_YTD_Achieved_PRICE_EF) + parseFloat(xtxt_YTD_Achieved_VOLUME_EF);
        txt_YTD_achieved.SetValue(xtxt_YTD_achieved.toFixed(_toFixed));

        //GrdInitType
        var xGrdInitType = GrdInitType.GetText();
        //GrdInitCategory
        var xGrdInitCategory = GrdInitCategory.GetText();
        //GrdSubCost
        var xGrdSubCost = GrdSubCost.GetText();

        if (xtxt_YTD_achieved < 0) {
            GrdInitType.SetText("Negative Cost Impact");
            GrdInitCategory.SetText(xGrdInitCategory);
            GrdSubCost.SetText(xGrdSubCost);
        }
        else {
            GrdInitType.SetText("Positive Cost Impact");
            GrdInitCategory.SetText(xGrdInitCategory);
            GrdSubCost.SetText(xGrdSubCost);
        }
    }
}
//Acheivement YTD Calcs END



// CPI YTD Calcs START

function calculate_txt_YTD_Cost_Avoid_Vs_CPI() {
    const Cost_Avoid_Vs_CPI = new Array("CPI_Effect_Jan", "CPI_Effect_Feb", "CPI_Effect_Mar", "CPI_Effect_Apr", "CPI_Effect_May", "CPI_Effect_Jun", "CPI_Effect_Jul", "CPI_Effect_Aug", "CPI_Effect_Sep", "CPI_Effect_Oct", "CPI_Effect_Nov", "CPI_Effect_Dec");
    var till_Month = projectMonth - 1;
    var start_month = 0;

    if (start_month != till_Month) {
        var xYTD_Cost_Avoid_Vs_CPI = 0
        while (start_month <= till_Month) {
            xYTD_Cost_Avoid_Vs_CPI = (parseFloat(xYTD_Cost_Avoid_Vs_CPI) + parseFloat($("#" + Cost_Avoid_Vs_CPI[start_month]).val()));
            start_month += 1;
        }
        txt_YTD_Cost_Avoid_Vs_CPI.SetValue(parseFloat(xYTD_Cost_Avoid_Vs_CPI).toFixed(_toFixed));
    }
    else {
        txt_YTD_Cost_Avoid_Vs_CPI.SetValue(parseFloat($("#" + Cost_Avoid_Vs_CPI[start_month]).val()).toFixed(_toFixed));
    }

    calculate_txt_FY_Cost_Avoid_Vs_CPI();
}

function calculate_txt_FY_Cost_Avoid_Vs_CPI() {
    if ($('#CPI_Effect_Total') != null) {
        txt_FY_Cost_Avoid_Vs_CPI.SetText(parseFloat($('#CPI_Effect_Total').val()).toFixed(_toFixed));
    }
}
// CPI YTD Calcs END

//ENH153-2 calculations
