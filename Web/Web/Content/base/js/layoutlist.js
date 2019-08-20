var layoutlist = {
    init: function (options) {
        var _this = this;
        _this.options = {
            title: "",
            path: "",
            url: "_List",
            controller: "",
            viewId: 0,
            viewtype: 0,
            viewtitle: "",
            eid: 0,
            ename: "",
            area: ['85%', '96%'],
            target: { type: parent, area: ['90%', '96%'] },
            searchcontrols: [{ field: "KeyWord" }, { field: "issearch" }],
            ischeckbox: false,
            columns: [],
            reduceheight: 0,
            click: function (data) {
                if (data == null) return;
                var id = data[_this.options.ename + "$ID"];
                $("#btn-toolbar-edit").show();
                //  _this.toobal_display_rule(id);
               // $("#btn-toolbar-external-link").attr("href", _this.options.path + "Form/" + id);
            },
            dbclick: function (data) {
                if (data == null) return;
                _this.options.target.type.dlg.openframe({
                    offset: ['10px'],
                    title: _this.options.title + '-' + data[_this.options.ename + "$Name"],
                    content: _this.options.path + "Detail/" + data[_this.options.ename + "$ID"],
                    area: _this.options.area
                })
            }
        };
        $.extend(_this.options, options || {});
        _this._render_page();
        _this._render_list();
        _this.bind();
        _this.load();
    },
    load: function () {
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

    },
    _render_page: function () {
        if (top.location.href == location.href) {
            $("#btn-toolbar-external-link").hide();
            $("#btn-toolbar-refresh").hide();
            $("#btn-toolbar-binoculars").css("position", "initial")
        };
        $("#btn-toolbar-external-link").attr("href", location.href)
    },
    _render_list: function () {
        var _this = this;
        var _grid = {
            primary: _this.options.ename + "$ID",
            columns: _this.options.columns,
            ischeckbox: _this.options.ischeckbox,
            filterable: {
                extra: true, operators: {
                    string: {
                        contains: "包含", eq: "等于", neq: "不等于", startswith: "开头为", doesnotcontain: "不包含", endswith: "结尾为"
                    }
					, date: {
					    eq: "等于", neq: "不等于", gte: "大于等于", gt: "大于", lte: "小于等于", lt: "小于"
					}
					, datetime: {
					    eq: "等于", neq: "不等于", gte: "大于等于", gt: "大于", lt: "小于", lte: "小于或等于"
					}
					, decimal: {
					    eq: "等于", neq: "不等于", gte: "大于等于", gt: "大于", lt: "小于", lte: "小于或等于"
					}
					, number: {
					    eq: "等于", neq: "不等于", gte: "大于等于", gt: "大于", lte: "小于等于", lt: "小于"
					}
					, "enum": {
					    eq: "等于", neq: "不等于"
					}
                }
            },
            searchcontrols: [{ field: "KeyWord" }, { field: "issearch" }, { field: "Filter" }],
            paramsdata: { vid: _this.options.viewId, eid: _this.options.eid, pid: _this.options.pid, peid: _this.options.peid },
            sortable: true,
            resizable: true,
            url: _this.options.url,
            click: function (data) {
                _this.options.click(data);
            },
            dbclick: function (data) {
                _this.options.dbclick(data);
            }
        }
        if (_this.options.pagesize) {
            _grid.pageSize = _this.options.pagesize;
        }
        _this.grid = new pagegride(_grid);
        $('<span title="列表筛选" class="cc " href="#" tabindex="-1"><i class="icon-filter"></i></span>').insertAfter($(".k-grid-header"));
        $(".k-grid-filter").hide();
        $(".cc").click(function () {
            if ($(this).hasClass("cccurr")) {
                $(this).removeClass("cccurr");
                $(".k-grid-filter").hide();
                $(".k-button").click();
                $(".k-grid-header th.k-with-icon .k-link").css("margin-right", "1.3em !important");
            }
            else {
                $(this).addClass("cccurr");
                $(".k-grid-filter").show();
                $(".k-grid-header th.k-with-icon .k-link").css("margin-right", "0px !important");
            }
        })
        setTimeout(function () { _this.autogridsize(); }, 500);
    },
    bind: function () {
        var _this = this;
        $("#KeyWord,.SearchField").keydown(function (event) {
            event = document.all ? window.event : event;
            if ((event.keyCode || event.which) == 13) {
                _this.search()
            }
        });
        $("#btnSearch,#btn_FieldSearch").click(function () {
            _this.search()
        });
        $("#btn_ClearSearchFiled").click(function () {
            $.each($("#search-panel").find(".SearchField"), function () {
                $(this).val("");
            });
            $("#search-panel").find(".search-span").each(function () {
                $(this).text($(this).data("title"));
            });
            $("#search-panel").find(".search-icon").each(function () { $(this).data("value", ""); });
            $("#search-panel").find(".search-span").each(function () { $(this).data("value", ""); });
            $("#search-panel").find("select").selectpicker('deselectAll');
            _this.search();
        });
        $("#btn-toolbar-refreshdata").click(function () {
            _this.refresh()
        });
        $("#btn-toolbar-add").click(function () {
            parent.dlg.openframe({
                title: "新增" + _this.options.title, content: _this.options.path + "Form",
                area: _this.options.area,
            });
        });
        $("#btn-toolbar-edit").click(function () {
            var _rows = _this.grid.getcheckedrows();
            if (_rows.length == 0) {
                dlg.alert("请选择要编辑的数据");
                return false
            };
            if (_rows.length > 1) {
                dlg.alert("请选择唯一一条数据");
                return false
            };
            parent.dlg.openframe({
                area: _this.options.area,
                title: _this.options.title + '-' + _rows[0][_this.options.ename + "$Name"], content: _this.options.path + "Form/" + _rows[0][_this.options.ename + "$ID"]
            })
        });
        $("#btn-toolbar-delete").click(function () {
            var ids = _this.grid.getcheckedid();
            if (ids.length == 0) {
                dlg.alert("请选择要删除的数据");
                return
            };
            dlg.confirm("确定删除选中的 <b>" + ids.length + "</b> 条数据吗？删除后不能恢复", function () {
                $.post(_this.options.path + "_Delete", {
                    ids: JSON.stringify(ids)
                }
				, function (data) {
				    if (data.Success) {
				        dlg.msg.info("删除成功");
				        _this.refresh();
				    }
				    else {
				        if (data.Message && data.Message != "") {
				            dlg.alert(data.Message);
				        } else {
				            dlg.alert("删除失败");
				        }
				    }
				}
				, "json")
            })
        });
        $("#btn-toolbar-disable").click(function () {
            var statecode = 1;
            var title = $(this).text().trim();
            if (title == "删除") {
                statecode = 1
            }
            else {
                statecode = 0
            };
            var ids = _this.grid.getcheckedid();
            if (ids.length == 0) {
                dlg.alert("请选择要" + title + "的数据");
                return
            };
            dlg.confirm("确定" + title + "选中的 <b>" + ids.length + "</b> 条数据吗？", function () {
                var _i = dlg.load(4);
                $.post(_this.options.path + "_Disable", {
                    ids: JSON.stringify(ids), statecode: statecode
                }
				, function (data) {
				    dlg.close(_i);
				    if (data.Success) {
				        dlg.msg.info("操作成功");
				        _this.refresh()
				    } else if (data.Message != "") {
				        dlg.alert(data.Message)
				    }
				    else {
				        dlg.alert("操作失败")
				    }
				}
				, "json")
            })
        });
        $("#btn-toolbar-import").click(function () {
            parent.dlg.openframe({
                title: '数据导入', content: "/Common/ImportData?eid=" + _this.options.eid
            })
        });
        $("#btn-toolbar-export").click(function () {
            var loadindex = parent.layer.load(1);
            var index = parent.dlg.msg.info("导出中...");
            $('#ifile').attr("src", "/Common/ExportList?v=" + _this.options.viewId + "&eid=" + _this.options.eid);
            setTimeout(function () { parent.layer.close(loadindex); parent.layer.close(index) }, 3000);
        });
        $("#btn-toolbar-refresh").click(function () {
            history.go(0)
        });
        $("#btn-toolbar-binoculars").click(function () {
            if (_this.options.viewtype != "自定义视图") {
                _this.toview(0, "高级查找-" + _this.options.title)
            }
            else {
                _this.toview(_this.options.viewId, _this.options.viewtype + "【" + _this.optionsviewtitle + "】")
            }
        });
        $("#btn-toolbar-share").click(function () {
            var ids = _this.grid.getcheckedid();
            if (ids.length == 0) {
                dlg.alert("请选择数据");
                return
            };
            var url = "/Common/Share?v=" + _this.options.viewId;
            if (ids.length == 1) {
                url = "/Common/Share?v=" + _this.options.viewId + "&id=" + ids[0]
            };
            $.post("/Common/_SaveID", {
                ids: ids.join(",")
            }
			, function (data) {
			    if (data) {
			        dlg.openframe({
			            title: "数据共享", offset: ['10px'], type: 2, area: ['430px', '460px'], fixed: true, maxmin: false, content: url, yes: function (index) {
			                parent.layer.close(index)
			            }
						,
			        })
			    }
			}
			, "json")
        });
        $("#btn-toolbar-assign").click(function () {
            var ids = _this.grid.getcheckedid();
            if (ids.length == 0) {
                dlg.alert("请选择数据");
                return
            };
            var url = "/Common/Assign?v=" + _this.options.viewId;
            $.post("/Common/_SaveID", {
                ids: ids.join(",")
            }
			, function (data) {
			    if (data) {
			        dlg.openframe({
			            title: "数据分派", offset: ['10px'], type: 2, area: ['430px', '460px'], fixed: true, maxmin: false, content: url, yes: function (index) {
			                parent.layer.close(index)
			            }
						,
			        })
			    }
			}
			, "json")
        });
        document.onkeydown = function (event) {
            var e = event || window.event || arguments.callee.caller.arguments[0];
            if (e && e.keyCode == 27) {
                parent.dlg.closeAll();
            }
        };
        $(window).resize(function () { _this.autogridsize(); });
    },
    toobal_display_rule: function (id) {
        var _this = this;
        $.post(_this.options.path + "_IsUnDisable", {
            id: id
        }
		, function (data) {

		    var $i = $("#btn-toolbar-disable").find("i");
		    var $span = $("#btn-toolbar-disable").find("span");
		    //   $("#btn-toolbar-disable").show();
		    //if (data.Item) {
		    //    $i.attr("class", "fa fa-trash-o");
		    //    $span.html("删除");
		    //    $("#btn-toolbar-delete").hide();
		    //}
		    //else {
		    //    $i.attr("class", "fa icon-ok");
		    //    $span.html("还原")
		    //   $("#btn-toolbar-delete").show();
		    //}
		}
		, "json")
    },
    refresh: function () {
        var _this = this;
        _this.grid.refresh();
        setTimeout(function () {
            $('.tooltip').tooltipster({
                side: "left",
                maxWidth: 600,
            });
        }, 500);
    },
    search: function () {
        var _this = this;
        $("#issearch").val(true);
        var _filter = [];
        $.each($(".SearchField"), function () {
            var _o = {
                field: $(this).data("field"),
                fieldlist: $(this).data("fieldlist"),
                type: $(this).data("type"),
                opera: $(this).data("opera"),
                sort: $(this).data("sort"),
                sql: $(this).data("sql"),
            };
            var value = $(this).val();
            if (typeof value == "string") {
                value = value.trim()
            }
            if (value instanceof Array) {
                value = value.join(",");
            }
            if (_o.type == "sql") {
                value = _o.sql;
            }
            if (value != "" && value != null) {
                _o.value = value;
                _filter.push(_o);
            }
        });
        $("#Filter").val(JSON.stringify(_filter));
        _this.refresh();
        $("#issearch").val(false)
    },
    toview: function (_viewId, title) {
        var _this = this;
        if (_viewId > 0) {
            parent.dlg.openframe({
                title: title, content: "/View?eid=" + _this.options.eid + "&v=" + _viewId
            })
        }
        else {
            parent.dlg.openframe({
                title: title, content: "/View?eid=" + _this.options.eid, full: function (layero) {
                }
				, restore: function (layero) {
				}
				,
            })
        }
    },
    open_field_dialog: function (e) {
        var value = $(e).data("value");
        var fieldid = $(e).data("fieldid");
        var id = $(e).data("id");
        dlg.openframe({
            title: "选择",
            offset: ['10px'],
            type: 2,
            //btn: ['确定', '取消'],
            //shade: 0,
            area: ['780px', '460px'],
            fixed: true, //不固定
            maxmin: false,
            content: "/View/RelationFieldSelectPanelMulti?height=280&t=0&fieldid=" + fieldid + "&value=" + ((typeof value) != "undefined" ? value : "") + "&id=" + id,
            yes: function (index) {
                parent.layer.close(index);
            },
        });
    },
    autogridsize: function () {
        var _this = this;
        $("#grid").height($(window).height() - (parseInt(_this.options.reduceheight || 0) + parseInt($("#search-panel").height() || 0)));
        _this.search();
    }
};

