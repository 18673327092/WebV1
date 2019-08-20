$(function () {
    //加载签到日历
    initSignDiv();
});

function getTime(timetype) {
    var date = new Date();
    var startdate = null;
    var enddate = null;
    var param = {};
    if (timetype == 1) {
        startdate = "1914-08-07 00:00:00";
        enddate = "2114-08-07 23:59:59";
    }
    if (timetype == 2) {
        startdate = date.format("yyyy-MM-dd") + " 00:00:00";
        enddate = date.format("yyyy-MM-dd") + " 23:59:59";
    }
    if (timetype == 3) {
        var date1 = new Date(date.getTime() - 1 * 24 * 60 * 60 * 1000);
        startdate = date1.format("yyyy-MM-dd") + " 00:00:00";
        enddate = date1.format("yyyy-MM-dd") + " 23:59:59";
    }
    if (timetype == 4) {
        date.setDate(1);
        startdate = date.format("yyyy-MM-dd") + " 00:00:00";
        date.setMonth(date.getMonth() + 1);
        var date1 = new Date(date.getTime() - 1 * 24 * 60 * 60 * 1000);
        enddate = date1.format("yyyy-MM-dd") + " 23:59:59";
    }
    if (timetype == 5) {
        date.setDate(1);
        var date1 = new Date(date.getTime() - 1 * 24 * 60 * 60 * 1000);
        enddate = date1.format("yyyy-MM-dd") + " 23:59:59";
        date.setMonth(date.getMonth() - 1);
        startdate = date.format("yyyy-MM-dd") + " 00:00:00";
    }
    param["startdate"] = startdate;
    param["enddate"] = enddate;
    return param;
}

Date.prototype.format = function (fmt) {
    var o = {
        "M+": this.getMonth() + 1, //月份
        "d+": this.getDate(), //日
        "h+": this.getHours(), //小时
        "m+": this.getMinutes(), //分
        "s+": this.getSeconds(), //秒
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度
        "S": this.getMilliseconds()
        //毫秒
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "")
                .substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k])
                    : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
};

function getTotal(timetype, obj) {
    $("#timetype").val(timetype);
    $("#totalSalesAmount").html("查询中");
    $("#totalSalesGrossProfit").html("查询中");
    $("#totalSalesItems").html("查询中");
    $("#receivables").html("查询中");
    try {

        $("#tab2").removeClass("tabS");
        $("#tab3").removeClass("tabS");
        $("#tab4").removeClass("tabS");
        $("#tab5").removeClass("tabS");

        $("#tab2").addClass("tabN");
        $("#tab3").addClass("tabN");
        $("#tab4").addClass("tabN");
        $("#tab5").addClass("tabN");

        $("#tab" + timetype).addClass("tabS");
    } catch (e) {
    }

    var param = getTime(timetype);
    var startdate = param["startdate"];
    var enddate = param["enddate"];

    if (startdate == undefined) {
        var date = new Date();
        startdate = date.format("yyyy-MM-dd") + " 00:00:00";
        enddate = date.format("yyyy-MM-dd") + " 23:59:59";
    }
    var param = {};
    param["startdate"] = startdate;
    param["enddate"] = enddate;
    param["index"] = "1";
    if ($("#branchid_total").length > 0) {
        param["branchid"] = $("#branchid_total").val();
    }

    $.post("/UCenter-webapp/SaleReport/GetTotal.json", param,
                    function (result, resultState) {
                        var salesReportTotalSB = result.salesReportTotalSB;
                        //var maxSalesAmountProductName=salesReportTotalSB.productname;
                        //var maxSalesAmountClientName=salesReportTotalSB.clientname;
                        var totalSalesAmount = salesReportTotalSB != undefined ? salesReportTotalSB.salesmoney : 0;
                        var totalSalesGrossProfit = salesReportTotalSB != undefined ? salesReportTotalSB.salesgrossprofit : 0;
                        var totalGrossProfitMargin = salesReportTotalSB != undefined ? salesReportTotalSB.grossprofitmargin : 0;
                        var totalSalesItems = salesReportTotalSB != undefined ? salesReportTotalSB.salesitems : 0;

                        if (null == totalSalesAmount) {
                            totalSalesAmount = '0';
                        }
                        if (null == totalSalesGrossProfit) {
                            totalSalesGrossProfit = '0';
                        }
                        if (null == totalGrossProfitMargin) {
                            totalGrossProfitMargin = '0';
                        }
                        if (null == totalSalesItems) {
                            totalSalesItems = '0';
                        }

                        $("#totalSalesAmount").html(formatMoney(totalSalesAmount) + "元");
                        $("#totalSalesGrossProfit").html(
                                formatMoney(totalSalesGrossProfit) + "元");
                        $("#totalSalesItems").html(totalSalesItems + "笔");

                        var rate = 0.00;
                        try {
                            // 日销售额
                            if (timetype == 2 || timetype == 3) {
                                if ($("#hidDailySaleTarget").val() == "") {
                                    $("#spanSaleTarget").html("<font color='#999' class='font12'>未设置目标值</font>");
                                    return;
                                }
                                var dailySaleTarget = parseFloat($(
                                        "#hidDailySaleTarget").val());
                                if (dailySaleTarget == 0) {
                                    rate = 100;
                                } else {
                                    rate = parseFloat(totalSalesAmount)
                                            / dailySaleTarget * 100;
                                    rate = String(rate.toFixed(2));
                                }
                            }

                                // 月销售额
                            else if (timetype == 4 || timetype == 5) {
                                if ($("#hidMonthlySaleTarget").val() == "") {
                                    $("#spanSaleTarget").html("<font color='#999' class='font12'>未设置目标值</font>");
                                    return;
                                }
                                var monthlySaleTarget = parseFloat($(
                                        "#hidMonthlySaleTarget").val());
                                if (monthlySaleTarget == 0) {
                                    rate = 100;
                                } else {
                                    rate = parseFloat(totalSalesAmount) / monthlySaleTarget * 100;
                                    rate = String(rate.toFixed(2));
                                }
                            }

                        } catch (e) {

                        }

                        if (!rate || rate + "" == "NaN" || rate + "" == "Infinity") {
                            rate = 0.00;
                        }

                        $("#spanSaleTarget").html(rate + "%");

                    });
}

