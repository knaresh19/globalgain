////@{
////    var profileData = this.Session["DefaultGAINSess"] as GAIN.Models.LoginSession;
////    var user_type = profileData.UserType;
////    var years_right = profileData.years_right;
////    var projectYear = profileData.ProjectYear;
////}

function URLContent(url) {
    return UrlContent + url;
}

$(function () {
    $("#BtnInitiative").on("click", function () {
        var min_py = 0; var max_py = 0;
        var py = projectYear;
        min_py = (+py - 1);
        max_py = (+py);
        ////debugger;;
        StartMonth.SetMinDate(new Date(min_py + '-01-01'));
        StartMonth.SetMaxDate(new Date(max_py + '-12-31'));
        EndMonth.SetMinDate(new Date(max_py + '-01-01'));
        EndMonth.SetMaxDate(new Date((max_py + 1) + '-12-31'));

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
                GrdActionType.clientEnabled = false;
                GrdActionType.SelectIndex(1);
            }
            else { GrdActionType.SelectIndex(0); }
            GrdInitType.SelectIndex(0);  GrdSynImpact.SelectIndex(0); GrdInitStatus.SelectIndex(0); TxPortName.SelectIndex(0); GrdInitCategory.SelectIndex(0); CboWebinarCat.SelectIndex(0);
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
                ////debugger;;
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
        min_py = (+py - 1);
        max_py = (+py);
        ////debugger;;
        StartMonth.SetMinDate(new Date(min_py + '-01-01'));
        StartMonth.SetMaxDate(new Date(max_py + '-12-31'));
        EndMonth.SetMinDate(new Date(max_py + '-01-01'));
        EndMonth.SetMaxDate(new Date((max_py + 1) + '-12-31'));

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
                GrdActionType.clientEnabled = false;
                GrdActionType.SelectIndex(2);
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
                ////debugger;;
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
    //debugger;;
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
    var min_py = 0; var max_py = 0;
    var py = projectYear;
    min_py = (+py - 1);
    max_py = (+py);
    StartMonth.SetMinDate(new Date(min_py + '-01-01'));
    StartMonth.SetMaxDate(new Date((max_py) + '-12-31'));
    EndMonth.SetMinDate(new Date(max_py + '-01-01'));
    EndMonth.SetMaxDate(new Date((max_py + 1) + '-12-31'));

    $("#FormStatus").val("Edit");
    //debugger;;

    var GrdInit_Type = 0, GrdAction_Type = 0, GrdSyn_Impact = 0, GrdInit_Status = 0, TxPort_Name = 0;
    var GrdInit_Category = 0, CboWebinar_Cat = 0, Grd_SubCost =0;

    CboHoValidity.AddItem("Y"); CboHoValidity.AddItem("N");
    CboRPOCValidity.AddItem("KO", "KO"); CboRPOCValidity.AddItem("Under Review", "UR"); CboRPOCValidity.AddItem("OK Level 1 - L1 if FY Target > 200 kUSD (L1= Cost controller)", "L1");
    CboRPOCValidity.AddItem("OK Level 2 - L2 if FY Target > 300 kUSD (L2 =Management RO)", "L2"); CboRPOCValidity.AddItem("OK Level 3 - L3 if FY Target > 500 kUSD (L3 = Coordinateur HO)", "L3");
    //debugger;;
    $.post(URLContent('ActiveInitiative/GetInfoById'), { Id: id }, function (data) {
        if (data != '') {
            var obj = JSON.parse(data); var SubCountryID = obj.SubCountryID;

            GrdInit_Type = obj.InitiativeType;
            GrdAction_Type = obj.ActionTypeID;
            GrdSyn_Impact = obj.SynergyImpactID;
            GrdInit_Status = obj.InitStatus;
            TxPort_Name = obj.PortID;
            GrdInit_Category = obj.CostCategoryID;
            CboWebinar_Cat = obj.SourceCategory;
            Grd_SubCost = obj.SubCostCategoryID;

           
          
            //GrdInitType.SetValue(obj.InitiativeType);
            //GrdActionType.SetValue(obj.ActionTypeID);
            //GrdSynImpact.SetValue(obj.SynergyImpactID);
            //GrdInitStatus.SetValue(obj.InitStatus);
            //TxPortName.SetValue(obj.PortID);
            //GrdInitCategory.SetValue(obj.CostCategoryID);
            //GrdSubCost.SetValue(obj.SubCostCategoryID);
            //CboWebinarCat.SetValue(obj.SourceCategory);
          

            $.post(URLContent('ActiveInitiative/GetInfoForPopUp'), { Id: id }, function (DDdata) {
                var obj1; GrdInitType.ClearItems(); GrdActionType.ClearItems(); GrdSynImpact.ClearItems(); GrdInitStatus.ClearItems(); TxPortName.ClearItems(); GrdInitCategory.ClearItems(); GrdSubCost.ClearItems();
                var uType = user_type;
                CboWebinarCat.ClearItems();
                $.each(DDdata[0]["SavingTypeData"], function (key, dd_value) {
                    dd_value = JSON.stringify(dd_value); obj1 = JSON.parse(dd_value);
                    if (obj1 != null) GrdInitType.AddItem(obj1.SavingTypeName, obj1.id);
                });
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

                if (GrdInit_Type != null)
                    GrdInitType.SelectIndex(GrdInit_Type);
                else
                    GrdInitType.SelectIndex(0);

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
            $("#GrdInitCategory").val(obj.CostTypeName);
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
            ////debugger;;
            OnStartMonthChanged();
            let startyear = new Date(obj.StartMonth).getFullYear()
            let nextyear = startyear + 1;

            //Change the labels 
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
            $.post(URLContent('ActiveInitiative/GetCountryBySub'), { Id: SubCountryID, Id2: id }, function (data) {
                var obj2;
                ////debugger;;
                GrdSubCountryPopup.ClearItems(); GrdBrandPopup.ClearItems(); GrdLegalEntityPopup.ClearItems();
                $.each(data[0]["SubCountryData"], function (key, value) {
                    value = JSON.stringify(value); obj2 = JSON.parse(value);
                    if (obj != null) GrdSubCountryPopup.AddItem(obj2.SubCountryName, obj2.id);
                });
                $.each(data[0]["BrandData"], function (key, value) {
                    value = JSON.stringify(value); obj2 = JSON.parse(value);
                    if (obj != null) GrdBrandPopup.AddItem(obj2.BrandName, obj2.id);
                });
                ////debugger;;
                $.each(data[0]["LegalEntityData"], function (key, value) {
                    value = JSON.stringify(value); obj2 = JSON.parse(value);
                    if (obj != null) GrdLegalEntityPopup.AddItem(obj2.LegalEntityName, obj2.id);
                });
                GrdSubCountryPopup.SelectIndex(0); GrdBrandPopup.SelectIndex(0); GrdLegalEntityPopup.SelectIndex(0);
                GrdBrandPopup.SetValue(brandId);
                GrdLegalEntityPopup.SetValue(legalentityidx);
            });
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
                isThereNewRecord = false;            BtnProcurementIniti
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
    txTarget12.clientEnabled = true;

    //GrdLegalEntity.SetText = '';
    $.post(URLContent('ActiveInitiative/RemoveSelectedGridLookup'), function (data) {
        console.log(data);
    });
    //GrdBrand.GetGridView().Refresh();
}

function OnInit(s, e) {
    if (projectYear > 2022) {
        BtnProcurementInitiative.hidden = false;
    }
    else {
        BtnProcurementInitiative.hidden = true;    
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

function OnSubCountryPopupChanged(s, e) {
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
        ////debugger;;
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
        GrdBrandPopup.SelectIndex(0);
    });
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
    $.post(URLContent('ActiveInitiative/GetItemFromSubCost'), { id: id }, function (data) {
        var obj; GrdActionType.ClearItems();
        $.each(data[0]["ActionTypeData"], function (key, value) {
            value = JSON.stringify(value); obj = JSON.parse(value);
            if (obj != null) GrdActionType.AddItem(obj.ActionTypeName, obj.id);
        });
        GrdActionType.SelectIndex(0);
    });
}

function OnGrdInitCategoryPopupChanged(s, e) {
    //debugger;;
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
    var dt = new Date(awal.getFullYear(), awal.getMonth() + 11, 1);
    EndMonth.SetMaxDate(dt);
}

function OnEndMonthChanged() {
    var targetstartselected = false; var targetstartselected2 = true; var startmon; /*var projectYear = '@profileData.ProjectYear';*/
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

    var formstatus; var initstatusvalue; var uType;
    uType = user_type;
    formstatus = $("#FormStatus").val();
    initstatusvalue = GrdInitStatus.GetValue();
    if (uType == 3 && formstatus == "Edit" && initstatusvalue != "4") {
        $("#chkAuto").prop("disabled", true);
        $(".txTarget").prop("disabled", true);
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
                    ////debugger;;
                    Swal.fire(
                        'Inconsistent Target',
                        'Sum of monthly target and 12 months target,both  should be positive or negative',
                        'error'
                    );
                    return;
                }
                //}

                //else {
                //    //debugger;;
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
                //    //debugger;;
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
                ////debugger;;
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
                    savingjan2: savingjan2, savingfeb2: savingfeb2, savingmar2: savingmar2, savingapr2: savingapr2, savingmay2: savingmay2, savingjun2: savingjun2, savingjul2: savingjul2, savingaug2: savingaug2, savingsep2: savingsep2, savingoct2: savingoct2, savingnov2: savingnov2, savingdec2: savingdec2
                }, function (data) {
                    ////debugger;;
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
    //debugger;;
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
            ////debugger;;

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
            //    //debugger;;
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
    var m = d.getMonth();
    //debugger;;
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

            m = 12
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