var fun = {
    add: function () {
        var title = "新增-" + layoutlist.options.title;
        var url = layoutlist.options.path + "Form";
        var tabid = "add-" + request("tabid");
        //parent.dlg.openframe({
        //    area: layoutlist.options.area,
        //    title: title, content: url
        //});
        if (layoutlist.options.isdialog != "False" && layoutlist.options.isdialog != false) {
            parent.dlg.openframe({
                area: layoutlist.options.area,
                title: title, content: url
            });
        } else {
            parent.tab.open(title, url, tabid, 'WBI');
        }
    },
    edit: function (id, name) {
        if (name !== '') {
            name = "-" + name;
        }
        var title = "编辑-" + layoutlist.options.title + name;
        var url = layoutlist.options.path + "Form?id=" + id;
        var tabid = "edit-" + request("tabid") + "-" + id;
        //parent.dlg.openframe({
        //    area: layoutlist.options.area,
        //    title: title, content: url
        //});
        if (layoutlist.options.isdialog != "False" && layoutlist.options.isdialog !== false) {
            parent.dlg.openframe({
                area: layoutlist.options.area,
                title: title, content: url
            });
        } else {
            parent.tab.open(title, url, tabid, 'WBI');
        }
    },
    copy: function (id, name) {
        if (name !== '') {
            name = "-" + name;
        }
        var title = "复制-" + layoutlist.options.title + name;
        var url = layoutlist.options.path + "Form?id=" + id + "&action=copy";
        var tabid = "copy-" + request("tabid");
        if (layoutlist.options.isdialog != "False" && layoutlist.options.isdialog != false) {
            parent.dlg.openframe({
                area: layoutlist.options.area,
                title: title, content: url
            });
        } else {
            parent.tab.open(title, url, tabid, 'WBI');
        }
    },
    detail: function (id, name, vid, row) {
        if (name !== '') {
            name = "-" + name;
        }
        var title = "查看-" + layoutlist.options.title + name;
        var url = layoutlist.options.path + "Detail?id=" + id + "&vid=" + vid + "&row=" + row;
        var tabid = "detail-" + request("tabid");
        if (layoutlist.options.isdialog != "False" && layoutlist.options.isdialog != false) {
            parent.dlg.openframe({
                area: layoutlist.options.area,
                title: title, content: url
            })
        } else {
            parent.tab.open(title, url, tabid, 'WBI');
        }
    },
    dele: function (id, name) {
        dlg.confirm("确定删除 " + layoutlist.options.title + " - " + name + " 吗？删除后不能恢复", function () {
            $.post(layoutlist.options.path + "_Delete", {
                ids: JSON.stringify([id])
            }
                , function (data) {
                    if (data.Success) {
                        dlg.msg.info("删除成功");
                        layoutlist.refresh();
                    }
                    else {
                        if (data.Message && data.Message != "") {
                            dlg.alert(data.Message);
                        } else {
                            dlg.alert("删除失败");
                        }
                    }
                }, "json");
        });
        return false;
    },
    edit_tr_input: function (e) {
        var field = $(e).data("field");
        var id = $(e).data("id");
        var value = $(e).val();
        $.post("/Common/_UpdateValue", { id: id, value: value, field: field }, function (data) {
            if (data.Success) {
                layer.msg("操作成功");
            } else {
                if (data.Message !== "") {
                    layer.msg(data.Message);
                }
            }
        }, "json");
    }
};