// 取得应收总额
function getClientReceive(timetype) {
    var param = getTime(timetype);
    param["branchid"] = null;
    param["index"] = "1";

    var startdate = param["startdate"];
    var enddate = param["enddate"];

    if (startdate == undefined) {
        var date = new Date();
        startdate = date.format("yyyy-MM-dd") + " 00:00:00";
        enddate = date.format("yyyy-MM-dd") + " 23:59:59";
    }
    param["startdate"] = startdate;
    param["enddate"] = enddate;
    if ($("#branchid_total").length > 0) {
        param["branchid"] = $("#branchid_total").val();
    }
    $.post("/UCenter-webapp/ClientReceive/Total.json", param, function (result,
            resultState) {
        if (resultState == "success") {
            if (result.serviceResult.statu == 1) {
                var t = result.serviceResult.t;
                var totalreceiveamt = t.totalreceiveamt.formatMoney();
                //当前欠款
                $("#receivables").html(formatMoney(totalreceiveamt) + "元");
            } else {
            }
        } else {
        }
    });
}

// 跳转到销售报表
function goSaleReport() {
    var title = "销售报表";
    var branchid = "";
    if ($("#branchid_warning").length > 0) {
        branchid = $("#branchid_warning").val();
    }
    var url = "SaleReport/Init.htm?branchid=" + branchid;
    var id = "SaleReport-Init";
    parent.addTabByTitleAndUrl(title, url, id, 'WBILL', true);
}
// 跳转到预警信息界面
function warningInfo1() {
    // 检查按钮权限
    if (!checkPerm("100305", "PERM_VIEW")) {
        return;
    } else {
        var title = "库存预警";
        var branchid = "";
        if ($("#branchid_warning").length > 0) {
            branchid = $("#branchid_warning").val();
        }
        var url = "Warning/Init1.htm?branchid=" + branchid;
        var id = "Warning-Init1";
        parent.addTabByTitleAndUrl(title, url, id, "WBILL", true);
    }
}
// 跳转到预警信息界面
function warningInfo3() {
    var title = "序列号预警";
    var branchid = "";
    if ($("#branchid_warning").length > 0) {
        branchid = $("#branchid_warning").val();
    }
    var url = "Warning/Init3.htm?branchid=" + branchid;
    var id = "Warning-Init3";
    parent.addTabByTitleAndUrl(title, url, id, "WBILL", true);
}
// 跳转到预警信息界面
function warningInfo2() {
    var title = "低价销售";
    var branchid = "";
    if ($("#branchid_warning").length > 0) {
        branchid = $("#branchid_warning").val();
    }
    var url = "Warning/Init2.htm?branchid=" + branchid;
    var id = "Warning-Init2";
    parent.addTabByTitleAndUrl(title, url, id, "WBILL", true);
}
// 	// 跳转到预警信息界面
// 	function warningInfo2() {
// 		var title = "低价销售";
// 		var url = "Warning/Init2.htm";
// 		var id = "Warning-Init2";
// 		parent.addTabByTitleAndUrl(title, url, id, "WBILL");
// 	}

//跳转“线上订单”
function toOnlineOrder() {
    if (!checkPerm("100100", "PERM_VIEW")) {
        return;
    }
    var title = "销售订单历史";
    var url = "SaleOrder/OnlineList.htm";
    var id = "SaleOrder-List";
    parent.addTabByTitleAndUrl(title, url, id, "WBILL", true);
}

