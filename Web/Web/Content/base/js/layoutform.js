var loadindex = 0;
var layoutform = {
    process: 0,
    path: "",
    area: "",
    controller: "",
    layer: { index: 0, loadindex: 0 },
    ID: 0,
    form: $("#form-submit"),
    isautoclose: false,
    closeandadd: false,
    isedit: false,
    formdata: "",
    returnurl: "",
    dialog: { height: 474 },
    submitvalidstatus: 0,
    //表单提交前置方法集合
    fn_beforeSendArr: [],
    fn_afterSubmitArr: [],
    fn_beforeSend: function () { },
    params: {
        ID: parseInt(($("#ID").val() || 0)),
        pid: parseInt(typeof request("pid") == "string" ? request("pid") : "0"),
        pname: typeof request("pname") == "string" ? decodeURIComponent(request("pname")) : "",
        peid: parseInt(typeof request("peid") == "string" ? request("peid") : "0"),
    },
    closepage: function () {
        var _this = this;
        if (_this.formdata != _this.form.serialize() && _this.formdata != "") {
            dlg.confirm("您有未保存的内容，确定关闭吗？", function () {
                if (top.location.href == location.href) {
                    window.opener = null;
                    window.open('', '_self');
                    window.close();
                } else {
                    parent.layer.close(_this.layer.index);
                }
            })
        } else {
            if (top.location.href == location.href) {
                window.opener = null;
                window.open('', '_self');
                window.close();
            } else {
                parent.layer.close(_this.layer.index);
            }
        }
    },
    setlistcookie: function () {
        var _this = this;
        $.cookie("list_isrefresh-" + _this.controller, "true", { path: '/' });
        $.cookie("tablist_isrefresh-" + _this.controller, "true", { path: '/' });
        $.cookie("viewlist_isrefresh-" + _this.controller, "true", { path: '/' });
    },
    _ajaxformparams: function () {
        var _this = this;
        return {
            iframe: true,
            beforeSend: function () {
                var result = _this.fn_beforeSend();
                $.each(_this.fn_beforeSendArr, function (i, fun) {
                    if (typeof fun == "function") {
                        if (fun() == false) { result = false; }
                    }
                });
                if (result == false) {
                    _this.process = 0; return false;
                }
                _this.form.validate();


                $.each($(".CustomizeController"), function () {
                    var id = $(this).attr("id").replace("Cus-", "");
                    $("#" + id).val($(this).val());
                });
                //多选下拉框赋值
                $.each($(".selectpickermultiple"), function () {
                    var value = $(this).val();
                    var _v = "";
                    if (value instanceof Array) {
                        _v = value.join(",");
                    }
                    $("input[name=" + $(this).attr("id") + "]").val(_v);
                });
                loadindex = layer.load(2);

                dlg.msg.info("正在提交数据…");
            },
            success: function (data) {
                var url = location.href;
                dlg.close(_this.layer.loadindex);
                //表单提交完成以后触发的方法
                $.each(_this.fn_afterSubmitArr, function (i, fun) {
                    if (typeof fun == "function") {
                        fun(data);
                    }
                });
                if (data.Success) {
                    _this.formdata = "";
                    //关闭加载层
                    //   layer.closeAll('loading');
                    _this.setlistcookie();

                    //编辑
                    if (_this.isedit != "False") {
                        setTimeout(function () { _this.process = 0; }, 1000)
                    }
                    //弹框
                    if (top.location.href != location.href) {
                        //弹框关闭
                        dlg.msg.info("保存成功", { time: 1000 }, function () {
                            if (_this.returnurl != "") {
                                location.href = _this.returnurl;
                            };
                            if (_this.isautoclose) {
                                parent.layer.close(_this.layer.index);
                            }
                            else if (_this.closeandadd) {
                                parent.layer.close(_this.layer.index);
                                parent.dlg.openframe({ title: "新增", content: _this.path + "Form" });
                            } else {
                                if (_this.isedit == "False") {
                                    var id = request("id");
                                    if (id != null) {
                                        url = url.replace("id=0", "id=" + data.Item);
                                        parent.layer.iframeSrc(_this.layer.index, url);
                                    } else {
                                        parent.layer.iframeSrc(_this.layer.index, _this.path + "Form/" + data.Item);
                                    }
                                }
                            }
                        });
                    } else {
                        //窗体关闭
                        dlg.msg.info("保存成功", { time: 1000 }, function () {
                            if (_this.returnurl != "") {
                                location.href = _this.returnurl;
                            };
                            //保存并关闭
                            if (_this.isautoclose) {
                                window.opener = null;
                                window.open('', '_self');
                                window.close();
                            }
                            //保存
                            else {
                                //新增
                                if (_this.isedit == "False") {
                                    var id = request("id");
                                    if (id != null) {
                                        location.href = url.replace("id=0", "id=" + data.Item);
                                    } else {
                                        location.href = location.href + "/" + data.Item;
                                    }
                                }
                                //编辑
                                else {
                                    //复制新增
                                    if (request("action") == "copy") {
                                        url = url.replace("&action=copy", "");
                                        location.href = url.replace(/(id=[0-9]+)/g, 'id=' + data.Item)
                                    }
                                    //  history.go(0);
                                }
                            }
                        });
                    }
                } else {
                    //自定义错误
                    if (data.Code == 1) {
                        dlg.msg.info(data.Message);
                    } else {
                        dlg.msg.info("保存失败，" + data.Message);
                    }

                    //console.log(data.Message);
                    _this.process = 0;
                }
            },
            error: function (arg1, arg2, ex) {
                //信息框-错误提示
                parent.dlg.msg.info(ex);
            },
            dataType: 'json'
        };
    },
    options: {},
    _submit: function () {
        var _this = this;
        _this.submitvalidstatus = 0;
        _this.valid = _this.form.validate({
            errorPlacement: function (error, element) {
                // console.log(JSON.stringify(error));
            },
            showErrors: function (errorMap, errorList) {
                if (_this.submitvalidstatus == 0) {
                    _this.submitvalidstatus = 1;
                    var message = "";
                    var element = "";
                    for (var m in errorMap) {
                        if (message == "") {
                            message = errorMap[m];
                            element = m;
                        }
                    }
                    $("#" + element).focus();
                    dlg.msg.info(message);

                    this.defaultShowErrors();
                }
            }
        });

        if (_this.valid.form()) {
            _this.process++;
            if (_this.process > 1) {
                parent.dlg.msg.info("请不要重复提交");
            } else {

                _this.form.submit();
            }
        }
    },
    _disable: function (e) {
        var _this = this;
        var statecode = 1;
        var title = $(e).text().trim();
        if (title == "删除") { statecode = 1; } else { statecode = 0; }
        var ids = [_this.params.ID];
        dlg.confirm("确定" + title + "吗？", function () {
            _this.layer.loadindex = dlg.load(4);
            $.post(_this.path + "_Disable", { ids: JSON.stringify(ids), statecode: statecode }, function (data) {
                dlg.close(_this.layer.loadindex);
                if (data.Success) {
                    dlg.msg.info("操作成功");
                    _this.setlistcookie();
                    layer.load(1);
                    history.go(0);
                } else {
                    dlg.alert("操作失败");
                }
            }, "json");
        })
    },
    bind: function () {
        var _this = this;
        var option = _this._ajaxformparams();
        $.extend(option, _this.options || {});
        _this.form.ajaxForm(option);
        $("#btn-form-save").on("click", function () { _this.isautoclose = false; _this._submit(); });
        $("#btn-form-save-and-close").on("click", function () { _this.isautoclose = true; _this._submit(); });
        $("#btn-form-save-and-add").on("click", function () { _this.closeandadd = true; _this._submit(); });
        $("#btn-form-refresh").on("click", function () { history.go(0); });
        $("#btn-form-close").on("click", function () { _this.closepage(); });
        $("#btn-form-detailedit-save").on("click", function () { _this.isautoclose = false; _this._submit(); });
        $("#btn-form-detailedit-cancel").on("click", function () { location.href = _this.returnurl; });
        $("#btn-form-detail").on("click", function () {
            layer.load(1);
            var url = location.href;
            location.href = url.replace("Form", "Detail");
        });
        setInterval(function () { _this.resize(); }, 100);
        $(window).resize(function () { _this.resize(); });
    },
    init: function () {
        var _this = this;
        if (parent.layer && parent.layer.getFrameIndex) {
            _this.layer.index = parent.layer.getFrameIndex(window.name); //获取窗口索引
        }
        setTimeout(function () { _this.formdata = _this.form.serialize(); }, 1000);
        _this.bind();
        $("textarea").parent("div").css({ "margin-bottom": "2px" });

        $.each($("input[id*=Layout_]"), function () {
            var id = $(this)[0].id.replace("Layout_", "");
            $("#" + id).val($(this).val());
        })
    },
    resize: function () {
        var _width = $(".filedtitle").width();
        var _width = _width + 24;
        $(".md12").find(".filedtitle").css({ width: _width, float: "left" });//.width(156)
        $(".md12").find(".filedvalue").width($(window).width() - _width - 25)
    },
    //多选下拉框控件赋值
    load_selectpickermultiple: function () {
        $.each($(".selectpickermultiple-hidden"), function () {
            var value = $(this).val();
            $("#" + $(this).attr("name")).val(value.toString().split(","));
            $('.selectpicker').selectpicker('refresh');//刷新插件
        });

        var selectallinterval = setInterval(function () {
            if ($(".bs-select-all").length > 0) {
                clearInterval(selectallinterval);
            }
            $(".bs-select-all").text("全选").css({ "width": "100%" });
            $(".bs-deselect-all").hide();
            $(".bs-select-all").on("click", function () {
                if ($(this).text() == "全选") {
                    $(this).text("取消全选");
                } else {
                    $(this).next("button").click();
                    $(this).text("全选");
                    return false;
                }
            });
        }, 500);
    }
};

