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
    if (uType == 3 && id.toLowerCase() == "work in progress" && xFormStatus != "New") {
        GrdInitStatus.SelectIndex(0);
        Swal.fire(
            'You can not change the status',
            'Agency user can not change to Work in Progress',
            'error'
        );
    }
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