//跳转“销售需送货”
function toSaledeliverList() {
    //是否开启出入库
    var isopenio = $("#isopenio").val();
    if ("1" == isopenio) {
        if (!checkPerm("100307", "PERM_VIEW")) {
            return;
        }
        var title = "出库历史";
        var url = "OutStorage/List.htm?saledeliver=1";
        var id = "OutStorage-List";
        parent.addTabByTitleAndUrl(title, url, id, "WBILL", true);
    } else {
        if (!checkPerm("100101", "PERM_VIEW")) {
            return;
        }
        var title = "销售历史";
        var url = "Sale/List.htm?saledeliver=1";
        var id = "SaleOrder-List";
        parent.addTabByTitleAndUrl(title, url, id, "WBILL", true);
    }
}

// 跳转到客户
function toClientRecive() {
    if (!checkPerm("110201", "PERM_VIEW")) {
        return;
    }

    var title = "客户应收欠款";
    var branchid = "";
    if ($("#branchid_warning").length > 0) {
        branchid = $("#branchid_warning").val();
    }
    //自动选择逾期未还客户
    var selectoverdue = 0;
    if ($("#div_clientreceivenumber").length > 0 && $("#div_clientreceivenumber").html().indexOf("逾期未还") > 0) {
        selectoverdue = 1;
    }
    var url = "ClientReceive/AmountsDueInit.htm?branchid=" + branchid + "&selectoverdue=" + selectoverdue;
    var id = "ClientReceive-AmountsDueInit";
    parent.addTabByTitleAndUrl(title, url, id, "WBI", true);
}

// 跳转到供应商
function toSuplierPay() {
    if (!checkPerm("110202", "PERM_VIEW")) {
        return;
    }

    var title = "供应商应付欠款";
    var branchid = "";
    if ($("#branchid_warning").length > 0) {
        branchid = $("#branchid_warning").val();
    }
    var url = "SupplierPay/AmountsDueToInit.htm?branchid=" + branchid;
    var id = "SupplierPay-AmountsDueToInit";
    parent.addTabByTitleAndUrl(title, url, id, "WBI", true);
}



// 跳转到入库
function toInStore() {
    // 检查按钮权限
    if (!checkPerm("100308", "PERM_VIEW")) {
        return;
    }
    var title = "入库历史";
    var branchid = "";
    if ($("#branchid_warning").length > 0) {
        branchid = $("#branchid_warning").val();
    }
    var url = "InStorage/List.htm?branchid=" + branchid;
    var id = "InStorage-List";
    parent.addTabByTitleAndUrl(title, url, id, 'WBILL', true);
}

// 跳转到出库
function toOutStore() {
    // 检查按钮权限
    if (!checkPerm("100307", "PERM_VIEW")) {
        return;
    }
    var title = "出库历史";
    var branchid = "";
    if ($("#branchid_warning").length > 0) {
        branchid = $("#branchid_warning").val();
    }
    var url = "OutStorage/List.htm?branchid=" + branchid;;
    var id = "OutStorage-List";
    parent.addTabByTitleAndUrl(title, url, id, 'WBILL', true);
}

// 跳转到系统公告
function goNewsList() {
    var title = "系统公告";
    var url = "news/init.htm";
    var id = "news-list";
    parent.addTabByTitleAndUrl(title, url, id, 'WMAIN');
}

// 打开系统公告
function previewNews(newsId) {
    var title = "公告详情";
    var url = "news/detail.htm?newsid=" + newsId;
    var id = "news-list-" + newsId;
    $('#newsshowarea').hide();
    parent.addTabByTitleAndUrl(title, url, id, 'WMAIN');
}

// 关闭系统公告
function closepreviewNews(newsId) {
    $.post("news/flagnews.json", { newsid: newsId }, function (result, resultState) {
        $('#newsshowarea').hide();
    });
}

function setSaleTarget() {
    parent.showMainPage("desktop/tosetsaletarget.htm", "设置目标值", 150, 350);
}

function setNote() {
    parent.showMainPage("desktop/tosetnote.htm", "设置老板寄语", 192, 355);
}

