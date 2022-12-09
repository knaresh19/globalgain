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

                GrdInitType.clientEnabled = false;

            }
            else {
                GrdActionType.clientEnabled = true;
                GrdActionType.SelectIndex(0);

                GrdInitType.clientEnabled = true;

            }

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







