var editcolumns = {
    init: function (_option) {
        var _this = this;
        _this.option = { UserID: "" };
        $.extend(_this.option, _option || {});
        this.fieldArr = [];
        this.columns_container = "#columns_container"
        this.fields = JSON.parse($("#fields").val()) || [];
        this.entityid = request("eid");
        this.fielddata = [];
        this.EntityName = $("#EntityName").val();
        this.bind();
        _this.initcolumns();
    },
    bind: function () {
        var _this = this;
        $("#columns_container").delegate("th", "click", function () {
            $(this).addClass("curr").siblings("th").removeClass("curr");
        })

        $("#columns_container").delegate("th", "dblclick", function () {
            _this.editcolumnattr(this);
        })


        $("li").delegate("#addcolumn", "click", function () { _this.addcolumn(); })
        $("li").delegate("#removecolumn", "click", function () { _this.removecolumn(); })
        $("li").delegate("#toleft", "click", function () { _this.movecolumn(0); })
        $("li").delegate("#toright", "click", function () { _this.movecolumn(1); })
        $("li").delegate("#editcolumnattr", "click", function () { _this.editcolumnattr(); })
        $("li").delegate("#editcolumnsort", "click", function () { _this.editcolumnsort(); })
    },
    initcolumns: function () {
        var _this = this;
        var tw = 0;
        $(_this.columns_container).find("thead").find("tr>th").remove();
        _this.fielddata = [];
        $.each(_this.fields, function (m, n) {
            if (n.field != null) {
                _this.fielddata.push(n);
                _this.fieldArr.push(n.field);
                var w = n.width <= 0 ? 80 : parseFloat(n.width);
                tw += w;
                var $th = $("<th></th>");
                $th.html(n.title);
                $th.width(w);
                for (var s in n) {
                    $th.data(s, n[s])
                }
                if (n.attr)
                    $th.attr(n.attr);
                //  $.extend($th.data(), n || {});
                //$th.data("field", n.field);
                //$th.data("fieldsql", n.fieldsql);
                //$th.data("title", n.title);
                //$th.data("fieldtype", n.fieldtype);
                //$th.data("jointablename", n.jointablename);
                //$th.data("onsql", n.onsql);
                $(_this.columns_container).find("thead").find("tr").append($th);
            }
        })
        $(_this.columns_container).width(tw);
        $("#fielddata").val(JSON.stringify(_this.fielddata));
    },
    //添加列
    addcolumn: function () {
        var _this = this;
        dlg.openframe({
            title: "添加列",
            offset: ['10px'],
            type: 2,
            btn: ['确定', '取消'],
            //shade: 0,
            area: ['533px', '353px'],
            fixed: false, //不固定
            maxmin: false,
            move: false,
            content: "/View/AddColumns?eid=" + _this.entityid,
            yes: function (index) {
                var sel_entity = layer.getChildFrame("#eid", index);
                var eid = sel_entity.val();
                var ename = sel_entity.find("option:checked").data("name");
                var etitle = sel_entity.find("option:checked").data("title");
                var sel_onsql = sel_entity.find("option:checked").data("onsql");
                var sel_jointablename = sel_entity.find("option:checked").data("jointablename");

                var selectTr = $(layer.getChildFrame("#grid", index)).find("tr[aria-selected=true]");
                $.each(selectTr, function (m, n) {
                    var width = 80;
                    var id = $(n).find("input").val();
                    var title = $(n).find("td:eq(1)").text();
                    if (etitle) {
                        title = title + "(" + etitle + ")"
                        width = 150;
                    }
                    var _field = $(n).find("td:eq(2)").text();
                    var fieldtype = $(n).find("td:eq(3)").text();
                    var fieldsql = $(n).find("td:eq(4)").text();
                    var jointablename = $(n).find("td:eq(5)").text();
                    var onsql = $(n).find("td:eq(6)").text();
                    var openlink = $(n).find("td:eq(7)").text();
                    var templatesql = $(n).find("td:eq(8)").text();
                    var template = $(n).find("td:eq(9)").text();
                    var format = $(n).find("td:eq(10)").text();
                    var type = $(n).find("td:eq(11)").text();
                    //var fieldsql = "";
                    var field = _field;
                    field = ename + "$" + _field;
                    //
                    filterable = "";
                    if (fieldtype == "选项集" || fieldtype == "关联其他表") {
                        field = jointablename.split(' ')[1].trim() + "$Name";
                    } else if (fieldtype == "时间") {
                        filterable = { ui: "datetimepicker" }
                    }

                    if (ename != _this.EntityName) {
                        fieldsql = ename + "." + _field + " AS " + field;
                        jointablename = sel_entity.find("option:checked").data("jointablename");
                        onsql = sel_entity.find("option:checked").data("onsql");
                    }


                    if (_this.fieldArr.indexOf(field) == -1) {
                        //_this.fieldArr.push(field);
                        _this.fields.push({
                            "name": field,
                            "fieldtype": fieldtype,
                            "title": title,
                            "field": field,
                            "fieldsql": fieldsql,
                            "width": width,
                            "onsql": onsql,
                            "jointablename": jointablename,
                            "filterable": filterable,
                            "openlink": openlink,
                            "templatesql": templatesql,
                            "entityname": ename,
                            "template": template == "" ? null : template,
                            "format": format,
                            "type": type
                            //"relationEntity": jointablename
                        });
                    }
                })
                _this.initcolumns();
                layer.close(index);
            },
        })
    },
    //移除列
    removecolumn: function () {
        var _this = this;
        var $this = $(_this.columns_container).find("thead").find("tr>th[class=curr]");
        if ($this[0]) {
            dlg.confirm("您确定要移除此列吗？", function () {
                var $this = $(_this.columns_container).find("thead").find("tr>th[class=curr]");
                var $this_data = $this.data();
                _this.fieldArr.remove($this_data.field);
                _this.fielddata.removeobj("field", $this_data.field);
                _this.fields.removeobj("field", $this_data.field);
                $this.remove();
                $("#fielddata").val(JSON.stringify(_this.fielddata));
            })
        } else {
            dlg.alert("请选择要移除的列")
        }

    },
    //移动列
    movecolumn: function (type) {
        var _this = this;
        var $this = $(_this.columns_container).find("thead").find("tr>th[class=curr]");
        if ($this[0]) {
            //右
            if (type == 1) {
                var $next = $this.next();
                $this.insertAfter($next);
            } else {
                var $prev = $this.prev();
                $this.insertBefore($prev);
                //左
            }
            var $ths = $(_this.columns_container).find("thead").find("tr>th");
            var _fielddata = [];
            $.each($ths, function (m, n) {
                var _field = $(n).data("field");
                $.each(_this.fields, function (i, j) {
                    j.attr = null;
                    if ($this.data("field") == j.field) {
                        j.attr = { class: "curr" };
                    }

                    if (_field == j.field) {
                        j.sort = m;
                        _fielddata.push(j)
                    }
                })
            })
            _this.fields = _fielddata;
            _this.initcolumns();
        }
    },
    _setfieldwidth: function (e) {
        $("#fieldwidth").hide();
        if ($(e).val() == -1) {
            $("#fieldwidth").show();
        } else {
            $("#fieldwidth").val($(e).val())
        }
    },
    //编辑列属性
    editcolumnattr: function (e) {
        var _this = this;
        var $this = $(_this.columns_container).find("thead").find("tr>th[class=curr]");
        if (e) {
            $this = $(e);
        }
        if ($this[0]) {
            var _field = $this.data("field");
            var _width = $this.data("width");
            var _ishidden = $this.data("hidden") || false;
            var _title = $this.text();
            var $dialog = $('<div><fieldset style="overflow-y:auto;margin:15px;">\
                            <div class="form-group">\
                                <label class="control-label" for="" title="">列标题：</label>\
                                <div class="control-div">\
                                    <input type="text" style="font-size:13px; padding: 10px 8px;" class="form-control" id="fieldtitle" value="' + _title + '">\
                                </div>\
                            </div>\
                            <div class="form-group">\
                                <label class="control-label" for="" title="">列宽：</label>\
                                <div class="control-div">\
                                     <select style="font-size:13px;" id="fieldwidth_select" onchange="editcolumns._setfieldwidth(this)"  class="form-control">\
                                        <option value="56" ' + (_width == 56 ? "selected" : "") + ' >ID</option>\
                                        <option value="70" ' + (_width == 70 ? "selected" : "") + ' >70</option>\
                                        <option value="80" ' + (_width == 80 ? "selected" : "") + ' >80</option>\
                                        <option value="90" ' + (_width == 90 ? "selected" : "") + ' >90</option>\
                                        <option value="95" ' + (_width == 95 ? "selected" : "") + ' >95</option>\
                                        <option value="100"' + (_width == 100 ? "selected" : "") + '>100</option>\
                                        <option value="110"' + (_width == 110 ? "selected" : "") + '>110</option>\
                                        <option value="120"' + (_width == 120 ? "selected" : "") + '>120</option>\
                                        <option value="150"' + (_width == 150 ? "selected" : "") + '>150</option>\
                                        <option value="200"' + (_width == 200 ? "selected" : "") + '>200</option>\
                                        <option value="250"' + (_width == 250 ? "selected" : "") + '>250</option>\
                                        <option value="350"' + (_width == 350 ? "selected" : "") + '>350</option>\
                                        <option value="500"' + (_width == 500 ? "selected" : "") + '>500</option>\
                                        <option value="-1"' + ((_width != 50 && _width != 80 && _width != 100 && _width != 120 && _width != 150 && _width != 200 && _width != 250 && _width != 350 && _width != 500) ? "selected" : "") + '>自定义</option>\
                                        <option value="0"' + (_width == 0 ? "selected" : "") + '>自动</option>\
                                    </select>\
                                    <input type="text" id="fieldwidth" style="margin-top:5px;display:none; font-size:13px; padding: 10px 8px;" class="form-control" id="fieldtitle" value="' + _width + '">\
                                </div>\
                            </div>\
                        </fieldset></div>');
            if (_this.option.UserID == "999") {
                $dialog.find("fieldset").append('<div class="form-group">\
                                                    <label class="control-label" for="" title="">其他属性：</label>\
                                                    <div class="control-div">\
                                                        <label class="checkbox-inline" style="display: inline-block">\
                                                            <input id="ishidden" ' + ((_ishidden) ? "checked" : "") + ' type="checkbox" >不显示\
                                                        </label>\
                                                    </div>\
                                                </div>');
            }
            dlg.openContent({
                id: "dic",
                offset: ["10px"],
                title: "编辑属性",
                content: $dialog.html(),
                area: ["373px", "350px"]
            }, function () {
                $.each(_this.fields, function (i, j) {
                    if (_field == j.field) {
                        j.title = $("#fieldtitle").val();
                        j.width = parseInt($("#fieldwidth").val());
                        $this.width(j.width);
                        if (_this.option.UserID == "999") {
                            j.hidden = $("#ishidden").is(":checked");
                        }
                    }
                });
                debugger
                _this.initcolumns();
                var index = layer.index; //获取当前弹层的索引号
                layer.close(index); //关闭当前弹层
            });

            if ($("#fieldwidth_select").val() == -1) {
                $("#fieldwidth").show();
            } else {
                $("#fieldwidth").val($(this).val())
            }

        } else {
            dlg.alert("请选择要编辑的列")
        }
    },
    //配置排序顺序
    editcolumnsort: function (e) {
        var _this = this;
        var _viewsort = $("#viewsort").val();
        var _onefieldsort, _onefieldsorttype, _twofieldsort, _twofieldsorttype = "";
        if (_viewsort != null && _viewsort != "" && _viewsort != "") {
            _onefieldsort = _viewsort.split(",")[0].split(" ")[0].replace(".", "$");
            _onefieldsorttype = _viewsort.split(",")[0].split(" ")[1].replace(".", "$");
            _twofieldsort = _viewsort.split(",")[1].split(" ")[0].replace(".", "$");
            _twofieldsorttype = _viewsort.split(",")[1].split(" ")[1].replace(".", "$");
        }
        var $dialog = $('<div><fieldset style="overflow-y:auto;margin:15px">\
                            <div class="form-group">\
                                <label class="control-label" for="" title="" style="font-size:12px;">排序依据：</label>\
                                <div class="control-div">\
                                   <select style="width: 80%;float:left;font-size:12px;" id="onefieldsort" class="form-control">\
                                    </select>\
                                    <select id="onefieldsorttype" style="width:20%;;font-size:12px;" class="form-control">\
                                        <option value="asc" ' + (_onefieldsorttype == "asc" ? "selected='selected'" : "") + '>升序</option>\
                                        <option value="desc" ' + (_onefieldsorttype == "desc" ? "selected='selected'" : "") + '>降序</option>\
                                    </select>\
                                </div>\
                            </div>\
                            <div class="form-group">\
                                <label class="control-label" for="" title="" style="font-size:12px;">第二依据：</label>\
                                <div class="control-div">\
                                   <select style="width: 80%;float:left;font-size:12px;" id="twofieldsort" class="form-control">\
                                    </select>\
                                    <select id="twofieldsorttype" style="width:20%;font-size:12px;" class="form-control">\
                                        <option value="asc" ' + (_twofieldsorttype == "asc" ? "selected='selected'" : "") + '>升序</option>\
                                        <option value="desc" ' + (_twofieldsorttype == "desc" ? "selected='selected'" : "") + '>降序</option>\
                                    </select>\
                                </div>\
                            </div>\
                        </fieldset></div>')
        $onefieldsort = $dialog.find("#onefieldsort");
        $onefieldsorttype = $dialog.find("#onefieldsorttype");
        $twofieldsort = $dialog.find("#twofieldsort");
        $twofieldsorttype = $dialog.find("#twofieldsorttype");

        $.each(_this.fields, function (m, n) {
            $onefieldsort.append("<option value='" + n.field + "' " + (_onefieldsort == n.field ? "selected='selected'" : "") + " >" + n.title + "</option>")
            $twofieldsort.append("<option value='" + n.field + "' " + (_twofieldsort == n.field ? "selected='selected'" : "") + " >" + n.title + "</option>")
        });
        $("#onefieldsorttype").val(_onefieldsorttype);
        $("#twofieldsorttype").val(_twofieldsorttype);
        dlg.openContent({
            id: "dic",
            title: "配置默认排序",
            content: $dialog.html(),
            area: ["373px"]
        }, function () {
            var index = layer.index; //获取当前弹层的索引号
            $("#viewsort").val($("#onefieldsort").val() + " " + $("#onefieldsorttype").val() + "," + $("#twofieldsort").val() + " " + $("#twofieldsorttype").val());
            layer.close(index); //关闭当前弹层
        });
    }
};
//(function ($, window, document) {
//    var obj = function () {
//        this.fieldArr = [];
//        this.columns_container = "#columns_container"
//        this.fields = JSON.parse($("#fields").val()) || [];
//        this.entityid = request("eid");
//        this.fielddata = [];
//        this.EntityName = $("#EntityName").val();
//        this.init();
//        this.bind();
//    }
//    obj.prototype = {

//    }
//    $.editcolumns = obj;
//})($, window, document)