//加载首页预警信息
function initWarehouseIOCount() {
    $("#incount").addClass("loaddingNum").text("");
    $("#outcount").addClass("loaddingNum").text("");
    var param = {};

    $.post("desktop/getWarehouseIOCount.json", param, function (result, resultState) {
        var incount = 0;//待入库数量
        var outcount = 0;//待出库数量
        var newincount = 0;//新待入库数量
        var newoutcount = 0;//新待出库数量
        if (resultState == "success") {
            incount = result.incount;
            outcount = result.outcount;
            newincount = result.newincount;
            newoutcount = result.newoutcount;
        } else {
            showErrorMsg("加载预警信息失败");
        }
        var inhtml = "";
        if (newincount > 0 && incount > 0) {
            inhtml = '<span class="font666">新增<span class="fontRed " id="newincount">' +
            newincount + '</span>笔待入库单,</span><br/><span class="font666">累计<span class="fontRed " id="incount">' +
            incount + '</span>笔待入库单需要处理。</span>';
        } else if (incount > 0) {
            inhtml = '<span class="font666">您有<span class="fontRed " id="incount">' +
            incount + '</span>笔待入库单需要处理。</span>';
        } else {
            inhtml = '<span class="font666">当前没有待入库单需要处理</span>';
        }
        $("#div_incount").empty().append(inhtml);
        var outhtml = "";
        if (newoutcount > 0 && outcount > 0) {
            outhtml = '<span class="font666">新增<span class="fontRed " id="newoutcount">' +
            newoutcount + '</span>笔待出库单,</span><br/><span class="font666">累计<span class="fontRed " id="outcount">' +
            outcount + '</span>笔待出库单需要处理。</span>';
        } else if (outcount > 0) {
            outhtml = '<span class="font666">您有<span class="fontRed " id="outcount">' +
            outcount + '</span>笔待出库单需要处理。</span>';
        } else {
            outhtml = '<span class="font666">当前没有待出库单需要处理</span>';
        }
        $("#div_outcount").empty().append(outhtml);
        //$("#incount").text(incount).removeClass("loaddingNum");
        //$("#outcount").text(outcount).removeClass("loaddingNum");
    });
}
//加载首页预警信息
function initWarningInfo() {
    var productversion = $("#productversion").val();
    $("#errorserialnumber").addClass("loaddingNum").text("");
    $("#lowsalesnumber").addClass("loaddingNum").text("");
    $("#supplierpaynumber").addClass("loaddingNum").text("");
    $("#highinventorynumber").addClass("loaddingNum").text("");
    $("#lowerinventorynumber").addClass("loaddingNum").text("");
    var param = {};
    if ($("#branchid_warning").length > 0) {
        param["branchid"] = $("#branchid_warning").val();
    }
    var clientOffset = 0;
    var supplierOffset = 0;
    var sameOffset = 0;
    $.YY_post("desktop/getSamePayAndReceivesOffsetNum.json", null,
            function (data) {
                var result = data;
                if (result != null && null != result.clientOffset) {
                    clientOffset = parseInt(result.clientOffset);
                    supplierOffset = parseInt(result.supplierOffset);
                    sameOffset = parseInt(result.sameOffset);
                }
            }, null,
            function () {
                showErrorMsg('获取客户关联供应商 抵消 应收 应付失败');
            }, null, false);

    if ('3' != productversion) {
        param["state"] = 1;
        $.post("desktop/getWarningInfo.json", param, function (result, resultState) {
            var errorserialnumber = 0;//errorserialnumber 异常序列号
            if (resultState == "success") {
                if (result != null && result.serviceResult != null && result.serviceResult.statu == 1) {
                    var obj = result.serviceResult.t;
                    if (obj != null && obj != undefined) {
                        errorserialnumber = obj.errorserialnumber;
                    }
                } else {
                    showMsg(result.serviceResult.message);
                }
            } else {
                showErrorMsg("加载预警信息失败");
            }
            $("#errorserialnumber").text(errorserialnumber).removeClass("loaddingNum");
            if (errorserialnumber > 0) {
                $("#warn_serial").show();
            } else {
                $("#warn_serial").hide();
            }
            $("#warn_serial").show();
        });
    }
    param["state"] = 2;
    $.post("desktop/getWarningInfo.json", param, function (result, resultState) {
        var lowsalesnumber = 0;//商品低价销售
        if (resultState == "success") {
            if (result != null && result.serviceResult != null && result.serviceResult.statu == 1) {
                var obj = result.serviceResult.t;
                if (obj != null && obj != undefined) {
                    lowsalesnumber = obj.lowsalesnumber;
                }
            } else {
                showMsg(result.serviceResult.message);
            }
        } else {
            showErrorMsg("加载预警信息失败");
        }
        var inhtml = "";
        if (lowsalesnumber > 0) {
            inhtml = '<span class="font666">最近7天低价销售了<span class="fontRed " id="lowsalesnumber">' +
            lowsalesnumber + '</span>个商品。</span>';
        } else {
            inhtml = '<span class="font666">最近7天销售正常，无低价销售 </span> ';
        }
        $("#div_lowsalesnumber").empty().append(inhtml);
        //$("#lowsalesnumber").text(lowsalesnumber).removeClass("loaddingNum");
    });
    if ('3' != productversion) {
        //未开启账期才之心，若已开启账期，则获取的是客户逾期未还数
        if (!isOpenAccountPeriod()) {
            $("#clientreceivenumber").addClass("loaddingNum").text("");
            param["state"] = 3;
            $.post("desktop/getWarningInfo.json", param, function (result, resultState) {
                var clientreceivenumber = 0;//客户应收欠款
                if (resultState == "success") {
                    if (result != null && result.serviceResult != null && result.serviceResult.statu == 1) {
                        var obj = result.serviceResult.t;
                        if (obj != null && obj != undefined) {
                            clientreceivenumber = obj.clientreceivenumber;
                        }
                    } else {
                        showMsg(result.serviceResult.message);
                    }
                } else {
                    showErrorMsg("加载预警信息失败");
                }
                var inhtml = "";
                if (clientreceivenumber > 0) {
                    inhtml = '<span class="font666">您有<span class="fontRed " id="clientreceivenumber">' +
                    clientreceivenumber + '</span>笔客户应收欠款。</span>';
                } else {
                    inhtml = '<span class="font666">当前没有任何客户欠款</span> ';//最近7天销售正常，未发生低价销售 
                }
                $("#div_clientreceivenumber").empty().append(inhtml);
                //$("#clientreceivenumber").text(clientreceivenumber).removeClass("loaddingNum");

            });
        } else {
            initClientReceive();
        }

        param["state"] = 4;
        param["deskClientwarningPage"] = 1;//首页预警
        $.post("desktop/getWarningInfo.json", param, function (result, resultState) {
            var supplierpaynumber = 0;//供应商应付欠款111
            if (resultState == "success") {
                if (result != null && result.serviceResult != null && result.serviceResult.statu == 1) {
                    var obj = result.serviceResult.t;
                    if (obj != null && obj != undefined) {
                        supplierpaynumber = obj.supplierpaynumber;
                    }
                } else {
                    showMsg(result.serviceResult.message);
                }
            } else {
                showErrorMsg("加载预警信息失败");
            }
            var inhtml = "";
            if (supplierpaynumber > 0) {
                inhtml = '<span class="font666">您有<span class="fontRed " id="clientreceivenumber">' +
                supplierpaynumber + '</span>笔供应商应付欠款。</span>';
            } else {
                inhtml = '<span class="font666">当前没有任何供应商欠款 </span>';//最近7天销售正常，未发生低价销售 
            }
            $("#div_supplierpaynumber").empty().append(inhtml);
            //$("#supplierpaynumber").text(supplierpaynumber).removeClass("loaddingNum");
        });
    }
    param["state"] = 5;
    $.post("desktop/getWarningInfo.json", param, function (result, resultState) {
        var highinventorynumber = 0;//商品超过预警线
        if (resultState == "success") {
            if (result != null && result.serviceResult != null && result.serviceResult.statu == 1) {
                var obj = result.serviceResult.t;
                if (obj != null && obj != undefined) {
                    highinventorynumber = obj.highinventorynumber;
                }
            } else {
                showMsg(result.serviceResult.message);
            }
        } else {
            showErrorMsg("加载预警信息失败");
        }
        param["state"] = 6;
        $.post("desktop/getWarningInfo.json", param, function (result, resultState) {
            var lowerinventorynumber = 0;//商品库存低于预警
            if (resultState == "success") {
                if (result != null && result.serviceResult != null && result.serviceResult.statu == 1) {
                    var obj = result.serviceResult.t;
                    if (obj != null && obj != undefined) {
                        lowerinventorynumber = obj.lowerinventorynumber;
                    }
                } else {
                    showMsg(result.serviceResult.message);
                }
            } else {
                showErrorMsg("加载预警信息失败");
            }
            var inhtml = "";
            if (highinventorynumber == 0 && lowerinventorynumber == 0) {
                inhtml = '<span class="font666">商品库存均处于正常范围内</span>';
            } else if (highinventorynumber > 0 && lowerinventorynumber == 0) {
                inhtml = '您有<span class="fontRed " id="highinventorynumber">' + highinventorynumber
                + '</span>个商品超过预警线。';
            } else if (lowerinventorynumber == 0 && lowerinventorynumber > 0) {
                inhtml = '<span class="font666">您有<span class="fontRed " id="lowerinventorynumber">' +
                lowerinventorynumber + '</span>个商品库存低于预警线,';
            } else {
                inhtml = '<span class="font666">您有<span class="fontRed " id="lowerinventorynumber">' +
                lowerinventorynumber + '</span>个商品库存低于预警线,' +
                    '<br>您有<span class="fontRed " id="highinventorynumber">' + highinventorynumber
                    + '</span>个商品超过预警线。';
            }
            $("#div_inventory").empty().append(inhtml);
            //$("#lowerinventorynumber").text(lowerinventorynumber).removeClass("loaddingNum");
        });
        //$("#highinventorynumber").text(highinventorynumber).removeClass("loaddingNum");
    });

    //无最低售价权限的员工，看不到低价销售预警
    var lowerprice = $("#lowerprice", parent.document).val();
    if ('0' == lowerprice) {
        $("#warn_lowsale").remove();
    }

    /*线上订单*/
    param["state"] = 8;
    $.post("desktop/getWarningInfo.json", param, function (result, resultState) {
        var onlineordercount = 0;
        if (resultState == "success") {
            if (result != null && result.serviceResult != null && result.serviceResult.statu == 1) {
                var obj = result.serviceResult.t;
                if (obj != null && obj != undefined) {
                    onlineordercount = obj.onlineordernumber;
                }
            } else {
                showMsg(result.serviceResult.message);
            }
        } else {
            showErrorMsg("加载预警信息失败");
        }
        var inhtml = "";
        if (onlineordercount == 0) {
            inhtml = '<span class="font666">当前没有线上展柜订单</span>';
        } else {
            inhtml = '您有<span class="fontRed " id="onlineordercount">' + onlineordercount
            + '</span>笔线上展柜订单。';
        }
        $("#div_onlineordercount").empty().append(inhtml);
    });

    /*销售需送货*/
    param["state"] = 9;
    $.post("desktop/getWarningInfo.json", param, function (result, resultState) {
        var saledelivernumber = 0;
        if (resultState == "success") {
            if (result != null && result.serviceResult != null && result.serviceResult.statu == 1) {
                var obj = result.serviceResult.t;
                if (obj != null && obj != undefined) {
                    saledelivernumber = obj.saledelivernumber;
                }
            } else {
                showMsg(result.serviceResult.message);
            }
        } else {
            showErrorMsg("加载预警信息失败");
        }
        var inhtml = "";
        if (saledelivernumber == 0) {
            inhtml = '<span class="font666">当前没有销售单需要送货。</span>';
        } else {
            inhtml = '您有<span class="fontRed" id="saledelivernumber">' + saledelivernumber + '</span>笔销售单需要送货。';
        }
        $("#div_saledelivernumber").empty().append(inhtml);
    });
}