function OpenValue(value) {
    var values = value.toString().split("$");
    var link = "";
    if (values.length == 3) {
        layoutlist.options.target.type.dlg.openframe({
            title: values[2], content: "/" + values[0] + "/Form/" + values[1]
        })
    };
    if (values.length == 4) {
        layoutlist.options.target.type.dlg.openframe({
            title: values[3],
            content: "/" + values[0] + "/" + values[1] + "/Form/" + values[2],
            area: layoutlist.options.area
        })
    }
};
setInterval(function () {
    if (layoutlist.grid) {
        var ids = layoutlist.grid.getcheckedid();
        if (ids.length > 0) {
            $(".Ischeck").show();
        }
        else {
            $(".Ischeck").hide();
        }
    }

}
, 100);

setInterval(function () {
    if (layoutlist.options) {
        var isrefresh = $.cookie("list_isrefresh-" + layoutlist.options.controller);
        if (isrefresh == "true") {
            $.cookie("list_isrefresh-" + layoutlist.options.controller, null, {
                path: '/'
            });
            layoutlist.refresh();
        }
        //var url = location.href.replace(location.origin, '');
        //refreshtask.run(url, function () {
        //    layoutlist.refresh();
        //})
    }

    var menuid = request("menuid");
    refreshtask.run(menuid, function () {
        layoutlist.refresh();
    });

    var v = request("v");
    refreshtask.run(v, function () {
        layoutlist.refresh();
    });
}, 200);


function totips(value) {
    if (value.toString().length > 20) {
        return '<span class="tooltip" title="' + value + '">' + (value.toString().substring(0, 20) + "...") + '</span>';
    } else { return value; }
}