//打开外键关联窗体
function OpenValue(value) {
    var values = value.toString().split("$");
    if (values[2] != 0) {
        var link = "";
        if (values.length == 3) {
            dlg.openframe({ title: values[2], content: "/" + values[0] + "/Form/" + values[1], area: ['98%', '96%'] });
        } if (values.length == 4) {
            dlg.openframe({ title: values[3], content: "/" + values[0] + "/" + values[1] + "/Form/" + values[2], area: ['98%', '96%'] });
        }
    }
};


//弹出关联字段值选择框（多选）
function open_field_dialogmulti(e) {
    var value = $(e).data("value");
    var fieldid = $(e).data("fieldid");
    var id = $(e).data("id");
    var filter = (typeof $(e).data("filter") == "string") ? $(e).data("filter") : "";//"new_card_cardid$ID__4";
    dlg.openframe({
        title: "选择",
        offset: ['10px'],
        type: 2,
        //btn: ['确定', '取消'],
        //shade: 0,
        area: ['780px', '460px'],
        fixed: true, //不固定
        maxmin: false,
        content: "/LookUp/RelationFieldSelectPanelMulti?height=280&page=1&fieldid=" + fieldid + "&value=" + ((typeof value) != "undefined" ? value : "") + "&id=" + id + "&filter=" + filter,
    });
};
//弹出关联字段值选择框（单选）
function open_field_dialog(e) {
    var value = $(e).data("value");
    var fieldid = $(e).data("fieldid");
    var id = $(e).data("id");
    var filter = (typeof $(e).data("filter") == "string") ? $(e).data("filter") : "";//"new_card_cardid$ID__4";
    dlg.openframe({
        title: "选择",
        offset: ['10px'],
        type: 2,
        //btn: ['确定', '取消'],
        //shade: 0,
        area: ['780px', '492px'],
        fixed: true, //不固定
        maxmin: false,
        content: "/LookUp/RelationFieldSelectPanel?t=1&fieldid=" + fieldid + "&value=" + ((typeof value) != "undefined" ? value : "") + "&id=" + id + "&filter=" + filter,
    });
}
;
document.onkeydown = function (event) {
    var e = event || window.event || arguments.callee.caller.arguments[0];
    if (e && e.keyCode == 27) { // 按 Esc 
        layoutform.closepage();
    }
};

function IsShowHead() {
    if (parseFloat($("#Form_Head").height()) > 50) {
        $("#Form_Head").find("ul:gt(0)").hide();
    } else {
        $("#Form_Head").find("ul:gt(0)").show();

    }
};

$(function () {
    IsShowHead();
    layoutform.load_selectpickermultiple();
});

$(window).resize(function () {
    // IsShowHead();
});


layui.use('element', function () {
    var $ = layui.jquery
        , element = layui.element();
    var active = {
        tabAdd: function () {

            element.tabAdd('demo', {
                title: '新选项' + (Math.random() * 1000 | 0)
                , content: '内容' + (Math.random() * 1000 | 0)
            })
        }
        , tabDelete: function () { element.tabDelete('demo', 2); }
        , tabChange: function () { element.tabChange('demo', 1); }
    };

    $('.site-demo-active').on('click', function () {
        var type = $(this).data('type');
        active[type] ? active[type].call(this) : '';
    });

    $(".layui-tab>ul>li").on('click', function () {
        $(".layui-tab>div").hide();
        $(".layui-tab>div:eq(" + $(this).index() + ")").show();
    });
    $(".layui-tab>div").hide();
    $(".layui-tab>div:eq(0)").show();
});