//加载客户欠款 预警
function initClientReceive() {
    var param = {};
    if ($("#branchid_warning").length > 0) {
        param["branchid"] = $("#branchid_warning").val();
    }
    if ('3' != productversion) {
        param["state"] = 3;
        param["isopenaccountperiod"] = 1;
        param["deskClientwarningPage"] = 1;//首页预警
        $.post("desktop/getWarningInfo.json", param, function (result, resultState) {
            var clientreceivenumber = 0; // 逾期欠款客户数
            if (resultState == "success") {
                if (result != null && result.serviceResult != null && result.serviceResult.statu == 1) {
                    var obj = result.serviceResult.t;
                    if (obj != null && obj != undefined) {
                        clientreceivenumber = obj.clientreceivenumber;
                    }
                } else {
                    showErrorMsg(result.serviceResult.message);
                }
            } else {
                showErrorMsg("加载预警信息失败");
            }
            var inhtml = "";
            if (clientreceivenumber > 0) {
                inhtml = '<span class="font666">您有<span class="fontRed " id="clientreceivenumber">' + clientreceivenumber + '</span>位客户欠款逾期未还。</span>';
                $('#allWarningInfoNum .num').show();
            } else {
                inhtml = '<span class="font666">所有客户欠款状态正常.</span> ';
            }
            $("#div_clientreceivenumber").empty().append(inhtml);
            // $("#clientreceivenumber").text(clientreceivenumber).removeClass("loaddingNum");

        });
    }

}

// 加载首页店铺动态
function initTopTenBill() {
    $(".dynamicContent").html("").addClass("DCloadding");
    var param = {};
    if ($("#branchid_condition").length > 0) {
        param["branchid"] = $("#branchid_condition").val();
    }
    $.post("desktop/getTopTenBill.json", param, function (result, resultState) {
        if (resultState == "success") {
            if (result.serviceResult.statu == 1) {
                var rows = result.serviceResult.rows;
                var len = rows == null ? 0 : rows.length;
                var dynContent = "";
                if (len > 0) {
                    $("#dynamicArea").removeClass("noData");
                    for (var n = 0; n < len; n++) {
                        var bill = rows[n];
                        var busiDate = bill.CreateDate + "";
                        busiDate = busiDate.substring(0, 16);
                        var BusiId = bill.BusiId;
                        var billContent = "";
                        var connectid = "";
                        if (bill.BusiType == "收款") {
                            connectid = bill.ClientId;
                        } else if (bill.BusiType == "付款") {
                            connectid = bill.SupplierId;
                        }
                        {
                            // 单据名称
                            billContent += "			<span class=\"font14\">新增"
                                    + bill.BusiType + "</span><br> ";
                            // 单号
                            if (bill.BusiNo != null) {
                                billContent += "			<spanclass=\"font999\"><a onclick=\"openBusiness('" + bill.BusiType + "','" + BusiId + "','" + connectid + "')\">"
                                        + bill.BusiNo + "</a></span><br>";
                            }
                            // 关联单号
                            if (bill.ConectBusiNo != null) {
                                billContent += "			<spanclass=\"font999\">关联："
                                        + bill.ConectBusiNo + "</span><br>";
                            }
                            // 往来单位--客户
                            if (bill.ClientName != null) {
                                billContent += "			<spanclass=\"font999\">"
                                        + bill.ClientName + "</span><br>";
                            }
                            // 往来单位--供应商
                            if (bill.SupplierName != null) {
                                billContent += "			<spanclass=\"font999\">"
                                        + bill.SupplierName + "</span><br>";
                            }
                            // 金额
                            if (bill.BusiAmt != null) {
                                billContent += "			<spanclass=\"font999 countNum\">"
                                        + (
                                                bill.BusiAmt == null ? "0" : bill.BusiAmt
                                                ) + "元</span><br>";
                            }
                            // 转出账户
                            if (bill.OutAccountName != null) {
                                billContent += "			<spanclass=\"font999\">转出："
                                        + bill.OutAccountName
                                        + "</span><br>";
                            }
                            // 转入账户
                            if (bill.InAccountName != null) {
                                billContent += "			<spanclass=\"font999\">转入："
                                        + bill.InAccountName + "</span><br>";
                            }
                            // 转出日期
                            if (bill.OutDate != null) {
                                billContent += "			<spanclass=\"font999\">"
                                        + bill.OutDate + "</span><br>";
                            }

                            // 经手人
                            if (bill.UserName != null) {
                                billContent += "			<spanclass=\"font999\">"
                                        + bill.UserName + "</span><br>";
                            }
                        }

                        var boxClass = "evenLine";
                        if (n % 2 == 0) {
                            boxClass = "oddLine";
                            dynContent += "<div class=\"fl\">";

                            dynContent += "<div class=\"" + boxClass + "\">";
                            dynContent += "	<div class=\"box\">";
                            dynContent += "		<div class=\"content\" >";
                            dynContent += billContent;
                            dynContent += "		</div>";
                            dynContent += "		<div class=\"angle\"></div>";
                            dynContent += "		<div class=\"time fontDeepBlue\">"
                                    + busiDate + "</div>";
                            dynContent += "	</div>";
                            dynContent += "</div>";
                        } else {
                            //dynContent += "<div class=\"fl\">";
                            dynContent += "<div class=\"" + boxClass + "\">";
                            dynContent += "	<div class=\"box\">";
                            dynContent += "		<div class=\"time fontDeepBlue\">"
                                    + busiDate + "</div>";
                            dynContent += "		<div class=\"angle\"></div>";
                            dynContent += "		<div class=\"content\" >";
                            dynContent += billContent;
                            dynContent += "		</div>";
                            dynContent += "	</div>";
                            dynContent += "</div>";
                        }

                        if (n % 2 == 1 || n == len) {
                            dynContent += "</div>";
                        }
                    }
                } else {
                    $("#dynamicArea").addClass("noData");
                    $(".dynamicContent").removeClass("DCloadding");
                }

                $(".dynamicContent").html(dynContent);
                $(".dynamicContent").removeClass("DCloadding");
            } else {
                showMsg(result.serviceResult.message);
            }
        } else {
            showErrorMsg("加载店铺动态失败");
        }
    });
}

function openBusiness(businesstype, busiid, connectid) {
    if (businesstype == '进货') {
        var title = "进货单详情";
        var url = "/Buy/Detail.htm?buyid=" + busiid;
        var id = "Buy-Detail";
        parent.addTabByTitleAndUrl(title, url, id, 'WBILL');
    }
    if (businesstype == '进货退货') {
        var title = "进货退货单详情";
        var url = "BuyReturn/Detail.htm?returnid=" + busiid;
        var id = "BuyReturn-Detail";
        parent.addTabByTitleAndUrl(title, url, id, "WBILL");
    }
    if (businesstype == '销售退货') {
        var title = "销售退货单详情";
        var url = "SaleReturn/Detail.htm?returnid=" + busiid;
        var id = "SaleReturn-Detail";
        parent.addTabByTitleAndUrl(title, url, id, 'WBILL');
    }
    if (businesstype == '销售') {
        var title = "销售单详情";
        var url = "Sale/Detail.htm?saleid=" + busiid;
        var id = "Sale-Detail";
        parent.addTabByTitleAndUrl(title, url, id, 'WBILL');
    }
    if (businesstype == '出库') {
        var title = "出库单详情";
        var url = "OutStorage/Detail.htm?ioid=" + busiid;
        var id = "OutStorage-Detail";
        parent.addTabByTitleAndUrl(title, url, id, "WBILL");
    }
    //收回
    if (businesstype == '收款') {
        var title = "收款单详细";
        var url = "ClientReceive/ReceivablesSingleDetailInit.htm?businessid=" + busiid + "&clientid=" + connectid;
        var id = "ClientReceive-ReceivablesSingleDetailInit";
        parent.addTabByTitleAndUrl(title, url, id, "WBI");
    }
    //支付
    if (businesstype == '付款') {
        var title = "付款单详细";
        var url = "SupplierPay/PaymentSingleDetailInit.htm?businessid=" + busiid + "&supplierid=" + connectid;
        var id = "SupplierPay-PaymentSingleDetailInit";
        parent.addTabByTitleAndUrl(title, url, id, "WBI");
    }

    if (businesstype == '入库') {
        var title = "入库单详细";
        var url = "InStorage/Detail.htm?ioid=" + busiid;
        var id = "InStorage-Detail";
        parent.addTabByTitleAndUrl(title, url, id, "WBILL");
    }
    if (businesstype == '转账') {

    }
    if (businesstype == '收入') {
        var title = "日常收支详细";
        var url = "IncomeAndPay/detail.htm?id=" + busiid;
        var tabid = "IncomeAndPay-detail";
        parent.addTabByTitleAndUrl(title, url, tabid, "WBI", true);
    }
    if (businesstype == '支出') {
        var title = "日常收支详细";
        var url = "IncomeAndPay/detail.htm?id=" + busiid;
        var tabid = "IncomeAndPay-detail";
        parent.addTabByTitleAndUrl(title, url, tabid, "WBI", true);
    }
    if (businesstype == '借入单') {
        var title = "借入单详细";
        var url = "Borrow/Detail.htm?lendid=" + busiid;
        var id = "Borrow-Detail";
        parent.addTabByTitleAndUrl(title, url, id, "WBILL");
    }
    if (businesstype == '借出单') {
        var title = "借出单详细";
        var url = "Lend/Detail.htm?lendid=" + busiid;
        var id = "Lend-Detail";
        parent.addTabByTitleAndUrl(title, url, id, "WBILL");
    }
    if (businesstype == '库存调拨') {
        var title = "调拨单详细";
        var url = "Transfer/detail.htm?tranid=" + busiid;
        var id = "TransferDetail";
        parent.addTabByTitleAndUrl(title, url, id, "WBILL");
    }
    if (businesstype == '组装拆卸') {
        var title = "组装拆卸单详细";
        var url = "Assembly/detail.htm?assemblyid=" + busiid;
        var id = "assemblyDetail";
        parent.addTabByTitleAndUrl(title, url, id, "WBILL");
    }
    if (businesstype == '盘点') {
        var title = "库存盘点单详情";
        var url = "InvTak/Detail.htm?takbillid=" + busiid;
        var id = "InvTak-Detail";
        parent.addTabByTitleAndUrl(title, url, id, "WBILL");
    }
}

function showNewFunTips() {
    if ($("#hidishownewfuntips").val() == "") {
        parent.layer.open({
            title: false,
            type: 2,
            area: ['600px', '460px'],
            fix: false, //不固定
            maxmin: false,
            closeBtn: false,
            content: "/UCenter-webapp/desktop/delaccountinitfun.htm",
            success: function (layero, index) {
            }
        });
    }
}
//加载签到日历
function initSignDiv() {
    var param = {};
    $.post("index/initCalendar.json", param, function (result, resultState) {
        if (resultState == "success") {
            if (result != null) {
                var calendarhtml = result.calendarhtml;
                if (calendarhtml != null && calendarhtml != undefined) {
                    //清空div内容，解决连续刷新导致内容重复的问题
                    $("#signData").html("");
                    $("#signData").append(calendarhtml);
                }
                if (result.todaySinTrue == 1) {
                    $("#isSignFlag").hide();
                }
                if (result.todaySinTrue != 1) {
                    $("#isSignFlag").show();
                }
            } else {
                showErrorMsg("加载签到日历失败");
            }
        } else {
            showErrorMsg("加载签到日历失败");
        }
    });
}

function sign() {
    var overdue = $("#overdue", parent.document).val();
    var isuserpay = $("#isuserpay", parent.document).val();
    if (overdue != undefined && overdue == "1") {
        if (isuserpay == 'false') {
            showErrorMsg("您的试用期已结束，积分功能暂时无法使用，请续费后继续该操作。");
        }
        else {
            showErrorMsg("您的账号已到期，积分功能暂时无法使用，请续费后继续该操作。");
        }
        return;
    }
    var param = {};
    $.post("index/sign.json", param, function (result, resultState) {
        if (resultState == "success") {
            if (result != null) {
                var obj = result.objectServiceResult.t;
                if (obj != null && obj != undefined) {
                    if (obj.signflag == 1) {
                        showSuccessMsg("签到成功");
                        $("#todayli").html("<div class=\"on\"></div>" + $("#todayli").html());
                        $("#signbtn").attr("onclick", "isSignTip()").html("已签到");
                        $("#kltitle").html("明日可领");
                        $("#klpoints").html(result.cangetponints + "<span>分</span>");
                        var totalpoints = parseInt($("#hidtotalpoints").val());
                        var signpoints = parseInt($("#hidsignpoints").val());
                        $("#totalpoints").html(totalpoints + signpoints + "<span>分</span>");
                        $("#signdays").html((obj.signdays) + "<span>天</span>");
                        $("#isSignFlag").hide();
                    }
                } else {
                    showErrorMsg("签到失败了，请重新再试试！");
                }
            } else {
                showErrorMsg("签到失败了，请重新再试试！");
            }
        } else {
            showErrorMsg("签到失败了，请重新再试试！");
        }
    });
}
function isSignTip() {
    showSuccessMsg("今日已签到");
}